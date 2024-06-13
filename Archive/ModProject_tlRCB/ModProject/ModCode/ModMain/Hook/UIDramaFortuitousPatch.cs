using System;
using System.IO;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace MOD_tlRCB.Hook
{
    [HarmonyPatch(typeof(UIDramaFortuitous))]
    [HarmonyPatch("UpdateUI")]
    public class UIDramaFortuitousPatch
    {
        private static string VideoPath = Path.Combine(g.mod.GetModPathRoot("tlRCB"), "ModAssets", "Video");

        [HarmonyPrefix]
        public static void Prefix(UIDramaFortuitous __instance)
        {
            string[] array = __instance.item.backgroud.Split('|');
            for (int i = 0; i < array.Length; i++)
            {
                string urlByBg = GetUrlByBg(array[i]);
                if (!string.IsNullOrWhiteSpace(urlByBg))
                {
                    __instance.item.backgroud = urlByBg;
                    __instance.imgBG1.sprite = SpriteTool.GetSpriteBigTex("Fortuitous/" + urlByBg);
                    break;
                }
            }
        }

        public static void DramaReplaceImg(UIDramaFortuitous __instance)
        {
            HandleBackground(__instance, __instance.item.backgroud);
        }

        private static string GetUrlByBg(string bg)
        {
            if (!bg.StartsWith("video", StringComparison.OrdinalIgnoreCase))
            {
                return bg;
            }
            string result = "";
            string path = bg.Substring(6);
            FileInfo fileInfo = new FileInfo(Path.Combine(VideoPath, path));
            if (fileInfo.Exists)
            {
                result = fileInfo.FullName;
            }
            else
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(VideoPath, path));
                if (directoryInfo.Exists)
                {
                    FileInfo[] files = directoryInfo.GetFiles();
                    if (files.Length < 1)
                    {
                        return null;
                    }
                    int num = CommonTool.Random(0, files.Length - 1);
                    result = files[num].FullName;
                }
            }
            return result;
        }

        private static bool HandleBackground(UIDramaFortuitous __instance, string bg)
        {
            if (!bg.EndsWith(".mp4", StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }
            __instance.imgBG1.enabled = false;
            GameObject gameObject = __instance.imgBG1.gameObject;
            GameObject gameObject2 = new GameObject("Video");
            gameObject2.transform.SetParent(gameObject.transform, worldPositionStays: false);
            RectTransform rectTransform = gameObject2.AddComponent<RectTransform>();
            rectTransform.pivot = new Vector2(0.5f, 0f);
            rectTransform.localPosition = new Vector3(0f, 0f);
            rectTransform.sizeDelta = rectTransform.GetParentSize();
            RawImage rawImage = gameObject2.AddComponent<RawImage>();
            VideoPlayer videoPlayer = gameObject2.AddComponent<VideoPlayer>();
            AudioSource audioSource = gameObject2.AddComponent<AudioSource>();
            videoPlayer.isLooping = true;
            videoPlayer.url = bg;
            videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            videoPlayer.targetTexture = new RenderTexture(1920, 1080, 0);
            rawImage.texture = videoPlayer.targetTexture;
            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            videoPlayer.Play();
            audioSource.volume = g.sounds.volumeBG;
            return true;
        }

        [HarmonyPostfix]
        public static void Postfix(UIDramaFortuitous __instance)
        {
            DramaReplaceImg(__instance);
        }
    }
}
