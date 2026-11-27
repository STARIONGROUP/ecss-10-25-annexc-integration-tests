// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebClientTestFixtureBase.cs" company="Starion Group S.A.">
//
//   Copyright 2016-2025 Starion Group S.A.
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
    using System.Threading;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using WebservicesIntegrationTests.Net;

    /// <summary>
    /// The web client test fixture base class.
    /// </summary>
    [SingleThreaded] 
    [TestFixture]
    public abstract class WebClientTestFixtureBase
    {
        [SetUp]
        public virtual void SetUp()
        {
            this.LoadSettings();
            this.WebClient = new WebClient(this.Settings.Username, this.Settings.Password);
        }

        [TearDown]
        public virtual void TearDown()
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

        public void CreateNewWebClientForUser(string userName, string passWord)
        {
            this.WebClient = new WebClient(userName, passWord);
        }

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
        public string GetPath(string path)
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
        public string GetJsonFromFile(string path)
        {
            var content = System.IO.File.ReadAllText(path);
            return content;
        }

        /// <summary>
        /// Polls /tasks/{id} until the CometTask has finished
        /// </summary>
        /// <param name="taskId">
        /// The id of the CometTask
        /// </param>
        /// <param name="timeout">
        /// The longest time to wait
        /// </param>
        /// <returns>
        /// The finished CometTask
        /// </returns>
        public JToken WaitForCometTaskToFinish(string taskId, TimeSpan timeout)
        {
            var taskUri = new Uri($"{this.Settings.Hostname}/tasks/{taskId}");
            var deadline = DateTime.UtcNow + timeout;

            while (true)
            {
                var cometTask = this.WebClient.GetJson(taskUri);

                if ((int)cometTask["statusKind"] != CometTaskStatusKind.Processing)
                {
                    return cometTask;
                }

                if (DateTime.UtcNow > deadline)
                {
                    Assert.Fail($"The CometTask {taskId} did not finish within {timeout}: {cometTask}");
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}