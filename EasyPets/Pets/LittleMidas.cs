using System.Collections.Generic;
using EasyPets.Behaviors;
using RogueGenesia.Data;

namespace EasyPets.Pets;

public class LittleMidas : PetData
{
    public override string GetName() => "Little Midas";

    public override string GetDescription() => "Gives you a coin for every 10 enemies";

    public override List<PetBehaviour> GetPetBehaviours()
    {
        return new List<PetBehaviour>
        {
            new IdlePetBehaviour(),
            new MoveToADifferentPositionPetBehaviour(),
            new MidasBehavior(),
            new SpewGoldBehavior(),
        };
    }
}