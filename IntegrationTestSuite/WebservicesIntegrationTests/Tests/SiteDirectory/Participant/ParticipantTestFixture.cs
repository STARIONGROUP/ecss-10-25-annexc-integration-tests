// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParticipantTestFixture.cs" company="Starion Group S.A.">
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
    using System.Net;

    using NUnit.Framework;

    [TestFixture]
    public class ParticipantTestFixture: WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("POST")]
        public void VerifyThatParticipantHasValidDomainOfExpertise()
        {
            // define the URI on which to perform a GET request
            var participantUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            
            var postBodyPath = this.GetPath("Tests/SiteDirectory/Participant/Post_Participant_With_Null_Domain.json");

            var postBody = this.GetJsonFromFile(postBodyPath);

            var exception = Assert.Catch<WebException>(() => this.WebClient.PostDto(participantUri, postBody));
            var errorMessage = this.WebClient.ExtractExceptionStringFromResponse(exception.Response);
            Assert.That(((HttpWebResponse)exception.Response).StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            Assert.IsTrue(errorMessage.Contains("Participant selected domain must be contained in participant domain list."));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatPersonThatIsAlreadyAParticipantInAnEngineeringModelCannotBeAdded()
        {
            // define the URI on which to perform a GET request
            var participantUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");

            var postBodyPath = this.GetPath("Tests/SiteDirectory/Participant/Create_Existing_Participant.json");

            var postBody = this.GetJsonFromFile(postBodyPath);

            var exception = Assert.Catch<WebException>(() => this.WebClient.PostDto(participantUri, postBody));
            var errorMessage = this.WebClient.ExtractExceptionStringFromResponse(exception.Response);
            Assert.That(((HttpWebResponse)exception.Response).StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.IsTrue(errorMessage.Contains("is already a Participant in EngineeringModel"));
        }
    }
}
