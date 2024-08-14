// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EngineeringModelTestFixture.cs" company="Starion Group S.A.">
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
    using System.ComponentModel;
    using System.Linq;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class EngineeringModelTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [NUnit.Framework.Category("GET")]
        public void VerifyThatAllEngineeringModelsAreReturnedFromWebApiAfterCreate()
        {
            // define the URI on which to perform a GET request 
            var engineeringModelUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/*");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(engineeringModelUri);

            // check if there are the only one EngineeringModel object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            EngineeringModelTestFixture.VerifyProperties(jArray);
            
            var postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetup.json");
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");

            var postBody = base.GetJsonFromFile(postBodyPath);
            
            _ = this.WebClient.PostDto(siteDirectoryUri, postBody);

            // get a response from the data-source as a JArray (JSON Array)
            jArray = this.WebClient.GetDto(engineeringModelUri);

            // check if there are 2 objects
            Assert.That(jArray.Count, Is.EqualTo(2));
        }

        [Test]
        [NUnit.Framework.Category("GET")]
        public void VerifyThatAllEngineeringModelsAreReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var engineeringModelUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/*");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(engineeringModelUri);

            Assert.That(jArray.Count, Is.EqualTo(1));

            EngineeringModelTestFixture.VerifyProperties(jArray);
        }

        [Test]
        [NUnit.Framework.Category("GET")]
        public void VerifyThatExpectedEngineeringModelsAreReturnedFromWebApi()
        {
            var postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetup.json");
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");

            var postBody = base.GetJsonFromFile(postBodyPath);

            _ = this.WebClient.PostDto(siteDirectoryUri, postBody);

            // define the URI on which to perform a GET request 
            var engineeringModelUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/*");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(engineeringModelUri);

            // check if there are 2 objects
            Assert.That(jArray.Count, Is.EqualTo(2));

            // re define the URI on which to perform a GET request
            engineeringModelUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/[5ILJnnLvU0mqhbFYqV2NVg]");

            // get a response from the data-source as a JArray (JSON Array)
            jArray = this.WebClient.GetDto(engineeringModelUri);

            // check if there are 1 object
            Assert.That(jArray.Count, Is.EqualTo(1));
            
            EngineeringModelTestFixture.VerifyProperties(jArray);

            // re define the URI on which to perform a GET request
            engineeringModelUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/[5ILJnnLvU0mqhbFYqV2NVg;mSE8H98tUkqlOpdDamldNQ]");

            // get a response from the data-source as a JArray (JSON Array)
            jArray = this.WebClient.GetDto(engineeringModelUri);

            // check if there are 2 objects
            Assert.That(jArray.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Verifies all properties of the EngineeringModel <see cref="JToken"/>
        /// </summary>
        /// <param name="jArray">
        /// The JSON array.
        /// </param>
        public static void VerifyProperties(JArray jArray)
        {
            // get a specific Requirement from the result by it's unique id
            var engineeringModel = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            // verify the amount of returned properties 
            Assert.AreEqual(8, engineeringModel.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("9ec982e4-ef72-4953-aa85-b158a95d8d56", (string)engineeringModel[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)engineeringModel[PropertyNames.RevisionNumber]);
            Assert.AreEqual("EngineeringModel", (string)engineeringModel[PropertyNames.ClassKind]);

            Assert.AreEqual("116f6253-89bb-47d4-aa24-d11d197e43c9", (string)engineeringModel[PropertyNames.EngineeringModelSetup]);
            Assert.AreEqual("2016-09-15T08:00:00.079Z", (string)engineeringModel[PropertyNames.LastModifiedOn]);
            
            var expectedCommonFileStores = new string[] { "8e5ca9cc-3da8-4e66-9172-7c3b2464a59c" };
            var commonFileStoreArray = (JArray)engineeringModel[PropertyNames.CommonFileStore];
            IList<string> commonFileStores = commonFileStoreArray.Select(x => (string)x).ToList();
            Assert.That(commonFileStores, Is.EquivalentTo(expectedCommonFileStores));

            var expectedIterations = new string[] { "e163c5ad-f32b-4387-b805-f4b34600bc2c" };
            var iterationArray = (JArray)engineeringModel[PropertyNames.Iteration];
            IList<string> iterations = iterationArray.Select(x => (string)x).ToList();
            Assert.That(iterations, Is.EquivalentTo(expectedIterations));

            var expectedLogEntries = new string[] { "4e2375eb-8e37-4df2-9c7b-dd896683a891" };
            var logEntryArray = (JArray)engineeringModel[PropertyNames.LogEntry];
            IList<string> logEntries = logEntryArray.Select(x => (string)x).ToList();
            Assert.That(logEntries, Is.EquivalentTo(expectedLogEntries));
        }
    }
}
