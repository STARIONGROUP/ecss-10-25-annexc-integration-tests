// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivedQuantityKindTestFixture.cs" company="Starion Group S.A.">
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
            var derivedQuantityKindUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/74d9c38f-5ace-4f90-8841-d0f9942e9d09");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(derivedQuantityKindUri);

            //check if there is the only one DerivedQuantityKind object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific DerivedQuantityKind from the result by it's unique id
            var derivedQuantityKind = jArray.Single(x => (string) x[PropertyNames.Iid] == "74d9c38f-5ace-4f90-8841-d0f9942e9d09");

            DerivedQuantityKindTestFixture.VerifyProperties(derivedQuantityKind);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedQuantityKindWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var derivedQuantityKindUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/74d9c38f-5ace-4f90-8841-d0f9942e9d09?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(derivedQuantityKindUri);

            //check if there are 3 objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific DerivedQuantityKind from the result by it's unique id
            var derivedQuantityKind = jArray.Single(x => (string) x[PropertyNames.Iid] == "74d9c38f-5ace-4f90-8841-d0f9942e9d09");
            DerivedQuantityKindTestFixture.VerifyProperties(derivedQuantityKind);
        }

        [Test]
        [Category("POST")]
        public void VerifyCyclicSelf()
        {
            var uri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
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
            Assert.That(derivedQuantityKind.Children().Count(), Is.EqualTo(15));

            // assert that the properties are what is expected
            Assert.That((string)derivedQuantityKind[PropertyNames.Iid], Is.EqualTo("74d9c38f-5ace-4f90-8841-d0f9942e9d09"));
            Assert.That((int)derivedQuantityKind[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)derivedQuantityKind[PropertyNames.ClassKind], Is.EqualTo("DerivedQuantityKind"));

            Assert.That((bool) derivedQuantityKind[PropertyNames.IsDeprecated], Is.False);
            Assert.That((string)derivedQuantityKind[PropertyNames.Name], Is.EqualTo("Test Derived QuantityKind"));
            Assert.That((string)derivedQuantityKind[PropertyNames.ShortName], Is.EqualTo("TestDerivedQuantityKind"));

            Assert.That((string)derivedQuantityKind[PropertyNames.Symbol], Is.EqualTo("symbol"));
            Assert.That((string)derivedQuantityKind[PropertyNames.DefaultScale], Is.EqualTo("53e82aeb-c42c-475c-b6bf-a102af883471"));
            Assert.That((string) derivedQuantityKind[PropertyNames.QuantityDimensionSymbol], Is.Null);

            var expectedPossibleScales = new string[]
            {
                "53e82aeb-c42c-475c-b6bf-a102af883471"
            };
            var possibleScalesArray = (JArray) derivedQuantityKind[PropertyNames.PossibleScale];
            IList<string> possibleScales = possibleScalesArray.Select(x => (string) x).ToList();
            Assert.That(possibleScales, Is.EquivalentTo(expectedPossibleScales));

            var expectedQuantityKindFactors = new List<OrderedItem>
            {
                new OrderedItem(2948121, "ab7e80da-6bc9-427f-b1fb-b97faeeca4c6")
            };
            var quantityKindsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                derivedQuantityKind[PropertyNames.QuantityKindFactor].ToString());
            Assert.That(quantityKindsArray, Is.EquivalentTo(expectedQuantityKindFactors));

            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) derivedQuantityKind[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            Assert.That(categories, Is.EquivalentTo(expectedCategories));

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) derivedQuantityKind[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) derivedQuantityKind[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) derivedQuantityKind[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));
        }
    }
}
