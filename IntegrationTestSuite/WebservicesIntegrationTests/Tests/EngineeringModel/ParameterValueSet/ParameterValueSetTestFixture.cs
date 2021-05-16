// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterValueSetTestFixture.cs" company="RHEA System S.A.">
//
//   Copyright 2016-2021 RHEA System S.A.
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
    public class ParameterValueSetTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterValueSetIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var parameterValueSetUri =
                new Uri(
                    string.Format(
                        UriFormat,
                        this.Settings.Hostname,
                        "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/6c5aff74-f983-4aa8-a9d6-293b3429307c/valueSet"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterValueSetUri);

            // check if there is the only one ParameterValueSet object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ParameterValueSet from the result by it's unique id
            var parameterValueSet =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "af5c88c6-301f-497b-81f7-53748c3900ed");

            ParameterValueSetTestFixture.VerifyProperties(parameterValueSet);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterValueSetWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var parameterValueSetUri =
                new Uri(
                    string.Format(
                        UriFormat,
                        this.Settings.Hostname,
                        "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/6C5AFF74-F983-4AA8-A9D6-293B3429307C/valueSet?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterValueSetUri);

            // check if there are 5 objects
            Assert.AreEqual(5, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ElementDefinition from the result by it's unique id
            ElementDefinitionTestFixture.VerifyProperties(jArray);

            // get a specific Parameter from the result by it's unique id
            var parameter = jArray.Single(x => (string)x[PropertyNames.Iid] == "6c5aff74-f983-4aa8-a9d6-293b3429307c");
            ParameterTestFixture.VerifyProperties(parameter);

            // get a specific ParameterValueSet from the result by it's unique id
            var parameterValueSet =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "af5c88c6-301f-497b-81f7-53748c3900ed");
            ParameterValueSetTestFixture.VerifyProperties(parameterValueSet);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatAParameterValueSetCannotBeDeletedAndCreatedWithWebApi()
        { 
            var iterationUri =
                new Uri(
                    string.Format(
                        UriFormat,
                        this.Settings.Hostname,
                        "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath =
                this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostNewParameterValueSetAndDeleteOne.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            Assert.Throws<WebException>(() => this.WebClient.PostDto(iterationUri, postBody));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatAParameterValueSetIsCreatedWhenPossibleStateIsAddedWithWebApi()
        {
            // POST state dependent Parameter and check what is returned
            var iterationUri =
                new Uri(
                    string.Format(
                        UriFormat,
                        this.Settings.Hostname,
                        "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath =
                this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostNewStateDependentParameter.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeringModel =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int)engineeringModel[PropertyNames.RevisionNumber]);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");
            Assert.AreEqual(2, (int)elementDefinition[PropertyNames.RevisionNumber]);

            var expectedParameters = new[]
                                         {
                                             "6c5aff74-f983-4aa8-a9d6-293b3429307c",
                                             "3f05483f-66ff-4f21-bc76-45956779f66e",
                                             "2cd4eb9c-e92c-41b2-968c-f03ff7010bad"
                                         };
            var parametersArray = (JArray)elementDefinition[PropertyNames.Parameter];
            IList<string> parameters = parametersArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedParameters, parameters);

            // get a specific Parameter from the result by it's unique id
            var parameter = jArray.Single(x => (string)x[PropertyNames.Iid] == "2cd4eb9c-e92c-41b2-968c-f03ff7010bad");
            Assert.AreEqual(2, (int)parameter[PropertyNames.RevisionNumber]);

            var valueSetsArray = (JArray)parameter[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string)x).ToList();
            Assert.AreEqual(1, valueSets.Count);

            var parameterValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == valueSets[0]);
            Assert.AreEqual(2, (int)parameter[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterValueSet", (string)parameterValueSet[PropertyNames.ClassKind]);

            Assert.AreEqual("MANUAL", (string)parameterValueSet[PropertyNames.ValueSwitch]);

            const string EmptyProperty = "[\"-\"]";
            Assert.AreEqual(EmptyProperty, (string)parameterValueSet[PropertyNames.Published]);
            Assert.AreEqual(EmptyProperty, (string)parameterValueSet[PropertyNames.Formula]);
            Assert.AreEqual(EmptyProperty, (string)parameterValueSet[PropertyNames.Computed]);
            Assert.AreEqual(EmptyProperty, (string)parameterValueSet[PropertyNames.Manual]);
            Assert.AreEqual(EmptyProperty, (string)parameterValueSet[PropertyNames.Reference]);

            Assert.AreEqual(
                "b91bfdbb-4277-4a03-b519-e4db839ef5d4",
                (string)parameterValueSet[PropertyNames.ActualState]);
            Assert.IsNull((string)parameterValueSet[PropertyNames.ActualOption]);

            // POST PossibleFiniteState and check what is returned
            postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostNewPossibleFiniteState.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            engineeringModel =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(3, (int)engineeringModel[PropertyNames.RevisionNumber]);

            // get a specific ActualFiniteStateList from the result by it's unique id
            var actualFiniteStateList =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "db690d7d-761c-47fd-96d3-840d698a89dc");
            Assert.AreEqual(3, (int)actualFiniteStateList[PropertyNames.RevisionNumber]);

            var actualStatesArray = (JArray)actualFiniteStateList[PropertyNames.ActualState];
            IList<string> actualStates = actualStatesArray.Select(x => (string)x).ToList();
            Assert.AreEqual(2, actualStates.Count);

            var expectedPossibleStates = new List<string> { "b8fdfac4-1c40-475a-ac6c-968654b689b6", "b8fdfac4-1c40-475a-ac6c-968654b689b7" };

            foreach (var actualState in actualStates)
            {
                // get an ActualFiniteState from the result by it's unique id for existed PossibleFiniteState
                var actualFiniteState = jArray.Single(x => (string)x[PropertyNames.Iid] == actualState);
                Assert.AreEqual(3, (int)actualFiniteState[PropertyNames.RevisionNumber]);

                Assert.AreEqual(3, (int)actualFiniteState[PropertyNames.RevisionNumber]);
                Assert.AreEqual("ActualFiniteState", (string)actualFiniteState[PropertyNames.ClassKind]);
                Assert.AreEqual("MANDATORY", (string)actualFiniteState[PropertyNames.Kind]);

                var possibleStateArray = (JArray)actualFiniteState[PropertyNames.PossibleState];
                IList<string> possibleStates = possibleStateArray.Select(x => (string)x).ToList();
                Assert.AreEqual(possibleStates.Count, 1);

                var possibleState = possibleStates.First();
                CollectionAssert.Contains(expectedPossibleStates, possibleState);
                expectedPossibleStates.Remove(possibleState);
            }

            CollectionAssert.IsEmpty(expectedPossibleStates, 
                "There still are expected possible states in the collection. " +
                "The ones that are still present there have not been found in the result set where they should have.");

            // get a specific PossibleFiniteStateList from the result by it's unique id
            var possibleFiniteStateList =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "449a5bca-34fd-454a-93f8-a56ac8383fee");
            Assert.AreEqual(3, (int)possibleFiniteStateList[PropertyNames.RevisionNumber]);
            var expectedPossibleFiniteStates = new List<OrderedItem>
                                                   {
                                                       new OrderedItem(73203278, "b8fdfac4-1c40-475a-ac6c-968654b689b6"),
                                                       new OrderedItem(73203279, "b8fdfac4-1c40-475a-ac6c-968654b689b7")
                                                   };
            var possibleFiniteStates =
                JsonConvert.DeserializeObject<List<OrderedItem>>(
                    possibleFiniteStateList[PropertyNames.PossibleState].ToString());
            CollectionAssert.AreEquivalent(expectedPossibleFiniteStates, possibleFiniteStates);

            // get a specific PossibleFiniteState from the result by it's unique id
            var possibleFiniteState =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "b8fdfac4-1c40-475a-ac6c-968654b689b7");
            Assert.AreEqual(3, (int)possibleFiniteState[PropertyNames.RevisionNumber]);
            Assert.AreEqual("PossibleFiniteState", (string)possibleFiniteState[PropertyNames.ClassKind]);
            Assert.AreEqual("Test PossibleFiniteState", (string)possibleFiniteState[PropertyNames.Name]);
            Assert.AreEqual("TestPossibleFiniteState", (string)possibleFiniteState[PropertyNames.ShortName]);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)possibleFiniteState[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)possibleFiniteState[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)possibleFiniteState[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            // get a specific Parameter from the result by it's unique id
            parameter = jArray.Single(x => (string)x[PropertyNames.Iid] == "2cd4eb9c-e92c-41b2-968c-f03ff7010bad");
            Assert.AreEqual(3, (int)parameter[PropertyNames.RevisionNumber]);

            valueSetsArray = (JArray)parameter[PropertyNames.ValueSet];
            valueSets = valueSetsArray.Select(x => (string)x).ToList();
            Assert.AreEqual(2, valueSets.Count);

            var actualStatesCheckList = actualStates.ToList();

            foreach (var valueSet in valueSets)
            {
                parameterValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == valueSet);
                Assert.AreEqual(3, (int)parameterValueSet[PropertyNames.RevisionNumber]);
                Assert.AreEqual("ParameterValueSet", (string)parameterValueSet[PropertyNames.ClassKind]);

                Assert.AreEqual("MANUAL", (string)parameterValueSet[PropertyNames.ValueSwitch]);
                Assert.AreEqual(EmptyProperty, (string)parameterValueSet[PropertyNames.Published]);
                Assert.AreEqual(EmptyProperty, (string)parameterValueSet[PropertyNames.Formula]);
                Assert.AreEqual(EmptyProperty, (string)parameterValueSet[PropertyNames.Computed]);
                Assert.AreEqual(EmptyProperty, (string)parameterValueSet[PropertyNames.Manual]);
                Assert.AreEqual(EmptyProperty, (string)parameterValueSet[PropertyNames.Reference]);

                var actualStatesCheck = (string) parameterValueSet[PropertyNames.ActualState];
                CollectionAssert.Contains(actualStatesCheckList, actualStatesCheck);
                actualStatesCheckList.Remove(actualStatesCheck);
                Assert.IsNull((string)parameterValueSet[PropertyNames.ActualOption]);
            }

            CollectionAssert.IsEmpty(actualStatesCheckList,
                "There still are expected actual states in the collection. " +
                "The ones that are still present there have not been found in the result set where they should have.");

            // POST PossibleFiniteStateList and check what is returned
            postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostNewPossibleFiniteStateList.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            engineeringModel =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(4, (int)engineeringModel[PropertyNames.RevisionNumber]);

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            Assert.AreEqual(4, (int)iteration[PropertyNames.RevisionNumber]);

            var expectedPossibleFiniteStateLists = new[]
                                                       {
                                                           "449a5bca-34fd-454a-93f8-a56ac8383fee",
                                                           "dc3e3763-b8ed-4159-acee-d6a0f4de3dba"
                                                       };
            var possibleFiniteStateListsArray = (JArray)iteration[PropertyNames.PossibleFiniteStateList];
            IList<string> possibleFiniteStateLists = possibleFiniteStateListsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPossibleFiniteStateLists, possibleFiniteStateLists);

            // get a specific PossibleFiniteStateList from the result by it's unique id
            possibleFiniteStateList =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "dc3e3763-b8ed-4159-acee-d6a0f4de3dba");

            Assert.AreEqual(4, (int)possibleFiniteStateList[PropertyNames.RevisionNumber]);
            Assert.AreEqual("PossibleFiniteStateList", (string)possibleFiniteStateList[PropertyNames.ClassKind]);

            Assert.AreEqual("PossibleFiniteStateList Test", (string)possibleFiniteStateList[PropertyNames.Name]);
            Assert.AreEqual("PossibleFiniteStateListTest", (string)possibleFiniteStateList[PropertyNames.ShortName]);

            Assert.AreEqual(
                "0e92edde-fdff-41db-9b1d-f2e484f12535",
                (string)possibleFiniteStateList[PropertyNames.Owner]);

            expectedPossibleFiniteStates = new List<OrderedItem>
                                               {
                                                   new OrderedItem(-2577538, "d9ed3b43-ba45-45d5-ba1b-196703998a01")
                                               };
            possibleFiniteStates =
                JsonConvert.DeserializeObject<List<OrderedItem>>(
                    possibleFiniteStateList[PropertyNames.PossibleState].ToString());
            CollectionAssert.AreEquivalent(expectedPossibleFiniteStates, possibleFiniteStates);

            Assert.IsNull((string)possibleFiniteStateList[PropertyNames.DefaultState]);

            var expectedCategories = new string[] { };
            var categoriesArray = (JArray)possibleFiniteStateList[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            expectedAliases = new string[] { };
            aliasesArray = (JArray)possibleFiniteStateList[PropertyNames.Alias];
            aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            expectedDefinitions = new string[] { };
            definitionsArray = (JArray)possibleFiniteStateList[PropertyNames.Definition];
            definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            expectedHyperlinks = new string[] { };
            hyperlinksArray = (JArray)possibleFiniteStateList[PropertyNames.HyperLink];
            h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            // get a specific PossibleFiniteState from the result by it's unique id
            possibleFiniteState =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "d9ed3b43-ba45-45d5-ba1b-196703998a01");
            Assert.AreEqual(4, (int)possibleFiniteState[PropertyNames.RevisionNumber]);
            Assert.AreEqual("PossibleFiniteState", (string)possibleFiniteState[PropertyNames.ClassKind]);
            Assert.AreEqual("PossibleFiniteState Test", (string)possibleFiniteState[PropertyNames.Name]);
            Assert.AreEqual("PossibleFiniteStateTest", (string)possibleFiniteState[PropertyNames.ShortName]);

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

            // get a specific ActualFiniteStateList from the result by it's unique id
            actualFiniteStateList =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "db690d7d-761c-47fd-96d3-840d698a89dc");
            Assert.AreEqual(4, (int)actualFiniteStateList[PropertyNames.RevisionNumber]);
            actualStatesArray = (JArray)actualFiniteStateList[PropertyNames.ActualState];
            actualStates = actualStatesArray.Select(x => (string)x).ToList();
            Assert.AreEqual(2, actualStates.Count);
            var expectedPossibleFiniteStateListsInActualFiniteStateList = new List<OrderedItem>
                                                                              {
                                                                                  new OrderedItem(
                                                                                      4598335,
                                                                                      "449a5bca-34fd-454a-93f8-a56ac8383fee"),
                                                                                  new OrderedItem(
                                                                                      4598336,
                                                                                      "dc3e3763-b8ed-4159-acee-d6a0f4de3dba")
                                                                              };
            var possibleFiniteStateListsInActualFiniteStateList =
                JsonConvert.DeserializeObject<List<OrderedItem>>(
                    actualFiniteStateList[PropertyNames.PossibleFiniteStateList].ToString());
            CollectionAssert.AreEquivalent(
                expectedPossibleFiniteStateListsInActualFiniteStateList,
                possibleFiniteStateListsInActualFiniteStateList);

            // get a specific ActualFiniteState from the result by it's unique id
            var actualFiniteState0 = jArray.Single(x => (string)x[PropertyNames.Iid] == actualStates[0]);
            Assert.AreEqual(4, (int)actualFiniteState0[PropertyNames.RevisionNumber]);

            var actualFiniteState1 = jArray.Single(x => (string)x[PropertyNames.Iid] == actualStates[1]);
            Assert.AreEqual(4, (int)actualFiniteState1[PropertyNames.RevisionNumber]);

            parameterValueSet = jArray.Single(x => (string)x[PropertyNames.ClassKind] == "ParameterValueSet" && (string)x[PropertyNames.ActualState] == actualStates[0]);
            Assert.AreEqual(4, (int)parameterValueSet[PropertyNames.RevisionNumber]);

            parameterValueSet = jArray.Single(x => (string)x[PropertyNames.ClassKind] == "ParameterValueSet" && (string)x[PropertyNames.ActualState] == actualStates[1]);
            Assert.AreEqual(4, (int)parameterValueSet[PropertyNames.RevisionNumber]);

            // get a specific Parameter from the result by it's unique id
            parameter = jArray.Single(x => (string)x[PropertyNames.Iid] == "2cd4eb9c-e92c-41b2-968c-f03ff7010bad");
            Assert.AreEqual(4, (int)parameter[PropertyNames.RevisionNumber]);
        }

        /// <summary>
        /// Verifies all properties of the ParameterValueSet <see cref="JToken"/>
        /// </summary>
        /// <param name="parameterValueSet">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ParameterValueSet object
        /// </param>
        public static void VerifyProperties(JToken parameterValueSet)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(11, parameterValueSet.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("af5c88c6-301f-497b-81f7-53748c3900ed", (string)parameterValueSet[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)parameterValueSet[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterValueSet", (string)parameterValueSet[PropertyNames.ClassKind]);

            Assert.AreEqual("MANUAL", (string)parameterValueSet[PropertyNames.ValueSwitch]);

            const string emptyProperty = "[\"-\"]";
            Assert.AreEqual(emptyProperty, (string)parameterValueSet[PropertyNames.Published]);
            Assert.AreEqual(emptyProperty, (string)parameterValueSet[PropertyNames.Formula]);
            Assert.AreEqual(emptyProperty, (string)parameterValueSet[PropertyNames.Computed]);
            Assert.AreEqual(emptyProperty, (string)parameterValueSet[PropertyNames.Manual]);
            Assert.AreEqual(emptyProperty, (string)parameterValueSet[PropertyNames.Reference]);

            Assert.IsNull((string)parameterValueSet[PropertyNames.ActualState]);
            Assert.IsNull((string)parameterValueSet[PropertyNames.ActualOption]);
        }

        [Test]
        [Category("POST")]
        public void Verify_that_the_computed_and_formula_property_of_a_ParameterValueSet_can_updated()
        {
            // POST state dependent Parameter and check what is returned
            var iterationUri =
                new Uri(
                    string.Format(
                        UriFormat,
                        this.Settings.Hostname,
                        "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath =
                this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostUpdateComputedValueOfParameterValueSet.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int)engineeeringModel[PropertyNames.RevisionNumber]);

            var parameterValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == "72ec3701-bcb5-4bf6-bd78-30fd1b65e3be");

            Assert.AreEqual("72ec3701-bcb5-4bf6-bd78-30fd1b65e3be", (string)parameterValueSet[PropertyNames.Iid]);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatParameterValueSetValuesAreSerializedAndDeserializedCorrectly([ValueSource(nameof(TestStrings))] string inputValue)
        {
            var iterationUri =
                new Uri(
                    string.Format(
                        UriFormat,
                        this.Settings.Hostname,
                        "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath =
                this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostUpdateParameterValueSetTemplate.json.txt");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var inputAsInnerJson = JsonConvert.ToString(inputValue); 
            postBody = postBody.Replace("<INNERJSON>", inputAsInnerJson);

            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var parameterValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == "72ec3701-bcb5-4bf6-bd78-30fd1b65e3be");

            Assert.AreEqual(inputValue, JsonConvert.DeserializeObject<List<string>>((string)parameterValueSet[PropertyNames.Formula])[0]);
            Assert.AreEqual(inputValue, JsonConvert.DeserializeObject<List<string>>((string)parameterValueSet[PropertyNames.Published])[0]);
            Assert.AreEqual(inputValue, JsonConvert.DeserializeObject<List<string>>((string)parameterValueSet[PropertyNames.Computed])[0]);
            Assert.AreEqual(inputValue, JsonConvert.DeserializeObject<List<string>>((string)parameterValueSet[PropertyNames.Manual])[0]);
            Assert.AreEqual(new List<string> {inputValue, inputValue}, JsonConvert.DeserializeObject<List<string>>((string)parameterValueSet[PropertyNames.Reference]));

            var parameterValueSetUri1 =
                new Uri(
                    string.Format(
                        UriFormat,
                        this.Settings.Hostname,
                        "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/3f05483f-66ff-4f21-bc76-45956779f66e/valueSet/72ec3701-bcb5-4bf6-bd78-30fd1b65e3be?revisionFrom=1&revisionTo=2"));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatGettingRevisionsWorks()
        {
            var iterationUri =
                new Uri(
                    string.Format(
                        UriFormat,
                        this.Settings.Hostname,
                        "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));

            var postBodyPath =
                this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostUpdateParameterValueSetTemplate.json.txt");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var inputAsInnerJson = JsonConvert.ToString("Test Revisions");
            postBody = postBody.Replace("<INNERJSON>", inputAsInnerJson);

            var jArray1 = this.WebClient.PostDto(iterationUri, postBody);
            Assert.AreEqual(3, jArray1.Count);

            var parameterValueSetUri1 =
                new Uri(
                    string.Format(
                        UriFormat,
                        this.Settings.Hostname,
                        "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/3f05483f-66ff-4f21-bc76-45956779f66e/valueSet/72ec3701-bcb5-4bf6-bd78-30fd1b65e3be?revisionFrom=1&revisionTo=2"));

            var jArray2 = this.WebClient.GetDto(parameterValueSetUri1);
            Assert.AreEqual(2, jArray2.Count);

            var parameterValueSetUri2 =
                new Uri(
                    string.Format(
                        UriFormat,
                        this.Settings.Hostname,
                        "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/3f05483f-66ff-4f21-bc76-45956779f66e/valueSet/72ec3701-bcb5-4bf6-bd78-30fd1b65e3be?revisionFrom=2000-01-01T12:00:00&revisionTo=2120-12-31T12:00:00"));

            var jArray3 = this.WebClient.GetDto(parameterValueSetUri2);
            Assert.AreEqual(2, jArray3.Count);
        }


        private const string JsonString = @"{""widget"": {
                ""debug"": ""on"",
                ""window"": {
                    ""title"": ""Sample Konfabulator Widget"",
                    ""name"": ""main_window"",
                    ""width"": 500,
                    ""height"": 500
                },
                ""image"": { 
                    ""src"": ""Images/Sun.png"",
                    ""name"": ""sun1"",
                    ""hOffset"": 250,
                    ""vOffset"": 250,
                    ""alignment"": ""center""
                },
                ""text"": {
                    ""data"": ""Click Here"",
                    ""size"": 36,
                    ""style"": ""bold"",
                    ""name"": ""text1"",
                    ""hOffset"": 250,
                    ""vOffset"": 100,
                    ""alignment"": ""center"",
                    ""onMouseUp"": ""sun1.opacity = (sun1.opacity / 100) * 90;""
                }
            }}";

        private const string XmlString = @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <bookstore>
                <book category=""cooking"">
                    <title lang=""en"">Everyday food</title>
                    <author>Some great cook</author>
                    <year>2005</year>
                    <price>30.00</price>
                    <data><![CDATA[Within this Character Data block I can
                        use double dashes as much as I want (along with <, &, ', and "")
                        *and* %MyParamEntity; will be expanded to the text
                        ""Has been expanded"" ... however, I can't use
                        the CEND sequence.If I need to use CEND I must escape one of the
                        brackets or the greater-than sign using concatenated CDATA sections.
                        ]]></data>
                </book>
                <book category=""children"">
                    <title lang=""en"">Harry the child</title>
                    <author>Some child author</author>
                    <year>2005</year>
                    <price>29.99</price>
                </book>
                <book category=""web"">
                    <title lang=""en"">Learning XML</title>
                    <author>Some XML expert</author>
                    <year>2003</year>
                    <price>39.95</price>
                </book>
            </bookstore>";

        private static readonly string[] TestStrings = 
        {
            "value with trailing spaces  ",
            "value with trailing space ",
            " value with leading spaces",
            "  value with leading space",
            "\t\t\tvalue with leading and trailing tabs \t",
            "\nvalue with leading and trailing linebreaks \r",
            "=2*(2+2)",
            "=2*\n(2+2)",
            "=2*\r(2+2)",
            "=2*\r\n(2+2)",
            "=2*\n\r(2+2)",
            "= 2 * \n ( 2 + 2 )",
            "=2*\b(2+2)",
            "=2*\f(2+2)",
            "=2*\t(2+2)",
            "Ar54WbBu + yhw - R:G!d)C!X_H % Vy ? V",
            "qm+L/{hp,qU[F\nnSyFymmZ\n+F(G/pP8@",
            "JSfJzH!U5:*wcnzT+{a5-L&+Xaq[g4",
            "EfRKJ[*A%uiM9MJ_h-z?9X(KYJQ/xL",
            "B_Dw+Tw.7g,.36]7(j8(k3/hxX,K_y",
            "qKt_C}@).D!ik.4W48ESR}w*VGvaub",
            "33CDr2NPZ[fJQ]p?aXT2L{giUUm}g#",
            "mpb-!ump7S{D)]Z9B@S([FXMRSq/9S",
            "D,VeZQRnV/}?}*qxMeX}N7*%R]!Tf/",
            "L$X7@P,JhcYM,-e4Z5,!ft.UbC[Y{n",
            "QWuAr.P$RUCf(NiV{7}tcwnia:.Fnp",
            "L%%t?cdpa?g#-PE4w6=[yU72Cgxz:f",
            ",GCeVX=$6R,(JJW[mLd4uF@{,Yr%NL",
            "i?5,/.G%D,M3im?8:,+ju}(CMh_E77",
            "}8Bn)rtS4BGTWThmT,=nu,q{[H?):9",
            "ScVmbHjSB[HS$8A*C{awPvvp{%@5Xr",
            "wy6bDVDuim}YLhB24=[y6!4vpM2pTw",
            "f:][.LfcN#(gH=Dq$6Lcp7TWQP7LH!",
            "!&.v8L44$ep69u+W-_5jq?DV@fi($H",
            "?_uB5Z(U$B6,cVPMPJv%q}d[+2PAMZ",
            "[_*q5d$U{qE7}r_7$fdf$h5yBFpPG+",
            XmlString,
            JsonString
        };
    }
}
