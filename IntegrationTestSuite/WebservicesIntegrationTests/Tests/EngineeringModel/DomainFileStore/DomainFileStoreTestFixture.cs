// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainFileStoreTestFixture.cs" company="RHEA System">
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
    using Newtonsoft.Json;

    [TestFixture]
    public class DomainFileStoreTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the DomainFileStore objects are returned from the data-source and that the 
        /// values of the DomainFileStore properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedDomainFileStoreIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var domainFileStoreUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(domainFileStoreUri);

            //check if there is the only one DomainFileStore object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific DomainFileStore from the result by it's unique id
            var domainFileStore =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "da7dddaa-02aa-4897-9935-e8d66c811a96");
            DomainFileStoreTestFixture.VerifyProperties(domainFileStore);
        }

        [Test]
        public void VerifyThatExpectedDomainFileStoreWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var domainFileStoreUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(domainFileStoreUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific DomainFileStore from the result by it's unique id
            var domainFileStore =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "da7dddaa-02aa-4897-9935-e8d66c811a96");
            DomainFileStoreTestFixture.VerifyProperties(domainFileStore);
        }

        /// <summary>
        /// Verifies all properties of the DomainFileStore <see cref="JToken"/>
        /// </summary>
        /// <param name="domainFileStore">
        /// The <see cref="JToken"/> that contains the properties of
        /// the DomainFileStore object
        /// </param>
        public static void VerifyProperties(JToken domainFileStore)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(9, domainFileStore.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("da7dddaa-02aa-4897-9935-e8d66c811a96", (string) domainFileStore[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) domainFileStore[PropertyNames.RevisionNumber]);
            Assert.AreEqual("DomainFileStore", (string) domainFileStore[PropertyNames.ClassKind]);

            Assert.AreEqual("Test DomainFileStore", (string) domainFileStore[PropertyNames.Name]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) domainFileStore[PropertyNames.Owner]);

            Assert.AreEqual("2016-11-02T13:58:35.936Z", (string) domainFileStore[PropertyNames.CreatedOn]);
            Assert.IsFalse((bool) domainFileStore[PropertyNames.IsHidden]);

            var expectedFiles = new string[]
            {
                "95bf0f17-1273-4338-98ae-839016242775"
            };
            var filesArray = (JArray) domainFileStore[PropertyNames.File];
            IList<string> files = filesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedFiles, files);

            var expectedFolders = new string[]
            {
                "67cdb7de-7721-40a0-9ca2-10a5cf7742fc"
            };
            var foldersArray = (JArray) domainFileStore[PropertyNames.Folder];
            IList<string> folders = foldersArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedFolders, folders);
        }

        [Test]
        public void VerifyThatDomainFileStoreWillNotBeDeletedWhenDomainOfExpertiseWillBeRemovedFromExistingEngineeringModelSetup()
        {
            var engineeringModelSetupsUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model"));

            var jArray = this.WebClient.GetDto(engineeringModelSetupsUri);
            Assert.AreEqual(1, jArray.Count);
            var engineeringModelSetup = jArray.Single(x => (string) x[PropertyNames.Iid] == "116f6253-89bb-47d4-aa24-d11d197e43c9");
            Assert.AreEqual("116f6253-89bb-47d4-aa24-d11d197e43c9", (string) engineeringModelSetup[PropertyNames.Iid]);
            var model = (string) engineeringModelSetup[PropertyNames.EngineeringModelIid];
            Assert.AreEqual("9ec982e4-ef72-4953-aa85-b158a95d8d56", model);

            // Check DomainOfExpertise in EngineeringModelSetup
            var expectedActiveDomains = new string[]
            {
                "0e92edde-fdff-41db-9b1d-f2e484f12535",
                "eb759723-14b9-49f4-8611-544d037bb764"
            };

            var activeDomainsArray = (JArray) engineeringModelSetup[PropertyNames.ActiveDomain];
            IList<string> activeDomainsList = activeDomainsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedActiveDomains, activeDomainsList);

            // Check DomainFileStore in EngineeringModel correlated to EngineeringModelSetup
            var engineeringModelUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/" + model));

            jArray = this.WebClient.GetDto(engineeringModelUri);
            var engineeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == model.ToString());
            var iterationsArray = (JArray) engineeringModel[PropertyNames.Iteration];

            var iterationUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/" + iterationsArray[0].ToString()));

            jArray = this.WebClient.GetDto(iterationUri);
            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == iterationsArray[0].ToString());
            var domainFileStoresArray = (JArray) iteration[PropertyNames.DomainFileStore];
            Assert.AreEqual(1, domainFileStoresArray.Count);

            // Check DomainFileStore after delete DomainOfExpertise in EngineeringModelSetup
            var siteDirectoryUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));

            var postBodyPath = this.GetPath("Tests/EngineeringModel/DomainFileStore/PostDeleteDomainOfExpertise.json");
            var postBody = base.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            engineeringModelSetupsUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model/116f6253-89bb-47d4-aa24-d11d197e43c9"));

            jArray = this.WebClient.GetDto(engineeringModelSetupsUri);
            engineeringModelSetup = jArray.Single(x => (string) x[PropertyNames.Iid] == "116f6253-89bb-47d4-aa24-d11d197e43c9");
            expectedActiveDomains = new string[] { };
            activeDomainsArray = (JArray) engineeringModelSetup[PropertyNames.ActiveDomain];
            activeDomainsList = activeDomainsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedActiveDomains, activeDomainsList);

            jArray = this.WebClient.GetDto(iterationUri);
            iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == iterationsArray[0].ToString());
            domainFileStoresArray = (JArray) iteration[PropertyNames.DomainFileStore];
            Assert.AreEqual(1, domainFileStoresArray.Count);

            // Check DomainFileStore after adding DomainOfExpertise in EngineeringModelSetup
            siteDirectoryUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));

            postBodyPath = this.GetPath("Tests/EngineeringModel/DomainFileStore/PostAddDomainOfExpertise.json");
            postBody = base.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            jArray = this.WebClient.GetDto(iterationUri);
            iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == iterationsArray[0].ToString());
            domainFileStoresArray = (JArray) iteration[PropertyNames.DomainFileStore];
            Assert.AreEqual(1, domainFileStoresArray.Count);
        }
    }
}