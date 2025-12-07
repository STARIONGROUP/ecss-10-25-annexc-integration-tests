// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeParameterTypeTestFixture.cs" company="Starion Group S.A.">
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
    public class DateTimeParameterTypeTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterTypeIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var dateTimeParameterTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/9c1d3e39-0754-4388-8e1e-070ca9a0e7b6");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(dateTimeParameterTypeUri);

            //check if there is the only one DateTimeParameterType object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific DateTimeParameterType from the result by it's unique id
            var dateTimeParameterType = jArray.Single(x => (string) x[PropertyNames.Iid] == "9c1d3e39-0754-4388-8e1e-070ca9a0e7b6");

            DateTimeParameterTypeTestFixture.VerifyProperties(dateTimeParameterType);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterTypeWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var dateTimeParameterTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/9c1d3e39-0754-4388-8e1e-070ca9a0e7b6?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(dateTimeParameterTypeUri);

            //check if there are 3 objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific DateTimeParameterType from the result by it's unique id
            var dateTimeParameterType = jArray.Single(x => (string) x[PropertyNames.Iid] == "9c1d3e39-0754-4388-8e1e-070ca9a0e7b6");
            DateTimeParameterTypeTestFixture.VerifyProperties(dateTimeParameterType);
        }

        /// <summary>
        /// Verifies all properties of the DateTimeParameterType <see cref="JToken"/>
        /// </summary>
        /// <param name="dateTimeParameterType">
        /// The <see cref="JToken"/> that contains the properties of
        /// the DateTimeParameterType object
        /// </param>
        public static void VerifyProperties(JToken dateTimeParameterType)
        {
            // verify the amount of returned properties 
            Assert.That(dateTimeParameterType.Children().Count(), Is.EqualTo(11));

            // assert that the properties are what is expected
            Assert.That((string)dateTimeParameterType[PropertyNames.Iid], Is.EqualTo("9c1d3e39-0754-4388-8e1e-070ca9a0e7b6"));
            Assert.That((int)dateTimeParameterType[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)dateTimeParameterType[PropertyNames.ClassKind], Is.EqualTo("DateTimeParameterType"));

            Assert.That((bool) dateTimeParameterType[PropertyNames.IsDeprecated], Is.False);
            Assert.That((string)dateTimeParameterType[PropertyNames.Name], Is.EqualTo("Test DateTime ParameterType"));
            Assert.That((string)dateTimeParameterType[PropertyNames.ShortName], Is.EqualTo("TestDateTimeParameterType"));

            Assert.That((string)dateTimeParameterType[PropertyNames.Symbol], Is.EqualTo("datetime"));

            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) dateTimeParameterType[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            Assert.That(categories, Is.EquivalentTo(expectedCategories));

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) dateTimeParameterType[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) dateTimeParameterType[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) dateTimeParameterType[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));
        }
    }
}
