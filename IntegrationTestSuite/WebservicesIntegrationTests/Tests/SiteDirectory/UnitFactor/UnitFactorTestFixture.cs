// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitFactorTestFixture.cs" company="Starion Group S.A.">
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

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class UnitFactorTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedUnitFactorIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var unitFactorUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/unit/c394eaa9-4832-4b2d-8d88-5e1b2c43732c/unitFactor");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(unitFactorUri);

            //check if there is the only one UnitFactor object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific UnitFactor from the result by it's unique id
            var unitFactor = jArray.Single(x => (string) x[PropertyNames.Iid] == "56c30a85-f648-4b31-87d2-153e8a74048b");

            VerifyProperties(unitFactor);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedUnitFactorWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var unitFactorUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/unit/c394eaa9-4832-4b2d-8d88-5e1b2c43732c/unitFactor?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(unitFactorUri);

            //check if there are 4 objects
            Assert.AreEqual(4, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a SiteReferenceDataLibrary SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");

            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific DerivedUnit from the result by it's unique id
            var derivedUnit = jArray.Single(x => (string) x[PropertyNames.Iid] == "c394eaa9-4832-4b2d-8d88-5e1b2c43732c");

            DerivedUnitTestFixture.VerifyProperties(derivedUnit);

            // get a specific UnitFactor from the result by it's unique id
            var unitFactor = jArray.Single(x => (string) x[PropertyNames.Iid] == "56c30a85-f648-4b31-87d2-153e8a74048b");

            VerifyProperties(unitFactor);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatUnitFactorCanBeAddedAndReorderedFromWebApi()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/SiteDirectory/UnitFactor/PostNewUnitFactor.json");

            var postBody = this.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            Assert.AreEqual(3, jArray.Count);

            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.AreEqual(2, (int) siteDirectory[PropertyNames.RevisionNumber]);

            var unitFactor = jArray.Single(x => (string) x[PropertyNames.Iid] == "7d48eebe-c4e1-4081-ab63-7e4584563708");
            Assert.AreEqual(2, (int) unitFactor[PropertyNames.RevisionNumber]);

            var derivedUnit = jArray.Single(x => (string) x[PropertyNames.Iid] == "c394eaa9-4832-4b2d-8d88-5e1b2c43732c");
            Assert.AreEqual(2, (int) derivedUnit[PropertyNames.RevisionNumber]);

            var expectedUnitFactorArray = new List<OrderedItem> { new OrderedItem(2, "7d48eebe-c4e1-4081-ab63-7e4584563708"), new OrderedItem(23307173, "56c30a85-f648-4b31-87d2-153e8a74048b") };
            var unitFactorArray = JsonConvert.DeserializeObject<List<OrderedItem>>(derivedUnit[PropertyNames.UnitFactor].ToString());
            CollectionAssert.AreEquivalent(expectedUnitFactorArray, unitFactorArray);

            postBodyPath = this.GetPath("Tests/SiteDirectory/UnitFactor/PostReorderUnitFactor.json");

            postBody = this.GetJsonFromFile(postBodyPath);
            jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            Assert.AreEqual(4, jArray.Count);

            siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            Assert.AreEqual(3, (int) siteDirectory[PropertyNames.RevisionNumber]);

            unitFactor = jArray.Single(x => (string) x[PropertyNames.Iid] == "7d48eebe-c4e1-4081-ab63-7e4584563708");
            Assert.AreEqual(3, (int) unitFactor[PropertyNames.RevisionNumber]);

            var unitFactor2 = jArray.Single(x => (string) x[PropertyNames.Iid] == "56c30a85-f648-4b31-87d2-153e8a74048b");
            Assert.AreEqual(3, (int) unitFactor2[PropertyNames.RevisionNumber]);

            derivedUnit = jArray.Single(x => (string) x[PropertyNames.Iid] == "c394eaa9-4832-4b2d-8d88-5e1b2c43732c");
            Assert.AreEqual(3, (int) derivedUnit[PropertyNames.RevisionNumber]);

            expectedUnitFactorArray = new List<OrderedItem> { new OrderedItem(1, "56c30a85-f648-4b31-87d2-153e8a74048b"), new OrderedItem(3, "7d48eebe-c4e1-4081-ab63-7e4584563708") };
            unitFactorArray = JsonConvert.DeserializeObject<List<OrderedItem>>(derivedUnit[PropertyNames.UnitFactor].ToString());
            CollectionAssert.AreEquivalent(expectedUnitFactorArray, unitFactorArray);
        }

        /// <summary>
        /// Verifies all properties of the UnitFactor <see cref="JToken"/>
        /// </summary>
        /// <param name="unitFactor">
        /// The <see cref="JToken"/> that contains the properties of
        /// the UnitFactor object
        /// </param>
        public static void VerifyProperties(JToken unitFactor)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(5, unitFactor.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("56c30a85-f648-4b31-87d2-153e8a74048b", (string) unitFactor[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) unitFactor[PropertyNames.RevisionNumber]);
            Assert.AreEqual("UnitFactor", (string) unitFactor[PropertyNames.ClassKind]);
            Assert.AreEqual("56842970-3915-4369-8712-61cfd8273ef9", (string) unitFactor[PropertyNames.Unit]);
            Assert.AreEqual("2", (string) unitFactor[PropertyNames.Exponent]);
        }
    }
}
