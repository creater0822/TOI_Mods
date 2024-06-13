using System;
using Il2CppSystem.Collections.Generic;

namespace MOD_tlRCB
{
    internal static class Dress
    {
        private static WorldUnitBase _unit = null;

        private static string _dressString = "";

        private static Action _callback = null;

        public static void Open(WorldUnitBase unit, Action after = null)
        {
            if (unit != null)
            {
                _unit = unit;
                _callback = after;
                g.events.On(EGameType.OneOpenUIEnd(UIType.ModDress), (Action)HandleUIOpen, 1);
                g.ui.OpenUI(UIType.ModDress);
            }
        }

        public static void Open(WorldUnitBase unit, int dramaId, DramaData dramaData = null)
        {
            Action after = null;
            if (dramaId > 0)
            {
                after = delegate
                {
                    DramaTool.OpenDrama(dramaId, dramaData);
                };
            }
            Open(unit, after);
        }

        private static void HandleUIOpen()
        {
            UIModDress modDress = g.ui.GetUI<UIModDress>(UIType.ModDress);
            UnitSexType sex = _unit.data.unitData.propertyData.sex;
            Dictionary<string, List<ConfRoleDressItem>> dictionary = null;
            switch (sex)
            {
                case UnitSexType.Man:
                    dictionary = g.conf.roleDress.allDressManItem;
                    break;
                case UnitSexType.Woman:
                    dictionary = g.conf.roleDress.allDressWomanItem;
                    break;
                default:
                    Log.Debug("换装性别错误, 系统没有对应性别的衣服数据");
                    return;
            }
            List<UICreatePlayerFacade.FacadeItemData> list = new List<UICreatePlayerFacade.FacadeItemData>();
            Dictionary<string, List<ConfRoleDressItem>> dictionary2 = new Dictionary<string, List<ConfRoleDressItem>>();
            Dictionary<string, List<ConfRoleDressItem>>.KeyCollection.Enumerator enumerator = dictionary.Keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string current = enumerator.Current;
                dictionary2.Add(current, dictionary[current]);
            }
            if (!dictionary2.ContainsKey("lianshi"))
            {
                dictionary2.Add("lianshi", new List<ConfRoleDressItem>());
            }
            Il2CppListAddToList<ConfRoleDressItem>(dictionary2["meixin"], dictionary2["lianshi"]);
            Il2CppListAddToList<ConfRoleDressItem>(dictionary2["quanlian"], dictionary2["lianshi"]);
            Il2CppListAddToList<ConfRoleDressItem>(dictionary2["zuolian"], dictionary2["lianshi"]);
            Il2CppListAddToList<ConfRoleDressItem>(dictionary2["youlian"], dictionary2["lianshi"]);
            dictionary2.Remove("meixin");
            dictionary2.Remove("quanlian");
            dictionary2.Remove("zuolian");
            dictionary2.Remove("youlian");
            enumerator = dictionary2.Keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string current2 = enumerator.Current;
                UICreatePlayerFacade.FacadeItemData item = new UICreatePlayerFacade.FacadeItemData(dictionary2[current2]);
                list.Add(item);
            }
            ModDataValueString modDataValueString = new ModDataValueString();
            modDataValueString.SetString(_unit.GetUnitDressString());
            modDress.unitSexType = sex;
            modDress.dressItems = list;
            modDress.Init();
            modDress.InitData(modDataValueString, sex);
            modDress.UpdateFacadeUI();
            modDress.UpdateModelData();
            Action action = delegate
            {
                _dressString = modDress.piptValue.text;
                g.ui.CloseUI(new UIType.UITypeBase("ModDress", UILayer.UI));
                _unit.ChangeDress(_dressString);
                if (_callback != null)
                {
                    _callback();
                }
                else
                {
                    g.timer.Frame((Action)DramaTool.NextDialogue, 1);
                }
                Clear();
                g.ui.GetUI<UINPCInfo>(UIType.NPCInfo)?.UpdateUI();
            };
            modDress.btnOK.onClick.RemoveAllListeners();
            modDress.btnOK.onClick.AddListener(action);
            void Il2CppListAddToList<T>(List<T> from, List<T> to)
            {
                List<T>.Enumerator enumerator2 = from.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    T current3 = enumerator2.Current;
                    to.Add(current3);
                }
            }
        }

        private static void Clear()
        {
            _unit = null;
            _dressString = "";
        }
    }
}
