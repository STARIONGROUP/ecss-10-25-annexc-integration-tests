// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AliasTestFixture.cs" company="Starion Group S.A.">
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
            Assert.That(iterations, Is.EquivalentTo(expectedIterations));

            var expectedLogEntries = new[] { "4e2375eb-8e37-4df2-9c7b-dd896683a891" };
            var logEntriesArray = (JArray)engineeringModel[PropertyNames.LogEntry];
            IList<string> logEntries = logEntriesArray.Select(x => (string)x).ToList();
            Assert.That(logEntries, Is.EquivalentTo(expectedLogEntries));

            var expectedCommonFileStores = new[] { "8e5ca9cc-3da8-4e66-9172-7c3b2464a59c" };
            var commonFileStoresArray = (JArray)engineeringModel[PropertyNames.CommonFileStore];
            IList<string> commonFileStores = commonFileStoresArray.Select(x => (string)x).ToList();
            Assert.That(commonFileStores, Is.EquivalentTo(expectedCommonFileStores));

            Assert.That((string)engineeringModel[PropertyNames.ClassKind], Is.EqualTo("EngineeringModel"));
            Assert.That((string)engineeringModel[PropertyNames.EngineeringModelSetup], Is.EqualTo("116f6253-89bb-47d4-aa24-d11d197e43c9"));
            Assert.That((string)engineeringModel[PropertyNames.Iid], Is.EqualTo("9ec982e4-ef72-4953-aa85-b158a95d8d56"));
            Assert.That((int)engineeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // Get a specific Option from the result by it's unique id
            var option = jArray.Single(x => (string)x[PropertyNames.Iid] == "bebcc9f4-ff20-4569-bbf6-d1acf27a8107");

            // verify the amount of returned properties of Option
            Assert.That(option.Children().Count(), Is.EqualTo(10));

            // assert that the properties of Option are what is expected
            Assert.That((string)option[PropertyNames.Iid], Is.EqualTo("bebcc9f4-ff20-4569-bbf6-d1acf27a8107"));
            Assert.That((int)option[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string)option[PropertyNames.ClassKind], Is.EqualTo("Option"));

            Assert.That((string)option[PropertyNames.Name], Is.EqualTo("Test Option"));
            Assert.That((string)option[PropertyNames.ShortName], Is.EqualTo("TestOption"));

            var expectedNestedElements = new string[] { };
            var nestedElementsArray = (JArray)option[PropertyNames.NestedElement];
            IList<string> nestedElements = nestedElementsArray.Select(x => (string)x).ToList();
            Assert.That(nestedElements, Is.EquivalentTo(expectedNestedElements));

            var expectedCategories = new string[] { };
            var categoriesArray = (JArray)option[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string)x).ToList();
            Assert.That(categories, Is.EquivalentTo(expectedCategories));

            var expectedAliases = new string[] { "4a506a69-5453-475a-b8d9-142331bd30ea" };
            var aliasesArray = (JArray)option[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)option[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperLinks = new string[] { };
            var hyperLinksArray = (JArray)option[PropertyNames.HyperLink];
            IList<string> hyperLinks = hyperLinksArray.Select(x => (string)x).ToList();
            Assert.That(hyperLinks, Is.EquivalentTo(expectedHyperLinks));

            // Get the added Alias from the result by it's unique id
            var alias = jArray.Single(x => (string)x[PropertyNames.Iid] == "4a506a69-5453-475a-b8d9-142331bd30ea");

            // Verify the amount of returned properties 
            Assert.That(alias.Children().Count(), Is.EqualTo(6));

            // Assert that the properties are equal to expected values      
            Assert.That((string)alias[PropertyNames.ClassKind], Is.EqualTo("Alias"));
            Assert.That((string)alias[PropertyNames.Content], Is.EqualTo("test alias"));
            Assert.That((string)alias[PropertyNames.Iid], Is.EqualTo("4a506a69-5453-475a-b8d9-142331bd30ea"));
            Assert.That((bool)alias[PropertyNames.IsSynonym], Is.EqualTo(false));
            Assert.That((string)alias[PropertyNames.LanguageCode], Is.EqualTo("nl"));
            Assert.That((int)alias[PropertyNames.RevisionNumber], Is.EqualTo(2));        
        }
    }
}
