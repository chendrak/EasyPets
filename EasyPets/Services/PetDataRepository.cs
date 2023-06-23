using System.Collections.Generic;

namespace EasyPets.Services;

public static class PetDataRepository
{
    private static Dictionary<string, PetDataHolder> dataDictionary = new();
    
    public static PetDataHolder? GetPetDataHolder(string name)
    {
        return dataDictionary.TryGetValue(name, out  var dataHolder) ? dataHolder : null;
    }

    public static void RegisterPetData(string key, PetDataHolder petDataHolder) => dataDictionary.Add(key, petDataHolder);

    public static void Reset() => dataDictionary.Clear();
}