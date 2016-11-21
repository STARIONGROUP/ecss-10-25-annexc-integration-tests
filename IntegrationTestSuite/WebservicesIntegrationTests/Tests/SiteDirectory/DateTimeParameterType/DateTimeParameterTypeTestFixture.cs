// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeParameterTypeTestFixture.cs" company="RHEA System">
//
//   Copyright 2016 RHEA System 
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
    using NUnit.Framework;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;

    [TestFixture]
    public class DateTimeParameterTypeTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the DateTimeParameterType objects are returned from the data-source and that the 
        /// values of the DateTimeParameterType properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedParameterTypeIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var dateTimeParameterTypeUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/9c1d3e39-0754-4388-8e1e-070ca9a0e7b6"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(dateTimeParameterTypeUri);

            //check if there is the only one DateTimeParameterType object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific DateTimeParameterType from the result by it's unique id
            var dateTimeParameterType =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "9c1d3e39-0754-4388-8e1e-070ca9a0e7b6");

            DateTimeParameterTypeTestFixture.VerifyProperties(dateTimeParameterType);
        }

        [Test]
        public void VerifyThatExpectedParameterTypeWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var dateTimeParameterTypeUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/9c1d3e39-0754-4388-8e1e-070ca9a0e7b6?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(dateTimeParameterTypeUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific DateTimeParameterType from the result by it's unique id
            var dateTimeParameterType =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "9c1d3e39-0754-4388-8e1e-070ca9a0e7b6");
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
            Assert.AreEqual(11, dateTimeParameterType.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("9c1d3e39-0754-4388-8e1e-070ca9a0e7b6", (string) dateTimeParameterType[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) dateTimeParameterType[PropertyNames.RevisionNumber]);
            Assert.AreEqual("DateTimeParameterType", (string) dateTimeParameterType[PropertyNames.ClassKind]);

            Assert.IsFalse((bool) dateTimeParameterType[PropertyNames.IsDeprecated]);
            Assert.AreEqual("Test DateTime ParameterType", (string) dateTimeParameterType[PropertyNames.Name]);
            Assert.AreEqual("TestDateTimeParameterType", (string) dateTimeParameterType[PropertyNames.ShortName]);

            Assert.AreEqual("datetime", (string) dateTimeParameterType[PropertyNames.Symbol]);

            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) dateTimeParameterType[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) dateTimeParameterType[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) dateTimeParameterType[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) dateTimeParameterType[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}