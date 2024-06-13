using System;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox
{
    internal class UIDaguiToolSetKey : MonoBehaviour
    {
        public CmdKey key = new CmdKey();
        public GameObject goOk;
        public Button btnOk;
        public GameObject goRe;
        public Button btnRe;
        public Text textMsg;
        public Action<CmdKey> okCall;
        public bool waitInpit = true;

        public KeyCode[] keys = new KeyCode[58]
        {
            KeyCode.F1,
            KeyCode.F2,
            KeyCode.F3,
            KeyCode.F4,
            KeyCode.F5,
            KeyCode.F6,
            KeyCode.F7,
            KeyCode.F8,
            KeyCode.F9,
            KeyCode.F10,
            KeyCode.F11,
            KeyCode.F12,
            KeyCode.Q,
            KeyCode.W,
            KeyCode.E,
            KeyCode.R,
            KeyCode.T,
            KeyCode.Y,
            KeyCode.U,
            KeyCode.I,
            KeyCode.O,
            KeyCode.P,
            KeyCode.A,
            KeyCode.S,
            KeyCode.D,
            KeyCode.F,
            KeyCode.G,
            KeyCode.H,
            KeyCode.J,
            KeyCode.K,
            KeyCode.L,
            KeyCode.Z,
            KeyCode.X,
            KeyCode.C,
            KeyCode.V,
            KeyCode.B,
            KeyCode.N,
            KeyCode.M,
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
            KeyCode.Alpha0,
            KeyCode.Keypad0,
            KeyCode.Keypad1,
            KeyCode.Keypad2,
            KeyCode.Keypad3,
            KeyCode.Keypad4,
            KeyCode.Keypad5,
            KeyCode.Keypad6,
            KeyCode.Keypad7,
            KeyCode.Keypad8,
            KeyCode.Keypad9
        };

        public UIDaguiToolSetKey(IntPtr ptr)
            : base(ptr)
        {
        }

        private void CallOk()
        {
            okCall?.Invoke(key);
            g.ui.CloseUI(GetComponent<UIBase>());
        }

        private void Awake()
        {
            goOk = base.transform.Find("Root/BtnOk").gameObject;
            btnOk = goOk.GetComponent<Button>();
            goRe = base.transform.Find("Root/BtnRe").gameObject;
            btnRe = goRe.GetComponent<Button>();
            textMsg = base.transform.Find("Root/teztKey").GetComponent<Text>();
            btnOk.onClick.AddListener((Action)delegate
            {
                string content = (string.IsNullOrEmpty(key.key) ? "Is it correct to cancel the shortcut key?" : ("determined to" + key.ToString() + "Set as shortcut key?"));
                g.ui.OpenUI<UICheckPopup>(UIType.CheckPopup).InitData("[Big Ghost] tool box", content, 2, (Action)delegate
                {
                    CallOk();
                });
            });
            btnRe.onClick.AddListener((Action)delegate
            {
                key = new CmdKey();
                waitInpit = true;
                btnOk.gameObject.SetActive(value: true);
                btnRe.gameObject.SetActive(value: true);
            });
            btnOk.gameObject.SetActive(value: true);
            btnRe.gameObject.SetActive(value: true);
        }

        private void Start()
        {
            UIDaguiTool.InitScroll(GetComponent<UIBase>());
            base.transform.Find("Root/btnClose").GetComponent<Button>().onClick.AddListener((Action)delegate
            {
                g.ui.CloseUI(GetComponent<UIBase>());
            });
        }

        public void InitData(Action<CmdKey> okCall)
        {
            this.okCall = okCall;
        }

        private void OnDestroy()
        {
            UIDaguiTool.DelScroll(GetComponent<UIBase>());
        }

        private void Update()
        {
            try
            {
                if (!waitInpit)
                {
                    return;
                }
                key.ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
                key.alt = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftAlt);
                key.shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                UpdateText();
                KeyCode[] array = keys;
                for (int i = 0; i < array.Length; i++)
                {
                    KeyCode keyCode = array[i];
                    if (Input.GetKeyDown(keyCode))
                    {
                        waitInpit = false;
                        key.key = keyCode.ToString();
                        UpdateText();
                        btnOk.gameObject.SetActive(value: true);
                        btnRe.gameObject.SetActive(value: true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(base.name + " Update " + ex.ToString());
            }
        }

        private void UpdateText()
        {
            string state = key.GetState();
            if (string.IsNullOrEmpty(state))
            {
                textMsg.text = "Waiting for shortcut key input...";
            }
            else
            {
                textMsg.text = state;
            }
        }
    }
}
