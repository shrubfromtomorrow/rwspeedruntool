using System.Xml.Linq;
using BepInEx.Logging;
using Menu.Remix.MixedUI;
using Menu.Remix.MixedUI.ValueTypes;
using UnityEngine;


namespace SpeedrunTool
{
    public class SpeedrunToolOptions : OptionInterface
    {

        public readonly Configurable<KeyCode> SpawnKey;
        private UIelement[]? options;

        public SpeedrunToolOptions(SpeedrunTool speedrunTool)
        {
            //SpawnKey = config.Bind<KeyCode>("SpawnKeybind", KeyCode.Alpha3);
            if(SpeedrunTool.settings.binds.TryGetValue("SpawnKey", out KeyCode key)) SpawnKey = config.Bind<KeyCode>("SpawnKeybind", key);
        }

        public override void Initialize()
        {
            base.Initialize();

            OpTab tab = new OpTab(this, "Config");
            Tabs = new[] { tab };

            options = new UIelement[]
            {
                new OpLabel(10f, 560f, "SpeedrunTool Config", true),
                new OpLabel(10f, 512f, "Button to spawn selected item:") {alignment = FLabelAlignment.Left, description = "Button to spawn selected item"},
                new OpKeyBinder(SpawnKey, new Vector2(200f, 505f), new Vector2(140f, 20f), false, OpKeyBinder.BindController.AnyController) {description = "Button to spawn selected item"},
            };
            tab.AddItems(options);
        }
    }
}
