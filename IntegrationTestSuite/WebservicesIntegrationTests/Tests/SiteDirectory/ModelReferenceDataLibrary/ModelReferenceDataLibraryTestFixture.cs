// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelReferenceDataLibraryTestFixture.cs" company="RHEA System">
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
    using Newtonsoft.Json;

    [TestFixture]
    public class ModelReferenceDataLibraryTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the ModelReferenceDataLibrary objects are returned from the data-source and that the 
        /// values of the ModelReferenceDataLibrary properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedModelReferenceDataLibraryeIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var modelReferenceDataLibraryUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model/116f6253-89bb-47d4-aa24-d11d197e43c9/requiredRdl"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(modelReferenceDataLibraryUri);

            //check if there is the only one ModelReferenceDataLibrary object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ModelReferenceDataLibrary from the result by it's unique id
            var modelReferenceDataLibrary =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "3483f2b5-ea29-45cc-8a46-f5f598558fc3");

            ModelReferenceDataLibraryTestFixture.VerifyProperties(modelReferenceDataLibrary);
        }

        [Test]
        public void VerifyThatExpectedModelReferenceDataLibraryWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var modelReferenceDataLibraryUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model/116f6253-89bb-47d4-aa24-d11d197e43c9/requiredRdl?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(modelReferenceDataLibraryUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific EngineeringModelSetup from the result by it's unique id
            var engineeringModelSetup =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "116f6253-89bb-47d4-aa24-d11d197e43c9");
            EngineeringModelSetupTestFixture.VerifyProperties(engineeringModelSetup);

            // get a specific ModelReferenceDataLibrary from the result by it's unique id
            var modelReferenceDataLibrary =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "3483f2b5-ea29-45cc-8a46-f5f598558fc3");
            ModelReferenceDataLibraryTestFixture.VerifyProperties(modelReferenceDataLibrary);
        }

        /// <summary>
        /// Verifies all properties of the ModelReferenceDataLibrary <see cref="JToken"/>
        /// </summary>
        /// <param name="modelReferenceDataLibrary">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ModelReferenceDataLibrary object
        /// </param>
        public static void VerifyProperties(JToken modelReferenceDataLibrary)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(21, modelReferenceDataLibrary.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("3483f2b5-ea29-45cc-8a46-f5f598558fc3",
                (string) modelReferenceDataLibrary[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) modelReferenceDataLibrary[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ModelReferenceDataLibrary", (string) modelReferenceDataLibrary[PropertyNames.ClassKind]);

            Assert.AreEqual("Test Model Reference Data Library", (string) modelReferenceDataLibrary[PropertyNames.Name]);
            Assert.AreEqual("TestModelReferenceDataLibrary", (string) modelReferenceDataLibrary[PropertyNames.ShortName]);

            Assert.AreEqual("c454c687-ba3e-44c4-86bc-44544b2c7880",
                (string) modelReferenceDataLibrary[PropertyNames.RequiredRdl]);

            var expectedDefinedCategories = new string[] {};
            var definedCategoriesArray = (JArray) modelReferenceDataLibrary[PropertyNames.DefinedCategory];
            IList<string> definedCategories = definedCategoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinedCategories, definedCategories);

            var expectedParameterTypes = new string[] {};
            var parameterTypesArray = (JArray) modelReferenceDataLibrary[PropertyNames.ParameterType];
            IList<string> parameterTypes = parameterTypesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterTypes, parameterTypes);

            var expectedBaseQuantityKinds = new string[] {};
            var baseQuantityKindsArray = (JArray) modelReferenceDataLibrary[PropertyNames.BaseQuantityKind];
            IList<string> baseQuantityKinds = baseQuantityKindsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedBaseQuantityKinds, baseQuantityKinds);

            var expectedScales = new string[] {};
            var scalesArray = (JArray) modelReferenceDataLibrary[PropertyNames.Scale];
            IList<string> scales = scalesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedScales, scales);

            var expectedUnitPrefixes = new string[] {};
            var unitPrefixesArray = (JArray) modelReferenceDataLibrary[PropertyNames.UnitPrefix];
            IList<string> unitPrefixes = unitPrefixesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedUnitPrefixes, unitPrefixes);

            var expectedUnits = new string[] {};
            var unitsArray = (JArray) modelReferenceDataLibrary[PropertyNames.Unit];
            IList<string> units = unitsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedUnits, units);

            var expectedBaseUnits = new string[] {};
            var baseUnitsArray = (JArray) modelReferenceDataLibrary[PropertyNames.BaseUnit];
            IList<string> baseUnits = baseUnitsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedBaseUnits, baseUnits);

            var expectedFileTypes = new string[] {};
            var fileTypesArray = (JArray) modelReferenceDataLibrary[PropertyNames.FileType];
            IList<string> fileTypes = fileTypesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedFileTypes, fileTypes);

            var expectedGlossaries = new string[] {};
            var glossariesArray = (JArray) modelReferenceDataLibrary[PropertyNames.Glossary];
            IList<string> glossaries = glossariesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedGlossaries, glossaries);

            var expectedReferenceSources = new string[] {};
            var referenceSourcesArray = (JArray) modelReferenceDataLibrary[PropertyNames.ReferenceSource];
            IList<string> referenceSources = referenceSourcesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedReferenceSources, referenceSources);

            var expectedRules = new string[] {};
            var rulesArray = (JArray) modelReferenceDataLibrary[PropertyNames.Rule];
            IList<string> rules = rulesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedRules, rules);

            var expectedConstants = new string[] {};
            var constantsArray = (JArray) modelReferenceDataLibrary[PropertyNames.Constant];
            IList<string> constants = constantsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedConstants, constants);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) modelReferenceDataLibrary[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) modelReferenceDataLibrary[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) modelReferenceDataLibrary[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}