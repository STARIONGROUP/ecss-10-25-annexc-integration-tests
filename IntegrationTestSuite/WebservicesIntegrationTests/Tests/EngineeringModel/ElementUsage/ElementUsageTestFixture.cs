// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementUsageTestFixture.cs" company="RHEA System">
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
    using NUnit.Framework;
    using Newtonsoft.Json.Linq;

    [TestFixture]
    public class ElementUsageTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the ElementUsage objects are returned from the data-source and that the 
        /// values of the ElementUsage properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedElementUsageIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var elementUsageUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/containedElement"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(elementUsageUri);

            //check if there is the only one ElementUsage object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ElementUsage from the result by it's unique id
            var elementUsage =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "75399754-ee45-4bca-b033-63e2019870d1");

            ElementUsageTestFixture.VerifyProperties(elementUsage);
        }

        [Test]
        public void VerifyThatExpectedElementUsageWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var elementUsageUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/containedElement?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(elementUsageUri);

            //check if there are 4 objects
            Assert.AreEqual(4, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");
            ElementDefinitionTestFixture.VerifyProperties(elementDefinition);

            // get a specific ElementUsage from the result by it's unique id
            var elementUsage =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "75399754-ee45-4bca-b033-63e2019870d1");
            ElementUsageTestFixture.VerifyProperties(elementUsage);
        }

        /// <summary>
        /// Verifies all properties of the ElementUsage <see cref="JToken"/>
        /// </summary>
        /// <param name="elementUsage">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ElementUsage object
        /// </param>
        public static void VerifyProperties(JToken elementUsage)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(14, elementUsage.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("75399754-ee45-4bca-b033-63e2019870d1",
                (string)elementUsage[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)elementUsage[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ElementUsage", (string)elementUsage[PropertyNames.ClassKind]);

            Assert.AreEqual("Test ElementUsage", (string)elementUsage[PropertyNames.Name]);
            Assert.AreEqual("TestElementUsage", (string)elementUsage[PropertyNames.ShortName]);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)elementUsage[PropertyNames.Owner]);
            Assert.AreEqual("UNDIRECTED", (string)elementUsage[PropertyNames.InterfaceEnd]);
            Assert.AreEqual("f73860b2-12f0-43e4-b8b2-c81862c0a159", (string)elementUsage[PropertyNames.ElementDefinition]);

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

            var expectedParameterOverride = new string[]
            {
                "93f767ed-4d22-45f6-ae97-d1dab0d36e1c"
            };
            var parameterOverride = (JArray)elementUsage[PropertyNames.ParameterOverride];
            IList<string> p = parameterOverride.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterOverride, p);
        }
    }
}
