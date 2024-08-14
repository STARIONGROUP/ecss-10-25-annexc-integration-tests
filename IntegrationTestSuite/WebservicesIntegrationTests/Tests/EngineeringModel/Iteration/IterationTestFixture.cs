// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IterationTestFixture.cs" company="Starion Group S.A.">
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
    using System.Net;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using WebservicesIntegrationTests.Net;

    [TestFixture]
    public class IterationTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedIterationIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(iterationUri);

            //check if there is the only one Iteration object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");

            VerifyProperties(iteration);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedIterationWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var iterationUri =
                new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(iterationUri);

            //check if there are 2 objects
            Assert.That(jArray.Count, Is.EqualTo(2));

            // get a specific Iteration from the result by it's unique id
            var iteration =
                jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");

            VerifyProperties(iteration);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatFrozenIterationDoesNotAcceptModelChanges()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/ElementDefinition/PostNewElementDefinition.json");
            var postBody = this.GetJsonFromFile(postBodyPath);

            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.Nothing);

            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            postBodyPath = this.GetPath("Tests/EngineeringModel/Iteration/POSTNewIterationSetup.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            //Check the amount of objects 
            Assert.AreEqual(4, jArray.Count);

            postBodyPath = this.GetPath("Tests/EngineeringModel/ElementDefinition/PostNewElementDefinition.json");
            postBody = this.GetJsonFromFile(postBodyPath);

            Assert.That(() => this.WebClient.PostDto(iterationUri, postBody), Throws.TypeOf<WebException>());

            var checkError = false;

            try
            {
                this.WebClient.PostDto(iterationUri, postBody);
            }
            catch (Exception ex)
            {
                checkError = true;
                var webException = ex as WebException;

                Assert.Multiple(() =>
                {
                    Assert.That(webException, Is.Not.Null);
                    Assert.That(webException.Response.Headers.AllKeys.Contains("CDP-Error-Tag"));
                    Assert.That(webException.Response.Headers.GetValues("CDP-Error-Tag").Single(), Is.EqualTo("#FROZEN_ITERATION"));
                });
            }

            Assert.That(checkError, Is.True, () => "Catch block was not accessed, while that was expected.");
        }

        [Test]
        [Category("POST")]
        public void VerifyThatExpectedIterationCanBeCreatedFromWebApi()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Iteration/POSTNewIterationSetup.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            //Check the amount of objects 
            Assert.That(jArray.Count, Is.EqualTo(4));

            //SiteDirectory properties
            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.That((int)siteDirectory[PropertyNames.RevisionNumber], Is.EqualTo(2));

            //EngineeringModelSetup properties
            var engineeringModelSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "116f6253-89bb-47d4-aa24-d11d197e43c9");
            Assert.That((int)engineeringModelSetup[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var expectedIterationSetups = new string[]
            {
                "836e6e3c-722f-49a7-b8fa-3fc7f4ac9531",
                "86163b0e-8341-4316-94fc-93ed60ad0dcf"
            };

            var iterationSetupsArray = (JArray)engineeringModelSetup[PropertyNames.IterationSetup];
            IList<string> iterationSetups = iterationSetupsArray.Select(x => (string)x).ToList();
            Assert.That(iterationSetups, Is.EquivalentTo(expectedIterationSetups));

            //IterationSetups properties
            //Existing iterationSetup
            var iterationSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "86163b0e-8341-4316-94fc-93ed60ad0dcf");
            Assert.That((int)iterationSetup[PropertyNames.RevisionNumber], Is.EqualTo(2));

            //New iterationSetup
            iterationSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "836e6e3c-722f-49a7-b8fa-3fc7f4ac9531");

            Assert.Multiple(() =>
            {
                Assert.That((int)iterationSetup[PropertyNames.RevisionNumber], Is.EqualTo(2));
                Assert.That((int)iterationSetup[PropertyNames.IterationNumber], Is.EqualTo(2));
                Assert.That((string)iterationSetup[PropertyNames.ClassKind], Is.EqualTo("IterationSetup"));
                Assert.That((string)iterationSetup[PropertyNames.Description], Is.EqualTo("IterationSetup Description"));
                Assert.That((string)iterationSetup[PropertyNames.IterationIid], Is.EqualTo("699da906-d22e-4969-b606-1fcb4bf5affd"));
                Assert.That((bool)iterationSetup[PropertyNames.IsDeleted], Is.EqualTo(false));
                Assert.That(iterationSetup[PropertyNames.SourceIterationSetup], Is.Empty);
                Assert.That(iterationSetup[PropertyNames.FrozenOn], Is.Empty);
                Assert.That(iterationSetup[PropertyNames.ModifiedOn], Is.Null); //null means unchanged, which is th expected result
                Assert.That(iterationSetup[PropertyNames.CreatedOn], Is.Empty); // empty means unchanged, which is the expected result
            });

            // GET EngineeringModel
            var engineeringModelUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56");

            // get a response from the data-source as a JArray (JSON Array)
            jArray = this.WebClient.GetDto(engineeringModelUri);

            // check if there is only one EngineeringModel object
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific EngineeringModel from the result by it's unique id
            var engineeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var expectedIterations = new string[]
            {
                "e163c5ad-f32b-4387-b805-f4b34600bc2c",
                "699da906-d22e-4969-b606-1fcb4bf5affd"
            };

            var iterationsArray = (JArray)engineeringModel[PropertyNames.Iteration];
            IList<string> iterations = iterationsArray.Select(x => (string)x).ToList();
            Assert.That(iterations, Is.EquivalentTo(expectedIterations));

            // GET Iteration
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/699da906-d22e-4969-b606-1fcb4bf5affd");

            // get a response from the data-source as a JArray (JSON Array)
            jArray = this.WebClient.GetDto(iterationUri);

            //check if there is only one Iteration object
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "699da906-d22e-4969-b606-1fcb4bf5affd");

            Assert.Multiple(() =>
            {
                Assert.That((int)iteration[PropertyNames.RevisionNumber], Is.EqualTo(2));
                Assert.That((string)iteration[PropertyNames.ClassKind], Is.EqualTo("Iteration"));

                Assert.That((string)iteration[PropertyNames.IterationSetup], Is.EqualTo("836e6e3c-722f-49a7-b8fa-3fc7f4ac9531"));
                Assert.That((string)iteration[PropertyNames.SourceIterationIid], Is.EqualTo("e163c5ad-f32b-4387-b805-f4b34600bc2c"));
            });

            var expectedOptions = new List<OrderedItem>
            {
                new OrderedItem(1, "bebcc9f4-ff20-4569-bbf6-d1acf27a8107")
            };

            var optionsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                iteration[PropertyNames.Option].ToString());

            Assert.That(optionsArray, Is.EquivalentTo(expectedOptions));

            var expectedPublications = new string[]
            {
                "790b9e60-476b-4b6d-8aba-0af15178535e"
            };

            var publicationsArray = (JArray)iteration[PropertyNames.Publication];
            IList<string> publications = publicationsArray.Select(x => (string)x).ToList();
            CollectionAssert.IsEmpty(publications); // publication shoudl be empty

            var expectedPossibleFiniteStateLists = new string[]
            {
                "449a5bca-34fd-454a-93f8-a56ac8383fee"
            };

            var possibleFiniteStateListsArray = (JArray)iteration[PropertyNames.PossibleFiniteStateList];
            IList<string> possibleFiniteStateLists = possibleFiniteStateListsArray.Select(x => (string)x).ToList();
            Assert.That(possibleFiniteStateLists, Is.EquivalentTo(expectedPossibleFiniteStateLists));

            Assert.That((string)iteration[PropertyNames.TopElement], Is.Null);

            var expectedElements = new string[]
            {
                "f73860b2-12f0-43e4-b8b2-c81862c0a159",
                "fe9295c5-af99-494e-86ff-e715837806ae"
            };

            var elementsArray = (JArray)iteration[PropertyNames.Element];
            IList<string> elements = elementsArray.Select(x => (string)x).ToList();
            Assert.That(elements, Is.EquivalentTo(expectedElements));

            var expectedRelationships = new string[]
            {
                "138f8a3e-69c6-4e21-b459-bc26b1319a2c"
            };

            var relationshipsArray = (JArray)iteration[PropertyNames.Relationship];
            IList<string> relationships = relationshipsArray.Select(x => (string)x).ToList();
            Assert.That(relationships, Is.EquivalentTo(expectedRelationships));

            var expectedExternalIdentifierMaps = new string[]
            {
                "a0cadcd1-b14f-4552-8f97-bec386a715d0"
            };

            var externalIdentifierMapsArray = (JArray)iteration[PropertyNames.ExternalIdentifierMap];
            IList<string> externalIdentifierMaps = externalIdentifierMapsArray.Select(x => (string)x).ToList();
            Assert.That(externalIdentifierMaps, Is.EquivalentTo(expectedExternalIdentifierMaps));

            var expectedRequirementsSpecifications = new string[]
            {
                "bf0cde90-9086-43d5-bcff-32a2f8331800",
                "8d0734f4-ca4b-4611-9187-f6970e2b02bc"
            };

            var requirementsSpecificationsArray = (JArray)iteration[PropertyNames.RequirementsSpecification];
            IList<string> requirementsSpecifications = requirementsSpecificationsArray.Select(x => (string)x).ToList();
            Assert.That(requirementsSpecifications, Is.EquivalentTo(expectedRequirementsSpecifications));

            var expectedDomainFileStores = new string[]
            {
                "da7dddaa-02aa-4897-9935-e8d66c811a96"
            };

            var domainFileStoresArray = (JArray)iteration[PropertyNames.DomainFileStore];
            IList<string> domainFileStores = domainFileStoresArray.Select(x => (string)x).ToList();
            Assert.That(domainFileStores, Is.EquivalentTo(expectedDomainFileStores));

            var expectedActualFiniteStateLists = new string[] { "db690d7d-761c-47fd-96d3-840d698a89dc" };
            var actualFiniteStateListsArray = (JArray)iteration[PropertyNames.ActualFiniteStateList];
            IList<string> actualFiniteStateLists = actualFiniteStateListsArray.Select(x => (string)x).ToList();
            Assert.That(actualFiniteStateLists, Is.EquivalentTo(expectedActualFiniteStateLists));

            Assert.That((string)iteration[PropertyNames.DefaultOption], Is.EqualTo("bebcc9f4-ff20-4569-bbf6-d1acf27a8107"));

            var expectedRuleVerificationLists = new string[]
            {
                "dc482120-2a11-439b-913d-6a924de9ee5f"
            };

            var ruleVerificationListsArray = (JArray)iteration[PropertyNames.RuleVerificationList];
            IList<string> ruleVerificationLists = ruleVerificationListsArray.Select(x => (string)x).ToList();
            Assert.That(ruleVerificationLists, Is.EquivalentTo(expectedRuleVerificationLists));
        }

        [Test]
        [Category("POST")]
        [CdpVersion_1_1_0]
        public void VerifyModifiedOnAndCreatedOnWorkAsExpected()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Iteration/POSTNewIterationSetup.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            //Check the amount of objects 
            Assert.That(jArray.Count, Is.EqualTo(4));

            var iterationSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "86163b0e-8341-4316-94fc-93ed60ad0dcf");
            Assert.That((int)iterationSetup[PropertyNames.RevisionNumber], Is.EqualTo(2));

            //New iterationSetup
            iterationSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "836e6e3c-722f-49a7-b8fa-3fc7f4ac9531");

            Assert.Multiple(() =>
            {
                Assert.That((int)iterationSetup[PropertyNames.RevisionNumber], Is.EqualTo(2));
                Assert.That((int)iterationSetup[PropertyNames.IterationNumber], Is.EqualTo(2));
                Assert.That((string)iterationSetup[PropertyNames.ClassKind], Is.EqualTo("IterationSetup"));
                Assert.That((string)iterationSetup[PropertyNames.Description], Is.EqualTo("IterationSetup Description"));
                Assert.That((string)iterationSetup[PropertyNames.IterationIid], Is.EqualTo("699da906-d22e-4969-b606-1fcb4bf5affd"));
                Assert.That((bool)iterationSetup[PropertyNames.IsDeleted], Is.EqualTo(false));
                Assert.That(iterationSetup[PropertyNames.SourceIterationSetup], Is.Empty);
                Assert.That(iterationSetup[PropertyNames.FrozenOn], Is.Empty);
                Assert.That(iterationSetup[PropertyNames.ModifiedOn], Is.Empty); //empty in this case means unchanged, which is the expected result
                Assert.That(iterationSetup[PropertyNames.CreatedOn], Is.Empty); // empty in this case means unchanged, which is the expected result
            });
        }

        [Test]
        [Category("POST")]
        public void VerifyThatIterationSetupCanBeMarkAsDeletedFromWebApi()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Iteration/POSTNewIterationSetup.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            //Existing iterationSetup
            var iterationSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "86163b0e-8341-4316-94fc-93ed60ad0dcf");

            Assert.Multiple(() =>
            {
                Assert.That((int)iterationSetup[PropertyNames.RevisionNumber], Is.EqualTo(2));
                Assert.That((int)iterationSetup[PropertyNames.IterationNumber], Is.EqualTo(1));
                Assert.That((string)iterationSetup[PropertyNames.ClassKind], Is.EqualTo("IterationSetup"));
                Assert.That((string)iterationSetup[PropertyNames.Description], Is.EqualTo("IterationSetup Description"));
                Assert.That((string)iterationSetup[PropertyNames.IterationIid], Is.EqualTo("e163c5ad-f32b-4387-b805-f4b34600bc2c"));
                Assert.That((bool)iterationSetup[PropertyNames.IsDeleted], Is.EqualTo(false));
                Assert.That(iterationSetup[PropertyNames.SourceIterationSetup], Is.Empty);
                Assert.That(iterationSetup[PropertyNames.FrozenOn], Is.Not.Null);
            });

            //New iterationSetup
            iterationSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "836e6e3c-722f-49a7-b8fa-3fc7f4ac9531");

            Assert.Multiple(() =>
            {
                Assert.That((int)iterationSetup[PropertyNames.RevisionNumber], Is.EqualTo(2));
                Assert.That((int)iterationSetup[PropertyNames.IterationNumber], Is.EqualTo(2));
                Assert.That((string)iterationSetup[PropertyNames.ClassKind], Is.EqualTo("IterationSetup"));
                Assert.That((string)iterationSetup[PropertyNames.Description], Is.EqualTo("IterationSetup Description"));
                Assert.That((string)iterationSetup[PropertyNames.IterationIid], Is.EqualTo("699da906-d22e-4969-b606-1fcb4bf5affd"));
                Assert.That((bool)iterationSetup[PropertyNames.IsDeleted], Is.EqualTo(false));
                Assert.That(iterationSetup[PropertyNames.SourceIterationSetup], Is.Empty);
                Assert.That(iterationSetup[PropertyNames.FrozenOn], Is.Empty);
            });

            //Check iteration before iterationSetup delete
            var engineeringModelUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56");
            jArray = this.WebClient.GetDto(engineeringModelUri);

            var engineeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            var expectedIterations = new string[]
            {
                "699da906-d22e-4969-b606-1fcb4bf5affd",
                "e163c5ad-f32b-4387-b805-f4b34600bc2c"
            };

            var iterationsArray = (JArray)engineeringModel[PropertyNames.Iteration];
            IList<string> iterations = iterationsArray.Select(x => (string)x).ToList();
            Assert.That(iterations, Is.EquivalentTo(expectedIterations));

            //PostDelete iterationSetup
            var iterationSetupUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model/116f6253-89bb-47d4-aa24-d11d197e43c9");
            postBodyPath = this.GetPath("Tests/EngineeringModel/Iteration/PostDeleteIterationSetup.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            //Check deleted iterationSetup
            iterationSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "86163b0e-8341-4316-94fc-93ed60ad0dcf");

            Assert.Multiple(() =>
            {
                Assert.That((int)iterationSetup[PropertyNames.RevisionNumber], Is.EqualTo(3));
                Assert.That((int)iterationSetup[PropertyNames.IterationNumber], Is.EqualTo(1));
                Assert.That((string)iterationSetup[PropertyNames.ClassKind], Is.EqualTo("IterationSetup"));
                Assert.That((string)iterationSetup[PropertyNames.Description], Is.EqualTo("IterationSetup Description"));
                Assert.That((string)iterationSetup[PropertyNames.IterationIid], Is.EqualTo("e163c5ad-f32b-4387-b805-f4b34600bc2c"));
                Assert.That((bool)iterationSetup[PropertyNames.IsDeleted], Is.EqualTo(true));
                Assert.That(iterationSetup[PropertyNames.SourceIterationSetup], Is.Empty);
                Assert.That(iterationSetup[PropertyNames.FrozenOn], Is.Not.Null);
            });

            //Check existing iterationSetup
            iterationSetupUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/model/116f6253-89bb-47d4-aa24-d11d197e43c9/iterationSetup");
            jArray = this.WebClient.GetDto(iterationSetupUri);

            Assert.That(jArray.Count, Is.EqualTo(2));
            iterationSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "86163b0e-8341-4316-94fc-93ed60ad0dcf");

            Assert.Multiple(() =>
            {
                Assert.That((bool)iterationSetup[PropertyNames.IsDeleted], Is.EqualTo(true));
                Assert.That(iterationSetup[PropertyNames.FrozenOn], Is.Empty);
            });

            iterationSetup = jArray.Single(x => (string)x[PropertyNames.Iid] == "836e6e3c-722f-49a7-b8fa-3fc7f4ac9531");

            Assert.Multiple(() =>
            {
                Assert.That((bool)iterationSetup[PropertyNames.IsDeleted], Is.EqualTo(false));
                Assert.That(iterationSetup[PropertyNames.SourceIterationSetup], Is.Empty);
                Assert.That(iterationSetup[PropertyNames.FrozenOn], Is.Empty);
            });

            //Check existing iteration after delete iterationSetup
            engineeringModelUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56");
            jArray = this.WebClient.GetDto(engineeringModelUri);

            engineeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeringModel[PropertyNames.RevisionNumber], Is.EqualTo(3));

            expectedIterations = new string[]
            {
                "699da906-d22e-4969-b606-1fcb4bf5affd"
            };

            iterationsArray = (JArray)engineeringModel[PropertyNames.Iteration];
            iterations = iterationsArray.Select(x => (string)x).ToList();
            Assert.That(iterations, Is.EquivalentTo(expectedIterations));

            var deletedIterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var exception = Assert.Catch<WebException>(() => this.WebClient.GetDto(deletedIterationUri));
            Assert.That(((HttpWebResponse)exception.Response).StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatRelationshipAsPropertyDeletionFromIterationCanBeDoneFromWebApi()
        {
            var uri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postBodyPath = this.GetPath("Tests/EngineeringModel/Iteration/PostDeleteRelationshipAsProperty.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(uri, postBody);

            Assert.Multiple(() =>
            {
                Assert.That(jArray.Count, Is.EqualTo(2));
                Assert.That(jArray[0]["classKind"].ToString(), Is.EqualTo("EngineeringModel"));
                Assert.That(jArray[1]["classKind"].ToString(), Is.EqualTo("Iteration"));
            });
        }

        [Test]
        [Category("POST")]
        public void VerifyThatRelationshipDeletionFromIterationCanBeDoneFromWebApi()
        {
            var uri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Iteration/PostDeleteRelationship.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(uri, postBody);

            Assert.Multiple(() =>
            {
                Assert.That(jArray.Count, Is.EqualTo(2));
                Assert.That(jArray[0]["classKind"].ToString(), Is.EqualTo("EngineeringModel"));
                Assert.That(jArray[1]["classKind"].ToString(), Is.EqualTo("Iteration"));
            });
        }

        /// <summary>
        /// Verifies all properties of the Iteration <see cref="JToken"/>
        /// </summary>
        /// <param name="iteration">
        /// The <see cref="JToken"/> that contains the properties of
        /// the Iteration object
        /// </param>
        public static void VerifyProperties(JToken iteration)
        {
            Assert.Multiple(() =>
            {
                // verify the amount of returned properties 
                Assert.That(iteration.Children().Count(), Is.EqualTo(17));

                // assert that the properties are what is expected
                Assert.That((string)iteration[PropertyNames.Iid], Is.EqualTo("e163c5ad-f32b-4387-b805-f4b34600bc2c"));
                Assert.That((int)iteration[PropertyNames.RevisionNumber], Is.EqualTo(1));
                Assert.That((string)iteration[PropertyNames.ClassKind], Is.EqualTo("Iteration"));

                Assert.That((string)iteration[PropertyNames.IterationSetup], Is.EqualTo("86163b0e-8341-4316-94fc-93ed60ad0dcf"));
                Assert.IsNull((string)iteration[PropertyNames.SourceIterationIid]);
            });

            var expectedOptions = new List<OrderedItem>
            {
                new OrderedItem(1, "bebcc9f4-ff20-4569-bbf6-d1acf27a8107")
            };

            var optionsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                iteration[PropertyNames.Option].ToString());

            Assert.That(optionsArray, Is.EquivalentTo(expectedOptions));

            var expectedPublications = new string[]
            {
                "790b9e60-476b-4b6d-8aba-0af15178535e"
            };

            var publicationsArray = (JArray)iteration[PropertyNames.Publication];
            IList<string> publications = publicationsArray.Select(x => (string)x).ToList();
            Assert.That(publications, Is.EquivalentTo(expectedPublications));

            var expectedPossibleFiniteStateLists = new string[]
            {
                "449a5bca-34fd-454a-93f8-a56ac8383fee"
            };

            var possibleFiniteStateListsArray = (JArray)iteration[PropertyNames.PossibleFiniteStateList];
            IList<string> possibleFiniteStateLists = possibleFiniteStateListsArray.Select(x => (string)x).ToList();
            Assert.That(possibleFiniteStateLists, Is.EquivalentTo(expectedPossibleFiniteStateLists));

            Assert.That((string)iteration[PropertyNames.TopElement], Is.Null);

            var expectedElements = new string[]
            {
                "f73860b2-12f0-43e4-b8b2-c81862c0a159",
                "fe9295c5-af99-494e-86ff-e715837806ae"
            };

            var elementsArray = (JArray)iteration[PropertyNames.Element];
            IList<string> elements = elementsArray.Select(x => (string)x).ToList();
            Assert.That(elements, Is.EquivalentTo(expectedElements));

            var expectedRelationships = new string[]
            {
                "320869e4-f6d6-4dd2-a696-1b1604f4c4b7",
                "138f8a3e-69c6-4e21-b459-bc26b1319a2c"
            };

            var relationshipsArray = (JArray)iteration[PropertyNames.Relationship];
            IList<string> relationships = relationshipsArray.Select(x => (string)x).ToList();
            Assert.That(relationships, Is.EquivalentTo(expectedRelationships));

            var expectedExternalIdentifierMaps = new string[]
            {
                "a0cadcd1-b14f-4552-8f97-bec386a715d0"
            };

            var externalIdentifierMapsArray = (JArray)iteration[PropertyNames.ExternalIdentifierMap];
            IList<string> externalIdentifierMaps = externalIdentifierMapsArray.Select(x => (string)x).ToList();
            Assert.That(externalIdentifierMaps, Is.EquivalentTo(expectedExternalIdentifierMaps));

            var expectedRequirementsSpecifications = new string[]
                { "bf0cde90-9086-43d5-bcff-32a2f8331800", "8d0734f4-ca4b-4611-9187-f6970e2b02bc" };

            var requirementsSpecificationsArray = (JArray)iteration[PropertyNames.RequirementsSpecification];
            IList<string> requirementsSpecifications = requirementsSpecificationsArray.Select(x => (string)x).ToList();
            Assert.That(requirementsSpecifications, Is.EquivalentTo(expectedRequirementsSpecifications));

            var expectedDomainFileStores = new string[]
            {
                "da7dddaa-02aa-4897-9935-e8d66c811a96"
            };

            var domainFileStoresArray = (JArray)iteration[PropertyNames.DomainFileStore];
            IList<string> domainFileStores = domainFileStoresArray.Select(x => (string)x).ToList();
            Assert.That(domainFileStores, Is.EquivalentTo(expectedDomainFileStores));

            var expectedActualFiniteStateLists = new string[] { "db690d7d-761c-47fd-96d3-840d698a89dc" };
            var actualFiniteStateListsArray = (JArray)iteration[PropertyNames.ActualFiniteStateList];
            IList<string> actualFiniteStateLists = actualFiniteStateListsArray.Select(x => (string)x).ToList();
            Assert.That(actualFiniteStateLists, Is.EquivalentTo(expectedActualFiniteStateLists));

            Assert.That((string)iteration[PropertyNames.DefaultOption], Is.EqualTo("bebcc9f4-ff20-4569-bbf6-d1acf27a8107"));

            var expectedRuleVerificationLists = new string[]
            {
                "dc482120-2a11-439b-913d-6a924de9ee5f"
            };

            var ruleVerificationListsArray = (JArray)iteration[PropertyNames.RuleVerificationList];
            IList<string> ruleVerificationLists = ruleVerificationListsArray.Select(x => (string)x).ToList();
            Assert.That(ruleVerificationLists, Is.EquivalentTo(expectedRuleVerificationLists));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatASameAsContainerObjectCannotBeUpdatedWhenParticipantHsReadAccessWithWebApi()
        {
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);

            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/EngineeringModel/Iteration/PostUpdateAccessRights.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            Assert.That(() => this.WebClient.PostDto(siteDirectoryUri, postBody), Throws.Nothing);

            this.CreateNewWebClientForUser(userName, passWord);

            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postBodyPath = this.GetPath("Tests/EngineeringModel/Iteration/PostUpdateIteration.json");

            postBody = this.GetJsonFromFile(postBodyPath);

            // Jane is not allowed to update
            var exception = Assert.Catch<WebException>(() => this.WebClient.PostDto(iterationUri, postBody));
            var errorMessage = this.WebClient.ExtractExceptionStringFromResponse(exception.Response);
            Assert.That(((HttpWebResponse)exception.Response).StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(errorMessage.Contains("The person Jane does not have an appropriate update permission for Iteration."), Is.True);
        }
    }
}
