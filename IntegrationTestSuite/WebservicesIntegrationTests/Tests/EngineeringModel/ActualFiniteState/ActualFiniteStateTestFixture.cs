// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActualFiniteStateTestFixture.cs" company="Starion Group S.A.">
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
    public class ActualFiniteStateTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedActualFiniteStateIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var actualFiniteStateUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/actualFiniteStateList/db690d7d-761c-47fd-96d3-840d698a89dc/actualState");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(actualFiniteStateUri);

            //check if there is the only one ActualFiniteStateList object 
            Assert.That (jArray.Count, Is.EqualTo(1));

            // get a specific ActualFiniteStateList from the result by it's unique id
            var actualFiniteState = jArray.Single(x => (string)x[PropertyNames.Iid] == "b91bfdbb-4277-4a03-b519-e4db839ef5d4");

            ActualFiniteStateTestFixture.VerifyProperties(actualFiniteState);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedActualFiniteStateWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var actualFiniteStateUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/actualFiniteStateList/db690d7d-761c-47fd-96d3-840d698a89dc/actualState?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(actualFiniteStateUri);

            // check if there are 4 objects
            Assert.AreEqual(4, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ActualFiniteStateList from the result by it's unique id
            var actualFiniteStateList = jArray.Single(x => (string)x[PropertyNames.Iid] == "db690d7d-761c-47fd-96d3-840d698a89dc");
            ActualFiniteStateListTestFixture.VerifyProperties(actualFiniteStateList);

            // get a specific ActualFiniteState from the result by it's unique id
            var actualFiniteState = jArray.Single(x => (string)x[PropertyNames.Iid] == "b91bfdbb-4277-4a03-b519-e4db839ef5d4");

            ActualFiniteStateTestFixture.VerifyProperties(actualFiniteState);
        }

        /// <summary>
        /// Verifies all properties of the ActualFiniteState <see cref="JToken"/>
        /// </summary>
        /// <param name="actualFiniteState">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ActualFiniteState object
        /// </param>
        public static void VerifyProperties(JToken actualFiniteState)
        {
            // verify the amount of returned properties 
            Assert.That(actualFiniteState.Children().Count(), Is.EqualTo(5));

            // assert that the properties are what is expected
            Assert.That((string)actualFiniteState[PropertyNames.Iid], Is.EqualTo("b91bfdbb-4277-4a03-b519-e4db839ef5d4"));
            Assert.That((int)actualFiniteState[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)actualFiniteState[PropertyNames.ClassKind], Is.EqualTo("ActualFiniteState"));
            Assert.That((string)actualFiniteState[PropertyNames.Kind], Is.EqualTo("MANDATORY"));

            var expectedPossibleStates = new string[] { "b8fdfac4-1c40-475a-ac6c-968654b689b6" };
            var possibleStateArray = (JArray)actualFiniteState[PropertyNames.PossibleState];
            IList<string> possibleStates = possibleStateArray.Select(x => (string)x).ToList();
            Assert.That(possibleStates, Is.EquivalentTo(expectedPossibleStates));
        }
    }
}
