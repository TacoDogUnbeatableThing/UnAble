using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Rhythm;
using UnityEngine;
using Object = UnityEngine.Object;

public static class FixRhythmGame
{
    // [HarmonyPatch(typeof(RhythmController), "FixedUpdate")]
    // [HarmonyPrefix]
    // private static void DisableFixedUpdate(ref bool __runOriginal)
    // {
    //     __runOriginal = false;
    // }
    //
    // [HarmonyPatch(typeof(RhythmController), "Update")]
    // [HarmonyPrefix]
    // private static void DisableUpdate(ref bool __runOriginal)
    // {
    //     __runOriginal = false;
    // }
    
    // No miss: Let the notes fly by
    
    [HarmonyPatch(typeof(RhythmController), "Miss", new Type[0])]
    [HarmonyPrefix, HarmonyPriority(Priority.HigherThanNormal)] 
    private static void MissIgnore1(RhythmController __instance, ref bool __runOriginal)
    {
        __runOriginal = false;
    }

    [HarmonyPatch(typeof(RhythmController), "Miss", typeof(float), typeof(bool))]
    [HarmonyPrefix, HarmonyPriority(Priority.HigherThanNormal)]
    private static void MissIgnore2(RhythmController __instance, ref bool __runOriginal)
    {
        __runOriginal = false;
    }

    // Remove Song bottom UI
    [HarmonyPatch(typeof(RhythmUiTrack), "Start")]
    [HarmonyPostfix]
    private static void DisableSongBottomUI(RhythmUiTrack __instance)
    {
        __instance.gameObject.SetActive(false);
    }
    [HarmonyPatch(typeof(RhythmHealthDisplay), "Start")]
    [HarmonyPostfix]
    private static void DisableSongBottomUI(RhythmHealthDisplay __instance)
    {
        __instance.transform.parent.gameObject.SetActive(false);
    }

    private static void OverridePastHitTime(BaseNote __instance, ref bool __result, ref bool __runOriginal,
        float hitTime)
    {
        // Wait 2 seconds to pass the screen
        __runOriginal = false;
        __result = __instance.controller.PastHitRange(hitTime + 20000);
    }

    [HarmonyPatch(typeof(BaseNote), "PastHitRange", typeof(float))]
    [HarmonyPrefix]
    private static void DeleteWayLayerTime(BaseNote __instance,  ref bool __result, ref bool __runOriginal, float hitTime)
    {
        OverridePastHitTime(__instance, ref __result, ref __runOriginal, hitTime);
    }
    [HarmonyPatch(typeof(BaseNote), "PastHitRange", new Type[0])]
    [HarmonyPrefix]
    private static void DeleteWayLayer(BaseNote __instance,  ref bool __result, ref bool __runOriginal)
    {
        OverridePastHitTime(__instance, ref __result, ref __runOriginal, __instance.hitTime);
    }

    [HarmonyPatch(typeof(BaseNote), "PlayerSwung")]
    [HarmonyPrefix]
    private static void DontSwing(ref bool __result, ref bool __runOriginal)
    {
        __runOriginal = false;
        __result = false;
    }

    [HarmonyPatch(typeof(BaseNote), "GetSongTransformPosition")]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> DontDestroyOnSongTransformPosition(IEnumerable<CodeInstruction> instructions)
    {
        // Boost the progress destroy threshold so we don't destroy ourselves here.
        return new CodeMatcher(instructions)
            .MatchForward(false, 
                new CodeMatch(OpCodes.Ldc_R4))
            .SetOperandAndAdvance(999999999.0f)
            .InstructionEnumeration();
    }

}
