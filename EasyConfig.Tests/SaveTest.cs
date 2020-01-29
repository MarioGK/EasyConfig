using System.IO;
using NUnit.Framework;

namespace EasyConfig.Tests
{
    public class SaveTest
    {
        [SetUp]
        public void Setup()
        {
            ConfigurationManager.SavePath = "Configs";
            
            if (Directory.Exists("Configs"))
            {
                Directory.Delete("Configs", true);
            }
        }

        [Test]
        public void Save()
        {
            var config = new ConfigTest {StringTest = "SaveTest", IntTest = 111};
            ConfigurationManager.Save(config);
            var gottenConfig = ConfigurationManager.Get<ConfigTest>();
            Assert.AreEqual(config.StringTest, gottenConfig.StringTest);
            Assert.AreEqual(config.IntTest, gottenConfig.IntTest);
        }
    }
}