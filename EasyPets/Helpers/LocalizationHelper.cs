using System;
using System.Collections.Generic;
using System.Linq;
using RogueGenesia.Data;

namespace EasyPets.Helpers;

public static class LocalizationHelper
{
    public static LocalizationDataList GetLocalizationDataListFrom(params string[] inputList)
    {
        if (inputList.Length % 2 != 0)
        {
            throw new NotSupportedException("Number of parameters must be divisible by 2");
        }

        var dictionary = new Dictionary<string, string>();
        
        for (var i = 0; i < inputList.Length; i += 2)
        {
            dictionary.Add(inputList[i], inputList[i+1]);
        }
        
        return GetLocalizationDataListFrom(dictionary);
    }
    
    public static LocalizationDataList GetLocalizationDataListFrom(Dictionary<string, string> inputList)
    {
        if (inputList.Count == 0)
        {
            throw new NotSupportedException("No Localizations provided");
        }

        var localizations = inputList.Select(dictionaryEntry => new LocalizationData
        {
            Key = dictionaryEntry.Key,
            Value = dictionaryEntry.Value
        });

        return new LocalizationDataList
        {
            localization = localizations.ToList()
        };
    }
}