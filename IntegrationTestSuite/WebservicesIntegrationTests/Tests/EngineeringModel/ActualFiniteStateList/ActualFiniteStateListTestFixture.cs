// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActualFiniteStateListTestFixture.cs" company="RHEA System">
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
    public class ActualFiniteStateListTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the ActualFiniteStateList objects are returned from the data-source and that the 
        /// values of the ActualFiniteStateList properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedActualFiniteStateListIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var actualFiniteStateListUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/actualFiniteStateList"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(actualFiniteStateListUri);

            //check if there is the only one ActualFiniteStateList object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ActualFiniteStateList from the result by it's unique id
            var actualFiniteStateList = jArray.Single(x => (string)x[PropertyNames.Iid] == "db690d7d-761c-47fd-96d3-840d698a89dc");

            ActualFiniteStateListTestFixture.VerifyProperties(actualFiniteStateList);
        }

        [Test]
        public void VerifyThatExpectedActualFiniteStateListWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var actualFiniteStateListUri = new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/actualFiniteStateList?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(actualFiniteStateListUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ActualFiniteStateList from the result by it's unique id
            var actualFiniteStateList =  jArray.Single(x => (string)x[PropertyNames.Iid] == "db690d7d-761c-47fd-96d3-840d698a89dc");

            ActualFiniteStateListTestFixture.VerifyProperties(actualFiniteStateList);
        }

        /// <summary>
        /// Verifies all properties of the ActualFiniteStateList <see cref="JToken"/>
        /// </summary>
        /// <param name="actualFiniteStateList">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ActualFiniteStateList object
        /// </param>
        public static void VerifyProperties(JToken actualFiniteStateList)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(7, actualFiniteStateList.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("db690d7d-761c-47fd-96d3-840d698a89dc", (string)actualFiniteStateList[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)actualFiniteStateList[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ActualFiniteStateList", (string)actualFiniteStateList[PropertyNames.ClassKind]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)actualFiniteStateList[PropertyNames.Owner]);

            var expectedPossibleFiniteStateLists = new List<OrderedItem>
            {
                new OrderedItem(4598335, "449a5bca-34fd-454a-93f8-a56ac8383fee")
            };

            var possibleFiniteStateLists = JsonConvert.DeserializeObject<List<OrderedItem>>(actualFiniteStateList[PropertyNames.PossibleFiniteStateList].ToString());
            CollectionAssert.AreEquivalent(expectedPossibleFiniteStateLists, possibleFiniteStateLists);

            var expectedActualStates = new string[] { "b91bfdbb-4277-4a03-b519-e4db839ef5d4" };
            var actualStatesArray = (JArray)actualFiniteStateList[PropertyNames.ActualState];
            IList<string> actualStates = actualStatesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedActualStates, actualStates);

            var expectedExcludedOptions = new string[] { };
            var excludedOptionsArray = (JArray)actualFiniteStateList[PropertyNames.ExcludeOption];
            IList<string> excludedOptions = excludedOptionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedExcludedOptions, excludedOptions);
        }
    }
}