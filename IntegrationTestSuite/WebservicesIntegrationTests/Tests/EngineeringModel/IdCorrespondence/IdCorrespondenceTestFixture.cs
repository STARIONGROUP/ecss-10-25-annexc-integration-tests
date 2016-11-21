// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdCorrespondenceTestFixture.cs" company="RHEA System">
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
    using System.Linq;
    using NUnit.Framework;
    using Newtonsoft.Json.Linq;

    [TestFixture]
    public class IdCorrespondenceTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the IdCorrespondence objects are returned from the data-source and that the 
        /// values of the IdCorrespondence properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedIdCorrespondenceIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var idCorrespondenceUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/externalIdentifierMap/a0cadcd1-b14f-4552-8f97-bec386a715d0/correspondence"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(idCorrespondenceUri);

            //check if there is the only one IdCorrespondence object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific IdCorrespondence from the result by it's unique id
            var idCorrespondence =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "ff6956dc-1882-4d61-8840-dedb3fba7b43");

            IdCorrespondenceTestFixture.VerifyProperties(idCorrespondence);
        }

        [Test]
        public void VerifyThatExpectedIdCorrespondenceWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var idCorrespondenceUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/externalIdentifierMap/a0cadcd1-b14f-4552-8f97-bec386a715d0/correspondence?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(idCorrespondenceUri);

            //check if there are 4 objects
            Assert.AreEqual(4, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ExternalIdentifierMap from the result by it's unique id
            var externalIdentifierMap =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "a0cadcd1-b14f-4552-8f97-bec386a715d0");
            ExternalIdentifierMapTestFixture.VerifyProperties(externalIdentifierMap);

            // get a specific IdCorrespondence from the result by it's unique id
            var idCorrespondence =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "ff6956dc-1882-4d61-8840-dedb3fba7b43");
            IdCorrespondenceTestFixture.VerifyProperties(idCorrespondence);
        }

        /// <summary>
        /// Verifies all properties of the IdCorrespondence <see cref="JToken"/>
        /// </summary>
        /// <param name="idCorrespondence">
        /// The <see cref="JToken"/> that contains the properties of
        /// the IdCorrespondence object
        /// </param>
        public static void VerifyProperties(JToken idCorrespondence)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(5, idCorrespondence.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("ff6956dc-1882-4d61-8840-dedb3fba7b43", (string) idCorrespondence[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) idCorrespondence[PropertyNames.RevisionNumber]);
            Assert.AreEqual("IdCorrespondence", (string) idCorrespondence[PropertyNames.ClassKind]);

            Assert.AreEqual("internalThing", (string) idCorrespondence[PropertyNames.ExternalId]);
            Assert.AreEqual("35a8ee03-6786-4c39-967d-3c5b438f0c64",
                (string) idCorrespondence[PropertyNames.InternalThing]);
        }
    }
}