using System.Collections.Generic;
using EasyPets.EasyPets.Logging;
using RogueGenesia.Data;

namespace EasyPets.EasyPets
{
    public class CustomPet : PetData
    {
        private string name;
        private LocalizationDataList nameLocalization;
        private LocalizationDataList descLocalization;
        
        public CustomPet()
        {
            Log.Debug("CustomPet()");
        }

        public void InitializeData(string name, LocalizationDataList nameLocalization,
            LocalizationDataList descLocalization)
        {
            this.name = name;
            this.nameLocalization = nameLocalization;
            this.descLocalization = descLocalization;
        }
        
        public override string GetName()
        {
            return nameLocalization.GetLocalized();
        }

        public override string GetDescription()
        {
            return descLocalization.GetLocalized();
        }

        public override List<PetBehaviour> GetPetBehaviours()
        {
            Log.Debug("CustomPet.GetPetBehaviours()");
            return base.GetPetBehaviours();
        }
    }
} 