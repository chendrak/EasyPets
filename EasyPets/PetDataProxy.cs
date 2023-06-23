using System.Collections.Generic;
using EasyPets.Logging;
using EasyPets.Services;
using RogueGenesia.Data;

namespace EasyPets.EasyPets
{
    public class PetDataProxy : PetData
    {
        private string name => linkedScriptableObject.name;

        private PetDataHolder dataHolder => PetDataRepository.GetPetDataHolder(name);
        
        private LocalizationDataList nameLocalization;
        private LocalizationDataList descLocalization;
        
        public PetDataProxy()
        {
            Log.Debug("CustomPet()");
        }

        public override string GetName()
        {
            return dataHolder.nameLocalization.GetLocalized();
        }

        public override string GetDescription()
        {
            return dataHolder.descriptionLocalization.GetLocalized();
        }

        public override List<PetBehaviour> GetPetBehaviours()
        {
            Log.Debug("CustomPet.GetPetBehaviours()");
            return dataHolder.behaviours;
        }
    }
}