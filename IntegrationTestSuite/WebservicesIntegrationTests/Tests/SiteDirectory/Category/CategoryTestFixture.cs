// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryTestFixture.cs" company="Starion Group S.A.">
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
    public class CategoryTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedCategoryIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var categoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/definedCategory");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(categoryUri);

            //check if there are two Category objects 
            Assert.AreEqual(3, jArray.Count);

            CategoryTestFixture.VerifyProperties(jArray);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedCategoryWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var categoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/definedCategory?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(categoryUri);

            //check if there are 4 objects
            Assert.AreEqual(5, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            CategoryTestFixture.VerifyProperties(jArray);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatSuperCategoryCanBeDeletedFromWebApi()
        {
            // Add super category
            var uri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/SiteDirectory/Category/PostAddSuperCategory.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(uri, postBody);

            // check if there are 2 objects
            Assert.AreEqual(2, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.AreEqual(2, (int)siteDirectory[PropertyNames.RevisionNumber]);

            // get a specific Category from the result by it's unique id
            var category = jArray.Single(x => (string)x[PropertyNames.Iid] == "cf059b19-235c-48be-87a3-9a8942c8e3e0");
            Assert.AreEqual(2, (int)category[PropertyNames.RevisionNumber]);
            var expectedSuperCategories = new string[]
                                              {
                                                  "167b5cb0-766e-4ab2-b728-a9c9a662b017"
                                              };
            var superCategoriesArray = (JArray)category["superCategory"];
            IList<string> superCategories = superCategoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedSuperCategories, superCategories);

            // Delete super category
            postBodyPath = this.GetPath("Tests/SiteDirectory/Category/PostDeleteSuperCategory.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(uri, postBody);

            // check if there are 2 objects
            Assert.AreEqual(2, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.AreEqual(3, (int)siteDirectory[PropertyNames.RevisionNumber]);

            // get a specific Category from the result by it's unique id
            category = jArray.Single(x => (string)x[PropertyNames.Iid] == "cf059b19-235c-48be-87a3-9a8942c8e3e0");
            Assert.AreEqual(3, (int)category[PropertyNames.RevisionNumber]);
            expectedSuperCategories = new string[]{};
            superCategoriesArray = (JArray)category["superCategory"];
            superCategories = superCategoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedSuperCategories, superCategories);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatPermissibleClassCanBeDeletedFromWebApi()
        {
            // Add Book class
            var uri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/SiteDirectory/Category/PostAddPermissibleClass.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(uri, postBody);

            // check if there are 2 objects
            Assert.AreEqual(2, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.AreEqual(2, (int)siteDirectory[PropertyNames.RevisionNumber]);

            // get a specific Category from the result by it's unique id
            var category = jArray.Single(x => (string)x[PropertyNames.Iid] == "cf059b19-235c-48be-87a3-9a8942c8e3e0");
            Assert.AreEqual(2, (int)category[PropertyNames.RevisionNumber]);
            var expectedPermissibleClasses = new string[]
                                               {
                                                   "BinaryRelationship",
                                                   "ElementDefinition",
                                                   "ElementUsage"
                                               };
            var permissibleClassesArray = (JArray)category["permissibleClass"];
            IList<string> permissibleClasses = permissibleClassesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPermissibleClasses, permissibleClasses);

            // Delete Book class
            postBodyPath = this.GetPath("Tests/SiteDirectory/Category/PostDeletePermissibleClass.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(uri, postBody);

            // check if there are 2 objects
            Assert.AreEqual(2, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.AreEqual(3, (int)siteDirectory[PropertyNames.RevisionNumber]);

            // get a specific Category from the result by it's unique id
            category = jArray.Single(x => (string)x[PropertyNames.Iid] == "cf059b19-235c-48be-87a3-9a8942c8e3e0");
            Assert.AreEqual(3, (int)category[PropertyNames.RevisionNumber]);
            expectedPermissibleClasses = new string[]
                                                 {
                                                     "ElementDefinition",
                                                     "ElementUsage"
                                                 };
            permissibleClassesArray = (JArray)category["permissibleClass"];
            permissibleClasses = permissibleClassesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPermissibleClasses, permissibleClasses);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatACategoryCanBeCreatedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/SiteDirectory/Category/PostNewCategory.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            
            // verify that the amount of returned properties 
            Assert.AreEqual(19, siteDirectory.Children().Count());

            Assert.AreEqual("f13de6f8-b03a-46e7-a492-53b2f260f294", (string)siteDirectory[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)siteDirectory[PropertyNames.RevisionNumber]);
            Assert.AreEqual("SiteDirectory", (string)siteDirectory[PropertyNames.ClassKind]);
            Assert.AreEqual("Test Site Directory", (string)siteDirectory[PropertyNames.Name]);
            Assert.AreEqual("TEST-SiteDir", (string)siteDirectory[PropertyNames.ShortName]);
            Assert.AreEqual("ee3ae5ff-ac5e-4957-bab1-7698fba2a267", (string)siteDirectory[PropertyNames.DefaultParticipantRole]);
            Assert.AreEqual("2428f4d9-f26d-4112-9d56-1c940748df69", (string)siteDirectory[PropertyNames.DefaultPersonRole]);

            var expectedOrganizations = new string[] { "cd22fc45-d898-4fac-85fc-fbcb7d7b12a7" };
            var organizationArray = (JArray)siteDirectory[PropertyNames.Organization];
            IList<string> organizations = organizationArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedOrganizations, organizations);

            var expectedPersons = new string[] { "77791b12-4c2c-4499-93fa-869df3692d22" };
            var personArray = (JArray)siteDirectory[PropertyNames.Person];
            IList<string> persons = personArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPersons, persons);

            var expectedparticipantRole = new string[] { "ee3ae5ff-ac5e-4957-bab1-7698fba2a267" };
            var participantRoleArray = (JArray)siteDirectory[PropertyNames.ParticipantRole];
            IList<string> participantRoles = participantRoleArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedparticipantRole, participantRoles);

            var expectedsiteReferenceDataLibraries = new string[] { "c454c687-ba3e-44c4-86bc-44544b2c7880" };
            var siteReferenceDataLibraryArray = (JArray)siteDirectory[PropertyNames.SiteReferenceDataLibrary];
            IList<string> siteReferenceDataLibraries = siteReferenceDataLibraryArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedsiteReferenceDataLibraries, siteReferenceDataLibraries);

            var expectedModels = new string[] { "116f6253-89bb-47d4-aa24-d11d197e43c9" };
            var modelArray = (JArray)siteDirectory[PropertyNames.Model];
            IList<string> models = modelArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedModels, models);

            var expectedPersonRoles = new string[] { "2428f4d9-f26d-4112-9d56-1c940748df69" };
            var personRoleArray = (JArray)siteDirectory[PropertyNames.PersonRole];
            IList<string> personRoles = personRoleArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPersonRoles, personRoles);

            var expectedlogEntries = new string[]
            {
                "98ba7b8a-1a1b-4569-a17c-b1ff620246a5",
                "66220289-e6ee-43cb-8fcd-d8e59a3dbf97"
            };
            var logEntryArray = (JArray)siteDirectory[PropertyNames.LogEntry];
            IList<string> logEntries = logEntryArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedlogEntries, logEntries);

            var expecteddomainGroups = new string[] { "86992db5-8ce2-4431-8ff5-6fe794d14687" };
            var domainGroupArray = (JArray)siteDirectory[PropertyNames.DomainGroup];
            IList<string> domainGroups = domainGroupArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expecteddomainGroups, domainGroups);

            var expectedDomains = new string[] 
            {
                "0e92edde-fdff-41db-9b1d-f2e484f12535",
                "eb759723-14b9-49f4-8611-544d037bb764"
            };
            var domainArray = (JArray)siteDirectory[PropertyNames.Domain];
            IList<string> domains = domainArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDomains, domains);

            var expectedNaturalLanguages = new string[] { "73bf30cc-3573-488f-8746-6c03ba47973e" };
            var naturalLanguageArray = (JArray)siteDirectory[PropertyNames.NaturalLanguage];
            IList<string> naturalLanguages = naturalLanguageArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedNaturalLanguages, naturalLanguages);

            var siteReferenceDataLibrary = jArray.Single(x => (string)x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");

            // verify the amount of returned properties 
            Assert.AreEqual(22, siteReferenceDataLibrary.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("c454c687-ba3e-44c4-86bc-44544b2c7880", (string)siteReferenceDataLibrary["iid"]);
            Assert.AreEqual(2, (int)siteReferenceDataLibrary["revisionNumber"]);
            Assert.AreEqual("SiteReferenceDataLibrary", (string)siteReferenceDataLibrary["classKind"]);

            Assert.IsFalse((bool)siteReferenceDataLibrary["isDeprecated"]);
            Assert.AreEqual("Test Reference Data Library", (string)siteReferenceDataLibrary["name"]);
            Assert.AreEqual("TestRDL", (string)siteReferenceDataLibrary["shortName"]);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)siteReferenceDataLibrary["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)siteReferenceDataLibrary["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)siteReferenceDataLibrary["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            var expectedDefinedCategories = new string[]
            {
                "cf059b19-235c-48be-87a3-9a8942c8e3e0",
                "107fc408-7e6d-4f1a-895a-1b6a6025ac20",
                "167b5cb0-766e-4ab2-b728-a9c9a662b017",
                "0d3f0a61-49ca-40d5-9a92-9a650c837c72"
            };
            var definedCategoriesArray = (JArray)siteReferenceDataLibrary["definedCategory"];
            IList<string> definedCategoriesList = definedCategoriesArray.Select(x => (string)x).ToList();
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
            var parameterTypesArray = (JArray)siteReferenceDataLibrary["parameterType"];
            IList<string> parameterTypesList = parameterTypesArray.Select(x => (string)x).ToList();
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
            var scalesArray = (JArray)siteReferenceDataLibrary["scale"];
            IList<string> scalesList = scalesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedScales, scalesList);

            var expectedUnitPrefixes = new string[]
            {
                "efa6380d-9508-4f3d-9b43-6ed33125b780"
            };
            var unitPrefixesArray = (JArray)siteReferenceDataLibrary["unitPrefix"];
            IList<string> unitPrefixesList = unitPrefixesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedUnitPrefixes, unitPrefixesList);

            var expectedUnits = new string[]
            {
                "56842970-3915-4369-8712-61cfd8273ef9",
                "12f48e1a-2996-46cc-8dc1-faf4e69ae115",
                "0f69c1f9-7896-45fc-830c-1e336d22a64a",
                "c394eaa9-4832-4b2d-8d88-5e1b2c43732c"
            };
            var unitsArray = (JArray)siteReferenceDataLibrary["unit"];
            IList<string> unitsList = unitsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedUnits, unitsList);

            var expectedBaseUnits = new string[]
            {
                "56842970-3915-4369-8712-61cfd8273ef9"
            };
            var baseUnitsArray = (JArray)siteReferenceDataLibrary["baseUnit"];
            IList<string> baseUnitsList = baseUnitsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedBaseUnits, baseUnitsList);

            var expectedFileTypes = new string[]
            {
                "db04ac55-dd60-4607-a4e1-a9f91c9704e6",
                "b16894e4-acb5-4e81-a118-16c00eb86d8f",
                "f340df66-d65b-4814-a063-01d4dea1941c"
            };
            var fileTypesArray = (JArray)siteReferenceDataLibrary["fileType"];
            IList<string> fileTypesList = fileTypesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedFileTypes, fileTypesList);

            var expectedGlossaries = new string[]
            {
                "bb08686b-ae03-49eb-9f48-c196b5ad6bda"
            };
            var glossariesArray = (JArray)siteReferenceDataLibrary["glossary"];
            IList<string> glossariesList = glossariesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedGlossaries, glossariesList);

            var expectedReferenceSources = new string[]
            {
                "ffd6c100-6c72-4d2a-8565-ff24bd576a89"
            };
            var referenceSourcesArray = (JArray)siteReferenceDataLibrary["referenceSource"];
            IList<string> referenceSourcesList = referenceSourcesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedReferenceSources, referenceSourcesList);

            var expectedRules = new string[]
            {
                "8569bd5c-de3c-4d92-855f-b2c0ca94de0e",
                "8a5cd66e-7313-4843-813f-37081ca81bb8",
                "2615f9ec-30a4-4c0e-a9d3-1d067959c248",
                "7a6186ca-10c1-4074-bec1-4a92ce6ae59d",
                "e7e4eec5-ad39-40a0-9548-9c40d8e6df1b"
            };
            var rulesArray = (JArray)siteReferenceDataLibrary["rule"];
            IList<string> rulesList = rulesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedRules, rulesList);

            Assert.IsEmpty(siteReferenceDataLibrary["requiredRdl"]);

            var expectedConstants = new string[]
            {
                "239754fe-834f-4394-9c3a-26cac7f866d3"
            };
            var constantsArray = (JArray)siteReferenceDataLibrary["constant"];
            IList<string> constantsList = constantsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedConstants, constantsList);

            // Category
            var category = jArray.Single(x => (string)x["iid"] == "0d3f0a61-49ca-40d5-9a92-9a650c837c72");
            Assert.AreEqual("0d3f0a61-49ca-40d5-9a92-9a650c837c72", (string)category["iid"]);
            Assert.AreEqual(2, (int)category["revisionNumber"]);
            Assert.AreEqual("Category", (string)category["classKind"]);

            Assert.IsFalse((bool)category["isAbstract"]);
            Assert.IsFalse((bool)category["isDeprecated"]);
            Assert.AreEqual("New Category", (string)category["name"]);
            Assert.AreEqual("NewCat", (string)category["shortName"]);

            var expectedSuperCategories = new string[] { };
            var superCategoriesArray = (JArray)category["superCategory"];
            IList<string> superCategories = superCategoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedSuperCategories, superCategories);

            var expectedPermissibleClasses = new string[]
            {
                "ElementDefinition",
                "ElementUsage"
            };
            var permissibleClassesArray = (JArray)category["permissibleClass"];
            IList<string> permissibleClasses = permissibleClassesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPermissibleClasses, permissibleClasses);

            var expectedCategoryAliases = new string[] { };
            var categoryAliasesArray = (JArray)category["alias"];
            IList<string> categoryAliases = categoryAliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCategoryAliases, categoryAliases);

            var expectedCategoryDefinitions = new string[] { };
            var categoryDefinitionsArray = (JArray)category["definition"];
            IList<string> categoryDefinitions = categoryDefinitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCategoryDefinitions, categoryDefinitions);

            var expectedCategoryHyperLinks = new string[] { };
            var categoryHyperLinksArray = (JArray)category["hyperLink"];
            IList<string> categoryHyperLinks = categoryHyperLinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCategoryHyperLinks, categoryHyperLinks);
        }

        /// <summary>
        /// Verifies all properties of the Category <see cref="JToken"/>
        /// </summary>
        /// <param name="category">
        /// The <see cref="JToken"/> that contains the properties of
        /// the Category object
        /// </param>
        public static void VerifyProperties(JToken category)
        {
            // assert that all objects are what is expected
            var categoryObject =
                category.Single(x => (string) x["iid"] == "cf059b19-235c-48be-87a3-9a8942c8e3e0");
            Assert.AreEqual("cf059b19-235c-48be-87a3-9a8942c8e3e0", (string) categoryObject["iid"]);
            Assert.AreEqual(1, (int) categoryObject["revisionNumber"]);
            Assert.AreEqual("Category", (string) categoryObject["classKind"]);

            Assert.IsFalse((bool) categoryObject["isAbstract"]);
            Assert.IsFalse((bool) categoryObject["isDeprecated"]);
            Assert.AreEqual("Test Category", (string) categoryObject["name"]);
            Assert.AreEqual("TestCategory", (string) categoryObject["shortName"]);

            var expectedSuperCategories = new string[] {};
            var superCategoriesArray = (JArray) categoryObject["superCategory"];
            IList<string> superCategories = superCategoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedSuperCategories, superCategories);

            var expectedPermissibleClasses = new string[]
            {
                "ElementDefinition",
                "ElementUsage"
            };
            var permissibleClassesArray = (JArray) categoryObject["permissibleClass"];
            IList<string> permissibleClasses = permissibleClassesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedPermissibleClasses, permissibleClasses);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) categoryObject["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) categoryObject["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) categoryObject["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            //next object
            categoryObject =
                category.Single(x => (string) x["iid"] == "107fc408-7e6d-4f1a-895a-1b6a6025ac20");
            Assert.AreEqual("107fc408-7e6d-4f1a-895a-1b6a6025ac20", (string) categoryObject["iid"]);
            Assert.AreEqual(1, (int) categoryObject["revisionNumber"]);
            Assert.AreEqual("Category", (string) categoryObject["classKind"]);

            Assert.IsFalse((bool) categoryObject["isAbstract"]);
            Assert.IsFalse((bool) categoryObject["isDeprecated"]);
            Assert.AreEqual("Test Category - BinaryRelationship", (string) categoryObject["name"]);
            Assert.AreEqual("TestCategoryBinaryRelationship", (string) categoryObject["shortName"]);

            expectedSuperCategories = new string[] {};
            superCategoriesArray = (JArray) categoryObject["superCategory"];
            superCategories = superCategoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedSuperCategories, superCategories);

            expectedPermissibleClasses = new string[]
            {
                "BinaryRelationship"
            };
            permissibleClassesArray = (JArray) categoryObject["permissibleClass"];
            permissibleClasses = permissibleClassesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedPermissibleClasses, permissibleClasses);

            expectedAliases = new string[] {};
            aliasesArray = (JArray) categoryObject["alias"];
            aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            expectedDefinitions = new string[] {};
            definitionsArray = (JArray) categoryObject["definition"];
            definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            expectedHyperlinks = new string[] {};
            hyperlinksArray = (JArray) categoryObject["hyperLink"];
            h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
