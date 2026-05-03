# ecss-10-25-annexc-integration-tests

This repository contains ECSS-E-TM-10-25 Annex C integration tests. The purpose of the software is to achieve 100% code coverage of all REST API queries that can be performed to determine wheter an ECSS-E-TM-10-25 Annex C implementation is compliant with the ECSS-E-TM-10-25 Annex C. On top of that this test-suite also covers CDP4-COMET specific extensions, these tests are decorated with an Nunit Category attribute so they can be filtered out by the Nunit runner. The Category used here name is 'CdpVersion_1_1_0'.
See [this website](https://github.com/nunit/docs/wiki/Console-Command-Line) for more info on Nunit test filtering.

## ECSS-E-TM-10-25

ECSS-E-TM-10-25 is a Technical Memorandum under the E-10 System engineering branch in the [ECSS](http://ecss.nl) series of standards, handbooks and technical memoranda. ECSS-E-TM-10-25 facilitates and promotes common data definitions and exchange among partner Agencies, European space industry and institutes, which are interested to collaborate on concurrent design, sharing analysis and design outputs and related reviews. This comprises a system decomposition up to equipment level and related standard lists of parameters and disciplines. Further it provides the starting point of the space system life cycle defining the parameter sets required to cover all project phases, although the present Technical Memorandum only addresses Phases 0 and A.

Read more about ECSS-E-TM-10-25 [here](http://ecss.nl/hbstms/ecss-e-tm-10-25a-engineering-design-model-data-exchange-cdf-20-october-2010/)

## Definition of ECSS-E-TM-10-25 Annex C (informative) 

The following interface technologies shall be supported by a fully compliant implementation of ECSS-E-TM-10-25:

* Annex C.2 - The Web Services REST API
  * the REST API that allows client applications to communicate with an ECSS-E-TM-10-25 compliant server at different levels of granularity.
  * The typical create, read, update and delete (CRUD) actions are supported, both for single ECSS-E-TM-10-25 objects and collections or graphs of ECSS-E-TM-10-25 objects.
* Annex C.3 - JSON Exchange File Format
  * The file format that is used to serialize ECSS-E-TM-10-25 datasets to a file for the purpose of file based exchange

## Definition of ECSS-E-TM-10-25 Annex C - Integration test suite extensions

In order to support the integration testing. 2 routes have been added to Annex C.2 with the following purpose:

  - An "upload an Annex C.3 compliant seed file" service. The seed file forms the basis for the tests and is the known state of the data set on which the tests have been based. By uploading such a seed file the complete data set is reset to the state described in the seed file.
  - A "restore" service that restores the dataset on the service to the last seeded file.
  
### Upload

The URL of the upload service is the following: "http(s)://hostname:port/Data/Exchange". The seed file shall be uploaded in a multi-part message. We use curl to upload an Annex C.3 file:

```
curl --form file=@"Data.zip" http://cdp4services-test.cdp4.org/Data/Exchange
```

### Restore

The URL of the restore service takes the following form: "http(s)://hostname:port/Data/Restore".

## Integration test suite how-to

The integration test suite is run via the `run-integration-tests.sh` script in the repository root. This script is the canonical entry point for both local development and CI — it handles seeding, building, running, and summarising results in one go. Do not invoke `dotnet test` directly against the projects unless you are debugging a single fixture; the suite assumes the database has been freshly seeded from `Data/` before the first test runs.

### Prerequisites

- A running ECSS-E-TM-10-25 Annex C compliant webservice reachable at the URL the script will hit (default `http://localhost:5000`).
- `Backtier__IsDbSeedEnabled=true` set on that webservice — without it the `/Data/Exchange` seed step returns a non-2xx response and the script aborts.
- The .NET 10 SDK on `PATH`.
- `zip` and `curl` on `PATH`. On Windows, run the script from Git Bash or WSL — there is no PowerShell equivalent.

### Running the suite

From the repository root:

```bash
./run-integration-tests.sh
```

To target a webservice at a different URL, set `WEBSERVICE_URL`:

```bash
WEBSERVICE_URL=http://my-host:5000 ./run-integration-tests.sh
```

The script then performs five phases, each announced with a banner in the console:

1. **Seed** — zips the *contents* of `Data/` (so `Header.json` and friends sit at the archive root, as Annex C.3 requires) and POSTs the archive to `${WEBSERVICE_URL}/Data/Exchange`. The zip and the server's response body are saved to `logs/<timestamp>/`. A non-2xx response prints a clear error and exits 1.
2. **Restore + build** — runs `dotnet restore` and `dotnet build -c Release --no-restore` against `IntegrationTestSuite/WebservicesIntegrationTests.sln`.
3. **Run each project** — `MessagePackIntegrationTests` first, then `WebservicesIntegrationTests`. Each runs with `--no-build`, the console logger at minimal verbosity, and a `trx` logger producing `<Project>.trx` in `logs/<timestamp>/<Project>/`. The console output is teed to `console.log` in the same directory.
4. **Summary** — prints PASS/FAIL per project and the path to the run's log directory.
5. **Failure detail** — parses every `.trx` and lists the names of all tests with `outcome="Failed"`, sorted and de-duplicated, so you can re-run them individually with `dotnet test --filter`.

The script's exit code is 0 only when every project returned 0, so it composes correctly in `&&` chains and CI pipelines.

### Where to look after a run

- `logs/<timestamp>/` — that run's artifacts. `logs/latest` is a symlink to the most recent run.
- `logs/<timestamp>/seed-response.txt` — the webservice's response if seeding failed.
- `logs/<timestamp>/<Project>/console.log` and `<Project>.trx` — per-project diagnostics.

### Notes

- The suite is `[SingleThreaded]` by design — every test runs against the same backend and shares state. Do not run two copies of the script against the same webservice concurrently.
- The fixtures hard-code IIDs and revisions that come from `Data/`. Never edit `Data/` to make a test pass; the seed *is* the contract.
- For deeper background on the development environment see the [project wiki](https://github.com/STARIONGROUP/ecss-10-25-annexc-integration-tests/wiki).

# CDP4-COMET

CDP4-COMET is the Starion Group implementation of ECSS-E-TM-10-25 and is a so-called Concurrent Design Platform or collaborative MBSE application. CDP4-COMET is a typical 3-tier application that contains the following application layers:
* Layer-1: Persistent Data Store (data layer)
  * implemented using [PostgreSQL](http://www.postgresql.org)
* Layer-2: REST Web Services (application layer)
  * COMET WebServices
  * compliant with ECSS-E-TM-10-25 Annex C.2
* Layer-3: Client tools (presentation layer), the following applications are available:
  * COMET-IME: A desktop application
  * COMET-ADDIN: A Microsoft Excel ADDIN
  * COMET-WEB: A browser application

More information about CDP4-COMET can be found on the Starion Group [Website](https://www.stariongroup.eu/services-solutions/system-engineering/concurrent-design/). The installer of the CDP4-COMET-IME can be downloaded here: https://www.stariongroup.eu/services-solutions/system-engineering/concurrent-design/cdp4-comet/.

# Sponsors

The ecss-10-25-annexc-integration-tests project is sponsored by the [Starion Group](https://www.stariongroup.eu)