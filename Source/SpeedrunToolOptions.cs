using System.Xml.Linq;
using BepInEx.Logging;
using Menu.Remix.MixedUI;
using Menu.Remix.MixedUI.ValueTypes;
using UnityEngine;


namespace SpeedrunTool
{
    public class SpeedrunToolOptions : OptionInterface
    {

        public readonly Configurable<KeyCode> OpenMenu;
        private OpHoldButton? ResetBinds;
        private UIelement[]? options;

        public SpeedrunToolOptions()
        {
            OpenMenu = config.Bind<KeyCode>("OpenMenu", KeyCode.P);
        }
        public override void Initialize()
        {
            base.Initialize();
            ResetBinds = new OpHoldButton(new Vector2(250f, 0f), new Vector2(120f, 30f), OptionInterface.Translate("RESET BINDS"), 80f) { colorEdge = new Color(0.85f, 0.35f, 0.4f) };

            OpTab tab = new OpTab(this, "Config");
            Tabs = new[] { tab };

            options = new UIelement[]
            {
                new OpLabel(10f, 560f, "SpeedrunTool Config", true),
                new OpLabel(10f, 512f, "Button to open the menu: ") {alignment = FLabelAlignment.Left, description = "Button to open the menu"},
                new OpKeyBinder(OpenMenu, new Vector2(160f, 505f), new Vector2(120f, 30f), false, OpKeyBinder.BindController.AnyController) {description = "Button to open the menu"},

                ResetBinds
            };
            this.ResetBinds.OnPressDone += (UIfocusable trigger) => Keybinds.ResetToDefaults(SpeedrunTool.settings.binds);
            tab.AddItems(options);
        }
    }
}
