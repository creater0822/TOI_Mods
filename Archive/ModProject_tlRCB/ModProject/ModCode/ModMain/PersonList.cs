using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_tlRCB
{
    public class PersonList
    {
        private bool _isOpen;

        private GameObject root;

        private List<WorldUnitBase> list = new List<WorldUnitBase>();

        public Action<WorldUnitBase> OnItemClick { get; set; }

        public PersonList(IEnumerable<WorldUnitBase> data)
        {
            list.AddRange(data);
        }

        public void Open()
        {
            if (_isOpen)
            {
                return;
            }
            _isOpen = true;
            root = CreateUI.NewCanvas();
            root.name = "YiNvConfigSystem";
            Transform transform = CreateUI.NewImage().transform;
            transform.SetParent(root.transform, worldPositionStays: false);
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(10000f, 10000f);
            transform.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.5f);
            Transform transform2 = CreateUI.NewImage(SpriteTool.GetSpriteBigTex("BG/huodebg")).transform;
            transform2.GetComponent<RectTransform>().sizeDelta = new Vector2(400f, 800f);
            transform2.SetParent(root.transform, worldPositionStays: false);
            transform2.GetComponent<RectTransform>().anchoredPosition = new Vector2(35f, -100f);
            GameObject gameObject = CreateUI.NewImage(SpriteTool.GetSprite("Common", "tuichu"));
            gameObject.transform.SetParent(root.transform, worldPositionStays: false);
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(200f, 300f);
            Action action = Close;
            gameObject.AddComponent<Button>().onClick.AddListener(action);
            GameObject gameObject2 = CreateUI.NewScrollView(new Vector2(310f, 520f));
            gameObject2.transform.SetParent(transform2, worldPositionStays: false);
            gameObject2.GetComponent<RectTransform>().anchoredPosition = new Vector2(-10f, 105f);
            VerticalLayoutGroup componentInChildren = gameObject2.GetComponentInChildren<VerticalLayoutGroup>();
            foreach (WorldUnitBase target in list)
            {
                string txt = target.GetName() + "(" + g.world.playerUnit.RelationCall(target) + ")";
                Action clickAction = delegate
                {
                    OnItemClick?.Invoke(target);
                    Close();
                };
                GameObject gameObject3 = CreateUI.NewButton(txt, clickAction, null, target.GetID());
                gameObject3.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 30f);
                gameObject3.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 30f);
                gameObject3.transform.SetParent(componentInChildren.transform, worldPositionStays: false);
            }
        }

        public void Close()
        {
            if (root != null)
            {
                UnityEngine.Object.Destroy(root);
            }
            _isOpen = false;
        }
    }
}
