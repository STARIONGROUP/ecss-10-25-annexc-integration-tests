// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpecializedQuantityKindTestFixture.cs" company="RHEA System S.A.">
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
    public class SpecializedQuantityKindTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedQuantityKindIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var specializedQuantityKindUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/0a6dc59d-4292-43be-a247-b8d7074d5d52");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(specializedQuantityKindUri);

            //check if there is the only one SpecializedQuantityKind object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific SpecializedQuantityKind from the result by it's unique id
            var specializedQuantityKind = jArray.Single(x => (string) x[PropertyNames.Iid] == "0a6dc59d-4292-43be-a247-b8d7074d5d52");

            SpecializedQuantityKindTestFixture.VerifyProperties(specializedQuantityKind);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedQuantityKindWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var specializedQuantityKindUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/0a6dc59d-4292-43be-a247-b8d7074d5d52?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(specializedQuantityKindUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific SpecializedQuantityKind from the result by it's unique id
            var specializedQuantityKind = jArray.Single(x => (string) x[PropertyNames.Iid] == "0a6dc59d-4292-43be-a247-b8d7074d5d52");
            SpecializedQuantityKindTestFixture.VerifyProperties(specializedQuantityKind);
        }

        /// <summary>
        /// Verifies all properties of the SpecializedQuantityKind <see cref="JToken"/>
        /// </summary>
        /// <param name="specializedQuantityKind">
        /// The <see cref="JToken"/> that contains the properties of
        /// the SpecializedQuantityKind object
        /// </param>
        public static void VerifyProperties(JToken specializedQuantityKind)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(15, specializedQuantityKind.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("0a6dc59d-4292-43be-a247-b8d7074d5d52", (string) specializedQuantityKind[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) specializedQuantityKind[PropertyNames.RevisionNumber]);
            Assert.AreEqual("SpecializedQuantityKind", (string) specializedQuantityKind[PropertyNames.ClassKind]);

            Assert.IsFalse((bool) specializedQuantityKind[PropertyNames.IsDeprecated]);
            Assert.AreEqual("Test Specialized QuantityKind", (string) specializedQuantityKind[PropertyNames.Name]);
            Assert.AreEqual("TestSpecializedQuantityKind", (string) specializedQuantityKind[PropertyNames.ShortName]);

            Assert.AreEqual("testsymbol", (string) specializedQuantityKind[PropertyNames.Symbol]);
            Assert.AreEqual("53e82aeb-c42c-475c-b6bf-a102af883471",
                (string) specializedQuantityKind[PropertyNames.DefaultScale]);
            Assert.AreEqual("", (string)specializedQuantityKind[PropertyNames.QuantityDimensionSymbol]);
            Assert.AreEqual("4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d", (string) specializedQuantityKind[PropertyNames.General]);

            var expectedPossibleScales = new string[]
            {
                "53e82aeb-c42c-475c-b6bf-a102af883471"
            };
            var possibleScalesArray = (JArray) specializedQuantityKind[PropertyNames.PossibleScale];
            IList<string> possibleScales = possibleScalesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedPossibleScales, possibleScales);

            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) specializedQuantityKind[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) specializedQuantityKind[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) specializedQuantityKind[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) specializedQuantityKind[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
