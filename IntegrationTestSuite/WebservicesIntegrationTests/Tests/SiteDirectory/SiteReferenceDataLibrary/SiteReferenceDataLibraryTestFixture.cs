﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteReferenceDataLibraryTestFixture.cs" company="Starion Group S.A.">
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
 
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class SiteReferenceDataLibraryTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedSiteReferenceDataLibraryIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var siteReferenceDataLibrariesUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(siteReferenceDataLibrariesUri);

            //check if there is only one SiteReferenceDataLibrary object
            Assert.AreEqual(1, jArray.Count);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");

            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedSiteReferenceDataLibraryWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var siteReferenceDataLibraryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(siteReferenceDataLibraryUri);

            //check if there are only two objects
            Assert.AreEqual(2, jArray.Count);

            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);
        }

        [Test]
        [Category("GET")]
        public void VerifyLibraryInReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var siteReferenceDataLibrariesUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary?includeReferenceData=true&includeAllContainers=true&extent=deep");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(siteReferenceDataLibrariesUri);

            var setOfUniqueParameterTypeIids = new HashSet<string>();
            var sumOfParameterTypeElements = 0;

            //check for all objects whose classKind name contains ParameterType or QuantityKind
            foreach (var obj in jArray)
            {
                if (((string) obj[PropertyNames.ClassKind]).Contains("ParameterType") ||
                    ((string) obj[PropertyNames.ClassKind]).Contains("QuantityKind"))
                {
                    sumOfParameterTypeElements++;
                    setOfUniqueParameterTypeIids.Add((string) obj[PropertyNames.Iid]);
                }
            }

            Assert.AreEqual(sumOfParameterTypeElements, setOfUniqueParameterTypeIids.Count);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatUnitDeletionAsPropertyFromRDLIsDoneAsDeprecationFromWebApi()
        {
            var uri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");

            var postBodyPath = this.GetPath("Tests/SiteDirectory/SiteReferenceDataLibrary/PostDeleteUnitAsProperty.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(uri, postBody);

            //check if there are only two objects
            Assert.AreEqual(2, jArray.Count);

            var siteDirectory = jArray.Single(x => (string)x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.AreEqual(2, (int)siteDirectory[PropertyNames.RevisionNumber]);

            // get a specific SimpleUnit from the result by it's unique id
            var simpleUnit = jArray.Single(x => (string)x["iid"] == "56842970-3915-4369-8712-61cfd8273ef9");
            Assert.IsTrue((bool)simpleUnit["isDeprecated"]);
        }

        /// <summary>
        /// Verifies all properties of the SiteReferenceDataLibrary <see cref="JToken"/>
        /// </summary>
        /// <param name="siteReferenceDataLibrary">
        /// The <see cref="JToken"/> that contains the properties of
        /// the SiteReferenceDataLibrary object
        /// </param>
        public static void VerifyProperties(JToken siteReferenceDataLibrary)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(22, siteReferenceDataLibrary.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("c454c687-ba3e-44c4-86bc-44544b2c7880", (string) siteReferenceDataLibrary["iid"]);
            Assert.AreEqual(1, (int) siteReferenceDataLibrary["revisionNumber"]);
            Assert.AreEqual("SiteReferenceDataLibrary", (string) siteReferenceDataLibrary["classKind"]);

            Assert.IsFalse((bool) siteReferenceDataLibrary["isDeprecated"]);
            Assert.AreEqual("Test Reference Data Library", (string) siteReferenceDataLibrary["name"]);
            Assert.AreEqual("TestRDL", (string) siteReferenceDataLibrary["shortName"]);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) siteReferenceDataLibrary["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) siteReferenceDataLibrary["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) siteReferenceDataLibrary["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            var expectedDefinedCategories = new string[]
            {
                "cf059b19-235c-48be-87a3-9a8942c8e3e0",
                "107fc408-7e6d-4f1a-895a-1b6a6025ac20",
                "167b5cb0-766e-4ab2-b728-a9c9a662b017"
            };
            var definedCategoriesArray = (JArray) siteReferenceDataLibrary["definedCategory"];
            IList<string> definedCategoriesList = definedCategoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinedCategories, definedCategoriesList);

            var expectedParameterTypes = new string[]
            {
                "35a9cf05-4eba-4cda-b60c-7cfeaac8f892",
                "33cf1171-3cd2-4494-8d54-639bfc583155",
                "9c1d3e39-0754-4388-8e1e-070ca9a0e7b6",
                "664d5611-c564-4eba-8f2e-e23b99385daf",
                "a21c15c4-3e1e-46b5-b109-5063dec1e254",
                "e4cfdb60-ed3a-455c-9a33-a3edc921637f",
                "4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d",
                "0a6dc59d-4292-43be-a247-b8d7074d5d52",
                "74d9c38f-5ace-4f90-8841-d0f9942e9d09",
                "0d3178f9-68d0-4b1a-afe8-d5df0b66f1d4",
                "4a783624-b2bc-4e6d-95b3-11d036f6e917"
            };
            var parameterTypesArray = (JArray) siteReferenceDataLibrary["parameterType"];
            IList<string> parameterTypesList = parameterTypesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterTypes, parameterTypesList);

            var expectedBaseQuantityKinds = new List<OrderedItem>
            {
                new OrderedItem(16544539, "4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d")
            };
            var baseQuantityKindsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                siteReferenceDataLibrary["baseQuantityKind"].ToString());
            CollectionAssert.AreEquivalent(expectedBaseQuantityKinds, baseQuantityKindsArray);

            var expectedScales = new string[]
            {
                "6326d1ea-c032-4a4b-8b10-608c59f1a923",
                "007b0e60-e67c-4060-88d2-2531ef9e7d9e",
                "541037e2-9f6a-466c-b56f-a09f81f36576",
                "53e82aeb-c42c-475c-b6bf-a102af883471",
                "f9d4b3c6-91a2-4f38-bb86-f504d6ac706f"
            };
            var scalesArray = (JArray) siteReferenceDataLibrary["scale"];
            IList<string> scalesList = scalesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedScales, scalesList);

            var expectedUnitPrefixes = new string[]
            {
                "efa6380d-9508-4f3d-9b43-6ed33125b780"
            };
            var unitPrefixesArray = (JArray) siteReferenceDataLibrary["unitPrefix"];
            IList<string> unitPrefixesList = unitPrefixesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedUnitPrefixes, unitPrefixesList);

            var expectedUnits = new string[]
            {
                "56842970-3915-4369-8712-61cfd8273ef9",
                "12f48e1a-2996-46cc-8dc1-faf4e69ae115",
                "0f69c1f9-7896-45fc-830c-1e336d22a64a",
                "c394eaa9-4832-4b2d-8d88-5e1b2c43732c"
            };
            var unitsArray = (JArray) siteReferenceDataLibrary["unit"];
            IList<string> unitsList = unitsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedUnits, unitsList);

            var expectedBaseUnits = new string[]
            {
                "56842970-3915-4369-8712-61cfd8273ef9"
            };
            var baseUnitsArray = (JArray) siteReferenceDataLibrary["baseUnit"];
            IList<string> baseUnitsList = baseUnitsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedBaseUnits, baseUnitsList);

            var expectedFileTypes = new string[]
            {
                "db04ac55-dd60-4607-a4e1-a9f91c9704e6",
                "b16894e4-acb5-4e81-a118-16c00eb86d8f",
                "f340df66-d65b-4814-a063-01d4dea1941c"
            };
            var fileTypesArray = (JArray) siteReferenceDataLibrary["fileType"];
            IList<string> fileTypesList = fileTypesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedFileTypes, fileTypesList);

            var expectedGlossaries = new string[]
            {
                "bb08686b-ae03-49eb-9f48-c196b5ad6bda"
            };
            var glossariesArray = (JArray) siteReferenceDataLibrary["glossary"];
            IList<string> glossariesList = glossariesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedGlossaries, glossariesList);

            var expectedReferenceSources = new string[]
            {
                "ffd6c100-6c72-4d2a-8565-ff24bd576a89"
            };
            var referenceSourcesArray = (JArray) siteReferenceDataLibrary["referenceSource"];
            IList<string> referenceSourcesList = referenceSourcesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedReferenceSources, referenceSourcesList);

            var expectedRules = new string[]
            {
                "8569bd5c-de3c-4d92-855f-b2c0ca94de0e",
                "8a5cd66e-7313-4843-813f-37081ca81bb8",
                "2615f9ec-30a4-4c0e-a9d3-1d067959c248",
                "7a6186ca-10c1-4074-bec1-4a92ce6ae59d",
                "e7e4eec5-ad39-40a0-9548-9c40d8e6df1b"
            };
            var rulesArray = (JArray) siteReferenceDataLibrary["rule"];
            IList<string> rulesList = rulesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedRules, rulesList);

            Assert.IsEmpty(siteReferenceDataLibrary["requiredRdl"]);

            var expectedConstants = new string[]
            {
                "239754fe-834f-4394-9c3a-26cac7f866d3"
            };
            var constantsArray = (JArray) siteReferenceDataLibrary["constant"];
            IList<string> constantsList = constantsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedConstants, constantsList);
        }
    }
}
