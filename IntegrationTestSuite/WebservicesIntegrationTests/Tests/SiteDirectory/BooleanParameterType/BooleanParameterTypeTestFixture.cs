﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanParameterTypeTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2021 Starion Group S.A.
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
    public class BooleanParameterTypeTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterTypeIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var booleanParameterTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/35a9cf05-4eba-4cda-b60c-7cfeaac8f892");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(booleanParameterTypeUri);

            //check if there is the only one BooleanParameterType object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific BooleanParameterType from the result by it's unique id
            var booleanParameterType = jArray.Single(x => (string) x[PropertyNames.Iid] == "35a9cf05-4eba-4cda-b60c-7cfeaac8f892");

            BooleanParameterTypeTestFixture.VerifyProperties(booleanParameterType);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterTypeWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var booleanParameterTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/35a9cf05-4eba-4cda-b60c-7cfeaac8f892?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(booleanParameterTypeUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific BooleanParameterType from the result by it's unique id
            var booleanParameterType = jArray.Single(x => (string) x[PropertyNames.Iid] == "35a9cf05-4eba-4cda-b60c-7cfeaac8f892");
            BooleanParameterTypeTestFixture.VerifyProperties(booleanParameterType);
        }

        /// <summary>
        /// Verifies all properties of the BooleanParameterType <see cref="JToken"/>
        /// </summary>
        /// <param name="booleanParameterType">
        /// The <see cref="JToken"/> that contains the properties of
        /// the BooleanParameterType object
        /// </param>
        public static void VerifyProperties(JToken booleanParameterType)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(11, booleanParameterType.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("35a9cf05-4eba-4cda-b60c-7cfeaac8f892", (string) booleanParameterType[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) booleanParameterType[PropertyNames.RevisionNumber]);
            Assert.AreEqual("BooleanParameterType", (string) booleanParameterType[PropertyNames.ClassKind]);

            Assert.IsFalse((bool) booleanParameterType[PropertyNames.IsDeprecated]);
            Assert.AreEqual("Test Boolean ParameterType", (string) booleanParameterType[PropertyNames.Name]);
            Assert.AreEqual("TestBooleanParameterType", (string) booleanParameterType[PropertyNames.ShortName]);

            Assert.AreEqual("bool", (string) booleanParameterType[PropertyNames.Symbol]);

            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) booleanParameterType[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) booleanParameterType[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) booleanParameterType[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) booleanParameterType[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
