// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonRoleTestFixture.cs" company="RHEA System S.A.">
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
    public class PersonRoleTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the PersonRole objects are returned from the data-source and that the 
        /// values of the PersonRole properties are equal to the expected value
        /// </summary>
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedPersonRoleIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var personRolesUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/personRole"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(personRolesUri);

            //check if there is only one PersonRole object
            Assert.AreEqual(1, jArray.Count);

            // get a specific PersonRole from the result by it's unique id
            var personRole = jArray.Single(x => (string) x["iid"] == "2428f4d9-f26d-4112-9d56-1c940748df69");

            PersonRoleTestFixture.VerifyProperties(personRole);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedPersonRoleWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var personRolesUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/personRole?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(personRolesUri);

            //check if there are only two objects
            Assert.AreEqual(2, jArray.Count);

            var siteDirectory = jArray.Single(x => (string)x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific PersonRole from the result by it's unique id
            var personRole = jArray.Single(x => (string)x["iid"] == "2428f4d9-f26d-4112-9d56-1c940748df69");
            PersonRoleTestFixture.VerifyProperties(personRole);
        }

        /// <summary>
        /// Verifies all properties of the PersonRole <see cref="JToken"/>
        /// </summary>
        /// <param name="personRole">
        /// The <see cref="JToken"/> that contains the properties of
        /// the PersonRole object
        /// </param>
        public static void VerifyProperties(JToken personRole)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(10, personRole.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("2428f4d9-f26d-4112-9d56-1c940748df69", (string) personRole["iid"]);
            Assert.AreEqual(1, (int) personRole["revisionNumber"]);
            Assert.AreEqual("PersonRole", (string) personRole["classKind"]);


            Assert.AreEqual("Test Person Role", (string) personRole["name"]);
            Assert.AreEqual("TestPersonRole", (string) personRole["shortName"]);
            Assert.IsFalse((bool) personRole["isDeprecated"]);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) personRole["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedAliases, aliases);

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) personRole["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedDefinitions, definitions);

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) personRole["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedHyperlinks, h);

            var expectedPersonPermission = new string[]
            {
                "9108f724-fa36-49c2-a589-e5e3f1e98be1",
                "a59e0ad9-bde7-4aeb-9871-e440380c44ed",
                "6d336a9b-c4c3-44aa-8e96-17d9f626f3d7",
                "272c94f3-8fe4-44c4-a915-616a0d93db26",
                "9211fa6a-ea92-43fc-bf2e-799ffd4b05ac",
                "1f56c492-8ce5-44a0-82eb-d515ac8653f7",
                "47b4f696-cbe6-411b-b0dd-1550207aa798",
                "0ec4ea88-3d84-411c-9a4a-0b1fb546281e",
                "e2c74973-a0e0-4212-96fe-4b0a236a07a0",
                "c5c23adb-82d6-4d6e-9192-183dbd67e022",
                "163e6bc3-4639-4204-9a5f-4266baafd25f",
                "fc37edbf-c4e3-4902-bae5-533631f36e29",
                "007e23c2-62c4-459d-8cb1-499d9d014bdc",
                "98b924e2-3709-4b2c-b304-b74e3eab13af",
                "db0e0a1d-c182-4c49-b375-abacfb77b5a5"
            };
            var personPermissionArray = (JArray) personRole["personPermission"];
            IList<string> permissions = personPermissionArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedPersonPermission, permissions);
        }
    }
}
