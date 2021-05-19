// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementUsageTestFixture.cs" company="RHEA System S.A.">
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

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class ElementUsageTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedElementUsageIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var elementUsageUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/fe9295c5-af99-494e-86ff-e715837806ae/containedElement");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(elementUsageUri);

            // check if there are the only two ElementUsage object 
            Assert.AreEqual(2, jArray.Count);

            ElementUsageTestFixture.VerifyProperties(jArray);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedElementUsageWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var elementUsageUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/fe9295c5-af99-494e-86ff-e715837806ae/containedElement?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(elementUsageUri);

            // check if there are 5 objects
            Assert.AreEqual(5, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ElementDefinition from the result by it's unique id
            ElementDefinitionTestFixture.VerifyProperties(jArray);

            ElementUsageTestFixture.VerifyProperties(jArray);
        }

        /// <summary>
        /// Verifies all properties of the ElementUsage <see cref="JToken"/>
        /// </summary>
        /// <param name="jArray">
        /// The JSON array.
        /// </param>
        public static void VerifyProperties(JArray jArray)
        {
            // get a specific Requirement from the result by it's unique id
            var elementUsage = jArray.Single(
                x => (string)x[PropertyNames.Iid] == "75399754-ee45-4bca-b033-63e2019870d1");

            // verify the amount of returned properties 
            Assert.AreEqual(14, elementUsage.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("75399754-ee45-4bca-b033-63e2019870d1", (string)elementUsage[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)elementUsage[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ElementUsage", (string)elementUsage[PropertyNames.ClassKind]);

            Assert.AreEqual("Test ElementUsage", (string)elementUsage[PropertyNames.Name]);
            Assert.AreEqual("TestElementUsage", (string)elementUsage[PropertyNames.ShortName]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)elementUsage[PropertyNames.Owner]);
            Assert.AreEqual("UNDIRECTED", (string)elementUsage[PropertyNames.InterfaceEnd]);
            Assert.AreEqual(
                "f73860b2-12f0-43e4-b8b2-c81862c0a159",
                (string)elementUsage[PropertyNames.ElementDefinition]);

            var expectedExcludedOptions = new string[] { };
            var excludedOptionsArray = (JArray)elementUsage[PropertyNames.ExcludeOption];
            IList<string> excludedOptions = excludedOptionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedExcludedOptions, excludedOptions);

            var expectedCategories = new string[] { };
            var categoriesArray = (JArray)elementUsage[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)elementUsage[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)elementUsage[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)elementUsage[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            var expectedParameterOverride = new[] { "93f767ed-4d22-45f6-ae97-d1dab0d36e1c" };
            var parameterOverride = (JArray)elementUsage[PropertyNames.ParameterOverride];
            IList<string> p = parameterOverride.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterOverride, p);

            // get a specific Requirement from the result by it's unique id
            elementUsage = jArray.SingleOrDefault(x => (string)x[PropertyNames.Iid] == "f95a1580-e533-4185-b520-208615780afe");

            if (elementUsage != null)
            {
                // verify the amount of returned properties 
            Assert.AreEqual(14, elementUsage.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("f95a1580-e533-4185-b520-208615780afe", (string)elementUsage[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)elementUsage[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ElementUsage", (string)elementUsage[PropertyNames.ClassKind]);

            Assert.AreEqual("Test ElementUsage 2", (string)elementUsage[PropertyNames.Name]);
            Assert.AreEqual("TestElementUsage2", (string)elementUsage[PropertyNames.ShortName]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)elementUsage[PropertyNames.Owner]);
            Assert.AreEqual("UNDIRECTED", (string)elementUsage[PropertyNames.InterfaceEnd]);
            Assert.AreEqual(
                "f73860b2-12f0-43e4-b8b2-c81862c0a159",
                (string)elementUsage[PropertyNames.ElementDefinition]);

            expectedExcludedOptions = new string[] { };
            excludedOptionsArray = (JArray)elementUsage[PropertyNames.ExcludeOption];
            excludedOptions = excludedOptionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedExcludedOptions, excludedOptions);

            expectedCategories = new string[] { };
            categoriesArray = (JArray)elementUsage[PropertyNames.Category];
            categories = categoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            expectedAliases = new string[] { };
            aliasesArray = (JArray)elementUsage[PropertyNames.Alias];
            aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            expectedDefinitions = new string[] { };
            definitionsArray = (JArray)elementUsage[PropertyNames.Definition];
            definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            expectedHyperlinks = new string[] { };
            hyperlinksArray = (JArray)elementUsage[PropertyNames.HyperLink];
            h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            expectedParameterOverride = new string[] { };
            parameterOverride = (JArray)elementUsage[PropertyNames.ParameterOverride];
            p = parameterOverride.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterOverride, p);
            }            
        }
    }
}
