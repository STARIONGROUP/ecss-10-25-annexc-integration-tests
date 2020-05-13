// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EngineeringModelSetupTestFixture.cs" company="RHEA System">
//
//   Copyright 2016-2020 RHEA System 
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

    using NUnit.Framework;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The purpose of the <see cref="EngineeringModelSetupTestFixture"/> is to GET and POST model objects
    /// </summary>
    [TestFixture]
    public class EngineeringModelSetupTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        public void VerifyThatNewEngineeringModelCanBeCreatedWithWebApi()
        {
            var siteDirectoryUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));
            var postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetup.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            //Check the amount of objects 
            Assert.AreEqual(5, jArray.Count);

            //SiteDirectory properties
            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.AreEqual(2, (int)siteDirectory[PropertyNames.RevisionNumber]);
            var expectedModels = new string[]
            {
                "116f6253-89bb-47d4-aa24-d11d197e43c9",
                "ba097bf8-c916-4134-8471-4a1eb4efb2f7"
            };
            var modelArray = (JArray)siteDirectory[PropertyNames.Model];
            IList<string> models = modelArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedModels, models);

            //EngineeringModelSetup properties
            var engineeringModelSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "ba097bf8-c916-4134-8471-4a1eb4efb2f7");
            var participantsArray = (JArray)engineeringModelSetup[PropertyNames.Participant];
            Assert.AreEqual(1, participantsArray.Count);

            Assert.AreEqual("1f3c2199-2ddf-4a52-a53a-97436a695d35", (string)engineeringModelSetup[PropertyNames.EngineeringModelIid]);
            Assert.AreEqual("integrationtest", (string)engineeringModelSetup[PropertyNames.Name]);
            Assert.AreEqual("integrationtest", (string)engineeringModelSetup[PropertyNames.ShortName]);
            Assert.AreEqual(2, (int)engineeringModelSetup[PropertyNames.RevisionNumber]);
            Assert.AreEqual("STUDY_MODEL", (string)engineeringModelSetup[PropertyNames.Kind]);
            Assert.AreEqual("PREPARATION_PHASE", (string)engineeringModelSetup[PropertyNames.StudyPhase]);
            Assert.AreEqual("EngineeringModelSetup", (string)engineeringModelSetup[PropertyNames.ClassKind]);
            Assert.IsEmpty(engineeringModelSetup[PropertyNames.SourceEngineeringModelSetupIid]);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)engineeringModelSetup[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)engineeringModelSetup[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)engineeringModelSetup[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            var expectedActiveDomains = new string[]
            {
                "0e92edde-fdff-41db-9b1d-f2e484f12535"
            };
            var activeDomainsArray = (JArray)engineeringModelSetup[PropertyNames.ActiveDomain];
            IList<string> activeDomainsList = activeDomainsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedActiveDomains, activeDomainsList);

            var expectedRequiredRdls = new string[]
            {
                "325a98b0-e8e9-4a7f-a038-98b9b618b705"
            };
            var requiredRdlsArray = (JArray)engineeringModelSetup[PropertyNames.RequiredRdl];
            IList<string> requiredRdlsList = requiredRdlsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedRequiredRdls, requiredRdlsList);

            var iterationSetupsArray = (JArray)engineeringModelSetup[PropertyNames.IterationSetup];
            Assert.AreEqual(1, iterationSetupsArray.Count);

            //ModelReferenceDataLibrary properties
            var modelReferenceDataLibrary = jArray.Single(x => (string)x[PropertyNames.Iid] == "325a98b0-e8e9-4a7f-a038-98b9b618b705");
            Assert.AreEqual(2, (int)modelReferenceDataLibrary[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ModelReferenceDataLibrary", (string)modelReferenceDataLibrary[PropertyNames.ClassKind]);
            Assert.AreEqual("integrationtest", (string)modelReferenceDataLibrary[PropertyNames.ShortName]);
            Assert.AreEqual("integrationtest Model RDL", (string)modelReferenceDataLibrary[PropertyNames.Name]);
            Assert.AreEqual("c454c687-ba3e-44c4-86bc-44544b2c7880", (string)modelReferenceDataLibrary[PropertyNames.RequiredRdl]);

            expectedAliases = new string[] { };
            aliasesArray = (JArray)modelReferenceDataLibrary[PropertyNames.Alias];
            aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedBaseQuantityKinds = new string[] { };
            var baseQuantityKindsArray = (JArray)modelReferenceDataLibrary[PropertyNames.BaseQuantityKind];
            IList<string> baseQuantityKinds = baseQuantityKindsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedBaseQuantityKinds, baseQuantityKinds);

            var expectedBaseUnits = new string[] { };
            var baseUnitsArray = (JArray)modelReferenceDataLibrary[PropertyNames.BaseUnit];
            IList<string> baseUnits = baseUnitsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedBaseUnits, baseUnits);

            var expectedConstants = new string[] { };
            var constantsArray = (JArray)modelReferenceDataLibrary[PropertyNames.Constant];
            IList<string> constants = constantsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedConstants, constants);

            var expectedDefinedCategories = new string[] { };
            var definedCategoriesArray = (JArray)modelReferenceDataLibrary[PropertyNames.DefinedCategory];
            IList<string> definedCategories = definedCategoriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinedCategories, definedCategories);

            expectedDefinitions = new string[] { };
            definitionsArray = (JArray)modelReferenceDataLibrary[PropertyNames.Definition];
            definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedFileTypes = new string[] { };
            var fileTypesArray = (JArray)modelReferenceDataLibrary[PropertyNames.FileType];
            IList<string> fileTypes = fileTypesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedFileTypes, fileTypes);

            var expectedGlossaries = new string[] { };
            var glossariesArray = (JArray)modelReferenceDataLibrary[PropertyNames.Glossary];
            IList<string> glossaries = glossariesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedGlossaries, glossaries);

            expectedHyperlinks = new string[] { };
            hyperlinksArray = (JArray)modelReferenceDataLibrary[PropertyNames.HyperLink];
            h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            var expectedParameterTypes = new string[] { };
            var parameterTypesArray = (JArray)modelReferenceDataLibrary[PropertyNames.ParameterType];
            IList<string> parameterTypes = parameterTypesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedParameterTypes, parameterTypes);

            var expectedReferenceSources = new string[] { };
            var referenceSourcesArray = (JArray)modelReferenceDataLibrary[PropertyNames.ReferenceSource];
            IList<string> referenceSources = referenceSourcesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedReferenceSources, referenceSources);

            var expectedRules = new string[] { };
            var rulesArray = (JArray)modelReferenceDataLibrary[PropertyNames.Rule];
            IList<string> rules = rulesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedRules, rules);

            var expectedScales = new string[] { };
            var scalesArray = (JArray)modelReferenceDataLibrary[PropertyNames.Scale];
            IList<string> scales = scalesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedScales, scales);

            var expectedUnitPrefixes = new string[] { };
            var unitPrefixesArray = (JArray)modelReferenceDataLibrary[PropertyNames.UnitPrefix];
            IList<string> unitPrefixes = unitPrefixesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedUnitPrefixes, unitPrefixes);

            var expectedUnits = new string[] { };
            var unitsArray = (JArray)modelReferenceDataLibrary[PropertyNames.Unit];
            IList<string> units = unitsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedUnits, units);

            //IterationSetup properties
            var iterationSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == iterationSetupsArray[0].ToString());
            Assert.AreEqual(2, (int)iterationSetup[PropertyNames.RevisionNumber]);
            Assert.AreEqual(1, (int)iterationSetup[PropertyNames.IterationNumber]);
            Assert.AreEqual("IterationSetup", (string)iterationSetup[PropertyNames.ClassKind]);
            Assert.AreEqual(false, (bool)iterationSetup[PropertyNames.IsDeleted]);
            Assert.IsEmpty(iterationSetup[PropertyNames.SourceIterationSetup]);
            Assert.IsEmpty(iterationSetup[PropertyNames.FrozenOn]);

            //Participant properties
            var participant = jArray.Single(x => (string)x[PropertyNames.Iid] == participantsArray[0].ToString());
            Assert.AreEqual("Participant", (string)participant[PropertyNames.ClassKind]);
            Assert.AreEqual(2, (int)participant[PropertyNames.RevisionNumber]);
            Assert.IsTrue((bool)participant[PropertyNames.IsActive]);
            Assert.AreEqual("77791b12-4c2c-4499-93fa-869df3692d22", (string)participant[PropertyNames.Person]);
            Assert.AreEqual("ee3ae5ff-ac5e-4957-bab1-7698fba2a267", (string)participant[PropertyNames.Role]);

            var expectedDomains = new string[]
            {
                "0e92edde-fdff-41db-9b1d-f2e484f12535"
            };
            var domainsArray = (JArray)participant[PropertyNames.Domain];
            IList<string> domains = domainsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDomains, domains);

            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)participant[PropertyNames.SelectedDomain]);

            //GET EngineeringModel
            // define the URI on which to perform a GET request
            var engineeringModelUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/1f3c2199-2ddf-4a52-a53a-97436a695d35"));

            // get a response from the data-source as a JArray (JSON Array)
            jArray = this.WebClient.GetDto(engineeringModelUri);

            //check if there is only one EngineeringModel object
            Assert.AreEqual(1, jArray.Count);

            // get a specific EngineeringModel from the result by it's unique id
            var engineeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "1f3c2199-2ddf-4a52-a53a-97436a695d35");

            Assert.AreEqual("EngineeringModel", (string)engineeringModel[PropertyNames.ClassKind]);
            Assert.AreEqual(1, (int)engineeringModel[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ba097bf8-c916-4134-8471-4a1eb4efb2f7", (string)engineeringModel[PropertyNames.EngineeringModelSetup]);
            var iterationsArray = (JArray)engineeringModel[PropertyNames.Iteration];
            Assert.AreEqual(1, iterationsArray.Count);

            var expectedCommonFileStores = new string[] { };
            var commonFileStoresArray = (JArray)engineeringModel[PropertyNames.CommonFileStore];
            IList<string> commonFileStores = commonFileStoresArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCommonFileStores, commonFileStores);

            var expectedLogEntries = new string[] { };
            var logEntriesArray = (JArray)engineeringModel[PropertyNames.LogEntry];
            IList<string> logEntries = logEntriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedLogEntries, logEntries);

            //GET Iteration
            // define the URI on which to perform a GET request
            var iterationUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/1f3c2199-2ddf-4a52-a53a-97436a695d35/iteration/" + iterationsArray[0].ToString()));

            // get a response from the data-source as a JArray (JSON Array)
            jArray = this.WebClient.GetDto(iterationUri);

            //check if there is only one Iteration object
            Assert.AreEqual(1, jArray.Count);

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == iterationsArray[0].ToString());

            Assert.AreEqual(1, (int)iteration[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Iteration", (string)iteration[PropertyNames.ClassKind]);

            Assert.AreEqual(iterationSetupsArray[0].ToString(), (string)iteration[PropertyNames.IterationSetup]);
            Assert.IsNull((string)iteration[PropertyNames.SourceIterationIid]);

            var expectedPublications = new string[] { };
            var publicationsArray = (JArray)iteration[PropertyNames.Publication];
            IList<string> publications = publicationsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPublications, publications);

            var expectedPossibleFiniteStateLists = new string[] { };
            var possibleFiniteStateListsArray = (JArray)iteration[PropertyNames.PossibleFiniteStateList];
            IList<string> possibleFiniteStateLists = possibleFiniteStateListsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPossibleFiniteStateLists, possibleFiniteStateLists);

            Assert.IsNull((string)iteration[PropertyNames.TopElement]);

            var expectedElements = new string[] { };
            var elementsArray = (JArray)iteration[PropertyNames.Element];
            IList<string> elements = elementsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedElements, elements);

            var expectedRelationships = new string[] { };
            var relationshipsArray = (JArray)iteration[PropertyNames.Relationship];
            IList<string> relationships = relationshipsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedRelationships, relationships);

            var expectedExternalIdentifierMaps = new string[] { };
            var externalIdentifierMapsArray = (JArray)iteration[PropertyNames.ExternalIdentifierMap];
            IList<string> externalIdentifierMaps = externalIdentifierMapsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedExternalIdentifierMaps, externalIdentifierMaps);

            var expectedRequirementsSpecifications = new string[] { };
            var requirementsSpecificationsArray = (JArray)iteration[PropertyNames.RequirementsSpecification];
            IList<string> requirementsSpecifications = requirementsSpecificationsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedRequirementsSpecifications, requirementsSpecifications);

            var domainFileStoresArray = (JArray)iteration[PropertyNames.DomainFileStore];
            Assert.AreEqual(0, domainFileStoresArray.Count);

            var expectedActualFiniteStateLists = new string[] { };
            var actualFiniteStateListsArray = (JArray)iteration[PropertyNames.ActualFiniteStateList];
            IList<string> actualFiniteStateLists = actualFiniteStateListsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedActualFiniteStateLists, actualFiniteStateLists);

            Assert.IsNull((string)iteration[PropertyNames.DefaultOption]);

            var expectedRuleVerificationLists = new string[] { };
            var ruleVerificationListsArray = (JArray)iteration[PropertyNames.RuleVerificationList];
            IList<string> ruleVerificationLists = ruleVerificationListsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedRuleVerificationLists, ruleVerificationLists);

            var optionsArray = (JArray)iteration[PropertyNames.Option];
            Assert.AreEqual(1, optionsArray.Count);
        }

        [Test]
        public void VerifyThatActiveDomainCanBeAddedToEngineeringModelSetupWithWebApi()
        {
            var siteDirectoryUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));
            var postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetup.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            // Check the amount of objects 
            Assert.AreEqual(5, jArray.Count);

            // Add active domain
            postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostAddActiveDomain.json");

            postBody = base.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            // SiteDirectory properties
            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.AreEqual(3, (int)siteDirectory[PropertyNames.RevisionNumber]);

            // EngineeringModelSetup properties
            var engineeringModelSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "ba097bf8-c916-4134-8471-4a1eb4efb2f7");

            Assert.AreEqual(3, (int)engineeringModelSetup[PropertyNames.RevisionNumber]);

            var expectedActiveDomains = new string[]
                                            {
                                                "0e92edde-fdff-41db-9b1d-f2e484f12535",
                                                "eb759723-14b9-49f4-8611-544d037bb764"
                                            };
            var activeDomainsArray = (JArray)engineeringModelSetup[PropertyNames.ActiveDomain];
            IList<string> activeDomainsList = activeDomainsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedActiveDomains, activeDomainsList);
        }

        [Test]
        public void VerifyThatNewEngineeringModelCanBeCreatedBasedOnExistingModelWithWebApi()
        {
            var siteDirectoryUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));
            var postBodyPath =
                this.GetPath(
                    "Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetupBasedOnExistingModel.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            //Check the amount of objects 
            Assert.AreEqual(9, jArray.Count);

            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.AreEqual(2, (int)siteDirectory[PropertyNames.RevisionNumber]);
            var expectedModels = new string[] { "116f6253-89bb-47d4-aa24-d11d197e43c9", "a54467e2-5cb4-450b-a081-1e2f8a6dcd80" };
            var modelArray = (JArray)siteDirectory[PropertyNames.Model];
            IList<string> models = modelArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedModels, models);

            var engineeringModelSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "a54467e2-5cb4-450b-a081-1e2f8a6dcd80");
            var participantsArray = (JArray)engineeringModelSetup[PropertyNames.Participant];
            Assert.AreEqual(1, participantsArray.Count);
            Assert.AreEqual("ae1eca97-77bd-4d0b-a17f-fc9fb76b04bd", (string)engineeringModelSetup[PropertyNames.EngineeringModelIid]);
            Assert.AreEqual("testderivefromexistingmodel", (string)engineeringModelSetup[PropertyNames.Name]);
            Assert.AreEqual("STUDY_MODEL", (string)engineeringModelSetup[PropertyNames.Kind]);
            Assert.AreEqual("PREPARATION_PHASE", (string)engineeringModelSetup[PropertyNames.StudyPhase]);

            var iterationSetup = jArray.Single(x => (string)x[PropertyNames.ClassKind] == "IterationSetup");
            Assert.AreEqual("IterationSetup Description", (string)iterationSetup["description"]);

            var modelReferenceDataLibrary = jArray.Single(x => (string)x[PropertyNames.ClassKind] == "ModelReferenceDataLibrary");
            Assert.AreEqual("TestModelReferenceDataLibrary", (string)modelReferenceDataLibrary[PropertyNames.ShortName]);
            Assert.AreEqual("Test Model Reference Data Library", (string)modelReferenceDataLibrary[PropertyNames.Name]);
            Assert.AreEqual("c454c687-ba3e-44c4-86bc-44544b2c7880", (string)modelReferenceDataLibrary[PropertyNames.RequiredRdl]);

            var participant = jArray.Single(x => (string)x[PropertyNames.ClassKind] == "Participant");
            Assert.IsTrue((bool)participant[PropertyNames.IsActive]);
            Assert.AreEqual("77791b12-4c2c-4499-93fa-869df3692d22", (string)participant[PropertyNames.Person]);
            var expectedDomains = new string[] {
                "0e92edde-fdff-41db-9b1d-f2e484f12535",
                "eb759723-14b9-49f4-8611-544d037bb764"
            };
            var domainsArray = (JArray)participant[PropertyNames.Domain];
            IList<string> domains = domainsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDomains, domains);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string)participant[PropertyNames.SelectedDomain]);
        }

        /// <summary>
        /// Verification that the EngineeringModelSetup objects are returned from the data-source and that the 
        /// values of the EngineeringModelSetup properties are equal to the expected values
        /// </summary>
        [Test]
        public void VerifyThatExpectedEngineeringModelSetupIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var engineeringModelSetupUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(engineeringModelSetupUri);

            //check if there is only one EngineeringModelSetup object
            Assert.AreEqual(1, jArray.Count);

            // get a specific EngineeringModelSetup from the result by it's unique id
            var engineeringModelSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "116f6253-89bb-47d4-aa24-d11d197e43c9");

            EngineeringModelSetupTestFixture.VerifyProperties(engineeringModelSetup);
        }

        [Test]
        public void VerifyThatExpectedEngineeringModelSetupWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var engineeringModelSetupsUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(engineeringModelSetupsUri);

            //check if there are only two objects
            Assert.AreEqual(2, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific PersonRole from the result by it's unique id
            var personRole = jArray.Single(x => (string)x[PropertyNames.Iid] == "116f6253-89bb-47d4-aa24-d11d197e43c9");
            EngineeringModelSetupTestFixture.VerifyProperties(personRole);
        }

        /// <summary>
        /// Verifies all properties of the EngineeringModelSetup <see cref="JToken"/>
        /// </summary>
        /// <param name="engineeringModelSetup">
        /// The <see cref="JToken"/> that contains the properties of
        /// the EngineeringModelSetup object
        /// </param>
        public static void VerifyProperties(JToken engineeringModelSetup)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(16, engineeringModelSetup.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("116f6253-89bb-47d4-aa24-d11d197e43c9", (string)engineeringModelSetup[PropertyNames.Iid]);
            Assert.AreEqual(1, (int)engineeringModelSetup[PropertyNames.RevisionNumber]);
            Assert.AreEqual("EngineeringModelSetup", (string)engineeringModelSetup[PropertyNames.ClassKind]);

            Assert.AreEqual("Test Engineering ModelSetup", (string)engineeringModelSetup[PropertyNames.Name]);
            Assert.AreEqual("TestEngineeringModelSetup", (string)engineeringModelSetup[PropertyNames.ShortName]);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray)engineeringModelSetup[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray)engineeringModelSetup[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray)engineeringModelSetup[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            Assert.IsEmpty(engineeringModelSetup[PropertyNames.SourceEngineeringModelSetupIid]);

            var expectedParticipants = new string[] { "284334dd-e8e5-42d6-bc8a-715c507a7f02" };
            var participantsArray = (JArray)engineeringModelSetup[PropertyNames.Participant];
            IList<string> participantsList = participantsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedParticipants, participantsList);

            var expectedActiveDomains = new string[] {
                "0e92edde-fdff-41db-9b1d-f2e484f12535",
                "eb759723-14b9-49f4-8611-544d037bb764"
            };
            var activeDomainsArray = (JArray)engineeringModelSetup[PropertyNames.ActiveDomain];
            IList<string> activeDomainsList = activeDomainsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedActiveDomains, activeDomainsList);

            Assert.AreEqual("STUDY_MODEL", (string)engineeringModelSetup[PropertyNames.Kind]);
            Assert.AreEqual("PREPARATION_PHASE", (string)engineeringModelSetup[PropertyNames.StudyPhase]);

            var expectedRequiredRdls = new string[] { "3483f2b5-ea29-45cc-8a46-f5f598558fc3" };
            var requiredRdlsArray = (JArray)engineeringModelSetup[PropertyNames.RequiredRdl];
            IList<string> requiredRdlsList = requiredRdlsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedRequiredRdls, requiredRdlsList);

            Assert.AreEqual("9ec982e4-ef72-4953-aa85-b158a95d8d56",
                (string)engineeringModelSetup[PropertyNames.EngineeringModelIid]);

            var expectedIterationSetups = new string[] { "86163b0e-8341-4316-94fc-93ed60ad0dcf" };
            var iterationSetupsArray = (JArray)engineeringModelSetup[PropertyNames.IterationSetup];
            IList<string> iterationSetupsList = iterationSetupsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedIterationSetups, iterationSetupsList);
        }

        [Test]
        public void VerifyThatDomainFileStoreWillNotBeCreateWhenDomainOfExpertiseWillBeAddedToExistingEngineeringModelSetup()
        {
            var siteDirectoryUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));

            var postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetup.json");
            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            var engineeringModelSetupsUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model"));

            jArray = this.WebClient.GetDto(engineeringModelSetupsUri);
            Assert.AreEqual(2, jArray.Count);
            var engineeringModelSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "ba097bf8-c916-4134-8471-4a1eb4efb2f7");
            Assert.AreEqual("ba097bf8-c916-4134-8471-4a1eb4efb2f7", (string)engineeringModelSetup[PropertyNames.Iid]);
            var model = (string)engineeringModelSetup[PropertyNames.EngineeringModelIid];
            Assert.AreEqual("1f3c2199-2ddf-4a52-a53a-97436a695d35", model);

            // Check DomainOfExpertise in EngineeringModelSetup
            var expectedActiveDomains = new string[]
            {
                "0e92edde-fdff-41db-9b1d-f2e484f12535"
            };

            var activeDomainsArray = (JArray)engineeringModelSetup[PropertyNames.ActiveDomain];
            IList<string> activeDomainsList = activeDomainsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedActiveDomains, activeDomainsList);

            // Check DomainFileStore in EngineeringModel correlated to EngineeringModelSetup
            var engineeringModelUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/" + model));

            jArray = this.WebClient.GetDto(engineeringModelUri);
            var engineeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == model.ToString());
            var iterationsArray = (JArray)engineeringModel[PropertyNames.Iteration];

            var iterationUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/1f3c2199-2ddf-4a52-a53a-97436a695d35/iteration/" + iterationsArray[0].ToString()));

            jArray = this.WebClient.GetDto(iterationUri);
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == iterationsArray[0].ToString());
            var domainFileStoresArray = (JArray)iteration[PropertyNames.DomainFileStore];
            Assert.AreEqual(0, domainFileStoresArray.Count);

            // Check DomainFileStore after postAdd DomainOfExpertise in EngineeringModelSetup
            postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostAddActiveDomain.json");
            postBody = base.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            engineeringModelSetupsUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model/ba097bf8-c916-4134-8471-4a1eb4efb2f7"));

            jArray = this.WebClient.GetDto(engineeringModelSetupsUri);
            engineeringModelSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "ba097bf8-c916-4134-8471-4a1eb4efb2f7");

            expectedActiveDomains = new string[]
            {
                "0e92edde-fdff-41db-9b1d-f2e484f12535",
                "eb759723-14b9-49f4-8611-544d037bb764"
            };

            activeDomainsArray = (JArray)engineeringModelSetup[PropertyNames.ActiveDomain];
            activeDomainsList = activeDomainsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedActiveDomains, activeDomainsList);

            jArray = this.WebClient.GetDto(iterationUri);
            iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == iterationsArray[0].ToString());
            domainFileStoresArray = (JArray)iteration[PropertyNames.DomainFileStore];
            Assert.AreEqual(0, domainFileStoresArray.Count);
        }

        [Test]
        public void VerifyThatDomainFileStoreWillNotBeDeletedWhenDomainOfExpertiseWillBeRemovedFromExistingEngineeringModelSetup()
        {
            var engineeringModelSetupsUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model"));

            var jArray = this.WebClient.GetDto(engineeringModelSetupsUri);
            Assert.AreEqual(1, jArray.Count);
            var engineeringModelSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "116f6253-89bb-47d4-aa24-d11d197e43c9");
            Assert.AreEqual("116f6253-89bb-47d4-aa24-d11d197e43c9", (string)engineeringModelSetup[PropertyNames.Iid]);
            var model = (string)engineeringModelSetup[PropertyNames.EngineeringModelIid];
            Assert.AreEqual("9ec982e4-ef72-4953-aa85-b158a95d8d56", model);

            // Check DomainOfExpertise in EngineeringModelSetup
            var expectedActiveDomains = new string[]
            {
                "0e92edde-fdff-41db-9b1d-f2e484f12535",
                "eb759723-14b9-49f4-8611-544d037bb764"
            };

            var activeDomainsArray = (JArray)engineeringModelSetup[PropertyNames.ActiveDomain];
            IList<string> activeDomainsList = activeDomainsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedActiveDomains, activeDomainsList);

            // Check DomainFileStore in EngineeringModel correlated to EngineeringModelSetup
            var engineeringModelUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/" + model));

            jArray = this.WebClient.GetDto(engineeringModelUri);
            var engineeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == model.ToString());
            var iterationsArray = (JArray)engineeringModel[PropertyNames.Iteration];

            var iterationUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/" + iterationsArray[0].ToString()));

            jArray = this.WebClient.GetDto(iterationUri);
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == iterationsArray[0].ToString());
            var domainFileStoresArray = (JArray)iteration[PropertyNames.DomainFileStore];
            Assert.AreEqual(1, domainFileStoresArray.Count);

            // Check DomainFileStore after postDelete DomainOfExpertise in EngineeringModelSetup
            var siteDirectoryUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));

            var postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostDeleteActiveDomain.json");
            var postBody = base.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            engineeringModelSetupsUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model/116f6253-89bb-47d4-aa24-d11d197e43c9"));

            jArray = this.WebClient.GetDto(engineeringModelSetupsUri);
            engineeringModelSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "116f6253-89bb-47d4-aa24-d11d197e43c9");

            expectedActiveDomains = new string[]
            {
                "eb759723-14b9-49f4-8611-544d037bb764"
            };

            activeDomainsArray = (JArray)engineeringModelSetup[PropertyNames.ActiveDomain];
            activeDomainsList = activeDomainsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedActiveDomains, activeDomainsList);

            jArray = this.WebClient.GetDto(iterationUri);
            iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == iterationsArray[0].ToString());
            domainFileStoresArray = (JArray)iteration[PropertyNames.DomainFileStore];
            Assert.AreEqual(1, domainFileStoresArray.Count);
        }
    }
}
