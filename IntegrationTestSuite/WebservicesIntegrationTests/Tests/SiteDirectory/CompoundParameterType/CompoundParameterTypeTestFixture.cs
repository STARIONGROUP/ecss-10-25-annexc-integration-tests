// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompoundParameterTypeTestFixture.cs" company="RHEA System S.A.">
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
    public class CompoundParameterTypeTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterTypeIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var compoundParameterTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/0d3178f9-68d0-4b1a-afe8-d5df0b66f1d4");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(compoundParameterTypeUri);

            //check if there is the only one CompoundParameterType object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific CompoundParameterType from the result by it's unique id
            var compoundParameterType = jArray.Single(x => (string) x[PropertyNames.Iid] == "0d3178f9-68d0-4b1a-afe8-d5df0b66f1d4");

            CompoundParameterTypeTestFixture.VerifyProperties(compoundParameterType);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterTypeWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var compoundParameterTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/0d3178f9-68d0-4b1a-afe8-d5df0b66f1d4?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(compoundParameterTypeUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific CompoundParameter from the result by it's unique id
            var compoundParameterType = jArray.Single(x => (string) x[PropertyNames.Iid] == "0d3178f9-68d0-4b1a-afe8-d5df0b66f1d4");
            CompoundParameterTypeTestFixture.VerifyProperties(compoundParameterType);
        }

        /// <summary>
        /// Verifies all properties of the CompoundParameterType <see cref="JToken"/>
        /// </summary>
        /// <param name="compoundParameterType">
        /// The <see cref="JToken"/> that contains the properties of
        /// the CompoundParameterType object
        /// </param>
        public static void VerifyProperties(JToken compoundParameterType)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(14, compoundParameterType.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("0d3178f9-68d0-4b1a-afe8-d5df0b66f1d4", (string) compoundParameterType[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) compoundParameterType[PropertyNames.RevisionNumber]);
            Assert.AreEqual("CompoundParameterType", (string) compoundParameterType[PropertyNames.ClassKind]);

            Assert.IsFalse((bool) compoundParameterType[PropertyNames.IsDeprecated]);
            Assert.AreEqual("TestCompoundParameterType", (string) compoundParameterType[PropertyNames.Name]);
            Assert.AreEqual("TestCompoundParameterType", (string) compoundParameterType[PropertyNames.ShortName]);

            Assert.AreEqual("testsymbol", (string) compoundParameterType[PropertyNames.Symbol]);
            Assert.IsFalse((bool) compoundParameterType[PropertyNames.IsFinalized]);

            var expectedComponents = new List<OrderedItem>
            {
                new OrderedItem(17256407, "b607fdc1-7578-48f9-8597-caba56df3177"),
                new OrderedItem(212499462, "8019277f-8bc7-463b-b3bb-46a404493e31")
            };
            var componentsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                compoundParameterType[PropertyNames.Component].ToString());
            CollectionAssert.AreEquivalent(expectedComponents, componentsArray);

            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) compoundParameterType[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) compoundParameterType[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) compoundParameterType[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) compoundParameterType[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }

        [Test]
        [Category("POST")]
        public void Verify_that_CompoundParameterType_can_be_posted()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");

            var postBodyPath = this.GetPath("Tests/SiteDirectory/CompoundParameterType/PostCompoundParameterType.json");
            var postBody = base.GetJsonFromFile(postBodyPath);

            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            //Check the amount of objects 
            Assert.AreEqual(5, jArray.Count);

            var siteDirectory = jArray.Single(x => (string)x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.AreEqual(2, (int)siteDirectory[PropertyNames.RevisionNumber]);

            var compoundParameterType = jArray.Single(x => (string)x[PropertyNames.Iid] == "7a2b0596-bb8f-4692-adc5-04ae813fd9bd");
            Assert.AreEqual(2, (int)compoundParameterType[PropertyNames.RevisionNumber]);
            Assert.AreEqual("test_CPT", (string)compoundParameterType[PropertyNames.ShortName]);
            Assert.AreEqual("testCreateCompoundParameterType", (string)compoundParameterType[PropertyNames.Name]);
            Assert.AreEqual("test_cpt", (string)compoundParameterType[PropertyNames.Symbol]);

            var component_1 = jArray.Single(x => (string)x[PropertyNames.Iid] == "ff206aaf-ca83-412a-a045-cc73a15259f2");
            Assert.AreEqual(2, (int)component_1[PropertyNames.RevisionNumber]);
            Assert.AreEqual("a21c15c4-3e1e-46b5-b109-5063dec1e254", (string)component_1[PropertyNames.ParameterType]);
            Assert.IsNull((string)component_1[PropertyNames.Scale]);
            Assert.AreEqual("a", (string)component_1[PropertyNames.ShortName]);

            var component_2 = jArray.Single(x => (string)x[PropertyNames.Iid] == "9cec7d00-fc6b-43d0-8a0d-13d3d73e5478");
            Assert.AreEqual(2, (int)component_2[PropertyNames.RevisionNumber]);
            Assert.AreEqual("4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d", (string)component_2[PropertyNames.ParameterType]);
            Assert.AreEqual("53e82aeb-c42c-475c-b6bf-a102af883471", (string)component_2[PropertyNames.Scale]);            
            Assert.AreEqual("b", (string)component_2[PropertyNames.ShortName]);
        }
    }
}
