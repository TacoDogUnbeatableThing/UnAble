using System.IO;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Video;

public static class OpeningVideoOverride
{
    public static void ChangeOpeningVideo()
    {
        foreach (var video in Object.FindObjectsOfType<VideoPlayer>())
        {
            string newVidPath = "file:///" + Path.GetFullPath($"BepInEx/plugins/UnAbleMod/animlogo.mov").Replace("\\", "/");
            if (video.clip.name == "LogoSpinny")
            {
                Debug.Log($"REPLACING VIDEO with URL \"{newVidPath}\"");
                video.Stop();
                video.source = VideoSource.Url;
                video.url = newVidPath;
                // video.Prepare();
                video.Play();
            }
            else
            {
                Debug.Log($"NOT replacing video: \"{video.clip.name}\"");
            }
        }
    }
}
