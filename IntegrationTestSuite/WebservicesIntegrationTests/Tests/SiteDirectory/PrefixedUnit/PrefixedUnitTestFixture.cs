// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrefixedUnitTestFixture.cs" company="Starion Group S.A.">
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
    public class PrefixedUnitTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedUnitIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var prefixedUnitUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/unit/0f69c1f9-7896-45fc-830c-1e336d22a64a");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(prefixedUnitUri);

            //check if there is the only one PrefixedUnit object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific PrefixedUnit from the result by it's unique id
            var prefixedUnit = jArray.Single(x => (string) x["iid"] == "0f69c1f9-7896-45fc-830c-1e336d22a64a");

            PrefixedUnitTestFixture.VerifyProperties(prefixedUnit);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedUnitWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var prefixedUnitUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/unit/0f69c1f9-7896-45fc-830c-1e336d22a64a?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(prefixedUnitUri);

            //check if there are 3 objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific PrefixedUnit from the result by it's unique id
            var prefixedUnit = jArray.Single(x => (string) x["iid"] == "0f69c1f9-7896-45fc-830c-1e336d22a64a");
            PrefixedUnitTestFixture.VerifyProperties(prefixedUnit);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatAPrefixedUnitCanBeCreatedWithWebApi()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/SiteDirectory/PrefixedUnit/PostNewPrefixedUnit.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

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
            Assert.That(organizations, Is.EquivalentTo(expectedOrganizations));

            var expectedPersons = new string[] { "77791b12-4c2c-4499-93fa-869df3692d22" };
            var personArray = (JArray)siteDirectory[PropertyNames.Person];
            IList<string> persons = personArray.Select(x => (string)x).ToList();
            Assert.That(persons, Is.EquivalentTo(expectedPersons));

            var expectedparticipantRole = new string[] { "ee3ae5ff-ac5e-4957-bab1-7698fba2a267" };
            var participantRoleArray = (JArray)siteDirectory[PropertyNames.ParticipantRole];
            IList<string> participantRoles = participantRoleArray.Select(x => (string)x).ToList();
            Assert.That(participantRoles, Is.EquivalentTo(expectedparticipantRole));

            var expectedsiteReferenceDataLibraries = new string[] { "c454c687-ba3e-44c4-86bc-44544b2c7880" };
            var siteReferenceDataLibraryArray = (JArray)siteDirectory[PropertyNames.SiteReferenceDataLibrary];
            IList<string> siteReferenceDataLibraries = siteReferenceDataLibraryArray.Select(x => (string)x).ToList();
            Assert.That(siteReferenceDataLibraries, Is.EquivalentTo(expectedsiteReferenceDataLibraries));

            var expectedModels = new string[] { "116f6253-89bb-47d4-aa24-d11d197e43c9" };
            var modelArray = (JArray)siteDirectory[PropertyNames.Model];
            IList<string> models = modelArray.Select(x => (string)x).ToList();
            Assert.That(models, Is.EquivalentTo(expectedModels));

            var expectedPersonRoles = new string[] { "2428f4d9-f26d-4112-9d56-1c940748df69" };
            var personRoleArray = (JArray)siteDirectory[PropertyNames.PersonRole];
            IList<string> personRoles = personRoleArray.Select(x => (string)x).ToList();
            Assert.That(personRoles, Is.EquivalentTo(expectedPersonRoles));

            var expectedlogEntries = new string[]
            {
                "98ba7b8a-1a1b-4569-a17c-b1ff620246a5",
                "66220289-e6ee-43cb-8fcd-d8e59a3dbf97"
            };
            var logEntryArray = (JArray)siteDirectory[PropertyNames.LogEntry];
            IList<string> logEntries = logEntryArray.Select(x => (string)x).ToList();
            Assert.That(logEntries, Is.EquivalentTo(expectedlogEntries));

            var expecteddomainGroups = new string[] { "86992db5-8ce2-4431-8ff5-6fe794d14687" };
            var domainGroupArray = (JArray)siteDirectory[PropertyNames.DomainGroup];
            IList<string> domainGroups = domainGroupArray.Select(x => (string)x).ToList();
            Assert.That(domainGroups, Is.EquivalentTo(expecteddomainGroups));

            var expectedDomains = new string[]
            {
                "0e92edde-fdff-41db-9b1d-f2e484f12535",
                "eb759723-14b9-49f4-8611-544d037bb764"
            };
            var domainArray = (JArray)siteDirectory[PropertyNames.Domain];
            IList<string> domains = domainArray.Select(x => (string)x).ToList();
            Assert.That(domains, Is.EquivalentTo(expectedDomains));

            var expectedNaturalLanguages = new string[] { "73bf30cc-3573-488f-8746-6c03ba47973e" };
            var naturalLanguageArray = (JArray)siteDirectory[PropertyNames.NaturalLanguage];
            IList<string> naturalLanguages = naturalLanguageArray.Select(x => (string)x).ToList();
            Assert.That(naturalLanguages, Is.EquivalentTo(expectedNaturalLanguages));

            // SiteReferenceDataLibrary
            var siteReferenceDataLibrary = jArray.Single(x => (string)x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");

            // verify the amount of returned properties 
            Assert.AreEqual(22, siteReferenceDataLibrary.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("c454c687-ba3e-44c4-86bc-44544b2c7880", (string)siteReferenceDataLibrary[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)siteReferenceDataLibrary[PropertyNames.RevisionNumber]);
            Assert.AreEqual("SiteReferenceDataLibrary", (string)siteReferenceDataLibrary[PropertyNames.ClassKind]);

            Assert.IsFalse((bool)siteReferenceDataLibrary[PropertyNames.IsDeprecated]);
            Assert.AreEqual("Test Reference Data Library", (string)siteReferenceDataLibrary[PropertyNames.Name]);
            Assert.AreEqual("TestRDL", (string)siteReferenceDataLibrary[PropertyNames.ShortName]);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)siteReferenceDataLibrary[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)siteReferenceDataLibrary[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)siteReferenceDataLibrary[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));

            var expectedDefinedCategories = new string[]
            {
                "cf059b19-235c-48be-87a3-9a8942c8e3e0",
                "107fc408-7e6d-4f1a-895a-1b6a6025ac20",
                "167b5cb0-766e-4ab2-b728-a9c9a662b017"
            };
            var definedCategoriesArray = (JArray)siteReferenceDataLibrary[PropertyNames.DefinedCategory];
            IList<string> definedCategoriesList = definedCategoriesArray.Select(x => (string)x).ToList();
            Assert.That(definedCategoriesList, Is.EquivalentTo(expectedDefinedCategories));

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
            var parameterTypesArray = (JArray)siteReferenceDataLibrary[PropertyNames.ParameterType];
            IList<string> parameterTypesList = parameterTypesArray.Select(x => (string)x).ToList();
            Assert.That(parameterTypesList, Is.EquivalentTo(expectedParameterTypes));

            var expectedBaseQuantityKinds = new List<OrderedItem>
            {
                new OrderedItem(16544539, "4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d")
            };
            var baseQuantityKindsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                siteReferenceDataLibrary[PropertyNames.BaseQuantityKind].ToString());
            Assert.That(baseQuantityKindsArray, Is.EquivalentTo(expectedBaseQuantityKinds));

            var expectedScales = new string[]
            {
                "6326d1ea-c032-4a4b-8b10-608c59f1a923",
                "007b0e60-e67c-4060-88d2-2531ef9e7d9e",
                "541037e2-9f6a-466c-b56f-a09f81f36576",
                "53e82aeb-c42c-475c-b6bf-a102af883471",
                "f9d4b3c6-91a2-4f38-bb86-f504d6ac706f"
            };
            var scalesArray = (JArray)siteReferenceDataLibrary[PropertyNames.Scale];
            IList<string> scalesList = scalesArray.Select(x => (string)x).ToList();
            Assert.That(scalesList, Is.EquivalentTo(expectedScales));

            var expectedUnitPrefixes = new string[]
            {
                "efa6380d-9508-4f3d-9b43-6ed33125b780"
            };
            var unitPrefixesArray = (JArray)siteReferenceDataLibrary[PropertyNames.UnitPrefix];
            IList<string> unitPrefixesList = unitPrefixesArray.Select(x => (string)x).ToList();
            Assert.That(unitPrefixesList, Is.EquivalentTo(expectedUnitPrefixes));

            var expectedUnits = new string[]
            {
                "56842970-3915-4369-8712-61cfd8273ef9",
                "12f48e1a-2996-46cc-8dc1-faf4e69ae115",
                "0f69c1f9-7896-45fc-830c-1e336d22a64a",
                "c394eaa9-4832-4b2d-8d88-5e1b2c43732c",
                "9fb8085f-a7d3-4068-95e3-686e4d954d1c"
            };
            var unitsArray = (JArray)siteReferenceDataLibrary[PropertyNames.Unit];
            IList<string> unitsList = unitsArray.Select(x => (string)x).ToList();
            Assert.That(unitsList, Is.EquivalentTo(expectedUnits));

            var expectedBaseUnits = new string[]
            {
                "56842970-3915-4369-8712-61cfd8273ef9"
            };
            var baseUnitsArray = (JArray)siteReferenceDataLibrary[PropertyNames.BaseUnit];
            IList<string> baseUnitsList = baseUnitsArray.Select(x => (string)x).ToList();
            Assert.That(baseUnitsList, Is.EquivalentTo(expectedBaseUnits));

            var expectedFileTypes = new string[]
            {
                "db04ac55-dd60-4607-a4e1-a9f91c9704e6",
                "b16894e4-acb5-4e81-a118-16c00eb86d8f",
                "f340df66-d65b-4814-a063-01d4dea1941c"
            };
            var fileTypesArray = (JArray)siteReferenceDataLibrary[PropertyNames.FileType];
            IList<string> fileTypesList = fileTypesArray.Select(x => (string)x).ToList();
            Assert.That(fileTypesList, Is.EquivalentTo(expectedFileTypes));

            var expectedGlossaries = new string[]
            {
                "bb08686b-ae03-49eb-9f48-c196b5ad6bda"
            };
            var glossariesArray = (JArray)siteReferenceDataLibrary[PropertyNames.Glossary];
            IList<string> glossariesList = glossariesArray.Select(x => (string)x).ToList();
            Assert.That(glossariesList, Is.EquivalentTo(expectedGlossaries));

            var expectedReferenceSources = new string[]
            {
                "ffd6c100-6c72-4d2a-8565-ff24bd576a89"
            };
            var referenceSourcesArray = (JArray)siteReferenceDataLibrary[PropertyNames.ReferenceSource];
            IList<string> referenceSourcesList = referenceSourcesArray.Select(x => (string)x).ToList();
            Assert.That(referenceSourcesList, Is.EquivalentTo(expectedReferenceSources));

            var expectedRules = new string[]
            {
                "8569bd5c-de3c-4d92-855f-b2c0ca94de0e",
                "8a5cd66e-7313-4843-813f-37081ca81bb8",
                "2615f9ec-30a4-4c0e-a9d3-1d067959c248",
                "7a6186ca-10c1-4074-bec1-4a92ce6ae59d",
                "e7e4eec5-ad39-40a0-9548-9c40d8e6df1b"
            };
            var rulesArray = (JArray)siteReferenceDataLibrary[PropertyNames.Rule];
            IList<string> rulesList = rulesArray.Select(x => (string)x).ToList();
            Assert.That(rulesList, Is.EquivalentTo(expectedRules));

            Assert.IsEmpty(siteReferenceDataLibrary["requiredRdl"]);

            var expectedConstants = new string[]
            {
                "239754fe-834f-4394-9c3a-26cac7f866d3"
            };
            var constantsArray = (JArray)siteReferenceDataLibrary[PropertyNames.Constant];
            IList<string> constantsList = constantsArray.Select(x => (string)x).ToList();
            Assert.That(constantsList, Is.EquivalentTo(expectedConstants));

            // PrefixedUnit
            var prefixedUnit = jArray.Single(x => (string)x[PropertyNames.Iid] == "9fb8085f-a7d3-4068-95e3-686e4d954d1c");
            
            // verify the amount of returned properties 
            Assert.AreEqual(9, prefixedUnit.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("9fb8085f-a7d3-4068-95e3-686e4d954d1c", (string)prefixedUnit[PropertyNames.Iid]);
            Assert.AreEqual(2, (int)prefixedUnit[PropertyNames.RevisionNumber]);
            Assert.AreEqual("PrefixedUnit", (string)prefixedUnit[PropertyNames.ClassKind]);

            Assert.IsFalse((bool)prefixedUnit[PropertyNames.IsDeprecated]);
            Assert.AreEqual("efa6380d-9508-4f3d-9b43-6ed33125b780", (string)prefixedUnit[PropertyNames.Prefix]);
            Assert.AreEqual("56842970-3915-4369-8712-61cfd8273ef9", (string)prefixedUnit[PropertyNames.ReferenceUnit]);

            var expectedPrefixedUnitAliases = new string[] { };
            var prefixedUnitAliasesArray = (JArray)prefixedUnit[PropertyNames.Alias];
            IList<string> prefixedUnitAliases = prefixedUnitAliasesArray.Select(x => (string)x).ToList();
            Assert.That(prefixedUnitAliases, Is.EquivalentTo(expectedPrefixedUnitAliases));

            var expectedPrefixedUnitDefinitions = new string[] { };
            var prefixedUnitDefinitionsArray = (JArray)prefixedUnit[PropertyNames.Definition];
            IList<string> prefixedUnitDefinitions = prefixedUnitDefinitionsArray.Select(x => (string)x).ToList();
            Assert.That(prefixedUnitDefinitions, Is.EquivalentTo(expectedPrefixedUnitDefinitions));

            var expectedPrefixedUnitHyperlinks = new string[] { };
            var prefixedUnitHyperlinksArray = (JArray)prefixedUnit[PropertyNames.HyperLink];
            IList<string> hyperLinks = prefixedUnitHyperlinksArray.Select(x => (string)x).ToList();
            Assert.That(hyperLinks, Is.EquivalentTo(expectedPrefixedUnitHyperlinks));
        }

        /// <summary>
        /// Verifies all properties of the PrefixedUnit <see cref="JToken"/>
        /// </summary>
        /// <param name="prefixedUnit">
        /// The <see cref="JToken"/> that contains the properties of
        /// the PrefixedUnit object
        /// </param>
        public static void VerifyProperties(JToken prefixedUnit)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(9, prefixedUnit.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("0f69c1f9-7896-45fc-830c-1e336d22a64a", (string) prefixedUnit["iid"]);
            Assert.AreEqual(1, (int) prefixedUnit["revisionNumber"]);
            Assert.AreEqual("PrefixedUnit", (string) prefixedUnit["classKind"]);

            Assert.IsFalse((bool) prefixedUnit["isDeprecated"]);
            Assert.AreEqual("efa6380d-9508-4f3d-9b43-6ed33125b780", (string) prefixedUnit["prefix"]);
            Assert.AreEqual("56842970-3915-4369-8712-61cfd8273ef9", (string) prefixedUnit["referenceUnit"]);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) prefixedUnit["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) prefixedUnit["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) prefixedUnit["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));
        }
    }
}
