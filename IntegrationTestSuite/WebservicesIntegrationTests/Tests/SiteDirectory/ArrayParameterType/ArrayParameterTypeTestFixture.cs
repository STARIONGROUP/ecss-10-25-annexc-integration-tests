// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayParameterTypeTestFixture.cs" company="RHEA System S.A.">
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
    public class ArrayParameterTypeTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterTypeIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var arrayParameterTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/4a783624-b2bc-4e6d-95b3-11d036f6e917");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(arrayParameterTypeUri);

            //check if there is the only one ArrayParameterType object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific ArrayParameterType from the result by it's unique id
            var arrayParameterType = jArray.Single(x => (string) x[PropertyNames.Iid] == "4a783624-b2bc-4e6d-95b3-11d036f6e917");

            VerifyProperties(arrayParameterType);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatArrayParameterTypeCanBePosted()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            
            var postBodyPath = this.GetPath("Tests/SiteDirectory/ArrayParameterType/PostArrayParameterType.json");
            var postBody = this.GetJsonFromFile(postBodyPath);

            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            //Check the amount of objects 
            Assert.AreEqual(5, jArray.Count);

            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.AreEqual(2, (int) siteDirectory[PropertyNames.RevisionNumber]);

            var arrayParameterType = jArray.Single(x => (string) x[PropertyNames.Iid] == "267b0d27-0421-453d-951a-4fcacc309a27");
            Assert.AreEqual(2, (int) arrayParameterType[PropertyNames.RevisionNumber]);
            Assert.AreEqual("cta", (string) arrayParameterType[PropertyNames.ShortName]);
            Assert.AreEqual("createTestArray", (string) arrayParameterType[PropertyNames.Name]);
            Assert.AreEqual("cta_s", (string) arrayParameterType[PropertyNames.Symbol]);

            var component_1 = jArray.Single(x => (string) x[PropertyNames.Iid] == "f51de2a2-279e-4b5e-8f07-bbc7e9993a6b");
            Assert.AreEqual(2, (int) component_1[PropertyNames.RevisionNumber]);
            Assert.AreEqual("4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d", (string) component_1[PropertyNames.ParameterType]);
            Assert.AreEqual("53e82aeb-c42c-475c-b6bf-a102af883471", (string) component_1[PropertyNames.Scale]);
            Assert.AreEqual("{1;1}", (string) component_1[PropertyNames.ShortName]);

            var component_2 = jArray.Single(x => (string) x[PropertyNames.Iid] == "0715f517-1f8b-462d-9189-b4ff20548266");
            Assert.AreEqual(2, (int) component_2[PropertyNames.RevisionNumber]);
            Assert.AreEqual("35a9cf05-4eba-4cda-b60c-7cfeaac8f892", (string) component_2[PropertyNames.ParameterType]);
            Assert.IsNull((string) component_2[PropertyNames.Scale]);
            Assert.AreEqual("{2;1}", (string) component_2[PropertyNames.ShortName]);

            //Reorder Dimension
            postBodyPath = this.GetPath("Tests/SiteDirectory/ArrayParameterType/PostUpdateArrayParameterType.json");
            postBody = this.GetJsonFromFile(postBodyPath);

            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            arrayParameterType = jArray.Single(x => (string)x[PropertyNames.Iid] == "267b0d27-0421-453d-951a-4fcacc309a27");
            Assert.AreEqual(3, (int)arrayParameterType[PropertyNames.RevisionNumber]);

            var expectedReorderedDimensions =
                new List<OrderedItem>
                {
                    new OrderedItem(87757889, (long)1),
                    new OrderedItem(87757890, (long)2)
                };

            var reorderedDimensions = JsonConvert.DeserializeObject<List<OrderedItem>>(
                arrayParameterType[PropertyNames.Dimension].ToString());

            CollectionAssert.AreEquivalent(
                expectedReorderedDimensions,
                reorderedDimensions);

            //Remove Dimension
            postBodyPath = this.GetPath("Tests/SiteDirectory/ArrayParameterType/PostDeleteDimension.json");
            postBody = this.GetJsonFromFile(postBodyPath);

            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            arrayParameterType = jArray.Single(x => (string)x[PropertyNames.Iid] == "267b0d27-0421-453d-951a-4fcacc309a27");
            Assert.AreEqual(4, (int)arrayParameterType[PropertyNames.RevisionNumber]);

            var expectedDimensions =
                new List<OrderedItem>
                {
                    new OrderedItem(87757889, (long)1)
                };

            var dimensions = JsonConvert.DeserializeObject<List<OrderedItem>>(
                arrayParameterType[PropertyNames.Dimension].ToString());

            CollectionAssert.AreEquivalent(
                expectedDimensions,
                dimensions);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterTypeWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var arrayParameterTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/4a783624-b2bc-4e6d-95b3-11d036f6e917?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(arrayParameterTypeUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");

            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific ArrayParameterType from the result by it's unique id
            var arrayParameterType = jArray.Single(x => (string) x[PropertyNames.Iid] == "4a783624-b2bc-4e6d-95b3-11d036f6e917");

            VerifyProperties(arrayParameterType);
        }

        /// <summary>
        /// Verifies all properties of the ArrayParameterType <see cref="JToken"/>
        /// </summary>
        /// <param name="arrayParameterType">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ArrayParameterType object
        /// </param>
        public static void VerifyProperties(JToken arrayParameterType)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(16, arrayParameterType.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("4a783624-b2bc-4e6d-95b3-11d036f6e917", (string) arrayParameterType[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) arrayParameterType[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ArrayParameterType", (string) arrayParameterType[PropertyNames.ClassKind]);

            Assert.IsFalse((bool) arrayParameterType[PropertyNames.IsDeprecated]);
            Assert.AreEqual("Test Array ParameterType", (string) arrayParameterType[PropertyNames.Name]);
            Assert.AreEqual("TestArrayParameterType", (string) arrayParameterType[PropertyNames.ShortName]);

            Assert.AreEqual("testarray", (string) arrayParameterType[PropertyNames.Symbol]);
            Assert.IsFalse((bool) arrayParameterType[PropertyNames.IsFinalized]);

            var expectedComponents = new List<OrderedItem>
            {
                new OrderedItem(13087068, "9f17b223-446e-4a0c-afdb-60222b8e459e"),
                new OrderedItem(517448615, "f3ddc526-1ce8-4298-bd95-13e95d6f4cdd")
            };

            var componentsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                arrayParameterType[PropertyNames.Component].ToString());

            CollectionAssert.AreEquivalent(expectedComponents, componentsArray);

            Assert.IsFalse((bool) arrayParameterType[PropertyNames.IsTensor]);

            var expectedDimensions = new List<OrderedItem>
            {
                new OrderedItem(110835215, "2")
            };

            var dimensionsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                arrayParameterType[PropertyNames.Dimension].ToString());

            CollectionAssert.AreEquivalent(expectedDimensions, dimensionsArray);

            var expectedCategories = new string[] { };
            var categoriesArray = (JArray) arrayParameterType[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] { };
            var aliasesArray = (JArray) arrayParameterType[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] { };
            var definitionsArray = (JArray) arrayParameterType[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] { };
            var hyperlinksArray = (JArray) arrayParameterType[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
