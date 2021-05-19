// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitPrefixTestFixture.cs" company="RHEA System S.A.">
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
    public class UnitPrefixTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedUnitPrefixIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var unitPrefixUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/unitPrefix");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(unitPrefixUri);

            //check if there is the only one UnitPrefix object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific UnitPrefix from the result by it's unique id
            var unitPrefix = jArray.Single(x => (string) x["iid"] == "efa6380d-9508-4f3d-9b43-6ed33125b780");

            UnitPrefixTestFixture.VerifyProperties(unitPrefix);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedUnitPrefixWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var unitPrefixUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/unitPrefix/efa6380d-9508-4f3d-9b43-6ed33125b780?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(unitPrefixUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific UnitPrefix from the result by it's unique id
            var unitPrefix = jArray.Single(x => (string) x["iid"] == "efa6380d-9508-4f3d-9b43-6ed33125b780");
            UnitPrefixTestFixture.VerifyProperties(unitPrefix);
        }

        /// <summary>
        /// Verifies all properties of the UnitPrefix <see cref="JToken"/>
        /// </summary>
        /// <param name="unitPrefix">
        /// The <see cref="JToken"/> that contains the properties of
        /// the UnitPrefix object
        /// </param>
        public static void VerifyProperties(JToken unitPrefix)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(10, unitPrefix.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("efa6380d-9508-4f3d-9b43-6ed33125b780", (string) unitPrefix["iid"]);
            Assert.AreEqual(1, (int) unitPrefix["revisionNumber"]);
            Assert.AreEqual("UnitPrefix", (string) unitPrefix["classKind"]);

            Assert.IsFalse((bool) unitPrefix["isDeprecated"]);
            Assert.AreEqual("kilo", (string) unitPrefix["name"]);
            Assert.AreEqual("k", (string) unitPrefix["shortName"]);

            Assert.AreEqual("1e3", (string) unitPrefix["conversionFactor"]);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) unitPrefix["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) unitPrefix["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) unitPrefix["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
