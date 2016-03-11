using System;
using System.Collections.Generic;
using FHSDK;
using FHSDK.FHHttpClient;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using tests.Mocks;

namespace tests
{
    [TestFixture]
    public class HttpClientTest
    {
        [Test]
        public async void ShouldPerformGet()
        {
            //given
            await FHClient.Init();
            var mock = new MockHttpClient();
            FHHttpClientFactory.Get = () => mock;
            const string method = "GET";

            //when
            await FHHttpClient.SendAsync(new Uri("http://localhost/test"), method, null,
                JObject.Parse("{'key-data': 'value'}"), TimeSpan.FromSeconds(20));

            //then
            Assert.IsNotNull(mock.Request);
            Assert.AreEqual("http://localhost/test?key-data=\"value\"", mock.Request.RequestUri.ToString());
        }

        [Test]
        public async void ShouldSendAsync()
        {
            //given
            await FHClient.Init();
            var mock = new MockHttpClient();
            FHHttpClientFactory.Get = () => mock;
            const string method = "POST";

            //when
            await
                FHHttpClient.SendAsync(new Uri("http://localhost/test"), method,
                    new Dictionary<string, string> {{"key", "value"}},
                    "request-data", TimeSpan.FromSeconds(20));

            //then
            Assert.IsNotNull(mock.Request);
            Assert.AreEqual(method, mock.Request.Method.Method);
            Assert.IsTrue(mock.Request.Headers.Contains("key"));
            Assert.AreEqual("\"request-data\"", await mock.Request.Content.ReadAsStringAsync());
            Assert.AreEqual(20, mock.Timeout.Seconds);
        }
    }
}