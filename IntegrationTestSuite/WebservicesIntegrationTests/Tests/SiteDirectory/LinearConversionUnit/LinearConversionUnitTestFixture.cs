// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearConversionUnitTestFixture.cs" company="Starion Group S.A.">
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
            Assert.That(jArray.Count, Is.EqualTo(1));

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
            Assert.That(jArray.Count, Is.EqualTo(3));

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
            Assert.That(linearConversionUnit.Children().Count(), Is.EqualTo(11));

            // assert that the properties are what is expected
            Assert.That((string) linearConversionUnit["iid"], Is.EqualTo("12f48e1a-2996-46cc-8dc1-faf4e69ae115"));
            Assert.That((int) linearConversionUnit["revisionNumber"], Is.EqualTo(1));
            Assert.That((string) linearConversionUnit["classKind"], Is.EqualTo("LinearConversionUnit"));

            Assert.That((bool) linearConversionUnit["isDeprecated"], Is.False);
            Assert.That((string) linearConversionUnit["name"], Is.EqualTo("Test Linear Conversion Unit"));
            Assert.That((string) linearConversionUnit["shortName"], Is.EqualTo("testLinearConversionUnit"));

            Assert.That((string) linearConversionUnit["referenceUnit"], Is.EqualTo("56842970-3915-4369-8712-61cfd8273ef9"));
            Assert.That((string) linearConversionUnit["conversionFactor"], Is.EqualTo("1000"));

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) linearConversionUnit["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) linearConversionUnit["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) linearConversionUnit["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));
        }
    }
}
