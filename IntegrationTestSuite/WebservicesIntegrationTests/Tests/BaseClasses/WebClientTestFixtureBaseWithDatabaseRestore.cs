// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebClientTestFixtureBase.cs" company="RHEA System S.A.">
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
    using NUnit.Framework;

    /// <summary>
    /// The web client test fixture base class with default database restore action.
    /// </summary>
    [SingleThreaded] 
    [TestFixture]
    public abstract class WebClientTestFixtureBaseWithDatabaseRestore : WebClientTestFixtureBase
    {
        /// <summary>
        /// privat static (evil!) so the setup only runs once for all TestFixtures.
        /// If this field would not be static, then an extra restore would be performed for every TestFixture
        /// </summary>
        private static bool _firstRun = true;

        public override void SetUp()
        {
            base.SetUp();

            if (!_firstRun) return;

            _firstRun = false;
            this.WebClient.Restore(this.Settings.Hostname);
        }

        public override void TearDown()
        {
            this.WebClient.Restore(this.Settings.Hostname);
            base.TearDown();
        }
    }
}
