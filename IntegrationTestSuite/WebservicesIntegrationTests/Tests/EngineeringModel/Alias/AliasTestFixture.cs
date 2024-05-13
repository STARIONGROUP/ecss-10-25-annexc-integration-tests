// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AliasTestFixture.cs" company="Starion Group S.A.">
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
    public class AliasTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("POST")]
        public void VerifyThatAnOptionAliasCanBeCreatedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Alias/PostNewAlias.json");

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

            var expectedLogEntries = new[] { "4e2375eb-8e37-4df2-9c7b-dd896683a891" };
            var logEntriesArray = (JArray)engineeringModel[PropertyNames.LogEntry];
            IList<string> logEntries = logEntriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedLogEntries, logEntries);

            var expectedCommonFileStores = new[] { "8e5ca9cc-3da8-4e66-9172-7c3b2464a59c" };
            var commonFileStoresArray = (JArray)engineeringModel[PropertyNames.CommonFileStore];
            IList<string> commonFileStores = commonFileStoresArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCommonFileStores, commonFileStores);

            Assert.AreEqual("EngineeringModel", (string)engineeringModel[PropertyNames.ClassKind]);
            Assert.AreEqual("116f6253-89bb-47d4-aa24-d11d197e43c9", (string)engineeringModel[PropertyNames.EngineeringModelSetup]);
            Assert.AreEqual("9ec982e4-ef72-4953-aa85-b158a95d8d56", (string)engineeringModel[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)engineeringModel[PropertyNames.RevisionNumber]);

            // Get a specific Option from the result by it's unique id
            var option = jArray.Single(x => (string)x[PropertyNames.Iid] == "bebcc9f4-ff20-4569-bbf6-d1acf27a8107");

            // verify the amount of returned properties of Option
            Assert.AreEqual(10, option.Children().Count());

            // assert that the properties of Option are what is expected
            Assert.AreEqual("bebcc9f4-ff20-4569-bbf6-d1acf27a8107", (string)option[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)option[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Option", (string)option[PropertyNames.ClassKind]);

            Assert.AreEqual("Test Option", (string)option[PropertyNames.Name]);
            Assert.AreEqual("TestOption", (string)option[PropertyNames.ShortName]);

            var expectedNestedElements = new string[] { };
            var nestedElementsArray = (JArray)option[PropertyNames.NestedElement];
            IList<string> nestedElements = nestedElementsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedNestedElements, nestedElements);

            var expectedCategories = new string[] { };
            var categoriesArray = (JArray)option[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] { "4a506a69-5453-475a-b8d9-142331bd30ea" };
            var aliasesArray = (JArray)option[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)option[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperLinks = new string[] { };
            var hyperLinksArray = (JArray)option[PropertyNames.HyperLink];
            IList<string> hyperLinks = hyperLinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperLinks, hyperLinks);

            // Get the added Alias from the result by it's unique id
            var alias = jArray.Single(x => (string)x[PropertyNames.Iid] == "4a506a69-5453-475a-b8d9-142331bd30ea");

            // Verify the amount of returned properties 
            Assert.AreEqual(6, alias.Children().Count());

            // Assert that the properties are equal to expected values      
            Assert.AreEqual("Alias", (string)alias[PropertyNames.ClassKind]);
            Assert.AreEqual("test alias", (string)alias[PropertyNames.Content]);
            Assert.AreEqual("4a506a69-5453-475a-b8d9-142331bd30ea", (string)alias[PropertyNames.Iid]);
            Assert.AreEqual(false, (bool)alias[PropertyNames.IsSynonym]);
            Assert.AreEqual("nl", (string)alias[PropertyNames.LanguageCode]);
            Assert.AreEqual(2, (int)alias[PropertyNames.RevisionNumber]);        
        }
    }
}
