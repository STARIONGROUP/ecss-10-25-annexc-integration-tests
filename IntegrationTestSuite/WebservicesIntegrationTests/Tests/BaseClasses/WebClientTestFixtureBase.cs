// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebClientTestFixtureBase.cs" company="RHEA S.A.">
//   Copyright (c) 2016 RHEA S.A.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebservicesIntegrationTests
{
    using System.IO;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using WebservicesIntegrationTests.Net;

    /// <summary>
    /// The web client test fixture base class.
    /// </summary>
    [TestFixture]
    public abstract class WebClientTestFixtureBase
    {
        /// <summary>
        /// The uri format.
        /// </summary>
        protected const string UriFormat = "{0}{1}";
        
        [SetUp]
        public void SetUp()
        {
            this.LoadSettings();
            this.WebClient = new WebClient(this.Settings.Username, this.Settings.Password);
        }

        [TearDown]
        public void TearDown()
        {
            this.WebClient = null;
        }

        /// <summary>
        /// Gets the <see cref="Net.Settings"/> for the current test fixture
        /// </summary>
        public Net.Settings Settings { get; protected set; }

        /// <summary>
        /// Gets the <see cref="WebClient"/> that is used to execute the integration tests
        /// </summary>
        public WebClient WebClient { get; private set; }

        /// <summary>
        /// Load the <see cref="WebservicesIntegrationTests.Net.Settings"/> and populate the <see cref="Settings"/> property.
        /// </summary>
        protected void LoadSettings()
        {
            var settingsFilePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "settings.json");
            using (StreamReader file = File.OpenText(settingsFilePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                this.Settings = (Net.Settings)serializer.Deserialize(file, typeof(Net.Settings));
            }
        }

        /// <summary>
        /// Convenience method to prefix the supplied partial path with the current Test directory.
        /// </summary>
        /// <param name="path">
        /// The partial path.
        /// </param>
        /// <returns>
        /// The supplied partial path with the current test directory prefixed.
        /// </returns>
        protected string GetPath(string path)
        {
            var expectedJsonPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, path);
            return expectedJsonPath;
        }

        /// <summary>
        /// Gets the JSON from the specified file
        /// </summary>
        /// <param name="path">
        /// The path to the file that contains JSON
        /// </param>
        /// <returns>
        /// a JSON string
        /// </returns>
        protected string GetJsonFromFile(string path)
        {
            var content = System.IO.File.ReadAllText(path);
            return content;
        }
    }
}