using UnityModManagerNet;
using ModKit;
using static SolastaModTemplate.Main;

namespace SolastaModTemplate.Menus.Viewers
{    public class SampleModMenu2 : IMenuSelectablePage
    {
        public string Name => "Sample Menu 2";

        public int Priority => 1;

        public void OnGUI(UnityModManager.ModEntry modEntry)
        {
            if (Mod == null || !Mod.Enabled) return;

            UI.Toggle("Toggle Me 3", ref Main.Settings.toggleTest3, 0, UI.AutoWidth());
            UI.Toggle("Toggle Me 4", ref Main.Settings.toggleTest4, 0, UI.AutoWidth());
        }
    }
}
