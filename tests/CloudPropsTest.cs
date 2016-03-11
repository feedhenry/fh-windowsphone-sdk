using FHSDK;
using FHSDK.Config;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using tests.Mocks;

namespace tests
{
    [TestFixture]
    public class CloudPropsTest
    {
        [Test]
        public void TestReadCloudPropsWithJsonContainingValues()
        {
            // given
            var json = JObject.Parse(@"{
                'url': 'URL',
                'hosts': {'host': 'HOST',  'environment': 'ENV'}
             }");
            var config = new FHConfig(new MockDeviceService());
            var props = new CloudProps(json, config);

            // when
            var host = props.GetCloudHost();
            var env = props.GetEnv();

            // then
            Assert.AreEqual("URL", host);
            Assert.AreEqual("ENV", env);
        }

        [Test]
        public void TestReadCloudPropsWithJsonEmptyValuesForUrlAndHostWithoutUrl()
        {
            // given
            var json = JObject.Parse(@"{
                'hosts': {'host': 'HOST',  'environment': 'ENV', 'releaseCloudUrl': 'RELEASE_CLOUD_URL'}
             }");
            var config = new FHConfig(new MockDeviceService());
            var props = new CloudProps(json, config);

            // when
            var host = props.GetCloudHost();
            var env = props.GetEnv();

            // then
            Assert.AreEqual("RELEASE_CLOUD_URL", host);
            Assert.AreEqual("ENV", env);
        }

        [Test]
        public void TestReadCloudPropsWithJsonEmptyValuesForUrlAndHostWithUrl()
        {
            // given
            var json = JObject.Parse(@"{
                'hosts': {'host': 'HOST', 'environment': 'ENV', 'url': 'URL_HOST'}
             }");
            var config = new FHConfig(new MockDeviceService());
            var props = new CloudProps(json, config);

            // when
            var host = props.GetCloudHost();
            var env = props.GetEnv();

            // then
            Assert.AreEqual("URL_HOST", host);
            Assert.AreEqual("ENV", env);
        }

        [Test]
        public void TestURLFormatting()
        {
            // given
            var json = JObject.Parse(@"{
                'url': 'http://someserver.com/',
                'hosts': {'host': 'HOST',  'environment': 'ENV'}
             }");
            var config = new FHConfig(new MockDeviceService());
            var props = new CloudProps(json, config);

            // when
            var host = props.GetCloudHost();

            // then
            Assert.AreEqual("http://someserver.com", host);
        }
    }
}