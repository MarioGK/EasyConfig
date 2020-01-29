using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace EasyConfig
{
    public static class ConfigurationManager
    {
        /// <summary>
        /// Determines which folder will be used to save the configuration files. Default = Configs.
        /// </summary>
        public static string SavePath { get; set; } = "Configs";

        /// <summary>
        /// This list is to just store the references of the configs gotten by the Get function to be used by the SaveAll to be saved when called.
        /// </summary>
        private static List<object> _configsFetched = new List<object>();

        /// <summary>
        /// Returns the saved configuration file.
        /// </summary>
        /// <typeparam name="T">Configuration class.</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            var name = typeof(T).Name;
            var filePath = Path.Combine(SavePath, $"{name}.json");
            
            MakeSureDirectoryExists();

            //If file does not exists, create a class with the default configuration and save
            if (!File.Exists(filePath))
            {
                try
                {
                    var defaultClass = Activator.CreateInstance<T>();
                    var json = JsonConvert.SerializeObject(defaultClass);
                    File.WriteAllText(filePath, json);
                    _configsFetched.Add(defaultClass);
                    return defaultClass;
                }
                catch(Exception e)
                {
                    e.Data.Add("ConfigTestError", $"Failed to create the initial configuration file with the name {name}.");
                    throw;
                }
            }

            //If file exists, read and return its contents.
            try
            {
                var fileContents = File.ReadAllText(filePath);
                var configObject = JsonConvert.DeserializeObject<T>(fileContents);
                _configsFetched.Add(configObject);
                return configObject;
            }
            catch(Exception e)
            {
                e.Data.Add("ConfigTestError", $"Failed to fetch the configuration file with the name {name}.");
                throw;
            }
        }

        /// <summary>
        /// Saves all configuration gotten using this class.
        /// </summary>
        public static void SaveAll()
        {
            foreach (var config in _configsFetched)
            {
                Save(config);
            }
        }

        /// <summary>
        /// Saves the configuration class into a file.
        /// </summary>
        /// <param name="config">Configuration class</param>
        /// <exception cref="Exception">Is thrown when it fails to save a config file.</exception>
        public static void Save(object config)
        {
            var name = config.GetType().Name;
            
            MakeSureDirectoryExists();

            try
            {
                var filePath = Path.Combine(SavePath, $"{name}.json");
            
                var json = JsonConvert.SerializeObject(config);
                File.WriteAllText(filePath, json);
            }
            catch(Exception e)
            {
                e.Data.Add("ConfigTestError", $"Failed to save the configuration file with the name {name}.");
                throw;
            }
        }

        private static void MakeSureDirectoryExists()
        {
            //If Directory does not exists, create it.
            try
            {
                if (!Directory.Exists(SavePath))
                {
                    Directory.CreateDirectory(SavePath);
                }
            }
            catch (Exception e)
            {
                e.Data.Add("ConfigTestError", $"Failed to create the ({SavePath}) directory.");
                throw;
            }
        }
    }
}