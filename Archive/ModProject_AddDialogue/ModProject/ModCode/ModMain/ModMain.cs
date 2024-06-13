using System;
using System.Reflection;

namespace MOD_sectContribution
{
    public class ModMain
    {
		private static HarmonyLib.Harmony harmony;

        public void Init()
        {
			if (harmony != null)
			{
				harmony.UnpatchSelf();
				harmony = null;
			}
			if (harmony == null)
			{
				harmony = new HarmonyLib.Harmony("MOD_sectContribution");
			}
			harmony.PatchAll(Assembly.GetExecutingAssembly());

            // Execute our little check-add function
            ConfigDramaOptions();
        }

        private static void ConfigDramaOptions()
        {
            // DramaDialogue '22604' is the NPC talk dialogue, get the options string.
            string dramaTalkOptions = g.conf.dramaDialogue.GetItem(22604).options;
            string[] arrayText = dramaTalkOptions.Split('|');

            if (dramaTalkOptions == "0") // Vanilla game
            {
                g.conf.dramaDialogue.GetItem(22604).options = "-1997001";
                Console.WriteLine("No conflict at DramaDialogue '22604', DramaOption has been added.");
                return; // We're done
            }
            else
            {
                for (int i = 0; i < arrayText.Length; i++)
                { // Check if the DramaOption's already present
                    if (arrayText[i] == "-1997001")
                    {
                        Console.WriteLine("The DramaOption is already present in the game data.");
                        return; // We can stop the function if this is the case
                    }
                    else
                    { // Just show me what other IDs are in there
                        Console.WriteLine("DramaOption: " + arrayText[i]);
                    }
                }
                // Concatenate the ID that we want to add.
                g.conf.dramaDialogue.GetItem(22604).options += "|-1997001";
                Console.WriteLine("DramaOption has been added.");
                return;
            }
        }
        
        public void Destroy()
        {
            Console.WriteLine("On Destroy:");
            Console.WriteLine("I don't think ModExcel modifications have to be reverted by our code, should probably be done automatically");
        }
    }
}
