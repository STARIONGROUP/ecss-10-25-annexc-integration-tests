// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdCorrespondenceTestFixture.cs" company="Starion Group S.A.">
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
    using System.Linq;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class IdCorrespondenceTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedIdCorrespondenceIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var idCorrespondenceUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/externalIdentifierMap/a0cadcd1-b14f-4552-8f97-bec386a715d0/correspondence");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(idCorrespondenceUri);

            //check if there is the only one IdCorrespondence object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific IdCorrespondence from the result by it's unique id
            var idCorrespondence = jArray.Single(x => (string) x[PropertyNames.Iid] == "ff6956dc-1882-4d61-8840-dedb3fba7b43");

            IdCorrespondenceTestFixture.VerifyProperties(idCorrespondence);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedIdCorrespondenceWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var idCorrespondenceUri =
                new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/externalIdentifierMap/a0cadcd1-b14f-4552-8f97-bec386a715d0/correspondence?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(idCorrespondenceUri);

            //check if there are 4 objects
            Assert.That(jArray.Count, Is.EqualTo(4));

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
            Assert.That(idCorrespondence.Children().Count(), Is.EqualTo(5));

            // assert that the properties are what is expected
            Assert.That((string)idCorrespondence[PropertyNames.Iid], Is.EqualTo("ff6956dc-1882-4d61-8840-dedb3fba7b43"));
            Assert.That((int)idCorrespondence[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)idCorrespondence[PropertyNames.ClassKind], Is.EqualTo("IdCorrespondence"));

            Assert.That((string)idCorrespondence[PropertyNames.ExternalId], Is.EqualTo("internalThing"));
            Assert.That((string) idCorrespondence[PropertyNames.InternalThing], Is.EqualTo("35a8ee03-6786-4c39-967d-3c5b438f0c64"));
        }
    }
}
