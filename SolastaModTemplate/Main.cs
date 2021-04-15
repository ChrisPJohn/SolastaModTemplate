using System;
using System.IO;
using System.Reflection;
using UnityModManagerNet;
using HarmonyLib;
using I2.Loc;
using Newtonsoft.Json.Linq;
using SolastaModApi;

namespace SolastaModTemplate
{
    public class Main
    {
        // [System.Diagnostics.Conditional("DEBUG")]
        public static void Log(string msg)
        {
            if (logger != null) logger.Log(msg);
        }

        public static void Error(Exception ex)
        {
            if (logger != null) logger.Error(ex.ToString());
        }

        public static void Error(string msg)
        {
            if (logger != null) logger.Error(msg);
        }

        public static UnityModManager.ModEntry.ModLogger logger;
        public static bool enabled;

        public static void LoadTranslations()
        {
            var languageSourceData = LocalizationManager.Sources[0];
            var translations = JObject.Parse(File.ReadAllText(UnityModManager.modsPath + @"/SolastaModApi/Translations.json"));
            foreach (var translationKey in translations)
            {
                foreach (var translationLanguage in (JObject)translationKey.Value)
                {
                    var languageIndex = languageSourceData.GetLanguageIndex(translationLanguage.Key);
                    languageSourceData.AddTerm(translationKey.Key).Languages[languageIndex] = translationLanguage.Value.ToString();
                }
            }
        }

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                logger = modEntry.Logger;

                ModBeforeDBReady();

                var harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
            return true;
        }

        [HarmonyPatch(typeof(MainMenuScreen), "RuntimeLoaded")]
        static class MainMenuScreen_RuntimeLoaded_Patch
        {
            static void Postfix()
            {
                ModAfterDBReady();
            }
        }

        // ENTRY POINT IF YOU NEED SERVICE LOCATORS ACCESS
        static void ModBeforeDBReady()
        {
            LoadTranslations();
        }

        // ENTRY POINT IF YOU NEED SAFE DATABASE ACCESS
        static void ModAfterDBReady()
        {
            // var cleric = DatabaseHelper.CharacterClassDefinitions.Cleric;
            // var skeleton = DatabaseHelper.MonsterDefinitions.Skeleton;
        }
    }
}