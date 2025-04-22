// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateParameterTypeTestFixture.cs" company="Starion Group S.A.">
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
    public class DateParameterTypeTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterTypeIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var dateParameterTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/33cf1171-3cd2-4494-8d54-639bfc583155");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(dateParameterTypeUri);

            //check if there is the only one DateParameterType object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific DateParameterType from the result by it's unique id
            var dateParameterType = jArray.Single(x => (string) x[PropertyNames.Iid] == "33cf1171-3cd2-4494-8d54-639bfc583155");

            DateParameterTypeTestFixture.VerifyProperties(dateParameterType);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterTypeWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var dateParameterTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/33cf1171-3cd2-4494-8d54-639bfc583155?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(dateParameterTypeUri);

            //check if there are 3 objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific DateParameterType from the result by it's unique id
            var dateParameterType = jArray.Single(x => (string) x[PropertyNames.Iid] == "33cf1171-3cd2-4494-8d54-639bfc583155");
            DateParameterTypeTestFixture.VerifyProperties(dateParameterType);
        }

        /// <summary>
        /// Verifies all properties of the DateParameterType <see cref="JToken"/>
        /// </summary>
        /// <param name="dateParameterType">
        /// The <see cref="JToken"/> that contains the properties of
        /// the DateParameterType object
        /// </param>
        public static void VerifyProperties(JToken dateParameterType)
        {
            // verify the amount of returned properties 
            Assert.That(dateParameterType.Children().Count(), Is.EqualTo(11));

            // assert that the properties are what is expected
            Assert.That((string) dateParameterType[PropertyNames.Iid], Is.EqualTo("33cf1171-3cd2-4494-8d54-639bfc583155"));
            Assert.That((int)dateParameterType[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)dateParameterType[PropertyNames.ClassKind], Is.EqualTo("DateParameterType"));

            Assert.That((bool) dateParameterType[PropertyNames.IsDeprecated], Is.False);
            Assert.That((string)dateParameterType[PropertyNames.Name], Is.EqualTo("Test Date ParameterType"));
            Assert.That((string)dateParameterType[PropertyNames.ShortName], Is.EqualTo("TestDateParameterType"));

            Assert.That((string)dateParameterType[PropertyNames.Symbol], Is.EqualTo("date"));

            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) dateParameterType[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            Assert.That(categories, Is.EquivalentTo(expectedCategories));

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) dateParameterType[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) dateParameterType[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) dateParameterType[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));
        }
    }
}
