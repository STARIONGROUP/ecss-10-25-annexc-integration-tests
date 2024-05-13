// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportTestFixture.cs" company="Starion Group S.A.">
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
    using System.IO;
    using System.Threading.Tasks;

    using ICSharpCode.SharpZipLib.Zip;

    using NUnit.Framework;

    [TestFixture]
    public class ExportTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("EXPORT")]
        public async Task Verify_That_EngineeringModel_and_files_Can_Be_Exported_WithWebApi()
        {
            string[] engineeringodelSetupIds = { "116f6253-89bb-47d4-aa24-d11d197e43c9" };

            // define the URI on which to perform a POST request
            var exportUri = new Uri($"{this.Settings.Hostname}/export?includeFileData=true");

            // Download a model export zip file
            // This fails on a Nancy COMET Webservice
            var responseBody = await this.WebClient.GetModelExportFile(exportUri, engineeringodelSetupIds);

            var path = Path.GetTempFileName();
            await File.WriteAllBytesAsync(path, responseBody);
            var zipFile = new ZipFile(path);

            // It is assumed that if some information is retrieved from the archive than it is not corrupted
            Assert.That(zipFile.Count, Is.EqualTo(8));

            var expectedZipEntries = new string[]
            {
                "Header.json",
                "SiteDirectory.json",
                "SiteReferenceDataLibraries/c454c687-ba3e-44c4-86bc-44544b2c7880.json",
                "ModelReferenceDataLibraries/3483f2b5-ea29-45cc-8a46-f5f598558fc3.json",
                "EngineeringModels/9ec982e4-ef72-4953-aa85-b158a95d8d56/9ec982e4-ef72-4953-aa85-b158a95d8d56.json",
                "EngineeringModels/9ec982e4-ef72-4953-aa85-b158a95d8d56/Iterations/e163c5ad-f32b-4387-b805-f4b34600bc2c.json",
                "EngineeringModels/9ec982e4-ef72-4953-aa85-b158a95d8d56/FileRevisions/",
                "EngineeringModels/9ec982e4-ef72-4953-aa85-b158a95d8d56/FileRevisions/B95EC201AE3EE89D407449D692E69BB97C228A7E"
            };

            var entries = new List<string>();

            foreach (ZipEntry entry in zipFile)
            {
                entries.Add(entry.Name);
            }

            Assert.That(entries, Is.EquivalentTo(expectedZipEntries));
        }

        [Test]
        [Category("EXPORT")]
        public async Task Verify_That_EngineeringModel_excluding_files_Can_Be_Exported_WithWebApi()
        {
            string[] engineeringodelSetupIds = { "116f6253-89bb-47d4-aa24-d11d197e43c9" };

            // define the URI on which to perform a POST request
            var exportUri = new Uri($"{this.Settings.Hostname}/export");

            // Download a model export zip file
            // This fails on a Nancy COMET Webservice
            var responseBody = await this.WebClient.GetModelExportFile(exportUri, engineeringodelSetupIds);

            var path = Path.GetTempFileName();
            await File.WriteAllBytesAsync(path, responseBody);
            var zipFile = new ZipFile(path);

            // It is assumed that if some information is retrieved from the archive than it is not corrupted
            Assert.That(zipFile.Count, Is.EqualTo(7));

            var expectedZipEntries = new string[]
            {
                "Header.json",
                "SiteDirectory.json",
                "SiteReferenceDataLibraries/c454c687-ba3e-44c4-86bc-44544b2c7880.json",
                "ModelReferenceDataLibraries/3483f2b5-ea29-45cc-8a46-f5f598558fc3.json",
                "EngineeringModels/9ec982e4-ef72-4953-aa85-b158a95d8d56/9ec982e4-ef72-4953-aa85-b158a95d8d56.json",
                "EngineeringModels/9ec982e4-ef72-4953-aa85-b158a95d8d56/Iterations/e163c5ad-f32b-4387-b805-f4b34600bc2c.json",
                "EngineeringModels/9ec982e4-ef72-4953-aa85-b158a95d8d56/FileRevisions/"
            };

            var entries = new List<string>();

            foreach (ZipEntry entry in zipFile)
            {
                entries.Add(entry.Name);
            }

            Assert.That(entries, Is.EquivalentTo(expectedZipEntries));
        }
    }
}
