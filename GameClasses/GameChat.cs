﻿using VikingAchievements.Utility;
using HarmonyLib;
using UnityEngine;
using static Chat;

namespace VikingAchievements.GameClasses; 

internal static class GameChat {
    /* A patch to redefine behavior of the chat when it getting a "completing an achievement" message */
    [HarmonyPatch(typeof(Chat), "OnNewChatMessage")]
    private class ChatOnNewChatMessage {
        private const byte GAP_SIZE = 4, 
                           FONT_SIZE = 20;

        /* Patch-method for redefine the behavior of the game chat
         * go - the game object which sent a message
         * text - the text of the message
         * ___m_hideTimer - timer for delaying the hiding of the chat */
        private static bool Prefix(GameObject go, string text, ref float ___m_hideTimer) {
            /* If it's not a "completing an achievement" message, exec the original method */
            if (!text.StartsWith(Announcer.NOT_INSTALLED_ALERT)) return true;
            
            /* If it's a required message, run the code below */
            if (go != Player.m_localPlayer.gameObject) ___m_hideTimer = -3f;  //Get an extra showing chat time, if there isn't our achievement
            string message = text.Replace(Announcer.NOT_INSTALLED_ALERT, "");  //Remove the "not mod installed alert" from the string
            instance.AddString($"<size={GAP_SIZE.ToString()}>\n</size>" + 
                               $"<b><size={FONT_SIZE.ToString()}>{message}</size></b>");  //Print the message into the chat
            return false;  //Don't exec the original method
        }
    }
}