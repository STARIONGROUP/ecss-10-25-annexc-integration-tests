// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RatioScaleTestFixture.cs" company="Starion Group S.A.">
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

namespace WebservicesIntegrationTests.Tests.SiteDirectory.RatioScale
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;
    
    [TestFixture]
    public class RatioScaleTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedScaleIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var ratioScaleUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/scale/53e82aeb-c42c-475c-b6bf-a102af883471");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(ratioScaleUri);

            //check if there is the only one RatioScale object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific RatioScale from the result by it's unique id
            var ratioScale = jArray.Single(x => (string) x["iid"] == "53e82aeb-c42c-475c-b6bf-a102af883471");

            RatioScaleTestFixture.VerifyProperties(ratioScale);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedScaleWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var ratioScaleUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/scale/53e82aeb-c42c-475c-b6bf-a102af883471?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(ratioScaleUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific RatioScale from the result by it's unique id
            var ratioScale = jArray.Single(x => (string) x["iid"] == "53e82aeb-c42c-475c-b6bf-a102af883471");
            RatioScaleTestFixture.VerifyProperties(ratioScale);
        }

        /// <summary>
        /// Verifies all properties of the RatioScale <see cref="JToken"/>
        /// </summary>
        /// <param name="ratioScale">
        /// The <see cref="JToken"/> that contains the properties of
        /// the RatioScale object
        /// </param>
        public static void VerifyProperties(JToken ratioScale)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(19, ratioScale.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("53e82aeb-c42c-475c-b6bf-a102af883471", (string) ratioScale["iid"]);
            Assert.AreEqual(1, (int) ratioScale["revisionNumber"]);
            Assert.AreEqual("RatioScale", (string) ratioScale["classKind"]);

            Assert.IsFalse((bool) ratioScale["isDeprecated"]);
            Assert.AreEqual("Test Ratio Scale", (string) ratioScale["name"]);
            Assert.AreEqual("TestRatioScale", (string) ratioScale["shortName"]);

            Assert.AreEqual("56842970-3915-4369-8712-61cfd8273ef9", (string) ratioScale["unit"]);

            var expectedValueDefinitions = new string[] {};
            var valueDefinitionsArray = (JArray) ratioScale["valueDefinition"];
            IList<string> valueDefinitions = valueDefinitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedValueDefinitions, valueDefinitions);

            Assert.AreEqual("REAL_NUMBER_SET", (string) ratioScale["numberSet"]);
            Assert.IsNull((string) ratioScale["minimumPermissibleValue"]);
            Assert.IsTrue((bool) ratioScale["isMinimumInclusive"]);
            Assert.IsNull((string) ratioScale["maximumPermissibleValue"]);
            Assert.IsTrue((bool) ratioScale["isMaximumInclusive"]);
            Assert.IsNull((string) ratioScale["positiveValueConnotation"]);
            Assert.IsNull((string) ratioScale["negativeValueConnotation"]);

            var expectedMappingToReferenceScales = new string[] {};
            var mappingToReferenceScalesArray = (JArray) ratioScale["mappingToReferenceScale"];
            IList<string> mappingToReferenceScales = mappingToReferenceScalesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedMappingToReferenceScales, mappingToReferenceScales);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) ratioScale["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) ratioScale["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) ratioScale["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
