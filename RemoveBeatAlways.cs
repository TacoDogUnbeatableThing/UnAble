
using System;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using Platformer.Components;
using Rhythm;
using UnityEngine;

public static class RemoveBeatAlways
{
    [HarmonyPatch(typeof(BaseCharacterController3D), "Start")]
    [HarmonyPostfix]
    private static void StartDisable(BaseCharacterController3D __instance)
    {
        __instance.gameObject.SetActive(false);
    }
    [HarmonyPatch(typeof(RhythmPlayer), "Start")]
    [HarmonyPostfix]
    private static void Disable(RhythmPlayer __instance)
    {
        // Disable later so we INITIALIZE our rewired, causes issues otherwise.
        __instance.StartCoroutine(DisableLater(__instance.gameObject, 0.5f));
    }

    private static IEnumerator DisableLater(GameObject gameObject, float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    public static void OnSceneLoaded()
    {
        var beatLookUp = GameObject.Find("Beat Look Up");
        if (beatLookUp != null)
        {
            beatLookUp.SetActive(false);
        }
    }
}
