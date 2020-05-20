using System.IO;
using System.Text.Json;

namespace EasyConfig
{
    public class ConfigurationWatcher<T>
    {
        public bool AutoStart { get; set; }

        private FileSystemWatcher _fileSystemWatcher;

        public ConfigurationWatcher()
        {
            if (AutoStart)
            {
                Start();
            }
        }

        public void Start()
        {
            _fileSystemWatcher = new FileSystemWatcher
            {
                Filter = ""
                
            };
            
            _fileSystemWatcher.Changed += FileSystemWatcherOnChanged;

            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void FileSystemWatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
            
            var configContents = File.ReadAllText(e.FullPath);
            var config = JsonSerializer.Deserialize<T>(configContents);
            ConfigChanged?.Invoke(config);
            
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            if (_fileSystemWatcher != null)
            {
                _fileSystemWatcher.EnableRaisingEvents = false;
                _fileSystemWatcher.Dispose();
                _fileSystemWatcher = null;
            }
        }

        public delegate void OnConfigChangedDelegate(T config);

        public event OnConfigChangedDelegate ConfigChanged;
    }
}