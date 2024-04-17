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
            using (StreamReader sr = new StreamReader(file))
            {
                for ((string line, int matchCount) = ("", 0); (line = sr.ReadLine()) != null;) // multiple variable initialization using touple deconstruction
                {
                    if (line.Contains(pattern))
                    {
                        if (files.Length > 1)
                        {
                            matches += matchCount == 0 ? file + ":" + line : "\n" + file + ":" + line; // don't add a newline when it's the first match
                        }
                        else
                        {
                            matches += matchCount == 0 ? line : "\n" + line;
                        }
                        matchCount++;
                    }
                }
            }
        }
        return matches;
    }
}