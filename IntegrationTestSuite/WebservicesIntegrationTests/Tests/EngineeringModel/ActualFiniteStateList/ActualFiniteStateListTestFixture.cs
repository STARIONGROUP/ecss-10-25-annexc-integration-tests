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

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class ActualFiniteStateListTestFixture : WebClientTestFixtureBase
    {
        public override void SetUp()
        {
            base.SetUp();

            this.WebClient.Restore(this.Settings.Hostname);
        }

        public override void TearDown()
        {
            this.WebClient.Restore(this.Settings.Hostname);

            base.TearDown();
        }

        /// <summary>
        /// Verification that the ActualFiniteStateList objects are returned from the data-source and that the 
        /// values of the ActualFiniteStateList properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedActualFiniteStateListIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var actualFiniteStateListUri = new Uri(
                string.Format(
                    UriFormat,
                    this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/actualFiniteStateList"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(actualFiniteStateListUri);

            // check if there is the only one ActualFiniteStateList object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ActualFiniteStateList from the result by it's unique id
            var actualFiniteStateList = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "db690d7d-761c-47fd-96d3-840d698a89dc");

            ActualFiniteStateListTestFixture.VerifyProperties(actualFiniteStateList);
        }

        [Test]
        public void VerifyThatExpectedActualFiniteStateListWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var actualFiniteStateListUri = new Uri(
                string.Format(
                    UriFormat,
                    this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/actualFiniteStateList?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(actualFiniteStateListUri);

            // check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ActualFiniteStateList from the result by it's unique id
            var actualFiniteStateList = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "db690d7d-761c-47fd-96d3-840d698a89dc");

            ActualFiniteStateListTestFixture.VerifyProperties(actualFiniteStateList);
        }

        [Test]
        public void VerifyThatActualStateCanBeCreatedWhenActualFiniteStateListIsCreatedWithWebApi()
        {
            var iterationUri = new Uri(
                string.Format(
                    UriFormat,
                    this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath = this.GetPath(
                "Tests/EngineeringModel/ActualFiniteStateList/PostNewActualFiniteStateList.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int)engineeeringModel[PropertyNames.RevisionNumber]);

            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            Assert.AreEqual(2, (int)iteration[PropertyNames.RevisionNumber]);

            var expectedActualFiniteStateLists = new[]
                                                     {
                                                         "db690d7d-761c-47fd-96d3-840d698a89dc",
                                                         "f4640b3f-7840-4cfc-9a15-80127f91c8db"
                                                     };
            var actualFiniteStateListsArray = (JArray)iteration[PropertyNames.ActualFiniteStateList];
            IList<string> actualFiniteStateLists = actualFiniteStateListsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedActualFiniteStateLists, actualFiniteStateLists);

            var actualFiniteStateList = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "f4640b3f-7840-4cfc-9a15-80127f91c8db");

            // verify the amount of returned properties 
            Assert.AreEqual(7, actualFiniteStateList.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("f4640b3f-7840-4cfc-9a15-80127f91c8db", (string)actualFiniteStateList[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)actualFiniteStateList[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ActualFiniteStateList", (string)actualFiniteStateList[PropertyNames.ClassKind]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)actualFiniteStateList[PropertyNames.Owner]);

            var expectedPossibleFiniteStateLists =
                new List<OrderedItem> { new OrderedItem(2356, "449a5bca-34fd-454a-93f8-a56ac8383fee") };

            var possibleFiniteStateLists =
                JsonConvert.DeserializeObject<List<OrderedItem>>(
                    actualFiniteStateList[PropertyNames.PossibleFiniteStateList].ToString());
            CollectionAssert.AreEquivalent(expectedPossibleFiniteStateLists, possibleFiniteStateLists);

            var expectedExcludedOptions = new string[] { };
            var excludedOptionsArray = (JArray)actualFiniteStateList[PropertyNames.ExcludeOption];
            IList<string> excludedOptions = excludedOptionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedExcludedOptions, excludedOptions);

            // get the created ActualState as a side effect of creating ActualFiniteStateList from the result 
            var actualStatesArray = (JArray)actualFiniteStateList[PropertyNames.ActualState];
            IList<string> actualStates = actualStatesArray.Select(x => (string)x).ToList();
            Assert.AreEqual(1, actualStates.Count);

            // get a specific ActualFiniteStateList from the result 
            var actualFiniteState = jArray.Single(x => (string)x[PropertyNames.Iid] == actualStates[0]);

            // verify the amount of returned properties 
            Assert.AreEqual(5, actualFiniteState.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual(2, (int)actualFiniteState[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ActualFiniteState", (string)actualFiniteState[PropertyNames.ClassKind]);
            Assert.AreEqual("MANDATORY", (string)actualFiniteState[PropertyNames.Kind]);

            var expectedPossibleStates = new[] { "b8fdfac4-1c40-475a-ac6c-968654b689b6" };
            var possibleStateArray = (JArray)actualFiniteState[PropertyNames.PossibleState];
            IList<string> possibleStates = possibleStateArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPossibleStates, possibleStates);
        }

        [Test]
        public void VerifyThatActualStateCanBeCreatedWhenPossibleFiniteStateListIsAddedWithWebApi()
        {
            // POST a new PossibleFiniteStateList
            var iterationUri = new Uri(
                string.Format(
                    UriFormat,
                    this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath = this.GetPath(
                "Tests/EngineeringModel/ActualFiniteStateList/PostPossibleFiniteStateList.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int)engineeeringModel[PropertyNames.RevisionNumber]);

            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            Assert.AreEqual(2, (int)iteration[PropertyNames.RevisionNumber]);

            var expectedPossibleFiniteStateLists = new[]
                                                       {
                                                           "449a5bca-34fd-454a-93f8-a56ac8383fee",
                                                           "bf42cb66-f340-4764-ab48-a3938a7c59eb"
                                                       };
            var possibleFiniteStateListsArray = (JArray)iteration[PropertyNames.PossibleFiniteStateList];
            IList<string> possibleFiniteStateLists = possibleFiniteStateListsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPossibleFiniteStateLists, possibleFiniteStateLists);

            var possibleFiniteStateList = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "bf42cb66-f340-4764-ab48-a3938a7c59eb");
            Assert.AreEqual(2, (int)iteration[PropertyNames.RevisionNumber]);

            // verify the amount of returned properties 
            Assert.AreEqual(12, possibleFiniteStateList.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("PossibleFiniteStateList", (string)possibleFiniteStateList[PropertyNames.ClassKind]);

            Assert.AreEqual("Test 2 Possible FiniteState List", (string)possibleFiniteStateList[PropertyNames.Name]);
            Assert.AreEqual("TestPossibleFiniteStateList2", (string)possibleFiniteStateList[PropertyNames.ShortName]);

            Assert.AreEqual(
                "0e92edde-fdff-41db-9b1d-f2e484f12535",
                (string)possibleFiniteStateList[PropertyNames.Owner]);

            var expectedPossibleFiniteStates =
                new List<OrderedItem>
                    {
                        new OrderedItem(12158, "20f4d6aa-f9d3-4e91-aae9-bfbe77ae9b79"),
                        new OrderedItem(12159, "e6eabd4f-6145-4788-8070-53bd031195bd")
                    };
            var possibleFiniteStates =
                JsonConvert.DeserializeObject<List<OrderedItem>>(
                    possibleFiniteStateList[PropertyNames.PossibleState].ToString());
            CollectionAssert.AreEquivalent(expectedPossibleFiniteStates, possibleFiniteStates);

            Assert.AreEqual(
                "20f4d6aa-f9d3-4e91-aae9-bfbe77ae9b79",
                (string)possibleFiniteStateList[PropertyNames.DefaultState]);

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

            // Check PossibleFiniteStates
            var possibleFiniteState = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "20f4d6aa-f9d3-4e91-aae9-bfbe77ae9b79");
            Assert.AreEqual(2, (int)iteration[PropertyNames.RevisionNumber]);

            // verify the amount of returned properties 
            Assert.AreEqual(8, possibleFiniteState.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("PossibleFiniteState", (string)possibleFiniteState[PropertyNames.ClassKind]);
            Assert.AreEqual("On", (string)possibleFiniteState[PropertyNames.Name]);
            Assert.AreEqual("On", (string)possibleFiniteState[PropertyNames.ShortName]);

            expectedAliases = new string[] { };
            aliasesArray = (JArray)possibleFiniteState[PropertyNames.Alias];
            aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            expectedDefinitions = new string[] { };
            definitionsArray = (JArray)possibleFiniteState[PropertyNames.Definition];
            definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            expectedHyperlinks = new string[] { };
            hyperlinksArray = (JArray)possibleFiniteState[PropertyNames.HyperLink];
            h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            possibleFiniteState = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "e6eabd4f-6145-4788-8070-53bd031195bd");
            Assert.AreEqual(2, (int)iteration[PropertyNames.RevisionNumber]);

            // verify the amount of returned properties 
            Assert.AreEqual(8, possibleFiniteState.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("PossibleFiniteState", (string)possibleFiniteState[PropertyNames.ClassKind]);
            Assert.AreEqual("Off", (string)possibleFiniteState[PropertyNames.Name]);
            Assert.AreEqual("Off", (string)possibleFiniteState[PropertyNames.ShortName]);

            expectedAliases = new string[] { };
            aliasesArray = (JArray)possibleFiniteState[PropertyNames.Alias];
            aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            expectedDefinitions = new string[] { };
            definitionsArray = (JArray)possibleFiniteState[PropertyNames.Definition];
            definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            expectedHyperlinks = new string[] { };
            hyperlinksArray = (JArray)possibleFiniteState[PropertyNames.HyperLink];
            h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            // Add a new PossibleFiniteStateList to ActualFiniteStateList
            postBodyPath = this.GetPath(
                "Tests/EngineeringModel/ActualFiniteStateList/PostActualFiniteStateListAddPossibleFiniteStateList.json");
            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            engineeeringModel = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(3, (int)engineeeringModel[PropertyNames.RevisionNumber]);

            var actualFiniteStateList = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "db690d7d-761c-47fd-96d3-840d698a89dc");
            Assert.AreEqual(3, (int)actualFiniteStateList[PropertyNames.RevisionNumber]);

            var expectedPossibleFiniteStateListsInActualFiniteStateList =
                new List<OrderedItem>
                    {
                        new OrderedItem(4598335, "449a5bca-34fd-454a-93f8-a56ac8383fee"),
                        new OrderedItem(4598336, "bf42cb66-f340-4764-ab48-a3938a7c59eb")
                    };

            var possibleFiniteStateListsInActualFiniteStateList = JsonConvert.DeserializeObject<List<OrderedItem>>(
                actualFiniteStateList[PropertyNames.PossibleFiniteStateList].ToString());
            CollectionAssert.AreEquivalent(
                expectedPossibleFiniteStateListsInActualFiniteStateList,
                possibleFiniteStateListsInActualFiniteStateList);

            // get the created ActualState as a side effect of adding PossibleFiniteStateList from the result by it's unique id
            var actualStatesArray = (JArray)actualFiniteStateList[PropertyNames.ActualState];
            IList<string> actualStates = actualStatesArray.Select(x => (string)x).ToList();
            Assert.AreEqual(2, actualStates.Count);

            // get a specific ActualFiniteStateList from the result by it's unique id
            var actualFiniteState = jArray.Single(
                x => (string)x[PropertyNames.ClassKind] == "ActualFiniteState"
                     && (string)x[PropertyNames.PossibleState][1] == "20f4d6aa-f9d3-4e91-aae9-bfbe77ae9b79");

            // verify the amount of returned properties 
            Assert.AreEqual(5, actualFiniteState.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual(3, (int)actualFiniteState[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ActualFiniteState", (string)actualFiniteState[PropertyNames.ClassKind]);
            Assert.AreEqual("MANDATORY", (string)actualFiniteState[PropertyNames.Kind]);

            var expectedPossibleStates = new[]
                                             {
                                                 "b8fdfac4-1c40-475a-ac6c-968654b689b6",
                                                 "20f4d6aa-f9d3-4e91-aae9-bfbe77ae9b79"
                                             };
            var possibleStateArray = (JArray)actualFiniteState[PropertyNames.PossibleState];
            IList<string> possibleStates = possibleStateArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEqual(expectedPossibleStates, possibleStates);

            // get a ActualFiniteStateList from the result by unknown id
            actualFiniteState = jArray.Single(
                x => (string)x[PropertyNames.ClassKind] == "ActualFiniteState"
                     && (string)x[PropertyNames.PossibleState][1] == "e6eabd4f-6145-4788-8070-53bd031195bd");

            // verify the amount of returned properties 
            Assert.AreEqual(5, actualFiniteState.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual(3, (int)actualFiniteState[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ActualFiniteState", (string)actualFiniteState[PropertyNames.ClassKind]);
            Assert.AreEqual("MANDATORY", (string)actualFiniteState[PropertyNames.Kind]);

            expectedPossibleStates = new[]
                                         {
                                             "b8fdfac4-1c40-475a-ac6c-968654b689b6",
                                             "e6eabd4f-6145-4788-8070-53bd031195bd"
                                         };
            possibleStateArray = (JArray)actualFiniteState[PropertyNames.PossibleState];
            possibleStates = possibleStateArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEqual(expectedPossibleStates, possibleStates);
        }

        [Test]
        public void VerifyThatActualStateCanBeCreatedWhenPossibleFiniteStateIsAddedWithWebApi()
        {
            // POST a new PossibleFiniteState
            var iterationUri = new Uri(
                string.Format(
                    UriFormat,
                    this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath = this.GetPath(
                "Tests/EngineeringModel/ActualFiniteStateList/PostNewPossibleFiniteState.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int)engineeeringModel[PropertyNames.RevisionNumber]);

            var possibleFiniteStateList = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "449a5bca-34fd-454a-93f8-a56ac8383fee");
            Assert.AreEqual(2, (int)possibleFiniteStateList[PropertyNames.RevisionNumber]);

            var expectedPossibleFiniteStates =
                new List<OrderedItem>
                    {
                        new OrderedItem(73203278, "b8fdfac4-1c40-475a-ac6c-968654b689b6"),
                        new OrderedItem(73203279, "20f4d6aa-f9d3-4e91-aae9-bfbe77ae9b79")
                    };
            var possibleFiniteStates =
                JsonConvert.DeserializeObject<List<OrderedItem>>(
                    possibleFiniteStateList[PropertyNames.PossibleState].ToString());
            CollectionAssert.AreEquivalent(expectedPossibleFiniteStates, possibleFiniteStates);

            var actualFiniteStateList = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "db690d7d-761c-47fd-96d3-840d698a89dc");
            Assert.AreEqual(2, (int)actualFiniteStateList[PropertyNames.RevisionNumber]);

            // get the created ActualState as a side effect of creating PossibleFiniteState from the result by it's unique id
            var actualStatesArray = (JArray)actualFiniteStateList[PropertyNames.ActualState];
            IList<string> actualStates = actualStatesArray.Select(x => (string)x).ToList();
            Assert.AreEqual(2, actualStates.Count);

            // get a specific ActualFiniteStateList from the result by unknown id
            var actualFiniteState1 = jArray.Single(
                x => (string)x[PropertyNames.ClassKind] == "ActualFiniteState"
                     && (string)x[PropertyNames.PossibleState][0] == "20f4d6aa-f9d3-4e91-aae9-bfbe77ae9b79");

            // verify the amount of returned properties 
            Assert.AreEqual(5, actualFiniteState1.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual(2, (int)actualFiniteState1[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ActualFiniteState", (string)actualFiniteState1[PropertyNames.ClassKind]);
            Assert.AreEqual("MANDATORY", (string)actualFiniteState1[PropertyNames.Kind]);

            var expectedPossibleStates = new[] { "20f4d6aa-f9d3-4e91-aae9-bfbe77ae9b79" };
            var possibleStateArray = (JArray)actualFiniteState1[PropertyNames.PossibleState];
            IList<string> possibleStates = possibleStateArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEqual(expectedPossibleStates, possibleStates);

            // get a specific ActualFiniteStateList from the result by unknown id
            var actualFiniteState2 = jArray.Single(
                x => (string)x[PropertyNames.ClassKind] == "ActualFiniteState"
                     && (string)x[PropertyNames.PossibleState][0] == "b8fdfac4-1c40-475a-ac6c-968654b689b6");

            // verify the amount of returned properties 
            Assert.AreEqual(5, actualFiniteState2.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual(2, (int)actualFiniteState2[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ActualFiniteState", (string)actualFiniteState2[PropertyNames.ClassKind]);
            Assert.AreEqual("MANDATORY", (string)actualFiniteState2[PropertyNames.Kind]);

            expectedPossibleStates = new[] { "b8fdfac4-1c40-475a-ac6c-968654b689b6" };
            possibleStateArray = (JArray)actualFiniteState2[PropertyNames.PossibleState];
            possibleStates = possibleStateArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEqual(expectedPossibleStates, possibleStates);
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

            var expectedPossibleFiniteStateLists =
                new List<OrderedItem> { new OrderedItem(4598335, "449a5bca-34fd-454a-93f8-a56ac8383fee") };

            var possibleFiniteStateLists =
                JsonConvert.DeserializeObject<List<OrderedItem>>(
                    actualFiniteStateList[PropertyNames.PossibleFiniteStateList].ToString());
            CollectionAssert.AreEquivalent(expectedPossibleFiniteStateLists, possibleFiniteStateLists);

            var expectedActualStates = new[] { "b91bfdbb-4277-4a03-b519-e4db839ef5d4" };
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