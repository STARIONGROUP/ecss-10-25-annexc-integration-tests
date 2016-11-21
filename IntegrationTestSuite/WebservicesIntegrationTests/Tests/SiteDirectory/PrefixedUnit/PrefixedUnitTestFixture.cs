// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrefixedUnitTestFixture.cs" company="RHEA System">
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
    public class PrefixedUnitTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the PrefixedUnit objects are returned from the data-source and that the 
        /// values of the PrefixedUnit properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedUnitIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var prefixedUnitUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/unit/0f69c1f9-7896-45fc-830c-1e336d22a64a"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(prefixedUnitUri);

            //check if there is the only one PrefixedUnit object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific PrefixedUnit from the result by it's unique id
            var prefixedUnit =
                jArray.Single(x => (string) x["iid"] == "0f69c1f9-7896-45fc-830c-1e336d22a64a");

            PrefixedUnitTestFixture.VerifyProperties(prefixedUnit);
        }

        [Test]
        public void VerifyThatExpectedUnitWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var prefixedUnitUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/unit/0f69c1f9-7896-45fc-830c-1e336d22a64a?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(prefixedUnitUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary =
                jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific PrefixedUnit from the result by it's unique id
            var prefixedUnit =
                jArray.Single(x => (string) x["iid"] == "0f69c1f9-7896-45fc-830c-1e336d22a64a");
            PrefixedUnitTestFixture.VerifyProperties(prefixedUnit);
        }

        /// <summary>
        /// Verifies all properties of the PrefixedUnit <see cref="JToken"/>
        /// </summary>
        /// <param name="prefixedUnit">
        /// The <see cref="JToken"/> that contains the properties of
        /// the PrefixedUnit object
        /// </param>
        public static void VerifyProperties(JToken prefixedUnit)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(9, prefixedUnit.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("0f69c1f9-7896-45fc-830c-1e336d22a64a", (string) prefixedUnit["iid"]);
            Assert.AreEqual(1, (int) prefixedUnit["revisionNumber"]);
            Assert.AreEqual("PrefixedUnit", (string) prefixedUnit["classKind"]);

            Assert.IsFalse((bool) prefixedUnit["isDeprecated"]);
            Assert.AreEqual("efa6380d-9508-4f3d-9b43-6ed33125b780", (string) prefixedUnit["prefix"]);
            Assert.AreEqual("56842970-3915-4369-8712-61cfd8273ef9", (string) prefixedUnit["referenceUnit"]);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) prefixedUnit["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) prefixedUnit["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) prefixedUnit["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}