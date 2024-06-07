
using System;
using System.Collections.Generic;
using System.IO;
using FMOD.Studio;
using HarmonyLib;
using Rhythm;
using UnityEngine;

public static class ReplaceSongsWithInstrumentals
{
    /*
     *
     * NOISZ - True -> audio.mp3
[Info   : Unity Log] PATH: NOISZ - Done In Love -> audio.mp3
[Info   : Unity Log] PATH: Low -> audio.mp3
[Info   : Unity Log] PATH: FOREVER WHEN -> audio.mp3
[Info   : Unity Log] PATH: Waiting Lumena -> audio.mp3
[Info   : Unity Log] PATH: PROPERRHYTHM MUST DIE -> audio.mp3
[Info   : Unity Log] PATH: Forever Now - DOG_NOISE Remix -> audio.mp3
[Info   : Unity Log] PATH: EMPTY DIARY DANTE -> audio.mp3
[Info   : Unity Log] PATH: Familiar -> audio.mp3
[Info   : Unity Log] PATH: Waiting -> audio.mp3
[Info   : Unity Log] PATH: FOREVER NOW -> audio.mp3
[Info   : Unity Log] PATH: Mirror -> audio.mp3
[Info   : Unity Log] PATH: PROPERRHYTHM -> audio.mp3
[Info   : Unity Log] PATH: EMPTY DIARY -> audio.mp
     */
        // Inject this into our beatmap parsing to replace our beatmap with the override beatmap
        private static void OverrideBeatmapAudio(string songName, ref string audioKey)
        {
            int splitIndex = songName.IndexOf("/", StringComparison.Ordinal);
            if (splitIndex != -1)
            {
                songName = songName.Substring(0, splitIndex);
            }

            string prevAudioKey = audioKey;

            string theoreticalReplacementPath = Path.GetFullPath($"BepInEx/plugins/UnAbleMod/SongsInstrumental/{songName}.mp3").Replace("\\", "/");

            if (File.Exists(theoreticalReplacementPath))
            {
                audioKey = theoreticalReplacementPath;
                Debug.Log($"REPLACED {songName} -> {theoreticalReplacementPath}");
            }
            else
            {
                Debug.Log($"NO REPLACEMENT FOUND: {songName} -> {theoreticalReplacementPath}");
            }
        }

        [HarmonyPatch(typeof(BeatmapParser), "ParseBeatmap", new Type[0])]
        [HarmonyPostfix]
        private static void ParseBeatmapInstance(BeatmapParser __instance)
        {
            Debug.Log("BBBBB");
            OverrideBeatmapAudio(__instance.beatmapPath, ref __instance.audioKey);
            __instance.beatmap.general.audioFilename = __instance.audioKey;
        }

        [HarmonyPatch(
            typeof(BeatmapParser), "ParseBeatmap",
            new[]{typeof(BeatmapIndex), typeof(string), typeof(BeatmapInfo), typeof(Beatmap), typeof(string)},
            new [] {ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out, ArgumentType.Out, ArgumentType.Out})
        ]
        [HarmonyPrefix]
        private static void ParseBeatmapStatic(BeatmapIndex beatmapIndex, string beatmapPath, out BeatmapInfo beatmapInfo, out Beatmap beatmap, out string songName, ref bool __runOriginal)
        {
            __runOriginal = false;

            // Ctrl + C, Ctrl + V go BRRRR
            BeatmapParserEngine beatmapParserEngine = new BeatmapParserEngine();
            beatmapInfo = beatmapIndex.FindByPath(beatmapPath, out songName);
            beatmap = ScriptableObject.CreateInstance<Beatmap>();
            string text = beatmapInfo.text;
            ref Beatmap local = ref beatmap;
            beatmapParserEngine.ReadBeatmap(text, ref local);

            Debug.Log("AAAAA");
            OverrideBeatmapAudio(beatmapPath, ref songName);
            beatmap.general.audioFilename = songName;
        }

        [HarmonyPatch(typeof(RhythmTracker), "PrepareInstance")]
        [HarmonyPrefix]
        private static void AllowPlayingFromFile(EventInstance instance, ref PlaySource source, string key)
        {
            OverrideBeatmapAudio(key, ref key);
            Debug.Log($"PLAY: {key}");
            if (key.Contains("SongsInstrumental"))
            {
                Debug.Log("REPLACING file");
                source = PlaySource.FromFile;
            }
        }
}
