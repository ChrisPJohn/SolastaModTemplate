using System;
using System.IO;
using I2.Loc;

namespace SolastaModTemplate
{
    class Translations
    {
        internal static void Load(string fromFolder)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(fromFolder);
            FileInfo[] files = directoryInfo.GetFiles($"Translations-??.txt");

            foreach (var file in files)
            {
                var filename = Path.Combine(fromFolder, file.Name);
                var code = file.Name.Substring(13, 2);
                var languageSourceData = LocalizationManager.Sources[0];
                var languageIndex = languageSourceData.GetLanguageIndexFromCode(code);

                if (languageIndex < 0)
                    Main.Error($"language {code} not currently loaded.");
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
                            }
                            catch
                            {
                                Main.Error($"invalid translation line \"{line}\".");
                                continue;
                            }
                            if (languageSourceData.ContainsTerm(term))
                            {
                                languageSourceData.RemoveTerm(term);
                                Main.Warning($"official game term {term} was overwritten with \"{text}\"");
                            }
                            languageSourceData.AddTerm(term).Languages[languageIndex] = text;
                        }
                    }
            }
        }
    }
}