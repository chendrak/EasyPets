using RogueGenesia.Actors.Survival;

namespace EasyPets.Collectibles;

public class MidasCoin : GoldCollectible
{
    public override string GetName(int ID) => $"MidasCoin ID: {ID}";

    public override float GetDropChanceFromCrate() => 0f;
}