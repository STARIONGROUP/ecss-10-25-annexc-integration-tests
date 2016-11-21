// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CyclicRatioScaleTestFixture.cs" company="RHEA System">
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
    public class CyclicRatioScaleTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the CyclicRatioScale objects are returned from the data-source and that the 
        /// values of the CyclicRatioScale properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedScaleIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var cyclicRatioScaleUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/scale/f9d4b3c6-91a2-4f38-bb86-f504d6ac706f"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(cyclicRatioScaleUri);

            //check if there is the only one CyclicRatioScale object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific CyclicRatioScale from the result by it's unique id
            var cyclicRatioScale =
                jArray.Single(x => (string) x["iid"] == "f9d4b3c6-91a2-4f38-bb86-f504d6ac706f");

            CyclicRatioScaleTestFixture.VerifyProperties(cyclicRatioScale);
        }

        [Test]
        public void VerifyThatExpectedScaleWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var cyclicRatioScaleUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/scale/f9d4b3c6-91a2-4f38-bb86-f504d6ac706f?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(cyclicRatioScaleUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary =
                jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific CyclicRatioScale from the result by it's unique id
            var cyclicRatioScale =
                jArray.Single(x => (string) x["iid"] == "f9d4b3c6-91a2-4f38-bb86-f504d6ac706f");
            CyclicRatioScaleTestFixture.VerifyProperties(cyclicRatioScale);
        }

        /// <summary>
        /// Verifies all properties of the CyclicRatioScale <see cref="JToken"/>
        /// </summary>
        /// <param name="cyclicRatioScale">
        /// The <see cref="JToken"/> that contains the properties of
        /// the CyclicRatioScale object
        /// </param>
        public static void VerifyProperties(JToken cyclicRatioScale)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(20, cyclicRatioScale.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("f9d4b3c6-91a2-4f38-bb86-f504d6ac706f", (string) cyclicRatioScale["iid"]);
            Assert.AreEqual(1, (int) cyclicRatioScale["revisionNumber"]);
            Assert.AreEqual("CyclicRatioScale", (string) cyclicRatioScale["classKind"]);

            Assert.AreEqual("Test Cyclic Ratio Scale", (string) cyclicRatioScale["name"]);
            Assert.AreEqual("TestCyclicRatioScale", (string) cyclicRatioScale["shortName"]);

            Assert.IsFalse((bool) cyclicRatioScale["isDeprecated"]);
            Assert.AreEqual("360", (string) cyclicRatioScale["modulus"]);
            Assert.AreEqual("56842970-3915-4369-8712-61cfd8273ef9", (string) cyclicRatioScale["unit"]);

            var expectedValueDefinitions = new string[] {};
            var valueDefinitionsArray = (JArray) cyclicRatioScale["valueDefinition"];
            IList<string> valueDefinitions = valueDefinitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedValueDefinitions, valueDefinitions);

            Assert.AreEqual("NATURAL_NUMBER_SET", (string) cyclicRatioScale["numberSet"]);
            Assert.AreEqual("0", (string) cyclicRatioScale["minimumPermissibleValue"]);
            Assert.IsTrue((bool) cyclicRatioScale["isMinimumInclusive"]);
            Assert.AreEqual("360", (string) cyclicRatioScale["maximumPermissibleValue"]);
            Assert.IsTrue((bool) cyclicRatioScale["isMaximumInclusive"]);
            Assert.IsNull((string) cyclicRatioScale["positiveValueConnotation"]);
            Assert.AreEqual("", (string)cyclicRatioScale["negativeValueConnotation"]);
            
            var expectedMappingToReferenceScales = new string[] {};
            var mappingToReferenceScalesArray = (JArray) cyclicRatioScale["mappingToReferenceScale"];
            IList<string> mappingToReferenceScales = mappingToReferenceScalesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedMappingToReferenceScales, mappingToReferenceScales);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) cyclicRatioScale["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) cyclicRatioScale["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) cyclicRatioScale["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}