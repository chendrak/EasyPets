using System.Linq;
using EasyPets.EasyPets.Logging;
using EasyPets.EasyPets.Services;
using ModGenesia;

namespace EasyPets.EasyPets
{
    public class EasyPets : RogueGenesiaMod
    {
        public const string MOD_NAME = "EasyPets";
        //Called when loading your mod dll
        public override void OnModLoaded(ModData modData)
        {
            Log.Initialize(MOD_NAME);
        }

        //Called when game is loading the content from all mods
        public override void OnRegisterModdedContent()
        {
            var modPaths = ModLoader.EnabledMods.Select(mod => mod.ModDirectory.FullName).ToList();
            PetTemplateLoader.Initialize(modPaths);
        }
                              
        //Called when game has finished to load all content
        public override void OnAllContentLoaded()
        {

        }
                        
        //Called when unloading your mod
        public override void OnModUnloaded()
        {

        }
    }
}