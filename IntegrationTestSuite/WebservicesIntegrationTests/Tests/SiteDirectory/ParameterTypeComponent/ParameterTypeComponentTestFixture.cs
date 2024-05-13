// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterTypeComponentTestFixture.cs" company="Starion Group S.A.">
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
    using System.Linq;

    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;

    using NUnit.Framework;

    [TestFixture]
    public class ParameterTypeComponentTestFixture : WebClientTestFixtureBaseWithDatabaseRestore
    {
        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterTypeComponentIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request 
            var arrayParameterTypeComponentUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/4a783624-b2bc-4e6d-95b3-11d036f6e917/component");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArrayParameterType = this.WebClient.GetDto(arrayParameterTypeComponentUri);

            //check if there are two ParameterTypeComponent objects 
            Assert.AreEqual(2, jArrayParameterType.Count);

            // define the URI on which to perform a GET request 
            var compoundParameterTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/0d3178f9-68d0-4b1a-afe8-d5df0b66f1d4/component");

            // get a response from the data-source as a JArray (JSON Array)
            var jArrayCompoundParameterType = this.WebClient.GetDto(compoundParameterTypeUri);

            //check if there two ParameterTypeComponent objects 
            Assert.AreEqual(2, jArrayCompoundParameterType.Count);

            var jArray = new JArray(jArrayParameterType.Union(jArrayCompoundParameterType));
            Assert.AreEqual(4, jArray.Count);

            ParameterTypeComponentTestFixture.VerifyProperties(jArray);
        }

        [Test]
        [Category("POST")]
        public void VerifyThatReorderParameterTypeComponentWorks()
        {
            var siteDirectoryUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294");
            var postBodyPath = this.GetPath("Tests/SiteDirectory/ParameterTypeComponent/PostReorderParameterTypeComponents.json");

            var postBody = base.GetJsonFromFile(postBodyPath);
            var jArray = this.WebClient.PostDto(siteDirectoryUri, postBody);

            var parameterType = jArray.Single(x => (string)x[PropertyNames.Iid] == "0d3178f9-68d0-4b1a-afe8-d5df0b66f1d4");

            var expectedOptions = new List<OrderedItem> { new OrderedItem(1, "8019277f-8bc7-463b-b3bb-46a404493e31"), new OrderedItem(2, "b607fdc1-7578-48f9-8597-caba56df3177") };
            var optionsArray = JsonConvert.DeserializeObject<List<OrderedItem>>(parameterType[PropertyNames.Component].ToString());
            CollectionAssert.AreEquivalent(expectedOptions, optionsArray);
        }

        [Test]
        [Category("GET")]
        public void VerifyThatExpectedParameterTypeComponentWithContainerIsReturnedFromWebApi()
        {
            // define the URI on which to perform a GET request
            var arrayParameterTypeComponentUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/4a783624-b2bc-4e6d-95b3-11d036f6e917/component?includeAllContainers=true");
            
            // get a response from the data-source as a JArray (JSON Array)
            var jArrayParameterType = this.WebClient.GetDto(arrayParameterTypeComponentUri);

            //check if there are 5 objects
            Assert.AreEqual(5, jArrayParameterType.Count);

            // get a specific SiteDirectory from the result by it's unique id
            var siteDirectory = jArrayParameterType.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");

            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            var siteReferenceDataLibrary = jArrayParameterType.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific ArrayParameterType from the result by it's unique id
            var arrayParameterType = jArrayParameterType.Single(x => (string) x[PropertyNames.Iid] == "4a783624-b2bc-4e6d-95b3-11d036f6e917");

            // define the URI on which to perform a GET request 
            var compoundParameterTypeUri = new Uri($"{this.Settings.Hostname}/SiteDirectory/f13de6f8-b03a-46e7-a492-53b2f260f294/siteReferenceDataLibrary/c454c687-ba3e-44c4-86bc-44544b2c7880/parameterType/0d3178f9-68d0-4b1a-afe8-d5df0b66f1d4/component?includeAllContainers=true");

            // get a response from the data-source as a JArray (JSON Array)
            var jArrayCompoundParameterType = this.WebClient.GetDto(compoundParameterTypeUri);

            //check if there are 5 objects
            Assert.AreEqual(5, jArrayParameterType.Count);

            // get a specific SiteDirectory from the result by it's unique id
            siteDirectory = jArrayCompoundParameterType.Single(x => (string) x[PropertyNames.Iid] == "f13de6f8-b03a-46e7-a492-53b2f260f294");
            
            SiteDirectoryTestFixture.VerifyProperties(siteDirectory);

            // get a specific SiteReferenceDataLibrary from the result by it's unique id
            siteReferenceDataLibrary = jArrayCompoundParameterType.Single(x => (string) x[PropertyNames.Iid] == "c454c687-ba3e-44c4-86bc-44544b2c7880");
            
            SiteReferenceDataLibraryTestFixture.VerifyProperties(siteReferenceDataLibrary);

            // get a specific CompoundParameterType from the result by it's unique id
            var compoundParameterType = jArrayCompoundParameterType.Single(x => (string) x[PropertyNames.Iid] == "0d3178f9-68d0-4b1a-afe8-d5df0b66f1d4");
            
            CompoundParameterTypeTestFixture.VerifyProperties(compoundParameterType);

            var jArray = new JArray(jArrayParameterType.Union(jArrayCompoundParameterType));
            Assert.AreEqual(10, jArray.Count);

            ParameterTypeComponentTestFixture.VerifyProperties(jArray);
        }

        /// <summary>
        /// Verifies all properties of the ParameterTypeComponent <see cref="JToken"/>
        /// </summary>
        /// <param name="parameterTypeComponent">
        /// The <see cref="JToken"/> that contains the properties of
        /// the ParameterTypeComponent object
        /// </param>
        public static void VerifyProperties(JToken parameterTypeComponent)
        {
            var parameterTypeComponentObject =
                parameterTypeComponent.Single(
                    x => (string) x[PropertyNames.Iid] == "b607fdc1-7578-48f9-8597-caba56df3177");
            
            // verify the amount of returned properties 
            Assert.AreEqual(6, parameterTypeComponentObject.Children().Count());
            
            // assert that the properties are what is expected
            Assert.AreEqual("b607fdc1-7578-48f9-8597-caba56df3177",
                (string) parameterTypeComponentObject[PropertyNames.Iid]);
           
            Assert.AreEqual(1, (int) parameterTypeComponentObject[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterTypeComponent", (string) parameterTypeComponentObject[PropertyNames.ClassKind]);
            
            Assert.AreEqual("4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d",
                (string) parameterTypeComponentObject[PropertyNames.ParameterType]);
            
            Assert.AreEqual("53e82aeb-c42c-475c-b6bf-a102af883471",
                (string) parameterTypeComponentObject[PropertyNames.Scale]);
            
            Assert.AreEqual("TestParameterTypeComponentA",
                (string) parameterTypeComponentObject[PropertyNames.ShortName]);

            parameterTypeComponentObject =
                parameterTypeComponent.Single(
                    x => (string) x[PropertyNames.Iid] == "8019277f-8bc7-463b-b3bb-46a404493e31");
 
            // verify the amount of returned properties 
            Assert.AreEqual(6, parameterTypeComponentObject.Children().Count());
            
            // assert that the properties are what is expected
            Assert.AreEqual("8019277f-8bc7-463b-b3bb-46a404493e31",
                (string) parameterTypeComponentObject[PropertyNames.Iid]);
            
            Assert.AreEqual(1, (int) parameterTypeComponentObject[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterTypeComponent", (string) parameterTypeComponentObject[PropertyNames.ClassKind]);
            
            Assert.AreEqual("4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d",
                (string) parameterTypeComponentObject[PropertyNames.ParameterType]);
            
            Assert.AreEqual("53e82aeb-c42c-475c-b6bf-a102af883471",
                (string) parameterTypeComponentObject[PropertyNames.Scale]);
           
            Assert.AreEqual("TestParameterTypeComponentB",
                (string) parameterTypeComponentObject[PropertyNames.ShortName]);

            parameterTypeComponentObject =
                parameterTypeComponent.Single(
                    x => (string) x[PropertyNames.Iid] == "9f17b223-446e-4a0c-afdb-60222b8e459e");
            
            // verify the amount of returned properties 
            Assert.AreEqual(6, parameterTypeComponentObject.Children().Count());
            
            // assert that the properties are what is expected
            Assert.AreEqual("9f17b223-446e-4a0c-afdb-60222b8e459e",
                (string) parameterTypeComponentObject[PropertyNames.Iid]);
            
            Assert.AreEqual(1, (int) parameterTypeComponentObject[PropertyNames.RevisionNumber]);
           
            Assert.AreEqual("ParameterTypeComponent", (string) parameterTypeComponentObject[PropertyNames.ClassKind]);
            
            Assert.AreEqual("4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d",
                (string) parameterTypeComponentObject[PropertyNames.ParameterType]);
            
            Assert.AreEqual("53e82aeb-c42c-475c-b6bf-a102af883471",
                (string) parameterTypeComponentObject[PropertyNames.Scale]);
            
            Assert.AreEqual("TestArrayParameterTypeComponentA",
                (string) parameterTypeComponentObject[PropertyNames.ShortName]);

            parameterTypeComponentObject =
                parameterTypeComponent.Single(
                    x => (string) x[PropertyNames.Iid] == "f3ddc526-1ce8-4298-bd95-13e95d6f4cdd");
            
            // verify the amount of returned properties 
            Assert.AreEqual(6, parameterTypeComponentObject.Children().Count());
            
            // assert that the properties are what is expected
            Assert.AreEqual("f3ddc526-1ce8-4298-bd95-13e95d6f4cdd",
                (string) parameterTypeComponentObject[PropertyNames.Iid]);
           
            Assert.AreEqual(1, (int) parameterTypeComponentObject[PropertyNames.RevisionNumber]);
            Assert.AreEqual("ParameterTypeComponent", (string) parameterTypeComponentObject[PropertyNames.ClassKind]);
            
            Assert.AreEqual("4f9f3d9b-f3de-4ef5-b6cb-2e22199fab0d",
                (string) parameterTypeComponentObject[PropertyNames.ParameterType]);
            
            Assert.AreEqual("53e82aeb-c42c-475c-b6bf-a102af883471",
                (string) parameterTypeComponentObject[PropertyNames.Scale]);
            
            Assert.AreEqual("TestArrayParameterTypeComponentB",
                (string) parameterTypeComponentObject[PropertyNames.ShortName]);
        }
    }
}
