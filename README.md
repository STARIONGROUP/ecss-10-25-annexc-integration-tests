# ecss-10-25-annexc-integration-tests
This repository contains ECSS-E-TM-10-25 Annex C integration tests. The purpose of the software is to achieve 100% code coverage of all REST API queries that can be performed to determine wheter an ECSS-E-TM-10-25 Annex C implementation is compliant with the ECSS-E-TM-10-25 Annex C.

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

# CDP4

The RHEA Concurrent Design Platform (CDP&trade;) is the RHEA implementation of ECSS-E-TM-10-25. The CDP4&trade; is a typical 3-tier application that contains the following application layers:
* Layer-1: Persistent Data Store (data layer)
  * implemented using [PostgreSQL](http://www.postgresql.org)
* Layer-2: REST Web Services (application layer)
  * CDP4&trade; WebServices
  * compliant with ECSS-E-TM-10-25 Annex C.2
* Layer-3: Client tools (presentation layer), the following applications are available:
  * CDP4-IME&trade;: A desktop application
  * CDP4-ADDIN&trade;: A Microsoft Excel ADDIN
  * CDP4-WEBAPP&trade;: A Web Application hosted by the CDP4 WebServices

More information about the CDP&trade; can be found on the RHEA GROUP [Website](http://www.rheagroup.com/products/cdp/).

A Public instance of the CDP4&trade; WebServices is available at http://cdp4services-public.rheagroup.com. The username and password are: admin/pass.  

# OCDT

The ESA Community open source implementation of ECSS-E-TM-10-25. More information can be found [here](https://ocdt.esa.int)

# Sponsors

The ecss-10-25-annexc-integration-tests project is sponsored by the [RHEA GROUP](http://www.rheagroup.com)