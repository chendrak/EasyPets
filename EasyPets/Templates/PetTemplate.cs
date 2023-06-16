using System.Collections.Generic;
using System.Linq;
using RogueGenesia.Data;

namespace EasyPets.EasyPets.Templates
{
    public class PetTemplate
    {
        public string? Name;
        public string? IconPath;
        public AnimationTemplate? IdleAnimation;
        public AnimationTemplate? RunAnimation;
        public Dictionary<string, string> NameLocalizations { get; set; } = new();
        public Dictionary<string, string> DescriptionLocalizations { get; set; } = new();
        public float SizeScale = 1.0f;

        private Result<LocalizationDataList> GetLocalizationDataListFrom(Dictionary<string, string> inputList)
        {
            if (inputList.Count == 0)
            {
                return Results.Fail<LocalizationDataList>("No Localizations provided");
            }

            var localizations = inputList.Select(dictionaryEntry => new LocalizationData
            {
                Key = dictionaryEntry.Key,
                Value = dictionaryEntry.Value
            });

            return Results.Ok(new LocalizationDataList
            {
                localization = localizations.ToList()
            });
        }

        public Result<LocalizationDataList> GetNameLocalizations() => GetLocalizationDataListFrom(NameLocalizations);
        public Result<LocalizationDataList> GetDescriptionLocalizations() => GetLocalizationDataListFrom(DescriptionLocalizations);
    }
}