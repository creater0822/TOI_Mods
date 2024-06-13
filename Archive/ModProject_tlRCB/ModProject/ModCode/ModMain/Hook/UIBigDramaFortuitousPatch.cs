using System;
using System.IO;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace MOD_tlRCB.Hook
{
    [HarmonyPatch(typeof(UIDramaBigTexture))]
    [HarmonyPatch("UpdateUI")]
    public class UIBigDramaFortuitousPatch
    {
        private static string VideoPath = Path.Combine(g.mod.GetModPathRoot("tlRCB"), "ModAssets", "Video");

        [HarmonyPostfix]
        public static void Postfix(UIDramaBigTexture __instance)
        {
            string[] array = __instance.item.backgroud.Split('|');
            foreach (string bg in array)
            {
                if (DramaReplaceImg(__instance, bg))
                {
                    break;
                }
            }
        }

        public static bool DramaReplaceImg(UIDramaBigTexture __instance, string bg)
        {
            bool flag = false;
            bool isLooping = false;
            string path = null;
            if (bg.StartsWith("videoL", StringComparison.OrdinalIgnoreCase))
            {
                flag = true;
                path = bg.Substring(7);
                isLooping = true;
            }
            else if (bg.StartsWith("video", StringComparison.OrdinalIgnoreCase))
            {
                flag = true;
                path = bg.Substring(6);
            }
            if (!flag)
            {
                return false;
            }
            string text = "";
            FileInfo fileInfo = new FileInfo(Path.Combine(VideoPath, path));
            if (!fileInfo.Exists)
            {
                return false;
            }
            if ((fileInfo.Attributes & FileAttributes.Directory) != 0)
            {
                FileInfo[] files = fileInfo.Directory.GetFiles();
                if (files.Length < 1)
                {
                    return false;
                }
                int num = CommonTool.Random(0, files.Length - 1);
                text = files[num].FullName;
            }
            else
            {
                text = fileInfo.FullName;
            }
            __instance.imgBG1.enabled = false;
            GameObject gameObject = __instance.imgBG1.gameObject;
            GameObject gameObject2 = new GameObject("Video");
            gameObject2.transform.SetParent(gameObject.transform, worldPositionStays: false);
            RectTransform rectTransform = gameObject2.AddComponent<RectTransform>();
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.localPosition = new Vector3(0f, 0f);
            rectTransform.sizeDelta = new Vector2(1920f, 1080f);
            RawImage rawImage = gameObject2.AddComponent<RawImage>();
            VideoPlayer videoPlayer = gameObject2.AddComponent<VideoPlayer>();
            AudioSource audioSource = gameObject2.AddComponent<AudioSource>();
            videoPlayer.isLooping = isLooping;
            videoPlayer.url = text;
            videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            videoPlayer.targetTexture = new RenderTexture(1920, 1080, 0);
            rawImage.texture = videoPlayer.targetTexture;
            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            videoPlayer.Play();
            audioSource.volume = g.sounds.volumeBG;
            Log.Debug($"读取视频 {text}; 音量：{audioSource.volume}");
            return true;
        }
    }
}
