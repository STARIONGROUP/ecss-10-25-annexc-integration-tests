// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebClient.cs" company="RHEA S.A.">
//   Copyright (c) 2016 RHEA S.A.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebservicesIntegrationTests.Net
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;

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
            var webResponse = this.RetrieveHttpPostResponse(uri, postBody);
            return this.ExtractJarrayFromResponse(webResponse);
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
            var jArray = JArray.Parse(response);
            return jArray;
        }

        /// <summary>
        /// Extracts a <see cref="IEnumerable{Dictionary{string, string}}"/> from the provided <see cref="WebResponse"/>
        /// </summary>
        /// <param name="webResponse">
        /// The <see cref="WebResponse"/> that from which the Dictionaries are extracted
        /// </param>
        /// <returns>
        /// A <see cref="IEnumerable{Dictionary{string, string}}"/> with the response or null if the response was empty
        /// </returns>
        private IEnumerable<Dictionary<string, string>> ExtractDictionariesFromResponse(WebResponse webResponse)
        {
            var responseStream = webResponse.GetResponseStream();
            if (responseStream == null)
            {
                return null;
            }

            var reader = new StreamReader(responseStream);
            var response = reader.ReadToEnd();
            
            var dictionaries = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(response);
            return dictionaries;
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
            request.Method = method;
            request.ContentType = JsonContentType;
            
            var encoded = Convert.ToBase64String(Encoding.GetEncoding(Utf8).GetBytes(this.UserName + ":" + this.Password));
            request.Headers.Add("Authorization", "Basic " + encoded);

            return request;
        } 
    }
}
