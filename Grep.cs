using System;
using System.Collections.Generic;
using System.IO;

using Xunit.Sdk;

public static class Grep
{
    public static string Match(string pattern, string flags, string[] files)
    {
        string matches = "";
        foreach (string file in files)
        {
            using (StreamReader sr = new StreamReader(files[0]))
            {
                for (string line; (line = sr.ReadLine()) != null;)
                {
                    if (line.Contains(pattern))
                        matches += matches == "" ? line : "\n" + line; // only add a newline when it's the first match
                }
            }
        }
        return matches;
    }
}