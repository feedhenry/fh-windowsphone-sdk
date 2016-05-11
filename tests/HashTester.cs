using FHSDK;
using FHSDK.Services;
using FHSDK.Services.Hash;
using FHSDK.Sync;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace tests
{
    [TestFixture]
    public class HashTester
    {
        [Test]
        public async void TestGenerateHashWithUnderscoreInKey()
        {
            // given
            await FHClient.Init();
            var data = new JObject
            {
                ["COMMENTS"] = "",
                ["FHID"] = "2553C7ED-9025-48F9-A346-EBE3E3AF943B",
                ["QUESTION_ID"] = 22,
                ["QUES_VALUE"] = "NO",
                ["VISIT_ID"] = 100220,
                ["TEST1_ttt"] = "test",
                ["TEST11_ttt"] = "test2"
            };

            // when
            var hash = FHSyncUtils.GenerateSHA1Hash(data);

            // then
            Assert.AreEqual("824d6ded431d16fe8f2ab02b0744ca06822a3fff", hash);
        }

        [Test]
        public async void TestObjectHash()
        {
            //given
            await FHClient.Init();
            var testObject = new JObject
            {
                ["testKey"] = "Test Data",
                ["testBoolKey"] = true,
                ["testNumKey"] = 10
            };
            var arr = new JArray {"obj1", "obj2"};
            testObject["testArrayKey"] = arr;
            var obj = new JObject
            {
                ["obj3key"] = "obj3",
                ["obj4key"] = "obj4"
            };
            testObject["testDictKey"] = obj;

            //when
            var hash = FHSyncUtils.GenerateSHA1Hash(testObject);

            //then
            Assert.AreEqual("5f4675723d658919ede35fac62fade8c6397df1d", hash);
        }

        [Test]
        public async void TestStringHash()
        {
            //given
            await FHClient.Init();
            const string text = "test";
            var hasher = ServiceFinder.Resolve<IHashService>();

            //when
            var nativeHashed = hasher.GenerateSha1Hash(text);

            Assert.AreEqual("a94a8fe5ccb19ba61c4c0873d391e987982fbbd3", nativeHashed);
        }
    }
}