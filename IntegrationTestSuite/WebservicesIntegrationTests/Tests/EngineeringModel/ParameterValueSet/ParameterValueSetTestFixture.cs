// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterValueSetTestFixture.cs" company="Starion Group S.A.">
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
            var parameterValueSetUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/6c5aff74-f983-4aa8-a9d6-293b3429307c/valueSet");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterValueSetUri);

            // check if there is the only one ParameterValueSet object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific ParameterValueSet from the result by it's unique id
            var parameterValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == "af5c88c6-301f-497b-81f7-53748c3900ed");

            ParameterValueSetTestFixture.VerifyProperties(parameterValueSet);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterValueSetWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var parameterValueSetUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/6C5AFF74-F983-4AA8-A9D6-293B3429307C/valueSet?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(parameterValueSetUri);

            // check if there are 5 objects
            Assert.That(jArray.Count, Is.EqualTo(5));

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ElementDefinition from the result by it's unique id
            ElementDefinitionTestFixture.VerifyProperties(jArray);

            // get a specific Parameter from the result by it's unique id
            var parameter = jArray.Single(x => (string)x[PropertyNames.Iid] == "6c5aff74-f983-4aa8-a9d6-293b3429307c");
            ParameterTestFixture.VerifyProperties(parameter);

            // get a specific ParameterValueSet from the result by it's unique id
            var parameterValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == "af5c88c6-301f-497b-81f7-53748c3900ed");
            ParameterValueSetTestFixture.VerifyProperties(parameterValueSet);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatAParameterValueSetCannotBeDeletedAndCreatedWithWebApi()
        { 
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostNewParameterValueSetAndDeleteOne.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            Assert.Throws<WebException>(() => this.WebClient.PostDto(iterationUri, postBody));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatAParameterValueSetIsCreatedWhenPossibleStateIsAddedWithWebApi()
        {
            // POST state dependent Parameter and check what is returned
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostNewStateDependentParameter.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition = jArray.Single(x => (string)x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");
            Assert.That((int)elementDefinition[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var expectedParameters = new[]
                                         {
                                             "6c5aff74-f983-4aa8-a9d6-293b3429307c",
                                             "3f05483f-66ff-4f21-bc76-45956779f66e",
                                             "2cd4eb9c-e92c-41b2-968c-f03ff7010bad"
                                         };
            var parametersArray = (JArray)elementDefinition[PropertyNames.Parameter];
            IList<string> parameters = parametersArray.Select(x => (string)x).ToList();
            Assert.That(parameters, Is.EquivalentTo(expectedParameters));

            // get a specific Parameter from the result by it's unique id
            var parameter = jArray.Single(x => (string)x[PropertyNames.Iid] == "2cd4eb9c-e92c-41b2-968c-f03ff7010bad");
            Assert.That((int)parameter[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var valueSetsArray = (JArray)parameter[PropertyNames.ValueSet];
            IList<string> valueSets = valueSetsArray.Select(x => (string)x).ToList();
            Assert.That(valueSets.Count, Is.EqualTo(1));

            var parameterValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == valueSets[0]);
            Assert.That((int)parameter[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string)parameterValueSet[PropertyNames.ClassKind], Is.EqualTo("ParameterValueSet"));

            Assert.That((string)parameterValueSet[PropertyNames.ValueSwitch], Is.EqualTo("MANUAL"));

            const string EmptyProperty = "[\"-\"]";
            Assert.That((string)parameterValueSet[PropertyNames.Published], Is.EqualTo(EmptyProperty));
            Assert.That((string)parameterValueSet[PropertyNames.Formula], Is.EqualTo(EmptyProperty));
            Assert.That((string)parameterValueSet[PropertyNames.Computed], Is.EqualTo(EmptyProperty));
            Assert.That((string)parameterValueSet[PropertyNames.Manual], Is.EqualTo(EmptyProperty));
            Assert.That((string)parameterValueSet[PropertyNames.Reference], Is.EqualTo(EmptyProperty));

            Assert.That((string)parameterValueSet[PropertyNames.ActualState], Is.EqualTo("b91bfdbb-4277-4a03-b519-e4db839ef5d4"));
            Assert.That((string)parameterValueSet[PropertyNames.ActualOption], Is.Null);

            // POST PossibleFiniteState and check what is returned
            postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostNewPossibleFiniteState.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            engineeringModel =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeringModel[PropertyNames.RevisionNumber], Is.EqualTo(3));

            // get a specific ActualFiniteStateList from the result by it's unique id
            var actualFiniteStateList =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "db690d7d-761c-47fd-96d3-840d698a89dc");
            Assert.That((int)actualFiniteStateList[PropertyNames.RevisionNumber], Is.EqualTo(3));

            var actualStatesArray = (JArray)actualFiniteStateList[PropertyNames.ActualState];
            IList<string> actualStates = actualStatesArray.Select(x => (string)x).ToList();
            Assert.That(actualStates.Count, Is.EqualTo(2));

            var expectedPossibleStates = new List<string> { "b8fdfac4-1c40-475a-ac6c-968654b689b6", "b8fdfac4-1c40-475a-ac6c-968654b689b7" };

            foreach (var actualState in actualStates)
            {
                // get an ActualFiniteState from the result by it's unique id for existed PossibleFiniteState
                var actualFiniteState = jArray.Single(x => (string)x[PropertyNames.Iid] == actualState);
                Assert.That((int)actualFiniteState[PropertyNames.RevisionNumber], Is.EqualTo(3));

                Assert.That((int)actualFiniteState[PropertyNames.RevisionNumber], Is.EqualTo(3));
                Assert.That((string)actualFiniteState[PropertyNames.ClassKind], Is.EqualTo("ActualFiniteState"));
                Assert.That((string)actualFiniteState[PropertyNames.Kind], Is.EqualTo("MANDATORY"));

                var possibleStateArray = (JArray)actualFiniteState[PropertyNames.PossibleState];
                IList<string> possibleStates = possibleStateArray.Select(x => (string)x).ToList();
                Assert.That(1, Is.EqualTo(possibleStates.Count));

                var possibleState = possibleStates.First();

                Assert.That(expectedPossibleStates, Does.Contain(possibleState));

                expectedPossibleStates.Remove(possibleState);
            }

            Assert.That(
                expectedPossibleStates,
                Is.Empty,
                "There still are expected possible states in the collection. " +
                "The ones that are still present there have not been found in the result set where they should have.");

            // get a specific PossibleFiniteStateList from the result by it's unique id
            var possibleFiniteStateList =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "449a5bca-34fd-454a-93f8-a56ac8383fee");
            Assert.That((int)possibleFiniteStateList[PropertyNames.RevisionNumber], Is.EqualTo(3));
            var expectedPossibleFiniteStates = new List<OrderedItem>
                                                   {
                                                       new OrderedItem(73203278, "b8fdfac4-1c40-475a-ac6c-968654b689b6"),
                                                       new OrderedItem(73203279, "b8fdfac4-1c40-475a-ac6c-968654b689b7")
                                                   };
            var possibleFiniteStates =
                JsonConvert.DeserializeObject<List<OrderedItem>>(
                    possibleFiniteStateList[PropertyNames.PossibleState].ToString());
            Assert.That(possibleFiniteStates, Is.EquivalentTo(expectedPossibleFiniteStates));

            // get a specific PossibleFiniteState from the result by it's unique id
            var possibleFiniteState =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "b8fdfac4-1c40-475a-ac6c-968654b689b7");
            Assert.That((int)possibleFiniteState[PropertyNames.RevisionNumber], Is.EqualTo(3));
            Assert.That((string)possibleFiniteState[PropertyNames.ClassKind], Is.EqualTo("PossibleFiniteState"));
            Assert.That((string)possibleFiniteState[PropertyNames.Name], Is.EqualTo("Test PossibleFiniteState"));
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

            // get a specific Parameter from the result by it's unique id
            parameter = jArray.Single(x => (string)x[PropertyNames.Iid] == "2cd4eb9c-e92c-41b2-968c-f03ff7010bad");
            Assert.That((int)parameter[PropertyNames.RevisionNumber], Is.EqualTo(3));

            valueSetsArray = (JArray)parameter[PropertyNames.ValueSet];
            valueSets = valueSetsArray.Select(x => (string)x).ToList();
            Assert.That(valueSets.Count, Is.EqualTo(2));

            var actualStatesCheckList = actualStates.ToList();

            foreach (var valueSet in valueSets)
            {
                parameterValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == valueSet);
                Assert.That((int)parameterValueSet[PropertyNames.RevisionNumber], Is.EqualTo(3));
                Assert.That((string)parameterValueSet[PropertyNames.ClassKind], Is.EqualTo("ParameterValueSet"));

                Assert.That((string)parameterValueSet[PropertyNames.ValueSwitch], Is.EqualTo("MANUAL"));
                Assert.That((string)parameterValueSet[PropertyNames.Published], Is.EqualTo(EmptyProperty));
                Assert.That((string)parameterValueSet[PropertyNames.Formula], Is.EqualTo(EmptyProperty));
                Assert.That((string)parameterValueSet[PropertyNames.Computed], Is.EqualTo(EmptyProperty));
                Assert.That((string)parameterValueSet[PropertyNames.Manual], Is.EqualTo(EmptyProperty));
                Assert.That((string)parameterValueSet[PropertyNames.Reference], Is.EqualTo(EmptyProperty));

                var actualStatesCheck = (string) parameterValueSet[PropertyNames.ActualState];
                Assert.That(actualStatesCheckList, Does.Contain(actualStatesCheck));

                actualStatesCheckList.Remove(actualStatesCheck);
                Assert.That((string)parameterValueSet[PropertyNames.ActualOption], Is.Null);
            }

            Assert.That(
                actualStatesCheckList,
                Is.Empty,
                "There still are expected actual states in the collection. " +
                "The ones that are still present there have not been found in the result set where they should have.");

            // POST PossibleFiniteStateList and check what is returned
            postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostNewPossibleFiniteStateList.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            engineeringModel =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeringModel[PropertyNames.RevisionNumber], Is.EqualTo(4));

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            Assert.That((int)iteration[PropertyNames.RevisionNumber], Is.EqualTo(4));

            var expectedPossibleFiniteStateLists = new[]
                                                       {
                                                           "449a5bca-34fd-454a-93f8-a56ac8383fee",
                                                           "dc3e3763-b8ed-4159-acee-d6a0f4de3dba"
                                                       };
            var possibleFiniteStateListsArray = (JArray)iteration[PropertyNames.PossibleFiniteStateList];
            IList<string> possibleFiniteStateLists = possibleFiniteStateListsArray.Select(x => (string)x).ToList();
            Assert.That(possibleFiniteStateLists, Is.EquivalentTo(expectedPossibleFiniteStateLists));

            // get a specific PossibleFiniteStateList from the result by it's unique id
            possibleFiniteStateList =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "dc3e3763-b8ed-4159-acee-d6a0f4de3dba");

            Assert.That((int)possibleFiniteStateList[PropertyNames.RevisionNumber], Is.EqualTo(4));
            Assert.That((string)possibleFiniteStateList[PropertyNames.ClassKind], Is.EqualTo("PossibleFiniteStateList"));

            Assert.That((string)possibleFiniteStateList[PropertyNames.Name], Is.EqualTo("PossibleFiniteStateList Test"));
            Assert.That((string)possibleFiniteStateList[PropertyNames.ShortName], Is.EqualTo("PossibleFiniteStateListTest"));

            Assert.That((string)possibleFiniteStateList[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            expectedPossibleFiniteStates = new List<OrderedItem>
                                               {
                                                   new OrderedItem(-2577538, "d9ed3b43-ba45-45d5-ba1b-196703998a01")
                                               };
            possibleFiniteStates =
                JsonConvert.DeserializeObject<List<OrderedItem>>(
                    possibleFiniteStateList[PropertyNames.PossibleState].ToString());
            Assert.That(possibleFiniteStates, Is.EquivalentTo(expectedPossibleFiniteStates));

            Assert.That((string)possibleFiniteStateList[PropertyNames.DefaultState], Is.Null);

            var expectedCategories = new string[] { };
            var categoriesArray = (JArray)possibleFiniteStateList[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string)x).ToList();
            Assert.That(categories, Is.EquivalentTo(expectedCategories));

            expectedAliases = new string[] { };
            aliasesArray = (JArray)possibleFiniteStateList[PropertyNames.Alias];
            aliases = aliasesArray.Select(x => (string)x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            expectedDefinitions = new string[] { };
            definitionsArray = (JArray)possibleFiniteStateList[PropertyNames.Definition];
            definitions = definitionsArray.Select(x => (string)x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            expectedHyperlinks = new string[] { };
            hyperlinksArray = (JArray)possibleFiniteStateList[PropertyNames.HyperLink];
            h = hyperlinksArray.Select(x => (string)x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));

            // get a specific PossibleFiniteState from the result by it's unique id
            possibleFiniteState =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "d9ed3b43-ba45-45d5-ba1b-196703998a01");
            Assert.That((int)possibleFiniteState[PropertyNames.RevisionNumber], Is.EqualTo(4));
            Assert.That((string)possibleFiniteState[PropertyNames.ClassKind], Is.EqualTo("PossibleFiniteState"));
            Assert.That((string)possibleFiniteState[PropertyNames.Name], Is.EqualTo("PossibleFiniteState Test"));
            Assert.That((string)possibleFiniteState[PropertyNames.ShortName], Is.EqualTo("PossibleFiniteStateTest"));

            expectedAliases = new string[] { };
            aliasesArray = (JArray)possibleFiniteState[PropertyNames.Alias];
            aliases = aliasesArray.Select(x => (string)x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            expectedDefinitions = new string[] { };
            definitionsArray = (JArray)possibleFiniteState[PropertyNames.Definition];
            definitions = definitionsArray.Select(x => (string)x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            expectedHyperlinks = new string[] { };
            hyperlinksArray = (JArray)possibleFiniteState[PropertyNames.HyperLink];
            h = hyperlinksArray.Select(x => (string)x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));

            // get a specific ActualFiniteStateList from the result by it's unique id
            actualFiniteStateList =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "db690d7d-761c-47fd-96d3-840d698a89dc");
            Assert.That((int)actualFiniteStateList[PropertyNames.RevisionNumber], Is.EqualTo(4));
            actualStatesArray = (JArray)actualFiniteStateList[PropertyNames.ActualState];
            actualStates = actualStatesArray.Select(x => (string)x).ToList();
            Assert.That(actualStates.Count, Is.EqualTo(2));
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
            Assert.That(possibleFiniteStateListsInActualFiniteStateList, Is.EquivalentTo(expectedPossibleFiniteStateListsInActualFiniteStateList));

            // get a specific ActualFiniteState from the result by it's unique id
            var actualFiniteState0 = jArray.Single(x => (string)x[PropertyNames.Iid] == actualStates[0]);
            Assert.That((int)actualFiniteState0[PropertyNames.RevisionNumber], Is.EqualTo(4));

            var actualFiniteState1 = jArray.Single(x => (string)x[PropertyNames.Iid] == actualStates[1]);
            Assert.That((int)actualFiniteState1[PropertyNames.RevisionNumber], Is.EqualTo(4));

            parameterValueSet = jArray.Single(x => (string)x[PropertyNames.ClassKind] == "ParameterValueSet" && (string)x[PropertyNames.ActualState] == actualStates[0]);
            Assert.That((int)parameterValueSet[PropertyNames.RevisionNumber], Is.EqualTo(4));

            parameterValueSet = jArray.Single(x => (string)x[PropertyNames.ClassKind] == "ParameterValueSet" && (string)x[PropertyNames.ActualState] == actualStates[1]);
            Assert.That((int)parameterValueSet[PropertyNames.RevisionNumber], Is.EqualTo(4));

            // get a specific Parameter from the result by it's unique id
            parameter = jArray.Single(x => (string)x[PropertyNames.Iid] == "2cd4eb9c-e92c-41b2-968c-f03ff7010bad");
            Assert.That((int)parameter[PropertyNames.RevisionNumber], Is.EqualTo(4));
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
            Assert.That(parameterValueSet.Children().Count(), Is.EqualTo(11));

            // assert that the properties are what is expected
            Assert.That((string)parameterValueSet[PropertyNames.Iid], Is.EqualTo("af5c88c6-301f-497b-81f7-53748c3900ed"));
            Assert.That((int)parameterValueSet[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)parameterValueSet[PropertyNames.ClassKind], Is.EqualTo("ParameterValueSet"));

            Assert.That((string)parameterValueSet[PropertyNames.ValueSwitch], Is.EqualTo("MANUAL"));

            const string emptyProperty = "[\"-\"]";
            Assert.That((string)parameterValueSet[PropertyNames.Published], Is.EqualTo(emptyProperty));
            Assert.That((string)parameterValueSet[PropertyNames.Formula], Is.EqualTo(emptyProperty));
            Assert.That((string)parameterValueSet[PropertyNames.Computed], Is.EqualTo(emptyProperty));
            Assert.That((string)parameterValueSet[PropertyNames.Manual], Is.EqualTo(emptyProperty));
            Assert.That((string)parameterValueSet[PropertyNames.Reference], Is.EqualTo(emptyProperty));

            Assert.That((string)parameterValueSet[PropertyNames.ActualState], Is.Null);
            Assert.That((string)parameterValueSet[PropertyNames.ActualOption], Is.Null);
        }

        [Test]
        [Category("POST")]
        public void Verify_that_the_computed_and_formula_property_of_a_ParameterValueSet_can_updated()
        {
            // POST state dependent Parameter and check what is returned
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostUpdateComputedValueOfParameterValueSet.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var parameterValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == "72ec3701-bcb5-4bf6-bd78-30fd1b65e3be");

            Assert.That((string)parameterValueSet[PropertyNames.Iid], Is.EqualTo("72ec3701-bcb5-4bf6-bd78-30fd1b65e3be"));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatParameterValueSetValuesAreSerializedAndDeserializedCorrectly([ValueSource(nameof(TestStrings))] string inputValue)
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostUpdateParameterValueSetTemplate.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var inputAsInnerJson = JsonConvert.SerializeObject(inputValue); 
            postBody = postBody.Replace("<INNERJSON>", $"[{inputAsInnerJson}]");

            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var parameterValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == "72ec3701-bcb5-4bf6-bd78-30fd1b65e3be");

            Assert.That(JsonConvert.DeserializeObject<List<string>>((string)parameterValueSet[PropertyNames.Formula])[0], Is.EqualTo(inputValue));
            Assert.That(JsonConvert.DeserializeObject<List<string>>((string)parameterValueSet[PropertyNames.Published])[0], Is.EqualTo(inputValue));
            Assert.That(JsonConvert.DeserializeObject<List<string>>((string)parameterValueSet[PropertyNames.Computed])[0], Is.EqualTo(inputValue));
            Assert.That(JsonConvert.DeserializeObject<List<string>>((string)parameterValueSet[PropertyNames.Manual])[0], Is.EqualTo(inputValue));
            Assert.That(JsonConvert.DeserializeObject<List<string>>((string)parameterValueSet[PropertyNames.Reference])[0], Is.EqualTo(inputValue));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatGettingRevisionsWorks()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostUpdateParameterValueSetTemplate.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var inputAsInnerJson = JsonConvert.ToString("Test Revisions");
            postBody = postBody.Replace("<INNERJSON>", $"[{inputAsInnerJson}]" );

            var jArray1 = this.WebClient.PostDto(iterationUri, postBody);
            Assert.That(jArray1.Count, Is.EqualTo(3));

            var parameterValueSetUri1 = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/3f05483f-66ff-4f21-bc76-45956779f66e/valueSet/72ec3701-bcb5-4bf6-bd78-30fd1b65e3be?revisionFrom=1&revisionTo=2");

            var jArray2 = this.WebClient.GetDto(parameterValueSetUri1);
            Assert.That(jArray2.Count, Is.EqualTo(2));

            var parameterValueSetUri2 = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/3f05483f-66ff-4f21-bc76-45956779f66e/valueSet/72ec3701-bcb5-4bf6-bd78-30fd1b65e3be?revisionFrom=2000-01-01T12:00:00&revisionTo=2120-12-31T12:00:00");

            var jArray3 = this.WebClient.GetDto(parameterValueSetUri2);
            Assert.That(jArray3.Count, Is.EqualTo(2));
        }

        [Test]
        public void VerifyParameterTypeFormulaWorks()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostNewParameterForEachParameterType.json");

            var postBody = this.GetJsonFromFile(postBodyPath);

            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var parameter = jArray.Single(x => (string)x[PropertyNames.Iid] == "3bd5bbdd-6cb5-434d-a7a5-c2c25d128114");

            var parameterValueSetId = (string)(parameter[PropertyNames.ValueSet]![0]);

            postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostUpdateParameterValueSetWithFormula.json");

            postBody = this.GetJsonFromFile(postBodyPath).Replace("<PARAMETERVALUESET>", parameterValueSetId);
            
            jArray = this.WebClient.PostDto(iterationUri, postBody);
        }

        [Test]
        [Category("POST")]
        [TestCase("ccd430e7-2200-4289-8eee-6b78838876a8", new[]{"1.5","2"}, new[]{"1.5","2"}, new[]{"2,5", "2"}, new []{"-"}, new []{"-","-","-"})] //ArrayParameterType
        [TestCase("4b4d36db-21b3-4781-830e-4f56fafea401", new[]{"1"}, new[]{"true"}, new[]{"2"}, new []{"-","-"})] //BooleanParameterType
        [TestCase("07877cdf-060a-4256-bb37-b791fbbe240d", new[]{"2024-01-05T00:00:00"}, new[]{"2024-01-05"}, new[]{"5th January 2024"}, new []{"-","-"})] //DateParameterType
        [TestCase("87d42051-eaa8-4c03-8dd6-4a8caa6d6544", new[]{"2024-01-05T00:00:01"}, new[]{"2024-01-05T00:00:01.0000000Z"}, new[]{"5th January 2024"}, new []{"-","-"})] //DateTimeParameterType
        [TestCase("3bd5bbdd-6cb5-434d-a7a5-c2c25d128114", new[]{"1.5"}, new[]{"1.5"}, new[]{"1,5"}, new []{"-","-"})] //DerivedParameterType with REAL_NUMBER
        [TestCase("2bd23d9d-009a-4f03-9262-f983bc0d0a87", new[]{"TestEnumerationValueDefinitionA"}, new[]{"TestEnumerationValueDefinitionA"}, new[]{"TestEnumerationValueDefinitionA|TestEnumerationValueDefinitionB"}, new[]{"Test Enumeration Value Definition A"}, new []{"-","-"})] //EnumerationParameterType
        [TestCase("d2ad2581-2157-4e0b-924e-150753f598b8", new[]{"00:00:01"}, new[]{ "00:00:01" }, new[]{"0002-01-01T00:00:01.0000000Z"}, new []{"-","-"})] //TimeOfDayParameterType
        [TestCase("d2ad2581-2157-4e0b-924e-150753f598b8", new[] { "00:00:01Z" }, new[] { "00:00:01Z" }, new[] { "0002-01-01T00:00:01.0000000Z" }, new[] { "-", "-" })] //TimeOfDayParameterType
        [TestCase("d2ad2581-2157-4e0b-924e-150753f598b8", new[] { "00:00:01.001Z" }, new[] { "00:00:01.001Z" }, new[] { "0002-01-01T00:00:01.0000000Z" }, new[] { "-", "-" })] //TimeOfDayParameterType
        [TestCase("d2ad2581-2157-4e0b-924e-150753f598b8", new[] { "0001-01-01T00:00:01" }, new[] { "0001-01-01T00:00:01" }, new[] { "0002-01-01T00:00:01.0000000Z" }, new[] { "-", "-" })] //TimeOfDayParameterType
        [TestCase("d2ad2581-2157-4e0b-924e-150753f598b8", new[] { "0001-01-01T00:00:01.001Z" }, new[] { "0001-01-01T00:00:01.001Z" }, new[] { "0002-01-01T00:00:01.0000000Z" }, new[] { "-", "-" })] //TimeOfDayParameterType
        [TestCase("ebd334f7-34f2-47e2-8738-a77f4f36ae51", new[]{"1.5","2"}, new[]{"1.5","2"}, new[]{"2,5", "2"}, new []{"-"}, new []{"-","-","-"})] //CompoundParameterType
        [TestCase("72ec3701-bcb5-4bf6-bd78-30fd1b65e3be", new[]{"Any text"}, new[]{"Any text"}, new[]{" "}, new []{""}, new []{"-","-"})] //TextParameterType
        public void VerifyParameterTypeValidationWorks(string parameterId, string[] validValue, string[] expectedCleanedValue, params string[][] invalidValues)
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostNewParameterForEachParameterType.json");

            var postBody = this.GetJsonFromFile(postBodyPath);

            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var parameterValueSetId = "72ec3701-bcb5-4bf6-bd78-30fd1b65e3be";

            if (parameterId != parameterValueSetId)
            {
                var parameter = jArray.Single(x => (string)x[PropertyNames.Iid] == parameterId);
                parameterValueSetId = (string)(parameter[PropertyNames.ValueSet]![0]);
            }

            var valueSetUpdatePath = this.GetPath("Tests/EngineeringModel/ParameterValueSet/PostUpdateParameterValueSetTemplate.json");

            var initialValueSetContent = this.GetJsonFromFile(valueSetUpdatePath).Replace("72ec3701-bcb5-4bf6-bd78-30fd1b65e3be", parameterValueSetId);

            foreach (var invalidValue in invalidValues)
            {
                postBody = initialValueSetContent.Replace("<INNERJSON>", JsonConvert.SerializeObject(invalidValue));
                Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.Exception.TypeOf<WebException>());
            }

            postBody = initialValueSetContent.Replace("<INNERJSON>", JsonConvert.SerializeObject(validValue));
            jArray = this.WebClient.PostDto(iterationUri, postBody);
            var newValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == parameterValueSetId);

            Assert.Multiple(() =>
            {
                Assert.That(JsonConvert.DeserializeObject<List<string>>((string)newValueSet[PropertyNames.Reference]), Is.EquivalentTo(expectedCleanedValue));
                Assert.That(JsonConvert.DeserializeObject<List<string>>((string)newValueSet[PropertyNames.Manual]), Is.EquivalentTo(expectedCleanedValue));
                Assert.That(JsonConvert.DeserializeObject<List<string>>((string)newValueSet[PropertyNames.Computed]), Is.EquivalentTo(expectedCleanedValue));
                Assert.That(JsonConvert.DeserializeObject<List<string>>((string)newValueSet[PropertyNames.Published]), Is.EquivalentTo(expectedCleanedValue));
                Assert.That(JsonConvert.DeserializeObject<List<string>>((string)newValueSet[PropertyNames.Formula]), Is.EquivalentTo(validValue));
            });
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
            @"a\tb\nc\rd",
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

        private static readonly string[] invalidValueArrayForSingleValue = { "-", "-" };
        private static readonly string[] invalidBooleanValues = { "2","TRUEE" };
    }
}
