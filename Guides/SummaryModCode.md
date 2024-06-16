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
  - [5.3 The API's Event listeners](#53-the-apis-event-listeners)
  - [5.4 Things I learnt about OOP](#54-things-i-learnt-about-oop)
  - [5.5 Further reading](#55-further-reading)

The "Table of content" reference is auto-generated into itself by VSCodium, so no suspecting me xD

## 5.1 Documentation?
Other than a ridiculously laggy (due to its size) Microsoft HTML help file, located at `\Mod\modFQA\代码编写教程\GGBH_API.chm` within your game installation, there is "practically" nothing that's really helpful in guiding at all. But why is that?!
> Because to a full- non-game developer, *who also <ins>knows nothing</ins> about Unity Engine nor OOP*, there is absolutely nothing to pave your way towards the path of even figuring out <ins>**what**</ins> it is that you're supposed to code.

- Where/how do you execute your code?
<br>*(Blindly writing the black alphabetical words of static/dynamic Classes/methods on white sheets of paper. But then which person do you hand those sheets of papers to, in order to have the written message conveyed?)*
- Calling it more bluntly: How do you convey your wish to manipulate a certain in-game object?
<br>*(E.g. what even are the literal words that can express each object, or the "post messenger" who is going to deliver your message to the game?)*

So how does it work? We'll divide this into multiple parts and go over them one by one..

## 5.2 The ModProject
The default TOI mod recipe consists of the main class `ModMain`, which holds the public methods `Init()` and `Destroy()`. This archetype of mods are technically managed by the MelonLoader Harmony-mod `GGBH_MOD.dll` *(shipped within the game files)* which for-loop invokes these two methods per game-mod. Upon pressing the **Write Code** button in Mod Creator, the game will auto-generate you a new Visual Studio project. Read more about the pre-configuration from [Mod Overview section 3.1](./ModSummary.md#31-soleid-namespace-and-excelmid) if you had skipped it.
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

## 5.3 The API's Event listeners
It is finally time to answer the brute-logical questions that I proposed in [section 5.1](#51-documentation), <ins>however I'm going to do so from my *"possibly flawed understanding"* about either the game API and/or the entirety of Unity Engine games!</ins> So take it with a grain of salt, although the pure-logical/inferential observations are all true.

...


## 5.4 Things I learnt about OOP
...

## 5.5 Further reading
...