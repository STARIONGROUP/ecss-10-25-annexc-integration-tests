// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivedQuantityKindTestFixture.cs" company="RHEA System S.A.">
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
    using System.Net;
    
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class DerivedQuantityKindTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedQuantityKindIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var derivedQuantityKindUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/74d9c38f-5ace-4f90-8841-d0f9942e9d09"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(derivedQuantityKindUri);

            //check if there is the only one DerivedQuantityKind object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific DerivedQuantityKind from the result by it's unique id
            var derivedQuantityKind =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "74d9c38f-5ace-4f90-8841-d0f9942e9d09");

            DerivedQuantityKindTestFixture.VerifyProperties(derivedQuantityKind);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedQuantityKindWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var derivedQuantityKindUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/74d9c38f-5ace-4f90-8841-d0f9942e9d09?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(derivedQuantityKindUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific DerivedQuantityKind from the result by it's unique id
            var derivedQuantityKind =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "74d9c38f-5ace-4f90-8841-d0f9942e9d09");
            DerivedQuantityKindTestFixture.VerifyProperties(derivedQuantityKind);
        }

        [Test]
        [Category("POST")]
        public void VerifyCyclicSelf()
        {
            var uri = new Uri(string.Format(UriFormat, this.Settings.Hostname,
                "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));
            var postBodyPath = this.GetPath("Tests/SiteDirectory/DerivedQuantityKind/PostNewCyclicFactor.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            
            // no additional error details are available here
            Assert.Throws<WebException>(() => this.WebClient.PostDto(uri, postBody));
        }

        /// <summary>
        /// Verifies all properties of the DerivedQuantityKind <see cref="JToken"/>
        /// </summary>
        /// <param name="derivedQuantityKind">
        /// The <see cref="JToken"/> that contains the properties of
        /// the DerivedQuantityKind object
        /// </param>
        public static void VerifyProperties(JToken derivedQuantityKind)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(15, derivedQuantityKind.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("74d9c38f-5ace-4f90-8841-d0f9942e9d09", (string) derivedQuantityKind[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) derivedQuantityKind[PropertyNames.RevisionNumber]);
            Assert.AreEqual("DerivedQuantityKind", (string) derivedQuantityKind[PropertyNames.ClassKind]);

            Assert.IsFalse((bool) derivedQuantityKind[PropertyNames.IsDeprecated]);
            Assert.AreEqual("Test Derived QuantityKind", (string) derivedQuantityKind[PropertyNames.Name]);
            Assert.AreEqual("TestDerivedQuantityKind", (string) derivedQuantityKind[PropertyNames.ShortName]);

            Assert.AreEqual("symbol", (string) derivedQuantityKind[PropertyNames.Symbol]);
            Assert.AreEqual("53e82aeb-c42c-475c-b6bf-a102af883471", (string) derivedQuantityKind[PropertyNames.DefaultScale]);
            Assert.IsNull((string) derivedQuantityKind[PropertyNames.QuantityDimensionSymbol]);

            var expectedPossibleScales = new string[]
            {
                "53e82aeb-c42c-475c-b6bf-a102af883471"
            };
            var possibleScalesArray = (JArray) derivedQuantityKind[PropertyNames.PossibleScale];
            IList<string> possibleScales = possibleScalesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedPossibleScales, possibleScales);

            var expectedQuantityKindFactors = new List<OrderedItem>
            {
                new OrderedItem(2948121, "ab7e80da-6bc9-427f-b1fb-b97faeeca4c6")
            };
            var quantityKindsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                derivedQuantityKind[PropertyNames.QuantityKindFactor].ToString());
            CollectionAssert.AreEquivalent(expectedQuantityKindFactors, quantityKindsArray);

            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) derivedQuantityKind[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) derivedQuantityKind[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) derivedQuantityKind[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) derivedQuantityKind[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
