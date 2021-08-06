// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttachmentTestFixture.cs" company="RHEA System S.A.">
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
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    using NUnit.Framework;

    [TestFixture]
    public class AttachmentTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("POST")]
        public async Task VerifyThatAnAttachmentCanBeDownloadedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postJsonPath = this.GetPath("Tests/EngineeringModel/Attachment/PostNewAttachment.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/Attachment/2990BA2444A937A28E7B1E2465FCDF949B8F5368");
            await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath);

            // Download a revision of the plain text file
            var getFileUriForTxt = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/element/f73860b2-12f0-43e4-b8b2-c81862c0a159/attachment/76e9b7fc-edc4-4ca3-89ba-eac014e7d9f9?includeFileData=true");
            var responseBodyForTxt = await this.WebClient.GetFileResponseBody(getFileUriForTxt);

            using (var sha1 = new SHA1Managed())
            {
                var hash = BitConverter.ToString(sha1.ComputeHash(responseBodyForTxt)).Replace("-", string.Empty);

                Assert.AreEqual("2990BA2444A937A28E7B1E2465FCDF949B8F5368", hash);
            }
        }
    }
}
