# 5. Overviewing ModCode (C#.Net)
As suggested in the Mod summary document, you **need** MelonLoader in order to execute any mod component that relies on code. And to start things off you'll need to enable `Unicode UTF-8 for worldwide language support` in your Windows settings. This can be done in one of two ways:

**Quick way**<br>
1. Create a new file named `Whatever.Reg` and open the file in your favorite text editor.
2. Paste the following text and save the file and run it to apply the Registry changes.
```
[HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage]
"ACP"="65001"
"OEMCP"="65001"
"MACCP"="65001"
```
3. At your Steam Client: Right click the game, open Properties and add to the launch options: `--melonloader.showconsole`

**The slow way**<br>
Install Open Shell if you're on a Windows version newer than W7, to have a functional Start Menu search feature.<br> *(And because anything newer than W7 is uglier than Nicki Minaj)*
1. Search in start menu for `intl.cpl` and just left click the only search result. 
<br>*(Or you could alternative just execute `intl.cpl` in PowerShell 7)*
2. Go to the `Administrative` tab and click `Change system locale...`, check the box `Beta: Use Unicode UTF-8 for worldwide language support` and press ok.
3. Same as step 3 of the quick way.

Idk if you have to restart the system before it works btw. Another quick fact to note is that the first time running the game with MelonLoader installed will be significantly slower. So don't panic that your game is permanently sluggish, unless you plan to use Fatury.

## The prerequisites
I will take into assumption that you (at least) have elementary programming knowledge. In my terms, that includes concept like: 
- Basic data types (what they represent)
- Logical & arithmetic operators
- Arrays/Lists/Dictionaries/Pointers (optionally)
- Static- & (superficially) understanding of Dynamic methods

The ideal situation is if you've accumulated those conceptual understandings from the language **C** and/or from its dialects. But if you gained those understandings from **Java** or even **Python**, that should be fine too. If you're a pure- **Python** or **R** user, you may only have to get familiar with the stricter type-casting rules that are non-existent in your language. *(Within function-scope there is `var` which you could use for a bit to get used accustomed with typecasting, idk xD)*

## Table of content
- [5. Overviewing ModCode (C#.Net)](#5-overviewing-modcode-cnet)
  - [The prerequisites](#the-prerequisites)
  - [Table of content](#table-of-content)
  - [5.1 Documentation?](#51-documentation)
  - [5.2 The ModProject](#52-the-modproject)
  - [5.3 Expressing interactions with specific in-game objects](#53-expressing-interactions-with-specific-in-game-objects)
    - [5.3.1 Event listeners](#531-event-listeners)
    - [5.3.2 Harmony patch-instances](#532-harmony-patch-instances)
  - [5.4 OOP-things that this game has taught me](#54-oop-things-that-this-game-has-taught-me)
  - [5.5 Further reading](#55-further-reading)

The "Table of content" reference is auto-generated into itself by VSCodium, so no suspecting me xD

## 5.1 Documentation?
Other than a ridiculously laggy (due to its size) Microsoft HTML help file, located at `\Mod\modFQA\代码编写教程\GGBH_API.chm` within your game installation, there is "practically" nothing that's really helpful in guiding at all. What `GGBH_API.chm` <ins>**does**</ins> provide, are the per-Class help-pages that lists all of its Properties, Fields and Methods whilst (half of the time) given a one-word or one-sentence (half-baked/vague/ambiguous) description. 

So why do I claim that there's "practically" no helpful guiding?!
> Because to a full- non-game developer, *who also <ins>knows nothing</ins> about Unity Engine nor OOP*, there is absolutely nothing to pave your way towards the path of even figuring out <ins>**what**</ins> it is that you're supposed to code.

- Where/how do you execute your code?
<br>*(Blindly writing the black alphabetical words of static/dynamic Classes/methods on white sheets of paper. But then which person do you hand those sheets of papers to, in order to have the written message conveyed?)*
- Calling it more bluntly: How do you convey your wish to manipulate a certain in-game object?
<br>*(E.g. what even are the literal words that can express each object, or the "post messenger" who is going to deliver your message to the game?)*

So how does it work? We'll divide this into multiple parts and go over them one by one..

## 5.2 The ModProject
The default TOI mod recipe consists of the main class `ModMain`, which holds the public methods `Init()` and `Destroy()`. Whilst the game API resides at `/ModCode/ModMain/dll/Assembly-CSharp.dll`. This archetype of mods are technically managed by the MelonLoader Harmony-mod `GGBH_MOD.dll` *(shipped within the game files)* which for-loop invokes these two methods per game-mod. Upon pressing the **Write Code** button in Mod Creator, the game will auto-generate you a new Visual Studio project. Read more about the pre-configuration from [Mod Overview section 3.1](./ModSummary.md#31-soleid-namespace-and-excelmid) if you had skipped it.
> Given to you is the choice to either use the game developers' designed mod recipe, or to completely make your own MelonLoader mod. Since I have only learnt to use the default mod recipe, I can only describe such mods.

With the TOI mod design, Harmony patches are applied from `ModMain.Init()` with..
<details>
<summary>This code</summary>

```
if (harmony != null)
{
    harmony.UnpatchSelf();
    harmony = null;
}
if (harmony == null)
{
    harmony = new HarmonyLib.Harmony("Your_Mod_Namespace");
}
harmony.PatchAll(Assembly.GetExecutingAssembly());
```
</details>

..Which you can read more about [here](https://harmony.pardeike.net/articles/patching.html) if you want to create Harmony patches and/or write your own patch/unpatch lines.

## 5.3 Expressing interactions with specific in-game objects
> To answer this question, I'm going to describe my *"possibly flawed understanding"* about either the game API and/or the entirety of Unity Engine games!</ins> So **do** take it with a grain of salt, although the pure-logical/inferential observations are all true.

<ins>This is done by calling the class `g` from `Assembly-CSharp.dll`, e.g. the game's API.</ins>

To my understanding, `g` is the 'global' object that *(in my vocabulary)* represents your game instance or game process. The attributes from this object, which you can access via the syntax `g.attributeName` (for example: `g.events`), are the so-called Mgr (Manager) classes that provide further functionalities to aid you in your "communications" with the game.

| Reference syntax | Class     | Description                                                                      |
| ---------------- | --------- | -------------------------------------------------------------------------------- |
| g.cache          | CacheMgr  | Handles reading/writing your game save data                                      |
| g.conf           | ConfMgr   | Handles the game's relational-data files, aka the ModExcel json-data             |
| g.data           | DataMgr   | Handles world object manipulation, e.g. generating anything on the map etc..     |
| g.dlc            | DLCMgr    | Handles what the name suggests it does                                           |
| g.events         | EventsMgr | Handles Events listeners, which is essential to invoking many object instances   |
| g.mod            | ModMgr    | Handles all TOI mods e.g. mod load order, importing/exporting mods, etc..        |
| g.ui             | UIMgr     | Handles UI objects, e.g. open/close/get etc..                                    |
| g.world          | WorldMgr  | Handles most of the live game's instances such as NPC units, world events, etc.. |

The reference table that I put up are the Manager classes of which I recognize the use cases.

### 5.3.1 Event listeners
As the name implies, these are essentially tasks that are constantly polling for certain game events that get triggered. 
1. Open `GGBH_API.chm` and use the **Search** tab on the left panel to search for the keyword `ETypeData` and select the first result.
2. Scroll a little bit down in the right panel to see the header **"继承层次"** (inheritance hierarchy), which lists all of the Event listeners.

These Event listeners can be defined into your ModMain as Action-functions, or elsewhere if you know what you're doing. <br>An example config of setting up a listener on `OpenUIEnd`, looks as followed:
1. Define the Field: `private Il2CppSystem.Action<ETypeData> onOpenUIEnd;` *(You can rename it anything, but `onOpenUIEnd` is the most self-explanatory)*
2. In `ModMain.Init()` assign `onOpenUIEnd = (Il2CppSystem.Action<ETypeData>)OnOpenUIEnd;`, where `OnOpenUIEnd` will be the name of the dynamic function in which you can execute code that targets the subject. *(E.g. during the end of loading/opening UI objects.)*
3. Now we activate the Event listener right below the Field assignment with the Events method..
<br>`g.events.On(string id, Il2CppSystem.Action<ETypeData> call)`
<br>.. Which in our case looks like `g.events.On(EGameType.OpenUIEnd, onOpenUIEnd);`
> Note that `string id` is obtained by copy/pasting from the help-page at `GGBH_API.chm`, but ditch the "Data" suffix. This inconsistent naming scheme is yet another reason why I call the API obnoxious.
4. In order to stop the Event listener, use the method `g.events.Off(string id, Il2CppSystem.Action<ETypeData> call);`, which in our example case is `g.events.Off(EGameType.OpenUIEnd, onOpenUIEnd);`. This stop method is usually called inside `ModMain.Destroy()`.

With the event listener configured, your Action-function will look like this:
```
private void OnOpenUIEnd(ETypeData e){
  // The parameter yields the listener's data, which needs to by typecast into the relevant Event, this case OpenUIEnd.
  OpenUIEnd edata = e.Cast<OpenUIEnd>();

  // Checks if an OpenUIEnd event opens up the Player info UI
  if (edata.uiType.uiName == UIType.PlayerInfo.uiName){
    Console.WriteLine("This 'Hello World!' will appear in your MelonLoader console.");
    // Your code...
  }
  // Checks if an OpenUIEnd event opens up the NPC info UI
  else if(edata.uiType.uiName == UIType.NPCInfo.uiName){
    // Your code...
  }
  // Etc..
}
```

Within the Event listern's Action-function, we can use the above-shown if-else syntax to determine which specific UI has been opened to run our code. Of course in here: `UIType` is the class that you can search from the help files, which will give you a gargantuan list of Field names. Each representing an UI-type from which you can call `.uiName` to get a string for matching the Event listener's.

### 5.3.2 Harmony patch-instances
Alternatively `using HarmonyLib` , you can access a Class method instance via `static` `Prefix()` or `Postfix()` method within a Harmony patch class. 
<br>*(Note that the Harmony patch class must be defined within the Namespace of which your `ModMain.Init()` method instantiated and patched. Otherwise you'd have to call it elsewhere by your own code.)*

Given the the `NPCInfo` The code will roughly look like this:
```
using HarmonyLib;

namespace YourModNamespace{
  [HarmonyPatch(typeof(UINPCInfo), "UpdateUI")] // This Python decorator-like syntax means the following class is a patch
  public class Patch_UINPCInfo_UpdateUI{ // You can name the class anything, but this name is comprehensive
    [HarmonyPostfix] // This line indicates a postfix method is coming
    private static void Postfix(UINPCInfo __instance) // The parameter __instance is indeed what it says it is
    {
      Console.WriteLine("This message will appear in your MelonLoader console after each UINPCInfo.UpdateUI() call");
      // Your code...
      // In here you can use __instance.methods , __instance.fields or __instance.properties
      // ...by searching for the UINPCInfo in the HTML help files
    }
  }
}
```

## 5.4 OOP-things that this game has taught me
- Many C# methods look like `.MethodName<ReturnType>()`, such as for example `List<int>()`
- Object // Class inheritance's syntax looks like `public class Cat : Animal` .
- The convention names **Pascal** and **Camel**, which describe conventions on Caps in names.
- Since 2012 or 2013 when Visual Studio became ugly bloatware, through the miracle of time..
<br>*The many of its 'new scriptkid' features which absolutely sucked, have finally become genuinely good.*


## 5.5 Further reading
This section will be reserved for potential (more in-depth) guide documents in which I could describe which combinations of data-files can create "what" things.

The list of stuff I'd potentially elaborate further:
- Interacting with other mods (e.g. seeing what's loaded)
- UI object location changing
- How to invoke [...] Class to use it?