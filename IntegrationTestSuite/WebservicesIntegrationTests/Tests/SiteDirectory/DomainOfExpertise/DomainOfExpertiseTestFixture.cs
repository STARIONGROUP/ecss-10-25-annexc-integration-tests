// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainOfExpertiseTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2025 Starion Group S.A.
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
    using System.Net.Http;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class DomainOfExpertiseTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedDomainOfExpertiseIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var domainOfExpertiseUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/domain");

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(domainOfExpertiseUri);
           
            Assert.That(jArray.Count, Is.EqualTo(2));

            // get a specific DomainOfExpertise from the result by it's unique id
            var domainOfExpertise = jArray.Single(x => (string)x["iid"] == "0e92edde-fdff-41db-9b1d-f2e484f12535");
            DomainOfExpertiseTestFixture.VerifyProperties(domainOfExpertise);

            // get a specific DomainOfExpertise from the result by it's unique id
            domainOfExpertise = jArray.Single(x => (string)x["iid"] == "eb759723-14b9-49f4-8611-544d037bb764");
            DomainOfExpertiseTestFixture.VerifyProperties(domainOfExpertise);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedDomainOfExpertiseWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var domainOfExpertiseUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/domain?includeAllContainers=true");

            // Get the response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(domainOfExpertiseUri);
            
            Assert.That(jArray.Count, Is.EqualTo(3));

            var siteDirectory = jArray.Single(x => (string)x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific DomainOfExpertise from the result by it's unique id
            var domainOfExpertise = jArray.Single(x => (string)x["iid"] == "0e92edde-fdff-41db-9b1d-f2e484f12535");
            DomainOfExpertiseTestFixture.VerifyProperties(domainOfExpertise);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatASameAsContainerObjectCannotBeUpdatedWhenPersonIsNotTheOwnerWithWebApi()
        {
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);

            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/SiteDirectory/DomainOfExpertise/PostUpdatePersonPermission.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            Assert.DoesNotThrow(() => this.WebClient.PostDto(siteDirectoryUri, postBody));

            this.CreateNewWebClientForUser(userName, passWord);

            postBodyPath = this.GetPath("Tests/SiteDirectory/DomainOfExpertise/PostUpdateDomainOfExpertise.json");

            postBody = this.GetJsonFromFile(postBodyPath);

            // Jane is not allowed to update
            var exception = Assert.Catch<WebException>(() => this.WebClient.PostDto(siteDirectoryUri, postBody));
            var errorMessage = this.WebClient.ExtractExceptionStringFromResponse(exception.Response);
            Assert.That(((HttpWebResponse)exception.Response).StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(errorMessage, Does.Contain("The person Jane does not have an appropriate update permission for DomainOfExpertise."));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatCreateDomainOfExpertiseWorks()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");

            var postBodyPath = this.GetPath("Tests/SiteDirectory/DomainOfExpertise/PostCreateDomainOfExpertise.json");

            var postBody = this.GetJsonFromFile(postBodyPath);

            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            // define the URI on which to perform a GET request
            var domainOfExpertiseUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/domain?includeAllContainers=true");

            // Get the response from the data-source as a JArray (JSON Array)
            jArray = this.WebClient.GetDto(domainOfExpertiseUri);
            
            Assert.That(jArray.Count, Is.EqualTo(4));

            // get a specific DomainOfExpertise from the result by it's unique id
            var domainOfExpertise = jArray.Single(x => (string)x["iid"] == "509b87d6-4262-476a-a12e-ee337df0d618");
            DomainOfExpertiseTestFixture.VerifyProperties(domainOfExpertise);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatCreateDomainOfExpertiseWithWaitTimeWorks()
        {
            // a POST that completes within the wait time answers like a POST without wait time
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294?waitTime=30");

            var postBodyPath = this.GetPath("Tests/SiteDirectory/DomainOfExpertise/PostCreateDomainOfExpertise.json");

            var postBody = this.GetJsonFromFile(postBodyPath);

            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            // the response contains the created DomainOfExpertise
            var createdDomainOfExpertise = jArray.Single(x => (string)x["iid"] == "509b87d6-4262-476a-a12e-ee337df0d618");
            Assert.That((string)createdDomainOfExpertise["classKind"], Is.EqualTo("DomainOfExpertise"));

            // the DomainOfExpertise has been stored
            var domainOfExpertiseUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/domain?includeAllContainers=true");
            jArray = this.WebClient.GetDto(domainOfExpertiseUri);

            Assert.That(jArray.Count, Is.EqualTo(4));

            var domainOfExpertise = jArray.Single(x => (string)x["iid"] == "509b87d6-4262-476a-a12e-ee337df0d618");
            DomainOfExpertiseTestFixture.VerifyProperties(domainOfExpertise);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatDomainsOfExpertiseAreCreatedInTheBackgroundWhenThePostExceedsItsWaitTimeWithWebApi()
        {
            // creating this many DomainsOfExpertise in one POST takes well over a second, so the POST exceeds its
            // waitTime: it answers with its CometTask and the work continues in the background
            var domainOfExpertiseIids = CreateIids(BulkDomainOfExpertiseCount);
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294?waitTime=1");

            var cometTask = this.WebClient.PostJson(siteDirectoryUri, CreateBulkDomainOfExpertisePostBody(domainOfExpertiseIids, withUnknownCategory: false));

            Assert.That((int)cometTask["statusKind"], Is.EqualTo(CometTaskStatusKind.Processing));

            var finishedTask = this.WaitForCometTaskToFinish((string)cometTask["id"], TimeSpan.FromMinutes(10));

            Assert.Multiple(() =>
            {
                Assert.That((int)finishedTask["statusKind"], Is.EqualTo(CometTaskStatusKind.Succeeded), finishedTask.ToString());
                Assert.That((int)finishedTask["revision"], Is.EqualTo(2));
                Assert.That(finishedTask["error"]?.Type, Is.EqualTo(JTokenType.Null));
            });

            // the DomainsOfExpertise have been stored
            Assert.That(this.GetDomainOfExpertiseIids(), Is.SupersetOf(domainOfExpertiseIids));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatAPostThatFailsAfterItsWaitTimeEndsWithAFailedTaskWithWebApi()
        {
            // the last DomainOfExpertise refers to a Category that does not exist; that is only detected when the
            // transaction is committed, after the wait time, so the failure can only be reported through the CometTask
            var domainOfExpertiseIids = CreateIids(BulkDomainOfExpertiseCount);
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294?waitTime=1");

            var cometTask = this.WebClient.PostJson(siteDirectoryUri, CreateBulkDomainOfExpertisePostBody(domainOfExpertiseIids, withUnknownCategory: true));

            Assert.That((int)cometTask["statusKind"], Is.EqualTo(CometTaskStatusKind.Processing));

            var finishedTask = this.WaitForCometTaskToFinish((string)cometTask["id"], TimeSpan.FromMinutes(10));

            Assert.Multiple(() =>
            {
                Assert.That((int)finishedTask["statusKind"], Is.EqualTo(CometTaskStatusKind.Failed), finishedTask.ToString());
                Assert.That((string)finishedTask["error"], Is.Not.Empty);
            });

            // nothing has been stored
            Assert.That(this.GetDomainOfExpertiseIids(), Has.None.AnyOf(domainOfExpertiseIids));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatReadWorksForParticipantNotBeingPartOfADomainOfExpertise()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");

            var postBodyPath = this.GetPath("Tests/SiteDirectory/DomainOfExpertise/PostCreateDomainOfExpertise.json");

            var postBody = this.GetJsonFromFile(postBodyPath);

            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            postBodyPath = this.GetPath("Tests/SiteDirectory/DomainOfExpertise/PostUpdatePersonPermission.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            Assert.DoesNotThrow(() => this.WebClient.PostDto(siteDirectoryUri, postBody));

            // define the URI on which to perform a GET request
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/?extent=deep");

            jArray = this.WebClient.GetDto(iterationUri);

            Assert.That(jArray.Count, Is.GreaterThan(0));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatWriteIsAllowedForParticipantNotBeingPartOfADomainOfExpertiseWhenAccessRightIsMODIFY()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");

            var postBodyPath = this.GetPath("Tests/SiteDirectory/DomainOfExpertise/PostCreateDomainOfExpertise.json");

            var postBody = this.GetJsonFromFile(postBodyPath);

            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            postBodyPath = this.GetPath("Tests/SiteDirectory/DomainOfExpertise/PostUpdatePersonPermission.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            Assert.DoesNotThrow(() => this.WebClient.PostDto(siteDirectoryUri, postBody));

            // define the URI on which to perform a GET request
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/?extent=deep");

            jArray = this.WebClient.GetDto(iterationUri);

            Assert.That(jArray.Count, Is.GreaterThan(0));

            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            postBodyPath = this.GetPath("Tests/SiteDirectory/DomainOfExpertise/PostNewElementDefinition.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            Assert.That(jArray.Count, Is.GreaterThan(0));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatWriteIsNOTAllowedForParticipantNotBeingPartOfADomainOfExpertiseWhenAccessRightIsMODIFY_IF_PARTICIPANT()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");

            var postBodyPath = this.GetPath("Tests/SiteDirectory/DomainOfExpertise/PostCreateDomainOfExpertise.json");

            var postBody = this.GetJsonFromFile(postBodyPath);

            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            postBodyPath = this.GetPath("Tests/SiteDirectory/DomainOfExpertise/PostUpdatePersonPermission.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            Assert.DoesNotThrow(() => this.WebClient.PostDto(siteDirectoryUri, postBody));

            // define the URI on which to perform a GET request
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/?extent=deep");

            jArray = this.WebClient.GetDto(iterationUri);

            Assert.That(jArray.Count, Is.GreaterThan(0));

            postBodyPath = this.GetPath("Tests/SiteDirectory/DomainOfExpertise/PostUpdateParticipantPermission.json");

            postBody = this.GetJsonFromFile(postBodyPath);

            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            Assert.That(jArray.Count, Is.GreaterThan(0));

            iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            postBodyPath = this.GetPath("Tests/SiteDirectory/DomainOfExpertise/PostNewElementDefinition.json");

            postBody = this.GetJsonFromFile(postBodyPath);

            var exception = Assert.Catch<WebException>(() => this.WebClient.PostDto(iterationUri, postBody));
            
            Assert.That(((HttpWebResponse)exception.Response).StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        /// <summary>
        /// Verifies the properties of the DomainOfExpertise <see cref="JToken"/>
        /// </summary>
        /// <param name="domainOfExpertise">
        /// The <see cref="JToken"/> that contains the properties of
        /// the DomainOfExpertise object
        /// </param>
        public static void VerifyProperties(JToken domainOfExpertise)
        {
            // verify that the amount of returned properties 
            Assert.That(domainOfExpertise.Children().Count(), Is.EqualTo(10));

            if ((string)domainOfExpertise["iid"] == "0e92edde-fdff-41db-9b1d-f2e484f12535")
            {
                Assert.That((string)domainOfExpertise["iid"], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));
                Assert.That((int)domainOfExpertise["revisionNumber"], Is.EqualTo(1));
                Assert.That((string)domainOfExpertise["classKind"], Is.EqualTo("DomainOfExpertise"));

                // assert that the properties are what is expected
                Assert.That((string)domainOfExpertise["name"], Is.EqualTo("Test Domain of Expertise"));
                Assert.That((string)domainOfExpertise["shortName"], Is.EqualTo("TST"));
                Assert.That((bool)domainOfExpertise["isDeprecated"], Is.False);

                var expectedAliases = new string[] { };
                var aliasesArray = (JArray)domainOfExpertise["alias"];
                IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
                Assert.That(aliases, Is.EquivalentTo(expectedAliases));

                var expectedDefinitions = new string[] { "23658615-a170-4c0f-ba71-da1a15c736ca" };
                var definitionsArray = (JArray)domainOfExpertise["definition"];
                IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
                Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

                var expectedHyperlinks = new string[] { };
                var hyperlinksArray = (JArray)domainOfExpertise["hyperLink"];
                IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
                Assert.That(h, Is.EquivalentTo(expectedHyperlinks));

                var expectedCategories = new string[] { };
                var categoryArray = (JArray)domainOfExpertise["category"];
                IList<string> categories = categoryArray.Select(x => (string)x).ToList();
                Assert.That(categories, Is.EquivalentTo(expectedCategories));
            }

            if ((string)domainOfExpertise["iid"] == "eb759723-14b9-49f4-8611-544d037bb764")
            {
                Assert.That((string)domainOfExpertise["iid"], Is.EqualTo("eb759723-14b9-49f4-8611-544d037bb764"));
                Assert.That((int)domainOfExpertise["revisionNumber"], Is.EqualTo(1));
                Assert.That((string)domainOfExpertise["classKind"], Is.EqualTo("DomainOfExpertise"));

                // assert that the properties are what is expected
                Assert.That((string)domainOfExpertise["name"], Is.EqualTo("Additional Test Domain of Expertise"));
                Assert.That((string)domainOfExpertise["shortName"], Is.EqualTo("ADD"));
                Assert.That((bool)domainOfExpertise["isDeprecated"], Is.False);

                var expectedAliases = new string[] { };
                var aliasesArray = (JArray)domainOfExpertise["alias"];
                IList<string> aliases = aliasesArray.Select(x => (string)x).ToList();
                Assert.That(aliases, Is.EquivalentTo(expectedAliases));

                var expectedDefinitions = new string[] {};
                var definitionsArray = (JArray)domainOfExpertise["definition"];
                IList<string> definitions = definitionsArray.Select(x => (string)x).ToList();
                Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

                var expectedHyperlinks = new string[] { };
                var hyperlinksArray = (JArray)domainOfExpertise["hyperLink"];
                IList<string> h = hyperlinksArray.Select(x => (string)x).ToList();
                Assert.That(h, Is.EquivalentTo(expectedHyperlinks));

                var expectedCategories = new string[] { };
                var categoryArray = (JArray)domainOfExpertise["category"];
                IList<string> categories = categoryArray.Select(x => (string)x).ToList();
                Assert.That(categories, Is.EquivalentTo(expectedCategories));
            }
        }

        /// <summary>
        /// The number of DomainsOfExpertise in the POST that exceeds its waitTime of 1 second. Creating them takes about
        /// 10 seconds on a developer machine
        /// </summary>
        private const int BulkDomainOfExpertiseCount = 1000;

        /// <summary>
        /// Creates new iids
        /// </summary>
        /// <param name="count">The number of iids</param>
        /// <returns>The iids</returns>
        private static string[] CreateIids(int count)
        {
            return Enumerable.Range(0, count).Select(_ => Guid.NewGuid().ToString()).ToArray();
        }

        /// <summary>
        /// Creates the body of a POST that creates DomainsOfExpertise in the SiteDirectory
        /// </summary>
        /// <param name="domainOfExpertiseIids">The iids of the DomainsOfExpertise</param>
        /// <param name="withUnknownCategory">Whether the last DomainOfExpertise refers to a Category that does not exist</param>
        /// <returns>The POST body</returns>
        private static string CreateBulkDomainOfExpertisePostBody(string[] domainOfExpertiseIids, bool withUnknownCategory)
        {
            var creates = new JArray();

            for (var i = 0; i < domainOfExpertiseIids.Length; i++)
            {
                var category = withUnknownCategory && i == domainOfExpertiseIids.Length - 1 ? new JArray(Guid.NewGuid().ToString()) : new JArray();

                creates.Add(new JObject
                {
                    ["iid"] = domainOfExpertiseIids[i],
                    ["classKind"] = "DomainOfExpertise",
                    ["revisionNumber"] = 0,
                    ["shortName"] = $"BulkDomain{i}",
                    ["name"] = $"Bulk Domain {i}",
                    ["isDeprecated"] = false,
                    ["category"] = category,
                    ["alias"] = new JArray(),
                    ["definition"] = new JArray(),
                    ["hyperLink"] = new JArray()
                });
            }

            var postBody = new JObject
            {
                ["_delete"] = new JArray(),
                ["_create"] = creates,
                ["_update"] = new JArray(new JObject
                {
                    ["classKind"] = "SiteDirectory",
                    ["iid"] = "f13de6f8-b03a-46e7-a492-53b2f260f294",
                    ["domain"] = new JArray(domainOfExpertiseIids.Cast<object>().ToArray())
                })
            };

            return postBody.ToString();
        }

        /// <summary>
        /// Gets the iids of the DomainsOfExpertise in the SiteDirectory
        /// </summary>
        /// <returns>The iids</returns>
        private string[] GetDomainOfExpertiseIids()
        {
            var domainsUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/domain");

            return this.WebClient.GetDto(domainsUri)
                .Where(x => (string)x["classKind"] == "DomainOfExpertise")
                .Select(x => (string)x["iid"])
                .ToArray();
        }
    }
}
