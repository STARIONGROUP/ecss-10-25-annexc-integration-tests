// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeOfDayParameterTypeTestFixture.cs" company="RHEA System S.A.">
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
    public class TimeOfDayParameterTypeTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterTypeIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var timeOfDayParameterTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/e4cfdb60-ed3a-455c-9a33-a3edc921637f");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(timeOfDayParameterTypeUri);

            //check if there is the only one TimeOfDayParameterType object 
            Assert.AreEqual(1, jArray.Count);

            // get a specific TimeOfDayParameterType from the result by it's unique id
            var timeOfDayParameterType = jArray.Single(x => (string) x[PropertyNames.Iid] == "e4cfdb60-ed3a-455c-9a33-a3edc921637f");

            TimeOfDayParameterTypeTestFixture.VerifyProperties(timeOfDayParameterType);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterTypeWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var timeOfDayParameterTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/e4cfdb60-ed3a-455c-9a33-a3edc921637f?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(timeOfDayParameterTypeUri);

            //check if there are 3 objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArray.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific TimeOfDayParameterType from the result by it's unique id
            var timeOfDayParameterType = jArray.Single(x => (string) x[PropertyNames.Iid] == "e4cfdb60-ed3a-455c-9a33-a3edc921637f");
            TimeOfDayParameterTypeTestFixture.VerifyProperties(timeOfDayParameterType);
        }

        /// <summary>
        /// Verifies all properties of the TimeOfDayParameterType <see cref="JToken"/>
        /// </summary>
        /// <param name="timeOfDayParameterType">
        /// The <see cref="JToken"/> that contains the properties of
        /// the TimeOfDayParameterType object
        /// </param>
        public static void VerifyProperties(JToken timeOfDayParameterType)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(11, timeOfDayParameterType.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("e4cfdb60-ed3a-455c-9a33-a3edc921637f", (string) timeOfDayParameterType[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) timeOfDayParameterType[PropertyNames.RevisionNumber]);
            Assert.AreEqual("TimeOfDayParameterType", (string) timeOfDayParameterType[PropertyNames.ClassKind]);

            Assert.IsFalse((bool) timeOfDayParameterType[PropertyNames.IsDeprecated]);
            Assert.AreEqual("Test Time Of Day ParameterType", (string) timeOfDayParameterType[PropertyNames.Name]);
            Assert.AreEqual("TestTimeOfDayParameterType", (string) timeOfDayParameterType[PropertyNames.ShortName]);

            Assert.AreEqual("timeofday", (string) timeOfDayParameterType[PropertyNames.Symbol]);

            var expectedCategories = new string[] {};
            var categoriesArray = (JArray) timeOfDayParameterType[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) timeOfDayParameterType[PropertyNames.Alias];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) timeOfDayParameterType[PropertyNames.Definition];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) timeOfDayParameterType[PropertyNames.HyperLink];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);
        }
    }
}
