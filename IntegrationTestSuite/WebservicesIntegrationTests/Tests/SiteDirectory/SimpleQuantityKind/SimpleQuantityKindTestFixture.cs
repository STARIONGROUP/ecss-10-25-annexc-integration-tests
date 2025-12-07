// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleQuantityKindTestFixture.cs" company="Starion Group S.A.">
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

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class SimpleQuantityKindTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedQuantityKindIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var simpleQuantityKindUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(simpleQuantityKindUri);

            //check if there is the only one SimpleQuantityKind object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific SimpleQuantityKind from the result by it's unique id
            var simpleQuantityKind = jArray.Single(x => (string) x[PropertyNames.Iid] == "4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d");

            SimpleQuantityKindTestFixture.VerifyProperties(simpleQuantityKind);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedQuantityKindWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var simpleQuantityKindUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(simpleQuantityKindUri);

            //check if there are 3 objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific SimpleQuantityKind from the result by it's unique id
            var simpleQuantityKind = jArray.Single(x => (string) x[PropertyNames.Iid] == "4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d");
            SimpleQuantityKindTestFixture.VerifyProperties(simpleQuantityKind);
        }

        /// <summary>
        /// Verifies all properties of the SimpleQuantityKind <see cref="JToken"/>
        /// </summary>
        /// <param name="simpleQuantityKind">
        /// The <see cref="JToken"/> that contains the properties of
        /// the SimpleQuantityKind object
        /// </param>
        public static void VerifyProperties(JToken simpleQuantityKind)
        {
            // verify the amount of returned properties 
            Assert.That(simpleQuantityKind.Children().Count(), Is.EqualTo(14));

            // assert that the properties are what is expected
            Assert.That((string) simpleQuantityKind[PropertyNames.Iid], Is.EqualTo("4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d"));
            Assert.That((int) simpleQuantityKind[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string) simpleQuantityKind[PropertyNames.ClassKind], Is.EqualTo("SimpleQuantityKind"));

            Assert.That((bool) simpleQuantityKind[PropertyNames.IsDeprecated], Is.False);
            Assert.That((string) simpleQuantityKind[PropertyNames.Name], Is.EqualTo("Test Simple QuantityKind"));
            Assert.That((string) simpleQuantityKind[PropertyNames.ShortName], Is.EqualTo("TestSimpleQuantityKind"));

            Assert.That((string) simpleQuantityKind[PropertyNames.Symbol], Is.EqualTo("testsymbol"));
            Assert.That((string) simpleQuantityKind[PropertyNames.DefaultScale], Is.EqualTo("53e82aeb-c42c-475c-b6bf-a102af883471"));
            Assert.That((string) simpleQuantityKind[PropertyNames.QuantityDimensionSymbol], Is.EqualTo("testQuantityDimensionSymbol"));

            var expectedPossibleScales = new string[]
            {
                "53e82aeb-c42c-475c-b6bf-a102af883471"
            };
            var possibleScalesArray = (JArray) simpleQuantityKind[PropertyNames.PossibleScale];
            IList<string> possibleScales = possibleScalesArray.Select(x => (string) x).ToList();
            Assert.That(possibleScales, Is.EquivalentTo(expectedPossibleScales));

            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) simpleQuantityKind[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            Assert.That(categories, Is.EquivalentTo(expectedCategories));

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) simpleQuantityKind[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) simpleQuantityKind[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) simpleQuantityKind[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));
        }
    }
}
