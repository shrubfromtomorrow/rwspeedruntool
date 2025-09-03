using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using UnityEngine;
using System.Runtime;

namespace SpeedrunTool
{
    //empty class that will get extensions
    public class SaveSettings {}

    public class GlobalSettings
    {
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public Dictionary<string, KeyCode> binds = new Dictionary<string, KeyCode>()
        {
            { "SpawnKey", KeyCode.Alpha3 }
        };

        public bool FirstRun = true;

        //TODO: need to find out how to not hardcode this


    }


}
