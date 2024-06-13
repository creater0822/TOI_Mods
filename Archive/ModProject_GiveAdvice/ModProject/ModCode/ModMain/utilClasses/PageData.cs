using System;
using System.Collections.Generic;

namespace MOD_LE2lAt
{
    internal class PageData
    {
        private int page = 1;

        private int size = 6;

        public void clearPage()
        {
            page = 1;
            size = 6;
        }

        public void dramaOptions(int dramaId, List<OptionData> options, Action onClickPage, string dramaText)
        {
            UICustomDramaDyn uICustomDramaDyn = new UICustomDramaDyn(dramaId);
            for (int i = (page - 1) * size; i < options.Count && i < page * size; i++)
            {
                int num = ModMain.mid() + i + 1;
                uICustomDramaDyn.dramaData.dialogueOptions[num] = options[i].text;
                uICustomDramaDyn.SetOptionCall(num, options[i].onClick);
            }
            if (page > 1)
            {
                int num2 = ModMain.mid() + 900;
                Action action = delegate
                {
                    page--;
                    onClickPage();
                };
                uICustomDramaDyn.dramaData.dialogueOptions[num2] = "Prev pg";
                uICustomDramaDyn.SetOptionCall(num2, action);
            }
            if (page * size < options.Count)
            {
                int num3 = ModMain.mid() + 910;
                Action action2 = delegate
                {
                    page++;
                    onClickPage();
                };
                uICustomDramaDyn.dramaData.dialogueOptions[num3] = "Next pg";
                uICustomDramaDyn.SetOptionCall(num3, action2);
            }
            Action action3 = delegate
            {
            };
            uICustomDramaDyn.dramaData.dialogueOptions[ModMain.mid() + 1011] = "Homepage";
            uICustomDramaDyn.SetOptionCall(ModMain.mid() + 1011, action3);
            if (dramaText != null)
            {
                uICustomDramaDyn.dramaData.dialogueText[dramaId] = dramaText;
            }
            uICustomDramaDyn.OpenUI();
        }
    }
}
