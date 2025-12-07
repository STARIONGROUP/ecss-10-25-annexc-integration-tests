// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionTestFixture.cs" company="Starion Group S.A.">
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
    public class OptionTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("POST")]
        public void VerifyThatNewOptionCanBeCreatedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Option/PostNewOption.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int) engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            Assert.That((int) iteration[PropertyNames.RevisionNumber], Is.EqualTo(2));
            var expectedOptions = new List<OrderedItem> { new OrderedItem(1, "bebcc9f4-ff20-4569-bbf6-d1acf27a8107"), new OrderedItem(2, "e90e4bcd-6e17-4b75-80fb-6cded78bed57") };
            var optionsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(iteration[PropertyNames.Option].ToString());
            Assert.That(optionsArray, Is.EquivalentTo(expectedOptions));

            var option = jArray.Single(x => (string) x[PropertyNames.Iid] == "e90e4bcd-6e17-4b75-80fb-6cded78bed57");
            Assert.That((int) option[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string) option[PropertyNames.Name], Is.EqualTo("test option"));
            Assert.That((string) option[PropertyNames.ShortName], Is.EqualTo("testoption"));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatNewOptionCanBeCreatedWithWebApiWhenOptionDependentParameterAlreadyExists()
        {
            //Post new optiondependent parameter
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Parameter/PostNewOptionDependentParameter.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            Assert.That((int)engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            //Post new option
            postBodyPath = this.GetPath("Tests/EngineeringModel/Option/PostNewOption.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            engineeeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(3));

            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            Assert.That((int)iteration[PropertyNames.RevisionNumber], Is.EqualTo(3));
            var expectedOptions = new List<OrderedItem> { new OrderedItem(1, "bebcc9f4-ff20-4569-bbf6-d1acf27a8107"), new OrderedItem(2, "e90e4bcd-6e17-4b75-80fb-6cded78bed57") };
            var optionsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(iteration[PropertyNames.Option].ToString());
            Assert.That(optionsArray, Is.EquivalentTo(expectedOptions));

            var option = jArray.Single(x => (string)x[PropertyNames.Iid] == "e90e4bcd-6e17-4b75-80fb-6cded78bed57");
            Assert.That((int)option[PropertyNames.RevisionNumber], Is.EqualTo(3));
            Assert.That((string)option[PropertyNames.Name], Is.EqualTo("test option"));
            Assert.That((string)option[PropertyNames.ShortName], Is.EqualTo("testoption"));
        }


        [Test]
        [Category("POST")]
        public void VerifyThatOptionsCanBeReorderedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Option/PostNewOption.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            postBodyPath = this.GetPath("Tests/EngineeringModel/Option/PostReorderOptions.json");
            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            Assert.That((int)iteration[PropertyNames.RevisionNumber], Is.EqualTo(3));

            var expectedOptions = new List<OrderedItem> { new OrderedItem(3, "e90e4bcd-6e17-4b75-80fb-6cded78bed57"), new OrderedItem(4, "bebcc9f4-ff20-4569-bbf6-d1acf27a8107") };
            var optionsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(iteration[PropertyNames.Option].ToString());
            Assert.That(optionsArray, Is.EquivalentTo(expectedOptions));
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedOptionIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var optionUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/option");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(optionUri);

            //check if there is the only one Option object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific Option from the result by it's unique id
            var option = jArray.Single(x => (string) x[PropertyNames.Iid] == "bebcc9f4-ff20-4569-bbf6-d1acf27a8107");

            VerifyProperties(option);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedOptionWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var optionUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/option?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(optionUri);

            //check if there are 3 objects
            Assert.That(jArray.Count, Is.EqualTo(3));

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
            Assert.That(option.Children().Count(), Is.EqualTo(10));

            // assert that the properties are what is expected
            Assert.That((string)option[PropertyNames.Iid], Is.EqualTo("bebcc9f4-ff20-4569-bbf6-d1acf27a8107"));
            Assert.That((int)option[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)option[PropertyNames.ClassKind], Is.EqualTo("Option"));

            Assert.That((string)option[PropertyNames.Name], Is.EqualTo("Test Option"));
            Assert.That((string)option[PropertyNames.ShortName], Is.EqualTo("TestOption"));

            var expectedNestedElements = new string[] { };
            var nestedElementsArray = (JArray) option[PropertyNames.NestedElement];
            IList<string> nestedElements = nestedElementsArray.Select(x => (string) x).ToList();
            Assert.That(nestedElements, Is.EquivalentTo(expectedNestedElements));

            var expectedCategories = new string[] { };
            var categoriesArray = (JArray) option[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            Assert.That(categories, Is.EquivalentTo(expectedCategories));

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray) option["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray) option["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray) option["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));
        }
    }
}
