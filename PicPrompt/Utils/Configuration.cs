using LitJson;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PicPrompt.Utils
{
    public class Configuration : IDisposable
    {
        private JsonData _data;
        private FileSystemWatcher _watcher;

        public Configuration(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            var info = new FileInfo(path);

            _data = JsonMapper.ToObject(File.ReadAllText(info.FullName));

            _watcher = new FileSystemWatcher(info.DirectoryName, info.Name)
            {
                NotifyFilter = NotifyFilters.LastWrite,
                EnableRaisingEvents = true
            };

            _watcher.Changed += (_, __) =>
            {
                // Without this, we get IOException :P
                new Task(async () => 
                {
                    await Task.Delay(100);

                    _data = JsonMapper.ToObject(File.ReadAllText(info.FullName));
                }).Start();
            };
        }

        public void Dispose()
        {
            if (_watcher != null)
            {
                _watcher.Dispose();
                _watcher = null;
            }    
        }

        public JsonData this[int index]
        {
            get => _data[index];
        }

        public JsonData this[string prop_name]
        {
            get => _data[prop_name];
        }
    }
}
