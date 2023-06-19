using System.IO;
using System.Linq;
using EasyPets.EasyPets.Buffs;
using EasyPets.EasyPets.Logging;
using EasyPets.EasyPets.Pets;
using EasyPets.EasyPets.Services;
using ModGenesia;
using RogueGenesia.Data;
using UnityEngine;

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
            // PetTemplateLoader.Initialize(modPaths);
            RegisterPetBuffs();
            RegisterCustomPets();
        }

        private void RegisterCustomPets()
        {
            Log.Debug("RegisterCustomPets");
            var placeholderAnimations = LoadPlaceHolderAnimations();
            
            Log.Debug($"placeholderAnimations: {placeholderAnimations}");
            
            var petSO = PetAPI.AddCustomPet(
                petName: nameof(LittleMidas), 
                type: typeof(LittleMidas),
                animations: placeholderAnimations,
                requiredPetDLC: ERequiredPetDLC.Any
            );
            
            petSO.Unlocked = true;
            petSO.PetScale = new Vector2(1f, 1f);
            
            Log.Debug($"Little Midas Pet SO: {petSO}");
        }

        private PetAnimations LoadPlaceHolderAnimations()
        {
            var basePath = Path.Combine(ThisModData.ModDirectory.FullName, "Pets", "Sonic");
            Log.Debug($"Placeholder basePath: {basePath}");
            var icon = ModGenesia.ModGenesia.LoadSprite(Path.Combine(basePath, "icon.png"));

            Log.Debug($"Placeholder Icon: {icon}");

            var idleTexture = ModGenesia.ModGenesia.LoadPNGTexture(Path.Combine(basePath, "idle.png"));
            Log.Debug($"Placeholder idleTexture: {idleTexture}");

            var runTexture = ModGenesia.ModGenesia.LoadPNGTexture(Path.Combine(basePath, "run.png"));
            Log.Debug($"Placeholder runTexture: {runTexture}");

            var idleState = PetAPI.CreatePetAnimationState(idleTexture, new Vector2Int(10, 1));
            var runState = PetAPI.CreatePetAnimationState(runTexture, new Vector2Int(8, 1));

            return PetAPI.CreatePetAnimations(
                icon: icon,
                idleAnimationState: idleState,
                runAnimationState: runState
            );
        }

        private void RegisterPetBuffs()
        {
            Log.Debug("RegisterPetBuffs");
            BuffAPI.RegisterBuff(nameof(LittleMidasBuff));
        }

        //Called when game has finished to load all content
        public override void OnAllContentLoaded()
        {

        }
                        
        //Called when unloading your mod
        public override void OnModUnloaded()
        {
            PetDataRepository.Reset();
        }
    }
}