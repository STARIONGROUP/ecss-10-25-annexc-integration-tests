// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonPermissionTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2025 Starion Group S.A.
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
            Assert.That(jArray.Count, Is.EqualTo(15));

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
            Assert.That(jArray.Count, Is.EqualTo(17));

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
            Assert.That((string) personPermissionObject["iid"], Is.EqualTo("9211fa6a-ea92-43fc-bf2e-799ffd4b05ac"));
            Assert.That((int) personPermissionObject["revisionNumber"], Is.EqualTo(1));
            Assert.That((string) personPermissionObject["classKind"], Is.EqualTo("PersonPermission"));
            Assert.That((bool) personPermissionObject["isDeprecated"], Is.False);
            Assert.That((string) personPermissionObject["accessRight"], Is.EqualTo("MODIFY"));
            Assert.That((string) personPermissionObject["objectClass"], Is.EqualTo("ModelReferenceDataLibrary"));

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "a59e0ad9-bde7-4aeb-9871-e440380c44ed");
            Assert.That((string) personPermissionObject["iid"], Is.EqualTo("a59e0ad9-bde7-4aeb-9871-e440380c44ed"));
            Assert.That((int) personPermissionObject["revisionNumber"], Is.EqualTo(1));
            Assert.That((string) personPermissionObject["classKind"], Is.EqualTo("PersonPermission"));
            Assert.That((bool) personPermissionObject["isDeprecated"], Is.False);
            Assert.That((string) personPermissionObject["accessRight"], Is.EqualTo("MODIFY"));
            Assert.That((string) personPermissionObject["objectClass"], Is.EqualTo("DomainOfExpertiseGroup"));

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "47b4f696-cbe6-411b-b0dd-1550207aa798");
            Assert.That((string) personPermissionObject["iid"], Is.EqualTo("47b4f696-cbe6-411b-b0dd-1550207aa798"));
            Assert.That((int) personPermissionObject["revisionNumber"], Is.EqualTo(1));
            Assert.That((string) personPermissionObject["classKind"], Is.EqualTo("PersonPermission"));
            Assert.That((bool) personPermissionObject["isDeprecated"], Is.False);
            Assert.That((string) personPermissionObject["accessRight"], Is.EqualTo("MODIFY"));
            Assert.That((string) personPermissionObject["objectClass"], Is.EqualTo("Participant"));

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "007e23c2-62c4-459d-8cb1-499d9d014bdc");
            Assert.That((string) personPermissionObject["iid"], Is.EqualTo("007e23c2-62c4-459d-8cb1-499d9d014bdc"));
            Assert.That((int) personPermissionObject["revisionNumber"], Is.EqualTo(1));
            Assert.That((string) personPermissionObject["classKind"], Is.EqualTo("PersonPermission"));
            Assert.That((bool) personPermissionObject["isDeprecated"], Is.False);
            Assert.That((string) personPermissionObject["accessRight"], Is.EqualTo("MODIFY"));
            Assert.That((string) personPermissionObject["objectClass"], Is.EqualTo("SiteDirectory"));

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "0ec4ea88-3d84-411c-9a4a-0b1fb546281e");
            Assert.That((string) personPermissionObject["iid"], Is.EqualTo("0ec4ea88-3d84-411c-9a4a-0b1fb546281e"));
            Assert.That((int) personPermissionObject["revisionNumber"], Is.EqualTo(1));
            Assert.That((string) personPermissionObject["classKind"], Is.EqualTo("PersonPermission"));
            Assert.That((bool) personPermissionObject["isDeprecated"], Is.False);
            Assert.That((string) personPermissionObject["accessRight"], Is.EqualTo("MODIFY"));
            Assert.That((string) personPermissionObject["objectClass"], Is.EqualTo("ParticipantPermission"));

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "163e6bc3-4639-4204-9a5f-4266baafd25f");
            Assert.That((string) personPermissionObject["iid"], Is.EqualTo("163e6bc3-4639-4204-9a5f-4266baafd25f"));
            Assert.That((int) personPermissionObject["revisionNumber"], Is.EqualTo(1));
            Assert.That((string) personPermissionObject["classKind"], Is.EqualTo("PersonPermission"));
            Assert.That((bool) personPermissionObject["isDeprecated"], Is.False);
            Assert.That((string) personPermissionObject["accessRight"], Is.EqualTo("MODIFY"));
            Assert.That((string) personPermissionObject["objectClass"], Is.EqualTo("PersonPermission"));

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "c5c23adb-82d6-4d6e-9192-183dbd67e022");
            Assert.That((string) personPermissionObject["iid"], Is.EqualTo("c5c23adb-82d6-4d6e-9192-183dbd67e022"));
            Assert.That((int) personPermissionObject["revisionNumber"], Is.EqualTo(1));
            Assert.That((string) personPermissionObject["classKind"], Is.EqualTo("PersonPermission"));
            Assert.That((bool) personPermissionObject["isDeprecated"], Is.False);
            Assert.That((string) personPermissionObject["accessRight"], Is.EqualTo("MODIFY"));
            Assert.That((string) personPermissionObject["objectClass"], Is.EqualTo("Person"));

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "272c94f3-8fe4-44c4-a915-616a0d93db26");
            Assert.That((string) personPermissionObject["iid"], Is.EqualTo("272c94f3-8fe4-44c4-a915-616a0d93db26"));
            Assert.That((int) personPermissionObject["revisionNumber"], Is.EqualTo(1));
            Assert.That((string) personPermissionObject["classKind"], Is.EqualTo("PersonPermission"));
            Assert.That((bool) personPermissionObject["isDeprecated"], Is.False);
            Assert.That((string) personPermissionObject["accessRight"], Is.EqualTo("MODIFY"));
            Assert.That((string) personPermissionObject["objectClass"], Is.EqualTo("IterationSetup"));

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "6d336a9b-c4c3-44aa-8e96-17d9f626f3d7");
            Assert.That((string) personPermissionObject["iid"], Is.EqualTo("6d336a9b-c4c3-44aa-8e96-17d9f626f3d7"));
            Assert.That((int) personPermissionObject["revisionNumber"], Is.EqualTo(1));
            Assert.That((string) personPermissionObject["classKind"], Is.EqualTo("PersonPermission"));
            Assert.That((bool) personPermissionObject["isDeprecated"], Is.False);
            Assert.That((string) personPermissionObject["accessRight"], Is.EqualTo("MODIFY"));
            Assert.That((string) personPermissionObject["objectClass"], Is.EqualTo("EngineeringModelSetup"));

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "db0e0a1d-c182-4c49-b375-abacfb77b5a5");
            Assert.That((string) personPermissionObject["iid"], Is.EqualTo("db0e0a1d-c182-4c49-b375-abacfb77b5a5"));
            Assert.That((int) personPermissionObject["revisionNumber"], Is.EqualTo(1));
            Assert.That((string) personPermissionObject["classKind"], Is.EqualTo("PersonPermission"));
            Assert.That((bool) personPermissionObject["isDeprecated"], Is.False);
            Assert.That((string) personPermissionObject["accessRight"], Is.EqualTo("MODIFY"));
            Assert.That((string) personPermissionObject["objectClass"], Is.EqualTo("SiteReferenceDataLibrary"));

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "9108f724-fa36-49c2-a589-e5e3f1e98be1");
            Assert.That((string) personPermissionObject["iid"], Is.EqualTo("9108f724-fa36-49c2-a589-e5e3f1e98be1"));
            Assert.That((int) personPermissionObject["revisionNumber"], Is.EqualTo(1));
            Assert.That((string) personPermissionObject["classKind"], Is.EqualTo("PersonPermission"));
            Assert.That((bool) personPermissionObject["isDeprecated"], Is.False);
            Assert.That((string) personPermissionObject["accessRight"], Is.EqualTo("MODIFY"));
            Assert.That((string) personPermissionObject["objectClass"], Is.EqualTo("DomainOfExpertise"));

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "1f56c492-8ce5-44a0-82eb-d515ac8653f7");
            Assert.That((string) personPermissionObject["iid"], Is.EqualTo("1f56c492-8ce5-44a0-82eb-d515ac8653f7"));
            Assert.That((int) personPermissionObject["revisionNumber"], Is.EqualTo(1));
            Assert.That((string) personPermissionObject["classKind"], Is.EqualTo("PersonPermission"));
            Assert.That((bool) personPermissionObject["isDeprecated"], Is.False);
            Assert.That((string) personPermissionObject["accessRight"], Is.EqualTo("MODIFY"));
            Assert.That((string) personPermissionObject["objectClass"], Is.EqualTo("Organization"));

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "e2c74973-a0e0-4212-96fe-4b0a236a07a0");
            Assert.That((string) personPermissionObject["iid"], Is.EqualTo("e2c74973-a0e0-4212-96fe-4b0a236a07a0"));
            Assert.That((int) personPermissionObject["revisionNumber"], Is.EqualTo(1));
            Assert.That((string) personPermissionObject["classKind"], Is.EqualTo("PersonPermission"));
            Assert.That((bool) personPermissionObject["isDeprecated"], Is.False);
            Assert.That((string) personPermissionObject["accessRight"], Is.EqualTo("MODIFY"));
            Assert.That((string) personPermissionObject["objectClass"], Is.EqualTo("ParticipantRole"));

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "fc37edbf-c4e3-4902-bae5-533631f36e29");
            Assert.That((string) personPermissionObject["iid"], Is.EqualTo("fc37edbf-c4e3-4902-bae5-533631f36e29"));
            Assert.That((int) personPermissionObject["revisionNumber"], Is.EqualTo(1));
            Assert.That((string) personPermissionObject["classKind"], Is.EqualTo("PersonPermission"));
            Assert.That((bool) personPermissionObject["isDeprecated"], Is.False);
            Assert.That((string) personPermissionObject["accessRight"], Is.EqualTo("MODIFY"));
            Assert.That((string) personPermissionObject["objectClass"], Is.EqualTo("PersonRole"));

            personPermissionObject = personPermission.Single(x => (string) x["iid"] == "98b924e2-3709-4b2c-b304-b74e3eab13af");
            Assert.That((string) personPermissionObject["iid"], Is.EqualTo("98b924e2-3709-4b2c-b304-b74e3eab13af"));
            Assert.That((int) personPermissionObject["revisionNumber"], Is.EqualTo(1));
            Assert.That((string) personPermissionObject["classKind"], Is.EqualTo("PersonPermission"));
            Assert.That((bool) personPermissionObject["isDeprecated"], Is.False);
            Assert.That((string) personPermissionObject["accessRight"], Is.EqualTo("MODIFY"));
            Assert.That((string) personPermissionObject["objectClass"], Is.EqualTo("SiteLogEntry"));
        }
    }
}
