// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParticipantTestFixture.cs" company="RHEA System">
//
//   Copyright 2020 RHEA System 
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
    using NUnit.Framework;
    using System.Net;

    [TestFixture]
    public class ParticipantTestFixture: WebClientTestFixtureBase
    {
        /// <summary>
        /// Verification that the Participant must have a domain of expertise upon update
        /// </summary>
        [Test]
        public void VerifyThatParticipantHasValidDomainOfExpertise()
        {
            // define the URI on which to perform a GET request
            var participantUri =
                new Uri(string.Format(UriFormat, this.Settings.Hostname,
                    "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));

            var postBodyPath = this.GetPath("Tests/SiteDirectory/Participant/Post_Participant_With_Null_Domain.json");

            var postBody = this.GetJsonFromFile(postBodyPath);

            var exception = Assert.Catch<WebException>(() => this.WebClient.PostDto(participantUri, postBody));
            var errorMessage = this.WebClient.ExtractExceptionStringFromResponse(exception.Response);
            Assert.AreEqual(HttpStatusCode.Forbidden, ((HttpWebResponse)exception.Response).StatusCode);
            Assert.IsTrue(errorMessage.Contains("Participant selected domain must be contained in participant domain list."));
        }
    }
}
