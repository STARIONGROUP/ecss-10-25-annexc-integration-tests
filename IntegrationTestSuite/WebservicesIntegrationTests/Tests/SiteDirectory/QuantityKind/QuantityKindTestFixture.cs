// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuantityKindTestFixture.cs" company="RHEA System S.A.">
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

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class QuantityKindTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("POST")]
        public void VerifyThatQuantityKindCanBeCreatedAndReorderedWithWebApi()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/SiteDirectory/QuantityKind/PostNewQuantityKind.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");

            // verify that the amount of returned properties 
            Assert.AreEqual(19, siteDirectory.Children().Count());

            Assert.AreEqual("f13de6f8-b03a-46e7-a492-53b2f260f294", (string) siteDirectory[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) siteDirectory[PropertyNames.RevisionNumber]);
            Assert.AreEqual("SiteDirectory", (string) siteDirectory[PropertyNames.ClassKind]);
            Assert.AreEqual("Test Site Directory", (string) siteDirectory[PropertyNames.Name]);
            Assert.AreEqual("TEST-SiteDir", (string) siteDirectory[PropertyNames.ShortName]);
            Assert.AreEqual("ee3ae5ff-ac5e-4957-bab1-7698fba2a267", (string) siteDirectory[PropertyNames.DefaultParticipantRole]);
            Assert.AreEqual("2428f4d9-f26d-4112-9d56-1c940748df69", (string) siteDirectory[PropertyNames.DefaultPersonRole]);

            var expectedOrganizations = new string[] { "cd22fc45-d898-4fac-85fc-fbcb7d7b12a7" };
            var organizationArray = (JArray) siteDirectory[PropertyNames.Organization];
            var organizations = organizationArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedOrganizations, organizations);

            var expectedPersons = new string[] { "77791b12-4c2c-4499-93fa-869df3692d22" };
            var personArray = (JArray) siteDirectory[PropertyNames.Person];
            var persons = personArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedPersons, persons);

            var expectedparticipantRole = new string[] { "ee3ae5ff-ac5e-4957-bab1-7698fba2a267" };
            var participantRoleArray = (JArray) siteDirectory[PropertyNames.ParticipantRole];
            var participantRoles = participantRoleArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedparticipantRole, participantRoles);

            var expectedsiteReferenceDataLibraries = new string[] { "c454c687-ba3e-44c4-86bc-44544b2c7880" };
            var siteReferenceDataLibraryArray = (JArray) siteDirectory[PropertyNames.SiteReferenceDataLibrary];
            var siteReferenceDataLibraries = siteReferenceDataLibraryArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedsiteReferenceDataLibraries, siteReferenceDataLibraries);

            var expectedModels = new string[] { "116f6253-89bb-47d4-aa24-d11d197e43c9" };
            var modelArray = (JArray) siteDirectory[PropertyNames.Model];
            var models = modelArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedModels, models);

            var expectedPersonRoles = new string[] { "2428f4d9-f26d-4112-9d56-1c940748df69" };
            var personRoleArray = (JArray) siteDirectory[PropertyNames.PersonRole];
            var personRoles = personRoleArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedPersonRoles, personRoles);

            var expectedlogEntries = new string[]
            {
                "98ba7b8a-1a1b-4569-a17c-b1ff620246a5",
                "66220289-e6ee-43cb-8fcd-d8e59a3dbf97"
            };

            var logEntryArray = (JArray) siteDirectory[PropertyNames.LogEntry];
            var logEntries = logEntryArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedlogEntries, logEntries);

            var expecteddomainGroups = new string[] { "86992db5-8ce2-4431-8ff5-6fe794d14687" };
            var domainGroupArray = (JArray) siteDirectory[PropertyNames.DomainGroup];
            var domainGroups = domainGroupArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expecteddomainGroups, domainGroups);

            var expectedDomains = new string[]
            {
                "0e92edde-fdff-41db-9b1d-f2e484f12535",
                "eb759723-14b9-49f4-8611-544d037bb764"
            };

            var domainArray = (JArray) siteDirectory[PropertyNames.Domain];
            var domains = domainArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDomains, domains);

            var expectedNaturalLanguages = new string[] { "73bf30cc-3573-488f-8746-6c03ba47973e" };
            var naturalLanguageArray = (JArray) siteDirectory[PropertyNames.NaturalLanguage];
            var naturalLanguages = naturalLanguageArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedNaturalLanguages, naturalLanguages);

            // SiteReferenceDataLibrary
            var siteReferenceDataLibrary = jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");

            // verify the amount of returned properties 
            Assert.AreEqual(23, siteReferenceDataLibrary.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("c454c687-ba3e-44c4-86bc-44544b2c7880", (string) siteReferenceDataLibrary[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) siteReferenceDataLibrary[PropertyNames.RevisionNumber]);
            Assert.AreEqual("SiteReferenceDataLibrary", (string) siteReferenceDataLibrary[PropertyNames.ClassKind]);

            Assert.IsFalse((bool) siteReferenceDataLibrary[PropertyNames.IsDeprecated]);
            Assert.AreEqual("Test Reference Data Library", (string) siteReferenceDataLibrary[PropertyNames.Name]);
            Assert.AreEqual("TestRDL", (string) siteReferenceDataLibrary[PropertyNames.ShortName]);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray) siteReferenceDataLibrary[PropertyNames.Alias];
            var aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray) siteReferenceDataLibrary[PropertyNames.Definition];
            var definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray) siteReferenceDataLibrary[PropertyNames.HyperLink];
            var h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            var expectedDefinedCategories = new string[]
            {
                "cf059b19-235c-48be-87a3-9a8942c8e3e0",
                "107fc408-7e6d-4f1a-895a-1b6a6025ac20",
                "167b5cb0-766e-4ab2-b728-a9c9a662b017"
            };

            var definedCategoriesArray = (JArray) siteReferenceDataLibrary[PropertyNames.DefinedCategory];
            var definedCategoriesList = definedCategoriesArray.Select(x => (string) x).ToList();
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
                "4a783624-b2bc-4e6d-95b3-11d036f6e917",
                "c36e050a-b4f9-4567-b29d-67af9216878e",
                "199bec0a-661d-408c-8ca7-718c636c5681",
                "8b8a44cd-0071-4acf-b33e-b8c3052821c5"
            };

            var parameterTypesArray = (JArray) siteReferenceDataLibrary[PropertyNames.ParameterType];
            var parameterTypesList = parameterTypesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterTypes, parameterTypesList);

            var expectedBaseQuantityKinds = new List<OrderedItem>
            {
                new OrderedItem(2948451, "c36e050a-b4f9-4567-b29d-67af9216878e"),
                new OrderedItem(2948452, "199bec0a-661d-408c-8ca7-718c636c5681"),
                new OrderedItem(2948453, "8b8a44cd-0071-4acf-b33e-b8c3052821c5"),
                new OrderedItem(16544539, "4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d")
            };

            var baseQuantityKindsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                siteReferenceDataLibrary[PropertyNames.BaseQuantityKind].ToString());

            CollectionAssert.AreEqual(expectedBaseQuantityKinds, baseQuantityKindsArray);

            var expectedScales = new string[]
            {
                "6326d1ea-c032-4a4b-8b10-608c59f1a923",
                "007b0e60-e67c-4060-88d2-2531ef9e7d9e",
                "541037e2-9f6a-466c-b56f-a09f81f36576",
                "53e82aeb-c42c-475c-b6bf-a102af883471",
                "f9d4b3c6-91a2-4f38-bb86-f504d6ac706f"
            };

            var scalesArray = (JArray) siteReferenceDataLibrary[PropertyNames.Scale];
            var scalesList = scalesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedScales, scalesList);

            var expectedUnitPrefixes = new string[]
            {
                "efa6380d-9508-4f3d-9b43-6ed33125b780"
            };

            var unitPrefixesArray = (JArray) siteReferenceDataLibrary[PropertyNames.UnitPrefix];
            var unitPrefixesList = unitPrefixesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedUnitPrefixes, unitPrefixesList);

            var expectedUnits = new string[]
            {
                "56842970-3915-4369-8712-61cfd8273ef9",
                "12f48e1a-2996-46cc-8dc1-faf4e69ae115",
                "0f69c1f9-7896-45fc-830c-1e336d22a64a",
                "c394eaa9-4832-4b2d-8d88-5e1b2c43732c"
            };

            var unitsArray = (JArray) siteReferenceDataLibrary[PropertyNames.Unit];
            var unitsList = unitsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedUnits, unitsList);

            var expectedBaseUnits = new string[]
            {
                "56842970-3915-4369-8712-61cfd8273ef9"
            };

            var baseUnitsArray = (JArray) siteReferenceDataLibrary[PropertyNames.BaseUnit];
            var baseUnitsList = baseUnitsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedBaseUnits, baseUnitsList);

            var expectedFileTypes = new string[]
            {
                "db04ac55-dd60-4607-a4e1-a9f91c9704e6",
                "b16894e4-acb5-4e81-a118-16c00eb86d8f",
                "f340df66-d65b-4814-a063-01d4dea1941c"
            };

            var fileTypesArray = (JArray) siteReferenceDataLibrary[PropertyNames.FileType];
            var fileTypesList = fileTypesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedFileTypes, fileTypesList);

            var expectedGlossaries = new string[]
            {
                "bb08686b-ae03-49eb-9f48-c196b5ad6bda"
            };

            var glossariesArray = (JArray) siteReferenceDataLibrary[PropertyNames.Glossary];
            var glossariesList = glossariesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedGlossaries, glossariesList);

            var expectedReferenceSources = new string[]
            {
                "ffd6c100-6c72-4d2a-8565-ff24bd576a89"
            };

            var referenceSourcesArray = (JArray) siteReferenceDataLibrary[PropertyNames.ReferenceSource];
            var referenceSourcesList = referenceSourcesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedReferenceSources, referenceSourcesList);

            var expectedRules = new string[]
            {
                "8569bd5c-de3c-4d92-855f-b2c0ca94de0e",
                "8a5cd66e-7313-4843-813f-37081ca81bb8",
                "2615f9ec-30a4-4c0e-a9d3-1d067959c248",
                "7a6186ca-10c1-4074-bec1-4a92ce6ae59d",
                "e7e4eec5-ad39-40a0-9548-9c40d8e6df1b"
            };

            var rulesArray = (JArray) siteReferenceDataLibrary[PropertyNames.Rule];
            var rulesList = rulesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedRules, rulesList);

            Assert.IsEmpty(siteReferenceDataLibrary[PropertyNames.RequiredRdl]);

            var expectedConstants = new string[]
            {
                "239754fe-834f-4394-9c3a-26cac7f866d3"
            };

            var constantsArray = (JArray) siteReferenceDataLibrary[PropertyNames.Constant];
            var constantsList = constantsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedConstants, constantsList);

            // DerivedQuantityKind
            var derivedQuantityKind = jArray.Single(x => (string) x[PropertyNames.Iid] == "c36e050a-b4f9-4567-b29d-67af9216878e");

            // verify the amount of returned properties 
            Assert.AreEqual(16, derivedQuantityKind.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("c36e050a-b4f9-4567-b29d-67af9216878e", (string) derivedQuantityKind[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) derivedQuantityKind[PropertyNames.RevisionNumber]);
            Assert.AreEqual("DerivedQuantityKind", (string) derivedQuantityKind[PropertyNames.ClassKind]);
            Assert.AreEqual("Test Derived QuantityKind", (string) derivedQuantityKind[PropertyNames.Name]);
            Assert.AreEqual("TestDerivedQuantityKind", (string) derivedQuantityKind[PropertyNames.ShortName]);
            Assert.AreEqual("testSymbol", (string) derivedQuantityKind[PropertyNames.Symbol]);
            Assert.AreEqual("testQuantityDimensionSymbol", (string) derivedQuantityKind[PropertyNames.QuantityDimensionSymbol]);
            Assert.AreEqual("53e82aeb-c42c-475c-b6bf-a102af883471", (string) derivedQuantityKind[PropertyNames.DefaultScale]);
            Assert.IsFalse((bool) derivedQuantityKind[PropertyNames.IsDeprecated]);

            var expectedQuantityKindFactors = new List<OrderedItem>
            {
                new OrderedItem(2948456, "3cec3570-df25-4b73-9283-2c6461dc8c36")
            };

            var quantityKindsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                derivedQuantityKind[PropertyNames.QuantityKindFactor].ToString());

            CollectionAssert.AreEquivalent(expectedQuantityKindFactors, quantityKindsArray);

            var expectedDerivedCategories = new string[] { };
            var derivedCategoriesArray = (JArray) derivedQuantityKind[PropertyNames.Category];
            var derivedCategories = derivedCategoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDerivedCategories, derivedCategories);

            var expectedDerivedPossibleScales = new[] { "53e82aeb-c42c-475c-b6bf-a102af883471" };
            var derivedPossibleScalesArray = (JArray) derivedQuantityKind[PropertyNames.PossibleScale];
            var derivedPossibleScales = derivedPossibleScalesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDerivedPossibleScales, derivedPossibleScales);

            var expectedDerivedAliases = new string[] { };
            var derivedAliasesArray = (JArray) derivedQuantityKind[PropertyNames.Alias];
            var derivedAliases = derivedAliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDerivedAliases, derivedAliases);

            var expectedDerivedDefinitions = new string[] { };
            var derivedDefinitionsArray = (JArray) derivedQuantityKind[PropertyNames.Definition];
            var derivedDefinitions = derivedDefinitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDerivedDefinitions, derivedDefinitions);

            var expectedDerivedHyperlinks = new string[] { };
            var derivedHyperlinksArray = (JArray) derivedQuantityKind[PropertyNames.HyperLink];
            var derivedHyperLinks = derivedHyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDerivedHyperlinks, derivedHyperLinks);

            // SimpleQuantityKind
            var simpleQuantityKind =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "199bec0a-661d-408c-8ca7-718c636c5681");

            // verify the amount of returned properties 
            Assert.AreEqual(15, simpleQuantityKind.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("199bec0a-661d-408c-8ca7-718c636c5681", (string) simpleQuantityKind[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) simpleQuantityKind[PropertyNames.RevisionNumber]);
            Assert.AreEqual("SimpleQuantityKind", (string) simpleQuantityKind[PropertyNames.ClassKind]);
            Assert.AreEqual("Test Simple QuantityKind", (string) simpleQuantityKind[PropertyNames.Name]);
            Assert.AreEqual("TestSimpleQuantityKind", (string) simpleQuantityKind[PropertyNames.ShortName]);
            Assert.AreEqual("testSymbol", (string) simpleQuantityKind[PropertyNames.Symbol]);
            Assert.AreEqual("testQuantityDimensionSymbol", (string) simpleQuantityKind[PropertyNames.QuantityDimensionSymbol]);
            Assert.AreEqual("53e82aeb-c42c-475c-b6bf-a102af883471", (string) simpleQuantityKind[PropertyNames.DefaultScale]);
            Assert.IsFalse((bool) simpleQuantityKind[PropertyNames.IsDeprecated]);

            var expectedSimpleCategories = new string[] { };
            var simpleCategoriesArray = (JArray) simpleQuantityKind[PropertyNames.Category];
            var simpleCategories = simpleCategoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedSimpleCategories, simpleCategories);

            var expectedSimplePossibleScales = new[] { "53e82aeb-c42c-475c-b6bf-a102af883471" };
            var simplePossibleScalesArray = (JArray) simpleQuantityKind[PropertyNames.PossibleScale];
            var simplePossibleScales = simplePossibleScalesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedSimplePossibleScales, simplePossibleScales);

            var expectedSimpleAliases = new string[] { };
            var simpleAliasesArray = (JArray) simpleQuantityKind[PropertyNames.Alias];
            var simpleAliases = simpleAliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedSimpleAliases, simpleAliases);

            var expectedSimpleDefinitions = new string[] { };
            var simpleDefinitionsArray = (JArray) simpleQuantityKind[PropertyNames.Definition];
            var simpleDefinitions = simpleDefinitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedSimpleDefinitions, simpleDefinitions);

            var expectedSimpleHyperlinks = new string[] { };
            var simpleHyperlinksArray = (JArray) simpleQuantityKind[PropertyNames.HyperLink];
            var simpleHyperLinks = simpleHyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedSimpleHyperlinks, simpleHyperLinks);

            // SpecializedQuantityKind
            var specializedQuantityKind =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "8b8a44cd-0071-4acf-b33e-b8c3052821c5");

            // verify the amount of returned properties 
            Assert.AreEqual(16, specializedQuantityKind.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("8b8a44cd-0071-4acf-b33e-b8c3052821c5", (string) specializedQuantityKind[PropertyNames.Iid]);
            Assert.AreEqual(2, (int) specializedQuantityKind[PropertyNames.RevisionNumber]);
            Assert.AreEqual("SpecializedQuantityKind", (string) specializedQuantityKind[PropertyNames.ClassKind]);
            Assert.AreEqual("Test Specialized QuantityKind", (string) specializedQuantityKind[PropertyNames.Name]);
            Assert.AreEqual("TestSpecializedQuantityKind", (string) specializedQuantityKind[PropertyNames.ShortName]);
            Assert.AreEqual("testSymbol", (string) specializedQuantityKind[PropertyNames.Symbol]);
            Assert.AreEqual("testQuantityDimensionSymbol", (string) specializedQuantityKind[PropertyNames.QuantityDimensionSymbol]);
            Assert.AreEqual("199bec0a-661d-408c-8ca7-718c636c5681", (string) specializedQuantityKind[PropertyNames.General]);
            Assert.AreEqual("53e82aeb-c42c-475c-b6bf-a102af883471", (string) specializedQuantityKind[PropertyNames.DefaultScale]);
            Assert.IsFalse((bool) specializedQuantityKind[PropertyNames.IsDeprecated]);

            var expectedSpecializedCategories = new string[] { };
            var specializedCategoriesArray = (JArray) specializedQuantityKind[PropertyNames.Category];
            var specializedCategories = specializedCategoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedSpecializedCategories, specializedCategories);

            var expectedSpecializedPossibleScales = new[] { "53e82aeb-c42c-475c-b6bf-a102af883471" };
            var specializedPossibleScalesArray = (JArray) specializedQuantityKind[PropertyNames.PossibleScale];
            var specializedPossibleScales = specializedPossibleScalesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedSpecializedPossibleScales, specializedPossibleScales);

            var expectedSpecializedAliases = new string[] { };
            var specializedAliasesArray = (JArray) specializedQuantityKind[PropertyNames.Alias];
            var specializedAliases = specializedAliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedSpecializedAliases, specializedAliases);

            var expectedSpecializedDefinitions = new string[] { };
            var specializedDefinitionsArray = (JArray) specializedQuantityKind[PropertyNames.Definition];
            var specializedDefinitions = specializedDefinitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedSpecializedDefinitions, specializedDefinitions);

            var expectedSpecializedHyperlinks = new string[] { };
            var specializedHyperlinksArray = (JArray) specializedQuantityKind[PropertyNames.HyperLink];
            var specializedHyperLinks = specializedHyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedSpecializedHyperlinks, specializedHyperLinks);

            postBodyPath = this.GetPath("Tests/SiteDirectory/QuantityKind/PostReorderQuantityKind.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            siteReferenceDataLibrary = jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");

            expectedBaseQuantityKinds = new List<OrderedItem>
            {
                new OrderedItem(1, "4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d"),
                new OrderedItem(2, "8b8a44cd-0071-4acf-b33e-b8c3052821c5"),
                new OrderedItem(3, "199bec0a-661d-408c-8ca7-718c636c5681"),
                new OrderedItem(4, "c36e050a-b4f9-4567-b29d-67af9216878e")
            };

            baseQuantityKindsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                siteReferenceDataLibrary[PropertyNames.BaseQuantityKind].ToString());

            CollectionAssert.AreEqual(expectedBaseQuantityKinds, baseQuantityKindsArray);
        }
    }
}
