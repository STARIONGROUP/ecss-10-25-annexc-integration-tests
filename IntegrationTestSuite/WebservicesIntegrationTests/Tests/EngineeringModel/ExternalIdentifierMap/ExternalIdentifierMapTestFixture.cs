﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExternalIdentifierMapTestFixture.cs" company="Starion Group S.A.">
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
    public class ExternalIdentifierMapTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedExternalIdentifierMapIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var externalIdentifierMapUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/externalIdentifierMap");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(externalIdentifierMapUri);

            //check if there is the only one ExternalIdentifierMap object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ExternalIdentifierMap from the result by it's unique id
            var externalIdentifierMap =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "a0cadcd1-b14f-4552-8f97-bec386a715d0");

            ExternalIdentifierMapTestFixture.VerifyProperties(externalIdentifierMap);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedExternalIdentifierMapWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var externalIdentifierMapUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/externalIdentifierMap?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(externalIdentifierMapUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ExternalIdentifierMap from the result by it's unique id
            var externalIdentifierMap =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "a0cadcd1-b14f-4552-8f97-bec386a715d0");
            ExternalIdentifierMapTestFixture.VerifyProperties(externalIdentifierMap);
        }

        /// <summary>
        /// Verifies all properties of the ExternalIdentifierMap <see cref="JToken"/>
        /// </summary>
        /// <param name="externalIdentifierMap">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ExternalIdentifierMap object
        /// </param>
        public static void VerifyProperties(JToken externalIdentifierMap)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(10, externalIdentifierMap.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("a0cadcd1-b14f-4552-8f97-bec386a715d0", (string) externalIdentifierMap[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) externalIdentifierMap[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ExternalIdentifierMap", (string) externalIdentifierMap[PropertyNames.ClassKind]);

            Assert.AreEqual("TestExternalIdentifierMap", (string) externalIdentifierMap[PropertyNames.Name]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) externalIdentifierMap[PropertyNames.Owner]);

            Assert.IsNull((string) externalIdentifierMap[PropertyNames.ExternalFormat]);
            Assert.AreEqual("1.0", (string) externalIdentifierMap[PropertyNames.ExternalToolVersion]);
            Assert.AreEqual("externalTool", (string) externalIdentifierMap[PropertyNames.ExternalToolName]);
            Assert.AreEqual("externalModel", (string) externalIdentifierMap[PropertyNames.ExternalModelName]);

            var expectedCorrespondence = new string[]
            {
                "ff6956dc-1882-4d61-8840-dedb3fba7b43"
            };
            var correspondenceArray = (JArray) externalIdentifierMap[PropertyNames.Correspondence];
            IList<string> correspondence = correspondenceArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCorrespondence, correspondence);
        }
    }
}
