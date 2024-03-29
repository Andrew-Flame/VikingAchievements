﻿using System;
using System.Linq;
using System.Reflection;
using VikingAchievements.Utility;

namespace Tests; 

internal static class AchievementsListTest {
    public static void Run() {
        System.Console.WriteLine("Achievements list test:");
        
        Assembly assembly = Assembly.GetAssembly(typeof(VikingAchievements.Master));
        const string resourceNamespace = "Achieves.Lists",
                     templateResourcePath = $"{resourceNamespace}.template.json";

        var resources = from resource in assembly.GetManifestResourceNames()
                        where resource.Contains(resourceNamespace) && !resource.Contains(templateResourcePath)
                        select resource;

        string templateJson = new ResourceReader(templateResourcePath, assembly).ReadAllStrings();
        int counter = 0;
        foreach (string resourcePath in resources) {
            string resourceJson = new ResourceReader(resourcePath, assembly).ReadAllStrings();
            Out.Write(Eval(templateJson, resourceJson), ++counter, resourcePath.Split('.')[2]);
        }
        
        System.Console.ForegroundColor = ConsoleColor.White;
        System.Console.WriteLine();
    }

    private static bool Eval(string templateJson, string resourceJson) {
        /* Create parsers */
        var templateParser = new JsonParser(templateJson);
        var fileParser = new JsonParser(resourceJson);
        
        /* Create objects using json data from files */
        var templateData = templateParser.ParseAchieves();
        var fileData = fileParser.ParseAchieves();
        
        /* Files must have the same number of objects */
        if (templateParser.AchievesCount != fileParser.AchievesCount) return false;
        
        /* Get enumerators from arrays */
        using var templateDataEnum = templateData.GetEnumerator();
        using var fileDataEnum = fileData.GetEnumerator();

        templateDataEnum.MoveNext();
        fileDataEnum.MoveNext();
        
        /* Compare data using enums */
        do {
            var templateObj = templateDataEnum.Current;
            var fileObj = fileDataEnum.Current;

            if (templateObj.Id != fileObj.Id || fileObj.Name is null or "" || fileObj.Description is null or "") return false;
        } while (templateDataEnum.MoveNext() && fileDataEnum.MoveNext());

        return true;
    }
}