using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace SpeedrunTool.Source
{
    //empty class that will get extensions
    public class SaveSettings {}

    [Serializable]
    public class Keybinds
    {
        public Dictionary<string, string> binds_to_file = new Dictionary<string, string>();
    }

    public class GlobalSettings
    {
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public Dictionary<string, KeyCode> binds = new Dictionary<string, KeyCode>();

        //TODO: need to find out how to not hardcode this
        public readonly string ModBasePath = "C:/Program Files(x86)/Steam/steamapps/common/Rain World/RainWorld_Data/StreamingAssets/mods/SpeedrunTool";


    }
}
