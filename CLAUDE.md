# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this repo is

Integration test suite that exercises an ECSS-E-TM-10-25 Annex C compliant REST API over HTTP. Tests assume a live webservice is reachable (default `http://localhost:5000`); they do not stand one up. The suite drives 100% coverage of Annex C.2 routes and also covers CDP4-COMET-specific extensions, which are tagged `[CdpVersion_1_1_0]` (an NUnit Category) so the runner can filter them out for vanilla compliance runs.

## Common commands

The runtime is .NET 10 (`net10.0`). Solution: `IntegrationTestSuite/WebservicesIntegrationTests.sln`.

```bash
# Restore + build everything
dotnet restore IntegrationTestSuite/WebservicesIntegrationTests.sln
dotnet build IntegrationTestSuite/WebservicesIntegrationTests.sln -c Release --no-restore

# Run all integration tests against a seeded service (also seeds Data/ via /Data/Exchange)
./run-integration-tests.sh                     # uses WEBSERVICE_URL or http://localhost:5000

# Run a single project
dotnet test IntegrationTestSuite/WebservicesIntegrationTests/WebservicesIntegrationTests.csproj -c Release
dotnet test IntegrationTestSuite/MessagePackIntegrationTests/MessagePackIntegrationTests.csproj -c Release

# Run a single fixture / single test
dotnet test IntegrationTestSuite/WebservicesIntegrationTests/WebservicesIntegrationTests.csproj \
  --filter "FullyQualifiedName~SiteDirectoryTestFixture"
dotnet test IntegrationTestSuite/WebservicesIntegrationTests/WebservicesIntegrationTests.csproj \
  --filter "Name=VerifyThatExpectedSiteDirectoryIsReturnedFromWebApi"

# Compliance run only (exclude CDP4-COMET extensions)
dotnet test ... --filter "Category!=CdpVersion_1_1_0"
```

`run-integration-tests.sh` zips the contents of `Data/` (Annex C.3 seed: `Header.json`, `SiteDirectory.json`, etc. must be at the archive root, not under `Data/`), POSTs it to `${WEBSERVICE_URL}/Data/Exchange`, then builds and runs each test project, writing per-run output to `logs/<timestamp>/` and updating the `logs/latest` symlink. The seeded webservice must have `Backtier__IsDbSeedEnabled=true`.

## Pre-flight before running tests

1. A compliant webservice is up and reachable at the URL in `settings.json` / `appsettings.json` (or `WEBSERVICE_URL`).
2. `Backtier__IsDbSeedEnabled=true` is set on the webservice (otherwise `/Data/Exchange` returns non-2xx).
3. The `Data/` directory matches the dataset the tests expect — every IID, GUID, and revision number hard-coded in the fixtures comes from this seed. Don't change `Data/` to make a test pass; the tests are the contract.

## Architecture

Two test projects sit under `IntegrationTestSuite/`:

- **`WebservicesIntegrationTests`** — the bulk of the suite. NUnit 4, Newtonsoft.Json. Drives the Annex C.2 REST API and asserts on `JArray` responses.
- **`MessagePackIntegrationTests`** — narrow project that verifies the MessagePack content-type negotiation produces the same graph as JSON. Uses `CDP4MessagePackSerializer-CE` and `Microsoft.Extensions.Configuration` (so its config file is `appsettings.json`, not `settings.json`).

### How a fixture works

`WebservicesIntegrationTests/Net/WebClient.cs` is the only HTTP client; tests never `new HttpClient` directly. It exposes `GetDto`, `PostDto`, `PostFile`, `GetFileResponseBody`, `GetModelExportFile`, and `Restore`, all using Basic auth with credentials from `settings.json`.

There are two base classes — pick the right one:

- **`WebClientTestFixtureBase`** — read-only fixtures (GET tests, e.g. `SiteDirectoryTestFixture`). No DB restore.
- **`WebClientTestFixtureBaseWithDatabaseRestore`** — fixtures that mutate state. Calls `/Data/Restore` once on first SetUp (a static `_firstRun` guard) and again on every TearDown, so each test starts from the seeded baseline. Use this whenever a test does POST/PUT/DELETE.

Both base classes are `[SingleThreaded]` because the whole suite shares one backend; do not parallelize.

Tests are organized by ECSS class under `Tests/<area>/<EntityName>/`:
- `Tests/SiteDirectory/...` — site-level entities (`Person`, `Category`, `EngineeringModelSetup`, parameter types, scales, RDLs, etc.)
- `Tests/EngineeringModel/...` — engineering-model entities (`Parameter`, `ParameterValueSet`, `Iteration`, file stores, requirements, expressions)
- `Tests/CherryPick/`, `Tests/Export/` — cross-cutting endpoints

Each entity directory contains one fixture class plus the JSON request bodies it posts (`PostNewX.json`, `PostUpdateX.json`, etc.). **Every JSON file must be registered in `WebservicesIntegrationTests.csproj` with `<CopyToOutputDirectory>Always</CopyToOutputDirectory>`** — there is no glob, so adding a new payload requires a corresponding `<None Update="...">` entry. Tests load files via `this.GetPath("Tests/Area/Entity/PostX.json")`.

`PropertyNames.cs` (in `Tests/SiteDirectory/`) holds string constants for every JSON property the tests assert on — use these constants instead of literal strings when extending fixtures.

### CDP version targeting

`CdpVersionAttribute` (abstract) extends `CategoryAttribute`; `CdpVersion_1_1_0Attribute` is a concrete subclass that both (a) tags a test with NUnit category `CdpVersion_1_1_0` for filtering and (b) is read reflectively by `WebClient.PostDto` to set the `Accept-CDP` / `CDP-Version` header for that single request. If you write a test that only makes sense on COMET 1.1.0+, decorate it with `[CdpVersion_1_1_0]` — do not just add the category by name.

## When adding a test that mutates state

1. Inherit from `WebClientTestFixtureBaseWithDatabaseRestore`, not the plain base.
2. Place the fixture under `Tests/<Area>/<Entity>/<Entity>TestFixture.cs`.
3. Put POST/PUT bodies as JSON files next to the fixture.
4. Add `<None Update="Tests/Area/Entity/YourFile.json"><CopyToOutputDirectory>Always</CopyToOutputDirectory></None>` to `WebservicesIntegrationTests.csproj`. Forgetting this manifests as `FileNotFoundException` at runtime, not a build error.
5. If the assertion depends on COMET-only behavior, add `[CdpVersion_1_1_0]`.
6. Use IIDs / shortguids that exist in `Data/SiteDirectory.json` and `Data/EngineeringModels/...` — the seed is the source of truth.
