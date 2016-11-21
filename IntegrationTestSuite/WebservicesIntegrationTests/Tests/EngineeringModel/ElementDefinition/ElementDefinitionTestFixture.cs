// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementDefinitionTestFixture.cs" company="RHEA System">
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
    public class ElementDefinitionTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the ElementDefinition objects are returned from the data-source and that the 
        /// values of the ElementDefinition properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedElementDefinitionIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var elementDefinitionUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(elementDefinitionUri);

            //check if there is the only one ElementDefinition object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");

            ElementDefinitionTestFixture.VerifyProperties(elementDefinition);
        }

        [Test]
        public void VerifyThatExpectedElementDefinitionWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var elementDefinitionUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(elementDefinitionUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific ElementDefinition from the result by it's unique id
            var elementDefinition =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "f73860b2-12f0-43e4-b8b2-c81862c0a159");
            ElementDefinitionTestFixture.VerifyProperties(elementDefinition);
        }

        /// <summary>
        /// Verifies all properties of the ElementDefinition <see cref="JToken"/>
        /// </summary>
        /// <param name="elementDefinition">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ElementDefinition object
        /// </param>
        public static void VerifyProperties(JToken elementDefinition)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(14, elementDefinition.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("f73860b2-12f0-43e4-b8b2-c81862c0a159",
                (string) elementDefinition[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) elementDefinition[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ElementDefinition", (string) elementDefinition[PropertyNames.ClassKind]);

            Assert.AreEqual("Test Element Definition", (string) elementDefinition[PropertyNames.Name]);
            Assert.AreEqual("TestElementDefinition", (string) elementDefinition[PropertyNames.ShortName]);

            var expectedContainedElements = new string[]
            {
                "75399754-ee45-4bca-b033-63e2019870d1"
            };
            var containedElementsArray = (JArray) elementDefinition[PropertyNames.ContainedElement];
            IList<string> containedElements = containedElementsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedContainedElements, containedElements);

            var expectedParameters = new string[]
            {
                "6c5aff74-f983-4aa8-a9d6-293b3429307c"
            };
            var parametersArray = (JArray) elementDefinition[PropertyNames.Parameter];
            IList<string> parameters = parametersArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedParameters, parameters);

            var expectedParameterGroups = new string[]
            {
                "b739b3c6-9cc0-4e64-9cc4-ef7463edf559"
            };
            var parameterGroupsArray = (JArray) elementDefinition[PropertyNames.ParameterGroup];
            IList<string> parameterGroups = parameterGroupsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterGroups, parameterGroups);

            var expectedreferencedElements = new string[] {};
            var referencedElementsArray = (JArray) elementDefinition[PropertyNames.ReferencedElement];
            IList<string> referencedElements = referencedElementsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedreferencedElements, referencedElements);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) elementDefinition[PropertyNames.Owner]);

            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) elementDefinition[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) elementDefinition[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) elementDefinition[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) elementDefinition[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}