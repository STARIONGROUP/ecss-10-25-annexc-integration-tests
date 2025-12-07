// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PossibleFiniteStateTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2025 Starion Group S.A.
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
    public class PossibleFiniteStateTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedPossibleFiniteStateIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var possibleFiniteStateUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/possibleFiniteStateList/449a5bca-34fd-454a-93f8-a56ac8383fee/possibleState");
           
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(possibleFiniteStateUri);

            //check if there is the only one PossibleFiniteStateList object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific PossibleFiniteStateList from the result by it's unique id
            var possibleFiniteState = jArray.Single(x => (string)x[PropertyNames.Iid] == "b8fdfac4-1c40-475a-ac6c-968654b689b6");

            PossibleFiniteStateTestFixture.VerifyProperties(possibleFiniteState);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedPossibleFiniteStateWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var possibleFiniteStateUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/possibleFiniteStateList/449a5bca-34fd-454a-93f8-a56ac8383fee/possibleState?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(possibleFiniteStateUri);

            // check if there are 4 objects
            Assert.That(jArray.Count, Is.EqualTo(4));

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific PossibleFiniteStateList from the result by it's unique id
            var possibleFiniteStateList = jArray.Single(x => (string)x[PropertyNames.Iid] == "449a5bca-34fd-454a-93f8-a56ac8383fee");
            PossibleFiniteStateListTestFixture.VerifyProperties(possibleFiniteStateList);

            // get a specific PossibleFiniteState from the result by it's unique id
            var possibleFiniteState = jArray.Single(x => (string)x[PropertyNames.Iid] == "b8fdfac4-1c40-475a-ac6c-968654b689b6");

            PossibleFiniteStateTestFixture.VerifyProperties(possibleFiniteState);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatTheLastPossibleFiniteStateCannotBeDeletedWithWebApi()
        {
            // define the URI on which to perform a GET request
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/PossibleFiniteState/PostDeletePossibleFiniteState.json");
            var postBody = base.GetJsonFromFile(postBodyPath);

            Assert.That(() => WebClient.PostDto(iterationUri, postBody), Throws.Exception.TypeOf<System.Net.WebException>());
        }

        /// <summary>
        /// Verifies all properties of the PossibleFiniteState <see cref="JToken"/>
        /// </summary>
        /// <param name="possibleFiniteState">
        /// The <see cref="JToken"/> that contains the properties of
        /// the PossibleFiniteState object
        /// </param>
        public static void VerifyProperties(JToken possibleFiniteState)
        {
            // verify the amount of returned properties 
            Assert.That(possibleFiniteState.Children().Count(), Is.EqualTo(8));

            // assert that the properties are what is expected
            Assert.That((string)possibleFiniteState[PropertyNames.Iid], Is.EqualTo("b8fdfac4-1c40-475a-ac6c-968654b689b6"));
            Assert.That((int)possibleFiniteState[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)possibleFiniteState[PropertyNames.ClassKind], Is.EqualTo("PossibleFiniteState"));
            Assert.That((string)possibleFiniteState[PropertyNames.Name], Is.EqualTo("Test Possible Finite State"));
            Assert.That((string)possibleFiniteState[PropertyNames.ShortName], Is.EqualTo("TestPossibleFiniteState"));

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)possibleFiniteState[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)possibleFiniteState[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)possibleFiniteState[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));
        }
    }
}
