// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleUnitTestFixture.cs" company="Starion Group S.A.">
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
    public class SimpleUnitTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedUnitIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var simpleUnitUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/unit/56842970-3915-4369-8712-61cfd8273ef9");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(simpleUnitUri);

            //check if there is the only one SimpleUnit object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific SimpleUnit from the result by it's unique id
            var simpleUnit = jArray.Single(x => (string) x["iid"] == "56842970-3915-4369-8712-61cfd8273ef9");

            SimpleUnitTestFixture.VerifyProperties(simpleUnit);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedUnitWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var simpleUnitUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/unit/56842970-3915-4369-8712-61cfd8273ef9?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(simpleUnitUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific SimpleUnit from the result by it's unique id
            var simpleUnit = jArray.Single(x => (string) x["iid"] == "56842970-3915-4369-8712-61cfd8273ef9");
            SimpleUnitTestFixture.VerifyProperties(simpleUnit);
        }

        /// <summary>
        /// Verifies all properties of the SimpleUnit <see cref="JToken"/>
        /// </summary>
        /// <param name="simpleUnit">
        /// The <see cref="JToken"/> that contains the properties of
        /// the SimpleUnit object
        /// </param>
        public static void VerifyProperties(JToken simpleUnit)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(9, simpleUnit.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("56842970-3915-4369-8712-61cfd8273ef9", (string) simpleUnit["iid"]);
            Assert.AreEqual(1, (int) simpleUnit["revisionNumber"]);
            Assert.AreEqual("SimpleUnit", (string) simpleUnit["classKind"]);

            Assert.IsFalse((bool) simpleUnit["isDeprecated"]);
            Assert.AreEqual("test simple unit", (string) simpleUnit["name"]);
            Assert.AreEqual("testsimpleunit", (string) simpleUnit["shortName"]);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) simpleUnit["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) simpleUnit["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) simpleUnit["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
