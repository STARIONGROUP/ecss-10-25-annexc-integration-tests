// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionTestFixture.cs" company="RHEA System">
//
//   Copyright 2016-2020 RHEA System 
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
    public class OptionTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        public void VerifyThatNewOptionCanBeCreatedWithWebApi()
        {
            var iterationUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Option/PostNewOption.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            Assert.AreEqual(2, (int) iteration[PropertyNames.RevisionNumber]);
            var expectedOptions = new List<OrderedItem> { new OrderedItem(1, "bebcc9f4-ff20-4569-bbf6-d1acf27a8107"), new OrderedItem(2, "e90e4bcd-6e17-4b75-80fb-6cded78bed57") };
            var optionsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(iteration[PropertyNames.Option].ToString());
            CollectionAssert.AreEquivalent(expectedOptions, optionsArray);

            var option = jArray.Single(x => (string) x[PropertyNames.Iid] == "e90e4bcd-6e17-4b75-80fb-6cded78bed57");
            Assert.AreEqual(2, (int) option[PropertyNames.RevisionNumber]);
            Assert.AreEqual("test option", (string) option[PropertyNames.Name]);
            Assert.AreEqual("testoption", (string) option[PropertyNames.ShortName]);
        }

        [Ignore("Reordering options is not possible in Webservice")]
        public void VerifyThatOptionsCanBeReorderedWithWebApi()
        {
            var iterationUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Option/PostNewOption.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            postBodyPath = this.GetPath("Tests/EngineeringModel/Option/PostReorderOptions.json");
            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            Assert.AreEqual(3, (int) iteration[PropertyNames.RevisionNumber]);

            var expectedOptions = new List<OrderedItem> { new OrderedItem(3, "e90e4bcd-6e17-4b75-80fb-6cded78bed57"), new OrderedItem(4, "bebcc9f4-ff20-4569-bbf6-d1acf27a8107") };
            var optionsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(iteration[PropertyNames.Option].ToString());
            CollectionAssert.AreEquivalent(expectedOptions, optionsArray);
        }

        /// <summary>
        /// Verification that the Option objects are returned from the data-source and that the 
        /// values of the Option properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedOptionIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var optionUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/option"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(optionUri);

            //check if there is the only one Option object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific Option from the result by it's unique id
            var option =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "bebcc9f4-ff20-4569-bbf6-d1acf27a8107");

            VerifyProperties(option);
        }

        [Test]
        public void VerifyThatExpectedOptionWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var optionUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/option?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(optionUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");

            IterationTestFixture.VerifyProperties(iteration);

            // get a specific Option from the result by it's unique id
            var option =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "bebcc9f4-ff20-4569-bbf6-d1acf27a8107");

            VerifyProperties(option);
        }

        /// <summary>
        /// Verifies all properties of the Option <see cref="JToken"/>
        /// </summary>
        /// <param name="option">
        /// The <see cref="JToken"/> that contains the properties of
        /// the Option object
        /// </param>
        public static void VerifyProperties(JToken option)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(10, option.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("bebcc9f4-ff20-4569-bbf6-d1acf27a8107", (string) option[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) option[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Option", (string) option[PropertyNames.ClassKind]);

            Assert.AreEqual("Test Option", (string) option[PropertyNames.Name]);
            Assert.AreEqual("TestOption", (string) option[PropertyNames.ShortName]);

            var expectedNestedElements = new string[] { };
            var nestedElementsArray = (JArray) option[PropertyNames.NestedElement];
            IList<string> nestedElements = nestedElementsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedNestedElements, nestedElements);

            var expectedCategories = new string[] { };
            var categoriesArray = (JArray) option[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray) option["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray) option["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray) option["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
