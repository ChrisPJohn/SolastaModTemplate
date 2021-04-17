using System;
using System.IO;
using System.Reflection;
using UnityModManagerNet;
using HarmonyLib;
using I2.Loc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using SolastaModApi;
using System.Collections.Generic;

namespace SolastaModTemplate
{
    public class Main
    {
        [Conditional("DEBUG")]
        internal static void Log(string msg) => Logger.Log(msg);

        internal static void Error(Exception ex) => Logger?.Error(ex.ToString());
        internal static void Error(string msg) => Logger?.Error(msg);
        internal static UnityModManager.ModEntry.ModLogger Logger { get; private set; }

        internal static void LoadTranslations()
        {
            var languageSourceData = LocalizationManager.Sources[0];
            var translationsPath = Path.Combine(UnityModManager.modsPath, @"SolastaModTemplate\Translations.json");
            var translations = JObject.Parse(File.ReadAllText(translationsPath));
            foreach (var translationKey in translations)
            {
                foreach (var translationLanguage in (JObject)translationKey.Value)
                {
                    try
                    {
                        var languageIndex = languageSourceData.GetLanguageIndex(translationLanguage.Key);
                        languageSourceData.AddTerm(translationKey.Key).Languages[languageIndex] = translationLanguage.Value.ToString();
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Error($"language {translationLanguage.Key} not installed");
                    }
                    catch (KeyNotFoundException e)
                    {
                        Error($"term {translationKey.Key} not found");
                    }
                }
            }
        }

        internal static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                Logger = modEntry.Logger;

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
        internal static class MainMenuScreen_RuntimeLoaded_Patch
        {
            internal static void Postfix()
            {
                ModAfterDBReady();
            }
        }

        // ENTRY POINT IF YOU NEED SERVICE LOCATORS ACCESS
        internal static void ModBeforeDBReady()
        {
            LoadTranslations();
        }

        // ENTRY POINT IF YOU NEED SAFE DATABASE ACCESS
        internal static void ModAfterDBReady()
        {
            // var cleric = DatabaseHelper.CharacterClassDefinitions.Cleric;
            // var skeleton = DatabaseHelper.MonsterDefinitions.Skeleton;
        }
    }
}