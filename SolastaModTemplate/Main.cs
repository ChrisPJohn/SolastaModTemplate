using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityModManagerNet;
using SolastaModApi;
using ModKit;
using ModKit.Utility;

namespace SolastaModTemplate
{
    public class Main
    {
        public static readonly string MOD_FOLDER = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [Conditional("DEBUG")]
        internal static void Log(string msg) => Logger.Log(msg);
        internal static void Error(Exception ex) => Logger?.Error(ex.ToString());
        internal static void Error(string msg) => Logger?.Error(msg);
        internal static void Warning(string msg) => Logger?.Warning(msg);
        internal static UnityModManager.ModEntry.ModLogger Logger { get; private set; }
        internal static ModManager<Core, Settings> Mod;
        internal static MenuManager Menu;
        internal static Settings Settings { get { return Mod.Settings; } }

        internal static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                Logger = modEntry.Logger;

                Mod = new ModManager<Core, Settings>();
                Menu = new MenuManager();
                modEntry.OnToggle = OnToggle;

                Translations.Load(MOD_FOLDER);
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool enabled)
        {
            if (enabled)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Mod.Enable(modEntry, assembly);
                Menu.Enable(modEntry, assembly);
            }
            else
            {
                Menu.Disable(modEntry);
                Mod.Disable(modEntry, false);
                ReflectionCache.Clear();
            }
            return true;
        }

        internal static void OnGameReady()
        {
            // example: use the ModApi to get a skeleton blueprint
            //
            var skeleton = DatabaseHelper.MonsterDefinitions.Skeleton;

            // example: how to add TEXTS to the game right
            //
            // . almost every game blueprint has a GuiPresentation attribute
            // . GuiPresentation has a Title and a Description
            // . Create an entry in Translations-en.txt for those (tab separated)
            // . Refer to those entries when assigning values to these attributes
            //
            // . DON'T FORGET TO CLEAN UP THIS EXAMPLE AND Translations-en.txt file
            // . ugly things will happen if you don't
            //
            skeleton.GuiPresentation.Title = "SolastaModTemplate/&FancySkeletonTitle";
            skeleton.GuiPresentation.Description = "SolastaModTemplate/&FancySkeletonDescription";
        }
    }
}