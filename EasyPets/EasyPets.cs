using System.IO;
using EasyPets.Buffs;
using EasyPets.Collectibles;
using EasyPets.Helpers;
using EasyPets.Logging;
using EasyPets.Pets;
using EasyPets.Services;
using ModGenesia;
using RogueGenesia.Data;
using UnityEngine;

namespace EasyPets
{
    public class EasyPetsMod : RogueGenesiaMod
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
            // var modPaths = ModLoader.EnabledMods.Select(mod => mod.ModDirectory.FullName).ToList();
            // PetTemplateLoader.Initialize(modPaths);
            RegisterCustomPets();
        }

        private void RegisterCustomPets()
        {
            Log.Debug("RegisterCustomPets");
            var placeholderAnimations = LoadPlaceHolderAnimations();
            
            Log.Debug($"placeholderAnimations: {placeholderAnimations}");
            RegisterLittleMidas(placeholderAnimations);
        }

        private static void RegisterLittleMidas(PetAnimations animations)
        {
            Log.Debug("RegisterLittleMidas");

            var nameLocalization = LocalizationHelper.GetLocalizationDataListFrom("en", "Midas Coin");
            var descLocalization = LocalizationHelper.GetLocalizationDataListFrom("en", "Midas Coin - It's the muns");

            // var midasCoinsSO = ContentAPI.AddCustomCollectible(
            //     key: nameof(MidasCoin),
            //     type: typeof(MidasCoin),
            //     CollectibleSprite: null,
            //     localisedName: nameLocalization,
            //     localisedDescription: descLocalization,
            //     pickUpSound: null
            // );
            //
            // Log.Debug($"midasCoinSO: {midasCoinsSO}");
            Log.Debug($"Modded collectibles: ");
            foreach (var collectible in ModdedGameData.ModdedCollectible)
            {
                Log.Debug($"{collectible.name} - {collectible}");
            }
            Log.Debug($"All collectibles: ");
            foreach (var collectible in GameDataGetter.GetAllCollectibles())
            {
                Log.Debug($"name: {collectible.name} - collectible: {collectible.GetCollectible()}");
            }
            
            Log.Debug($"All collectible constructors: ");
            foreach (var collectible in CollectibleSO.CollectibleConstructorList)
            {
                Log.Debug($"{collectible.Key} - {collectible.Value}");
            }
            
            BuffAPI.RegisterBuff(nameof(LittleMidasBuff));

            var petSO = PetAPI.AddCustomPet(
                petName: nameof(LittleMidas), 
                type: typeof(LittleMidas),
                animations: animations,
                requiredPetDLC: ERequiredPetDLC.Any
            );
            
            petSO.Unlocked = true;
            petSO.PetScale = new Vector2(1f, 1f);
            
            Log.Debug($"Little Midas Pet SO: {petSO}");
        }

        private PetAnimations LoadPlaceHolderAnimations()
        {
            var basePath = Path.Combine(ThisModData.ModDirectory.FullName, "Pets", "LittleMidas");
            Log.Debug($"Placeholder basePath: {basePath}");
            var icon = ModGenesia.ModGenesia.LoadSprite(Path.Combine(basePath, "icon.png"));

            Log.Debug($"Placeholder Icon: {icon}");

            var idleTexture = ModGenesia.ModGenesia.LoadPNGTexture(Path.Combine(basePath, "idle.png"));
            Log.Debug($"Placeholder idleTexture: {idleTexture}");

            var runTexture = ModGenesia.ModGenesia.LoadPNGTexture(Path.Combine(basePath, "run.png"));
            Log.Debug($"Placeholder runTexture: {runTexture}");

            var idleState = PetAPI.CreatePetAnimationState(idleTexture, new Vector2Int(5, 1));
            var runState = PetAPI.CreatePetAnimationState(runTexture, new Vector2Int(4, 1));

            return PetAPI.CreatePetAnimations(
                icon: icon,
                idleAnimationState: idleState,
                runAnimationState: runState
            );
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