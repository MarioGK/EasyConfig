using System.IO;
using NUnit.Framework;

namespace EasyConfig.Tests
{
    public class GetTest
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
        public void Get()
        {
            var config = ConfigurationManager.Get<ConfigTest>();
            Assert.AreEqual("DefaultString", config.StringTest);
            Assert.AreEqual(10, config.IntTest);
        }
    }
}