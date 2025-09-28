// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntervalScaleTestFixture.cs" company="Starion Group S.A.">
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
    public class IntervalScaleTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedScaleIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var intervalScaleUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/scale/6326d1ea-c032-4a4b-8b10-608c59f1a923");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(intervalScaleUri);

            //check if there is the only one IntervalScale object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific IntervalScale from the result by it's unique id
            var intervalScale = jArray.Single(x => (string) x["iid"] == "6326d1ea-c032-4a4b-8b10-608c59f1a923");

            IntervalScaleTestFixture.VerifyProperties(intervalScale);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedScaleWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var intervalScaleUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/scale/6326d1ea-c032-4a4b-8b10-608c59f1a923?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(intervalScaleUri);

            //check if there are 3 objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x["iid"] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific IntervalScale from the result by it's unique id
            var intervalScale = jArray.Single(x => (string) x["iid"] == "6326d1ea-c032-4a4b-8b10-608c59f1a923");
            IntervalScaleTestFixture.VerifyProperties(intervalScale);
        }

        /// <summary>
        /// Verifies all properties of the IntervalScale <see cref="JToken"/>
        /// </summary>
        /// <param name="intervalScale">
        /// The <see cref="JToken"/> that contains the properties of
        /// the IntervalScale object
        /// </param>
        public static void VerifyProperties(JToken intervalScale)
        {
            // verify the amount of returned properties 
            Assert.That(intervalScale.Children().Count(), Is.EqualTo(19));

            // assert that the properties are what is expected
            Assert.That((string) intervalScale["iid"], Is.EqualTo("6326d1ea-c032-4a4b-8b10-608c59f1a923"));
            Assert.That((int) intervalScale["revisionNumber"], Is.EqualTo(1));
            Assert.That((string) intervalScale["classKind"], Is.EqualTo("IntervalScale"));

            Assert.That((string) intervalScale["name"], Is.EqualTo("Test Interval Scale"));
            Assert.That((string) intervalScale["shortName"], Is.EqualTo("TestIntervalScale"));

            Assert.That((bool) intervalScale["isDeprecated"], Is.False);
            Assert.That((string) intervalScale["unit"], Is.EqualTo("56842970-3915-4369-8712-61cfd8273ef9"));

            var expectedValueDefinitions = new string[] {};
            var valueDefinitionsArray = (JArray) intervalScale["valueDefinition"];
            IList<string> valueDefinitions = valueDefinitionsArray.Select(x => (string) x).ToList();
            Assert.That(valueDefinitions, Is.EquivalentTo(expectedValueDefinitions));

            Assert.That((string) intervalScale["numberSet"], Is.EqualTo("NATURAL_NUMBER_SET"));
            Assert.That((string) intervalScale["minimumPermissibleValue"], Is.EqualTo("1"));
            Assert.That((bool) intervalScale["isMinimumInclusive"], Is.True);
            Assert.That((string) intervalScale["maximumPermissibleValue"], Is.EqualTo("2"));
            Assert.That((bool) intervalScale["isMaximumInclusive"], Is.True);
            Assert.That((string) intervalScale["positiveValueConnotation"], Is.EqualTo("positive Value Connotation"));
            Assert.That((string) intervalScale["negativeValueConnotation"], Is.EqualTo("negative Value Connotation"));

            var expectedMappingToReferenceScales = new string[] {};
            var mappingToReferenceScalesArray = (JArray) intervalScale["mappingToReferenceScale"];
            IList<string> mappingToReferenceScales = mappingToReferenceScalesArray.Select(x => (string) x).ToList();
            Assert.That(mappingToReferenceScales, Is.EquivalentTo(expectedMappingToReferenceScales));

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) intervalScale["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) intervalScale["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) intervalScale["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));
        }
    }
}
