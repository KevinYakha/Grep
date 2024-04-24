using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Xunit.Sdk;

public static class Grep
{
    private enum Flags
    {
        addLineNumber,
        onlyFileNames,
        caseInsensitive,
        invertSearch,
        onlyExactLineMatch,
        COUNT
    }
    public static string Match(string pattern, string flags, string[] files)
    {
        bool[] flagsArray = ProcessFlags(flags);
        string matches = "";
        int matchCount = 0;
        foreach (string file in files)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                for ((string line, int lineNumber) = ("",  1); (line = sr.ReadLine()) != null; lineNumber++) // multiple variable initialization using touple deconstruction
                {
                    if (line.Contains(pattern))
                    {
                        if (files.Length > 1)
                            matches = FormatMatch(matches, file, line, lineNumber, matchCount, flagsArray);
                        else if (flagsArray[(int)Flags.onlyFileNames]) // return file name if onlyFileNames flag was set and any match was found
                            return matches = file;
                        else
                            matches = FormatMatch(matches, line, lineNumber, matchCount, flagsArray);
                        matchCount++;
                    }
                }
            }
        }
        return matches;
    }
    private static string FormatMatch(string matches, string fileName, string line, int lineNumber, int matchCount, bool[] flags)
    {
        if (flags[(int)Flags.onlyFileNames])
            matches += matches.Contains(fileName) ? "" : matchCount == 0 ? fileName : $"\n{fileName}";
        else if (flags[(int)Flags.addLineNumber])
            matches += matchCount == 0 ? $"{fileName}:{lineNumber}:{line}" : $"\n{fileName}:{lineNumber}:{line}"; // don't add a newline when it's the first match
        else
            matches += matchCount == 0 ? $"{fileName}:{line}" : $"\n{fileName}:{line}";
        return matches;
    }
    private static string FormatMatch(string matches, string line, int lineNumber, int matchCount, bool[] flags)
    {
        if (flags[(int)Flags.addLineNumber])
            matches += matchCount == 0 ? $"{lineNumber}:{line}" : $"\n{lineNumber}:{line}"; // don't add a newline when it's the first match
        else
            matches += matchCount == 0 ? $"{line}" : $"\n{line}";
        return matches;
    }
    private static bool[] ProcessFlags(string flags)
    {
        bool[] flagsArray = new bool[(int)Flags.COUNT];
        foreach (string flag in flags.Split(' '))
        {
            switch (flag)
            {
                case "-n":
                    flagsArray[(int)Flags.addLineNumber] = true;
                    break;
                case "-l":
                    flagsArray[(int)Flags.onlyFileNames] = true;
                    break;
                case "-i":
                    flagsArray[(int)Flags.caseInsensitive] = true;
                    break;
                case "-v":
                    flagsArray[(int)Flags.invertSearch] = true;
                    break;
                case "-x":
                    flagsArray[(int)Flags.onlyExactLineMatch] = true;
                    break;
                default:
                    break;
            }
        }
        return flagsArray;
    }
}