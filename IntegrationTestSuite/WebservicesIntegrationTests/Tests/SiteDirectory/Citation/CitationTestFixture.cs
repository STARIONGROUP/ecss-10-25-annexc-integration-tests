// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CitationTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2021 Starion Group S.A.
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

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class CitationTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("POST")]
        public void VerifyThatACitationCanBeCreatedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/SiteDirectory/Citation/PostNewCitation.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");

            // verify that the amount of returned properties 
            Assert.AreEqual(19, siteDirectory.Children().Count());

            // Assert values are as expected
            Assert.AreEqual("f13de6f8-b03a-46e7-a492-53b2f260f294", (string)siteDirectory[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)siteDirectory[PropertyNames.RevisionNumber]);
            Assert.AreEqual("SiteDirectory", (string)siteDirectory[PropertyNames.ClassKind]);
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

            var expectedDomains = new string[]
            {
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

            //Definition
            var definition = jArray.Single(x => (string)x[PropertyNames.Iid] == "23658615-a170-4c0f-ba71-da1a15c736ca");

            // verify that the amount of returned properties 
            Assert.AreEqual(8, definition.Children().Count());

            // Assert values are as expected
            Assert.AreEqual("23658615-a170-4c0f-ba71-da1a15c736ca", (string)definition[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)definition[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Definition", (string)definition[PropertyNames.ClassKind]);
            Assert.AreEqual("Test domain of expertise that is used for the verification of ECSS-E-TM-10-25 Web API", (string)definition[PropertyNames.Content]);
            Assert.AreEqual("en-GB", (string)definition[PropertyNames.LanguageCode]);

            var expectedCitations = new string[] { "b80c270b-8c00-4ed1-979e-9b72611aa6a0" };
            var citationsArray = (JArray)definition[PropertyNames.Citation];
            IList<string> citations = citationsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCitations, citations);

            var expectedExamples = new string[] { };
            var examplesArray = (JArray)definition[PropertyNames.Example];
            IList<string> examples = examplesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedExamples, examples);

            var expectedNotes = new List<OrderedItem> { };
            var notesArray = (JArray)definition[PropertyNames.Note];
            IList<string> notes = notesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedNotes, notes);

            //Citation
            var citation = jArray.Single(x => (string)x[PropertyNames.Iid] == "b80c270b-8c00-4ed1-979e-9b72611aa6a0");

            // verify that the amount of returned properties 
            Assert.AreEqual(8, citation.Children().Count());

            // Assert values are as expected
            Assert.AreEqual("b80c270b-8c00-4ed1-979e-9b72611aa6a0", (string)citation[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)citation[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Citation", (string)citation[PropertyNames.ClassKind]);
            Assert.AreEqual("NewCitation", (string)citation[PropertyNames.ShortName]);
            Assert.AreEqual("none", (string)citation[PropertyNames.Remark]);
            Assert.IsFalse((bool)citation[PropertyNames.IsAdaptation]);
            Assert.AreEqual("Chapter 1", (string)citation[PropertyNames.Location]);
            Assert.AreEqual("ffd6c100-6c72-4d2a-8565-ff24bd576a89", (string)citation[PropertyNames.Source]);
        }
    }
}
