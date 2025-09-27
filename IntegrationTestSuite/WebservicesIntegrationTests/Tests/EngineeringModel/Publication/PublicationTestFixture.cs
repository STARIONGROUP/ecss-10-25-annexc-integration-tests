// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PublicationTestFixture.cs" company="Starion Group S.A.">
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
    public class PublicationTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedPublicationTestIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var publicationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/publication");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(publicationUri);

            //check if there is the only one Publication object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific Publication from the result by it's unique id
            var publication = jArray.Single(x => (string) x[PropertyNames.Iid] == "790b9e60-476b-4b6d-8aba-0af15178535e");

            PublicationTestFixture.VerifyProperties(publication);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedPublicationWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var publicationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/publication?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(publicationUri);

            //check if there are 3 objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            IterationTestFixture.VerifyProperties(iteration);

            // get a specific Publication from the result by it's unique id
            var publication = jArray.Single(x => (string) x[PropertyNames.Iid] == "790b9e60-476b-4b6d-8aba-0af15178535e");
            PublicationTestFixture.VerifyProperties(publication);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatPublicationCanBePostedAndAppropriateObjectsAreReturnedFromWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postBodyPath = this.GetPath("Tests/EngineeringModel/Publication/PostUpdateParameterValueSet.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(iterationUri, postBody);

            var engineeeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int)engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific ParameterValueSet from the result by it's unique id
            var parameterValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == "72ec3701-bcb5-4bf6-bd78-30fd1b65e3be");

            Assert.AreEqual(2, (int)parameterValueSet[PropertyNames.RevisionNumber]);
            Assert.AreEqual("[\"test\"]", (string)parameterValueSet[PropertyNames.Manual]);

            postBodyPath = this.GetPath("Tests/EngineeringModel/Publication/PostNewPublication.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(iterationUri, postBody);

            engineeeringModel = jArray.Single(x => (string)x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(3, (int)engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string)x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");
            Assert.AreEqual(3, (int)iteration[PropertyNames.RevisionNumber]);

            var expectedPublications = new string[]
                                           {
                                               "790b9e60-476b-4b6d-8aba-0af15178535e",
                                               "aec92178-186c-40d5-b23a-78f0423906f6"
                                           };
            var publicationsArray = (JArray)iteration[PropertyNames.Publication];
            IList<string> publications = publicationsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPublications, publications);

            parameterValueSet = jArray.Single(x => (string)x[PropertyNames.Iid] == "72ec3701-bcb5-4bf6-bd78-30fd1b65e3be");
            Assert.AreEqual(3, (int)parameterValueSet[PropertyNames.RevisionNumber]);
            Assert.AreEqual("[\"test\"]", (string)parameterValueSet[PropertyNames.Published]);

            // get a specific Publication from the result by it's unique id
            var publication = jArray.Single(x => (string)x[PropertyNames.Iid] == "aec92178-186c-40d5-b23a-78f0423906f6");

            Assert.AreEqual(3, (int)publication[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Publication", (string)publication[PropertyNames.ClassKind]);

            var expectedDomains = new string[]
                                      {
                                          "0e92edde-fdff-41db-9b1d-f2e484f12535",
                                          "eb759723-14b9-49f4-8611-544d037bb764"
                                      };
            var domainsArray = (JArray)publication[PropertyNames.Domain];
            IList<string> domains = domainsArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedDomains, domains);

            var expectedPublishedParameters = new string[]
                                                  {
                                                      "3f05483f-66ff-4f21-bc76-45956779f66e"
                                                  };
            var publishedParametersArray = (JArray)publication[PropertyNames.PublishedParameter];
            IList<string> publishedParameters = publishedParametersArray.Select(x => (string)x).ToList();
            CollectionAssert.AreEquivalent(expectedPublishedParameters, publishedParameters);
        }

        /// <summary>
        /// Verifies all properties of the Publication <see cref="JToken"/>
        /// </summary>
        /// <param name="publication">
        /// The <see cref="JToken"/> that contains the properties of
        /// the Publication object
        /// </param>
        public static void VerifyProperties(JToken publication)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(6, publication.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("790b9e60-476b-4b6d-8aba-0af15178535e", (string) publication[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) publication[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Publication", (string) publication[PropertyNames.ClassKind]);

            Assert.AreEqual("2016-10-25T09:00:35.936Z", (string) publication[PropertyNames.CreatedOn]);

            var expectedDomains = new string[] {};
            var domainsArray = (JArray) publication[PropertyNames.Domain];
            IList<string> domains = domainsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDomains, domains);

            var expectedPublishedParameters = new string[] {};
            var publishedParametersArray = (JArray) publication[PropertyNames.PublishedParameter];
            IList<string> publishedParameters = publishedParametersArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedPublishedParameters, publishedParameters);
        }
    }
}
