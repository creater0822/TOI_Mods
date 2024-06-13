using System;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;

namespace LuciferModifier
{
    public static class CreateUI
    {
        private static Font m_font;
        private static Font m_fontEx;
        private static int sort;

        public static Font font
        {
            get
            {
                if (m_font == null)
                {
                    InitFont();
                }
                return m_font;
            }
        }

        public static Font fontEx
        {
            get
            {
                if (m_fontEx == null)
                {
                    InitFont();
                }
                return m_fontEx;
            }
        }

        static CreateUI()
        {
            sort = 10000;
            try
            {
                InitFont();
            }
            catch (Exception)
            {
                MelonLogger.Msg("Failed to obtain font!");
            }
        }

        private static void InitFont()
        {
            try
            {
                GameObject gameObject = Resources.Load<GameObject>("UI/Item/AchievementItem");
                Transform transform = gameObject.transform;
                m_font = transform.Find("G:goItem/Title").GetComponent<Text>().font;
                m_fontEx = transform.Find("G:goItem/Achievement").GetComponent<Text>().font;
            }
            catch (Exception)
            {
            }
        }

        public static Font GetFont(int id)
        {
            return (id == 1) ? fontEx : font;
        }

        public static GameObject NewCanvas(int sortLayer = int.MinValue)
        {
            GameObject gameObject = new GameObject("Canvas");
            gameObject.AddComponent<RectTransform>();
            gameObject.AddComponent<Canvas>();
            gameObject.AddComponent<CanvasScaler>();
            gameObject.AddComponent<GraphicRaycaster>();
            gameObject.layer = int.MaxValue;
            Canvas component = gameObject.GetComponent<Canvas>();
            component.renderMode = RenderMode.ScreenSpaceOverlay;
            component.sortingOrder = ((sortLayer != int.MinValue) ? sortLayer : (++sort));
            CanvasScaler component2 = gameObject.GetComponent<CanvasScaler>();
            component2.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            Il2CppArrayBase<CanvasScaler> il2CppArrayBase = UnityEngine.Object.FindObjectsOfType<CanvasScaler>();
            component2.referenceResolution = new Vector2(1920f, 1080f);
            return gameObject;
        }

        public static GameObject NewScrollView(Vector2 size = default(Vector2), BarType barType = BarType.Vertical, ContentType contentType = ContentType.VerticalLayout, Vector2 spacing = default(Vector2), Vector2 gridSize = default(Vector2))
        {
            GameObject gameObject = new GameObject("ScrollView");
            gameObject.AddComponent<RectTransform>();
            gameObject.AddComponent<ScrollRect>();
            gameObject.layer = int.MaxValue;
            RectTransform component = gameObject.GetComponent<RectTransform>();
            component.anchoredPosition = new Vector2(0f, 0f);
            component.sizeDelta = ((size == default(Vector2)) ? new Vector2(100f, 100f) : size);
            ScrollRect component2 = gameObject.GetComponent<ScrollRect>();
            component2.horizontal = false;
            component2.scrollSensitivity = 10f;
            GameObject gameObject2 = new GameObject("Viewport");
            gameObject2.AddComponent<RectTransform>();
            gameObject2.AddComponent<RectMask2D>();
            gameObject2.AddComponent<Image>();
            gameObject2.layer = int.MaxValue;
            RectTransform component3 = gameObject2.GetComponent<RectTransform>();
            component3.SetParent(component, worldPositionStays: false);
            component3.anchorMin = new Vector2(0f, 0f);
            component3.anchorMax = new Vector2(1f, 1f);
            component3.anchoredPosition = new Vector2(0f, 0f);
            component3.sizeDelta = new Vector2(-17f, -17f);
            component3.pivot = new Vector2(0f, 1f);
            Image component4 = gameObject2.GetComponent<Image>();
            component4.color = new Color(0f, 0f, 0f, 0f);
            GameObject gameObject3 = new GameObject("Content");
            gameObject3.AddComponent<RectTransform>();
            gameObject3.AddComponent<ContentSizeFitter>();
            gameObject2.layer = int.MaxValue;
            RectTransform component5 = gameObject3.GetComponent<RectTransform>();
            Image image = gameObject3.AddComponent<Image>();
            image.color = new Color(0f, 0f, 0f, 0.01f);
            image.raycastTarget = true;
            component5.SetParent(component3, worldPositionStays: false);
            component5.anchorMin = new Vector2(0f, 1f);
            component5.anchorMax = new Vector2(1f, 1f);
            component5.anchoredPosition = new Vector2(0f, 0f);
            component5.sizeDelta = new Vector2(0f, 0f);
            component5.pivot = new Vector2(0.5f, 1f);
            ContentSizeFitter component6 = component5.GetComponent<ContentSizeFitter>();
            component6.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            switch (contentType)
            {
                case ContentType.VerticalLayout:
                    {
                        VerticalLayoutGroup verticalLayoutGroup = gameObject3.AddComponent<VerticalLayoutGroup>();
                        verticalLayoutGroup.spacing = spacing.y;
                        verticalLayoutGroup.childControlHeight = false;
                        verticalLayoutGroup.childControlWidth = false;
                        verticalLayoutGroup.childForceExpandHeight = false;
                        verticalLayoutGroup.childForceExpandWidth = false;
                        verticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
                        break;
                    }
                case ContentType.Grid:
                    {
                        GridLayoutGroup gridLayoutGroup = gameObject3.AddComponent<GridLayoutGroup>();
                        gridLayoutGroup.cellSize = ((gridSize == default(Vector2)) ? new Vector2(100f, 100f) : gridSize);
                        gridLayoutGroup.spacing = spacing;
                        break;
                    }
            }
            component2.viewport = component3;
            component2.content = component5;
            if (barType == BarType.Vertical)
            {
                GameObject gameObject4 = new GameObject("ScrollbarVertical");
                gameObject4.AddComponent<RectTransform>();
                gameObject4.AddComponent<Image>();
                gameObject4.AddComponent<Scrollbar>();
                gameObject4.layer = int.MaxValue;
                RectTransform component7 = gameObject4.GetComponent<RectTransform>();
                component7.SetParent(component, worldPositionStays: false);
                component7.anchorMin = new Vector2(1f, 0f);
                component7.anchorMax = new Vector2(1f, 1f);
                component7.anchoredPosition = new Vector2(0f, 0f);
                component7.sizeDelta = new Vector2(8f, 0f);
                component7.pivot = new Vector2(1f, 1f);
                Image component8 = gameObject4.GetComponent<Image>();
                component8.sprite = SpriteTool.GetSprite("Common", "ladongtiaodi");
                Scrollbar component9 = gameObject4.GetComponent<Scrollbar>();
                component9.direction = Scrollbar.Direction.BottomToTop;
                GameObject gameObject5 = new GameObject("SlidingArea");
                gameObject5.AddComponent<RectTransform>();
                gameObject5.layer = int.MaxValue;
                RectTransform component10 = gameObject5.GetComponent<RectTransform>();
                component10.SetParent(component7, worldPositionStays: false);
                component10.anchorMin = new Vector2(0f, 0f);
                component10.anchorMax = new Vector2(1f, 1f);
                component10.anchoredPosition = new Vector2(0f, 0f);
                component10.sizeDelta = new Vector2(0f, 0f);
                component10.pivot = new Vector2(0.5f, 0.5f);
                GameObject gameObject6 = new GameObject("Handle");
                gameObject6.AddComponent<RectTransform>();
                gameObject6.AddComponent<Image>();
                gameObject6.layer = int.MaxValue;
                RectTransform component11 = gameObject6.GetComponent<RectTransform>();
                component11.SetParent(component10, worldPositionStays: false);
                component11.anchorMin = new Vector2(0f, 0.5f);
                component11.anchorMax = new Vector2(1f, 1f);
                component11.anchoredPosition = new Vector2(0f, 0f);
                component11.sizeDelta = new Vector2(-1f, -1f);
                component11.pivot = new Vector2(0.5f, 0.5f);
                Image component12 = gameObject6.GetComponent<Image>();
                component12.sprite = SpriteTool.GetSprite("Common", "ladongtiao");
                component12.type = Image.Type.Tiled;
                component9.targetGraphic = component12;
                component9.handleRect = component11;
                component2.verticalScrollbar = component9;
                component2.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
                component2.verticalScrollbarSpacing = -3f;
            }
            return gameObject;
        }

        public static GameObject NewImage(Sprite sprite = null)
        {
            GameObject gameObject = new GameObject("Image");
            gameObject.AddComponent<RectTransform>();
            gameObject.AddComponent<Image>();
            gameObject.layer = int.MaxValue;
            RectTransform component = gameObject.GetComponent<RectTransform>();
            Image component2 = gameObject.GetComponent<Image>();
            if (sprite == null)
            {
                component.sizeDelta = new Vector2(100f, 100f);
            }
            else
            {
                component2.sprite = sprite;
                component2.SetNativeSize();
            }
            return gameObject;
        }

        public static GameObject NewRawImage(Vector2 size = default(Vector2))
        {
            GameObject gameObject = new GameObject("Image");
            gameObject.AddComponent<RectTransform>();
            gameObject.AddComponent<RawImage>();
            gameObject.layer = int.MaxValue;
            RectTransform component = gameObject.GetComponent<RectTransform>();
            Image component2 = gameObject.GetComponent<Image>();
            if (size == default(Vector2))
            {
                component.sizeDelta = new Vector2(100f, 100f);
            }
            else
            {
                component.sizeDelta = size;
            }
            return gameObject;
        }

        public static GameObject NewText(string s = null, Vector2 size = default(Vector2), int fontID = 0)
        {
            GameObject gameObject = new GameObject("Text");
            gameObject.AddComponent<RectTransform>();
            gameObject.AddComponent<Text>();
            gameObject.layer = int.MaxValue;
            RectTransform component = gameObject.GetComponent<RectTransform>();
            Text component2 = gameObject.GetComponent<Text>();
            component2.text = s;
            component2.fontSize = 20;
            component2.font = GetFont(fontID);
            if (size == default(Vector2))
            {
                component.sizeDelta = new Vector2(90f, 30f);
            }
            else
            {
                component.sizeDelta = size;
            }
            return gameObject;
        }

        public static GameObject New()
        {
            GameObject gameObject = new GameObject("");
            gameObject.AddComponent<RectTransform>();
            gameObject.AddComponent<Text>();
            return gameObject;
        }

        public static GameObject NewDropdown(string[] options)
        {
            GameObject gameObject = new GameObject("Dropdown");
            gameObject.AddComponent<RectTransform>();
            gameObject.AddComponent<Dropdown>();
            gameObject.AddComponent<Image>();
            gameObject.layer = int.MaxValue;
            RectTransform component = gameObject.GetComponent<RectTransform>();
            component.sizeDelta = new Vector2(200f, 30f);
            Image component2 = gameObject.GetComponent<Image>();
            component2.color = new Color(1f, 0f, 0f, 1f);
            Dropdown component3 = gameObject.GetComponent<Dropdown>();
            component3.targetGraphic = component2;
            GameObject gameObject2 = new GameObject("Label");
            gameObject2.AddComponent<RectTransform>();
            gameObject2.AddComponent<Text>();
            gameObject2.layer = int.MaxValue;
            RectTransform component4 = gameObject2.GetComponent<RectTransform>();
            component4.SetParent(component, worldPositionStays: false);
            component4.anchorMin = new Vector2(0f, 0f);
            component4.anchorMax = new Vector2(1f, 1f);
            component4.anchoredPosition = new Vector2(-7.5f, 0.5f);
            component4.sizeDelta = new Vector2(-35f, -13f);
            component4.pivot = new Vector2(0.5f, 0.5f);
            Text component5 = gameObject2.GetComponent<Text>();
            component5.fontSize = 14;
            component5.font = font;
            component5.color = new Color(0f, 0f, 0f);
            component5.alignment = TextAnchor.MiddleLeft;
            GameObject gameObject3 = new GameObject("Arrow");
            gameObject3.AddComponent<RectTransform>();
            gameObject3.AddComponent<Image>();
            gameObject3.layer = int.MaxValue;
            RectTransform component6 = gameObject3.GetComponent<RectTransform>();
            component6.SetParent(component, worldPositionStays: false);
            component6.anchorMin = new Vector2(1f, 0.5f);
            component6.anchorMax = new Vector2(1f, 0.5f);
            component6.anchoredPosition = new Vector2(-15f, 0f);
            component6.sizeDelta = new Vector2(20f, 20f);
            component6.pivot = new Vector2(0.5f, 0.5f);
            Image component7 = gameObject3.GetComponent<Image>();
            component7.color = new Color(1f, 1f, 0f, 1f);
            GameObject gameObject4 = NewScrollView(default(Vector2), BarType.Vertical, ContentType.Node);
            gameObject4.SetActive(value: false);
            gameObject4.name = "Template";
            RectTransform component8 = gameObject4.GetComponent<RectTransform>();
            component8.SetParent(component, worldPositionStays: false);
            component8.anchorMin = new Vector2(0f, 0f);
            component8.anchorMax = new Vector2(1f, 0f);
            component8.anchoredPosition = new Vector2(0f, 2f);
            component8.sizeDelta = new Vector2(0f, 150f);
            component8.pivot = new Vector2(0.5f, 1f);
            ScrollRect component9 = gameObject4.GetComponent<ScrollRect>();
            component9.content.sizeDelta = new Vector2(0f, 28f);
            component9.viewport.sizeDelta = new Vector2(-18f, 0f);
            Image image = gameObject4.AddComponent<Image>();
            GameObject gameObject5 = new GameObject("Item");
            gameObject5.AddComponent<RectTransform>();
            gameObject5.AddComponent<Toggle>();
            gameObject5.layer = int.MaxValue;
            RectTransform component10 = gameObject5.GetComponent<RectTransform>();
            component10.SetParent(component9.content, worldPositionStays: false);
            component10.anchorMin = new Vector2(0f, 0.5f);
            component10.anchorMax = new Vector2(1f, 0.5f);
            component10.anchoredPosition = new Vector2(0f, 0f);
            component10.sizeDelta = new Vector2(0f, 20f);
            component10.pivot = new Vector2(0.5f, 0.5f);
            Toggle component11 = gameObject5.GetComponent<Toggle>();
            GameObject gameObject6 = new GameObject("Item Background");
            gameObject6.AddComponent<RectTransform>();
            gameObject6.AddComponent<Image>();
            gameObject6.layer = int.MaxValue;
            RectTransform component12 = gameObject6.GetComponent<RectTransform>();
            component12.SetParent(component10, worldPositionStays: false);
            component12.anchorMin = new Vector2(0f, 0f);
            component12.anchorMax = new Vector2(1f, 1f);
            component12.anchoredPosition = new Vector2(0f, 0f);
            component12.sizeDelta = new Vector2(0f, 0f);
            component12.pivot = new Vector2(0.5f, 0.5f);
            Image component13 = gameObject6.GetComponent<Image>();
            component13.color = new Color(1f, 0f, 0f, 1f);
            GameObject gameObject7 = new GameObject("Item Checkmark");
            gameObject7.AddComponent<RectTransform>();
            gameObject7.AddComponent<Image>();
            gameObject7.layer = int.MaxValue;
            RectTransform component14 = gameObject7.GetComponent<RectTransform>();
            component14.SetParent(component10, worldPositionStays: false);
            component14.anchorMin = new Vector2(0f, 0.5f);
            component14.anchorMax = new Vector2(0f, 0.5f);
            component14.anchoredPosition = new Vector2(10f, 0f);
            component14.sizeDelta = new Vector2(20f, 20f);
            component14.pivot = new Vector2(0.5f, 0.5f);
            Image component15 = gameObject7.GetComponent<Image>();
            component15.color = new Color(1f, 1f, 0f, 1f);
            GameObject gameObject8 = new GameObject("Item Label");
            gameObject8.AddComponent<RectTransform>();
            gameObject8.AddComponent<Text>();
            gameObject8.layer = int.MaxValue;
            RectTransform component16 = gameObject8.GetComponent<RectTransform>();
            component16.SetParent(component10, worldPositionStays: false);
            component16.anchorMin = new Vector2(0f, 0f);
            component16.anchorMax = new Vector2(1f, 1f);
            component16.anchoredPosition = new Vector2(0f, 0f);
            component16.sizeDelta = new Vector2(0f, 0f);
            component16.pivot = new Vector2(0.5f, 0.5f);
            Text component17 = gameObject8.GetComponent<Text>();
            component17.fontSize = 14;
            component17.color = new Color(0f, 0f, 0f);
            component11.targetGraphic = component13;
            component11.graphic = component15;
            component3.targetGraphic = component2;
            component3.template = component8;
            component3.captionText = component5;
            component3.itemText = component17;
            component3.options = new List<Dropdown.OptionData>();
            for (int i = 0; i < options.Length; i++)
            {
                component3.options.Add(new Dropdown.OptionData(options[i]));
            }
            if (options.Length != 0)
            {
                component5.text = options[0];
                component17.text = options[0];
            }
            else
            {
                component5.text = string.Empty;
                component17.text = string.Empty;
            }
            return gameObject;
        }

        public static GameObject NewButton(Action clickAction, Sprite sprite = null)
        {
            if (sprite == null)
            {
                sprite = SpriteTool.GetSprite("Common", "tongyongbutton");
            }
            GameObject gameObject = NewImage(sprite);
            Button button = gameObject.AddComponent<Button>();
            if (clickAction != null)
            {
                button.onClick.AddListener(clickAction);
            }
            return gameObject;
        }

        public static GameObject NewToggle(string text = "", Vector2 textSize = default(Vector2))
        {
            GameObject gameObject = NewImage(SpriteTool.GetSprite("Common", "kuang"));
            Toggle toggle = gameObject.AddComponent<Toggle>();
            GameObject gameObject2 = NewImage(SpriteTool.GetSprite("Common", "xianshigou"));
            gameObject2.transform.SetParent(gameObject.transform, worldPositionStays: false);
            toggle.targetGraphic = gameObject.GetComponent<Image>();
            toggle.graphic = gameObject2.GetComponent<Image>();
            if (!string.IsNullOrWhiteSpace(text))
            {
                GameObject gameObject3 = NewText(text);
                gameObject3.transform.SetParent(gameObject.transform, worldPositionStays: false);
                gameObject3.GetComponent<RectTransform>().sizeDelta = ((textSize == default(Vector2)) ? new Vector2(100f, 30f) : textSize);
                gameObject3.GetComponent<RectTransform>().anchoredPosition = new Vector2(15f + gameObject3.GetComponent<RectTransform>().sizeDelta.x / 2f, 0f);
                gameObject3.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
                gameObject3.GetComponent<Text>().color = Color.black;
            }
            return gameObject;
        }

        public static GameObject NewInputField(Action<string> onValueChange = null, Sprite sprite = null, string placeholder = "")
        {
            if (sprite == null)
            {
                sprite = SpriteTool.GetSprite("Common", "shuxingbg");
            }
            GameObject gameObject = NewImage(sprite);
            InputField inputField = gameObject.AddComponent<InputField>();
            if (onValueChange != null)
            {
                inputField.onValueChange.AddListener(onValueChange);
            }
            RectTransform component = gameObject.GetComponent<RectTransform>();
            component.sizeDelta = new Vector2(330f, 50f);
            Text component2 = NewText(placeholder, gameObject.GetComponent<RectTransform>().sizeDelta).GetComponent<Text>();
            component2.transform.SetParent(gameObject.transform, worldPositionStays: false);
            component2.color = new Color(1f, 1f, 1f, 0.5f);
            component2.alignment = TextAnchor.MiddleCenter;
            component2.GetComponent<RectTransform>().sizeDelta = new Vector2(260f, 50f);
            Text component3 = NewText("", gameObject.GetComponent<RectTransform>().sizeDelta).GetComponent<Text>();
            component3.transform.SetParent(gameObject.transform, worldPositionStays: false);
            component3.color = new Color(1f, 1f, 1f, 1f);
            component3.alignment = TextAnchor.MiddleCenter;
            component3.GetComponent<RectTransform>().sizeDelta = new Vector2(260f, 50f);
            inputField.placeholder = component2;
            inputField.textComponent = component3;
            return gameObject;
        }
    }
}
