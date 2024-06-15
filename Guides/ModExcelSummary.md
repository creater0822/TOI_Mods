# 4. Overviewing ModExcel (json/xlsx)
This is none of other than a proportion of the game's data (incl. configs) that has been exposed, in the form of a relational database-like manner of data-files. First we'll look at the file structure conventions. Then a summary of places *(read: official game files that are included with the game)* from which we can obtain our meta-data, however low the quality and quantity may be. Lastly I'll attempt to describe some of the files of interest that I have seen throughout my time of modding. *(This last part will be a work-in-progress, depending on my mood/motivation to write more/less.)*

## Table of content
- [4. Overviewing ModExcel (json/xlsx)](#4-overviewing-modexcel-jsonxlsx)
  - [Table of content](#table-of-content)
  - [4.1 Data file convention](#41-data-file-convention)
    - [Json format](#json-format)
    - [Xlsx format](#xlsx-format)
  - [4.2 References and meta-data](#42-references-and-meta-data)
    - [4.2.1 StringFunctions.xlsx](#421-stringfunctionsxlsx)
    - [4.2.2 Immortal\_IDs.xlsx](#422-immortal_idsxlsx)
  - [4.3 File-topics of interest](#43-file-topics-of-interest)

## 4.1 Data file convention
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

### Json format
- Accepts non-quoted values (e.g. boolean and numerical)
- Supports both trailing commas
- Supports Java-style comments, e.g. // this is comment
- Within the objects, any key-order is fine.
- The Java-style comments *are preserved* after becoming part of a playable mod.
- Doesn't support the `ExcelMID` feature.

### Xlsx format
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

## 4.2 References and meta-data
Within the game's installation folder, you'll find the directory `\Mod\modFQA\配置修改教程` with 4 folders:
1. 配置（只读）-> Topic-bundled xlsx examples
2. 配置（只读）Json格式 -> Json-style data files (incomplete)
3. 配置对照 -> Chinese documentations within Excel files
4. 配置表头 -> Excel-style data files (headers only)

Going over it chronologically, **folder 2** contains a collection of json files that contain the vanilla game's data. Learning how to use ModExcel starts at reading these English file names and linguistically (one by one) judge if a file may pertain to the in-game feature which you'd like to mod. When you decide to study a specific data-file, you want to open its Excel-equivalent from **folder 4**, because the first row of that file will contain a description (for better or for worse.) 
> Based on the English file name, attribute-names, Chinese -> English translated description row (in Excel) and the context with however-many objects you have; You'll have to 'determine/guess' how it works! *~That's just how -void-of-documentation- it is..*

Supplementary to the above described, **folder 1** contains a bunch of Excel example-files that are named <ins>in Chinese</ins> after the category it represents. For example `道心配置表.xlsx` (Tao Heart Configuration Table), which contains the worksheets `TaoistSeed`, `TaoistHeart`, `TaoistHeartEffect` and `FateFeatureGroup`. This Excel file <ins>will not work</ins> in your `ModExcel` folder, because the file name will not be recognized, nor will the game API read all 4 worksheets. But its purpose is to indeed show (us players) that those 4 data files are relationally linked together, to define/represent the Taoist Mind/Heart feature.

<ins>Last but most important is **folder 3**</ins>, which sadly needs to be manually translated (per cell) with either Excel's built-in translator or any online translator. Within this repo you can find the (name-only)translated Excel file [StringFunctions.xlsx](./ModExcel/StringFunctions.xlsx) and [Immortal_IDs.xlsx](./ModExcel/Immortal_IDs.xlsx) which has a decent proportion of the main sheet Microsoft/Google-Translated.

### 4.2.1 StringFunctions.xlsx
This file holds all(?) of the pre-coded function-references that enable their actual coded function to be called from the relationnal-database file definitions, from which the game evaluates. The content is categorized in different worksheets, of whom the majority were comprehensively given an English name by me, knowing what the respective content pertains to. *(The literal translation of the Chinese sheet names are garbage.)* So here we'll go over them one by one..

**DramaFunc**<br>
Describes all of the functions that can be executed by drama-related data-files. The data files that are often used to execute these Drama Functions are:
- `FortuitousEvent` *(World events; for example triggering something after walking 1 tile or opening the city pub)*
- `DramaDialogue` *(Dialogues; holding text, (optional) portrait(s) of World- or Drama-units and option buttons and/or next-dialogue reference)*
- `DramaOptions` *(Defines the drama option buttons, also used to define which next dialogue will be called)*
- `DungeonTrigger` *(Can trigger drama functions from within battle instances)*
<br>There absolutely are <ins>many-many-more</ins> data files that can invoke these functions, but those are for you to discover.

**DramaCond**<br>
Describes all of the drama-related conditional check-functions, which can be added to any of the data-files that I summarized about **DramaFunc**. Other frequently used data files that can invoke these commands are:
- `NpcCondition` *(the least logical malfunctioning data file in the game, which I'd highly recommend against ever using for non-simple tasks.)*
- `RoleLogLocal` *(rarely used in my experience)*
<br>And <ins>many-many-more</ins> for you to discover, although I'd recommend learning ModCode at that point.

**BattleDramaFunc**<br>
These are special Drama Functions that are related to battle instances, often executed with `DungeonTrigger`. I don't have much experience with these kinds of functions so Idk anything else that invokes these, but I can imagine that there are many. *(Many that I've never even looked at yet.)*

**WorldEffectFunc**<br>
These function are used to define any effect that (no pun intended) 'takes effect' in the game world, aka outside of battle. The data file that references these functions is `RoleEffect`, which is one of the essential building block of any and all destinies in the game.

**FontColor**<br>
Tags and key-references to rnriches the otherwise plain text in the game with stuff such as text colors, item names *(that can be hovered over to show the item)*, icons and more. Beyond just different colors, I haven't ever cared to try anything else.

**SectRuleFunc**<br>
Used to define sect traits and rules which are referenced in `SchoolSlogan`, a data file that doesn't accept negative IDs.

**DecorationID**<br>
I've never actually used this in ModExcel, but it's pretty much a graphical representation of which ID pertains to what picture. This might be handy if you want to add stuff to the world map or in a certain battle dungeon.

### 4.2.2 Immortal_IDs.xlsx
This file contains worksheets about many different ID-categories that are (mostly) related to **battle instances**. Any worksheet that I don't describe is btw stuff that I still don't understand the meaning of. So let's get started sheet-by-sheet again:

**TypeID**<br>
This meta-data describes the essence within the data-file `BattleEffect`. If you open up `BattleEffect.json` you'll be greeted by json-objects that are **full of senseless numbers!**

<details>
<summary>See example 1:</summary>

```
{
  "id": "30114411",
  "typeID": "1122",
  "targetID": "1001",
  "value1": "8",
  "value2": "1",
  "value3": "&3011441_nlxh",
  "value4": "0",
  "value5": "0",
  "value6": "0",
  "value7": "0",
  "value8": "0",
  "overlay": "0",
  "layCount": "1",
  "duration": "-1",
  "classID": "0",
  "effect1": "Unit/jialan",
  "effect2": "0",
  "effect3": "0",
  "effect4": "0",
  "effect5": "0",
  "effect6": "0",
  "icon": "0",
  "headIcon": "0",
  "name": "0",
  "desc": "0",
  "valueDesc": "0"
}
```
</details>

The value of the key called `"typeID"` is what we need to Ctrl+F search for, within the **TypeID** worksheet. This search gives us the description *"When you consume a specified attribute, adjust the value of the attribute consumed"*. When we look further at the columns right next, we have **parameter 1 - 8**, which corresponds to the keys `value1` up to `value8`. Because `effect 1122` only has 3 parameters, the `BattleEffect` object that refers to `1122` only has meaningful data in `value1` up to `value3`. *(As `value4` - `value8` just need to be `0` and don't do anything.)* The key `"value3": "&3011441_nlxh"` is a key-reference towards an object in the data-file `BattleSkillValue`. `BattleSkillValue` is dynamic (numerical) value-definition that is used for skill description with Realm-variable colored numbered that might even have an icon with it.

So if we open `BattleSkillValue` and Ctrl+F search, we find..

<details>
<summary>Example 1: reference value</summary>

```
{
  "id": "9679",
  "key": "&3011441_nlxh",
  "value1": "-200",
  "value2": "-200",
  "value3": "-200",
  "value4": "-200",
  "value5": "-200",
  "value6": "-200",
  "value7": "-200",
  "value8": "-200",
  "value9": "-200",
  "value10": "-200",
  "valueMon": "Disable",
  "colorBind": "0",
  "icon": "nianli",
  "valueScale": "x0.01|f2"
}
```
</details>

In here we indeed see a key named `"key": "&3011441_nlxh"`. In here we have `value1` - `value10`, which correspond to the unit's Taoist Realm *(e.g 1 = Qi Refining and 10 = Transcendent.)* 

> Side-note: The object-ID in `BattleSkilLValue` (e.g. `"id": "9679"`) <ins>is independent</ins> to the object-ID in `BattleEffect`, as only the `"key"` value needs to match. *(Relational-database 101 basically)*

Returning to the `BattleEffect` example, we find out that it <ins>modifies</ins> the amount of Mental Power (Focus) spent, (looking at param2) percentually reduced, by a 10,000 ratio meaning `-200 = -2%`. Note that `"valueScale": "x0.01|f2"` in `BattleSkillValue` needs to match with the described type Effect's ratio.

**TargetID**<br>
This horizontal one-row table is used to describe the data-file `BattleEffectSelection`, which is relation-linked to the key `"targetID"` inside `BattleEffect`. As you see, `BattleEffectSelection` is used to define all of the stuff that you can see are described. If we look back in the worksheet `TypeID`, each Effect Type either has the description `"Target selector ID"` under the **Action Target** column, or hasn't. The Effect types that do have such a description *can* be target-configured via the `targetID` key. Otherwise, the effect can only target the unit which is explicitly described by the (English translated) description sentence(s).

**StatID**<br>
Describes the in-battle stat IDs as the worksheet name suggests. Remember **example 1** we saw earlier where the value `8` meant `Energy`. This table explains it all. There is of course more than 1 Battle effect that refers to this ID table.

**MonsterTypeID , dmgCauseTypeID , dmgReceiveTypeID**<br>
These are quite self-explanatory.

**skillAttrID**<br>
Along with the description being somewhat self-explanatory, reading (for example) `BattleEffect` type `3002` will give a better understanding of how/where those values could be used at. Similar to the `StatID` table, there is of course more than 1 Battle effect that refers to this ID table.

**HaloAttrID**<br>
Related to auras and/or some summoned units like the Spiritual Sword, *but I haven't yet spent time to figure out 'if or which' `BattleEffect` type uses this table.*

**effectAttrID**<br>
Used by `BattleEffect` type `3004` to modify the parameter-values of other `BattleEffect` types.

**targetEffectID**<br>
Used by `BattlEffect` type `3005` to specify the ambiguous description-properties. I haven't yet spent time to figure this out.

**specialEffectID**<br>
My favorite ID table among these BattleFunction-parameter references, used by `BattleEffect` type `6001`.

**Special effects ID comp**<br>
*I haven't even figured out a clue of where this table pertains to..*

## 4.3 File-topics of interest
*Coming soon (maybe...)*