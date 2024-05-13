// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementDefinitionTestFixture.cs" company="Starion Group S.A.">
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

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class ElementDefinitionTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("POST")]
        public void VerifyThatADomainExpertUserCanCreateAnElementDefinition()
        {
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ElementDefinition/PostNewElementDefinitionForDomainExpertUser.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            Assert.AreEqual(4, jArray.Count);

            var engineeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int)engineeringModel[PropertyNames.RevisionNumber]);

            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            Assert.AreEqual(2, (int)iteration[PropertyNames.RevisionNumber]);

            var elementDefinition = jArray.Single(x => (string)x[PropertyNames.Iid] == "f959dc33-58ff-4b6f-a3b0-d265690b4085");
            Assert.AreEqual(2, (int)elementDefinition[PropertyNames.RevisionNumber]);
        }
        
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedElementDefinitionIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var elementDefinitionUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(elementDefinitionUri);

            // check if there are only two ElementDefinition object 
            Assert.AreEqual(2, jArray.Count);

            ElementDefinitionTestFixture.VerifyProperties(jArray);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedElementDefinitionWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var elementDefinitionUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(elementDefinitionUri);

            // check if there are 4 objects
            Assert.AreEqual(4, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            ElementDefinitionTestFixture.VerifyProperties(jArray);
        }

        // The given test is prepared for a development server 
        // and is not eligible for the current data
        [Test]
        [Category("POST")]
        public void VerifyThatCategoryDeletionFromElementDefinitionCanBeDoneFromWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ElementDefinition/POSTDeleteCategory.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            // check if there are 2 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific EngineeringModel from the result by it's unique id
            var engineeeringModel = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int)engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");
            Assert.AreEqual(2, (int)elementDefinition[PropertyNames.RevisionNumber]);
            var expectedCategories = new string[] { };
            var categoriesArray = (JArray)elementDefinition[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatParameterDeletionAsInstanceFromElementDefinitionCanBeDoneFromWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ElementDefinition/PostDeleteParameterAsInstance.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            // check if there are 2 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific EngineeringModel from the result by it's unique id
            var engineeeringModel = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int)engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");
            Assert.AreEqual(2, (int)elementDefinition[PropertyNames.RevisionNumber]);
            var expectedParameters = new[]
                                         {
                                             "6c5aff74-f983-4aa8-a9d6-293b3429307c"
                                         };
            var parametersArray = (JArray)elementDefinition[PropertyNames.Parameter];
            IList<string> parameters = parametersArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedParameters, parameters);

            // define the URI on which to perform a GET request 
            var parameterUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/3f05483f-66ff-4f21-bc76-45956779f66e");
            Assert.That(() => this.WebClient.GetDto(parameterUri), Throws.Exception.TypeOf<System.Net.WebException>());
        }

        [Test]
        [Category("POST")]
        public void VerifyThatParameterDeletionAsPropertyFromElementDefinitionCanBeDoneFromWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ElementDefinition/PostDeleteParameterAsProperty.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);
            
            // check if there are 2 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific EngineeringModel from the result by it's unique id
            var engineeeringModel = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int)engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");
            Assert.AreEqual(2, (int)elementDefinition[PropertyNames.RevisionNumber]);
            var expectedParameters = new[]
                                         {
                                             "6c5aff74-f983-4aa8-a9d6-293b3429307c"
                                         };
            var parametersArray = (JArray)elementDefinition[PropertyNames.Parameter];
            IList<string> parameters = parametersArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedParameters, parameters);

            // define the URI on which to perform a GET request 
            var parameterUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/parameter/3f05483f-66ff-4f21-bc76-45956779f66e");
            Assert.That(() => this.WebClient.GetDto(parameterUri), Throws.Exception.TypeOf<System.Net.WebException>());
        }

        [Test]
        [Category("POST")]
        public void VerifyThatAnElementDefinitionCanBeCreatedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ElementDefinition/PostNewElementDefinition.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            // Verify the amount of returned properties of the EngineeringModel
            Assert.AreEqual(8, engineeringModel.Children().Count());

            // Assert the properties of EngineeringModel have expected values
            var expectedIterations = new[] { "e163c5ad-f32b-4387-b805-f4b34600bc2c" };
            var iterationsArray = (JArray)engineeringModel[PropertyNames.Iteration];
            IList<string> iterations = iterationsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedIterations, iterations);

            var logEntriesArray = (JArray)engineeringModel[PropertyNames.LogEntry];
            IList<string> logEntries = logEntriesArray.Select(x => (string)x).ToList();
            Assert.AreEqual(2, logEntries.Count);

            var expectedCommonFileStores = new[] { "8e5ca9cc-3da8-4e66-9172-7c3b2464a59c" };
            var commonFileStoresArray = (JArray)engineeringModel[PropertyNames.CommonFileStore];
            IList<string> commonFileStores = commonFileStoresArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCommonFileStores, commonFileStores);

            Assert.AreEqual("EngineeringModel", (string)engineeringModel[PropertyNames.ClassKind]);
            Assert.AreEqual("116f6253-89bb-47d4-aa24-d11d197e43c9", (string)engineeringModel[PropertyNames.EngineeringModelSetup]);
            Assert.AreEqual("9ec982e4-ef72-4953-aa85-b158a95d8d56", (string)engineeringModel[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)engineeringModel[PropertyNames.RevisionNumber]);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition = jArray.Single(x => (string)x[PropertyNames.Iid] == "f959dc33-58ff-4b6f-a3b0-d265690b4084");

            // verify the amount of returned properties 
            Assert.AreEqual(14, elementDefinition.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("f959dc33-58ff-4b6f-a3b0-d265690b4084", (string)elementDefinition[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)elementDefinition[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ElementDefinition", (string)elementDefinition[PropertyNames.ClassKind]);
            Assert.AreEqual("Test Element Definition", (string)elementDefinition[PropertyNames.Name]);
            Assert.AreEqual("TestElementDefinition", (string)elementDefinition[PropertyNames.ShortName]);

            var expectedContainedElements = new string[] { };
            var containedElementsArray = (JArray)elementDefinition[PropertyNames.ContainedElement];
            IList<string> containedElements = containedElementsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedContainedElements, containedElements);

            var expectedParameters = new string[] { };
            var parametersArray = (JArray)elementDefinition[PropertyNames.Parameter];
            IList<string> parameters = parametersArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedParameters, parameters);

            var expectedParameterGroups = new string[] { };
            var parameterGroupsArray = (JArray)elementDefinition[PropertyNames.ParameterGroup];
            IList<string> parameterGroups = parameterGroupsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterGroups, parameterGroups);

            var expectedreferencedElements = new string[] { };
            var referencedElementsArray = (JArray)elementDefinition[PropertyNames.ReferencedElement];
            IList<string> referencedElements = referencedElementsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedreferencedElements, referencedElements);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)elementDefinition[PropertyNames.Owner]);

            var expectedCategories = new string[] { };
            var categoriesArray = (JArray)elementDefinition[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)elementDefinition[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)elementDefinition[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)elementDefinition[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }

        /// <summary>
        /// Verifies all properties of the ElementDefinitions
        /// </summary>
        /// <param name="jArray">
        /// The j array.
        /// </param>
        public static void VerifyProperties(JArray jArray)
        {
            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition = jArray.SingleOrDefault(
                x => (string)x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");
            if (elementDefinition != null)
            {
                // verify the amount of returned properties 
                Assert.AreEqual(14, elementDefinition.Children().Count());

                // assert that the properties are what is expected
                Assert.AreEqual("f73860b2-12f0-43e4-b8b2-c81862c0a159", (string)elementDefinition[PropertyNames.Iid]);
                Assert.AreEqual(1, (int)elementDefinition[PropertyNames.RevisionNumber]);
                Assert.AreEqual("ElementDefinition", (string)elementDefinition[PropertyNames.ClassKind]);

                Assert.AreEqual("Test Element Definition", (string)elementDefinition[PropertyNames.Name]);
                Assert.AreEqual("TestElementDefinition", (string)elementDefinition[PropertyNames.ShortName]);

                var expectedContainedElements = new string[] { };
                var containedElementsArray = (JArray)elementDefinition[PropertyNames.ContainedElement];
                IList<string> containedElements = containedElementsArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedContainedElements, containedElements);

                var expectedParameters = new[]
                                             {
                                                 "6c5aff74-f983-4aa8-a9d6-293b3429307c",
                                                 "3f05483f-66ff-4f21-bc76-45956779f66e"
                                             };
                var parametersArray = (JArray)elementDefinition[PropertyNames.Parameter];
                IList<string> parameters = parametersArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedParameters, parameters);

                var expectedParameterGroups = new[] { "b739b3c6-9cc0-4e64-9cc4-ef7463edf559" };
                var parameterGroupsArray = (JArray)elementDefinition[PropertyNames.ParameterGroup];
                IList<string> parameterGroups = parameterGroupsArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedParameterGroups, parameterGroups);

                var expectedreferencedElements = new string[] { };
                var referencedElementsArray = (JArray)elementDefinition[PropertyNames.ReferencedElement];
                IList<string> referencedElements = referencedElementsArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedreferencedElements, referencedElements);

                Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)elementDefinition[PropertyNames.Owner]);

                var expectedCategories = new[] { "cf059b19-235c-48be-87a3-9a8942c8e3e0" };
                var categoriesArray = (JArray)elementDefinition[PropertyNames.Category];
                IList<string> categories = categoriesArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedCategories, categories);

                var expectedAliases = new string[] { };
                var aliasesArray = (JArray)elementDefinition[PropertyNames.Alias];
                IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedAliases, aliases);

                var expectedDefinitions = new string[] { };
                var definitionsArray = (JArray)elementDefinition[PropertyNames.Definition];
                IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

                var expectedHyperlinks = new string[] { };
                var hyperlinksArray = (JArray)elementDefinition[PropertyNames.HyperLink];
                IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedHyperlinks, h);
            }

            // get a specific ElementDefinition from the result by it's unique id
            elementDefinition = jArray.SingleOrDefault(
                x => (string)x[PropertyNames.Iid] == "fe9295c5-af99-494e-86ff-e715837806ae");

            if (elementDefinition != null)
            {
                // verify the amount of returned properties 
                Assert.AreEqual(14, elementDefinition.Children().Count());

                // assert that the properties are what is expected
                Assert.AreEqual("fe9295c5-af99-494e-86ff-e715837806ae", (string)elementDefinition[PropertyNames.Iid]);
                Assert.AreEqual(1, (int)elementDefinition[PropertyNames.RevisionNumber]);
                Assert.AreEqual("ElementDefinition", (string)elementDefinition[PropertyNames.ClassKind]);

                Assert.AreEqual("Test 2 Element Definition", (string)elementDefinition[PropertyNames.Name]);
                Assert.AreEqual("TestTwoElementDefinition", (string)elementDefinition[PropertyNames.ShortName]);

                var expectedContainedElements = new[]
                                                {
                                                    "75399754-ee45-4bca-b033-63e2019870d1",
                                                    "f95a1580-e533-4185-b520-208615780afe"
                                                };
                var containedElementsArray = (JArray)elementDefinition[PropertyNames.ContainedElement];
                var containedElements = containedElementsArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedContainedElements, containedElements);

                var expectedParameters = new string[] { };
                var parametersArray = (JArray)elementDefinition[PropertyNames.Parameter];
                var parameters = parametersArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedParameters, parameters);

                var expectedParameterGroups = new string[] { };
                var parameterGroupsArray = (JArray)elementDefinition[PropertyNames.ParameterGroup];
                var parameterGroups = parameterGroupsArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedParameterGroups, parameterGroups);

                var expectedreferencedElements = new string[] { };
                var referencedElementsArray = (JArray)elementDefinition[PropertyNames.ReferencedElement];
                var referencedElements = referencedElementsArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedreferencedElements, referencedElements);

                Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)elementDefinition[PropertyNames.Owner]);

                var expectedCategories = new string[] { };
                var categoriesArray = (JArray)elementDefinition[PropertyNames.Category];
                var categories = categoriesArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedCategories, categories);

                var expectedAliases = new string[] { };
                var aliasesArray = (JArray)elementDefinition[PropertyNames.Alias];
                var aliases = aliasesArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedAliases, aliases);

                var expectedDefinitions = new string[] { };
                var definitionsArray = (JArray)elementDefinition[PropertyNames.Definition];
                var definitions = definitionsArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

                var expectedHyperlinks = new string[] { };
                var hyperlinksArray = (JArray)elementDefinition[PropertyNames.HyperLink];
                var h = hyperlinksArray.Select(x => (string)x).ToList();
                CollectionAssert.AreEquivalent(expectedHyperlinks, h);
            }
        }
    }
}
