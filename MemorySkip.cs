using System;
using System.Collections;
using HarmonyLib;
using UnityEngine;

public static class MemorySkip
{
    [HarmonyPatch(typeof(GetMemory), "Start")]
    [HarmonyPostfix]
    private static void SkipMemory(GetMemory __instance)
    {
        __instance.StartCoroutine(SkipLayer(__instance));
    }

    private static IEnumerator SkipLayer(GetMemory __instance)
    {
        yield return new WaitForSeconds(4f);
        __instance.GoToNextScene();
    }
}
