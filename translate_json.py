# Originally made by Celestine Lorewriter
import time
import sys
import traceback
import logging
import shutil # To backup the original
import json5 # Not a builtin and might slightly be slower
from deep_translator import GoogleTranslator # But the bottleneck's the translation anyway

def translate_to_english(text):
    translated_text = GoogleTranslator(source='auto', target='en').translate(text)
    return translated_text

def translate_json(json_file):
    start = time.time()
    with open(json_file, 'r', encoding='utf-8') as file:
        data = json5.load(file)
    print("File has been read, time to translate it")
    try:
        for index, item in enumerate(data):
            item['en'] = translate_to_english(item['ch'])
            print(f"translated {index}: {item['en']}")
    #except KeyboardInterrupt:
    except (KeyboardInterrupt, RequestError) as e:
        logging.error(traceback.format_exc())
        print(f"Script has been interupted! Trying to save the currently translated data..")
        with open(json_file, 'w', encoding='utf-8') as file:
            json5.dump(data, file, ensure_ascii=False, indent=2, quote_keys=True)
    
    print(f"Translation is done, time to save the data")
    with open(json_file, 'w', encoding='utf-8') as file:
        # Without quote_keys=True you'll lose the quotes with this method
        json5.dump(data, file, ensure_ascii=False, indent=2, quote_keys=True)
    end = time.time()
    print(f"Time spent: {end-start}")

def part_translate_json(json_file):
    start = time.time()
    with open(json_file, 'r', encoding='utf-8') as file:
        data = json5.load(file)
    print("File has been read, time to translate it")
    output = []
    try:
        for index, item in enumerate(data):
            if item['ch'] == "":
                continue # Nothing to translate
            if (item['ch'] != item['en']) & (item['en'] != ""):
                continue # Assuming that English is already available
            item['en'] = translate_to_english(item['ch'])
            print(f"translated {index}: {item['en']}")
            output.append(item) # Adds the translated dict to the output data
    #except KeyboardInterrupt:
    except (KeyboardInterrupt, RequestError) as e:
        logging.error(traceback.format_exc())
        print(f"Script has been interupted! Trying to save the currently translated data..")
        with open(json_file, 'w', encoding='utf-8') as file:
            json5.dump(output, file, ensure_ascii=False, indent=2, quote_keys=True)
    
    print(f"Translation is done, time to save the data")
    with open(json_file, 'w', encoding='utf-8') as file:
        json5.dump(output, file, ensure_ascii=False, indent=2, quote_keys=True)
    end = time.time()
    print(f"Time spent: {end-start}")

# Script code
if __name__ == "__main__":
    print("Input 1 for 'LocalText' or 2 for 'RoleLogLocal'")
    input_choice = input("Enter value: ")
    if input_choice == "1":
        json_file = 'LocalText.json'
    elif input_choice == "2":
        json_file = 'RoleLogLocal.json'
    else:
        sys.exit()
    print("-----------------------------")

    print("Input 1 for full translate or 2 for partial")
    input_choice2 = input("Enter value: ")
    print("-----------------------------")
    if input_choice2 == "1":
        shutil.copyfile(json_file, f"{json_file}.bak") # Creates a backup file
        translate_json(json_file)
    elif input_choice2 == "2":
        shutil.copyfile(json_file, f"{json_file}.bak") # Creates a backup file
        part_translate_json(json_file)
    else:
        sys.exit()
