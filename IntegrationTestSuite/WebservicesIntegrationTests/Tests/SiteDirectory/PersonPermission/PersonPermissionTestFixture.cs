// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonPermissionTestFixture.cs" company="RHEA System S.A.">
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
    using System.Linq;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class PersonPermissionTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedPersonPermissionIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var personPermissionUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/personRole/2428f4d9-f26d-4112-9d56-1c940748df69/personPermission");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(personPermissionUri);

            //check if there are 15 PersonPermission objects 
            Assert.AreEqual(15, jArray.Count);

            PersonPermissionTestFixture.VerifyProperties(jArray);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedPersonPermissionWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var personPermissionUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/personRole/2428f4d9-f26d-4112-9d56-1c940748df69/personPermission?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(personPermissionUri);

            //check if there are 17 objects
            Assert.AreEqual(17, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific PersonRole from the result by it's unique id
            var personRole = jArray.Single(x => (string) x["iid"] == "2428f4d9-f26d-4112-9d56-1c940748df69");
            PersonRoleTestFixture.VerifyProperties(personRole);

            PersonPermissionTestFixture.VerifyProperties(jArray);
        }

        /// <summary>
        /// Verifies all properties of the PersonPermission <see cref="JToken"/>
        /// </summary>
        /// <param name="personPermission">
        /// The <see cref="JToken"/> that contains the properties of
        /// the PersonPermission objects
        /// </param>
        public static void VerifyProperties(JToken personPermission)
        {
            // assert that all objects are what is expected
            var personPermissionObject = personPermission.Single(x => (string) x["iid"] == "9211fa6a-ea92-43fc-bf2e-799ffd4b05ac");
            Assert.AreEqual("9211fa6a-ea92-43fc-bf2e-799ffd4b05ac", (string) personPermissionObject["iid"]);
            Assert.AreEqual(1, (int) personPermissionObject["revisionNumber"]);
            Assert.AreEqual("PersonPermission", (string) personPermissionObject["classKind"]);
            Assert.IsFalse((bool) personPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) personPermissionObject["accessRight"]);
            Assert.AreEqual("ModelReferenceDataLibrary", (string) personPermissionObject["objectClass"]);

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "a59e0ad9-bde7-4aeb-9871-e440380c44ed");
            Assert.AreEqual("a59e0ad9-bde7-4aeb-9871-e440380c44ed", (string) personPermissionObject["iid"]);
            Assert.AreEqual(1, (int) personPermissionObject["revisionNumber"]);
            Assert.AreEqual("PersonPermission", (string) personPermissionObject["classKind"]);
            Assert.IsFalse((bool) personPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) personPermissionObject["accessRight"]);
            Assert.AreEqual("DomainOfExpertiseGroup", (string) personPermissionObject["objectClass"]);

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "47b4f696-cbe6-411b-b0dd-1550207aa798");
            Assert.AreEqual("47b4f696-cbe6-411b-b0dd-1550207aa798", (string) personPermissionObject["iid"]);
            Assert.AreEqual(1, (int) personPermissionObject["revisionNumber"]);
            Assert.AreEqual("PersonPermission", (string) personPermissionObject["classKind"]);
            Assert.IsFalse((bool) personPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) personPermissionObject["accessRight"]);
            Assert.AreEqual("Participant", (string) personPermissionObject["objectClass"]);

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "007e23c2-62c4-459d-8cb1-499d9d014bdc");
            Assert.AreEqual("007e23c2-62c4-459d-8cb1-499d9d014bdc", (string) personPermissionObject["iid"]);
            Assert.AreEqual(1, (int) personPermissionObject["revisionNumber"]);
            Assert.AreEqual("PersonPermission", (string) personPermissionObject["classKind"]);
            Assert.IsFalse((bool) personPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) personPermissionObject["accessRight"]);
            Assert.AreEqual("SiteDirectory", (string) personPermissionObject["objectClass"]);

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "0ec4ea88-3d84-411c-9a4a-0b1fb546281e");
            Assert.AreEqual("0ec4ea88-3d84-411c-9a4a-0b1fb546281e", (string) personPermissionObject["iid"]);
            Assert.AreEqual(1, (int) personPermissionObject["revisionNumber"]);
            Assert.AreEqual("PersonPermission", (string) personPermissionObject["classKind"]);
            Assert.IsFalse((bool) personPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) personPermissionObject["accessRight"]);
            Assert.AreEqual("ParticipantPermission", (string) personPermissionObject["objectClass"]);

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "163e6bc3-4639-4204-9a5f-4266baafd25f");
            Assert.AreEqual("163e6bc3-4639-4204-9a5f-4266baafd25f", (string) personPermissionObject["iid"]);
            Assert.AreEqual(1, (int) personPermissionObject["revisionNumber"]);
            Assert.AreEqual("PersonPermission", (string) personPermissionObject["classKind"]);
            Assert.IsFalse((bool) personPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) personPermissionObject["accessRight"]);
            Assert.AreEqual("PersonPermission", (string) personPermissionObject["objectClass"]);

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "c5c23adb-82d6-4d6e-9192-183dbd67e022");
            Assert.AreEqual("c5c23adb-82d6-4d6e-9192-183dbd67e022", (string) personPermissionObject["iid"]);
            Assert.AreEqual(1, (int) personPermissionObject["revisionNumber"]);
            Assert.AreEqual("PersonPermission", (string) personPermissionObject["classKind"]);
            Assert.IsFalse((bool) personPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) personPermissionObject["accessRight"]);
            Assert.AreEqual("Person", (string) personPermissionObject["objectClass"]);

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "272c94f3-8fe4-44c4-a915-616a0d93db26");
            Assert.AreEqual("272c94f3-8fe4-44c4-a915-616a0d93db26", (string) personPermissionObject["iid"]);
            Assert.AreEqual(1, (int) personPermissionObject["revisionNumber"]);
            Assert.AreEqual("PersonPermission", (string) personPermissionObject["classKind"]);
            Assert.IsFalse((bool) personPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) personPermissionObject["accessRight"]);
            Assert.AreEqual("IterationSetup", (string) personPermissionObject["objectClass"]);

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "6d336a9b-c4c3-44aa-8e96-17d9f626f3d7");
            Assert.AreEqual("6d336a9b-c4c3-44aa-8e96-17d9f626f3d7", (string) personPermissionObject["iid"]);
            Assert.AreEqual(1, (int) personPermissionObject["revisionNumber"]);
            Assert.AreEqual("PersonPermission", (string) personPermissionObject["classKind"]);
            Assert.IsFalse((bool) personPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) personPermissionObject["accessRight"]);
            Assert.AreEqual("EngineeringModelSetup", (string) personPermissionObject["objectClass"]);

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "db0e0a1d-c182-4c49-b375-abacfb77b5a5");
            Assert.AreEqual("db0e0a1d-c182-4c49-b375-abacfb77b5a5", (string) personPermissionObject["iid"]);
            Assert.AreEqual(1, (int) personPermissionObject["revisionNumber"]);
            Assert.AreEqual("PersonPermission", (string) personPermissionObject["classKind"]);
            Assert.IsFalse((bool) personPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) personPermissionObject["accessRight"]);
            Assert.AreEqual("SiteReferenceDataLibrary", (string) personPermissionObject["objectClass"]);

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "9108f724-fa36-49c2-a589-e5e3f1e98be1");
            Assert.AreEqual("9108f724-fa36-49c2-a589-e5e3f1e98be1", (string) personPermissionObject["iid"]);
            Assert.AreEqual(1, (int) personPermissionObject["revisionNumber"]);
            Assert.AreEqual("PersonPermission", (string) personPermissionObject["classKind"]);
            Assert.IsFalse((bool) personPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) personPermissionObject["accessRight"]);
            Assert.AreEqual("DomainOfExpertise", (string) personPermissionObject["objectClass"]);

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "1f56c492-8ce5-44a0-82eb-d515ac8653f7");
            Assert.AreEqual("1f56c492-8ce5-44a0-82eb-d515ac8653f7", (string) personPermissionObject["iid"]);
            Assert.AreEqual(1, (int) personPermissionObject["revisionNumber"]);
            Assert.AreEqual("PersonPermission", (string) personPermissionObject["classKind"]);
            Assert.IsFalse((bool) personPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) personPermissionObject["accessRight"]);
            Assert.AreEqual("Organization", (string) personPermissionObject["objectClass"]);

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "e2c74973-a0e0-4212-96fe-4b0a236a07a0");
            Assert.AreEqual("e2c74973-a0e0-4212-96fe-4b0a236a07a0", (string) personPermissionObject["iid"]);
            Assert.AreEqual(1, (int) personPermissionObject["revisionNumber"]);
            Assert.AreEqual("PersonPermission", (string) personPermissionObject["classKind"]);
            Assert.IsFalse((bool) personPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) personPermissionObject["accessRight"]);
            Assert.AreEqual("ParticipantRole", (string) personPermissionObject["objectClass"]);

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "fc37edbf-c4e3-4902-bae5-533631f36e29");
            Assert.AreEqual("fc37edbf-c4e3-4902-bae5-533631f36e29", (string) personPermissionObject["iid"]);
            Assert.AreEqual(1, (int) personPermissionObject["revisionNumber"]);
            Assert.AreEqual("PersonPermission", (string) personPermissionObject["classKind"]);
            Assert.IsFalse((bool) personPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) personPermissionObject["accessRight"]);
            Assert.AreEqual("PersonRole", (string) personPermissionObject["objectClass"]);

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "98b924e2-3709-4b2c-b304-b74e3eab13af");
            Assert.AreEqual("98b924e2-3709-4b2c-b304-b74e3eab13af", (string) personPermissionObject["iid"]);
            Assert.AreEqual(1, (int) personPermissionObject["revisionNumber"]);
            Assert.AreEqual("PersonPermission", (string) personPermissionObject["classKind"]);
            Assert.IsFalse((bool) personPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) personPermissionObject["accessRight"]);
            Assert.AreEqual("SiteLogEntry", (string) personPermissionObject["objectClass"]);
        }
    }
}
