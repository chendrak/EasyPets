using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using EasyPets.EasyPets.Logging;
using EasyPets.EasyPets.Templates;
using ModGenesia;
using RogueGenesia.Data;
using UnityEngine;

namespace EasyPets.EasyPets.Services;

public static class PetTemplateLoader
{
    public static void Initialize(List<string> modPaths)
    {
        LoadPets(modPaths);
    }

    private static void LoadPets(List<string> modPaths)
    {
        Log.Info($"Attempting to find custom pets in {string.Join(", ", modPaths)}");
        foreach (var modPath in modPaths)
        {
            // Scan for *.pets.json files in mods subfolders
            var petJsonFile = Directory.GetFiles(modPath, "*.pets.json");
            Log.Info($"Found pet json files: {string.Join(", ", petJsonFile)}");
            foreach (var jsonFile in petJsonFile)
            {
                try
                {
                    var assetBasePath = Path.GetDirectoryName(jsonFile);
                    LoadPetsFromFile(jsonFile, assetBasePath!);
                }
                catch (Exception ex)
                {
                    Log.Error($"Unable to load pets from file {jsonFile}: {ex}");
                }
            }
        }
    }
    
    private static void LoadPetsFromFile(string fileName, string assetBasePath)
    {
        Log.Info($"Loading pets from file {fileName}");

        var json = File.ReadAllText(fileName);
        var templateFile = JsonDeserializer.Deserialize<TemplateFile>(json);

        Log.Info($"Loaded {templateFile.Pets.Count} pet templates");

        var modSource = templateFile.ModSource ?? EasyPets.MOD_NAME;
        foreach (var template in templateFile.Pets)
        {
            Log.Debug($"Attempting to add {template.Name}");

            var icon = ModGenesia.ModGenesia.LoadSprite(Path.Combine(assetBasePath, template.IconPath!));
            if (icon == null)
            {
                Log.Error($"{template.Name}: Unable to load icon from: {template.IconPath}");
                continue;
            }

            Log.Debug($"Loading idle animation for {template.Name}");
            var idleAnimationResult = template.IdleAnimation?.ToPixelAnimationData(assetBasePath) ?? Results.Fail<PixelAnimationData>("No idle animation provided");
            if (idleAnimationResult.IsFailed) {
                Log.Error($"{template.Name}: Unable to load idle animation: {idleAnimationResult.ErrorMessage}");
                continue;
            }
            
            var runAnimationResult = template.RunAnimation?.ToPixelAnimationData(assetBasePath) ?? Results.Fail<PixelAnimationData>("No run animation provided");
            if (runAnimationResult.IsFailed) {
                Log.Error($"{template.Name}: Unable to load run animation: {idleAnimationResult.ErrorMessage}");
                continue;
            }

            var nameLocalizationResult = template.GetNameLocalizations();
            if (nameLocalizationResult.IsFailed) {
                Log.Error($"{template.Name}: Unable to get name localizations: {nameLocalizationResult.ErrorMessage}");
                continue;
            }

            var descLocalizationResult = template.GetDescriptionLocalizations();
            if (descLocalizationResult.IsFailed) {
                Log.Error($"{template.Name}: Unable to get description localizations: {descLocalizationResult.ErrorMessage}");
                continue;
            }

            try
            {
                var petSO = PetAPI.AddCustomPet(
                    petName: template.Name,
                    type: typeof(CustomPet),
                    animations: new PetAnimations
                    {
                        Icon = icon,
                        IdleAnimation = idleAnimationResult.Data,
                        RunAnimation = runAnimationResult.Data
                    },
                    ERequiredPetDLC.Dog
                );
                
                petSO.Unlocked = true;
                petSO.PetScale = new Vector2(template.SizeScale, template.SizeScale);

                var petData = petSO.GetPetData();
                (petData as CustomPet)?.InitializeData(petSO.name, nameLocalizationResult.Data, descLocalizationResult.Data);

            }
            catch (Exception ex)
            {
                Log.Error($"Error adding {template.Name}: {ex}");
            }
        }
    }
}