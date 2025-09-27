// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrdinalScaleTestFixture.cs" company="Starion Group S.A.">
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
    public class OrdinalScaleTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedScaleIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var ordinalScaleUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/scale/541037e2-9f6a-466c-b56f-a09f81f36576");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(ordinalScaleUri);

            //check if there is the only one OrdinalScale object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific OrdinalScale from the result by it's unique id
            var ordinalScale = jArray.Single(x => (string) x["iid"] == "541037e2-9f6a-466c-b56f-a09f81f36576");

            OrdinalScaleTestFixture.VerifyProperties(ordinalScale);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedScaleWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var ordinalScaleUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/scale/541037e2-9f6a-466c-b56f-a09f81f36576?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(ordinalScaleUri);

            //check if there are 3 objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific OrdinalScale from the result by it's unique id
            var ordinalScale = jArray.Single(x => (string) x["iid"] == "541037e2-9f6a-466c-b56f-a09f81f36576");
            OrdinalScaleTestFixture.VerifyProperties(ordinalScale);
        }

        /// <summary>
        /// Verifies all properties of the OrdinalScale <see cref="JToken"/>
        /// </summary>
        /// <param name="ordinalScale">
        /// The <see cref="JToken"/> that contains the properties of
        /// the OrdinalScale object
        /// </param>
        public static void VerifyProperties(JToken ordinalScale)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(20, ordinalScale.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("541037e2-9f6a-466c-b56f-a09f81f36576", (string) ordinalScale["iid"]);
            Assert.AreEqual(1, (int) ordinalScale["revisionNumber"]);
            Assert.AreEqual("OrdinalScale", (string) ordinalScale["classKind"]);

            Assert.IsTrue((bool) ordinalScale["useShortNameValues"]);
            Assert.AreEqual("Test Ordinal Scale", (string) ordinalScale["name"]);
            Assert.AreEqual("TestOrdinalScale", (string) ordinalScale["shortName"]);

            Assert.IsFalse((bool) ordinalScale["isDeprecated"]);
            Assert.AreEqual("56842970-3915-4369-8712-61cfd8273ef9", (string) ordinalScale["unit"]);

            var expectedValueDefinitions = new string[]
            {
                "bf4ce301-402a-4eef-98b7-45faf37a1ea4"
            };
            var valueDefinitionsArray = (JArray) ordinalScale["valueDefinition"];
            IList<string> valueDefinitions = valueDefinitionsArray.Select(x => (string) x).ToList();
            Assert.That(valueDefinitions, Is.EquivalentTo(expectedValueDefinitions));

            Assert.AreEqual("NATURAL_NUMBER_SET", (string) ordinalScale["numberSet"]);
            Assert.AreEqual("0", (string) ordinalScale["minimumPermissibleValue"]);
            Assert.IsTrue((bool) ordinalScale["isMinimumInclusive"]);
            Assert.AreEqual("100", (string) ordinalScale["maximumPermissibleValue"]);
            Assert.IsTrue((bool) ordinalScale["isMaximumInclusive"]);
            Assert.AreEqual("positive Value Connotation", (string) ordinalScale["positiveValueConnotation"]);
            Assert.AreEqual("negative Value Connotation", (string) ordinalScale["negativeValueConnotation"]);

            var expectedMappingToReferenceScales = new string[] {};
            var mappingToReferenceScalesArray = (JArray) ordinalScale["mappingToReferenceScale"];
            IList<string> mappingToReferenceScales = mappingToReferenceScalesArray.Select(x => (string) x).ToList();
            Assert.That(mappingToReferenceScales, Is.EquivalentTo(expectedMappingToReferenceScales));

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) ordinalScale["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) ordinalScale["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) ordinalScale["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));
        }
    }
}
