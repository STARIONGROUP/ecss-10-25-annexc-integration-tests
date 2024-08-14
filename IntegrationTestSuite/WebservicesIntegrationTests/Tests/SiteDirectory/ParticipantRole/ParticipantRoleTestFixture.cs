// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParticipantRoleTestFixture.cs" company="Starion Group S.A.">
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
    public class ParticipantRoleTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParticipantRoleIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var participantRolesUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/participantRole");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(participantRolesUri);

            //check if there is only one ParticipantRole object
            Assert.AreEqual(1, jArray.Count);

            // get a specific ParticipantRole from the result by it's unique id
            var participantRole = jArray.Single(x => (string) x["iid"] == "ee3ae5ff-ac5e-4957-bab1-7698fba2a267");

            ParticipantRoleTestFixture.VerifyProperties(participantRole);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParticipantRoleWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var participantRolesUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/participantRole?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(participantRolesUri);

            //check if there are only two objects
            Assert.AreEqual(2, jArray.Count);

            var siteDirectory = jArray.Single(x => (string) x["iid"] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific ParticipantRole from the result by it's unique id
            var participantRole = jArray.Single(x => (string) x["iid"] == "ee3ae5ff-ac5e-4957-bab1-7698fba2a267");
            ParticipantRoleTestFixture.VerifyProperties(participantRole);
        }

        /// <summary>
        /// Verifies all properties of the ParticipantRole <see cref="JToken"/>
        /// </summary>
        /// <param name="participantRole">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ParticipantRole object
        /// </param>
        public static void VerifyProperties(JToken participantRole)
        {
            // verify the amount of returned properties 
            Assert.AreEqual(10, participantRole.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("ee3ae5ff-ac5e-4957-bab1-7698fba2a267", (string) participantRole["iid"]);
            Assert.AreEqual(1, (int) participantRole["revisionNumber"]);
            Assert.AreEqual("ParticipantRole", (string) participantRole["classKind"]);
            
            Assert.AreEqual("Test Participant Role", (string) participantRole["name"]);
            Assert.AreEqual("TestParticipantRole", (string) participantRole["shortName"]);
            Assert.IsFalse((bool) participantRole["isDeprecated"]);

            var expectedAliases = new string[] {};
            var aliasesArray = (JArray) participantRole["alias"];
            IList<string> aliases = aliasesArray.Select(x => (string) x).ToList();
            Assert.That(aliases, Is.EquivalentTo(expectedAliases));

            var expectedDefinitions = new string[] {};
            var definitionsArray = (JArray) participantRole["definition"];
            IList<string> definitions = definitionsArray.Select(x => (string) x).ToList();
            Assert.That(definitions, Is.EquivalentTo(expectedDefinitions));

            var expectedHyperlinks = new string[] {};
            var hyperlinksArray = (JArray) participantRole["hyperLink"];
            IList<string> h = hyperlinksArray.Select(x => (string) x).ToList();
            Assert.That(h, Is.EquivalentTo(expectedHyperlinks));

            var expectedParticipantPermissions = new string[]
            {
                "ac6fe12e-fdaa-4b94-9822-312d0897f5cb",
                "ee7a7848-90d5-4914-b258-4a00c0543db0",
                "af6526f8-66f4-4a0b-9c49-d9ae00e97108",
                "0145d47c-2971-4f89-9dc8-2c5e0453805e",
                "39a2a62a-655d-45c1-8b6f-6bad372f935b",
                "939bdada-f827-4f27-8761-ba78918431dd",
                "4ac0b072-8d58-46e0-8073-a48013c4c5be",
                "c8656ec9-3377-4ea6-9d20-825e8f6fe335",
                "a18e23ea-77a1-481f-ad4a-b23a9022f93f",
                "b380b256-8938-4dab-98c5-554cb619e81b",
                "8fdfec18-72d7-4e14-bcd1-5298f7594333",
                "c189f7cb-056e-4659-affa-b3ff4f6ba881",
                "df41e148-e228-4f19-b15a-2b68b75891e9",
                "8460a98f-6958-4caa-aa13-47b350e4bea9",
                "72cd93f4-e8f7-446b-a369-838b5a1ecc57",
                "b9eefd51-9da8-4f3f-b67c-8259ebad3b72",
                "7f8f6f00-1a3d-412c-a276-747306a4f961",
                "f0bb5b06-b7b1-4e67-ae02-c71d73e71d00",
                "bd276630-1d3f-45d0-9aa5-0efb9accbadb",
                "c3cf7ff9-4a4f-44a4-b91f-9918a12f664b",
                "dbe6a4b9-3b14-4580-8d3a-33ddf325c651",
                "bc7a0939-d6ee-4a08-9bb5-c350755dcd0d",
                "1bebf97d-114a-4baa-ba61-0f01eeb7e1ad",
                "8f3627a8-7782-4b1c-8677-e546db6965a9"
            };
            var participantPermissionsArray = (JArray) participantRole["participantPermission"];
            IList<string> participantPermissions = participantPermissionsArray.Select(x => (string) x).ToList();
            Assert.That(participantPermissions, Is.EquivalentTo(expectedParticipantPermissions));
        }
    }
}
