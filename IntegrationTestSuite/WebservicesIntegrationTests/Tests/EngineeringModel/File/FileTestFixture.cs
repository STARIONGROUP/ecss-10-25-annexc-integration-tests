// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileTestFixture.cs" company="RHEA System S.A.">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    using Ionic.Zip;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class FileTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
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
            Assert.AreEqual(1, jArray.Count);

            // get a specific File from the result by it's unique id
            var file =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "95bf0f17-1273-4338-98ae-839016242775");

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
            Assert.AreEqual(4, jArray.Count);

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
        public async Task VerifyThatFileAndSubsequentRevisionCannotBeUploadedWithWebApiWhenParticipantIsNotAnOwner()
        {
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postJsonPath = this.GetPath("Tests/EngineeringModel/File/PostNewFile.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/File/2990BA2444A937A28E7B1E2465FCDF949B8F5368");

            // Jane is not allowed to upload
            Assert.ThrowsAsync<HttpRequestException>(async () => await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath));
        }

        [Test]
        [Category("POST")]
        public async Task VerifyThatFileAndSubsequentRevisionCanBeDeletedByOwner()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postJsonPath = this.GetPath("Tests/EngineeringModel/File/PostNewFile.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/File/2990BA2444A937A28E7B1E2465FCDF949B8F5368");

            var jArray = await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath);

            // check if there is a correct amount of objects
            Assert.AreEqual(4, jArray.Count);

            var fileUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postJsonPath = this.GetPath("Tests/EngineeringModel/File/PostDeleteFile.json");
            var postBody = this.GetJsonFromFile(postJsonPath);
            jArray = this.WebClient.PostDto(fileUri, postBody);

            // check if there is a correct amount of objects
            Assert.AreEqual(2, jArray.Count);

            // get a specific EngineeringModel from the result by it's unique id
            var engineeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");

            // get a specific DomainFileStore from the result by it's unique id
            var domainFileStore = jArray.Single(x => (string) x[PropertyNames.Iid] == "da7dddaa-02aa-4897-9935-e8d66c811a96");

            var possibleFiles = domainFileStore[PropertyNames.File].Select(x => (string) x).ToList();
            var expectedFiles = new[] { "95bf0f17-1273-4338-98ae-839016242775" };

            CollectionAssert.AreEqual(expectedFiles, possibleFiles);
        }

        [Test]
        [Category("POST")]
        public async Task VerifyThatFileAndSubsequentRevisionCannotBeDeletedWhenUserIsNotTheOwner()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postJsonPath = this.GetPath("Tests/EngineeringModel/File/PostNewFile.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/File/2990BA2444A937A28E7B1E2465FCDF949B8F5368");

            //Add file as admin
            var jArray = await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath);

            // check if there is a correct amount of objects
            Assert.AreEqual(4, jArray.Count);

            //Change user to Jane
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            // Jane is not allowed to delete
            Assert.ThrowsAsync<HttpRequestException>(async () => await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath));
        }

        [Test]
        [Category("POST")]
        public async Task VerifyThatFileAndSubsequentRevisionCanBeUploadedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            var postJsonPath = this.GetPath("Tests/EngineeringModel/File/PostNewFile.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/File/2990BA2444A937A28E7B1E2465FCDF949B8F5368");

            var jArray = await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath);

            // check if there is a correct amount of objects
            Assert.AreEqual(4, jArray.Count);

            // get a specific EngineeeringModel from the result by it's unique id
            var engineeeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(2, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            // get a specific domainFileStore from the result by it's unique id
            var domainFileStore =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "da7dddaa-02aa-4897-9935-e8d66c811a96");

            Assert.AreEqual(2, (int) domainFileStore[PropertyNames.RevisionNumber]);

            // get a specific File from the result by it's unique id
            var file =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "8ac6db3e-9525-4f3e-93ea-707076c07fc1");

            Assert.AreEqual(2, (int) file[PropertyNames.RevisionNumber]);
            Assert.AreEqual("File", (string) file[PropertyNames.ClassKind]);

            Assert.IsNull((string) file[PropertyNames.LockedBy]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) file[PropertyNames.Owner]);

            var expectedCategories = new string[] { };
            var categoriesArray = (JArray) file[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedFileRevisions = new string[]
            {
                "76e9b7fc-edc4-4ca3-89ba-eac014e7d9f8"
            };

            var fileRevisionsArray = (JArray) file[PropertyNames.FileRevision];
            IList<string> fileRevisions = fileRevisionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedFileRevisions, fileRevisions);

            // get a specific FileRevision from the result by it's unique id
            var fileRevision =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "76e9b7fc-edc4-4ca3-89ba-eac014e7d9f8");

            Assert.AreEqual(2, (int) fileRevision[PropertyNames.RevisionNumber]);
            Assert.AreEqual("FileRevision", (string) fileRevision[PropertyNames.ClassKind]);
            Assert.AreEqual("FileTest", (string) fileRevision[PropertyNames.Name]);

            Assert.IsNull((string) fileRevision[PropertyNames.ContainingFolder]);
            Assert.AreEqual("284334dd-e8e5-42d6-bc8a-715c507a7f02", (string) fileRevision[PropertyNames.Creator]);
            Assert.AreEqual("2990BA2444A937A28E7B1E2465FCDF949B8F5368", (string) fileRevision[PropertyNames.ContentHash]);

            var expectedFileTypes = new List<OrderedItem>
            {
                new OrderedItem(3177, "b16894e4-acb5-4e81-a118-16c00eb86d8f")
            };

            var fileTypesArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                fileRevision[PropertyNames.FileType].ToString());

            CollectionAssert.AreEquivalent(expectedFileTypes, fileTypesArray);

            // Subsequent revision
            var fileUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            postJsonPath = this.GetPath("Tests/EngineeringModel/File/PostNewFileRevision.json");
            postFilePath = this.GetPath("Tests/EngineeringModel/File/1525ED651E5B609DAE099DEEDA8DBDB49CFF956F");

            jArray = await this.WebClient.PostFile(fileUri, postJsonPath, postFilePath);

            // get a specific EngineeeringModel from the result by it's unique id
            engineeeringModel = jArray.Single(x => (string) x[PropertyNames.Iid] == "9ec982e4-ef72-4953-aa85-b158a95d8d56");
            Assert.AreEqual(3, (int) engineeeringModel[PropertyNames.RevisionNumber]);

            // check if there is a correct amount of objects
            Assert.AreEqual(3, jArray.Count);

            // get a specific File from the result by it's unique id
            file = jArray.Single(x => (string) x[PropertyNames.Iid] == "95bf0f17-1273-4338-98ae-839016242775");

            Assert.AreEqual(3, (int) file[PropertyNames.RevisionNumber]);

            expectedFileRevisions = new string[]
            {
                "5544bb87-dc38-45d5-9d92-c580d3fe0442",
                "1304d40a-cb2e-4608-a353-cb7f65000559"
            };

            fileRevisionsArray = (JArray) file[PropertyNames.FileRevision];
            fileRevisions = fileRevisionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedFileRevisions, fileRevisions);

            // get a specific FileRevision from the result by it's unique id
            fileRevision =
                jArray.Single(x => (string) x[PropertyNames.Iid] == "1304d40a-cb2e-4608-a353-cb7f65000559");

            Assert.AreEqual(3, (int) fileRevision[PropertyNames.RevisionNumber]);
            Assert.AreEqual("FileRevision", (string) fileRevision[PropertyNames.ClassKind]);
            Assert.AreEqual("Revision 2_1525ED651E5B609DAE099DEEDA8DBDB49CFF956F", (string) fileRevision[PropertyNames.Name]);

            Assert.IsNull((string) fileRevision[PropertyNames.ContainingFolder]);
            Assert.AreEqual("284334dd-e8e5-42d6-bc8a-715c507a7f02", (string) fileRevision[PropertyNames.Creator]);
            Assert.AreEqual("1525ED651E5B609DAE099DEEDA8DBDB49CFF956F", (string) fileRevision[PropertyNames.ContentHash]);

            expectedFileTypes = new List<OrderedItem>
            {
                new OrderedItem(6608, "b16894e4-acb5-4e81-a118-16c00eb86d8f")
            };

            fileTypesArray = JsonConvert.DeserializeObject<List<OrderedItem>>(
                fileRevision[PropertyNames.FileType].ToString());

            CollectionAssert.AreEquivalent(expectedFileTypes, fileTypesArray);
        }

        [Test]
        [Category("POST")]
        public async Task VerifyThatFileCannotBeUploadedWhenParticipantIsNotOwner()
        {
            SiteDirectoryTestFixture.AddDomainExpertUserJane(this, out var userName, out var passWord);
            this.CreateNewWebClientForUser(userName, passWord);

            // Subsequent revision
            var fileUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/file/95bf0f17-1273-4338-98ae-839016242775");
            var postJsonPath = this.GetPath("Tests/EngineeringModel/File/PostNewFile.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/File/2990BA2444A937A28E7B1E2465FCDF949B8F5368");

            // Jane is not allowed to upload
            Assert.ThrowsAsync<HttpRequestException>(async () => await this.WebClient.PostFile(fileUri, postJsonPath, postFilePath));
        }

        [Test]
        [Category("POST")]
        public async Task VerifyThatAFileRevisionCanBeDownloadedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            var postJsonPath = this.GetPath("Tests/EngineeringModel/File/PostNewFile.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/File/2990BA2444A937A28E7B1E2465FCDF949B8F5368");
            await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath);

            var fileUri2 = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");

            postJsonPath = this.GetPath("Tests/EngineeringModel/File/PostNewFileBinaryRevision.json");
            postFilePath = this.GetPath("Tests/EngineeringModel/File/3F64667F0F27A4C4FA1B4BF374033938A542FDD1");
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
            var postJsonPath = this.GetPath("Tests/EngineeringModel/File/PostNewFolderWithFile.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/File/2990BA2444A937A28E7B1E2465FCDF949B8F5368");
            await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath);

            // Download a zip archive of the folder
            var getFileUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96/folder/e80daca0-5c6e-4236-ae34-d23c36244059?includeFileData=true");
            var responseBody = await this.WebClient.GetFileResponseBody(getFileUri);

            var path = Path.GetTempFileName();
            File.WriteAllBytes(path, responseBody);
            var zip = new ZipFile(path);

            // It is assumed that if some information is retrieved from the archive than it is not corrupted
            Assert.AreEqual(2, zip.Count);

            var expectedZipEntries = new string[]
            {
                "TestFolder/",
                "TestFolder/FileTest.txt"
            };

            CollectionAssert.AreEquivalent(expectedZipEntries, zip.EntryFileNames.ToArray());
        }

        [Test]
        [Category("POST")]
        public async Task VerifyThatAFileStoreCanBeDownloadedWithWebApi()
        {
            var iterationUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c");
            
            var postJsonPath = this.GetPath("Tests/EngineeringModel/File/PostNewFolderWithFile.json");
            var postFilePath = this.GetPath("Tests/EngineeringModel/File/2990BA2444A937A28E7B1E2465FCDF949B8F5368");
            await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath);
            
            postJsonPath = this.GetPath("Tests/EngineeringModel/File/PostNewFileBinaryRevision.json");
            postFilePath = this.GetPath("Tests/EngineeringModel/File/3F64667F0F27A4C4FA1B4BF374033938A542FDD1");
            await this.WebClient.PostFile(iterationUri, postJsonPath, postFilePath);

            // Download a zip archive of the folder
            var getFileUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/9ec982e4-ef72-4953-aa85-b158a95d8d56/iteration/e163c5ad-f32b-4387-b805-f4b34600bc2c/domainFileStore/da7dddaa-02aa-4897-9935-e8d66c811a96?includeFileData=true");
            var responseBody = await this.WebClient.GetFileResponseBody(getFileUri);

            var path = Path.GetTempFileName();
            File.WriteAllBytes(path, responseBody);
            var zip = new ZipFile(path);

            // It is assumed that if some information is retrieved from the archive that it is not corrupted
            Assert.AreEqual(5, zip.Count);

            var expectedZipEntries = new string[]
            {
                "FileRevision.tst",
                "UserManual_3F64667F0F27A4C4FA1B4BF374033938A542FDD1.txt",
                "Test Folder/",
                "TestFolder/",
                "TestFolder/FileTest.txt"
            };

            CollectionAssert.AreEquivalent(expectedZipEntries, zip.EntryFileNames.ToArray());
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
            Assert.AreEqual(7, file.Children().Count());

            // assert that the properties are what is expected
            Assert.AreEqual("95bf0f17-1273-4338-98ae-839016242775", (string) file[PropertyNames.Iid]);
            Assert.AreEqual(1, (int) file[PropertyNames.RevisionNumber]);
            Assert.AreEqual("File", (string) file[PropertyNames.ClassKind]);

            Assert.IsNull((string) file[PropertyNames.LockedBy]);
            Assert.AreEqual("0e92edde-fdff-41db-9b1d-f2e484f12535", (string) file[PropertyNames.Owner]);

            var expectedCategories = new string[] { };
            var categoriesArray = (JArray) file[PropertyNames.Category];
            IList<string> categories = categoriesArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedCategories, categories);

            var expectedFileRevisions = new string[]
            {
                "5544bb87-dc38-45d5-9d92-c580d3fe0442"
            };

            var fileRevisionsArray = (JArray) file[PropertyNames.FileRevision];
            IList<string> fileRevisions = fileRevisionsArray.Select(x => (string) x).ToList();
            CollectionAssert.AreEquivalent(expectedFileRevisions, fileRevisions);
        }
    }
}
