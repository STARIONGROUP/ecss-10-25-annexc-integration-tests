#!/usr/bin/env bash
set -uo pipefail

PROJECTS=(
  "IntegrationTestSuite/MessagePackIntegrationTests/MessagePackIntegrationTests.csproj"
  "IntegrationTestSuite/WebservicesIntegrationTests/WebservicesIntegrationTests.csproj"
)

# --- Seed config ---
DATA_DIR="Data"
WEBSERVICE_URL="${WEBSERVICE_URL:-http://localhost:5000}"

# --- Setup logs directory (timestamped, plus a 'latest' symlink) ---
TS=$(date +%Y%m%d-%H%M%S)
LOG_DIR="logs/${TS}"
mkdir -p "${LOG_DIR}"
ln -sfn "${TS}" "logs/latest"

# --- Color helpers (no-op if not a TTY) ---
if [[ -t 1 ]]; then
  GREEN=$'\e[32m'; RED=$'\e[31m'; YELLOW=$'\e[33m'; BOLD=$'\e[1m'; RESET=$'\e[0m'
else
  GREEN=""; RED=""; YELLOW=""; BOLD=""; RESET=""
fi

banner() {
  echo ""
  echo "${BOLD}${YELLOW}================================================================${RESET}"
  echo "${BOLD}${YELLOW}$1${RESET}"
  echo "${BOLD}${YELLOW}================================================================${RESET}"
}

# --- Seed: zip the Data dir contents and POST to /Data/Exchange ---
seed_database() {
  banner "Seeding database from ${DATA_DIR}/ -> ${WEBSERVICE_URL}/Data/Exchange"

  if [[ ! -d "${DATA_DIR}" ]]; then
    echo "${RED}ERROR: Data directory not found at ${DATA_DIR}${RESET}" >&2
    exit 1
  fi

  local zip_path="${LOG_DIR}/Data.zip"
  local response_body="${LOG_DIR}/seed-response.txt"

  # Zip the *contents* of Data/ so JSON files end up at the archive root
  # (Annex C.3 expects Header.json etc. at the top level, not under Data/)
  echo "Zipping ${DATA_DIR}/ -> ${zip_path}"
  ( cd "${DATA_DIR}" && zip -rq "$(realpath "${OLDPWD}/${zip_path}")" . )
  ls -lh "${zip_path}"

  echo ""
  echo "Posting seed file..."
  local http_code
  http_code=$(curl -sS -o "${response_body}" -w "%{http_code}" \
    --form file=@"${zip_path}" \
    "${WEBSERVICE_URL}/Data/Exchange")

  echo "HTTP ${http_code}"
  if [[ "${http_code}" =~ ^2 ]]; then
    echo "${GREEN}Seed OK${RESET}"
  else
    echo "${RED}Seed FAILED. Response body:${RESET}" >&2
    cat "${response_body}" >&2
    echo "" >&2
    echo "${RED}Check that:${RESET}" >&2
    echo "  - the webservice is running at ${WEBSERVICE_URL}" >&2
    echo "  - Backtier__IsDbSeedEnabled=true is set on the webservice" >&2
    exit 1
  fi
}

seed_database

# --- Build once up front so per-project runs use --no-build ---
banner "Restoring & building integration test solution (Release)"
dotnet restore IntegrationTestSuite/WebservicesIntegrationTests.sln
dotnet build IntegrationTestSuite/WebservicesIntegrationTests.sln -c Release --no-restore

# --- Run each project in order, capture exit codes ---
declare -a RESULTS=()
declare -a EXIT_CODES=()

for PROJECT in "${PROJECTS[@]}"; do
  NAME=$(basename "${PROJECT}" .csproj)
  PROJECT_LOG_DIR="${LOG_DIR}/${NAME}"
  mkdir -p "${PROJECT_LOG_DIR}"
  CONSOLE_LOG="${PROJECT_LOG_DIR}/console.log"

  banner "Running: ${NAME}"

  dotnet test "${PROJECT}" \
    -c Release \
    --no-build \
    --logger "console;verbosity=minimal" \
    --logger "trx;LogFileName=${NAME}.trx" \
    --results-directory "${PROJECT_LOG_DIR}" \
    2>&1 | tee "${CONSOLE_LOG}"
  EXIT=${PIPESTATUS[0]}
  EXIT_CODES+=("${EXIT}")

  if [[ ${EXIT} -eq 0 ]]; then
    RESULTS+=("${GREEN}PASS${RESET}  ${NAME}")
  else
    RESULTS+=("${RED}FAIL${RESET}  ${NAME}  (exit ${EXIT})")
  fi
done

# --- Final summary ---
banner "Test run summary"
echo "Logs:    ${LOG_DIR}/  (also: logs/latest)"
echo ""
for R in "${RESULTS[@]}"; do
  echo "  ${R}"
done
echo ""

# --- List failed tests by name, parsed from each .trx ---
ANY_FAILED=0
for PROJECT in "${PROJECTS[@]}"; do
  NAME=$(basename "${PROJECT}" .csproj)
  TRX_FILE="${LOG_DIR}/${NAME}/${NAME}.trx"
  [[ -f "${TRX_FILE}" ]] || continue

  # UnitTestResult elements have both `testName` and `outcome` attributes.
  FAILS=$(grep -E '<UnitTestResult[^>]*outcome="Failed"' "${TRX_FILE}" \
          | sed -nE 's/.*testName="([^"]+)".*/\1/p' \
          | sort -u)

  if [[ -n "${FAILS}" ]]; then
    ANY_FAILED=1
    echo "${RED}${BOLD}Failed in ${NAME}:${RESET}"
    while IFS= read -r LINE; do
      echo "  ${RED}-${RESET} ${LINE}"
    done <<< "${FAILS}"
    echo ""
  fi
done

# --- Exit non-zero if any project failed (so CI / && chains behave correctly) ---
for E in "${EXIT_CODES[@]}"; do
  if [[ ${E} -ne 0 ]]; then
    exit 1
  fi
done
exit 0