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
    using System.IO;
    using System.IO.Compression;
    using System.Linq;

    using NUnit.Framework;

    /// <summary>
    /// Verifies that an EngineeringModel with more things of one ClassKind than fit in a single SQL
    /// statement (PostgreSQL allows 65535 bind parameters) can be used as the source of a new model.
    /// </summary>
    /// <remarks>
    /// LargeModel.zip is built from the Data folder by IntegrationTestSuite/Tools/MakeLargeModel.cs; rebuild it
    /// when the Data folder changes.
    /// The test seeds LargeModel.zip, which also replaces the data-set that /Data/Restore restores.
    /// The tear down therefore seeds the regular Data folder again, so that the tests that run
    /// afterwards start from the usual state.
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

        private const string SourceModelIid = "9ec982e4-ef72-4953-aa85-b158a95d8d56";

        private const string SourceIterationIid = "e163c5ad-f32b-4387-b805-f4b34600bc2c";

        private const string NewModelIid = "ae1eca97-77bd-4d0b-a17f-fc9fb76b04bd";

        public override void SetUp()
        {
            base.SetUp();

            var largeModel = File.ReadAllBytes(this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/LargeModel/LargeModel.zip"));
            this.WebClient.Seed(this.Settings.Hostname, largeModel).GetAwaiter().GetResult();
        }

        public override void TearDown()
        {
            this.WebClient.Seed(this.Settings.Hostname, CreateArchive(this.GetPath("Data"))).GetAwaiter().GetResult();

            base.TearDown();
        }

        [Test]
        [Category("POST")]
        public void VerifyThatNewEngineeringModelCanBeCreatedBasedOnALargeExistingModelWithWebApi()
        {
            var sourceElementsUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/{SourceModelIid}/iteration/{SourceIterationIid}/element");
            Assert.That(this.WebClient.GetDto(sourceElementsUri).Count(x => (string)x[PropertyNames.ClassKind] == "ElementDefinition"), Is.EqualTo(ElementDefinitionCount));

            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBody = this.GetJsonFromFile(this.GetPath("Tests/SiteDirectory/EngineeringModelSetup/PostEngineeringModelSetupBasedOnExistingModel.json"));

            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            var iterationSetup = jArray.Single(x => (string)x[PropertyNames.ClassKind] == "IterationSetup");
            var newIterationIid = (string)iterationSetup[PropertyNames.IterationIid];

            var newElementsUri = new Uri($"{this.Settings.Hostname}/EngineeringModel/{NewModelIid}/iteration/{newIterationIid}/element");
            var newElements = this.WebClient.GetDto(newElementsUri).Where(x => (string)x[PropertyNames.ClassKind] == "ElementDefinition").ToList();

            Assert.Multiple(() =>
            {
                Assert.That(newElements, Has.Count.EqualTo(ElementDefinitionCount));
                Assert.That(newElements.Select(x => (string)x[PropertyNames.Iid]).Distinct().Count(), Is.EqualTo(ElementDefinitionCount));
                Assert.That(newElements.Any(x => (string)x[PropertyNames.ShortName] == "LargeModelElement0"), Is.True);
                Assert.That(newElements.Any(x => (string)x[PropertyNames.ShortName] == "LargeModelElement24999"), Is.True);
            });
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
