// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParticipantPermissionTestFixture.cs" company="RHEA System">
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
    using Newtonsoft.Json;
    using NUnit.Framework;
    using Newtonsoft.Json.Linq;

    [TestFixture]
    public class ParticipantPermissionTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the ParticipantPermission objects are returned from the data-source and that the 
        /// values of the ParticipantPermission properties are equal to the expected value
        /// </summary>
        [Test]
        public void VerifyThatExpectedParticipantPermissionIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var participantPermissionUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/participantRole/ee3ae5ff-ac5e-4957-bab1-7698fba2a267/participantPermission"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(participantPermissionUri);

            //check if there are 24 ParticipantPermission objects 
            Assert.AreEqual(24, jArray.Count);

            ParticipantPermissionTestFixture.VerifyProperties(jArray);
        }

        [Test]
        public void VerifyThatExpectedParticipantPermissionWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var participantPermissionUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/participantRole/ee3ae5ff-ac5e-4957-bab1-7698fba2a267/participantPermission?includeAllContainers=true"));

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(participantPermissionUri);

            //check if there are 26 objects
            Assert.AreEqual(26, jArray.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific ParticipantRole from the result by it's unique id
            var participantRole =
                jArray.Single(x => (string) x["iid"] == "ee3ae5ff-ac5e-4957-bab1-7698fba2a267");
            ParticipantRoleTestFixture.VerifyProperties(participantRole);

            ParticipantPermissionTestFixture.VerifyProperties(jArray);
        }

        /// <summary>
        /// Verifies all properties of the ParticipantPermission <see cref="JToken"/>
        /// </summary>
        /// <param name="participantPermission">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ParticipantPermission objects
        /// </param>
        public static void VerifyProperties(JToken participantPermission)
        {
            var participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "c8656ec9-3377-4ea6-9d20-825e8f6fe335");
            Assert.AreEqual("c8656ec9-3377-4ea6-9d20-825e8f6fe335", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("File", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "8460a98f-6958-4caa-aa13-47b350e4bea9");
            Assert.AreEqual("8460a98f-6958-4caa-aa13-47b350e4bea9", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("Parameter", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "c3cf7ff9-4a4f-44a4-b91f-9918a12f664b");
            Assert.AreEqual("c3cf7ff9-4a4f-44a4-b91f-9918a12f664b", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("Relationship", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "8fdfec18-72d7-4e14-bcd1-5298f7594333");
            Assert.AreEqual("8fdfec18-72d7-4e14-bcd1-5298f7594333", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("ModelLogEntry", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "b380b256-8938-4dab-98c5-554cb619e81b");
            Assert.AreEqual("b380b256-8938-4dab-98c5-554cb619e81b", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("Iteration", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "39a2a62a-655d-45c1-8b6f-6bad372f935b");
            Assert.AreEqual("39a2a62a-655d-45c1-8b6f-6bad372f935b", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("ElementUsage", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "1bebf97d-114a-4baa-ba61-0f01eeb7e1ad");
            Assert.AreEqual("1bebf97d-114a-4baa-ba61-0f01eeb7e1ad", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("RequirementsSpecification", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "72cd93f4-e8f7-446b-a369-838b5a1ecc57");
            Assert.AreEqual("72cd93f4-e8f7-446b-a369-838b5a1ecc57", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("ParameterGroup", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "4ac0b072-8d58-46e0-8073-a48013c4c5be");
            Assert.AreEqual("4ac0b072-8d58-46e0-8073-a48013c4c5be", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("ExternalIdentifierMap", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "bc7a0939-d6ee-4a08-9bb5-c350755dcd0d");
            Assert.AreEqual("bc7a0939-d6ee-4a08-9bb5-c350755dcd0d", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("RequirementsGroup", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "0145d47c-2971-4f89-9dc8-2c5e0453805e");
            Assert.AreEqual("0145d47c-2971-4f89-9dc8-2c5e0453805e", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("ElementDefinition", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "b9eefd51-9da8-4f3f-b67c-8259ebad3b72");
            Assert.AreEqual("b9eefd51-9da8-4f3f-b67c-8259ebad3b72", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("ParameterOverride", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "c189f7cb-056e-4659-affa-b3ff4f6ba881");
            Assert.AreEqual("c189f7cb-056e-4659-affa-b3ff4f6ba881", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("NestedElement", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "dbe6a4b9-3b14-4580-8d3a-33ddf325c651");
            Assert.AreEqual("dbe6a4b9-3b14-4580-8d3a-33ddf325c651", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("Requirement", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "bd276630-1d3f-45d0-9aa5-0efb9accbadb");
            Assert.AreEqual("bd276630-1d3f-45d0-9aa5-0efb9accbadb", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("Publication", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "df41e148-e228-4f19-b15a-2b68b75891e9");
            Assert.AreEqual("df41e148-e228-4f19-b15a-2b68b75891e9", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("NestedParameter", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "ee7a7848-90d5-4914-b258-4a00c0543db0");
            Assert.AreEqual("ee7a7848-90d5-4914-b258-4a00c0543db0", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("CommonFileStore", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "ac6fe12e-fdaa-4b94-9822-312d0897f5cb");
            Assert.AreEqual("ac6fe12e-fdaa-4b94-9822-312d0897f5cb", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("ActualFiniteStateList", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "f0bb5b06-b7b1-4e67-ae02-c71d73e71d00");
            Assert.AreEqual("f0bb5b06-b7b1-4e67-ae02-c71d73e71d00", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("PossibleFiniteStateList", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "7f8f6f00-1a3d-412c-a276-747306a4f961");
            Assert.AreEqual("7f8f6f00-1a3d-412c-a276-747306a4f961", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("ParameterSubscription", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "a18e23ea-77a1-481f-ad4a-b23a9022f93f");
            Assert.AreEqual("a18e23ea-77a1-481f-ad4a-b23a9022f93f", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("Folder", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "8f3627a8-7782-4b1c-8677-e546db6965a9");
            Assert.AreEqual("8f3627a8-7782-4b1c-8677-e546db6965a9", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("RuleVerificationList", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "af6526f8-66f4-4a0b-9c49-d9ae00e97108");
            Assert.AreEqual("af6526f8-66f4-4a0b-9c49-d9ae00e97108", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("DomainFileStore", (string) participantPermissionObject["objectClass"]);

            participantPermissionObject =
                participantPermission.Single(x => (string) x["iid"] == "939bdada-f827-4f27-8761-ba78918431dd");
            Assert.AreEqual("939bdada-f827-4f27-8761-ba78918431dd", (string) participantPermissionObject["iid"]);
            Assert.AreEqual(1, (int) participantPermissionObject["revisionNumber"]);
            Assert.AreEqual("ParticipantPermission", (string) participantPermissionObject["classKind"]);
            Assert.IsFalse((bool) participantPermissionObject["isDeprecated"]);
            Assert.AreEqual("MODIFY", (string) participantPermissionObject["accessRight"]);
            Assert.AreEqual("EngineeringModel", (string) participantPermissionObject["objectClass"]);
        }
    }
}