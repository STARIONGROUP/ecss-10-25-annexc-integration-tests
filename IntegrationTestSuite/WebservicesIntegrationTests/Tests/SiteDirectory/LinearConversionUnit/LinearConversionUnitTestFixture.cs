// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearConversionUnitTestFixture.cs" company="RHEA System S.A.">
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

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class LinearConversionUnitTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedUnitIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var linearConversionUnitUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/unit/12f48e1a-2996-46cc-8dc1-faf4e69ae115");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(linearConversionUnitUri);

            //check if there is the only one LinearConversionUnit object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific LinearConversionUnit from the result by it's unique id
            var linearConversionUnit = jArray.Single(x => (string) x["iid"] == "12f48e1a-2996-46cc-8dc1-faf4e69ae115");

            LinearConversionUnitTestFixture.VerifyProperties(linearConversionUnit);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedUnitWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var linearConversionUnitUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/unit/12f48e1a-2996-46cc-8dc1-faf4e69ae115?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(linearConversionUnitUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific LinearConversionUnit from the result by it's unique id
            var linearConversionUnit = jArray.Single(x => (string) x["iid"] == "12f48e1a-2996-46cc-8dc1-faf4e69ae115");
            LinearConversionUnitTestFixture.VerifyProperties(linearConversionUnit);
        }

        /// <summary>
        /// Verifies all properties of the LinearConversionUnit <see cref="JToken"/>
        /// </summary>
        /// <param name="linearConversionUnit">
        /// The <see cref="JToken"/> that contains the properties of
        /// the LinearConversionUnit object
        /// </param>
        public static void VerifyProperties(JToken linearConversionUnit)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(11, linearConversionUnit.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("12f48e1a-2996-46cc-8dc1-faf4e69ae115", (string) linearConversionUnit["iid"]);
            Assert.AreEqual(1, (int) linearConversionUnit["revisionNumber"]);
            Assert.AreEqual("LinearConversionUnit", (string) linearConversionUnit["classKind"]);

            Assert.IsFalse((bool) linearConversionUnit["isDeprecated"]);
            Assert.AreEqual("Test Linear Conversion Unit", (string) linearConversionUnit["name"]);
            Assert.AreEqual("testLinearConversionUnit", (string) linearConversionUnit["shortName"]);

            Assert.AreEqual("56842970-3915-4369-8712-61cfd8273ef9", (string) linearConversionUnit["referenceUnit"]);
            Assert.AreEqual("1000", (string) linearConversionUnit["conversionFactor"]);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) linearConversionUnit["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) linearConversionUnit["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) linearConversionUnit["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
