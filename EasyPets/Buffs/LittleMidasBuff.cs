using ModGenesia;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;
using RogueGenesia.GameManager;

namespace EasyPets.Buffs;

public class LittleMidasBuff : Buff
{
    private const int MONSTERS_TO_KILL_PER_GOLD = 10;
    private int numMonstersKilled;
    private int numGoldGiven;

    public LittleMidasBuff(IEntity owner, IEntity origin) : base(BuffAPI.GetBuffID(nameof(LittleMidasBuff)), owner, origin, duration: 0f, level: 1, buffStacking: BuffStacking.Ignore)
    {
        _hasDuration = false;
        OnBuffStart();
    }

    // Default Buff Icon is loaded if not overridden
    // public override void LoadBuffIcon()
    // {
    //     base.LoadBuffIcon();
    // }

    public override void OnBuffStart()
    {
        GameEventManager.OnMonsterKilled.AddListener(OnMonsterKilled);
        numMonstersKilled = 0;
    }

    private void OnMonsterKilled(Monster monster)
    {
        numMonstersKilled++;
        if (numMonstersKilled % MONSTERS_TO_KILL_PER_GOLD == 0)
        {
            GameData.PlayerDatabase[0].Gold += 1;
            numGoldGiven++;
        }
    }

    public override void OnBuffEnd()
    {
        GameEventManager.OnMonsterKilled.RemoveListener(OnMonsterKilled);
    }

    public override string GetName() => "Little Midas";

    public override string GetDescription() =>
        $"Little Midas is with you, giving you a golden coin for every 10 monsters slain! So far, you have earned {numGoldGiven} gold!";
}