// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerationParameterTypeTestFixture.cs" company="RHEA System">
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
    public class EnumerationParameterTypeTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the EnumerationParameterType objects are returned from the data-source and that the 
        /// values of the EnumerationParameterType properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedParameterTypeIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var enumerationParameterTypeUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/664d5611-c564-4eba-8f2e-e23b99385daf"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(enumerationParameterTypeUri);

            //check if there is the only one EnumerationParameterType object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific EnumerationParameterType from the result by it's unique id
            var enumerationParameterType =
                jArray.Single(x => (string) x["iid"] == "664d5611-c564-4eba-8f2e-e23b99385daf");

            EnumerationParameterTypeTestFixture.VerifyProperties(enumerationParameterType);
        }

        [Test]
        public void VerifyThatExpectedParameterTypeWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var enumerationParameterTypeUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/664d5611-c564-4eba-8f2e-e23b99385daf?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(enumerationParameterTypeUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary =
                jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific EnumerationParameterType from the result by it's unique id
            var enumerationParameterType =
                jArray.Single(x => (string) x["iid"] == "664d5611-c564-4eba-8f2e-e23b99385daf");
            EnumerationParameterTypeTestFixture.VerifyProperties(enumerationParameterType);
        }

        /// <summary>
        /// Verifies all properties of the EnumerationParameterType <see cref="JToken"/>
        /// </summary>
        /// <param name="enumerationParameterType">
        /// The <see cref="JToken"/> that contains the properties of
        /// the EnumerationParameterType object
        /// </param>
        public static void VerifyProperties(JToken enumerationParameterType)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(13, enumerationParameterType.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("664d5611-c564-4eba-8f2e-e23b99385daf", (string) enumerationParameterType["iid"]);
            Assert.AreEqual(1, (int) enumerationParameterType["revisionNumber"]);
            Assert.AreEqual("EnumerationParameterType", (string) enumerationParameterType["classKind"]);

            Assert.IsFalse((bool) enumerationParameterType["isDeprecated"]);
            Assert.AreEqual("Test Enumeration ParameterType", (string) enumerationParameterType["name"]);
            Assert.AreEqual("TestEnumerationParameterType", (string) enumerationParameterType["shortName"]);

            Assert.IsFalse((bool) enumerationParameterType["allowMultiSelect"]);

            var expectedValueDefinitions = new List<OrderedItem>
            {
                new OrderedItem(211917624, "4a594906-0302-45c7-bc4b-dbe60e2658f8"),
                new OrderedItem(425701823, "6604dc56-158b-47f1-8cbb-425d2eafd377")
            };
            var valueDefinitionsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                enumerationParameterType["valueDefinition"].ToString());
            CollectionAssert.AreEquivalent(expectedValueDefinitions, valueDefinitionsArray);

            Assert.AreEqual("enumerationparametertype", (string) enumerationParameterType["symbol"]);

            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) enumerationParameterType["category"];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) enumerationParameterType["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) enumerationParameterType["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) enumerationParameterType["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}