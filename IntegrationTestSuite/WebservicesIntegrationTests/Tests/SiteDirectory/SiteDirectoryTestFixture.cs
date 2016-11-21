// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteDirectoryTestFixture.cs" company="RHEA System">
//
//   Copyright 2016 RHEA System 
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebservicesIntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The purpose of the <see cref="SiteDirectoryTestFixture"/> is to execute integration tests using the GET and POST
    /// verbs on SiteDirectory objects
    /// </summary>
    public class SiteDirectoryTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the SiteDirectory object is returned from the data-source and that the 
        /// values of the SiteDirectory properties are equal to to expected value.
        /// </summary>
        [Test]
        public void VerifyThatExpectedSiteDirectoryIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request.
            var siteDirectoryUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));

            // Get the response from the data-source as a JArray (JSON Array).
            var jArray = this.WebClient.GetDto(siteDirectoryUri);

            // assert that the returned SiteDirectory count = 1.
            Assert.AreEqual(1, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id.
            var siteDirectory = jArray.Single(x => (string)x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");

            // verify the properties.
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);
        }

        /// <summary>
        /// Verification that the SiteDirectory object is returned from the data-source and that the 
        /// values of the SiteDirectory properties are equal to to expected value.
        /// </summary>
        [Test]
        public void VerifyThatExpectedSiteDirectoryIsReturnedFromWebApiWhenUniqueIdIsNotSpecified()
        {
            // define the URI on which to perform a GET request
            var siteDirectoryUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory"));

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(siteDirectoryUri);

            // assert that the returned SiteDirectory count = 1
            Assert.AreEqual(1, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string)x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");

            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);
        }

        /// <summary>
        /// Verification that the SiteDirectory object is returned from the data-source and that the 
        /// values of the SiteDirectory properties are equal to to expected value
        /// </summary>
        [Test]        
        public void VerifyThatExpectedSiteDirectoryWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var siteDirectoryUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory?includeAllContainers=true"));

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(siteDirectoryUri);

            // assert that the returned SiteDirectory count = 1
            Assert.AreEqual(1, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string)x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");

            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);
        }   

        /// <summary>
        /// Verifies the properties of the Person <see cref="JToken"/>
        /// </summary>
        /// <param name="siteDirectory">
        /// The <see cref="JToken"/> that contains the properties of
        /// the SiteDirectory object
        /// </param>
        public static void VerifyProperties(JToken siteDirectory)
        {
            // verify that the amount of returned properties 
            Assert.AreEqual(19, siteDirectory.Children().Count());

            Assert.AreEqual("f13de6f8-b03a-46e7-a492-53b2f260f294", (string)siteDirectory["iid"]);
            Assert.AreEqual(1, (int)siteDirectory["revisionNumber"]);
            Assert.AreEqual("SiteDirectory", (string)siteDirectory["classKind"]);
            Assert.AreEqual("2015-04-17T07:48:14.56Z", (string)siteDirectory["lastModifiedOn"]);
            Assert.AreEqual("2016-09-01T08:14:45.461Z", (string)siteDirectory["createdOn"]);
            Assert.AreEqual("Test Site Directory", (string)siteDirectory["name"]);
            Assert.AreEqual("TEST-SiteDir", (string)siteDirectory["shortName"]);
            Assert.AreEqual("ee3ae5ff-ac5e-4957-bab1-7698fba2a267", (string)siteDirectory["defaultParticipantRole"]);
            Assert.AreEqual("2428f4d9-f26d-4112-9d56-1c940748df69", (string)siteDirectory["defaultPersonRole"]);

            var expectedOrganizations = new string[] { "cd22fc45-d898-4fac-85fc-fbcb7d7b12a7" };
            var organizationArray = (JArray)siteDirectory["organization"];
            IList<string> organizations = organizationArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedOrganizations, organizations);

            var expectedPersons = new string[] { "77791b12-4c2c-4499-93fa-869df3692d22" };
            var personArray = (JArray)siteDirectory["person"];
            IList<string> persons = personArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPersons, persons);

            var expectedparticipantRole = new string[] { "ee3ae5ff-ac5e-4957-bab1-7698fba2a267" };
            var participantRoleArray = (JArray)siteDirectory["participantRole"];
            IList<string> participantRoles = participantRoleArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedparticipantRole, participantRoles);

            var expectedsiteReferenceDataLibraries = new string[] { "c454c687-ba3e-44c4-86bc-44544b2c7880" };
            var siteReferenceDataLibraryArray = (JArray)siteDirectory["siteReferenceDataLibrary"];
            IList<string> siteReferenceDataLibraries = siteReferenceDataLibraryArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedsiteReferenceDataLibraries, siteReferenceDataLibraries);

            var expectedModels = new string[] { "116f6253-89bb-47d4-aa24-d11d197e43c9" };
            var modelArray = (JArray)siteDirectory["model"];
            IList<string> models = modelArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedModels, models);

            var expectedPersonRoles = new string[] { "2428f4d9-f26d-4112-9d56-1c940748df69" };
            var personRoleArray = (JArray)siteDirectory["personRole"];
            IList<string> personRoles = personRoleArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPersonRoles, personRoles);

            var expectedlogEntries = new string[]
            {
                "98ba7b8a-1a1b-4569-a17c-b1ff620246a5",
                "66220289-e6ee-43cb-8fcd-d8e59a3dbf97"
            };
            var logEntryArray = (JArray)siteDirectory["logEntry"];
            IList<string> logEntries = logEntryArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedlogEntries, logEntries);

            var expecteddomainGroups = new string[] { "86992db5-8ce2-4431-8ff5-6fe794d14687" };
            var domainGroupArray = (JArray)siteDirectory["domainGroup"];
            IList<string> domainGroups = domainGroupArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expecteddomainGroups, domainGroups);

            var expectedDomains = new string[] { "0e92edde-fdff-41db-9b1d-f2e484f12535" };
            var domainArray = (JArray)siteDirectory["domain"];
            IList<string> domains = domainArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDomains, domains);

            var expectedNaturalLanguages = new string[] { "73bf30cc-3573-488f-8746-6c03ba47973e" };
            var naturalLanguageArray = (JArray)siteDirectory["naturalLanguage"];
            IList<string> naturalLanguages = naturalLanguageArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedNaturalLanguages, naturalLanguages);
        }
    }
}
