// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantTestFixture.cs" company="RHEA System">
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

    [TestFixture]
    public class ConstantTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the Constant objects are returned from the data-source and that the 
        /// values of the Constant properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedConstantIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var constantUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/constant"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(constantUri);

            //check if there is the only one Constant object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific Constant from the result by it's unique id
            var constant =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "239754fe-834f-4394-9c3a-26cac7f866d3");

            ConstantTestFixture.VerifyProperties(constant);
        }

        [Test]
        public void VerifyThatExpectedConstantWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var constantUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/constant?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(constantUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific Constant from the result by it's unique id
            var constant =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "239754fe-834f-4394-9c3a-26cac7f866d3");
            ConstantTestFixture.VerifyProperties(constant);
        }

        /// <summary>
        /// Verifies all properties of the Constant <see cref="JToken"/>
        /// </summary>
        /// <param name="constant">
        /// The <see cref="JToken"/> that contains the properties of
        /// the Constant object
        /// </param>
        public static void VerifyProperties(JToken constant)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(13, constant.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("239754fe-834f-4394-9c3a-26cac7f866d3", (string) constant[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) constant[PropertyNames.RevisionNumber]);
            Assert.AreEqual("Constant", (string) constant[PropertyNames.ClassKind]);

            Assert.IsFalse((bool) constant[PropertyNames.IsDeprecated]);
            Assert.AreEqual("Test Constant", (string) constant[PropertyNames.Name]);
            Assert.AreEqual("TestConstant", (string) constant[PropertyNames.ShortName]);

            Assert.AreEqual("35a9cf05-4eba-4cda-b60c-7cfeaac8f892", (string) constant[PropertyNames.ParameterType]);

            Assert.AreEqual("[\"true\"]", (string) constant[PropertyNames.Value]);

            Assert.IsNull((string) constant[PropertyNames.Scale]);

            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) constant[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) constant["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) constant["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) constant["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}