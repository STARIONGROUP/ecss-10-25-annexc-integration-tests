// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonTestFixture.cs" company="RHEA System S.A.">
//
//   Copyright 2016-2021 RHEA System S.A.
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
    using System.Net;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    public class PersonTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedPersonIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var personsUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/person"));

            // Get the response from the data-source as a JArray (JSON Array).
            var jArray = this.WebClient.GetDto(personsUri);

            // assert that there is only 1 Person object.
            Assert.AreEqual(1, jArray.Count);

            // get a specific Person from the result by it's unique id.
            var person = jArray.Single(x => (string) x[PropertyNames.Iid] == "77791b12-4c2c-4499-93fa-869df3692d22");

            PersonTestFixture.VerifyProperties(person);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedPersonWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var personsUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/person?includeAllContainers=true"));

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(personsUri);

            // assert that the returned person count = 2
            Assert.AreEqual(2, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific Person from the result by it's unique id
            var person = jArray.Single(x => (string) x[PropertyNames.Iid] == "77791b12-4c2c-4499-93fa-869df3692d22");
            PersonTestFixture.VerifyProperties(person);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatTelephoneDeletionAsPropertyFromPersonCanBeDoneFromWebApi()
        {
            var uri = new Uri(
                string.Format(
                    UriFormat,
                    this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));
            var postBodyPath = this.GetPath("Tests/SiteDirectory/Person/PostDeleteTelephoneAsProperty.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(uri, postBody);

            // check if there are 2 objects
            Assert.AreEqual(2, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.AreEqual(2, (int)siteDirectory[PropertyNames.RevisionNumber]);

            // get a specific Person from the result by it's unique id
            var person = jArray.Single(x => (string)x[PropertyNames.Iid] == "77791b12-4c2c-4499-93fa-869df3692d22");
            Assert.AreEqual(2, (int)person[PropertyNames.RevisionNumber]);
            var expectedTelephoneNumbers = new string[]
                                               {
                                                   "0367167c-80cb-4f99-a24b-e713efd15436"
                                               };
            var telephoneNumbers = (JArray)person[PropertyNames.TelephoneNumber];
            IList<string> t = telephoneNumbers.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedTelephoneNumbers, t);

            // define the URI on which to perform a GET request 
            var phoneUri = new Uri(
                string.Format(
                    UriFormat,
                    this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/person/77791b12-4c2c-4499-93fa-869df3692d22/telephoneNumber/7f85a641-1844-4064-b19d-c6a447543ab3"));

            Assert.That(() => this.WebClient.GetDto(phoneUri), Throws.Exception.TypeOf<System.Net.WebException>());
        }

        [Test]
        [Category("POST")]
        public void Verify_That_person_with_null_role_and_null_password_can_be_posted()
        {
            var uri = new Uri(
                string.Format(
                    UriFormat,
                    this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));
            var postBodyPath = this.GetPath("Tests/SiteDirectory/Person/Post_Person_With_Role_Null.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(uri, postBody);

            // check if there are 2 objects
            Assert.AreEqual(2, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            
            // verify that the amount of returned properties 
            Assert.AreEqual(19, siteDirectory.Children().Count());

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

            var expectedPersons = new string[] { "77791b12-4c2c-4499-93fa-869df3692d22", "01a6d208-7bb5-4855-a6fb-eb3d03f1337b" };
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

            // get a specific Person from the result by it's unique id
            var person = jArray.Single(x => (string)x[PropertyNames.Iid] == "01a6d208-7bb5-4855-a6fb-eb3d03f1337b");
            // verify that the amount of returned properties 
            Assert.AreEqual(18, person.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("01a6d208-7bb5-4855-a6fb-eb3d03f1337b", (string)person[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)person[PropertyNames.RevisionNumber]);
            Assert.AreEqual("no", (string)person[PropertyNames.GivenName]);
            Assert.AreEqual("role", (string)person[PropertyNames.Surname]);
            Assert.AreEqual(null, (string)person[PropertyNames.OrganizationalUnit]);
            Assert.AreEqual(null, (string)person[PropertyNames.Organization]);
            Assert.AreEqual(null, (string)person[PropertyNames.DefaultDomain]);
            Assert.IsFalse((bool)person[PropertyNames.IsActive]);
            Assert.AreEqual(null, (string)person[PropertyNames.Role]);
            Assert.AreEqual(null, (string)person[PropertyNames.DefaultEmailAddress]);
            Assert.AreEqual(null, (string)person[PropertyNames.DefaultTelephoneNumber]);
            Assert.AreEqual("norole", (string)person[PropertyNames.ShortName]);
            Assert.IsFalse((bool)person[PropertyNames.IsDeprecated]);

            var expectedEmailAddresses = new string[]{};
            var emailAddresses = (JArray)person[PropertyNames.EmailAddress];
            IList<string> e = emailAddresses.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedEmailAddresses, e);

            var expectedTelephoneNumbers = new string[]{};
            var telephoneNumbers = (JArray)person[PropertyNames.TelephoneNumber];
            IList<string> t = telephoneNumbers.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedTelephoneNumbers, t);

            var userPreferences = (JArray)person[PropertyNames.UserPreference];
            IList<string> up = userPreferences.Select(x => (string)x).ToList();
            Assert.IsEmpty(up);
        }

        [Test]
        [Category("POST")]
        public void Verify_That_password_policies_are_implemented_correctly()
        {
            var uri = new Uri(
                string.Format(
                    UriFormat,
                    this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));

            var postBodyPath = this.GetPath("Tests/SiteDirectory/Person/Post_Persons_With_Passwords.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(uri, postBody);

            // check if there are 4 objects
            Assert.AreEqual(4, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");

            var expectedPersons = new []
            {
                "77791b12-4c2c-4499-93fa-869df3692d22", 
                "01a6d208-7bb5-4855-a6fb-eb3d03f1337b",
                "01a6d208-7bb5-4855-a6fb-eb3d03f1337c",
                "01a6d208-7bb5-4855-a6fb-eb3d03f1337d"
            };

            var personArray = (JArray)siteDirectory[PropertyNames.Person];
            IList<string> persons = personArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPersons, persons);

            // get a specific Person from the result by it's unique id
            var personWithNullPassword = jArray.Single(x => (string)x[PropertyNames.Iid] == "01a6d208-7bb5-4855-a6fb-eb3d03f1337b");
            var personWithEmptyPassword = jArray.Single(x => (string)x[PropertyNames.Iid] == "01a6d208-7bb5-4855-a6fb-eb3d03f1337c");
            var personWithPassword = jArray.Single(x => (string)x[PropertyNames.Iid] == "01a6d208-7bb5-4855-a6fb-eb3d03f1337d");

            var personWithNullPasswordUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/person/01a6d208-7bb5-4855-a6fb-eb3d03f1337b"));

            this.CreateNewWebClientForUser(personWithNullPassword[PropertyNames.ShortName].ToString(), null);
            var exception = Assert.Catch<WebException>(() => this.WebClient.GetDto(personWithNullPasswordUri));
            Assert.AreEqual(HttpStatusCode.Unauthorized, ((HttpWebResponse)exception.Response).StatusCode);

            var personWithEmptyPasswordUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/person/01a6d208-7bb5-4855-a6fb-eb3d03f1337c"));

            this.CreateNewWebClientForUser(personWithEmptyPassword[PropertyNames.ShortName].ToString(), "");
            exception = Assert.Catch<WebException>(() => this.WebClient.GetDto(personWithEmptyPasswordUri));
            var errorMessage = this.WebClient.ExtractExceptionStringFromResponse(exception.Response);
            Assert.AreEqual(HttpStatusCode.InternalServerError, ((HttpWebResponse)exception.Response).StatusCode);
            Assert.IsTrue(errorMessage.Contains("Sequence contains no elements"));

            var personWithPasswordUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/person/01a6d208-7bb5-4855-a6fb-eb3d03f1337d"));

            this.CreateNewWebClientForUser(personWithPassword[PropertyNames.ShortName].ToString(), "pass");
            exception = Assert.Catch<WebException>(() => this.WebClient.GetDto(personWithPasswordUri));
            errorMessage = this.WebClient.ExtractExceptionStringFromResponse(exception.Response);
            Assert.AreEqual(HttpStatusCode.InternalServerError, ((HttpWebResponse) exception.Response).StatusCode);
            Assert.IsTrue(errorMessage.Contains("Sequence contains no elements"));
        }

        /// <summary>
        /// Verifies the properties of the Person <see cref="JToken"/>
        /// </summary>
        /// <param name="person">
        /// The <see cref="JToken"/> that contains the properties of
        /// the Person object
        /// </param>
        public static void VerifyProperties(JToken person)
        {
            // verify that the amount of returned properties 
            Assert.AreEqual(18, person.Children().Count());

            Assert.AreEqual("77791b12-4c2c-4499-93fa-869df3692d22", (string) person[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) person[PropertyNames.RevisionNumber]);

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
            var emailAddresses = (JArray)person[PropertyNames.EmailAddress];
            IList<string> e = emailAddresses.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedEmailAddresses, e);

            // verify that there are 2 telephoneNumbers for this person
            var expectedTelephoneNumbers = new string[]
            {
                "7f85a641-1844-4064-b19d-c6a447543ab3",
                "0367167c-80cb-4f99-a24b-e713efd15436"
            };
            var telephoneNumbers = (JArray) person[PropertyNames.TelephoneNumber];
            IList<string> t = telephoneNumbers.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedTelephoneNumbers, t);

            // verify that there are no userPreference for this person
            var userPreferences = (JArray) person[PropertyNames.UserPreference];
            IList<string> up = userPreferences.Select(x => (string) x).ToList();
            Assert.IsEmpty(up);
        }
    }
}
