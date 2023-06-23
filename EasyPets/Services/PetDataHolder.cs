using System.Collections.Generic;
using RogueGenesia.Data;

namespace EasyPets.Services;

public class PetDataHolder
{
    public PetDataHolder(string name, LocalizationDataList nameLocalization, LocalizationDataList descriptionLocalization, List<PetBehaviour> behaviours)
    {
        this.name = name;
        this.nameLocalization = nameLocalization;
        this.descriptionLocalization = descriptionLocalization;
        this.behaviours = behaviours;
    }

    public string name { get; private set; }
    public LocalizationDataList nameLocalization { get; private set; }
    public LocalizationDataList descriptionLocalization { get; private set; }
    public List<PetBehaviour> behaviours { get; private set; }
}