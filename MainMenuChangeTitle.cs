using System;
using HarmonyLib;
using Platformer.Components;
using TMPro;
using UnityEngine;

public static class MainMenuChangeTitle
{
    [HarmonyPatch(typeof(WhiteLabelMainMenu), "Start")]
    [HarmonyPostfix]
    private static void ChangeTitle(WhiteLabelMainMenu __instance)
    {
        foreach (var txt in __instance.GetComponentsInChildren<TMP_Text>(true))
        {
            Update(txt);
        }
    }

    private static void Update(TMP_Text txt)
    {
        txt.text = txt.text.Replace("UNBEATABLE", "UN<color=#00000000>BEAT</color>ABLE");
        txt.text = txt.text.Replace("unbeatable", "un<color=#00000000>beat</color>able");
        txt.text = txt.text.Replace("Unbeatable", "Un<color=#00000000>beat</color>able");
    }
    
    
    [HarmonyPatch(typeof(WhiteLabelMainMenu), "LevelSelect")]
    [HarmonyPostfix]
    private static void ChangeDifficulty(WhiteLabelMainMenu __instance)
    {
        Update(__instance.DifficultyNameA);
        Update(__instance.DifficultyNameB);
    }
}
