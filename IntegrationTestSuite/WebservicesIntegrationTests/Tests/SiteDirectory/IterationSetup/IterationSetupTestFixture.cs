// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivedQuantityKindTestFixture.cs" company="RHEA System S.A.">
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
    using System.Net;

    using NUnit.Framework;

    [TestFixture]
    public class IterationSetupTestFixture : WebClientTestFixtureBase
    {
        [Test]
        [Category("POST")]
        public void VerifyCyclicSelf()
        {
            var uri = new Uri(string.Format(UriFormat, this.Settings.Hostname,
                "/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294"));
            var postBodyPath = this.GetPath("Tests/SiteDirectory/IterationSetup/PostUpdateCyclicIterationSetup.json");

            var postBody = base.GetJsonFromFile(postBodyPath);

            // no additional error details are available here
            Assert.Throws<WebException>(() => this.WebClient.PostDto(uri, postBody));
        }
    }
}
