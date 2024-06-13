using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using MelonLoader;
using UnhollowerBaseLib;
using UnityEngine;

namespace LuciferModifier
{
    public static class Tools
    {
        public static T FindObjectOfType<T>() where T : UnityEngine.Object
        {
            Il2CppArrayBase<T> il2CppArrayBase = UnityEngine.Object.FindObjectsOfType<T>();
            if (il2CppArrayBase.Length < 1)
            {
                return null;
            }
            return il2CppArrayBase[0];
        }

        public static List<T> FindObjectsOfType<T>() where T : UnityEngine.Object
        {
            Il2CppArrayBase<T> collection = UnityEngine.Object.FindObjectsOfType<T>();
            return new List<T>(collection);
        }

        public static void AddScale(GameObject go, float maxScale = 1.2f, Action enter = null, Action exit = null, float baseScale = 1f, GameObject triggerObj = null)
        {
            if (triggerObj == null)
            {
                triggerObj = go;
            }
            float scale = 1f;
            UIEventListener uIEventListener = triggerObj.AddComponent<UIEventListener>();
            Action action = delegate
            {
                enter?.Invoke();
                scale = maxScale;
            };
            Action action2 = delegate
            {
                exit?.Invoke();
                scale = 1f;
            };
            uIEventListener.onMouseEnterCall = action;
            uIEventListener.onMouseExitCall = action2;
            uIEventListener.onPressStartCall = action2;
            TimerCoroutine timer = null;
            float size = 1f;
            Action action3 = delegate
            {
                if (go == null)
                {
                    g.timer.Stop(timer);
                }
                else
                {
                    size = Mathf.Lerp(size, scale, Time.deltaTime * 30f);
                    go.transform.localScale = Vector3.one * size * baseScale;
                }
            };
            timer = g.timer.Frame(action3, 1, loop: true);
        }

        public static string GetHttp(string url)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "GET";
                httpWebRequest.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                string result = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                httpWebResponse.Close();
                return result;
            }
            catch (Exception ex)
            {
                MelonLogger.Msg(ex.Message + "\n" + ex.StackTrace);
                return "";
            }
        }

        public static T GetComponentOrAdd<T>(GameObject g) where T : Component
        {
            T val = g.GetComponent<T>();
            if (val == null)
            {
                val = g.AddComponent<T>();
            }
            return val;
        }

        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Open);
                MD5 mD = new MD5CryptoServiceProvider();
                byte[] array = mD.ComputeHash(fileStream);
                fileStream.Close();
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < array.Length; i++)
                {
                    stringBuilder.Append(array[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
            catch (Exception)
            {
            }
            return "00000000000000000000000000000000";
        }

        public static string GZipCompressString(string rawString)
        {
            if (string.IsNullOrEmpty(rawString) || rawString.Length == 0)
            {
                return "";
            }
            byte[] bytes = Encoding.UTF8.GetBytes(rawString.ToString());
            byte[] inArray = Compress(bytes);
            return Convert.ToBase64String(inArray);
        }

        private static byte[] Compress(byte[] rawData)
        {
            MemoryStream memoryStream = new MemoryStream();
            GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, leaveOpen: true);
            gZipStream.Write(rawData, 0, rawData.Length);
            gZipStream.Close();
            return memoryStream.ToArray();
        }

        public static string GetStringByString(string Value)
        {
            return GZipDecompressString(Value);
        }

        public static DataSet GetDatasetByString(string Value)
        {
            DataSet dataSet = new DataSet();
            string s = GZipDecompressString(Value);
            StringReader reader = new StringReader(s);
            dataSet.ReadXml((TextReader)reader);
            return dataSet;
        }

        public static string GZipDecompressString(string zippedString)
        {
            if (string.IsNullOrEmpty(zippedString) || zippedString.Length == 0)
            {
                return "";
            }
            byte[] zippedData = Convert.FromBase64String(zippedString.ToString());
            return Encoding.UTF8.GetString(Decompress(zippedData));
        }

        public static byte[] Decompress(byte[] zippedData)
        {
            MemoryStream stream = new MemoryStream(zippedData);
            GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress);
            MemoryStream memoryStream = new MemoryStream();
            byte[] array = new byte[1024];
            while (true)
            {
                int num = gZipStream.Read(array, 0, array.Length);
                if (num <= 0)
                {
                    break;
                }
                memoryStream.Write(array, 0, num);
            }
            gZipStream.Close();
            return memoryStream.ToArray();
        }

        public static bool SavePhotoFromUrl(string FileName, string Url)
        {
            MelonLogger.Msg("Download pictures   url=" + Url + "   FileName=" + FileName);
            bool result = false;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
                WebResponse response = httpWebRequest.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (!response.ContentType.ToLower().StartsWith("text/"))
                {
                    result = SaveBinaryFile(response, FileName);
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Msg("imgErr:" + ex.Message + "\n" + ex.StackTrace);
            }
            return result;
        }

        private static bool SaveBinaryFile(WebResponse response, string FileName)
        {
            bool result = true;
            byte[] array = new byte[1024];
            try
            {
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }
                Stream stream = File.Create(FileName);
                Stream responseStream = response.GetResponseStream();
                int num;
                do
                {
                    num = responseStream.Read(array, 0, array.Length);
                    if (num > 0)
                    {
                        stream.Write(array, 0, num);
                    }
                }
                while (num > 0);
                stream.Close();
                responseStream.Close();
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}
