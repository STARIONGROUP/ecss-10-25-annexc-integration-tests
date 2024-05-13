# ecss-10-25-annexc-integration-tests
This repository contains ECSS-E-TM-10-25 Annex C integration tests. The purpose of the software is to achieve 100% code coverage of all REST API queries that can be performed to determine wheter an ECSS-E-TM-10-25 Annex C implementation is compliant with the ECSS-E-TM-10-25 Annex C.
In May 2020 a few tests were added for a specific CDP4 extension. These tests are decorated with an Nunit Category attribute so they can be filtered out by the Nunit runner. The Category used here name is 'CdpVersion_1_1_0'.
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

Read more about setting up your development environnment [here](https://github.com/STARIONGROUP/ecss-10-25-annexc-integration-tests/wiki)

# COMET

COMET is the Starion Group implementation of ECSS-E-TM-10-25 and is a so-called Concurrent Design Platform or collaborative MBSE application. COMET is a typical 3-tier application that contains the following application layers:
* Layer-1: Persistent Data Store (data layer)
  * implemented using [PostgreSQL](http://www.postgresql.org)
* Layer-2: REST Web Services (application layer)
  * COMET WebServices
  * compliant with ECSS-E-TM-10-25 Annex C.2
* Layer-3: Client tools (presentation layer), the following applications are available:
  * COMET-IME: A desktop application
  * COMET-ADDIN: A Microsoft Excel ADDIN

More information about COMET can be found on the Starion Group [Website](https://www.stariongroup.eu/services-solutions/system-engineering/concurrent-design/). A demo installer of COMET-IME can be downloaded here: https://www.stariongroup.eu/services-solutions/system-engineering/concurrent-design/cdp4-comet/.

A Public instance of COMET WebServices that is used to verify COMET WebServices compliance is available at http://cdp4services-test.cdp4.org. The content that is serviced by this instance is loaded by the contents of the [Data folder](./Data/) . The username and password to access this COMET WebServices instances are: admin/pass.   

# OCDT

The ESA Community open source implementation of ECSS-E-TM-10-25. More information can be found [here](https://ocdt.esa.int)

# Sponsors

The ecss-10-25-annexc-integration-tests project is sponsored by the [Starion Group](https://www.stariongroup.eu)