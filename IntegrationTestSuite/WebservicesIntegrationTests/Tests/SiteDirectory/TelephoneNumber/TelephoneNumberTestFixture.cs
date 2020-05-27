// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelephoneNumberTestFixture.cs" company="RHEA System">
//
//   Copyright 2016-2020 RHEA System 
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
    public class TelephoneNumberTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        /// <summary>
        /// Verification that the TelephoneNumber objects are returned from the data-source and that the 
        /// values of the TelephoneNumber properties are equal to to expected value.
        /// </summary>
        [Test]
        public void VerifyThatExpectedTelephoneNumbersIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var telephoneNumbersUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/person/77791b12-4c2c-4499-93fa-869df3692d22/telephoneNumber"));

            // Get the response from the data-source as a JArray (JSON Array).
            var jArray = this.WebClient.GetDto(telephoneNumbersUri);

            // assert that there are 2 TelephoneNumber objects.
            Assert.AreEqual(2, jArray.Count);

            VerifyProperties(jArray);
        }

        [Test]
        public void VerifyThatExpectedTelephoneNumbersWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var telephoneNumbersUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/person/77791b12-4c2c-4499-93fa-869df3692d22/telephoneNumber?includeAllContainers=true"));

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(telephoneNumbersUri);

            // assert that there are 4 objects
            Assert.AreEqual(4, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");

            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific Person from the result by it's unique id
            var person = jArray.Single(x => (string) x[PropertyNames.Iid] == "77791b12-4c2c-4499-93fa-869df3692d22");
            PersonTestFixture.VerifyProperties(person);

            VerifyProperties(jArray);
        }

        [Test]
        public void VerifyThatVcardTypeCanBeRemoved()
        {
            var uri = new Uri(
                string.Format(
                    UriFormat,
                    this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));

            var postBodyPath = this.GetPath("Tests/SiteDirectory/TelephoneNumber/PostNewTelephoneNumber.json");
            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(uri, postBody);

            var telephoneNumber =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "2e6559d6-2ccb-4627-a5f4-5906fb59e969");

            var expectedVcardTypes = new string[]
            {
                "HOME"
            };

            var vcardTypesArray = (JArray) telephoneNumber[PropertyNames.VcardType];
            IList<string> vcardTypes = vcardTypesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedVcardTypes, vcardTypes);

            postBodyPath = this.GetPath("Tests/SiteDirectory/TelephoneNumber/PostDeleteTelephoneNumberVcardType.json");
            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(uri, postBody);

            telephoneNumber =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "2e6559d6-2ccb-4627-a5f4-5906fb59e969");

            expectedVcardTypes = new string[] { };

            vcardTypesArray = (JArray) telephoneNumber[PropertyNames.VcardType];
            vcardTypes = vcardTypesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedVcardTypes, vcardTypes);
        }

        [Test]
        public void VerifyThatVerifyThatATelephoneNumberCanBeCreatedWithWebApi()
        {
            var uri = new Uri(
                string.Format(
                    UriFormat,
                    this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));

            var postBodyPath = this.GetPath("Tests/SiteDirectory/TelephoneNumber/PostNewTelephoneNumber.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(uri, postBody);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");

            // verify that the amount of returned properties 
            Assert.AreEqual(19, siteDirectory.Children().Count());

            Assert.AreEqual("f13de6f8-b03a-46e7-a492-53b2f260f294", (string) siteDirectory[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) siteDirectory[PropertyNames.RevisionNumber]);
            Assert.AreEqual("SiteDirectory", (string) siteDirectory[PropertyNames.ClassKind]);
            Assert.AreEqual("Test Site Directory", (string) siteDirectory[PropertyNames.Name]);
            Assert.AreEqual("TEST-SiteDir", (string) siteDirectory[PropertyNames.ShortName]);

            Assert.AreEqual("ee3ae5ff-ac5e-4957-bab1-7698fba2a267",
                (string) siteDirectory[PropertyNames.DefaultParticipantRole]);

            Assert.AreEqual("2428f4d9-f26d-4112-9d56-1c940748df69",
                (string) siteDirectory[PropertyNames.DefaultPersonRole]);

            var expectedOrganizations = new string[] { "cd22fc45-d898-4fac-85fc-fbcb7d7b12a7" };
            var organizationArray = (JArray) siteDirectory[PropertyNames.Organization];
            IList<string> organizations = organizationArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedOrganizations, organizations);

            var expectedPersons = new string[]
                { "77791b12-4c2c-4499-93fa-869df3692d22" };

            var personArray = (JArray) siteDirectory[PropertyNames.Person];
            IList<string> persons = personArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedPersons, persons);

            var expectedparticipantRole = new string[] { "ee3ae5ff-ac5e-4957-bab1-7698fba2a267" };
            var participantRoleArray = (JArray) siteDirectory[PropertyNames.ParticipantRole];
            IList<string> participantRoles = participantRoleArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedparticipantRole, participantRoles);

            var expectedsiteReferenceDataLibraries = new string[] { "c454c687-ba3e-44c4-86bc-44544b2c7880" };
            var siteReferenceDataLibraryArray = (JArray) siteDirectory[PropertyNames.SiteReferenceDataLibrary];
            IList<string> siteReferenceDataLibraries = siteReferenceDataLibraryArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedsiteReferenceDataLibraries, siteReferenceDataLibraries);

            var expectedModels = new string[] { "116f6253-89bb-47d4-aa24-d11d197e43c9" };
            var modelArray = (JArray) siteDirectory[PropertyNames.Model];
            IList<string> models = modelArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedModels, models);

            var expectedPersonRoles = new string[] { "2428f4d9-f26d-4112-9d56-1c940748df69" };
            var personRoleArray = (JArray) siteDirectory[PropertyNames.PersonRole];
            IList<string> personRoles = personRoleArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedPersonRoles, personRoles);

            var expectedlogEntries = new string[]
            {
                "98ba7b8a-1a1b-4569-a17c-b1ff620246a5",
                "66220289-e6ee-43cb-8fcd-d8e59a3dbf97"
            };

            var logEntryArray = (JArray) siteDirectory[PropertyNames.LogEntry];
            IList<string> logEntries = logEntryArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedlogEntries, logEntries);

            var expecteddomainGroups = new string[] { "86992db5-8ce2-4431-8ff5-6fe794d14687" };
            var domainGroupArray = (JArray) siteDirectory[PropertyNames.DomainGroup];
            IList<string> domainGroups = domainGroupArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expecteddomainGroups, domainGroups);

            var expectedDomains = new string[]
            {
                "0e92edde-fdff-41db-9b1d-f2e484f12535",
                "eb759723-14b9-49f4-8611-544d037bb764"
            };

            var domainArray = (JArray) siteDirectory[PropertyNames.Domain];
            IList<string> domains = domainArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDomains, domains);

            var expectedNaturalLanguages = new string[] { "73bf30cc-3573-488f-8746-6c03ba47973e" };
            var naturalLanguageArray = (JArray) siteDirectory[PropertyNames.NaturalLanguage];
            IList<string> naturalLanguages = naturalLanguageArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedNaturalLanguages, naturalLanguages);

            // get a specific Person from the result by it's unique id
            var person =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "77791b12-4c2c-4499-93fa-869df3692d22");

            // verify that the amount of returned properties 
            Assert.AreEqual(18, person.Children().Count());

            Assert.AreEqual("77791b12-4c2c-4499-93fa-869df3692d22", (string) person[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) person[PropertyNames.RevisionNumber]);

            // assert that the properties are what is expected
            Assert.AreEqual("John", (string) person[PropertyNames.GivenName]);
            Assert.AreEqual("Doe", (string) person[PropertyNames.Surname]);
            Assert.AreEqual("", (string) person[PropertyNames.OrganizationalUnit]);
            Assert.AreEqual(null, (string) person[PropertyNames.Organization]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) person[PropertyNames.DefaultDomain]);
            Assert.IsTrue((bool) person[PropertyNames.IsActive]);
            Assert.AreEqual("2428f4d9-f26d-4112-9d56-1c940748df69", (string) person[PropertyNames.Role]);
            Assert.AreEqual(null, (string) person[PropertyNames.DefaultEmailAddress]);
            Assert.AreEqual(null, (string) person[PropertyNames.DefaultTelephoneNumber]);
            Assert.AreEqual("admin", (string) person[PropertyNames.ShortName]);
            Assert.IsFalse((bool) person[PropertyNames.IsDeprecated]);

            // verify that there are 2 emailAddresses for this person
            var expectedEmailAddresses = new string[]
            {
                "c855d849-62c6-447b-b4e4-db20ba836a91",
                "325620cd-4354-4ddb-9c66-e75550da643a"
            };

            var emailAddresses = (JArray) person[PropertyNames.EmailAddress];
            IList<string> e = emailAddresses.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedEmailAddresses, e);

            // verify that there are 2 telephoneNumbers for this person
            var expectedTelephoneNumbers = new string[]
            {
                "7f85a641-1844-4064-b19d-c6a447543ab3",
                "0367167c-80cb-4f99-a24b-e713efd15436",
                "2e6559d6-2ccb-4627-a5f4-5906fb59e969"
            };

            var telephoneNumbers = (JArray) person[PropertyNames.TelephoneNumber];
            IList<string> t = telephoneNumbers.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedTelephoneNumbers, t);

            // verify that there are no userPreference for this person
            var userPreferences = (JArray) person[PropertyNames.UserPreference];
            IList<string> up = userPreferences.Select(x => (string) x).ToList();
            Assert.IsEmpty(up);

            // get a specific telephoneNumber from the result by it's unique id
            var telephoneNumber =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "2e6559d6-2ccb-4627-a5f4-5906fb59e969");

            // assert that the properties are what is expected
            Assert.AreEqual("2e6559d6-2ccb-4627-a5f4-5906fb59e969", (string) telephoneNumber[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) telephoneNumber[PropertyNames.RevisionNumber]);
            Assert.AreEqual("TelephoneNumber", (string) telephoneNumber[PropertyNames.ClassKind]);
            Assert.AreEqual("+38-012-345-67-89", (string) telephoneNumber[PropertyNames.Value]);

            var expectedVcardTypes = new string[]
            {
                "HOME"
            };

            var vcardTypesArray = (JArray) telephoneNumber[PropertyNames.VcardType];
            IList<string> vcardTypes = vcardTypesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedVcardTypes, vcardTypes);
        }

        /// <summary>
        /// Verifies the properties of the TelephoneNumber <see cref="JToken"/>
        /// </summary>
        /// <param name="telephoneNumber">
        /// The <see cref="JToken"/> that contains the properties of
        /// the TelephoneNumber object
        /// </param>
        public static void VerifyProperties(JToken telephoneNumber)
        {
            var telephoneNumberObject =
                telephoneNumber.Single(x => (string) x[PropertyNames.Iid] == "7f85a641-1844-4064-b19d-c6a447543ab3");

            Assert.AreEqual("7f85a641-1844-4064-b19d-c6a447543ab3", (string) telephoneNumberObject[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) telephoneNumberObject[PropertyNames.RevisionNumber]);
            Assert.AreEqual("TelephoneNumber", (string) telephoneNumberObject[PropertyNames.ClassKind]);
            Assert.AreEqual("+38-044-444-44-44", (string) telephoneNumberObject[PropertyNames.Value]);

            var expectedVcardTypes = new string[]
            {
                "HOME"
            };

            var vcardTypesArray = (JArray) telephoneNumberObject[PropertyNames.VcardType];
            IList<string> vcardTypes = vcardTypesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedVcardTypes, vcardTypes);

            telephoneNumberObject =
                telephoneNumber.Single(x => (string) x[PropertyNames.Iid] == "0367167c-80cb-4f99-a24b-e713efd15436");

            Assert.AreEqual("0367167c-80cb-4f99-a24b-e713efd15436", (string) telephoneNumberObject[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) telephoneNumberObject[PropertyNames.RevisionNumber]);
            Assert.AreEqual("TelephoneNumber", (string) telephoneNumberObject[PropertyNames.ClassKind]);
            Assert.AreEqual("+38-066-666-66-66", (string) telephoneNumberObject[PropertyNames.Value]);

            expectedVcardTypes = new string[]
            {
                "WORK",
                "CELL"
            };

            vcardTypesArray = (JArray) telephoneNumberObject[PropertyNames.VcardType];
            vcardTypes = vcardTypesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedVcardTypes, vcardTypes);
        }
    }
}
