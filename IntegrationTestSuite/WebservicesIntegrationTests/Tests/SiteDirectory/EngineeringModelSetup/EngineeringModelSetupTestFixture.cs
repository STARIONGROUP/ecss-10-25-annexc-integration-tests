// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EngineeringModelSetupTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2024 Starion Group S.A.
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
    public class EngineeringModelSetupTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("POST")]
        public void VerifyThatNewEngineeringModelCanBeCreatedWithWebApi()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetup.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            //Check the amount of objects 
            Assert.That(jArray.Count, Is.EqualTo(5));

            //SiteDirectory properties
            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.That((int)siteDirectory[PropertyNames.RevisionNumber], Is.EqualTo(2));

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

            Assert.Multiple(() =>
            {
                Assert.That(participantsArray.Count, Is.EqualTo(1));
                Assert.That((string)engineeringModelSetup[PropertyNames.EngineeringModelIid], Is.EqualTo("1f3c2199-2ddf-4a52-a53a-97436a695d35"));
                Assert.That((string)engineeringModelSetup[PropertyNames.Name], Is.EqualTo("integrationtest"));
                Assert.That((string)engineeringModelSetup[PropertyNames.ShortName], Is.EqualTo("integrationtest"));
                Assert.That((int)engineeringModelSetup[PropertyNames.RevisionNumber], Is.EqualTo(2));
                Assert.That((string)engineeringModelSetup[PropertyNames.Kind], Is.EqualTo("STUDY_MODEL"));
                Assert.That((string)engineeringModelSetup[PropertyNames.StudyPhase], Is.EqualTo("PREPARATION_PHASE"));
                Assert.That((string)engineeringModelSetup[PropertyNames.ClassKind], Is.EqualTo("EngineeringModelSetup"));
                Assert.IsEmpty(engineeringModelSetup[PropertyNames.SourceEngineeringModelSetupIid]);
            });

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
            Assert.That(iterationSetupsArray.Count, Is.EqualTo(1));

            //ModelReferenceDataLibrary properties
            var modelReferenceDataLibrary = jArray.Single(x => (string)x[PropertyNames.Iid] == "325a98b0-e8e9-4a7f-a038-98b9b618b705");

            Assert.Multiple(() =>
            {
                Assert.That((int)modelReferenceDataLibrary[PropertyNames.RevisionNumber], Is.EqualTo(2));
                Assert.That((string)modelReferenceDataLibrary[PropertyNames.ClassKind], Is.EqualTo("ModelReferenceDataLibrary"));
                Assert.That((string)modelReferenceDataLibrary[PropertyNames.ShortName], Is.EqualTo("integrationtest"));
                Assert.That((string)modelReferenceDataLibrary[PropertyNames.Name], Is.EqualTo("integrationtest Model RDL"));
                Assert.That((string)modelReferenceDataLibrary[PropertyNames.RequiredRdl], Is.EqualTo("c454c687-ba3e-44c4-86bc-44544b2c7880"));
            });

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

            Assert.Multiple(() =>
            {
                Assert.That((int)iterationSetup[PropertyNames.RevisionNumber], Is.EqualTo(2));
                Assert.That((int)iterationSetup[PropertyNames.IterationNumber], Is.EqualTo(1));
                Assert.That((string)iterationSetup[PropertyNames.ClassKind], Is.EqualTo("IterationSetup"));
                Assert.That((bool)iterationSetup[PropertyNames.IsDeleted], Is.EqualTo(false));
                Assert.That(iterationSetup[PropertyNames.SourceIterationSetup], Is.Empty);
                Assert.That(iterationSetup[PropertyNames.FrozenOn], Is.Empty);
            });

            //Participant properties
            var participant = jArray.Single(x => (string)x[PropertyNames.Iid] == participantsArray[0].ToString());

            Assert.Multiple(() =>
            {
                Assert.That((string)participant[PropertyNames.ClassKind], Is.EqualTo("Participant"));
                Assert.That((int)participant[PropertyNames.RevisionNumber], Is.EqualTo(2));
                Assert.That((bool)participant[PropertyNames.IsActive], Is.True);
                Assert.That((string)participant[PropertyNames.Person], Is.EqualTo("77791b12-4c2c-4499-93fa-869df3692d22"));
                Assert.That((string)participant[PropertyNames.Role], Is.EqualTo("ee3ae5ff-ac5e-4957-bab1-7698fba2a267"));
            });

            var expectedDomains = new string[]
            {
                "0e92edde-fdff-41db-9b1d-f2e484f12535"
            };

            var domainsArray = (JArray)participant[PropertyNames.Domain];
            IList<string> domains = domainsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDomains, domains);

            Assert.That((string)participant[PropertyNames.SelectedDomain], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            //GET EngineeringModel
            // define the URI on which to perform a GET request
            var engineeringModelUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/1f3c2199-2ddf-4a52-a53a-97436a695d35");

            // get a response from the data-source as a JArray (JSON Array)
            jArray = this.WebClient.GetDto(engineeringModelUri);

            //check if there is only one EngineeringModel object
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific EngineeringModel from the result by it's unique id
            var engineeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "1f3c2199-2ddf-4a52-a53a-97436a695d35");

            Assert.Multiple(() =>
            {
                Assert.That((string)engineeringModel[PropertyNames.ClassKind], Is.EqualTo("EngineeringModel"));
                Assert.That((int)engineeringModel[PropertyNames.RevisionNumber], Is.EqualTo(1));
                Assert.That((string)engineeringModel[PropertyNames.EngineeringModelSetup], Is.EqualTo("ba097bf8-c916-4134-8471-4a1eb4efb2f7"));
            });

            var iterationsArray = (JArray)engineeringModel[PropertyNames.Iteration];
            Assert.That(iterationsArray.Count, Is.EqualTo(1));

            var expectedCommonFileStores = new string[] { };
            var commonFileStoresArray = (JArray)engineeringModel[PropertyNames.CommonFileStore];
            IList<string> commonFileStores = commonFileStoresArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedCommonFileStores, commonFileStores);

            var expectedLogEntries = new string[] { };
            var logEntriesArray = (JArray)engineeringModel[PropertyNames.LogEntry];
            IList<string> logEntries = logEntriesArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedLogEntries, logEntries);

            // define the URI on which to perform a GET request
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/1f3c2199-2ddf-4a52-a53a-97436a695d35/iteration/{iterationsArray[0].ToString()}");

            // get a response from the data-source as a JArray (JSON Array)
            jArray = this.WebClient.GetDto(iterationUri);

            //check if there is only one Iteration object
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == iterationsArray[0].ToString());

            Assert.Multiple(() =>
            {
                Assert.That((int)iteration[PropertyNames.RevisionNumber], Is.EqualTo(1));
                Assert.That((string)iteration[PropertyNames.ClassKind], Is.EqualTo("Iteration"));

                Assert.That((string)iteration[PropertyNames.IterationSetup], Is.EqualTo(iterationSetupsArray[0].ToString()));
                Assert.That((string)iteration[PropertyNames.SourceIterationIid], Is.Null);
            });

            var expectedPublications = new string[] { };
            var publicationsArray = (JArray)iteration[PropertyNames.Publication];
            IList<string> publications = publicationsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPublications, publications);

            var expectedPossibleFiniteStateLists = new string[] { };
            var possibleFiniteStateListsArray = (JArray)iteration[PropertyNames.PossibleFiniteStateList];
            IList<string> possibleFiniteStateLists = possibleFiniteStateListsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPossibleFiniteStateLists, possibleFiniteStateLists);

            Assert.That((string)iteration[PropertyNames.TopElement], Is.Null);

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
            Assert.That(domainFileStoresArray.Count, Is.EqualTo(0));

            var expectedActualFiniteStateLists = new string[] { };
            var actualFiniteStateListsArray = (JArray)iteration[PropertyNames.ActualFiniteStateList];
            IList<string> actualFiniteStateLists = actualFiniteStateListsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedActualFiniteStateLists, actualFiniteStateLists);

            Assert.That((string)iteration[PropertyNames.DefaultOption], Is.Null);

            var expectedRuleVerificationLists = new string[] { };
            var ruleVerificationListsArray = (JArray)iteration[PropertyNames.RuleVerificationList];
            IList<string> ruleVerificationLists = ruleVerificationListsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedRuleVerificationLists, ruleVerificationLists);

            var optionsArray = (JArray)iteration[PropertyNames.Option];
            Assert.That(optionsArray.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatActiveDomainCanBeAddedToEngineeringModelSetupWithWebApi()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetup.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            // Check the amount of objects 
            Assert.That(jArray.Count, Is.EqualTo(5));

            // Add active domain
            postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostAddActiveDomain.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            // SiteDirectory properties
            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.That((int)siteDirectory[PropertyNames.RevisionNumber], Is.EqualTo(3));

            // EngineeringModelSetup properties
            var engineeringModelSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "ba097bf8-c916-4134-8471-4a1eb4efb2f7");

            Assert.That((int)engineeringModelSetup[PropertyNames.RevisionNumber], Is.EqualTo(3));

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
        [Category("POST")]
        public void VerifyThatNewEngineeringModelCanBeCreatedBasedOnExistingModelWithWebApi()
        {
            //GET old Iteration for checks later in this testfixture
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c?extent=deep");
            var jArray = this.WebClient.GetDto(iterationUri);
            Assert.That(jArray.Count, Is.EqualTo(50));

            var oldCommonFileStore = jArray.Single(x => (string)x[PropertyNames.Name] == "TestFileStore");
            Assert.That(oldCommonFileStore[PropertyNames.CreatedOn], Is.Not.Null); // Important for validity of later checks!

            var oldDomainFileStore = jArray.Single(x => (string)x[PropertyNames.Name] == "Test DomainFileStore");
            Assert.That(oldDomainFileStore[PropertyNames.CreatedOn], Is.Not.Null); // Important for validity of later checks!

            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetupBasedOnExistingModel.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            Assert.That(jArray.Count, Is.EqualTo(9));

            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.That((int)siteDirectory[PropertyNames.RevisionNumber], Is.EqualTo(2));
            var expectedModels = new string[] { "116f6253-89bb-47d4-aa24-d11d197e43c9", "a54467e2-5cb4-450b-a081-1e2f8a6dcd80" };
            var modelArray = (JArray)siteDirectory[PropertyNames.Model];
            IList<string> models = modelArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedModels, models);

            var engineeringModelSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "a54467e2-5cb4-450b-a081-1e2f8a6dcd80");
            var participantsArray = (JArray)engineeringModelSetup[PropertyNames.Participant];

            Assert.Multiple(() =>
            {
                Assert.That(participantsArray.Count, Is.EqualTo(1));
                Assert.That((string)engineeringModelSetup[PropertyNames.EngineeringModelIid], Is.EqualTo("ae1eca97-77bd-4d0b-a17f-fc9fb76b04bd"));
                Assert.That((string)engineeringModelSetup[PropertyNames.Name], Is.EqualTo("testderivefromexistingmodel"));
                Assert.That((string)engineeringModelSetup[PropertyNames.Kind], Is.EqualTo("STUDY_MODEL"));
                Assert.That((string)engineeringModelSetup[PropertyNames.StudyPhase], Is.EqualTo("PREPARATION_PHASE"));
            });

            var iterationSetup = jArray.Single(x => (string)x[PropertyNames.ClassKind] == "IterationSetup");

            Assert.Multiple(() =>
            {
                Assert.That((string)iterationSetup["description"], Is.EqualTo("IterationSetup Description"));
                Assert.That((DateTime)iterationSetup["createdOn"], Is.Not.Null);
                Assert.That(iterationSetup["modifiedOn"], Is.Null);
            });

            var modelReferenceDataLibrary = jArray.Single(x => (string)x[PropertyNames.ClassKind] == "ModelReferenceDataLibrary");

            Assert.Multiple(() =>
            {
                Assert.That((string)modelReferenceDataLibrary[PropertyNames.ShortName], Is.EqualTo("testderivefromexistingmodelMRDL"));
                Assert.That((string)modelReferenceDataLibrary[PropertyNames.Name], Is.EqualTo("testderivefromexistingmodel Model RDL"));
                Assert.That((string)modelReferenceDataLibrary[PropertyNames.RequiredRdl], Is.EqualTo("c454c687-ba3e-44c4-86bc-44544b2c7880"));
            });

            var participant = jArray.Single(x => (string)x[PropertyNames.ClassKind] == "Participant");

            Assert.Multiple(() =>
            {
                Assert.That((bool)participant[PropertyNames.IsActive], Is.True);
                Assert.That((string)participant[PropertyNames.Person], Is.EqualTo("77791b12-4c2c-4499-93fa-869df3692d22"));
            });

            var expectedDomains = new string[]
            {
                "0e92edde-fdff-41db-9b1d-f2e484f12535",
                "eb759723-14b9-49f4-8611-544d037bb764"
            };

            var domainsArray = (JArray)participant[PropertyNames.Domain];
            IList<string> domains = domainsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDomains, domains);
            Assert.That((string)participant[PropertyNames.SelectedDomain], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            //Check if model can be read
            //GET new Iteration
            // define the URI on which to perform a GET request
            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/{engineeringModelSetup[PropertyNames.EngineeringModelIid]}/iteration/{iterationSetup[PropertyNames.IterationIid]}?extent=deep");

            // get a response from the data-source as a JArray (JSON Array)
            jArray = this.WebClient.GetDto(iterationUri);

            //check if there is only one Iteration object
            Assert.That(jArray.Count, Is.EqualTo(50));

            var newCommonFileStore = jArray.Single(x => (string)x[PropertyNames.Name] == "TestFileStore");
            var newDomainFileStore = jArray.Single(x => (string)x[PropertyNames.Name] == "Test DomainFileStore");

            Assert.Multiple(() =>
            {
                Assert.That(newCommonFileStore[PropertyNames.CreatedOn], Is.Not.EqualTo(oldCommonFileStore[PropertyNames.CreatedOn]));
                Assert.That(newDomainFileStore[PropertyNames.CreatedOn], Is.Not.EqualTo(oldDomainFileStore[PropertyNames.CreatedOn]));
            });
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedEngineeringModelSetupIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var engineeringModelSetupUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(engineeringModelSetupUri);

            //check if there is only one EngineeringModelSetup object
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific EngineeringModelSetup from the result by it's unique id
            var engineeringModelSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "116f6253-89bb-47d4-aa24-d11d197e43c9");

            VerifyProperties(engineeringModelSetup);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedEngineeringModelSetupWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var engineeringModelSetupsUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(engineeringModelSetupsUri);

            //check if there are only two objects
            Assert.That(jArray.Count, Is.EqualTo(2));

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific PersonRole from the result by it's unique id
            var personRole = jArray.Single(x => (string)x[PropertyNames.Iid] == "116f6253-89bb-47d4-aa24-d11d197e43c9");
            VerifyProperties(personRole);
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
            Assert.That(engineeringModelSetup.Children().Count(), Is.EqualTo(16));

            // assert that the properties are what is expected

            Assert.Multiple(() =>
            {
                Assert.That((string)engineeringModelSetup[PropertyNames.Iid], Is.EqualTo("116f6253-89bb-47d4-aa24-d11d197e43c9"));
                Assert.That((int)engineeringModelSetup[PropertyNames.RevisionNumber], Is.EqualTo(1));
                Assert.That((string)engineeringModelSetup[PropertyNames.ClassKind], Is.EqualTo("EngineeringModelSetup"));

                Assert.That((string)engineeringModelSetup[PropertyNames.Name], Is.EqualTo("Test Engineering ModelSetup"));
                Assert.That((string)engineeringModelSetup[PropertyNames.ShortName], Is.EqualTo("TestEngineeringModelSetup"));
            });

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

            Assert.That(engineeringModelSetup[PropertyNames.SourceEngineeringModelSetupIid], Is.Empty);

            var expectedParticipants = new string[] { "284334dd-e8e5-42d6-bc8a-715c507a7f02" };
            var participantsArray = (JArray)engineeringModelSetup[PropertyNames.Participant];
            IList<string> participantsList = participantsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedParticipants, participantsList);

            var expectedActiveDomains = new string[]
            {
                "0e92edde-fdff-41db-9b1d-f2e484f12535",
                "eb759723-14b9-49f4-8611-544d037bb764"
            };

            var activeDomainsArray = (JArray)engineeringModelSetup[PropertyNames.ActiveDomain];
            IList<string> activeDomainsList = activeDomainsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedActiveDomains, activeDomainsList);

            Assert.Multiple(() =>
            {
                Assert.That((string)engineeringModelSetup[PropertyNames.Kind], Is.EqualTo("STUDY_MODEL"));
                Assert.That((string)engineeringModelSetup[PropertyNames.StudyPhase], Is.EqualTo("PREPARATION_PHASE"));
            });

            var expectedRequiredRdls = new string[] { "3483f2b5-ea29-45cc-8a46-f5f598558fc3" };
            var requiredRdlsArray = (JArray)engineeringModelSetup[PropertyNames.RequiredRdl];
            IList<string> requiredRdlsList = requiredRdlsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedRequiredRdls, requiredRdlsList);

            Assert.That((string)engineeringModelSetup[PropertyNames.EngineeringModelIid], Is.EqualTo("9ec982e4-ef72-4953-aa85-b158a95d8d56"));

            var expectedIterationSetups = new string[] { "86163b0e-8341-4316-94fc-93ed60ad0dcf" };
            var iterationSetupsArray = (JArray)engineeringModelSetup[PropertyNames.IterationSetup];
            IList<string> iterationSetupsList = iterationSetupsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedIterationSetups, iterationSetupsList);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatDomainFileStoreWillNotBeCreateWhenDomainOfExpertiseWillBeAddedToExistingEngineeringModelSetup()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");

            var postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetup.json");
            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            var engineeringModelSetupsUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model");

            jArray = this.WebClient.GetDto(engineeringModelSetupsUri);
            Assert.That(jArray.Count, Is.EqualTo(2));
            var engineeringModelSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "ba097bf8-c916-4134-8471-4a1eb4efb2f7");
            Assert.That((string)engineeringModelSetup[PropertyNames.Iid], Is.EqualTo("ba097bf8-c916-4134-8471-4a1eb4efb2f7"));
            var model = (string)engineeringModelSetup[PropertyNames.EngineeringModelIid];
            Assert.That(model, Is.EqualTo("1f3c2199-2ddf-4a52-a53a-97436a695d35"));

            // Check DomainOfExpertise in EngineeringModelSetup
            var expectedActiveDomains = new string[]
            {
                "0e92edde-fdff-41db-9b1d-f2e484f12535"
            };

            var activeDomainsArray = (JArray)engineeringModelSetup[PropertyNames.ActiveDomain];
            IList<string> activeDomainsList = activeDomainsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedActiveDomains, activeDomainsList);

            // Check DomainFileStore in EngineeringModel correlated to EngineeringModelSetup
            var engineeringModelUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/{model}");

            jArray = this.WebClient.GetDto(engineeringModelUri);
            var engineeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == model.ToString());
            var iterationsArray = (JArray)engineeringModel[PropertyNames.Iteration];

            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/1f3c2199-2ddf-4a52-a53a-97436a695d35/iteration/{iterationsArray[0].ToString()}");

            jArray = this.WebClient.GetDto(iterationUri);
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == iterationsArray[0].ToString());
            var domainFileStoresArray = (JArray)iteration[PropertyNames.DomainFileStore];
            Assert.That(domainFileStoresArray.Count, Is.EqualTo(0));

            // Check DomainFileStore after postAdd DomainOfExpertise in EngineeringModelSetup
            postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostAddActiveDomain.json");
            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            engineeringModelSetupsUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model/ba097bf8-c916-4134-8471-4a1eb4efb2f7");

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
            Assert.That(domainFileStoresArray.Count, Is.EqualTo(0));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatDomainFileStoreWillNotBeDeletedWhenDomainOfExpertiseWillBeRemovedFromExistingEngineeringModelSetup()
        {
            var engineeringModelSetupsUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model");

            var jArray = this.WebClient.GetDto(engineeringModelSetupsUri);
            Assert.That(jArray.Count, Is.EqualTo(1));
            var engineeringModelSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "116f6253-89bb-47d4-aa24-d11d197e43c9");
            Assert.That((string)engineeringModelSetup[PropertyNames.Iid], Is.EqualTo("116f6253-89bb-47d4-aa24-d11d197e43c9"));
            var model = (string)engineeringModelSetup[PropertyNames.EngineeringModelIid];
            Assert.That(model, Is.EqualTo("9ec982e4-ef72-4953-aa85-b158a95d8d56"));

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
            var engineeringModelUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/{model}");

            jArray = this.WebClient.GetDto(engineeringModelUri);
            var engineeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == model.ToString());
            var iterationsArray = (JArray)engineeringModel[PropertyNames.Iteration];

            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/{iterationsArray[0].ToString()}");

            jArray = this.WebClient.GetDto(iterationUri);
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == iterationsArray[0].ToString());
            var domainFileStoresArray = (JArray)iteration[PropertyNames.DomainFileStore];
            Assert.That(domainFileStoresArray.Count, Is.EqualTo(1));

            // Check DomainFileStore after postDelete DomainOfExpertise in EngineeringModelSetup
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");

            var postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostDeleteActiveDomain.json");
            var postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            engineeringModelSetupsUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model/116f6253-89bb-47d4-aa24-d11d197e43c9");

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
            Assert.That(domainFileStoresArray.Count, Is.EqualTo(1));
        }
    }
}
