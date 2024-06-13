using System;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIAttributeType : MonoBehaviour
    {
        public static DataStruct<string, string>[] allAttr = new DataStruct<string, string>[101]
        {
            new DataStruct<string, string>("atk", "player_gongji"),
            new DataStruct<string, string>("def", "player_fangyu"),
            new DataStruct<string, string>("hp", "player_tili"),
            new DataStruct<string, string>("hpMax", "player_tilishangxian"),
            new DataStruct<string, string>("mp", "player_lingli"),
            new DataStruct<string, string>("mpMax", "player_linglishangxian"),
            new DataStruct<string, string>("sp", "player_nianli"),
            new DataStruct<string, string>("spMax", "player_nianlishangxian"),
            new DataStruct<string, string>("dp", "player_daoli"),
            new DataStruct<string, string>("dpMax", "player_daolishangxian"),
            new DataStruct<string, string>("age", "common_nianling"),
            new DataStruct<string, string>("ageYear", "common_nianling"),
            new DataStruct<string, string>("life", "common_shouming"),
            new DataStruct<string, string>("lifeYear", "common_shouming"),
            new DataStruct<string, string>("talent", "player_wuxing"),
            new DataStruct<string, string>("health", "player_jiankang"),
            new DataStruct<string, string>("healthMax", "player_jiankangshangxian"),
            new DataStruct<string, string>("luck", "player_xingyun"),
            new DataStruct<string, string>("energy", "player_jingli"),
            new DataStruct<string, string>("energyMax", "player_jinglishangxian"),
            new DataStruct<string, string>("mood", "player_xinqing"),
            new DataStruct<string, string>("moodMax", "player_xinqingshangxian"),
            new DataStruct<string, string>("msp", "player_yisu"),
            new DataStruct<string, string>("fsp", "player_jiaoli"),
            new DataStruct<string, string>("hpRes", "player_tilihuifu"),
            new DataStruct<string, string>("mpRes", "player_linglihuifu"),
            new DataStruct<string, string>("spRes", "player_nianlihuifu"),
            new DataStruct<string, string>("crit", "player_huixin"),
            new DataStruct<string, string>("guard", "player_huxin"),
            new DataStruct<string, string>("critV", "player_huixinbeilv"),
            new DataStruct<string, string>("guardV", "player_huxinbeilv"),
            new DataStruct<string, string>("basPil", "player_liandanzizi"),
            new DataStruct<string, string>("basEqp", "player_lianqizizi"),
            new DataStruct<string, string>("basGeo", "player_fengshuizizi"),
            new DataStruct<string, string>("basSym", "player_huafuzizi"),
            new DataStruct<string, string>("basHerb", "player_yaocai"),
            new DataStruct<string, string>("basMine", "player_kuangcai"),
            new DataStruct<string, string>("basSword", "player_jianfazizi"),
            new DataStruct<string, string>("basSpear", "player_qiangfazizi"),
            new DataStruct<string, string>("basBlade", "player_daofazizi"),
            new DataStruct<string, string>("basFist", "player_quanfazizi"),
            new DataStruct<string, string>("basPalm", "player_zhangfazizi"),
            new DataStruct<string, string>("basFinger", "player_zhifazizi"),
            new DataStruct<string, string>("basFire", "player_huolinggen"),
            new DataStruct<string, string>("basFroze", "player_shuilinggen"),
            new DataStruct<string, string>("basThunder", "player_leilinggen"),
            new DataStruct<string, string>("basWind", "player_fenglinggen"),
            new DataStruct<string, string>("basEarth", "player_tulinggen"),
            new DataStruct<string, string>("basWood", "player_mulinggen"),
            new DataStruct<string, string>("beauty", "common_meili"),
            new DataStruct<string, string>("pfr", "player_wumian"),
            new DataStruct<string, string>("mfr", "player_momian"),
            new DataStruct<string, string>("exp", "player_jingyan"),
            new DataStruct<string, string>("standUp", "player_zhengdaozhi"),
            new DataStruct<string, string>("standDown", "player_modaozhi"),
            new DataStruct<string, string>("reputation", "common_shenwang"),
            new DataStruct<string, string>("abilityPoint", "playerInfo_daodian"),
            new DataStruct<string, string>("healthUpStable", "player_healthUpStable"),
            new DataStruct<string, string>("healthDownStable", "player_healthDownStable"),
            new DataStruct<string, string>("energyUpStable", "player_energyUpRate"),
            new DataStruct<string, string>("energyDownStable", "player_energyDownRate"),
            new DataStruct<string, string>("moodUpStable", "player_moodUpStable"),
            new DataStruct<string, string>("moodDownStable", "player_moodDownStable"),
            new DataStruct<string, string>("closeStable", "player_closeStable"),
            new DataStruct<string, string>("hateStable", "player_hateStable"),
            new DataStruct<string, string>("expGrow", "player_expGrow"),
            new DataStruct<string, string>("townBuyDecline", "player_townBuyDecline"),
            new DataStruct<string, string>("schoolBuyDecline", "player_schoolBuyDecline"),
            new DataStruct<string, string>("stepGrow", "player_stepGrow"),
            new DataStruct<string, string>("lockInitScore", "prop_qiangdu"),
            new DataStruct<string, string>("propGridNum", "prop_shangxian"),
            new DataStruct<string, string>("swordGrow", "player_swordGrow"),
            new DataStruct<string, string>("spearGrow", "player_spearGrow"),
            new DataStruct<string, string>("bladeGrow", "player_bladeGrow"),
            new DataStruct<string, string>("fistGrow", "player_fistGrow"),
            new DataStruct<string, string>("palmGrow", "player_palmGrow"),
            new DataStruct<string, string>("fingerGrow", "player_fingerGrow"),
            new DataStruct<string, string>("fireGrow", "player_fireGrow"),
            new DataStruct<string, string>("frozeGrow", "player_frozeGrow"),
            new DataStruct<string, string>("thunderGrow", "player_thunderGrow"),
            new DataStruct<string, string>("windGrow", "player_windGrow"),
            new DataStruct<string, string>("earthGrow", "player_earthGrow"),
            new DataStruct<string, string>("woodGrow", "player_woodGrow"),
            new DataStruct<string, string>("pillLayer", "player_danyaodiejia"),
            new DataStruct<string, string>("abilityExp", "playerInfo_xinde"),
            new DataStruct<string, string>("immortalPoint", "playerInfo_xianli"),
            new DataStruct<string, string>("basMgAll", "player_quanlinggen"),
            new DataStruct<string, string>("basWpAll", "player_quangognfa"),
            new DataStruct<string, string>("basArtAll", "player_quanjiyi"),
            new DataStruct<string, string>("basAll", "player_suoyouzhandouzizi"),
            new DataStruct<string, string>("basWpAllAny", "player_renyigongfa"),
            new DataStruct<string, string>("basMgAllAny", "player_renyilinggen"),
            new DataStruct<string, string>("basAllAny", "player_renyizhandou"),
            new DataStruct<string, string>("basWpMax", "player_zuigaogongfa"),
            new DataStruct<string, string>("basMgMax", "player_zuigaolinggen"),
            new DataStruct<string, string>("basAllMax", "player_zuigaozhandou"),
            new DataStruct<string, string>("basArtMax", "player_zuigaojiyi"),
            new DataStruct<string, string>("basWpMin", "player_zuidigongfa"),
            new DataStruct<string, string>("basMgMin", "player_zuidilinggen"),
            new DataStruct<string, string>("basAllMin", "player_zuidizhandou"),
            new DataStruct<string, string>("basArtMin", "player_zuidijiyi")
        };

        public DataStruct<string, string> selectItem;
        public Action<string, string> call;
        public Transform leftRoot;
        public Transform rightRoot;
        public Transform typeRoot;
        public GameObject goItem;
        public GameObject typeItem;
        public Text textTitle;
        public Text leftTitle;
        public Text rightTitle;
        public Button btnClose;
        public Button btnOk;

        public UIAttributeType(IntPtr ptr)
            : base(ptr)
        {
        }

        public static string GetAttrName(string attr)
        {
            DataStruct<string, string>[] array = allAttr;
            foreach (DataStruct<string, string> dataStruct in array)
            {
                if (dataStruct.t1 == attr)
                {
                    return GameTool.LS(dataStruct.t2);
                }
            }
            return "";
        }

        private void Awake()
        {
            leftRoot = base.transform.Find("Root/Left/View/Root");
            rightRoot = base.transform.Find("Root/Right/View/Root");
            typeRoot = base.transform.Find("Root/typeRoot");
            goItem = base.transform.Find("Item").gameObject;
            typeItem = base.transform.Find("typeItem").gameObject;
            textTitle = base.transform.Find("Root/Text1").GetComponent<Text>();
            leftTitle = base.transform.Find("Root/Text2").GetComponent<Text>();
            rightTitle = base.transform.Find("Root/Text3").GetComponent<Text>();
            btnClose = base.transform.Find("Root/btnClose").GetComponent<Button>();
            btnOk = base.transform.Find("Root/BtnOk").GetComponent<Button>();
            btnClose.onClick.AddListener((Action)CloseUI);
            btnOk.onClick.AddListener((Action)OnBtnOk);
            base.gameObject.AddComponent<UIFastClose>();
            goItem.GetComponent<Text>().color = Color.black;
            textTitle.text = "Select properties";
            leftTitle.text = "Select props below..";
            rightTitle.text = "Selected properties";
        }

        public void InitData(UIDaguiToolItem toolItem, int index)
        {
            DataStruct<string, string>[] array = allAttr;
            foreach (DataStruct<string, string> dataStruct in array)
            {
                DataStruct<string, string> selectItem = dataStruct;
                _ = dataStruct.t1;
                string text = GameTool.LS(dataStruct.t2);
                if (!string.IsNullOrEmpty(text))
                {
                    GameObject obj = UnityEngine.Object.Instantiate(goItem, rightRoot);
                    obj.GetComponent<Text>().text = text;
                    obj.AddComponent<Button>().onClick.AddListener((Action)delegate
                    {
                        this.selectItem = selectItem;
                        UpdateLeft();
                    });
                    obj.SetActive(value: true);
                }
            }
        }

        public void CloseUI()
        {
            g.ui.CloseUI(GetComponent<UIBase>());
        }

        public void UpdateLeft()
        {
            UnityAPIEx.DestroyChild(leftRoot);
            string text = GameTool.LS(selectItem.t2);
            GameObject obj = UnityEngine.Object.Instantiate(goItem, leftRoot);
            obj.GetComponent<Text>().text = text;
            obj.AddComponent<Button>().onClick.AddListener((Action)delegate
            {
                UnityAPIEx.DestroyChild(leftRoot);
                selectItem = null;
            });
            obj.SetActive(value: true);
        }

        public void OnBtnOk()
        {
            if (selectItem == null)
            {
                UITipItem.AddTip("Please select a property first!", 0f);
                return;
            }
            call(selectItem.t1, GameTool.LS(selectItem.t2));
            CloseUI();
        }

        private void Start()
        {
            UIDaguiTool.InitScroll(GetComponent<UIBase>());
        }

        private void OnDestroy()
        {
            UIDaguiTool.DelScroll(GetComponent<UIBase>());
        }
    }
}
