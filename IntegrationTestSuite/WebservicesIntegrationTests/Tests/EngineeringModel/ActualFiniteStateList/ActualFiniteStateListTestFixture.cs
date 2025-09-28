// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActualFiniteStateListTestFixture.cs" company="Starion Group S.A.">
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

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class ActualFiniteStateListTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedActualFiniteStateListIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var actualFiniteStateListUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/actualFiniteStateList");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(actualFiniteStateListUri);

            // check if there is the only one ActualFiniteStateList object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific ActualFiniteStateList from the result by it's unique id
            var actualFiniteStateList = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "db690d7d-761c-47fd-96d3-840d698a89dc");

            VerifyProperties(actualFiniteStateList);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedActualFiniteStateListWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var actualFiniteStateListUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/actualFiniteStateList?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(actualFiniteStateListUri);

            // check if there are 3 objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ActualFiniteStateList from the result by it's unique id
            var actualFiniteStateList = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "db690d7d-761c-47fd-96d3-840d698a89dc");

            VerifyProperties(actualFiniteStateList);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatActualStateCanBeCreatedWhenActualFiniteStateListIsCreatedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postBodyPath = this.GetPath("Tests/EngineeringModel/ActualFiniteStateList/PostNewActualFiniteStateList.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            Assert.That((int)engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            Assert.That((int)iteration[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var expectedActualFiniteStateLists = new[]
            {
                "db690d7d-761c-47fd-96d3-840d698a89dc",
                "f4640b3f-7840-4cfc-9a15-80127f91c8db"
            };

            var actualFiniteStateListsArray = (JArray) iteration[PropertyNames.ActualFiniteStateList];
            IList<string> actualFiniteStateLists = actualFiniteStateListsArray.Select(x => (string) x).ToList();
            Assert.That(actualFiniteStateLists, Is.EquivalentTo(expectedActualFiniteStateLists));

            var actualFiniteStateList = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "f4640b3f-7840-4cfc-9a15-80127f91c8db");

            // verify the amount of returned properties 
            Assert.That(actualFiniteStateList.Children().Count(), Is.EqualTo(7));

            // assert that the properties are what is expected
            Assert.That((string)actualFiniteStateList[PropertyNames.Iid], Is.EqualTo("f4640b3f-7840-4cfc-9a15-80127f91c8db"));
            Assert.That((int)actualFiniteStateList[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string)actualFiniteStateList[PropertyNames.ClassKind], Is.EqualTo("ActualFiniteStateList"));
            Assert.That((string)actualFiniteStateList[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            var expectedPossibleFiniteStateLists =
                new List<OrderedItem> { new OrderedItem(2356, "449a5bca-34fd-454a-93f8-a56ac8383fee") };

            var possibleFiniteStateLists =
                JsonConvert.DeserializeObject<List<OrderedItem>>(
                    actualFiniteStateList[PropertyNames.PossibleFiniteStateList].ToString());

            Assert.That(possibleFiniteStateLists, Is.EquivalentTo(expectedPossibleFiniteStateLists));

            var expectedExcludedOptions = new string[] { };
            var excludedOptionsArray = (JArray) actualFiniteStateList[PropertyNames.ExcludeOption];
            IList<string> excludedOptions = excludedOptionsArray.Select(x => (string) x).ToList();
            Assert.That(excludedOptions, Is.EquivalentTo(expectedExcludedOptions));

            // get the created ActualState as a side effect of creating ActualFiniteStateList from the result 
            var actualStatesArray = (JArray) actualFiniteStateList[PropertyNames.ActualState];
            IList<string> actualStates = actualStatesArray.Select(x => (string) x).ToList();
            Assert.That(actualStates.Count, Is.EqualTo(1));

            // get a specific ActualFiniteStateList from the result 
            var actualFiniteState = jArray.Single(x => (string) x[PropertyNames.Iid] == actualStates[0]);

            // verify the amount of returned properties 
            Assert.That(actualFiniteState.Children().Count(), Is.EqualTo(5));

            // assert that the properties are what is expected
            Assert.That((int)actualFiniteState[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string)actualFiniteState[PropertyNames.ClassKind], Is.EqualTo("ActualFiniteState"));
            Assert.That((string)actualFiniteState[PropertyNames.Kind], Is.EqualTo("MANDATORY"));

            var expectedPossibleStates = new[] { "b8fdfac4-1c40-475a-ac6c-968654b689b6" };
            var possibleStateArray = (JArray) actualFiniteState[PropertyNames.PossibleState];
            IList<string> possibleStates = possibleStateArray.Select(x => (string) x).ToList();
            Assert.That(possibleStates, Is.EquivalentTo(expectedPossibleStates));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatActualStateCanBeCreatedWhenPossibleFiniteStateListIsAddedWithWebApi()
        {
            // POST a new PossibleFiniteStateList
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postBodyPath = this.GetPath("Tests/EngineeringModel/ActualFiniteStateList/PostPossibleFiniteStateList.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            Assert.That((int)engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            Assert.That((int)iteration[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var expectedPossibleFiniteStateLists = new[]
            {
                "449a5bca-34fd-454a-93f8-a56ac8383fee",
                "bf42cb66-f340-4764-ab48-a3938a7c59eb"
            };

            var possibleFiniteStateListsArray = (JArray) iteration[PropertyNames.PossibleFiniteStateList];
            IList<string> possibleFiniteStateLists = possibleFiniteStateListsArray.Select(x => (string) x).ToList();
            Assert.That(possibleFiniteStateLists, Is.EquivalentTo(expectedPossibleFiniteStateLists));

            var possibleFiniteStateList = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "bf42cb66-f340-4764-ab48-a3938a7c59eb");

            Assert.That((int)iteration[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // verify the amount of returned properties 
            Assert.That(possibleFiniteStateList.Children().Count(), Is.EqualTo(12));

            // assert that the properties are what is expected
            Assert.That((string)possibleFiniteStateList[PropertyNames.ClassKind], Is.EqualTo("PossibleFiniteStateList"));

            Assert.That((string)possibleFiniteStateList[PropertyNames.Name], Is.EqualTo("Test 2 Possible FiniteState List"));
            Assert.That((string)possibleFiniteStateList[PropertyNames.ShortName], Is.EqualTo("TestPossibleFiniteStateList2"));

            Assert.That((string) possibleFiniteStateList[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            var expectedPossibleFiniteStates =
                new List<OrderedItem>
                {
                    new OrderedItem(12158, "20f4d6aa-f9d3-4e91-aae9-bfbe77ae9b79"),
                    new OrderedItem(12159, "e6eabd4f-6145-4788-8070-53bd031195bd")
                };

            var possibleFiniteStates =
                JsonConvert.DeserializeObject<List<OrderedItem>>(
                    possibleFiniteStateList[PropertyNames.PossibleState].ToString());

            Assert.That(possibleFiniteStates, Is.EquivalentTo(expectedPossibleFiniteStates));

            Assert.That((string) possibleFiniteStateList[PropertyNames.DefaultState], Is.EqualTo("20f4d6aa-f9d3-4e91-aae9-bfbe77ae9b79"));

            var expectedCategories = new string[] { };
            var categoriesArray = (JArray) possibleFiniteStateList[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            Assert.That(categories, Is.EquivalentTo(expectedCategories));

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray) possibleFiniteStateList[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray) possibleFiniteStateList[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray) possibleFiniteStateList[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));

            // Check PossibleFiniteStates
            var possibleFiniteState = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "20f4d6aa-f9d3-4e91-aae9-bfbe77ae9b79");

            Assert.That((int)iteration[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // verify the amount of returned properties 
            Assert.That(possibleFiniteState.Children().Count(), Is.EqualTo(8));

            // assert that the properties are what is expected
            Assert.That((string)possibleFiniteState[PropertyNames.ClassKind], Is.EqualTo("PossibleFiniteState"));
            Assert.That((string)possibleFiniteState[PropertyNames.Name], Is.EqualTo("On"));
            Assert.That((string)possibleFiniteState[PropertyNames.ShortName], Is.EqualTo("On"));

            expectedAliases = new string[] { };
            aliasesArray = (JArray) possibleFiniteState[PropertyNames.Alias];
            aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            expectedDefinitions = new string[] { };
            definitionsArray = (JArray) possibleFiniteState[PropertyNames.Definition];
            definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            expectedHyperlinks = new string[] { };
            hyperlinksArray = (JArray) possibleFiniteState[PropertyNames.HyperLink];
            h = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));

            possibleFiniteState = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "e6eabd4f-6145-4788-8070-53bd031195bd");

            Assert.That((int)iteration[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // verify the amount of returned properties 
            Assert.That(possibleFiniteState.Children().Count(), Is.EqualTo(8));

            // assert that the properties are what is expected
            Assert.That((string)possibleFiniteState[PropertyNames.ClassKind], Is.EqualTo("PossibleFiniteState"));
            Assert.That((string)possibleFiniteState[PropertyNames.Name], Is.EqualTo("Off"));
            Assert.That((string)possibleFiniteState[PropertyNames.ShortName], Is.EqualTo("Off"));

            expectedAliases = new string[] { };
            aliasesArray = (JArray) possibleFiniteState[PropertyNames.Alias];
            aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            expectedDefinitions = new string[] { };
            definitionsArray = (JArray) possibleFiniteState[PropertyNames.Definition];
            definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            expectedHyperlinks = new string[] { };
            hyperlinksArray = (JArray) possibleFiniteState[PropertyNames.HyperLink];
            h = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));

            // Add a new PossibleFiniteStateList to ActualFiniteStateList
            postBodyPath = this.GetPath("Tests/EngineeringModel/ActualFiniteStateList/PostActualFiniteStateListAddPossibleFiniteStateList.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            engineeeringModel = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            Assert.That((int)engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(3));

            var actualFiniteStateList = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "db690d7d-761c-47fd-96d3-840d698a89dc");

            Assert.That((int)actualFiniteStateList[PropertyNames.RevisionNumber], Is.EqualTo(3));

            var expectedPossibleFiniteStateListsInActualFiniteStateList =
                new List<OrderedItem>
                {
                    new OrderedItem(4598335, "449a5bca-34fd-454a-93f8-a56ac8383fee"),
                    new OrderedItem(4598336, "bf42cb66-f340-4764-ab48-a3938a7c59eb")
                };

            var possibleFiniteStateListsInActualFiniteStateList = JsonConvert.DeserializeObject<List<OrderedItem>>(
                actualFiniteStateList[PropertyNames.PossibleFiniteStateList].ToString());

            Assert.That(
                possibleFiniteStateListsInActualFiniteStateList,
                Is.EquivalentTo(expectedPossibleFiniteStateListsInActualFiniteStateList));

            // get the created ActualState as a side effect of adding PossibleFiniteStateList from the result by it's unique id
            var actualStatesArray = (JArray) actualFiniteStateList[PropertyNames.ActualState];
            IList<string> actualStates = actualStatesArray.Select(x => (string) x).ToList();
            Assert.That(actualStates.Count, Is.EqualTo(2));

            // get a specific ActualFiniteStateList from the result by it's unique id
            var actualFiniteState = jArray.Single(
                x => (string) x[PropertyNames.ClassKind] == "ActualFiniteState"
                     && (string) x[PropertyNames.PossibleState][0] == "20f4d6aa-f9d3-4e91-aae9-bfbe77ae9b79");

            // verify the amount of returned properties 
            Assert.That(actualFiniteState.Children().Count(), Is.EqualTo(5));

            // assert that the properties are what is expected
            Assert.That((int)actualFiniteState[PropertyNames.RevisionNumber], Is.EqualTo(3));
            Assert.That((string)actualFiniteState[PropertyNames.ClassKind], Is.EqualTo("ActualFiniteState"));
            Assert.That((string)actualFiniteState[PropertyNames.Kind], Is.EqualTo("MANDATORY"));

            var expectedPossibleStates = new[]
            {
                "20f4d6aa-f9d3-4e91-aae9-bfbe77ae9b79",
                "b8fdfac4-1c40-475a-ac6c-968654b689b6"
            };

            var possibleStateArray = (JArray) actualFiniteState[PropertyNames.PossibleState];

            IList<string> possibleStates = possibleStateArray.Select(x => (string) x).ToList();
            Assert.That(possibleStates, Is.EqualTo(expectedPossibleStates));

            // get a ActualFiniteStateList from the result by unknown id
            actualFiniteState = jArray.Single(
                x => (string) x[PropertyNames.ClassKind] == "ActualFiniteState"
                     && (string) x[PropertyNames.PossibleState][1] == "e6eabd4f-6145-4788-8070-53bd031195bd");

            // verify the amount of returned properties 
            Assert.That(actualFiniteState.Children().Count(), Is.EqualTo(5));

            // assert that the properties are what is expected
            Assert.That((int)actualFiniteState[PropertyNames.RevisionNumber], Is.EqualTo(3));
            Assert.That((string)actualFiniteState[PropertyNames.ClassKind], Is.EqualTo("ActualFiniteState"));
            Assert.That((string)actualFiniteState[PropertyNames.Kind], Is.EqualTo("MANDATORY"));

            expectedPossibleStates = new[]
            {
                "b8fdfac4-1c40-475a-ac6c-968654b689b6",
                "e6eabd4f-6145-4788-8070-53bd031195bd"
            };

            possibleStateArray = (JArray) actualFiniteState[PropertyNames.PossibleState];
            possibleStates = possibleStateArray.Select(x => (string) x).ToList();
            Assert.That(possibleStates, Is.EqualTo(expectedPossibleStates));

            // Reorder containment
            postBodyPath = this.GetPath("Tests/EngineeringModel/ActualFiniteStateList/PostActualFiniteStateListRearrangePossibleFiniteStateList.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            actualFiniteStateList = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "db690d7d-761c-47fd-96d3-840d698a89dc");

            Assert.That((int)actualFiniteStateList[PropertyNames.RevisionNumber], Is.EqualTo(4));

            var expectedReorderedPossibleFiniteStateListsInActualFiniteStateList =
                new List<OrderedItem>
                {
                    new OrderedItem(4598336, "bf42cb66-f340-4764-ab48-a3938a7c59eb"),
                    new OrderedItem(4598337, "449a5bca-34fd-454a-93f8-a56ac8383fee")
                };

            var reorderedPossibleFiniteStateListsInActualFiniteStateList = JsonConvert.DeserializeObject<List<OrderedItem>>(
                actualFiniteStateList[PropertyNames.PossibleFiniteStateList].ToString());

            Assert.That(
                reorderedPossibleFiniteStateListsInActualFiniteStateList,
                Is.EquivalentTo(expectedReorderedPossibleFiniteStateListsInActualFiniteStateList));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatActualStateCanBeCreatedWhenPossibleFiniteStateIsAddedWithWebApi()
        {
            // POST a new PossibleFiniteState
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postBodyPath = this.GetPath("Tests/EngineeringModel/ActualFiniteStateList/PostNewPossibleFiniteState.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            Assert.That((int)engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var possibleFiniteStateList = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "449a5bca-34fd-454a-93f8-a56ac8383fee");

            Assert.That((int)possibleFiniteStateList[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var expectedPossibleFiniteStates =
                new List<OrderedItem>
                {
                    new OrderedItem(73203278, "b8fdfac4-1c40-475a-ac6c-968654b689b6"),
                    new OrderedItem(73203279, "20f4d6aa-f9d3-4e91-aae9-bfbe77ae9b79")
                };

            var possibleFiniteStates =
                JsonConvert.DeserializeObject<List<OrderedItem>>(
                    possibleFiniteStateList[PropertyNames.PossibleState].ToString());

            Assert.That(possibleFiniteStates, Is.EquivalentTo(expectedPossibleFiniteStates));

            var actualFiniteStateList = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "db690d7d-761c-47fd-96d3-840d698a89dc");

            Assert.That((int)actualFiniteStateList[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // get the created ActualState as a side effect of creating PossibleFiniteState from the result by it's unique id
            var actualStatesArray = (JArray) actualFiniteStateList[PropertyNames.ActualState];
            IList<string> actualStates = actualStatesArray.Select(x => (string) x).ToList();
            Assert.That(actualStates.Count, Is.EqualTo(2));

            // get a specific ActualFiniteStateList from the result by unknown id
            var actualFiniteState1 = jArray.Single(
                x => (string) x[PropertyNames.ClassKind] == "ActualFiniteState"
                     && (string) x[PropertyNames.PossibleState][0] == "20f4d6aa-f9d3-4e91-aae9-bfbe77ae9b79");

            // verify the amount of returned properties 
            Assert.That(actualFiniteState1.Children().Count(), Is.EqualTo(5));

            // assert that the properties are what is expected
            Assert.That((int)actualFiniteState1[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string)actualFiniteState1[PropertyNames.ClassKind], Is.EqualTo("ActualFiniteState"));
            Assert.That((string)actualFiniteState1[PropertyNames.Kind], Is.EqualTo("MANDATORY"));

            var expectedPossibleStates = new[] { "20f4d6aa-f9d3-4e91-aae9-bfbe77ae9b79" };
            var possibleStateArray = (JArray) actualFiniteState1[PropertyNames.PossibleState];
            IList<string> possibleStates = possibleStateArray.Select(x => (string) x).ToList();
            Assert.That(possibleStates, Is.EqualTo(expectedPossibleStates));

            // get a specific ActualFiniteStateList from the result by unknown id
            var actualFiniteState2 = jArray.Single(
                x => (string) x[PropertyNames.ClassKind] == "ActualFiniteState"
                     && (string) x[PropertyNames.PossibleState][0] == "b8fdfac4-1c40-475a-ac6c-968654b689b6");

            // verify the amount of returned properties 
            Assert.That(actualFiniteState2.Children().Count(), Is.EqualTo(5));

            // assert that the properties are what is expected
            Assert.That((int)actualFiniteState2[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string)actualFiniteState2[PropertyNames.ClassKind], Is.EqualTo("ActualFiniteState"));
            Assert.That((string)actualFiniteState2[PropertyNames.Kind], Is.EqualTo("MANDATORY"));

            expectedPossibleStates = new[] { "b8fdfac4-1c40-475a-ac6c-968654b689b6" };
            possibleStateArray = (JArray) actualFiniteState2[PropertyNames.PossibleState];
            possibleStates = possibleStateArray.Select(x => (string) x).ToList();
            Assert.That(possibleStates, Is.EqualTo(expectedPossibleStates));
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
            Assert.That(actualFiniteStateList.Children().Count(), Is.EqualTo(7));

            // assert that the properties are what is expected
            Assert.That((string)actualFiniteStateList[PropertyNames.Iid], Is.EqualTo("db690d7d-761c-47fd-96d3-840d698a89dc"));
            Assert.That((int)actualFiniteStateList[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)actualFiniteStateList[PropertyNames.ClassKind], Is.EqualTo("ActualFiniteStateList"));
            Assert.That((string)actualFiniteStateList[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            var expectedPossibleFiniteStateLists =
                new List<OrderedItem> { new OrderedItem(4598335, "449a5bca-34fd-454a-93f8-a56ac8383fee") };

            var possibleFiniteStateLists =
                JsonConvert.DeserializeObject<List<OrderedItem>>(
                    actualFiniteStateList[PropertyNames.PossibleFiniteStateList].ToString());

            Assert.That(possibleFiniteStateLists, Is.EquivalentTo(expectedPossibleFiniteStateLists));

            var expectedActualStates = new[] { "b91bfdbb-4277-4a03-b519-e4db839ef5d4" };
            var actualStatesArray = (JArray) actualFiniteStateList[PropertyNames.ActualState];
            IList<string> actualStates = actualStatesArray.Select(x => (string) x).ToList();
            Assert.That(actualStates, Is.EquivalentTo(expectedActualStates));

            var expectedExcludedOptions = new string[] { };
            var excludedOptionsArray = (JArray) actualFiniteStateList[PropertyNames.ExcludeOption];
            IList<string> excludedOptions = excludedOptionsArray.Select(x => (string) x).ToList();
            Assert.That(excludedOptions, Is.EquivalentTo(expectedExcludedOptions));
        }

        /// <summary>
        /// Verify that ActualFiniteState can be saved when kind is changed to forbidden
        /// </summary>
        [Test]
        [Category("POST")]
        public void VerifyThatActualFiniteStateCanBeUpdatedWithWebApi()
        {
            // POST a new PossibleFiniteState
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postBodyPath = this.GetPath("Tests/EngineeringModel/ActualFiniteStateList/PostActualFiniteStateListUpdateActualFiniteState.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeringModel = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            Assert.That((int)engineeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var actualFiniteStateList = jArray.Single(
                x => (string) x[PropertyNames.Iid] == "db690d7d-761c-47fd-96d3-840d698a89dc");

            Assert.That((int)actualFiniteStateList[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // get a specific ActualFiniteStateList from the result by unknown id
            var actualFiniteState = jArray.Single(x => (string) x[PropertyNames.ClassKind] == "ActualFiniteState");

            // verify the amount of returned properties
            Assert.That(actualFiniteState.Children().Count(), Is.EqualTo(5));

            // assert that the properties are what is expected
            Assert.That((int)actualFiniteState[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string)actualFiniteState[PropertyNames.ClassKind], Is.EqualTo("ActualFiniteState"));
            Assert.That((string)actualFiniteState[PropertyNames.Kind], Is.EqualTo("FORBIDDEN"));
        }
    }
}
