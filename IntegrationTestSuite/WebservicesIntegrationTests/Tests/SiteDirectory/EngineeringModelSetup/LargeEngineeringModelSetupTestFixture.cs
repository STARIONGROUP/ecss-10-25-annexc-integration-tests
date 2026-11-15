// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LargeEngineeringModelSetupTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2026 Starion Group S.A.
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
    using System.IO.Compression;
    using System.Linq;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using WebservicesIntegrationTests.Net;

    /// <summary>
    /// Verifies that an EngineeringModel with more things of one ClassKind than fit in a single SQL
    /// statement (PostgreSQL allows 65535 bind parameters) can be seeded, restored and used as the
    /// source of a new model.
    /// </summary>
    /// <remarks>
    /// LargeModel.zip is built from the Data folder by IntegrationTestSuite/Tools/MakeLargeModel.cs; rebuild it
    /// when the Data folder changes.
    /// The fixture seeds LargeModel.zip once, which also makes it the data-set that /Data/Restore restores, and
    /// every test starts with a restore of it. The one time tear down seeds the regular Data folder again, so that
    /// the tests that run afterwards start from the usual state.
    /// </remarks>
    [TestFixture]
    [Category("LargeModel")]
    public class LargeEngineeringModelSetupTestFixture : WebClientTestFixtureBase
    {
        /// <summary>
        /// The number of ElementDefinitions in the iteration of LargeModel.zip: the 2 of the regular
        /// data plus the 25000 that IntegrationTestSuite/Tools/MakeLargeModel.cs adds
        /// </summary>
        private const int ElementDefinitionCount = 25002;

        private const string SiteDirectoryIid = "f13de6f8-b03a-46e7-a492-53b2f260f294";

        private const string SourceModelSetupIid = "116f6253-89bb-47d4-aa24-d11d197e43c9";

        private const string SourceModelIid = "9ec982e4-ef72-4953-aa85-b158a95d8d56";

        private const string SourceIterationIid = "e163c5ad-f32b-4387-b805-f4b34600bc2c";

        private const string NewModelSetupIid = "a54467e2-5cb4-450b-a081-1e2f8a6dcd80";

        private const string NewModelIid = "ae1eca97-77bd-4d0b-a17f-fc9fb76b04bd";

        [OneTimeSetUp]
        public void SeedLargeModel()
        {
            this.LoadSettings();

            var largeModel = File.ReadAllBytes(this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/LargeModel/LargeModel.zip"));

            try
            {
                this.CreateSeedClient().Seed(this.Settings.Hostname, largeModel).GetAwaiter().GetResult();
            }
            catch
            {
                // NUnit does not guarantee the one time tear down after a failed one time set up;
                // put the regular data back here, so the tests that run afterwards are not affected
                this.SeedRegularData();
                throw;
            }
        }

        [OneTimeTearDown]
        public void SeedRegularData()
        {
            this.CreateSeedClient().Seed(this.Settings.Hostname, CreateArchive(this.GetPath("Data"))).GetAwaiter().GetResult();
        }

        public override void SetUp()
        {
            base.SetUp();

            // every test starts from the seeded large model, which also verifies that it can be restored
            this.WebClient.Restore(this.Settings.Hostname);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatNewEngineeringModelCanBeCreatedBasedOnALargeExistingModelWithWebApi()
        {
            Assert.That(this.GetElementDefinitions(SourceModelIid, SourceIterationIid), Has.Count.EqualTo(ElementDefinitionCount));

            var newIterationIid = this.CopySourceModel();
            var newElements = this.GetElementDefinitions(NewModelIid, newIterationIid);

            Assert.Multiple(() =>
            {
                Assert.That(newElements, Has.Count.EqualTo(ElementDefinitionCount));
                Assert.That(newElements.Select(x => (string)x[PropertyNames.Iid]).Distinct().Count(), Is.EqualTo(ElementDefinitionCount));
                Assert.That(newElements.Any(x => (string)x[PropertyNames.ShortName] == "LargeModelElement0"), Is.True);
                Assert.That(newElements.Any(x => (string)x[PropertyNames.ShortName] == "LargeModelElement24999"), Is.True);
            });
        }

        [Test]
        [Category("POST")]
        public void VerifyThatALargeModelCanBeRestoredWithWebApi()
        {
            // change the data store, so that the restore has something to undo
            this.CopySourceModel();
            Assert.That(this.GetModelSetupIids(), Is.EquivalentTo(new[] { SourceModelSetupIid, NewModelSetupIid }));

            this.WebClient.Restore(this.Settings.Hostname);

            var sourceElements = this.GetElementDefinitions(SourceModelIid, SourceIterationIid);

            Assert.Multiple(() =>
            {
                Assert.That(this.GetModelSetupIids(), Is.EquivalentTo(new[] { SourceModelSetupIid }));
                Assert.That(sourceElements, Has.Count.EqualTo(ElementDefinitionCount));
                Assert.That(sourceElements.Any(x => (string)x[PropertyNames.ShortName] == "LargeModelElement24999"), Is.True);
            });
        }

        /// <summary>
        /// Creates a new EngineeringModel with the large model as its source
        /// </summary>
        /// <returns>The iid of the iteration of the new model</returns>
        private string CopySourceModel()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/{SiteDirectoryIid}");
            var postBody = this.GetJsonFromFile(this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetupBasedOnExistingModel.json"));

            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            var iterationSetup = jArray.Single(x => (string)x[PropertyNames.ClassKind] == "IterationSetup");
            return (string)iterationSetup[PropertyNames.IterationIid];
        }

        /// <summary>
        /// Gets the ElementDefinitions of an iteration
        /// </summary>
        /// <param name="modelIid">The iid of the EngineeringModel</param>
        /// <param name="iterationIid">The iid of the Iteration</param>
        /// <returns>The ElementDefinitions</returns>
        private List<JToken> GetElementDefinitions(string modelIid, string iterationIid)
        {
            var uri = new Uri($"{this.Settings.Hostname}/EngineeringModel/{modelIid}/iteration/{iterationIid}/element");
            return this.WebClient.GetDto(uri).Where(x => (string)x[PropertyNames.ClassKind] == "ElementDefinition").ToList();
        }

        /// <summary>
        /// Gets the iids of the EngineeringModelSetups in the SiteDirectory
        /// </summary>
        /// <returns>The iids</returns>
        private string[] GetModelSetupIids()
        {
            var uri = new Uri($"{this.Settings.Hostname}/SiteDirectory/{SiteDirectoryIid}");
            var siteDirectory = this.WebClient.GetDto(uri).Single(x => (string)x[PropertyNames.Iid] == SiteDirectoryIid);
            return ((JArray)siteDirectory[PropertyNames.Model]).Select(x => (string)x).ToArray();
        }

        /// <summary>
        /// Creates a <see cref="WebClient"/> for the one time set up and tear down, which run outside of the per test set up
        /// </summary>
        /// <returns>The <see cref="WebClient"/></returns>
        private WebClient CreateSeedClient()
        {
            return new WebClient(this.Settings.Username, this.Settings.Password);
        }

        /// <summary>
        /// Creates an Annex C.3 archive from a folder, the same way the CI workflows zip the Data folder
        /// </summary>
        /// <param name="folder">The folder to archive</param>
        /// <returns>The content of the archive</returns>
        private static byte[] CreateArchive(string folder)
        {
            using (var stream = new MemoryStream())
            {
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in Directory.GetFiles(folder, "*", SearchOption.AllDirectories))
                    {
                        // Annex C.3 entries must use forward slashes
                        var entryName = Path.GetRelativePath(folder, file).Replace(Path.DirectorySeparatorChar, '/');
                        archive.CreateEntryFromFile(file, entryName);
                    }
                }

                return stream.ToArray();
            }
        }
    }
}
