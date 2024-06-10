using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Object = UnityEngine.Object;

[BepInPlugin("tacodog.unbeatable.UnAbleMod", "UnAbleMod", "1.0.0")]
// It works for now
// [BepInIncompatibility("tacodog.unbeatable.custombeatmaps")]
public class UnAbleMod : BaseUnityPlugin
{

    private void Awake()
    {
        Logger.LogInfo("Example Mod: Awake!!");

        var patches = new []
        {
            typeof(RemoveBeatAlways),
            typeof(FixRhythmGame),
            typeof(MainMenuChangeTitle),
            typeof(MemorySkip),
            typeof(ReplaceSongsWithInstrumentals)
        };
        foreach (var patch in patches)
        {
            try
            {
                Harmony.CreateAndPatchAll(patch);
            }
            catch (Exception e)
            {
                Logger.LogError($"Patch failed for type {patch.Name}");
                Debug.LogException(e);
            }
        }


        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (scene.name == "BootUp")
            {
                OpeningVideoOverride.ChangeOpeningVideo();
            }

            RemoveBeatAlways.OnSceneLoaded();
            
        };
    }
}

