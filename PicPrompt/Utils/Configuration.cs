using LitJson;
using System;
using System.IO;
using System.Threading;

namespace PicPrompt.Utils
{
    public class Configuration
    {
        private JsonData _data;

        public Configuration(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            var info = new FileInfo(path);

            _data = JsonMapper.ToObject(File.ReadAllText(info.FullName));

            // TODO: reload data when file changes
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
