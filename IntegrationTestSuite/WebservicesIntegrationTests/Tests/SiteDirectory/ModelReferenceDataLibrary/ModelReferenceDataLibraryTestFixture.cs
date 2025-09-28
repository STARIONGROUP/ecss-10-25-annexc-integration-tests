// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelReferenceDataLibraryTestFixture.cs" company="Starion Group S.A.">
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
    public class ModelReferenceDataLibraryTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedModelReferenceDataLibraryeIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var modelReferenceDataLibraryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model/116f6253-89bb-47d4-aa24-d11d197e43c9/requiredRdl");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(modelReferenceDataLibraryUri);

            //check if there is the only one ModelReferenceDataLibrary object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific ModelReferenceDataLibrary from the result by it's unique id
            var modelReferenceDataLibrary = jArray.Single(x => (string) x[PropertyNames.Iid] == "3483f2b5-ea29-45cc-8a46-f5f598558fc3");

            ModelReferenceDataLibraryTestFixture.VerifyProperties(modelReferenceDataLibrary);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedModelReferenceDataLibraryWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var modelReferenceDataLibraryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model/116f6253-89bb-47d4-aa24-d11d197e43c9/requiredRdl?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(modelReferenceDataLibraryUri);

            //check if there are 3 objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific EngineeringModelSetup from the result by it's unique id
            var engineeringModelSetup = jArray.Single(x => (string) x[PropertyNames.Iid] == "116f6253-89bb-47d4-aa24-d11d197e43c9");
            EngineeringModelSetupTestFixture.VerifyProperties(engineeringModelSetup);

            // get a specific ModelReferenceDataLibrary from the result by it's unique id
            var modelReferenceDataLibrary = jArray.Single(x => (string) x[PropertyNames.Iid] == "3483f2b5-ea29-45cc-8a46-f5f598558fc3");
            ModelReferenceDataLibraryTestFixture.VerifyProperties(modelReferenceDataLibrary);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatARuleWithoutSpecificCategoriesForItCanBeMovedFromModelRdlToSiteRdlFromWebApi()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");

            var postBodyPath = this.GetPath("Tests/SiteDirectory/ModelReferenceDataLibrary/POSTMoveIndBinaryRelShipRuleFromModelRDLToSiteRDL.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            //Check the amount of objects 
            Assert.That(jArray.Count, Is.EqualTo(4));

            //SiteDirectory properties
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.That((int) siteDirectory[PropertyNames.RevisionNumber], Is.EqualTo(2));

            //SiteReferenceDataLibrary properties
            var siteRdl = jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            Assert.That((int) siteRdl[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var expectedRules = new string[]
            {
                "8569bd5c-de3c-4d92-855f-b2c0ca94de0e",
                "8a5cd66e-7313-4843-813f-37081ca81bb8",
                "2615f9ec-30a4-4c0e-a9d3-1d067959c248",
                "7a6186ca-10c1-4074-bec1-4a92ce6ae59d",
                "e7e4eec5-ad39-40a0-9548-9c40d8e6df1b",
                "9a472bc5-86c0-4cad-8a1d-47d0fbf37e53"
            };
            var rulesArray = (JArray) siteRdl[PropertyNames.Rule];
            IList<string> rulesList = rulesArray.Select(x => (string) x).ToList();
            Assert.That(rulesList, Is.EquivalentTo(expectedRules));

            //ModelReferenceDataLibrary properties
            var modelRdl = jArray.Single(x => (string) x[PropertyNames.Iid] == "3483f2b5-ea29-45cc-8a46-f5f598558fc3");
            Assert.That((int) modelRdl[PropertyNames.RevisionNumber], Is.EqualTo(2));

            expectedRules = new string[]
            {
                "2fe3d938-394c-4c97-8422-d7916cff5c9b"
            };
            rulesArray = (JArray) modelRdl[PropertyNames.Rule];
            IList<string> rules = rulesArray.Select(x => (string) x).ToList();
            Assert.That(rules, Is.EquivalentTo(expectedRules));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatARuleCanBeMovedFromModelRdlToSiteRdlFromWebApi()
        {
            this.WebClient.Restore(this.Settings.Hostname);

            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");

            var postBodyPath = this.GetPath("Tests/SiteDirectory/ModelReferenceDataLibrary/POSTMoveBinaryRelShipRuleFromModelRDLToSiteRDL.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            //Check the amount of objects 
            Assert.That(jArray.Count, Is.EqualTo(6));

            //SiteDirectory properties
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.That((int) siteDirectory[PropertyNames.RevisionNumber], Is.EqualTo(2));

            //SiteReferenceDataLibrary properties
            var siteRdl = jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            Assert.That((int) siteRdl[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var expectedRules = new string[]
            {
                "8569bd5c-de3c-4d92-855f-b2c0ca94de0e",
                "8a5cd66e-7313-4843-813f-37081ca81bb8",
                "2615f9ec-30a4-4c0e-a9d3-1d067959c248",
                "7a6186ca-10c1-4074-bec1-4a92ce6ae59d",
                "e7e4eec5-ad39-40a0-9548-9c40d8e6df1b",
                "2fe3d938-394c-4c97-8422-d7916cff5c9b"
            };
            var rulesArray = (JArray) siteRdl[PropertyNames.Rule];
            IList<string> rulesList = rulesArray.Select(x => (string) x).ToList();
            Assert.That(rulesList, Is.EquivalentTo(expectedRules));

            var expectedDefinedCategories = new string[]
            {
                "cf059b19-235c-48be-87a3-9a8942c8e3e0",
                "107fc408-7e6d-4f1a-895a-1b6a6025ac20",
                "167b5cb0-766e-4ab2-b728-a9c9a662b017",
                "9ee5ba72-6cfa-432f-bc21-932aa3b82814",
                "2fd0ce0a-e4d1-4438-a35b-8c312c2c901a"
            };
            var definedCategoriesArray = (JArray) siteRdl["definedCategory"];
            IList<string> definedCategoriesList = definedCategoriesArray.Select(x => (string) x).ToList();
            Assert.That(definedCategoriesList, Is.EquivalentTo(expectedDefinedCategories));

            //ModelReferenceDataLibrary properties
            var modelRdl = jArray.Single(x => (string) x[PropertyNames.Iid] == "3483f2b5-ea29-45cc-8a46-f5f598558fc3");
            Assert.That((int) modelRdl[PropertyNames.RevisionNumber], Is.EqualTo(2));

            expectedRules = new string[]
            {
                "9a472bc5-86c0-4cad-8a1d-47d0fbf37e53"
            };
            rulesArray = (JArray) modelRdl[PropertyNames.Rule];
            IList<string> rules = rulesArray.Select(x => (string) x).ToList();
            Assert.That(rules, Is.EquivalentTo(expectedRules));

            expectedDefinedCategories = new string[] {};
            definedCategoriesArray = (JArray) modelRdl[PropertyNames.DefinedCategory];
            IList<string> definedCategories = definedCategoriesArray.Select(x => (string) x).ToList();
            Assert.That(definedCategories, Is.EquivalentTo(expectedDefinedCategories));
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
            Assert.That(modelReferenceDataLibrary.Children().Count(), Is.EqualTo(21));

            // assert that the properties are what is expected
            Assert.That((string) modelReferenceDataLibrary[PropertyNames.Iid], Is.EqualTo("3483f2b5-ea29-45cc-8a46-f5f598558fc3"));
            Assert.That((int) modelReferenceDataLibrary[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string) modelReferenceDataLibrary[PropertyNames.ClassKind], Is.EqualTo("ModelReferenceDataLibrary"));

            Assert.That((string) modelReferenceDataLibrary[PropertyNames.Name], Is.EqualTo("Test Model Reference Data Library"));
            Assert.That((string) modelReferenceDataLibrary[PropertyNames.ShortName], Is.EqualTo("TestModelReferenceDataLibrary"));

            Assert.That((string) modelReferenceDataLibrary[PropertyNames.RequiredRdl], Is.EqualTo("c454c687-ba3e-44c4-86bc-44544b2c7880"));

            var expectedDefinedCategories = new string[]
            {
                "9ee5ba72-6cfa-432f-bc21-932aa3b82814",
                "2fd0ce0a-e4d1-4438-a35b-8c312c2c901a"
            };
            var definedCategoriesArray = (JArray) modelReferenceDataLibrary[PropertyNames.DefinedCategory];
            IList<string> definedCategories = definedCategoriesArray.Select(x => (string) x).ToList();
            Assert.That(definedCategories, Is.EquivalentTo(expectedDefinedCategories));

            var expectedParameterTypes = new string[] {};
            var parameterTypesArray = (JArray) modelReferenceDataLibrary[PropertyNames.ParameterType];
            IList<string> parameterTypes = parameterTypesArray.Select(x => (string) x).ToList();
            Assert.That(parameterTypes, Is.EquivalentTo(expectedParameterTypes));

            var expectedBaseQuantityKinds = new string[] {};
            var baseQuantityKindsArray = (JArray) modelReferenceDataLibrary[PropertyNames.BaseQuantityKind];
            IList<string> baseQuantityKinds = baseQuantityKindsArray.Select(x => (string) x).ToList();
            Assert.That(baseQuantityKinds, Is.EquivalentTo(expectedBaseQuantityKinds));

            var expectedScales = new string[] {};
            var scalesArray = (JArray) modelReferenceDataLibrary[PropertyNames.Scale];
            IList<string> scales = scalesArray.Select(x => (string) x).ToList();
            Assert.That(scales, Is.EquivalentTo(expectedScales));

            var expectedUnitPrefixes = new string[] {};
            var unitPrefixesArray = (JArray) modelReferenceDataLibrary[PropertyNames.UnitPrefix];
            IList<string> unitPrefixes = unitPrefixesArray.Select(x => (string) x).ToList();
            Assert.That(unitPrefixes, Is.EquivalentTo(expectedUnitPrefixes));

            var expectedUnits = new string[] {};
            var unitsArray = (JArray) modelReferenceDataLibrary[PropertyNames.Unit];
            IList<string> units = unitsArray.Select(x => (string) x).ToList();
            Assert.That(units, Is.EquivalentTo(expectedUnits));

            var expectedBaseUnits = new string[] {};
            var baseUnitsArray = (JArray) modelReferenceDataLibrary[PropertyNames.BaseUnit];
            IList<string> baseUnits = baseUnitsArray.Select(x => (string) x).ToList();
            Assert.That(baseUnits, Is.EquivalentTo(expectedBaseUnits));

            var expectedFileTypes = new string[] {};
            var fileTypesArray = (JArray) modelReferenceDataLibrary[PropertyNames.FileType];
            IList<string> fileTypes = fileTypesArray.Select(x => (string) x).ToList();
            Assert.That(fileTypes, Is.EquivalentTo(expectedFileTypes));

            var expectedGlossaries = new string[] {};
            var glossariesArray = (JArray) modelReferenceDataLibrary[PropertyNames.Glossary];
            IList<string> glossaries = glossariesArray.Select(x => (string) x).ToList();
            Assert.That(glossaries, Is.EquivalentTo(expectedGlossaries));

            var expectedReferenceSources = new string[] {};
            var referenceSourcesArray = (JArray) modelReferenceDataLibrary[PropertyNames.ReferenceSource];
            IList<string> referenceSources = referenceSourcesArray.Select(x => (string) x).ToList();
            Assert.That(referenceSources, Is.EquivalentTo(expectedReferenceSources));

            var expectedRules = new string[]
            {
                "2fe3d938-394c-4c97-8422-d7916cff5c9b",
                "9a472bc5-86c0-4cad-8a1d-47d0fbf37e53"
            };
            var rulesArray = (JArray) modelReferenceDataLibrary[PropertyNames.Rule];
            IList<string> rules = rulesArray.Select(x => (string) x).ToList();
            Assert.That(rules, Is.EquivalentTo(expectedRules));

            var expectedConstants = new string[] {};
            var constantsArray = (JArray) modelReferenceDataLibrary[PropertyNames.Constant];
            IList<string> constants = constantsArray.Select(x => (string) x).ToList();
            Assert.That(constants, Is.EquivalentTo(expectedConstants));

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) modelReferenceDataLibrary[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) modelReferenceDataLibrary[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) modelReferenceDataLibrary[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));
        }
    }
}
