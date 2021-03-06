﻿using LitJson;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PicPrompt.Utils
{
    public class Configuration : IDisposable
    {
        public FileInfo FileInfo { get; private set; }

        protected JsonData _data;
        private FileSystemWatcher _watcher;
        
        public Configuration()
        {
        }

        public Configuration(string path)
        {
            Load(path);
        }

        public void Dispose()
        {
            if (_watcher != null)
            {
                _watcher.Dispose();
                _watcher = null;
            }
        }

        public void Load(string path)
        {
            FileInfo = new FileInfo(path);

            _data = JsonMapper.ToObject(File.ReadAllText(FileInfo.FullName));
            _watcher = new FileSystemWatcher(FileInfo.DirectoryName, FileInfo.Name)
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

                    _data = JsonMapper.ToObject(File.ReadAllText(FileInfo.FullName));
                }).Start();
            };
        }

        public void Save()
        {
            if (_data != null)
            {
                File.WriteAllText(FileInfo.FullName, _data.ToJson());
            }            
        }

        public JsonData this[int index]
        {
            get => _data[index];
            set
            {
                _data[index] = value;
            }
        }

        public JsonData this[string prop_name]
        {
            get => _data[prop_name];
            set
            {
                _data[prop_name] = value;
            }
        }
    }
}
