// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PossibleFiniteStateListTestFixture.cs" company="RHEA System">
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
    public class PossibleFiniteStateListTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the PossibleFiniteStateList objects are returned from the data-source and that the 
        /// values of the PossibleFiniteStateList properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedPossibleFiniteStateListIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var possibleFiniteStateListUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/possibleFiniteStateList"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(possibleFiniteStateListUri);

            //check if there is the only one PossibleFiniteStateList object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific PossibleFiniteStateList from the result by it's unique id
            var possibleFiniteStateList =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "449a5bca-34fd-454a-93f8-a56ac8383fee");

            PossibleFiniteStateListTestFixture.VerifyProperties(possibleFiniteStateList);
        }

        [Test]
        public void VerifyThatExpectedPossibleFiniteStateListWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var possibleFiniteStateListUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/possibleFiniteStateList?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(possibleFiniteStateListUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific PossibleFiniteStateList from the result by it's unique id
            var possibleFiniteStateList =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "449a5bca-34fd-454a-93f8-a56ac8383fee");
            PossibleFiniteStateListTestFixture.VerifyProperties(possibleFiniteStateList);
        }

        /// <summary>
        /// Verifies all properties of the PossibleFiniteStateList <see cref="JToken"/>
        /// </summary>
        /// <param name="possibleFiniteStateList">
        /// The <see cref="JToken"/> that contains the properties of
        /// the PossibleFiniteStateList object
        /// </param>
        public static void VerifyProperties(JToken possibleFiniteStateList)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(12, possibleFiniteStateList.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("449a5bca-34fd-454a-93f8-a56ac8383fee", (string)possibleFiniteStateList[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)possibleFiniteStateList[PropertyNames.RevisionNumber]);
            Assert.AreEqual("PossibleFiniteStateList", (string)possibleFiniteStateList[PropertyNames.ClassKind]);

            Assert.AreEqual("Test Possible FiniteState List", (string)possibleFiniteStateList[PropertyNames.Name]);
            Assert.AreEqual("TestPossibleFiniteStateList", (string)possibleFiniteStateList[PropertyNames.ShortName]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)possibleFiniteStateList[PropertyNames.Owner]);

            var expectedPossibleFiniteStates = new List<OrderedItem>
            {
                new OrderedItem(73203278, "b8fdfac4-1c40-475a-ac6c-968654b689b6")
            };
            var possibleFiniteStates = JsonConvert.DeserializeObject<List<OrderedItem>>(
                possibleFiniteStateList[PropertyNames.PossibleState].ToString());
            CollectionAssert.AreEquivalent(expectedPossibleFiniteStates, possibleFiniteStates);

            Assert.AreEqual("b8fdfac4-1c40-475a-ac6c-968654b689b6", (string)possibleFiniteStateList[PropertyNames.DefaultState]);

            var expectedCategories = new string[] { };
            var categoriesArray = (JArray)possibleFiniteStateList[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)possibleFiniteStateList[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)possibleFiniteStateList[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)possibleFiniteStateList[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}