// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextParameterTypeTestFixture.cs" company="RHEA System S.A.">
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

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class TextParameterTypeTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterTypeIsReturnedFromWebApi()
        {
            var textParameterTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/a21c15c4-3e1e-46b5-b109-5063dec1e254");

            var jArray = this.WebClient.GetDto(textParameterTypeUri);

            Assert.AreEqual(1, jArray.Count);

            var textParameterType = jArray.Single(x => (string) x[PropertyNames.Iid] == "a21c15c4-3e1e-46b5-b109-5063dec1e254");

            TextParameterTypeTestFixture.VerifyProperties(textParameterType);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterTypeWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var textParameterTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/a21c15c4-3e1e-46b5-b109-5063dec1e254?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(textParameterTypeUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific TextParameterType from the result by it's unique id
            var textParameterType = jArray.Single(x => (string) x[PropertyNames.Iid] == "a21c15c4-3e1e-46b5-b109-5063dec1e254");
            TextParameterTypeTestFixture.VerifyProperties(textParameterType);
        }

        /// <summary>
        /// Verifies all properties of the TextParameterType <see cref="JToken"/>
        /// </summary>
        /// <param name="textParameterType">
        /// The <see cref="JToken"/> that contains the properties of
        /// the TextParameterType object
        /// </param>
        public static void VerifyProperties(JToken textParameterType)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(12, textParameterType.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("a21c15c4-3e1e-46b5-b109-5063dec1e254", (string) textParameterType[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) textParameterType[PropertyNames.RevisionNumber]);
            Assert.AreEqual("TextParameterType", (string) textParameterType[PropertyNames.ClassKind]);

            Assert.IsFalse((bool) textParameterType[PropertyNames.IsDeprecated]);
            Assert.AreEqual("Test Text ParameterType", (string) textParameterType[PropertyNames.Name]);
            Assert.AreEqual("TestTextParameterType", (string) textParameterType[PropertyNames.ShortName]);

            Assert.AreEqual("text", (string) textParameterType[PropertyNames.Symbol]);

            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) textParameterType[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) textParameterType[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) textParameterType[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) textParameterType[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
