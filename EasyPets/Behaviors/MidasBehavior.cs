using EasyPets.EasyPets.Buffs;
using EasyPets.EasyPets.Logging;
using ModGenesia;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;

namespace EasyPets.EasyPets.Behaviors;

public class MidasBehavior : PetBehaviour
{
    public override string GetBehaviourName() => "MidasBehavior";
    
    public override void OnPickBehaviour(Pet pet)
    {
        Log.Debug("Applying LittleMidasBuff");
        pet.Player.AddBuff(new LittleMidasBuff(pet.Player, pet.Player));
    }

    public override void Update(Pet pet)
    {
        base.Update(pet);
    }

    public override bool CanChangebehaviour(Pet pet) => true;

    public override float GetPickWeight(Pet pet)
    {
        return pet.Player.HasBuff(BuffAPI.GetBuffID(nameof(LittleMidasBuff))) ? 0f : 100f;
    }
}