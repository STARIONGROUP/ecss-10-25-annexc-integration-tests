// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebClient.cs" company="Starion Group S.A.">
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

namespace WebservicesIntegrationTests.Net
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The purpose of the <see cref="WebClient"/> class is to provide access to
    /// an ECSS-E-TM-10-25 Annex C data-source
    /// </summary>
    public class WebClient
    {
        /// <summary>
        /// The UTF8 encoding string.
        /// </summary>
        private const string Utf8 = "UTF-8";

        /// <summary>
        /// The JSON content type.
        /// </summary>
        private const string JsonContentType = "application/json";

        /// <summary>
        /// The post method.
        /// </summary>
        private const string HttpPostMethod = "POST";

        /// <summary>
        /// The get method.
        /// </summary>
        private const string HttpGetMethod = "GET";

        /// <summary>
        /// Initializes a new instance of the <see cref="WebClient"/> class
        /// </summary>
        /// <param name="username">
        /// The user name that is used to authenticate
        /// </param>
        /// <param name="password">
        /// The password that is used to authenticate
        /// </param>
        public WebClient(string username, string password)
        {
            this.UserName = username;
            this.Password = password;
        }

        /// <summary>
        /// Gets the user name that is used to authenticate
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets the password that is used to authenticate
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Restores the data-set on the data-source
        /// </summary>
        /// <param name="hostname">
        /// The hostname where the services are hosted
        /// </param>
        public void Restore(string hostname)
        {
            var uriBuilder = new UriBuilder(hostname);

            uriBuilder.Path = Path.Combine(uriBuilder.Path, "Data/Restore");

            var uri = uriBuilder.Uri;

            var request = this.CreateWebRequest(uri, HttpPostMethod);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(string.Empty);
                streamWriter.Close();
            }

            request.GetResponse();
        }
        
        /// <summary>
        /// Performs a GET request on the provided URI and returns an <see cref="JArray"/>
        /// </summary>
        /// <param name="uri">
        /// The <see cref="Uri"/> that the GET request is performed on
        /// </param>
        /// <returns>
        /// A <see cref="JArray"/> with the response or null if the response was empty
        /// </returns>
        public JArray GetDto(Uri uri)
        {
            var webResponse = this.RetrieveHttpGetResponse(uri);
            return this.ExtractJarrayFromResponse(webResponse);
        }

        /// <summary>
        /// Performs a POST request on the provided <see cref="Uri"/> and returns a pretty printed JSON string
        /// </summary>
        /// <param name="uri">
        /// The <see cref="Uri"/> that the POST request is performed on</param>
        /// <param name="postBody">
        /// The content that is to be posted to the <see cref="Uri"/>
        /// </param>
        /// <returns>
        /// A <see cref="JArray"/> with the response or null if the response was empty
        /// </returns>
        public JArray PostDto(Uri uri, string postBody)
        {
            var stackTrace = new StackTrace();
            var frame = stackTrace.GetFrames()?[1];
            var method = frame?.GetMethod();

            //Only one or zero version attributes allowed
            var version = method?.GetCustomAttributes<CdpVersionAttribute>().SingleOrDefault()?.Version;

            var webResponse = 
                version != null 
                    ? this.RetrieveHttpPostResponseForVersion(uri, postBody, version) 
                    : this.RetrieveHttpPostResponse(uri, postBody);

            return this.ExtractJarrayFromResponse(webResponse);
        }

        /// <summary>
        /// Performs a POST request on the provided <see cref="Uri"/>, uploads a file and returns a pretty printed JSON string.
        /// </summary>
        /// <param name="uri">
        /// The <see cref="Uri"/> that the POST request is performed on
        /// </param>
        /// <param name="postJsonPath">
        /// The path to a file that contains JSON post message.
        /// </param>
        /// <param name="postFilePath">
        /// The path to a file that is uploaded.
        /// </param>
        /// <returns>
        /// A <see cref="JArray"/> with the response or null if the response was empty.
        /// </returns>
        public async Task<JArray>  PostFile(Uri uri, string postJsonPath, string postFilePath)
        {
            var response = await this.RetrieveHttpPostResponse(uri, postJsonPath, postFilePath);
            return this.ExtractJarrayFromResponse(response);
        }

        /// <summary>
        /// Get file response body.
        /// </summary>
        /// <param name="uri">
        /// The uri.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> that contain the file body.
        /// </returns>
        public async Task<byte[]> GetFileResponseBody(Uri uri)
        {
            return await this.RetrieveHttpFileGetResponse(uri);
        }

        /// <summary>
        /// Get model export file as byte array.
        /// </summary>
        /// <param name="uri">
        /// The uri.
        /// </param>
        /// <param name="engineeringModelSetupIds">
        /// EngineeringModelSetup Ids.
        /// </param>
        /// <returns>
        /// The <see cref="byte[]"/> of the file.
        /// </returns>
        public async Task<byte[]> GetModelExportFile(Uri uri, string[] engineeringModelSetupIds)
        {
            return await this.RetrieveHttpModelExportResponse(uri, engineeringModelSetupIds);
        }

        /// <summary>
        /// Extracts a JSON Array from the provided <see cref="WebResponse"/>
        /// </summary>
        /// <param name="webResponse">
        /// The <see cref="WebResponse"/> 
        /// </param>
        /// <returns>
        /// A <see cref="JArray"/> with the response or null if the response was empty
        /// </returns>
        private JArray ExtractJarrayFromResponse(WebResponse webResponse)
        {
            var responseStream = webResponse.GetResponseStream();

            if (responseStream == null)
            {
                return null;
            }

            var reader = new StreamReader(responseStream);
            var response = reader.ReadToEnd();

            using (JsonReader jsonReader = new JsonTextReader(new StringReader(response)))
            {
                jsonReader.DateParseHandling = DateParseHandling.None;
                var jArray = JArray.Load(jsonReader);
                return jArray;
            }
        }

        /// <summary>
        /// Extracts a <see cref="string"/> from the provided <see cref="WebResponse"/>
        /// </summary>
        /// <param name="webResponse">
        /// The <see cref="WebResponse"/> 
        /// </param>
        /// <returns>
        /// A <see cref="string"/>
        /// </returns>
        public string ExtractExceptionStringFromResponse(WebResponse webResponse)
        {
            var responseStream = webResponse.GetResponseStream();

            if (responseStream == null)
            {
                return null;
            }

            var reader = new StreamReader(responseStream);
            var response = reader.ReadToEnd();

            return JsonConvert.DeserializeObject<string>(response);
        }

        /// <summary>
        /// Extracts a JSON Array from the provided <see cref="string"/>
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// A <see cref="JArray"/> with the response or null if the response was empty.
        /// </returns>
        private JArray ExtractJarrayFromResponse(string response)
        {
            return JArray.Parse(response);
        }

        /// <summary>
        /// Performs a POST request on the provided <see cref="Uri"/> and return a  <see cref="HttpWebResponse"/>
        /// </summary>
        /// <param name="uri">
        /// The <see cref="Uri"/> that the POST request is performed on
        /// </param>
        /// <param name="postBody">
        /// The content that is to be posted to the <see cref="Uri"/>
        /// </param>
        /// <returns>
        /// The <see cref="HttpWebResponse"/>.
        /// </returns>
        private HttpWebResponse RetrieveHttpPostResponse(Uri uri, string postBody)
        {
            var request = this.CreateWebRequest(uri, HttpPostMethod);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postBody);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var webResponse = (HttpWebResponse)request.GetResponse();
            return webResponse;
        }

        /// <summary>
        /// Performs a POST request on the provided <see cref="Uri"/> and return a  <see cref="HttpWebResponse"/>
        /// </summary>
        /// <param name="uri">
        /// The <see cref="Uri"/> that the POST request is performed on
        /// </param>
        /// <param name="postBody">
        /// The content that is to be posted to the <see cref="Uri"/>
        /// </param>
        /// <param name="version">
        ///A specific CDP version number is needed
        /// </param>
        /// <returns>
        /// The <see cref="HttpWebResponse"/>.
        /// </returns>
        private HttpWebResponse RetrieveHttpPostResponseForVersion(Uri uri, string postBody, string version)
        {
            var request = this.CreateWebRequest(uri, HttpPostMethod);
            request.Headers.Add("Accept-CDP", version);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postBody);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var webResponse = (HttpWebResponse)request.GetResponse();
            return webResponse;
        }

        /// <summary>
        /// Performs a POST request on the provided <see cref="Uri"/> and returns a  <see cref="string"/>
        /// </summary>
        /// <param name="uri">
        /// The <see cref="Uri"/> that the POST request is performed on
        /// </param>
        /// <param name="postJsonPath">
        /// The path to a file that contains JSON post message.
        /// </param>
        /// <param name="postFilePath">
        /// The path to a file that is uploaded.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private async Task<string> RetrieveHttpPostResponse(Uri uri, string postJsonPath, string postFilePath)
        {
            var httpClient = new HttpClient();
            var encoded = Convert.ToBase64String(Encoding.GetEncoding(Utf8).GetBytes(this.UserName + ":" + this.Password));
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);

            var json = File.ReadAllBytes(postJsonPath);
            var jsonContent = new ByteArrayContent(json);
            jsonContent.Headers.Add("Content-Type", "application/json");

            var file = File.ReadAllBytes(postFilePath);
            var fileContent = new ByteArrayContent(file);
            var fileName = Path.GetFileName(postFilePath);
            fileContent.Headers.Add("Content-Type", "application/octet-stream");
            fileContent.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");

            var form = new MultipartFormDataContent { jsonContent, fileContent };

            var httpResponse = await httpClient.PostAsync(uri, form);

            httpResponse.EnsureSuccessStatusCode();
            httpClient.Dispose();
            var response = await httpResponse.Content.ReadAsStringAsync();

            return response;
        }

        /// <summary>
        /// Retrieve response body from file get response.
        /// </summary>
        /// <param name="uri">
        /// The uri.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> that contain the response body.
        /// </returns>
        private async Task<byte[]> RetrieveHttpFileGetResponse(Uri uri)
        {
            var request = this.CreateWebRequest(uri, HttpGetMethod);
            var httpResponse = (HttpWebResponse)request.GetResponse();

            var content = new StreamContent(httpResponse.GetResponseStream());

            content.Headers.Add("Content-Type", httpResponse.ContentType);

            var multipartContent = await content.ReadAsMultipartAsync();

            var body = await multipartContent.Contents[1].ReadAsByteArrayAsync();

            return body;
        }

        /// <summary>
        /// Retrieve response body from file get response.
        /// </summary>
        /// <param name="uri">
        /// The uri.
        /// </param>
        /// <param name="engineeringModelSetupIds">
        /// The EngineeringModelSetup Ids.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> that contain the response body.
        /// </returns>
        private async Task<byte[]> RetrieveHttpModelExportResponse(Uri uri, string[] engineeringModelSetupIds)
        {
            var request = this.CreateWebRequest(uri, HttpPostMethod);

            using (StreamWriter requestWriter = new StreamWriter(request.GetRequestStream()))
            {
                requestWriter.Write(JsonConvert.SerializeObject(engineeringModelSetupIds));
            }

            var httpResponse = (HttpWebResponse)request.GetResponse();
            var content = new StreamContent(httpResponse.GetResponseStream());
            var body = await content.ReadAsByteArrayAsync();

            return body;
        }

        /// <summary>
        /// Performs a GET request on the provided URI and returns the <see cref="HttpWebResponse"/>
        /// </summary>
        /// <param name="uri">
        /// The <see cref="Uri"/> that the GET request is performed on
        /// </param>
        /// <returns>
        /// A <see cref="HttpWebResponse"/>
        /// </returns>
        private HttpWebResponse RetrieveHttpGetResponse(Uri uri)
        {
            var request = this.CreateWebRequest(uri, HttpGetMethod);

            var webResponse = (HttpWebResponse)request.GetResponse();
            return webResponse;
        }

        /// <summary>
        /// Creates an instance of <see cref="HttpWebRequest"/> with the 
        /// </summary>
        /// <param name="uri">
        /// The <see cref="Uri"/> of the web request
        /// </param>
        /// <param name="method">
        /// The HTTP Verb
        /// </param>
        /// <returns>
        /// An instance of <see cref="HttpWebRequest"/>
        /// </returns>
        private HttpWebRequest CreateWebRequest(Uri uri, string method)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Timeout = 999999999;
            request.Method = method;
            request.ContentType = JsonContentType;

            var encoded = Convert.ToBase64String(Encoding.GetEncoding(Utf8).GetBytes(this.UserName + ":" + this.Password));
            request.Headers.Add("Authorization", "Basic " + encoded);

            return request;
        } 
    }
}
