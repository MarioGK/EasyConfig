using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyConfig
{
    public static class ConfigurationManager
    {
        /// <summary>
        /// Determines which folder will be used to save the configuration files. Default = Configs.
        /// </summary>
        public static string SavePath { get; set; } = "Configs";

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
                    var json = JsonSerializer.Serialize(defaultClass);
                    File.WriteAllText(filePath, json);
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
                var configObject = JsonSerializer.Deserialize<T>(fileContents);
                return configObject;
            }
            catch(Exception e)
            {
                e.Data.Add("ConfigTestError", $"Failed to fetch the configuration file with the name {name}.");
                throw;
            }
        }

        /// <summary>
        /// Returns the saved configuration file.
        /// </summary>
        /// <typeparam name="T">Configuration class.</typeparam>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>()
        {
            return await Task.Run(Get<T>);
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
            
                var json = JsonSerializer.Serialize(config);
                File.WriteAllText(filePath, json);
            }
            catch(Exception e)
            {
                e.Data.Add("ConfigTestError", $"Failed to save the configuration file with the name {name}.");
                throw;
            }
        }

        public static async Task SaveAsync(object config)
        {
            await Task.Run(() => Save(config));
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