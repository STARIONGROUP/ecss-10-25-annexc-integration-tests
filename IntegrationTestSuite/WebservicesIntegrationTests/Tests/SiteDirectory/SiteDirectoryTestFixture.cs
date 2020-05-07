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
    using System.IO;
    using System.Linq;

    using Ionic.Zip;

    using NUnit.Framework;

    using Newtonsoft.Json.Linq;

    using WebservicesIntegrationTests.Net;

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

            // assert that the returned count is as expected
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

        [Test]
        public void VerifyThatExpectedSiteDirectoryIsReturnedFromWebApiWhenUniqueIdIsSpecifiedAndExtentEqualsDeep()
        {
            // define the URI on which to perform a GET request
            var siteDirectoryUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294?extent=deep&includeReferenceData=false"));

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(siteDirectoryUri);
            
            // assert that the returned count is as expected
            Assert.AreEqual(60, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string)x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");

            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);
        }

        [Test]
        public void VerifyThatGetWithWildCardReturnsExpectedResult()
        {
            // define the URI on which to perform a GET request
            var siteDirectoryUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory/*?extent=deep&includeReferenceData=false"));
            
            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(siteDirectoryUri);

            // assert that the returned count is as expected
            Assert.AreEqual(60, jArray.Count);

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

            // assert that the returned count is as expected
            Assert.AreEqual(1, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string)x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");

            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);
        }

        [Test]
        public void VerifyThatAModelCanBeExportedWithWebApi()
        {
            string[] engineeringodelSetupIds = { "116f6253-89bb-47d4-aa24-d11d197e43c9" };

            // define the URI on which to perform a POST request
            var siteDirectoryUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294?export=true"));

            // Download a model export zip file
            var responseBody = this.WebClient.GetModelExportFile(siteDirectoryUri, engineeringodelSetupIds);

            var path = Path.GetTempFileName();
            File.WriteAllBytes(path, responseBody);
            ZipFile zip = new ZipFile(path);

            // It is assumed that if some information is retrieved from the archive than it is not corrupted
            Assert.AreEqual(7, zip.Count);

            var expectedZipEntries = new string[]
                                            {
                                                "Header.json",
                                                "SiteDirectory.json",
                                                "SiteReferenceDataLibraries/c454c687-ba3e-44c4-86bc-44544b2c7880.json",
                                                "ModelReferenceDataLibraries/3483f2b5-ea29-45cc-8a46-f5f598558fc3.json",
                                                "EngineeringModels/9ec982e4-ef72-4953-aa85-b158a95d8d56/9ec982e4-ef72-4953-aa85-b158a95d8d56.json",
                                                "EngineeringModels/9ec982e4-ef72-4953-aa85-b158a95d8d56/Iterations/e163c5ad-f32b-4387-b805-f4b34600bc2c.json",
                                                "EngineeringModels/9ec982e4-ef72-4953-aa85-b158a95d8d56/FileRevisions/B95EC201AE3EE89D407449D692E69BB97C228A7E"
                                            };
            CollectionAssert.AreEquivalent(expectedZipEntries, zip.EntryFileNames.ToArray());
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

            Assert.AreEqual("f13de6f8-b03a-46e7-a492-53b2f260f294", (string)siteDirectory[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)siteDirectory[PropertyNames.RevisionNumber]);
            Assert.AreEqual("SiteDirectory", (string)siteDirectory[PropertyNames.ClassKind]);
            Assert.AreEqual("2015-04-17T07:48:14.560Z", (string)siteDirectory[PropertyNames.LastModifiedOn]);
            Assert.AreEqual("2016-09-01T08:14:45.461Z", (string)siteDirectory[PropertyNames.CreatedOn]);
            Assert.AreEqual("Test Site Directory", (string)siteDirectory[PropertyNames.Name]);
            Assert.AreEqual("TEST-SiteDir", (string)siteDirectory[PropertyNames.ShortName]);
            Assert.AreEqual("ee3ae5ff-ac5e-4957-bab1-7698fba2a267", (string)siteDirectory[PropertyNames.DefaultParticipantRole]);
            Assert.AreEqual("2428f4d9-f26d-4112-9d56-1c940748df69", (string)siteDirectory[PropertyNames.DefaultPersonRole]);

            var expectedOrganizations = new string[] { "cd22fc45-d898-4fac-85fc-fbcb7d7b12a7" };
            var organizationArray = (JArray)siteDirectory[PropertyNames.Organization];
            IList<string> organizations = organizationArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedOrganizations, organizations);

            var expectedPersons = new string[] { "77791b12-4c2c-4499-93fa-869df3692d22" };
            var personArray = (JArray)siteDirectory[PropertyNames.Person];
            IList<string> persons = personArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPersons, persons);

            var expectedparticipantRole = new string[] { "ee3ae5ff-ac5e-4957-bab1-7698fba2a267" };
            var participantRoleArray = (JArray)siteDirectory[PropertyNames.ParticipantRole];
            IList<string> participantRoles = participantRoleArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedparticipantRole, participantRoles);

            var expectedsiteReferenceDataLibraries = new string[] { "c454c687-ba3e-44c4-86bc-44544b2c7880" };
            var siteReferenceDataLibraryArray = (JArray)siteDirectory[PropertyNames.SiteReferenceDataLibrary];
            IList<string> siteReferenceDataLibraries = siteReferenceDataLibraryArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedsiteReferenceDataLibraries, siteReferenceDataLibraries);

            var expectedModels = new string[] { "116f6253-89bb-47d4-aa24-d11d197e43c9" };
            var modelArray = (JArray)siteDirectory[PropertyNames.Model];
            IList<string> models = modelArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedModels, models);

            var expectedPersonRoles = new string[] { "2428f4d9-f26d-4112-9d56-1c940748df69" };
            var personRoleArray = (JArray)siteDirectory[PropertyNames.PersonRole];
            IList<string> personRoles = personRoleArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPersonRoles, personRoles);

            var expectedlogEntries = new string[]
            {
                "98ba7b8a-1a1b-4569-a17c-b1ff620246a5",
                "66220289-e6ee-43cb-8fcd-d8e59a3dbf97"
            };
            var logEntryArray = (JArray)siteDirectory[PropertyNames.LogEntry];
            IList<string> logEntries = logEntryArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedlogEntries, logEntries);

            var expecteddomainGroups = new string[] { "86992db5-8ce2-4431-8ff5-6fe794d14687" };
            var domainGroupArray = (JArray)siteDirectory[PropertyNames.DomainGroup];
            IList<string> domainGroups = domainGroupArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expecteddomainGroups, domainGroups);

            var expectedDomains = new string[] {
                "0e92edde-fdff-41db-9b1d-f2e484f12535",
                "eb759723-14b9-49f4-8611-544d037bb764"
            };
            var domainArray = (JArray)siteDirectory[PropertyNames.Domain];
            IList<string> domains = domainArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDomains, domains);

            var expectedNaturalLanguages = new string[] { "73bf30cc-3573-488f-8746-6c03ba47973e" };
            var naturalLanguageArray = (JArray)siteDirectory[PropertyNames.NaturalLanguage];
            IList<string> naturalLanguages = naturalLanguageArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedNaturalLanguages, naturalLanguages);
        }

        /// <summary>
        /// An isolated method that can add a specific user to the datastore using a <see cref="WebClientTestFixtureBase"/>'s <see cref="WebClient"/>
        /// </summary>
        public static void AddDomainExpertUserJane(WebClientTestFixtureBase testFixture, out string userName, out string passWord)
        {
            // define the URI on which to perform a GET request
            var uri =
                new Uri(string.Format(UriFormat, testFixture.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));

            var postBodyPath = testFixture.GetPath("Tests/SiteDirectory/POSTNewDomainExpertUser.json");

            var postBody = testFixture.GetJsonFromFile(postBodyPath);
            var jArray = testFixture.WebClient.PostDto(uri, postBody);

            // check if there are 29 objects
            Assert.AreEqual(29, jArray.Count);

            userName = "Jane";
            passWord = "Jane";
        }
    }
}
