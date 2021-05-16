// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivedUnitTestFixture.cs" company="RHEA System S.A.">
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

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class DerivedUnitTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedUnitIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var derivedUnitUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/unit/c394eaa9-4832-4b2d-8d88-5e1b2c43732c"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(derivedUnitUri);

            //check if there is the only one DerivedUnit object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific DerivedUnit from the result by it's unique id
            var derivedUnit =
                jArray.Single(x => (string) x["iid"] == "c394eaa9-4832-4b2d-8d88-5e1b2c43732c");
            DerivedUnitTestFixture.VerifyProperties(derivedUnit);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedUnitWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var derivedUnitUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/unit/c394eaa9-4832-4b2d-8d88-5e1b2c43732c?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(derivedUnitUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary =
                jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific DerivedUnit from the result by it's unique id
            var derivedUnit =
                jArray.Single(x => (string) x["iid"] == "c394eaa9-4832-4b2d-8d88-5e1b2c43732c");
            DerivedUnitTestFixture.VerifyProperties(derivedUnit);
        }

        /// <summary>
        /// Verifies all properties of the DerivedUnit <see cref="JToken"/>
        /// </summary>
        /// <param name="derivedUnit">
        /// The <see cref="JToken"/> that contains the properties of
        /// the DerivedUnit object
        /// </param>
        public static void VerifyProperties(JToken derivedUnit)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(10, derivedUnit.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("c394eaa9-4832-4b2d-8d88-5e1b2c43732c", (string) derivedUnit["iid"]);
            Assert.AreEqual(1, (int) derivedUnit["revisionNumber"]);
            Assert.AreEqual("DerivedUnit", (string) derivedUnit["classKind"]);

            Assert.IsFalse((bool) derivedUnit["isDeprecated"]);
            Assert.AreEqual("Test Derived Unit", (string) derivedUnit["name"]);
            Assert.AreEqual("TestDerivedUnit", (string) derivedUnit["shortName"]);

            var expectedUnitFactors = new List<OrderedItem>
            {
                new OrderedItem(23307173, "56c30a85-f648-4b31-87d2-153e8a74048b")
            };
            var unitFactorsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                derivedUnit["unitFactor"].ToString());
            CollectionAssert.AreEquivalent(expectedUnitFactors, unitFactorsArray);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) derivedUnit["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) derivedUnit["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) derivedUnit["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
