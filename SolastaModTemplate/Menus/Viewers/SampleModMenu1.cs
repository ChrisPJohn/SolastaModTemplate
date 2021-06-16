using UnityModManagerNet;
using ModKit;
using static SolastaModTemplate.Main;

namespace SolastaModTemplate.Menus.Viewers
{
    public class SampleModMenu1 : IMenuSelectablePage
    {
        public string Name => "Sample Menu 1";

        public int Priority => 2;

        public void OnGUI(UnityModManager.ModEntry modEntry)
        {
            if (Mod == null || !Mod.Enabled) return;

            UI.Toggle("Toggle Me 1", ref Main.Settings.toggleTest1, 0, UI.AutoWidth());
            UI.Toggle("Toggle Me 2", ref Main.Settings.toggleTest2, 0, UI.AutoWidth());
        }
    }
}
