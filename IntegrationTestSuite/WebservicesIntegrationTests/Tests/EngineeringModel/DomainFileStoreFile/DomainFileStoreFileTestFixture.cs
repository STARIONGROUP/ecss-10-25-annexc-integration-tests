// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainFileStoreFileTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2023 Starion Group S.A.
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
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    using ICSharpCode.SharpZipLib.Zip;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NLog.LayoutRenderers;

    using NUnit.Framework;

    [TestFixture]
    public class DomainFileStoreFileTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedFileIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var fileUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/file");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(fileUri);

            //check if there is the only one File object 
            Assert.That(jArray.Count, Is.EqualTo(1));

            // get a specific File from the result by it's unique id
            var file = jArray.Single(x => (string) x[PropertyNames.Iid] == "95bf0f17-1273-4338-98ae-839016242775");

            VerifyProperties(file);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedFileWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var fileUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/file?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArray = this.WebClient.GetDto(fileUri);

            //check if there are 4 objects
            Assert.That(jArray.Count, Is.EqualTo(4));

            // get a specific Iteration from the result by it's unique id
            var iteration = jArray.Single(x => (string) x[PropertyNames.Iid] == "e163c5ad-f32b-4387-b805-f4b34600bc2c");

            IterationTestFixture.VerifyProperties(iteration);

            // get a specific DomainFileStore from the result by it's unique id
            var domainFileStore = jArray.Single(x => (string) x[PropertyNames.Iid] == "da7dddaa-02aa-4897-9935-e8d66c811a96");

            DomainFileStoreTestFixture.VerifyProperties(domainFileStore);

            // get a specific File from the result by it's unique id
            var file = jArray.Single(x => (string) x[PropertyNames.Iid] == "95bf0f17-1273-4338-98ae-839016242775");

            VerifyProperties(file);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatFileAndSubsequentRevisionCannotBeUploadedWithWebApiWhenParticipantIsNotAnOwner()
        {
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postJsonPath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/PostNewFile.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/2990BA2444A937A28E7B1E2465FCDF949B8F5368");

            // Jane is not allowed to upload
            Assert.ThrowsAsync<HttpRequestException>(async () => await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath));
        }

        [Test]
        [Category("POST")]
        public async Task VerifyThatFileAndSubsequentRevisionCanBeDeletedByOwner()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postJsonPath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/PostNewFile.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/2990BA2444A937A28E7B1E2465FCDF949B8F5368");

            var jArray = await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath);

            // check if there is a correct amount of objects
            Assert.That(jArray.Count, Is.EqualTo(4));

            var fileUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postJsonPath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/PostDeleteFile.json");
            var postBody = this.GetJsonFromFile(postJsonPath);
            jArray = this.WebClient.PostDto(fileUri, postBody);

            // check if there is a correct amount of objects
            Assert.That(jArray.Count, Is.EqualTo(2));

            // get a specific EngineeringModel from the result by it's unique id
            var engineeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            // get a specific DomainFileStore from the result by it's unique id
            var domainFileStore = jArray.Single(x => (string) x[PropertyNames.Iid] == "da7dddaa-02aa-4897-9935-e8d66c811a96");

            var possibleFiles = domainFileStore[PropertyNames.File].Select(x => (string) x).ToList();
            var expectedFiles = new[] { "95bf0f17-1273-4338-98ae-839016242775" };

            Assert.That(possibleFiles, Is.EqualTo(expectedFiles) );
        }

        [Test]
        [Category("POST")]
        public async Task VerifyThatFileAndSubsequentRevisionCannotBeDeletedWhenUserIsNotTheOwner()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postJsonPath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/PostNewFile.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/2990BA2444A937A28E7B1E2465FCDF949B8F5368");

            //Add file as admin
            var jArray = await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath);

            // check if there is a correct amount of objects
            Assert.That(jArray.Count, Is.EqualTo(4));

            //Change user to Jane
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            var fileUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postJsonPath = this.GetPath("Tests/EngineeringModel/CommonFileStoreFile/PostDeleteFile.json");
            var postBody = this.GetJsonFromFile(postJsonPath);

            // Jane is not allowed to delete
            Assert.That(() => this.WebClient.PostDto(fileUri, postBody), Throws.TypeOf<WebException>());
        }

        [Test]
        [Category("POST")]
        public async Task VerifyThatFileAndSubsequentRevisionCanBeUploadedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postJsonPath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/PostNewFile.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/2990BA2444A937A28E7B1E2465FCDF949B8F5368");

            var jArray = await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath);

            // check if there is a correct amount of objects
            Assert.That(jArray.Count, Is.EqualTo(4));

            // get a specific EngineeeringModel from the result by it's unique id
            var engineeeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int) engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // get a specific domainFileStore from the result by it's unique id
            var domainFileStore = jArray.Single(x => (string) x[PropertyNames.Iid] == "da7dddaa-02aa-4897-9935-e8d66c811a96");

            Assert.That((int) domainFileStore[PropertyNames.RevisionNumber], Is.EqualTo(2));

            // get a specific File from the result by it's unique id
            var file =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "8ac6db3e-9525-4f3e-93ea-707076c07fc1");

            Assert.That((int) file[PropertyNames.RevisionNumber], Is.EqualTo(2));
            Assert.That((string) file[PropertyNames.ClassKind], Is.EqualTo("File"));

            Assert.That((string) file[PropertyNames.LockedBy], Is.Null);
            Assert.That((string)file[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            var expectedCategories = new string[] { };
            var categoriesArray = (JArray) file[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            Assert.That(categories, Is.EquivalentTo(expectedCategories));

            var expectedFileRevisions = new string[]
            {
                "76e9b7fc-edc4-4ca3-89ba-eac014e7d9f8"
            };

            var fileRevisionsArray = (JArray) file[PropertyNames.FileRevision];
            IList<string> fileRevisions = fileRevisionsArray.Select(x => (string) x).ToList();
            Assert.That(fileRevisions, Is.EquivalentTo(expectedFileRevisions));

            // get a specific FileRevision from the result by it's unique id
            var fileRevision =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "76e9b7fc-edc4-4ca3-89ba-eac014e7d9f8");

            Assert.That( (int) fileRevision[PropertyNames.RevisionNumber], Is.EqualTo(2));
                            Assert.That((string)fileRevision[PropertyNames.ClassKind], Is.EqualTo("FileRevision"));
            Assert.That((string)fileRevision[PropertyNames.Name], Is.EqualTo("FileTest"));

            Assert.IsNull((string) fileRevision[PropertyNames.ContainingFolder]);
            Assert.That((string)fileRevision[PropertyNames.Creator], Is.EqualTo("284334dd-e8e5-42d6-bc8a-715c507a7f02"));
            Assert.That((string)fileRevision[PropertyNames.ContentHash], Is.EqualTo("2990BA2444A937A28E7B1E2465FCDF949B8F5368"));

            var expectedFileTypes = new List<OrderedItem>
            {
                new OrderedItem(3177, "b16894e4-acb5-4e81-a118-16c00eb86d8f")
            };

            var fileTypesArray = JsonConvert.DeserializeObject<List<OrderedItem>>(fileRevision[PropertyNames.FileType].ToString());

            Assert.That(fileTypesArray, Is.EqualTo(expectedFileTypes));

            // Subsequent revision
            var fileUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postJsonPath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/PostNewFileRevision.json");
            postFilePath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/1525ED651E5B609DAE099DEEDA8DBDB49CFF956F");

            jArray = await this.WebClient.PostFile(fileUri, postJsonPath, postFilePath);

            // get a specific EngineeeringModel from the result by it's unique id
            engineeeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.That((int)engineeeringModel[PropertyNames.RevisionNumber], Is.EqualTo(3));

            // check if there is a correct amount of objects
            Assert.That(jArray.Count, Is.EqualTo(3));

            // get a specific File from the result by it's unique id
            file = jArray.Single(x => (string) x[PropertyNames.Iid] == "95bf0f17-1273-4338-98ae-839016242775");

            Assert.That((int)file[PropertyNames.RevisionNumber], Is.EqualTo(3));

            expectedFileRevisions = new string[]
            {
                "5544bb87-dc38-45d5-9d92-c580d3fe0442",
                "1304d40a-cb2e-4608-a353-cb7f65000559"
            };

            fileRevisionsArray = (JArray) file[PropertyNames.FileRevision];
            fileRevisions = fileRevisionsArray.Select(x => (string) x).ToList();
            Assert.That(fileRevisions, Is.EquivalentTo(expectedFileRevisions));
            
            // get a specific FileRevision from the result by it's unique id
            fileRevision = jArray.Single(x => (string) x[PropertyNames.Iid] == "1304d40a-cb2e-4608-a353-cb7f65000559");

            Assert.That((int)fileRevision[PropertyNames.RevisionNumber], Is.EqualTo(3));
            Assert.That((string)fileRevision[PropertyNames.ClassKind], Is.EqualTo("FileRevision"));
            Assert.That((string)fileRevision[PropertyNames.Name], Is.EqualTo("Revision 2_1525ED651E5B609DAE099DEEDA8DBDB49CFF956F"));

            Assert.IsNull((string) fileRevision[PropertyNames.ContainingFolder]);
            Assert.That((string)fileRevision[PropertyNames.Creator], Is.EqualTo("284334dd-e8e5-42d6-bc8a-715c507a7f02"));
            Assert.That((string)fileRevision[PropertyNames.ContentHash], Is.EqualTo("1525ED651E5B609DAE099DEEDA8DBDB49CFF956F"));

            expectedFileTypes = new List<OrderedItem>
            {
                new OrderedItem(6608, "b16894e4-acb5-4e81-a118-16c00eb86d8f")
            };

            fileTypesArray = JsonConvert.DeserializeObject<List<OrderedItem>>(fileRevision[PropertyNames.FileType].ToString());

            Assert.That(expectedFileTypes, Is.EqualTo(fileTypesArray));
        }

        [Test]
        [Category("POST")]
        public void VerifyThatFileCannotBeUploadedWhenParticipantIsNotOwner()
        {
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            // Subsequent revision
            var fileUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/file/95bf0f17-1273-4338-98ae-839016242775");
            var postJsonPath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/PostNewFile.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/2990BA2444A937A28E7B1E2465FCDF949B8F5368");

            // Jane is not allowed to upload
            Assert.ThrowsAsync<HttpRequestException>(async () => await this.WebClient.PostFile(fileUri, postJsonPath, postFilePath));
        }

        [Test]
        [Category("POST")]
        public async Task VerifyThatAFileRevisionCanBeDownloadedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postJsonPath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/PostNewFile.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/2990BA2444A937A28E7B1E2465FCDF949B8F5368");
            await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath);

            var fileUri2 = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            postJsonPath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/PostNewFileBinaryRevision.json");
            postFilePath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/3F64667F0F27A4C4FA1B4BF374033938A542FDD1");
            await this.WebClient.PostFile(fileUri2, postJsonPath, postFilePath);

            // Download a revision of the plain text file
            var getFileUriForTxt = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/file/8ac6db3e-9525-4f3e-93ea-707076c07fc1/fileRevision/76e9b7fc-edc4-4ca3-89ba-eac014e7d9f8?includeFileData=true");
            var responseBodyForTxt = await this.WebClient.GetFileResponseBody(getFileUriForTxt);

            // Download a revision of the pdf file
            var getFileUriForPdf = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/file/8ac6db3e-9525-4f3e-93ea-707076c07fc1/fileRevision/e5b46d1b-7d51-4433-b515-25d7d37a0b50?includeFileData=true");
            var responseBodyForPdf = await this.WebClient.GetFileResponseBody(getFileUriForPdf);

            using (var sha1 = new SHA1Managed())
            {
                var hash = BitConverter.ToString(sha1.ComputeHash(responseBodyForTxt)).Replace("-", string.Empty);

                Assert.AreEqual("2990BA2444A937A28E7B1E2465FCDF949B8F5368", hash);
            }

            using (var sha1 = new SHA1Managed())
            {
                var hash = BitConverter.ToString(sha1.ComputeHash(responseBodyForPdf)).Replace("-", string.Empty);

                Assert.AreEqual("3F64667F0F27A4C4FA1B4BF374033938A542FDD1", hash);
            }
        }

        [Test]
        [Category("POST")]
        public async Task VerifyThatAFolderCanBeDownloadedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postJsonPath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/PostNewFolderWithFile.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/2990BA2444A937A28E7B1E2465FCDF949B8F5368");
            await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath);

            // Download a zip archive of the folder
            var getFileUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/folder/e80daca0-5c6e-4236-ae34-d23c36244059?includeFileData=true");
            var responseBody = await this.WebClient.GetFileResponseBody(getFileUri);

            var path = Path.GetTempFileName();
            File.WriteAllBytes(path, responseBody);
            var zipFile = new ICSharpCode.SharpZipLib.Zip.ZipFile(path);

            // folder items are not included as entries nor are empty folders
            Assert.That(zipFile.Count, Is.EqualTo(1));

            var expectedZipEntries = new string[]
            {
                "TestFolder/FileTest.txt"
            };

            var entries = new List<string>();

            foreach (ZipEntry entry in zipFile)
            {
                entries.Add(entry.Name);
            }

            Assert.That(entries, Is.EquivalentTo(expectedZipEntries));
        }

        [Test]
        [Category("POST")]
        public async Task VerifyThatAFileStoreCanBeDownloadedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            
            var postJsonPath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/PostNewFolderWithFile.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/2990BA2444A937A28E7B1E2465FCDF949B8F5368");
            var result = await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath);
            
            postJsonPath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/PostNewFileBinaryRevision.json");
            postFilePath = this.GetPath("Tests/EngineeringModel/DomainFileStoreFile/3F64667F0F27A4C4FA1B4BF374033938A542FDD1");
            result = await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath);

            // Download a zip archive of the folder
            var getFileUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96?includeFileData=true");
            var responseBody = await this.WebClient.GetFileResponseBody(getFileUri);

            var path = Path.GetTempFileName();
            File.WriteAllBytes(path, responseBody);
            var zipFile = new ICSharpCode.SharpZipLib.Zip.ZipFile(path);

            // folder items are not included as entries nor are empty folders
            Assert.That(zipFile.Count, Is.EqualTo(3));

            var expectedZipEntries = new string[]
            {
                "FileRevision.tst",
                "UserManual_3F64667F0F27A4C4FA1B4BF374033938A542FDD1.txt",
                "TestFolder/FileTest.txt"
            };

            var entries = new List<string>();
            foreach (ZipEntry entry in zipFile)
            {
                entries.Add(entry.Name);
            }

            Assert.That(entries, Is.EquivalentTo(expectedZipEntries));
        }

        /// <summary>
        /// Verifies all properties of the File <see cref="JToken"/>
        /// </summary>
        /// <param name="file">
        /// The <see cref="JToken"/> that contains the properties of
        /// the File object
        /// </param>
        public static void VerifyProperties(JToken file)
        {
            // verify the amount of returned properties 
            Assert.That(file.Children().Count(), Is.EqualTo(7));

            // assert that the properties are what is expected
            Assert.That((string)file[PropertyNames.Iid], Is.EqualTo("95bf0f17-1273-4338-98ae-839016242775"));
            Assert.That((int)file[PropertyNames.RevisionNumber], Is.EqualTo(1));
            Assert.That((string)file[PropertyNames.ClassKind], Is.EqualTo("File"));

            Assert.That((string) file[PropertyNames.LockedBy], Is.Null);
            Assert.That((string)file[PropertyNames.Owner], Is.EqualTo("0e92edde-fdff-41db-9b1d-f2e484f12535"));

            var expectedCategories = new string[] { };
            var categoriesArray = (JArray) file[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            Assert.That(categories, Is.EquivalentTo(expectedCategories));

            var expectedFileRevisions = new string[]
            {
                "5544bb87-dc38-45d5-9d92-c580d3fe0442"
            };

            var fileRevisionsArray = (JArray) file[PropertyNames.FileRevision];
            IList<string> fileRevisions = fileRevisionsArray.Select(x => (string) x).ToList();
            Assert.That(fileRevisions, Is.EquivalentTo(expectedFileRevisions));
        }
    }
}
