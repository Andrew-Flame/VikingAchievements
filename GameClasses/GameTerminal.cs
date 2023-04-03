﻿// ReSharper disable ObjectCreationAsStatement
using System.Linq;
using AwesomeAchievements.TerminalCommands;
using HarmonyLib;
using static Terminal;

namespace AwesomeAchievements.GameClasses; 

internal static class GameTerminal {
    [HarmonyPatch(typeof(Terminal), "Awake")]
    private static class TerminalAwake {
        private static void Postfix() {
            /* Add new console commands */
            new ConsoleCommand("achievement-add", "add achievement to the current character",
                               AchieveHandler.AddAchieves, 
                               isCheat: true, isSecret: false,
                               optionsFetcher: AchieveHandler.GetAchievesForList().ToList);
            new ConsoleCommand("achievement-remove", "remove achievement from the current character",
                               AchieveHandler.RemoveAchieves,
                               isCheat: true, isSecret: false,
                               optionsFetcher: AchieveHandler.GetAchievesForList().ToList);
        }
    }
}