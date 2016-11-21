// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EngineeringModelSetupTestFixture.cs" company="RHEA System">
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
    using WebservicesIntegrationTests.Net;

    /// <summary>
    /// The purpose of the <see cref="EngineeringModelSetupTestFixture"/> is to GET and POST model objects
    /// </summary>
    [TestFixture]
    public class EngineeringModelSetupTestFixture : WebClientTestFixtureBase
    {
        public override void SetUp()
        {
            base.SetUp();

            this.WebClient.Restore(this.Settings.Hostname);
        }

        public override void TearDown()
        {
            this.WebClient.Restore(this.Settings.Hostname);

            base.TearDown();
        }

        [Test]
        public void VerifyThatNewEngineeringModelCanBeCreatedWithWebApi()
        {
            var siteDirectoryUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));
            var postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetup.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var response = this.WebClient.PostDto(siteDirectoryUri, postBody);

            Console.WriteLine(response);
        }

        [Test]
        public void VerifyThatNewEngineeringModelCanBeCreatedBasedOnExistingModelWithWebApi()
        {
            var siteDirectoryUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));
            var postBodyPath =
                this.GetPath(
                    "Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetupBasedOnExistingModel.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            //check if there is the only one Glossary object 
            Assert.AreEqual(5, jArray.Count);

            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.AreEqual(2, (int)siteDirectory[PropertyNames.RevisionNumber]);
            var expectedModels = new string[] { "116f6253-89bb-47d4-aa24-d11d197e43c9", "ba097bf8-c916-4134-8471-4a1eb4efb2f7" };
            var modelArray = (JArray)siteDirectory[PropertyNames.Model];
            IList<string> models = modelArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedModels, models);

            var engineeringModelSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "ba097bf8-c916-4134-8471-4a1eb4efb2f7");            
            var participantsArray = (JArray)engineeringModelSetup[PropertyNames.Participant];
            Assert.AreEqual(1, participantsArray.Count); 
            Assert.AreEqual("1f3c2199-2ddf-4a52-a53a-97436a695d35", (string)engineeringModelSetup[PropertyNames.EngineeringModelIid]);
            Assert.AreEqual("integrationtest", (string)engineeringModelSetup[PropertyNames.Name]);
            Assert.AreEqual("STUDY_MODEL", (string)engineeringModelSetup[PropertyNames.Kind]);
            Assert.AreEqual("PREPARATION_PHASE", (string)engineeringModelSetup[PropertyNames.StudyPhase]);

            var iterationSetup = jArray.Single(x => (string)x[PropertyNames.ClassKind] == "IterationSetup");
            Assert.AreEqual("Iteration 1", (string)iterationSetup["description"]);

            var modelReferenceDataLibrary = jArray.Single(x => (string)x[PropertyNames.Iid] == "325a98b0-e8e9-4a7f-a038-98b9b618b705");
            Assert.AreEqual("integrationtest", (string)modelReferenceDataLibrary[PropertyNames.ShortName]);
            Assert.AreEqual("integrationtest Model RDL", (string)modelReferenceDataLibrary[PropertyNames.Name]);
            Assert.AreEqual("c454c687-ba3e-44c4-86bc-44544b2c7880", (string)modelReferenceDataLibrary[PropertyNames.RequiredRdl]);

            var participant = jArray.Single(x => (string)x[PropertyNames.ClassKind] == "Participant");
            Assert.IsTrue((bool)participant[PropertyNames.IsActive]);
            Assert.AreEqual("77791b12-4c2c-4499-93fa-869df3692d22", (string)participant[PropertyNames.Person]);
            var expectedDomains = new string[] { "0e92edde-fdff-41db-9b1d-f2e484f12535" };
            var domainsArray = (JArray)engineeringModelSetup[PropertyNames.Domain];
            IList<string> domains = domainsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDomains, domains);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", participant[PropertyNames.SelectedDomain]);
        }

        /// <summary>
        /// Verification that the EngineeringModelSetup objects are returned from the data-source and that the 
        /// values of the EngineeringModelSetup properties are equal to the expected values
        /// </summary>
        [Test]
        public void VerifyThatExpectedEngineeringModelSetupIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var engineeringModelSetupUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(engineeringModelSetupUri);

            //check if there is only one EngineeringModelSetup object
            Assert.AreEqual(1, jArray.Count);

            // get a specific EngineeringModelSetup from the result by it's unique id
            var engineeringModelSetup = jArray.Single(x => (string) x["iid"] == "116f6253-89bb-47d4-aa24-d11d197e43c9");

            EngineeringModelSetupTestFixture.VerifyProperties(engineeringModelSetup);
        }

        [Test]
        public void VerifyThatExpectedEngineeringModelSetupWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var engineeringModelSetupsUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(engineeringModelSetupsUri);

            //check if there are only two objects
            Assert.AreEqual(2, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific PersonRole from the result by it's unique id
            var personRole = jArray.Single(x => (string) x["iid"] == "116f6253-89bb-47d4-aa24-d11d197e43c9");
            EngineeringModelSetupTestFixture.VerifyProperties(personRole);
        }

        /// <summary>
        /// Verifies all properties of the EngineeringModelSetup <see cref="JToken"/>
        /// </summary>
        /// <param name="engineeringModelSetup">
        /// The <see cref="JToken"/> that contains the properties of
        /// the EngineeringModelSetup object
        /// </param>
        public static void VerifyProperties(JToken engineeringModelSetup)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(16, engineeringModelSetup.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("116f6253-89bb-47d4-aa24-d11d197e43c9", (string) engineeringModelSetup["iid"]);
            Assert.AreEqual(1, (int) engineeringModelSetup["revisionNumber"]);
            Assert.AreEqual("EngineeringModelSetup", (string) engineeringModelSetup["classKind"]);

            Assert.AreEqual("Test Engineering ModelSetup", (string) engineeringModelSetup["name"]);
            Assert.AreEqual("TestEngineeringModelSetup", (string) engineeringModelSetup["shortName"]);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) engineeringModelSetup["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) engineeringModelSetup["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) engineeringModelSetup["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            Assert.IsEmpty(engineeringModelSetup["sourceEngineeringModelSetupIid"]);

            var expectedParticipants = new string[] {"284334dd-e8e5-42d6-bc8a-715c507a7f02"};
            var participantsArray = (JArray) engineeringModelSetup["participant"];
            IList<string> participantsList = participantsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedParticipants, participantsList);

            var expectedActiveDomains = new string[] {"0e92edde-fdff-41db-9b1d-f2e484f12535"};
            var activeDomainsArray = (JArray) engineeringModelSetup["activeDomain"];
            IList<string> activeDomainsList = activeDomainsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedActiveDomains, activeDomainsList);

            Assert.AreEqual("STUDY_MODEL", (string) engineeringModelSetup["kind"]);
            Assert.AreEqual("PREPARATION_PHASE", (string) engineeringModelSetup["studyPhase"]);

            var expectedRequiredRdls = new string[] {"3483f2b5-ea29-45cc-8a46-f5f598558fc3"};
            var requiredRdlsArray = (JArray) engineeringModelSetup["requiredRdl"];
            IList<string> requiredRdlsList = requiredRdlsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedRequiredRdls, requiredRdlsList);

            Assert.AreEqual("9ec982e4-ef72-4953-aa85-b158a95d8d56",
                (string) engineeringModelSetup["engineeringModelIid"]);

            var expectedIterationSetups = new string[] {"86163b0e-8341-4316-94fc-93ed60ad0dcf"};
            var iterationSetupsArray = (JArray) engineeringModelSetup["iterationSetup"];
            IList<string> iterationSetupsList = iterationSetupsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedIterationSetups, iterationSetupsList);
        }
    }
}