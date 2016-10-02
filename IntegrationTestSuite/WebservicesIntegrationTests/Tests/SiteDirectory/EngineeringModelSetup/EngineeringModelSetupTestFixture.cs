// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EngineeringModelSetupTestFixture.cs" company="RHEA System">
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
    using NUnit.Framework;
    using WebservicesIntegrationTests.Net;

    /// <summary>
    /// The purpose of the <see cref="EngineeringModelSetupTestFixture"/> is to GET and POST person objects
    /// </summary>
    [TestFixture]
    public class EngineeringModelSetupTestFixture : WebClientTestFixtureBase
    {
        [Test]
        public void VerifyThatNewEngineeringModelCanBeCreatedWithWebApi()
        {
            var siteDirectoryUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));
            var postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetup.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var response = this.WebClient.PostDto(siteDirectoryUri, postBody);

            Console.WriteLine(response);
        }

        [Test]
        public void VerifyThatNewEngineeringModelCanBeCreatedBasedOnExistingModelWithWebApi()
        {
            var siteDirectoryUri = new Uri(string.Format(UriFormat, this.Settings.Hostname, "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));
            var postBodyPath = this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetupBasedOnExistingModel.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var response = this.WebClient.PostDto(siteDirectoryUri, postBody);

            Console.WriteLine(response);
        }
    }
}
