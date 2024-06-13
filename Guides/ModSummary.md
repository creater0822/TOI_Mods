# Overview to modding Tale of Immortal
> Written by Creater Cyfire, last revision: Jun 12, 2024

Modding in Tale of Immortal *-e.g. using the game's API-* is best described as *<ins>'appreciating modern art'</ins>*, or in other words:
1. It completely lacks any form of official English documentation.
2. The existing Chinese documentation *(afaik with Google Translate)* is extremely ambiguous; Not only in terms of linguistic quality, but also in terms of 'availability' of description. *For example: A function parameter description describes an ID table, but lacks the source reference.*
3. The API's Class structure *(in my opinion)* is extremely obnoxious: Afaik poor naming choices, etc..

But why am I writing this?!<br>
It is of course **not** to 'just trash' on the game developers, because in fact: The fact that a full-fledged mod API along with *any* documentation exists, is already infinitely better than anything with none of the above. The point that I aim to exclaim is that things "taking long" or "being difficult" doesn't at all mean it's your (intellectual) inability! It simply means that the API and its (non-existing) English documentation isn't doing a good enough job to make itself usable for your case.

Nonetheless, modding this game **can** be a lot of fun! And here I'll hopefully to make your introductory experience less obscured as mine was.<br>
From here you can either skip to the [Table of content](#table-of-content), or continue to read about my personal story with this game.

## My story...
- I'm a worshipper // slave of **logic**.
- I'm the kind of person that **hates** memorizing stuff.
- I never memorize facts; I 'absorb' logic-based concepts into my consciousness, and continue to live with it like inhaling/exhaling.
- <ins>I'm a 100%-cheat-player!</ins> Having *-never-in-my-life-* spent a singular playing a videogame without cheating xD

With all of that said: This summary is basically written in the perspective of (dare I say it) **overwhelming amount of brute- logic**, but <ins>barely any</ins> specialized 'convention-related' knowledge. It (indeed) means that I'll be assuming any 1+1=2 equivalently-easy facts to be better self-discovered; Whilst spending time/effort to write about what kind of logic-based implementations are relevant to modding this game, so you can begin your own exploration knowing what and where to find your way out.

## Table of content
- [Overview to modding Tale of Immortal](#overview-to-modding-tale-of-immortal)
  - [My story...](#my-story)
  - [Table of content](#table-of-content)
  - [How/where to start?](#howwhere-to-start)
  - [1. Tools](#1-tools)
  - [2. All creation categories](#2-all-creation-categories)
  - [3. Mod Creator](#3-mod-creator)
    - [3.1 soleID, namespace and ExcelMID](#31-soleid-namespace-and-excelmid)
    - [3.2 Mod creator features](#32-mod-creator-features)
  - [4. ModExcel (json/xlsx)](#4-modexcel-jsonxlsx)
    - [4.1 Data file convention](#41-data-file-convention)
    - [4.2 References and meta-data](#42-references-and-meta-data)
    - [4.3 File-topics of interest](#43-file-topics-of-interest)
  - [5. ModCode (using C#.Net)](#5-modcode-using-cnet)
  - [6. Source reconstruction](#6-source-reconstruction)

## How/where to start?
Let's start with some quick info:
- The game is built in `Unity 2020.3.9f99` with game type being `Il2Cpp`
- The game's API is built in `.Net 4.7.2`
- Some mods are served with [MelonLoader](https://github.com/LavaGang/MelonLoader), by default version `0.5.4` is preinstalled with the game.<br> 
*Can be upgraded up to [0.5.7](https://github.com/LavaGang/MelonLoader/releases/tag/v0.5.7), the highest version before .Net 4.7.2 was dropped.*
- The MelonLoader config comes with `Harmony`, which you can read more about [here](https://harmony.pardeike.net/articles/patching.html)

Assuming that you've bought the game from Steam, its default location is: `DIR\steamapps\common\鬼谷八荒\MelonLoader`<br>
*-With `DIR` being the path where you configured your Steam's game folder to be at-*

Existing TOI mods can be obtained from various places:
- [Steam Workshop](https://steamcommunity.com/app/1468810/workshop/): (Should have the most amount of mods)
- [NexusMods](https://www.nexusmods.com/taleofimmortal): I don't think it has a lot of stuff that you can't already find on Steam
- [SkyMods](https://catalogue.smods.ru/?app=1468810): A Steam Workshop archive which idk if I'd fully trust, but it's there..
- [3DM](https://mod.3dmgame.com/Guigubahuang): Chinese site with some stuff that can't be found on Steam, but in my opinion (after source code auditing) nothing that's interesting
- [hzgame](https://www.hzgamecn.top/forum/46.html): Chinese forum which has mods that are locked behind registering an account to comment, which I never bothered trying

## 1. Tools
A list of everything I use to mod this game:
- [VSCodium](https://github.com/VSCodium/vscodium) + [Prettier Code formatter](https://marketplace.visualstudio.com/items?itemName=esbenp.prettier-vscode): To edit Json files and beautify game-dumped Json files that uses too many new-lines for Arrays.<br>
*VSCodium is a VSCode fork that doesn't have Microsoft's telemetry. Using it with VSCode extensions requires, read [this documentation](https://github.com/VSCodium/vscodium/blob/master/docs/index.md#extensions-marketplace)*
- [Notepad++](https://github.com/notepad-plus-plus/notepad-plus-plus): I like using this tool to collapse `ModExportData.cache` files to quick-select code blocks of Json-files.
- [VS Community Edition](https://visualstudio.microsoft.com/vs/community/): Used to write and build ModCode libraries.
- [ilSpy](https://github.com/icsharpcode/ILSpy): My DotNet decompiler choice for TOI mods, as I've found it to give better code than dnSpy.<br>
*Use settings `C# 7.2 / VS 2017.4` to read stuff*
- [dnSpyEx](https://github.com/dnSpyEx/dnSpy): Has more features than ilSpy. And rarely when ilSpy's code interpretation doesn't look good, this might give you better code.
- [Unity Explorer](https://github.com/yukieiji/UnityExplorer): A collection of in-game tools that allow you to set hooks, runs C# code, inspect objects etc..<br> 
*Choose the `MelonLoader.Il2CPP` version*
- [Mod Decryption Tool](https://steamcommunity.com/sharedfiles/filedetails/?id=3225372871): My very own TOI tool which uses the game API to dump full Json (data)files, decrypt/(re)encrypt mod files, etc..
- [Ghidra](https://github.com/NationalSecurityAgency/ghidra): Used to [generate](https://discord.com/channels/814020065875329065/983609507915628564/1125081847362232360) pseudocode for `GameAssembly.dll` which is a C++ library.<br> 
*See this [Discord post](https://discord.com/channels/814020065875329065/983609507915628564/1228438021179838484) if you want an existing project.*
- [IDA Pro](https://hex-rays.com/ida-pro/): For the same purpose as using Ghidra if you have the money or pirated it.
- [AssetStudio](https://github.com/Perfare/AssetStudio): I once used this to explore Unity Assets.
- [Asset Bundle Extractor](https://github.com/SeriousCache/UABE): I once used this to modify text within the assets of a mod.<br>
*(But never again, cause it took forever to manually click thousands of text items)*

In case you didn't see Unity on the list, then you've seen it right! Because I have sworn to **never** subject myself to learning how to create Unity Assets for my own ego, self-worth, sanity and god-complex's sake.

## 2. All creation categories
Mods can be built upon one or multiple categories:
1. Mod Creator (in-game)<br>
 *Every Mod Project is defined, (optionally) tested in Debug Mode and exported for distribution/gameplay from this interface. It also includes a pre-configured GUI of mod creation features, which is also <ins>**the only English**</ins> related to modding that's been made available by the game devs themselves.
<br>This part works <ins>independent</ins> of MelonLoader.*
2. ModExcel (relational-database)<br>
 *Under the hood of the pre-configured Mod Creator GUI you'll find Json data. ModExcel is the unrestricted (by finite UI) 'real' experience of modding the vanilla game's API-exposed data.
<br>This part works <ins>independent</ins> of MelonLoader.*
3. ModCode (programming)<br>
 *The final form of modding in TOI in which you're directly communicating with the game API.
<br>This part <ins>depends</ins> on MelonLoader to function!*
4. ModRes (designing)<br>
 *Since I've never learnt how this works, I don't have the credits to write about it topic either.*

From this point I would recommend reading my [TOI_Mod_Management_Guide](https://drive.google.com/file/d/1vmrPlddnmfrfcJGC9yQz2IT41MPYLY7U/view?usp=drive_link) first, if you haven't already and/or don't already know all basics to managing this game's mods. Assuming that you know everything described in there, we'll continue..

## 3. Mod Creator
This tool is crucial for the generation of mod projects and exportation of playable Mod folders. Since this tool support English, I'll start with advanced settings which I would recommend any mod creator, regardless of which creation category you're focused into. After that I'll briefly summarize the tool's creative features which are highly limited.

### 3.1 soleID, namespace and ExcelMID
Upon creating a new Mod Project, the first course of action is opening the `ModProject` folder, then opening the files `ModProject.cache` and `ModData.cache` in your favorite text editor.
1. In `ModProject.cache`, change the random string in the line `"soleID":"randomString"` into something unique but comprehensive.<br>
 *The `soleID` is a mod's unique ID which the game API recognizes before it is initiated. Having this unique value be a comprehensive name is a good practice that can make your life easier, if or when you decide to delve into the rabbit of hole with ModCode.*
2. In `ModData.cache`, go to the end of the file where you'll find `"modNamespace":"MOD_randomString"` and (again) change the value into something comprehensive.<br>
 *The namespace doesn't necessarily need the prefix `MOD_`, nor does it have to match your mod's `soleID`. But I would recommend naming it `MOD_YourSoleID`, with of course the `soleID` in place of "YourSoleID".*

The above two steps are especially recommended if you're making a code mod. Next up are two optional settings, neither of which I'd recommend unless you know what you're doing:

- *(Optional)* Also in `ModData.cache` and right before the `namespace` key, you'll find the key `"excelMID"` with a big integer behind it.<br> 
 *This is an ID-offset value which you could use, if you're defining your ModExcel files in Excel, as it doesn't work for Excel.*
- *(Optional)* Back in `ModProject.cache` you'll find a line `"excelEncrypt":true`. You can set it to `false` in order to not encrypt any data file in `ModExcel`.<br>
 *Idk if it's a placebo effect, but in my experience I seem to run into 'slightly more' ModExcel load-issues shared between all mods, while I have one where `excelEncrypt` is turned off. I'd only recommend using this if you're testing your ModExcel config in a non-Debug save, whilst your mod has ModCode as well. (Then you could drop/replace the Json file that you're edit/testing.)*

### 3.2 Mod creator features
If you already know how to mod using ModExcel and/or want to start things off on a higher phase, you can skip this section. Here I'll just briefly describe what stuff you can do. I **won't** go over the many pre-added *(limited)* ModExcel functions that the Mod Creator has to offer, because your English comprehension is as good as mine.

<details>
<summary>Mod Creator interface features: Expand to read</summary>

**Create destiny:**<br>
As the button suggests, it allows you to create destinies.

**Create Adventure:**<br>
Creates world map events, for example: Moving 1 square on the map triggers a Drama dialogue, or gives you a Nurture Destiny.

**Generate NPC:** <ins>Invaluable!!</ins><br>
This is one of the Mod Creator features that I could recommend using over ModExcel. Because unlike ModExcel which is plain text writing, the Mod Creator graphically shows you how your NPC will look like, during the configuration.

**Create Minor Sect:** <ins>Mildly useful</ins><br>
Dragon Hamlet Mountain is a "small sect" or "minor sect". You can create however many you want, but there can only be 1 small sect in a game world. Creating sects in ModExcel is extremely obnoxious due to the bad documentations. So this is a fairly good tool if you want to make something quick.

**Create normal Sect:** <ins>Mildly useful</ins><br>
Lets you create however many normal sects you want, they can all be generated into your next game save. Same reason as the above mentioned, it's pretty handy unless you want to create something complex.

**Customize Normal Sect:** <ins>Mildly useful</ins><br>
Let's you 'mod' one of the existing normal sects.

**Customized manual:** Invaluable!!<br>
This feature is made to assist you in testing/creating your own skills in ModExcel. It's essentially a tool that lets you input any skill ID to instantiate a Debug Mode dummy-room for you to test your skill.

**All General Configuration > Extract NPC:** <ins>Horrible!!</ins><br>
<ins>Even I (up until this date) don't yet know how to properly use its ModExcel counterpart.</ins> This data-file is by far the most putrid thing of the entire game's mod system that I have ever seen. I'd absolutely recommend against using it for **any** 'advanced' conditional purpose!

**All General Configuration > World Map Location:**<br>
I've never used this feature in the Mod Creator, so idk how well it works.

**All General Configuration > World map event:**<br>
Can be used to create interactive world map icons/squares, on which you could sect some pre-defined commands.

**All General Configuration > Mission:**<br>
I've never created missions, neither with the Mod Creator nor in ModExcel.

**All General Configuration > Item:**<br>
Lets you create various items. I've only had experience creating rings, mounts and elixirs using this feature. It works fine for those three, but ModExcel is more consistent (for example) if you want to bulk-create a ring/mount category for all 10 realms.

**All General Configuration > Mount Model:** <ins>Invaluable!!</ins><br>
An interface to select, scale, and x/y-shift the PNG file of your mount model. It also lets you add various animation effects. None of this would've been possible in a blind text editor, if you wanted to write the Json or Excel file from scratch. I would recommend making/finalizing this in the Mod Creator, then extract the Json data in order to re-assign a better ID etc..

**All General Configuration > Plot:**<br>
Creates Drama Dialogues and lets you add Drama Options to it. This is definitely a lot less headache compared to doing it in ModExcel, but also a lot less versatile in terms of Drama Function support.

**All General Configuration > Plot NPC:**<br>
Creates plot-only NPCs. E.g. just a portrait with a name on it, which you can use in Drama dialogues, in case your dialogue is against a non- World Unit entity.

**All General Configuration > Dungeon:**<br>
Creates battle instances, which I've only superficially tried out once. So idk how good/bad it really is.
</details><br>

**Advanced:**
- **Modify Config**: Clicking this button for the first tiem generates the (empty) ModExcel folder **and** generates the random (large) number as your Mod project's excelMID, in case you want to use it.
- **Write Code**: Clicking this button for the first time generates the ModCode folder, which contains the default Visual Studio project template that's been given to us by the devs.
- **Modify Resources**: Clicking this button for the first time generates the ModRes folder, which contains the default Unity project template that's been given to us by the devs.

## 4. ModExcel (json/xlsx)
This is none of other than a proportion of the game's data (incl. configs) that has been exposed, in the form of a relational database-like manner of data-files. First we'll look at the file structure conventions. Then a summary of places *(read: official game files that are included with the game)* from which we can obtain our meta-data, however low the quality and quantity may be. Lastly I'll attempt to describe some of the files of interest that I have seen throughout my time of modding. *(This last part will be a work-in-progress, depending on my mood/motivation to write more/less.)*

### 4.1 Data file convention
- The file name equals the data-construct which it pertains to (case-sensitive) and is defined in either json- or xlsx-format.
- The game reads from the json-style, which is the extension type that you'll obtain (even) if you "Export" an xlsx-version of the data-file into a playable Mod-folder.
- Within the `ModExcel` folder you can create multiple sub-folder layers given any name to topic-separate a data-file. For example separate the lines in `LocalText` per skill, per quest dialogue, etc..
- In order to patch the value of 1 variable, the (patch)data file only needs that object's ID and variable & value defined. You don't need to copy/paste the other N-variables of that object.
- **Most** of the data files support both positive and negative integer ids, but not every data file does! *(As far as I know, only `SchoolSlogan` doesn't accept negative ids.)*
- The majority of the game's text are defined in either `LocalText` or `RoleLogLocal`, while other data-files would key-reference their text to those files.
- You **can** make use of both json and xlsx in the same `ModExcel` project, for example `LocalText.xlsx` with `RoleCreateFeature.json`. But!!
- You **cannot** have define both a json and xlsx of the same data-file within the same directory-level. *(I forgot which of the 2 has priority, but that's not really important.)*
  - Bad Example: `/ModExcel/LocalText.json` + `/ModExcel/LocalText.xlsx`
  - Good example: `/ModExcel/LocalText.json` + `/ModExcel/CheatDialogue/LocalText.xlsx`

**Json format:**
- Accepts non-quoted values (e.g. boolean and numerical)
- Supports both trailing commas
- Supports Java-style comments, e.g. // this is comment
- Within the objects, any key-order is fine.
- The Java-style comments *are preserved* after becoming part of a playable mod.
- Doesn't support the `ExcelMID` feature.

**Xlsx format:**
- The data is defined in the first sheet and the sheet name must equal the file name (minus extension type ofc.)
- A conventional data-table with variable per column and observation per row.
- The 1st row of the data-table **must** be comments or empty, while <ins>the variable names must be defined on the 2nd row</ins>. The 3rd row is used to define the data type of the variable, e.g. string or integer.
- To input negative values in a cell, you must use a single-quote `'` prefix before the value. Otherwise Excel considers it a function- / formula-cell.
- I've never tried using cell-functions in ModExcel data, therefore don't recommend it as I don't even know if it's allowed.
- The first empty row at the bottom of your table marks the end of the table. Any data-row below it are considered comments.
- Empty columns on the right can be used for per-row comments as well, but I recommend:
  1. Defining the data table with every variable that the respective data-file has.
  2. (Similar to the row-rule) leave one column empty before picking the next column(s) for comments.
- Any Excel sheet other than the first/main one can be used to save more comments, be used to store reference-data, or whatever..
- When the xlsx file is transformed into Json, all of its 'comment cells' are ignored and *will not be stored* in the playable Mod folder.
- Supports `ExcelMID` for object IDs. Syntax example: 
  - ExcelMID = `123456000`
  - You want your destiny's name text in `LocalText` to have the id `123456001` and the description's text id to be `123456002`
  - Thus destiny name id = `MID&001` and description id = `MID&002`
- You can decorate your cells and text with fancy colors and line-borders to make your files easier for yourself to navigate through.
- Windows users that have used custom UX-color profiles might be cursed in Microsoft Office software, as those applications depend on your system-UX whilst the actual file-defined color values aren't! For example: Your World file with dark background and white text is in fact still a white background on black text on any other vanilla Windows config.

With all of that summarized, I would personally like to use the json-style more. Plain-text editors are significantly faster (read: more responsive) than Excel and you can comment per line, opposed to Excel where you can only add comments at the far-bottom or far-right. *(Yes you can add mouse-hover comments per cell, but I'm not a fan of that..)* Making it worse, Excel doesn't let you open two workbooks with identical names..<br>
On large-scale mods (example: hundreds of dialogues panels & options) however, Excel might become a more suitable choice. *(Cause yea.. Its drag-fill feature and per-row observation vs Json's multiple-row per-observation making it easier to skim over in Excel.)*

Long story short: Neither implementation styles are perfect as both have their pros and cons. So this is where you're on your own to figure out how you'd want to get things done.

### 4.2 References and meta-data
...

### 4.3 File-topics of interest
*Coming soon (maybe...)*

## 5. ModCode (using C#.Net)
...

## 6. Source reconstruction
...