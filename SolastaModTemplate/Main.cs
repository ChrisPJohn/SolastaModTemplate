﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityModManagerNet;
using HarmonyLib;
using I2.Loc;
using SolastaModApi;

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

        internal static void LoadTranslations()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(MOD_FOLDER);
            FileInfo[] files = directoryInfo.GetFiles($"Translations-??.txt");

            foreach (var file in files)
            {
                var filename = Path.Combine(MOD_FOLDER, file.Name);
                var code = file.Name.Substring(13, 2);
                var languageSourceData = LocalizationManager.Sources[0];
                var languageIndex = languageSourceData.GetLanguageIndexFromCode(code);

                if (languageIndex < 0)
                    Error($"language {code} not currently loaded.");
                else
                    using (var sr = new StreamReader(filename))
                    {
                        String line, term, text;
                        while ((line = sr.ReadLine()) != null)
                        {
                            try
                            {
                                var splitted = line.Split(new[] { '\t', ' ' }, 2);
                                term = splitted[0];
                                text = splitted[1];
                            } catch
                            {
                                Error($"invalid translation line \"{line}\".");
                                continue;
                            }
                            if (languageSourceData.ContainsTerm(term))
                            {
                                languageSourceData.RemoveTerm(term);
                                Warning($"official game term {term} was overwritten with \"{text}\"");
                            }
                            languageSourceData.AddTerm(term).Languages[languageIndex] = text;
                        }
                    }
            }
        }

        internal static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                Logger = modEntry.Logger;

                LoadTranslations();

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

        internal static void ModEntryPoint()
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