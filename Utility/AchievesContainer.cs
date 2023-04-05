﻿using System;
using AwesomeAchievements.AchieveLists;
using AwesomeAchievements.Achieves;
using Newtonsoft.Json;
using UnityEngine;

namespace AwesomeAchievements.Utility; 

/* Container realizing assess for all achievements patches */
internal static class AchievesContainer {
    private static Achievement[] _data;
    
    /* Method for initializing this type
     * language - the language in which the achievements should be
     * can throw an exception if there no at least one achievement class for patching */
    public static void Init(string language) {
        var achieveList = GetAchievementList(language);
        _data = new Achievement[achieveList.Length];

        const string classesNamespace = "AwesomeAchievements.Achieves.PatchedAchieves";  //Namespace where classes contained
        for (ushort i = 0; i < achieveList.Length; i++) {
            var achieveJson = achieveList[i];
            Type achieveClass = Type.GetType($"{classesNamespace}.{achieveJson.id}.{achieveJson.id}"); //Get type of the achievement class
            if (achieveClass == null) throw new UnityException("Class of the achievement \"" + achieveJson.id + "\" not found");  //Check class for null
            Achievement achievement = 
                (Achievement)Activator.CreateInstance(achieveClass, achieveJson.name, achieveJson.description); //Get instance of the achievement class
            _data[i] = achievement;  //Add the achievement to array
        }
    }

    /* Method for getting an achievement by its id from container
     * id - the id of the achievement that we need to get
     * returns the instance of the achievement
     * can throw an exception if there no achievement with the same id in the container */
    public static Achievement GetAchievement(string id) {
        foreach (Achievement achievement in _data)  //Cycle through the container data
            if (achievement.Id == id) return achievement;  //Get the achievement with the same id
        
        /* If the achievement with the same id isn't in the container */
        throw new UnityException("Error while getting achievement from container");
    }

    /* Method for deleting an achievement by its id from container
     * id - the id of the achievement that we need to delete
     * can throw an exception if there no achievement with the same id in the container */
    public static void DeleteAchievement(string id) {
        var newData = new Achievement[_data.Length - 1];  //Create new array with a less lenght
        
        /* Cycle trough the array with achievements */
        bool flag = false;
        for (ushort i = 0; i < newData.Length; i++) {
            if (_data[i].Id != id && !flag) {  //Until find an achievement with the same id, just fill in a new array
                newData[i] = _data[i];
                continue;
            }

            /* When find such achievement */
            if (_data[i].Id == id) {
                flag = true;  //Raise the flag
                _data[i].UnpatchAll();  //Unpatch all patches
            }
            
            newData[i] = _data[i + 1];  //Then fill in the array with an offset to the left
        }

        /* Throw an exception if the container doesn't contain the achievement with the same id */
        if (!flag && _data[_data.Length - 1].Id != id)
            throw new UnityException("Can't delete an achievement with the same id from the container");
        
        _data = newData;  //Change the reference of the old array to the new array
    }
    
    /* Method for getting the array of the achievement json objects
     * language - the language in which the achievements should be
     * returns the array with achievement json objects containing their ids, names and descriptions */
    private static AchievementJsonObject[] GetAchievementList(string language) {
        const string resourceNamespace = "AwesomeAchievements.AchieveLists";  //The main namespace where json documents are stored
        ResourceReader listReader = new ResourceReader($"{resourceNamespace}.{language}.json");  //Create a resource reader for json list
        return JsonConvert.DeserializeObject<AchievementJsonArray>(listReader.ReadAllStrings()).data;  //Return deserialized json data
    }
}