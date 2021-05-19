// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogarithmicScaleTestFixture.cs" company="RHEA System S.A.">
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
    public class LogarithmicScaleTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedScaleIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var logarithmicScaleUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/scale/007b0e60-e67c-4060-88d2-2531ef9e7d9e");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(logarithmicScaleUri);

            //check if there is the only one LogarithmicScale object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific LogarithmicScale from the result by it's unique id
            var logarithmicScale = jArray.Single(x => (string) x["iid"] == "007b0e60-e67c-4060-88d2-2531ef9e7d9e");

            LogarithmicScaleTestFixture.VerifyProperties(logarithmicScale);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedScaleWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var logarithmicScaleUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/scale/007b0e60-e67c-4060-88d2-2531ef9e7d9e?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(logarithmicScaleUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific LogarithmicScale from the result by it's unique id
            var logarithmicScale = jArray.Single(x => (string) x["iid"] == "007b0e60-e67c-4060-88d2-2531ef9e7d9e");
            LogarithmicScaleTestFixture.VerifyProperties(logarithmicScale);
        }

        /// <summary>
        /// Verifies all properties of the LogarithmicScale <see cref="JToken"/>
        /// </summary>
        /// <param name="logarithmicScale">
        /// The <see cref="JToken"/> that contains the properties of
        /// the LogarithmicScale object
        /// </param>
        public static void VerifyProperties(JToken logarithmicScale)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(24, logarithmicScale.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("007b0e60-e67c-4060-88d2-2531ef9e7d9e", (string) logarithmicScale["iid"]);
            Assert.AreEqual(1, (int) logarithmicScale["revisionNumber"]);
            Assert.AreEqual("LogarithmicScale", (string) logarithmicScale["classKind"]);

            Assert.AreEqual("Test Logarithmic Scale", (string) logarithmicScale["name"]);
            Assert.AreEqual("TestLogarithmicScale", (string) logarithmicScale["shortName"]);
            Assert.AreEqual("TestLogarithmicScale", (string) logarithmicScale["shortName"]);

            Assert.IsFalse((bool) logarithmicScale["isDeprecated"]);
            Assert.AreEqual("56842970-3915-4369-8712-61cfd8273ef9", (string) logarithmicScale["unit"]);

            Assert.AreEqual("BASE10", (string) logarithmicScale["logarithmBase"]);
            Assert.AreEqual("10", (string) logarithmicScale["factor"]);
            Assert.AreEqual("1", (string) logarithmicScale["exponent"]);
            Assert.AreEqual("4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d", (string) logarithmicScale["referenceQuantityKind"]);

            var expectedReferenceQuantityValues = new string[]
            {
                "4fedaa27-bb84-4e53-8a39-956bf90a4579"
            };
            var referenceQuantityValuesArray = (JArray) logarithmicScale["referenceQuantityValue"];
            IList<string> referenceQuantityValues = referenceQuantityValuesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedReferenceQuantityValues, referenceQuantityValues);

            var expectedValueDefinitions = new string[] {};
            var valueDefinitionsArray = (JArray) logarithmicScale["valueDefinition"];
            IList<string> valueDefinitions = valueDefinitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedValueDefinitions, valueDefinitions);

            Assert.AreEqual("REAL_NUMBER_SET", (string) logarithmicScale["numberSet"]);
            Assert.AreEqual("0", (string) logarithmicScale["minimumPermissibleValue"]);
            Assert.IsFalse((bool) logarithmicScale["isMinimumInclusive"]);
            Assert.IsNull((string) logarithmicScale["maximumPermissibleValue"]);
            Assert.IsTrue((bool) logarithmicScale["isMaximumInclusive"]);
            Assert.AreEqual("", logarithmicScale["positiveValueConnotation"]);
            Assert.AreEqual("", logarithmicScale["negativeValueConnotation"]);
            
            var expectedMappingToReferenceScales = new string[] {};
            var mappingToReferenceScalesArray = (JArray) logarithmicScale["mappingToReferenceScale"];
            IList<string> mappingToReferenceScales = mappingToReferenceScalesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedMappingToReferenceScales, mappingToReferenceScales);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) logarithmicScale["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) logarithmicScale["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) logarithmicScale["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
