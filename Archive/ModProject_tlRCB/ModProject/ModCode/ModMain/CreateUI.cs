using System;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_tlRCB
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
                MelonLogger.Msg("获取字体失败！");
            }
        }

        private static void InitFont()
        {
            try
            {
                Transform transform = Resources.Load<GameObject>("UI/Item/AchievementItem").transform;
                m_font = transform.Find("G:goItem/Title").GetComponent<Text>().font;
                m_fontEx = transform.Find("G:goItem/Achievement").GetComponent<Text>().font;
            }
            catch (Exception)
            {
            }
        }

        public static Font GetFont(int id)
        {
            if (id != 1)
            {
                return font;
            }
            return fontEx;
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
            int sortingOrder = ((sortLayer != int.MinValue) ? sortLayer : (++sort));
            component.sortingOrder = sortingOrder;
            CanvasScaler component2 = gameObject.GetComponent<CanvasScaler>();
            component2.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            UnityEngine.Object.FindObjectsOfType<CanvasScaler>();
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
            gameObject2.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            GameObject gameObject3 = new GameObject("Content");
            gameObject3.AddComponent<RectTransform>();
            gameObject3.AddComponent<ContentSizeFitter>();
            gameObject2.layer = int.MaxValue;
            RectTransform component4 = gameObject3.GetComponent<RectTransform>();
            Image image = gameObject3.AddComponent<Image>();
            image.color = new Color(0f, 0f, 0f, 0.01f);
            image.raycastTarget = true;
            component4.SetParent(component3, worldPositionStays: false);
            component4.anchorMin = new Vector2(0f, 1f);
            component4.anchorMax = new Vector2(1f, 1f);
            component4.anchoredPosition = new Vector2(0f, 0f);
            component4.sizeDelta = new Vector2(0f, 0f);
            component4.pivot = new Vector2(0.5f, 1f);
            component4.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            switch (contentType)
            {
                case ContentType.Grid:
                    {
                        GridLayoutGroup gridLayoutGroup = gameObject3.AddComponent<GridLayoutGroup>();
                        gridLayoutGroup.cellSize = ((gridSize == default(Vector2)) ? new Vector2(100f, 100f) : gridSize);
                        gridLayoutGroup.spacing = spacing;
                        break;
                    }
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
            }
            component2.viewport = component3;
            component2.content = component4;
            if (barType == BarType.Vertical)
            {
                GameObject gameObject4 = new GameObject("ScrollbarVertical");
                gameObject4.AddComponent<RectTransform>();
                gameObject4.AddComponent<Image>();
                gameObject4.AddComponent<Scrollbar>();
                gameObject4.layer = int.MaxValue;
                RectTransform component5 = gameObject4.GetComponent<RectTransform>();
                component5.SetParent(component, worldPositionStays: false);
                component5.anchorMin = new Vector2(1f, 0f);
                component5.anchorMax = new Vector2(1f, 1f);
                component5.anchoredPosition = new Vector2(0f, 0f);
                component5.sizeDelta = new Vector2(8f, 0f);
                component5.pivot = new Vector2(1f, 1f);
                gameObject4.GetComponent<Image>().sprite = SpriteTool.GetSprite("Common", "ladongtiaodi");
                Scrollbar component6 = gameObject4.GetComponent<Scrollbar>();
                component6.direction = Scrollbar.Direction.BottomToTop;
                GameObject gameObject5 = new GameObject("SlidingArea");
                gameObject5.AddComponent<RectTransform>();
                gameObject5.layer = int.MaxValue;
                RectTransform component7 = gameObject5.GetComponent<RectTransform>();
                component7.SetParent(component5, worldPositionStays: false);
                component7.anchorMin = new Vector2(0f, 0f);
                component7.anchorMax = new Vector2(1f, 1f);
                component7.anchoredPosition = new Vector2(0f, 0f);
                component7.sizeDelta = new Vector2(0f, 0f);
                component7.pivot = new Vector2(0.5f, 0.5f);
                GameObject gameObject6 = new GameObject("Handle");
                gameObject6.AddComponent<RectTransform>();
                gameObject6.AddComponent<Image>();
                gameObject6.layer = int.MaxValue;
                RectTransform component8 = gameObject6.GetComponent<RectTransform>();
                component8.SetParent(component7, worldPositionStays: false);
                component8.anchorMin = new Vector2(0f, 0.5f);
                component8.anchorMax = new Vector2(1f, 1f);
                component8.anchoredPosition = new Vector2(0f, 0f);
                component8.sizeDelta = new Vector2(-1f, -1f);
                component8.pivot = new Vector2(0.5f, 0.5f);
                Image component9 = gameObject6.GetComponent<Image>();
                component9.sprite = SpriteTool.GetSprite("Common", "ladongtiao");
                component9.type = Image.Type.Tiled;
                component6.targetGraphic = component9;
                component6.handleRect = component8;
                component2.verticalScrollbar = component6;
                component2.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
                component2.verticalScrollbarSpacing = -3f;
            }
            return gameObject;
        }

        public static GameObject NewImage(Sprite sprite = null, string name = "Image")
        {
            GameObject gameObject = new GameObject(name);
            gameObject.AddComponent<RectTransform>();
            gameObject.AddComponent<Image>();
            gameObject.layer = int.MaxValue;
            RectTransform component = gameObject.GetComponent<RectTransform>();
            Image component2 = gameObject.GetComponent<Image>();
            if (sprite == null)
            {
                component.sizeDelta = new Vector2(100f, 100f);
                return gameObject;
            }
            component2.sprite = sprite;
            component2.SetNativeSize();
            return gameObject;
        }

        public static GameObject NewRawImage(Vector2 size = default(Vector2))
        {
            GameObject gameObject = new GameObject("Image");
            gameObject.AddComponent<RectTransform>();
            gameObject.AddComponent<RawImage>();
            gameObject.layer = int.MaxValue;
            RectTransform component = gameObject.GetComponent<RectTransform>();
            gameObject.GetComponent<Image>();
            component.sizeDelta = ((!(size == default(Vector2))) ? size : new Vector2(100f, 100f));
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
            component2.color = Color.black;
            component.sizeDelta = ((!(size == default(Vector2))) ? size : new Vector2(90f, 30f));
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
            gameObject3.GetComponent<Image>().color = new Color(1f, 1f, 0f, 1f);
            GameObject gameObject4 = NewScrollView(default(Vector2), BarType.Vertical, ContentType.Node);
            gameObject4.SetActive(value: false);
            gameObject4.name = "Template";
            RectTransform component7 = gameObject4.GetComponent<RectTransform>();
            component7.SetParent(component, worldPositionStays: false);
            component7.anchorMin = new Vector2(0f, 0f);
            component7.anchorMax = new Vector2(1f, 0f);
            component7.anchoredPosition = new Vector2(0f, 2f);
            component7.sizeDelta = new Vector2(0f, 150f);
            component7.pivot = new Vector2(0.5f, 1f);
            ScrollRect component8 = gameObject4.GetComponent<ScrollRect>();
            component8.content.sizeDelta = new Vector2(0f, 28f);
            component8.viewport.sizeDelta = new Vector2(-18f, 0f);
            gameObject4.AddComponent<Image>();
            GameObject gameObject5 = new GameObject("Item");
            gameObject5.AddComponent<RectTransform>();
            gameObject5.AddComponent<Toggle>();
            gameObject5.layer = int.MaxValue;
            RectTransform component9 = gameObject5.GetComponent<RectTransform>();
            component9.SetParent(component8.content, worldPositionStays: false);
            component9.anchorMin = new Vector2(0f, 0.5f);
            component9.anchorMax = new Vector2(1f, 0.5f);
            component9.anchoredPosition = new Vector2(0f, 0f);
            component9.sizeDelta = new Vector2(0f, 20f);
            component9.pivot = new Vector2(0.5f, 0.5f);
            Toggle component10 = gameObject5.GetComponent<Toggle>();
            GameObject gameObject6 = new GameObject("Item Background");
            gameObject6.AddComponent<RectTransform>();
            gameObject6.AddComponent<Image>();
            gameObject6.layer = int.MaxValue;
            RectTransform component11 = gameObject6.GetComponent<RectTransform>();
            component11.SetParent(component9, worldPositionStays: false);
            component11.anchorMin = new Vector2(0f, 0f);
            component11.anchorMax = new Vector2(1f, 1f);
            component11.anchoredPosition = new Vector2(0f, 0f);
            component11.sizeDelta = new Vector2(0f, 0f);
            component11.pivot = new Vector2(0.5f, 0.5f);
            Image component12 = gameObject6.GetComponent<Image>();
            component12.color = new Color(1f, 0f, 0f, 1f);
            GameObject gameObject7 = new GameObject("Item Checkmark");
            gameObject7.AddComponent<RectTransform>();
            gameObject7.AddComponent<Image>();
            gameObject7.layer = int.MaxValue;
            RectTransform component13 = gameObject7.GetComponent<RectTransform>();
            component13.SetParent(component9, worldPositionStays: false);
            component13.anchorMin = new Vector2(0f, 0.5f);
            component13.anchorMax = new Vector2(0f, 0.5f);
            component13.anchoredPosition = new Vector2(10f, 0f);
            component13.sizeDelta = new Vector2(20f, 20f);
            component13.pivot = new Vector2(0.5f, 0.5f);
            Image component14 = gameObject7.GetComponent<Image>();
            component14.color = new Color(1f, 1f, 0f, 1f);
            GameObject gameObject8 = new GameObject("Item Label");
            gameObject8.AddComponent<RectTransform>();
            gameObject8.AddComponent<Text>();
            gameObject8.layer = int.MaxValue;
            RectTransform component15 = gameObject8.GetComponent<RectTransform>();
            component15.SetParent(component9, worldPositionStays: false);
            component15.anchorMin = new Vector2(0f, 0f);
            component15.anchorMax = new Vector2(1f, 1f);
            component15.anchoredPosition = new Vector2(0f, 0f);
            component15.sizeDelta = new Vector2(0f, 0f);
            component15.pivot = new Vector2(0.5f, 0.5f);
            Text component16 = gameObject8.GetComponent<Text>();
            component16.fontSize = 14;
            component16.color = new Color(0f, 0f, 0f);
            component10.targetGraphic = component12;
            component10.graphic = component14;
            component3.targetGraphic = component2;
            component3.template = component7;
            component3.captionText = component5;
            component3.itemText = component16;
            component3.options = new List<Dropdown.OptionData>();
            for (int i = 0; i < options.Length; i++)
            {
                component3.options.Add(new Dropdown.OptionData(options[i]));
            }
            if (options.Length != 0)
            {
                component5.text = options[0];
                component16.text = options[0];
            }
            else
            {
                component5.text = string.Empty;
                component16.text = string.Empty;
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

        public static GameObject NewButton(string name, Sprite sprite = null, string key = null)
        {
            if (sprite == null)
            {
                sprite = SpriteTool.GetSprite("Common", "tongyongbutton");
            }
            GameObject gameObject = NewImage(sprite, key);
            gameObject.AddComponent<Button>();
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100f, 30f);
            GameObject gameObject2 = NewText(name);
            gameObject2.transform.SetParent(gameObject.transform);
            gameObject2.GetComponent<RectTransform>().localPosition = new Vector2(0f, 0f);
            Text component = gameObject2.GetComponent<Text>();
            component.alignment = TextAnchor.MiddleCenter;
            component.color = Color.black;
            component.fontSize = 15;
            return gameObject;
        }

        public static GameObject NewButton(string txt, Action clickAction, Sprite sprite = null, string key = null)
        {
            if (sprite == null)
            {
                sprite = SpriteTool.GetSprite("Common", "tongyongbutton");
            }
            GameObject gameObject = NewImage(sprite, key);
            Button button = gameObject.AddComponent<Button>();
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100f, 30f);
            GameObject gameObject2 = NewText(txt);
            gameObject2.transform.SetParent(gameObject.transform);
            gameObject2.GetComponent<RectTransform>().localPosition = new Vector2(0f, 0f);
            Text component = gameObject2.GetComponent<Text>();
            component.alignment = TextAnchor.MiddleCenter;
            component.color = Color.black;
            component.fontSize = 15;
            if (clickAction != null)
            {
                button.onClick.AddListener(clickAction);
            }
            return gameObject;
        }

        public static HorizontalLayoutGroup NewHorizontalLayoutGroup(string name = null)
        {
            return new GameObject(name ?? "HorizontalLayoutGroup").AddComponent<HorizontalLayoutGroup>();
        }

        public static VerticalLayoutGroup NewVerticalLayoutGroup(string name = null)
        {
            return new GameObject(name ?? "VerticalLayoutGroup").AddComponent<VerticalLayoutGroup>();
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
                gameObject3.GetComponent<RectTransform>().anchoredPosition = new Vector2((float)(15.0 + (double)gameObject3.GetComponent<RectTransform>().sizeDelta.x / 2.0), 0f);
                gameObject3.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
                gameObject3.GetComponent<Text>().color = Color.black;
            }
            return gameObject;
        }

        public static GameObject NewToggle2(string str_text = "", string skyTip = null)
        {
            GameObject gameObject = new GameObject("toggle");
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            Transform transform = gameObject.transform;
            rectTransform.sizeDelta = new Vector2(400f, 50f);
            GameObject gameObject2 = NewImage(SpriteTool.GetSprite("Common", "kuang"));
            gameObject2.transform.SetParent(transform);
            gameObject2.GetComponent<RectTransform>().sizeDelta = new Vector2(25f, 25f);
            Toggle toggle = gameObject2.AddComponent<Toggle>();
            GameObject gameObject3 = NewImage(SpriteTool.GetSprite("Common", "xianshigou"));
            gameObject3.GetComponent<RectTransform>().sizeDelta = new Vector2(25f, 25f);
            gameObject3.transform.SetParent(gameObject2.transform, worldPositionStays: false);
            toggle.targetGraphic = gameObject2.GetComponent<Image>();
            toggle.graphic = gameObject3.GetComponent<Image>();
            gameObject2.GetComponent<RectTransform>().localPosition = new Vector2(50f, 0f);
            GameObject gameObject4 = NewText(str_text, new Vector2(300f, 50f));
            gameObject4.transform.SetParent(transform);
            gameObject4.GetComponent<RectTransform>().localPosition = new Vector2(0f, 0f);
            gameObject4.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200f, 0f);
            gameObject4.GetComponent<Text>().color = Color.black;
            gameObject4.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
            if (!string.IsNullOrWhiteSpace(skyTip))
            {
                gameObject4.transform.AddSkyTip(skyTip);
            }
            rectTransform.pivot = new Vector2(0f, 1f);
            return gameObject;
        }

        public static GameObject NewSlider()
        {
            GameObject gameObject = new GameObject("huadongtiao");
            gameObject.AddComponent<RectTransform>();
            gameObject.AddComponent<Slider>();
            GameObject gameObject2 = NewImage(SpriteTool.GetSprite("Common", "ladongtiaodi"), "Background");
            RectTransform component = gameObject2.GetComponent<RectTransform>();
            gameObject2.GetComponent<Image>().type = Image.Type.Simple;
            component.sizeDelta = new Vector2(200f, 15f);
            gameObject2.transform.SetParent(gameObject.transform);
            gameObject2.GetComponent<RectTransform>().localPosition = new Vector2(0f, 0f);
            return gameObject;
        }

        public static GameObject NewTextSliderInput2(string str_text, GameObject gameSlider, string skyTip = null)
        {
            GameObject gameObject = new GameObject("kuang");
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(400f, 50f);
            GameObject gameObject2 = NewText(str_text, new Vector2(300f, 50f));
            gameObject2.transform.SetParent(gameObject.transform);
            gameObject2.GetComponent<RectTransform>().localPosition = new Vector2(0f, 0f);
            gameObject2.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200f, 0f);
            gameObject2.GetComponent<Text>().color = Color.black;
            gameObject2.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
            if (!string.IsNullOrWhiteSpace(skyTip))
            {
                gameObject2.transform.AddSkyTip(skyTip);
            }
            rectTransform.pivot = new Vector2(0f, 1f);
            return gameObject;
        }

        public static GameObject NewTextSliderInput(string str_text, GameObject gameSlider, string skyTip = null)
        {
            GameObject gameObject = new GameObject("kuang");
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(400f, 50f);
            GameObject gameObject2 = NewInputField();
            InputField component = gameObject2.GetComponent<InputField>();
            component.readOnly = true;
            component.enabled = false;
            component.text = "";
            gameObject2.transform.SetParent(gameObject.transform);
            gameObject2.GetComponent<RectTransform>().localPosition = new Vector2(0f, 0f);
            gameObject2.GetComponent<RectTransform>().anchoredPosition = new Vector2(175f, 0f);
            component.contentType = InputField.ContentType.IntegerNumber;
            UnityEngine.Object.Instantiate(gameSlider, gameObject2.transform, worldPositionStays: true).GetComponent<RectTransform>().localPosition = new Vector2(0f, 0f);
            GameObject gameObject3 = NewText(str_text, new Vector2(300f, 50f));
            gameObject3.transform.SetParent(gameObject.transform);
            gameObject3.GetComponent<RectTransform>().localPosition = new Vector2(0f, 0f);
            gameObject3.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200f, 0f);
            gameObject3.GetComponent<Text>().color = Color.black;
            gameObject3.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
            if (!string.IsNullOrWhiteSpace(skyTip))
            {
                gameObject3.transform.AddSkyTip(skyTip);
            }
            rectTransform.pivot = new Vector2(0f, 1f);
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
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(330f, 50f);
            Text component = NewText(placeholder, gameObject.GetComponent<RectTransform>().sizeDelta).GetComponent<Text>();
            component.transform.SetParent(gameObject.transform, worldPositionStays: false);
            component.color = new Color(1f, 1f, 1f, 0.5f);
            component.alignment = TextAnchor.MiddleCenter;
            component.GetComponent<RectTransform>().sizeDelta = new Vector2(260f, 50f);
            Text component2 = NewText("", gameObject.GetComponent<RectTransform>().sizeDelta).GetComponent<Text>();
            component2.transform.SetParent(gameObject.transform, worldPositionStays: false);
            component2.color = new Color(1f, 1f, 1f, 1f);
            component2.alignment = TextAnchor.MiddleCenter;
            component2.GetComponent<RectTransform>().sizeDelta = new Vector2(260f, 50f);
            inputField.placeholder = component;
            inputField.textComponent = component2;
            return gameObject;
        }
    }
}
