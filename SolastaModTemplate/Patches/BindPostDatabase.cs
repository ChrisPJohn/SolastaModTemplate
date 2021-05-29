using HarmonyLib;

namespace SolastaModTemplate.Patches
{
    class BindPostDatabase
    {
        [HarmonyPatch(typeof(GameManager), "BindPostDatabase")]
        internal static class GameManager_BindPostDatabase_Patch
        {
            internal static void Postfix()
            {
                Main.ModEntryPoint();
            }
        }
    }
}