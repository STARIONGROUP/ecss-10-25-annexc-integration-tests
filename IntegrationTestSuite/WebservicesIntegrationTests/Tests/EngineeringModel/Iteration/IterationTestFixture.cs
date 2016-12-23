// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IterationTestFixture.cs" company="RHEA System">
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
    public class IterationTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the Iteration objects are returned from the data-source and that the 
        /// values of the Iteration properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedIterationIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var iterationUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(iterationUri);

            //check if there is the only one Iteration object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");

            IterationTestFixture.VerifyProperties(iteration);
        }

        [Test]
        public void VerifyThatExpectedIterationWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var iterationUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(iterationUri);

            //check if there are 2 objects
            Assert.AreEqual(2, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);
        }

        /// <summary>
        /// Verifies all properties of the Iteration <see cref="JToken"/>
        /// </summary>
        /// <param name="iteration">
        /// The <see cref="JToken"/> that contains the properties of
        /// the Iteration object
        /// </param>
        public static void VerifyProperties(JToken iteration)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(17, iteration.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("e163c5ad-f32b-4387-b805-f4b34600bc2c", (string) iteration[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) iteration[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Iteration", (string) iteration[PropertyNames.ClassKind]);

            Assert.AreEqual("86163b0e-8341-4316-94fc-93ed60ad0dcf", (string) iteration[PropertyNames.IterationSetup]);
            Assert.IsNull((string) iteration[PropertyNames.SourceIterationIid]);

            var expectedOptions = new List<OrderedItem>
            {
                new OrderedItem(1, "bebcc9f4-ff20-4569-bbf6-d1acf27a8107")
            };
            var optionsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                iteration[PropertyNames.Option].ToString());
            CollectionAssert.AreEquivalent(expectedOptions, optionsArray);

            var expectedPublications = new string[]
            {
                "790b9e60-476b-4b6d-8aba-0af15178535e"
            };
            var publicationsArray = (JArray) iteration[PropertyNames.Publication];
            IList<string> publications = publicationsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedPublications, publications);

            var expectedPossibleFiniteStateLists = new string[]
            {
                "449a5bca-34fd-454a-93f8-a56ac8383fee"
            };
            var possibleFiniteStateListsArray = (JArray) iteration[PropertyNames.PossibleFiniteStateList];
            IList<string> possibleFiniteStateLists = possibleFiniteStateListsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedPossibleFiniteStateLists, possibleFiniteStateLists);

            Assert.IsNull((string) iteration[PropertyNames.TopElement]);

            var expectedElements = new string[]
            {
                "f73860b2-12f0-43e4-b8b2-c81862c0a159"
            };
            var elementsArray = (JArray) iteration[PropertyNames.Element];
            IList<string> elements = elementsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedElements, elements);

            var expectedRelationships = new string[]
            {
                "320869e4-f6d6-4dd2-a696-1b1604f4c4b7",
                "138f8a3e-69c6-4e21-b459-bc26b1319a2c"
            };
            var relationshipsArray = (JArray) iteration[PropertyNames.Relationship];
            IList<string> relationships = relationshipsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedRelationships, relationships);

            var expectedExternalIdentifierMaps = new string[]
            {
                "a0cadcd1-b14f-4552-8f97-bec386a715d0"
            };
            var externalIdentifierMapsArray = (JArray) iteration[PropertyNames.ExternalIdentifierMap];
            IList<string> externalIdentifierMaps = externalIdentifierMapsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedExternalIdentifierMaps, externalIdentifierMaps);

            var expectedRequirementsSpecifications = new string[] {"bf0cde90-9086-43d5-bcff-32a2f8331800", "8d0734f4-ca4b-4611-9187-f6970e2b02bc" };
            var requirementsSpecificationsArray = (JArray) iteration[PropertyNames.RequirementsSpecification];
            IList<string> requirementsSpecifications = requirementsSpecificationsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedRequirementsSpecifications, requirementsSpecifications);

            var expectedDomainFileStores = new string[]
            {
                "da7dddaa-02aa-4897-9935-e8d66c811a96"
            };
            var domainFileStoresArray = (JArray) iteration[PropertyNames.DomainFileStore];
            IList<string> domainFileStores = domainFileStoresArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDomainFileStores, domainFileStores);

            var expectedActualFiniteStateLists = new string[] { "db690d7d-761c-47fd-96d3-840d698a89dc" };
            var actualFiniteStateListsArray = (JArray) iteration[PropertyNames.ActualFiniteStateList];
            IList<string> actualFiniteStateLists = actualFiniteStateListsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedActualFiniteStateLists, actualFiniteStateLists);

            Assert.AreEqual("bebcc9f4-ff20-4569-bbf6-d1acf27a8107", (string) iteration[PropertyNames.DefaultOption]);

            var expectedRuleVerificationLists = new string[]
            {
                "dc482120-2a11-439b-913d-6a924de9ee5f"
            };
            var ruleVerificationListsArray = (JArray) iteration[PropertyNames.RuleVerificationList];
            IList<string> ruleVerificationLists = ruleVerificationListsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedRuleVerificationLists, ruleVerificationLists);
        }
    }
}