// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PossibleFiniteStateListTestFixture.cs" company="Starion Group S.A.">
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
    using System.Net;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class PossibleFiniteStateListTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedPossibleFiniteStateListIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var possibleFiniteStateListUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/possibleFiniteStateList");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(possibleFiniteStateListUri);

            //check if there is the only one PossibleFiniteStateList object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific PossibleFiniteStateList from the result by it's unique id
            var possibleFiniteStateList = jArray.Single(x => (string)x[PropertyNames.Iid] == "449a5bca-34fd-454a-93f8-a56ac8383fee");

            PossibleFiniteStateListTestFixture.VerifyProperties(possibleFiniteStateList);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedPossibleFiniteStateListWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var possibleFiniteStateListUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/possibleFiniteStateList?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(possibleFiniteStateListUri);

            //check if there are 3 objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific PossibleFiniteStateList from the result by it's unique id
            var possibleFiniteStateList = jArray.Single(x => (string)x[PropertyNames.Iid] == "449a5bca-34fd-454a-93f8-a56ac8383fee");
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
            Assert.That(possibleFiniteStateList.Children().Count(), Is.EqualTo(12));

            // assert that the properties are what is expected
            Assert.That((string)possibleFiniteStateList[PropertyNames.Iid], Is.EqualTo("449a5bca-34fd-454a-93f8-a56ac8383fee"));
            Assert.That((int)possibleFiniteStateList[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)possibleFiniteStateList[PropertyNames.ClassKind], Is.EqualTo("PossibleFiniteStateList"));

            Assert.That((string)possibleFiniteStateList[PropertyNames.Name], Is.EqualTo("Test Possible FiniteState List"));
            Assert.That((string)possibleFiniteStateList[PropertyNames.ShortName], Is.EqualTo("TestPossibleFiniteStateList"));

            Assert.That((string)possibleFiniteStateList[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            var expectedPossibleFiniteStates = new List<OrderedItem>
            {
                new OrderedItem(73203278, "b8fdfac4-1c40-475a-ac6c-968654b689b6")
            };
            var possibleFiniteStates = JsonConvert.DeserializeObject<List<OrderedItem>>(
                possibleFiniteStateList[PropertyNames.PossibleState].ToString());
            Assert.That(possibleFiniteStates, Is.EquivalentTo(expectedPossibleFiniteStates));

            Assert.That((string)possibleFiniteStateList[PropertyNames.DefaultState], Is.EqualTo("b8fdfac4-1c40-475a-ac6c-968654b689b6"));

            var expectedCategories = new string[] { };
            var categoriesArray = (JArray)possibleFiniteStateList[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string)x).ToList();
            Assert.That(categories, Is.EquivalentTo(expectedCategories));

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)possibleFiniteStateList[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)possibleFiniteStateList[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)possibleFiniteStateList[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatRelationshipAsPropertyDeletionFromIterationCanBeDoneFromWebApi1()
        {
            var uri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath1 = this.GetPath("Tests/EngineeringModel/PossibleFiniteStateList/PostCreatePossibleFiniteState.json");
            var postBody1 = this.GetJsonFromFile(postBodyPath1);

            var jArray1 = this.WebClient.PostDto(uri, postBody1);
            Assert.That(jArray1.Count, Is.EqualTo(6));

            var postBodyPath2 = this.GetPath("Tests/EngineeringModel/PossibleFiniteStateList/PostDeletePossibleFiniteStateAsProperty.json");

            var postBody2 = this.GetJsonFromFile(postBodyPath2);
            var jArray = this.WebClient.PostDto(uri, postBody2);
            this.VerifyDeleteResponse(jArray);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatWrongPartialReorderFails()
        {
            var uri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postBodyPath1 = this.GetPath("Tests/EngineeringModel/PossibleFiniteStateList/PostCreatePossibleFiniteStateListContainingTwoStates.json");
            var postBody1 = this.GetJsonFromFile(postBodyPath1);
            var jArray1 = this.WebClient.PostDto(uri, postBody1);
            Assert.That(jArray1.Count, Is.EqualTo(5));

            uri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath1 = this.GetPath("Tests/EngineeringModel/PossibleFiniteStateList/PostCreatePossibleFiniteStateForWrongReorder.json");
            postBody1 = this.GetJsonFromFile(postBodyPath1);

            jArray1 = this.WebClient.PostDto(uri, postBody1);
            Assert.That(jArray1.Count, Is.EqualTo(3));

            var postBodyPath2 = this.GetPath("Tests/EngineeringModel/PossibleFiniteStateList/PostReorderStatesOfPossibleFiniteStateListWrong.json");

            var postBody2 = this.GetJsonFromFile(postBodyPath2);

            var exception = Assert.Throws<System.Net.WebException>(() => this.WebClient.PostDto(uri, postBody2));
            var response = exception?.Response as HttpWebResponse;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatRelationshipAsPropertyDeletionFromIterationCanBeDoneFromWebApi2()
        {
            var uri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postBodyPath1 = this.GetPath("Tests/EngineeringModel/PossibleFiniteStateList/PostCreatePossibleFiniteState.json");
            var postBody1 = this.GetJsonFromFile(postBodyPath1);

            var jArray1 = this.WebClient.PostDto(uri, postBody1);
            
            Assert.That(jArray1.Count, Is.EqualTo(6));

            var postBodyPath2 = this.GetPath("Tests/EngineeringModel/PossibleFiniteStateList/PostDeletePossibleFiniteState.json");

            var postBody2 = this.GetJsonFromFile(postBodyPath2);
            var jArray = this.WebClient.PostDto(uri, postBody2);
            this.VerifyDeleteResponse(jArray);
        }

        private void VerifyDeleteResponse(JArray jArray)
        {
            Assert.That(jArray.Count, Is.EqualTo(4)); // modification in actual states as well as side-effect

            var model = jArray.Single(x => x["classKind"].ToString() == "EngineeringModel");
            var pfsl = jArray.Single(x => x["classKind"].ToString() == "PossibleFiniteStateList");
            var afsl = jArray.Single(x => x["classKind"].ToString() == "ActualFiniteStateList");
            var afs = jArray.Single(x => x["classKind"].ToString() == "ActualFiniteState");

            Assert.That(model["iid"].ToString(), Is.EqualTo("9ec982e4-ef72-4953-aa85-b158a95d8d56"));
            Assert.That(pfsl["iid"].ToString(), Is.EqualTo("449a5bca-34fd-454a-93f8-a56ac8383fee"));
        }

        [Test]
        [Category("POST")]
        public void Verify_that_Possible_states_in_PossibleFiniteStateList_can_be_reordered()
        {
            var uri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postBodyPath1 = this.GetPath("Tests/EngineeringModel/PossibleFiniteStateList/PostCreatePossibleFiniteStateListContainingTwoStates.json");
            var postBody1 = this.GetJsonFromFile(postBodyPath1);
            var jArray1 = this.WebClient.PostDto(uri, postBody1);
            Assert.That(jArray1.Count, Is.EqualTo(5));

            var postBodyPath2 = this.GetPath("Tests/EngineeringModel/PossibleFiniteStateList/PostReorderStatesOfPossibleFiniteStateList.json");
            var postBody2 = this.GetJsonFromFile(postBodyPath2);
            var jArray2 = this.WebClient.PostDto(uri, postBody2);

            //response should only contain EngineeeringModel, PossibleFiniteStateList and 2 PossibleFiniteState objects
            var engineeringModel = jArray2.Where(x => x["classKind"].ToString() == "EngineeringModel").ToList();
            var possibleFiniteStateList = jArray2.Where(x => x["classKind"].ToString() == "PossibleFiniteStateList").ToList();
            var possibleFiniteStates = jArray2.Where(x => x["classKind"].ToString() == "PossibleFiniteState").ToList();

            Assert.That(1, Is.EqualTo(engineeringModel.Count));
            Assert.That(1, Is.EqualTo(possibleFiniteStateList.Count));
            Assert.That(2, Is.EqualTo(possibleFiniteStates.Count));

            Assert.That(2, Is.EqualTo(possibleFiniteStateList.First()["possibleState"].Count()));

            var firstPossibleState = possibleFiniteStateList.First()["possibleState"].First();
            var secondPossibleState = possibleFiniteStateList.First()["possibleState"].Last();

            Assert.That(13512213, Is.EqualTo((int)firstPossibleState["k"]));
            Assert.That("8ca48538-d39d-4b09-8944-77c34535ce7a", Is.EqualTo((string)firstPossibleState["v"]));

            Assert.That(125842400, Is.EqualTo((int)secondPossibleState["k"]));
            Assert.That("a6f9789d-26a7-45e6-a528-3cbd1fce3880", Is.EqualTo((string)secondPossibleState["v"]));

        }
    }
}
