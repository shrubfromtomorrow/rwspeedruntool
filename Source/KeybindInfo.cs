using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpeedrunTool
{
    [Serializable]
    public class Keybinds
    {
        public Dictionary<string, string> binds_to_file = new Dictionary<string, string>();

        public static Dictionary<string, KeyCode> ResetToDefaults(Dictionary<string, KeyCode> _binds)
        {
            Dictionary<string, KeyCode> binds = _binds;

            //Any Default Binds Go Here
            binds.Add("Spawn Rarefaction Cell", KeyCode.Alpha3);

            return binds;
        }
    }

    public class CategoryInfo
    {
        public string Name;
        public List<string> Functions = new();

        public static List<string> Categories = new();
        internal static Dictionary<string, CategoryInfo> CategoryInfos = new();

        public CategoryInfo(string Name)
        {
            this.Name = Name;
            
            foreach (var bindable in SpeedrunTool.bindMethods)
            {
                string name = bindable.Key;
                string cat = bindable.Value.category;

                AddFunction(cat, name);
            }
        }

        public void Add(string method) => Functions.Add(method);

        public static void AddFunction(string category, string name)
        {
            if (!CategoryInfos.TryGetValue(category, out CategoryInfo info))
            {
                info = new CategoryInfo(category);

                if (!Categories.Contains(category)) Categories.Add(category);
                CategoryInfos.Add(category, info);
            }

            info.Add(name);
        }
    }
}
