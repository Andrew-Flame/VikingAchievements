﻿using AwesomeAchievements.Utility;
using UnityEngine;

namespace AwesomeAchievements.AchieveTab; 

/* Class for work with achievement tab */  
internal static class TabManager {
    
    /* Method for initializing this type */
    public static void Init() {
        /* Get required game objects */
        GameObject rootInventory = InventoryGui.instance.gameObject;
        GameObject infoPanel = InventoryGui.instance.m_infoPanel.gameObject;

        LogInfo.Log("An achievement tab manager has been initialized");
    }
}