using System.IO;
using NUnit.Framework;

namespace EasyConfig.Tests
{
    public class SaveAllTest
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
            var config = ConfigurationManager.Get<ConfigTest>();
            ConfigurationManager.SaveAll();
            FileAssert.Exists(Path.Combine(ConfigurationManager.SavePath, $"{config.GetType().Name}.json"));
        }
    }
}