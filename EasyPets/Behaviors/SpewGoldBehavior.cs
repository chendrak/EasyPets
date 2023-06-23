using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using EasyPets.Buffs;
using EasyPets.Collectibles;
using EasyPets.Helpers;
using EasyPets.Logging;
using ModGenesia;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;
using RogueGenesia.GameManager;
using UnityEngine;

namespace EasyPets.Behaviors;

public class SpewGoldBehavior : PetBehaviour
{
    private const float SPEW_DURATION = 3f;
    private const float COINS_TO_SPEW = 10f;

    private float lastCoinSpewed = 0f;
    private float spewWaitTime = 0f;
    private int lastCoinId = 0;
    
    private ConcurrentDictionary<string, MidasCoin> coins = new ();

    public SpewGoldBehavior()
    {
        GameEventManager.OnCollectibleSpawn.AddListener(OnCollectibleSpawned);
    }

    private void OnCollectibleSpawned(string loot, Collectible collectible)
    {
        if (collectible is MidasCoin coin)
        {
            var wasAdded = coins.TryAdd(coin.GetName(), coin);

            var sourceLocation = coin.transform.position;
            var targetLocation = GetRandomPositionAroundPet(sourceLocation);

            CoroutineHelper.StartCoroutine(FlingCoinTo(coin, sourceLocation, targetLocation));
            Log.Debug($"OnCollectibleSpawned(loot: [{loot}], collectible: [{collectible}], wasAdded: [{wasAdded}]");
        }
    }

    private IEnumerator FlingCoinTo(MidasCoin coin, Vector3 sourceLocation, Vector3 targetLocation)
    {
        var flingInProgress = true;
        var force = GravityHelper.GetRequiredForce(sourceLocation, targetLocation);
        var velocity = force;
        var gravity = new Vector3(0.0f, -9.8f, 0.0f);

        while (flingInProgress)
        {
            velocity += gravity * Time.deltaTime * EnemyManager.GlobalTimescale;
            coin.transform.position += velocity * Time.deltaTime * EnemyManager.GlobalTimescale;

            yield return new WaitForEndOfFrame();
            flingInProgress = coin.transform.position.y <= 0.0;
        }
    }

    private Vector3 GetRandomPositionAroundPet(Vector3 sourceLocation)
    {
        float num = (float) (2.0 + (double) Random.value * 2.0);
        float f = (float) ((double) Random.value * 2.0 * 3.1415927410125732);
        var targetPositionOffset = new Vector3(Mathf.Sin(f), 0.0f, Mathf.Cos(f)) * num;
        return sourceLocation + targetPositionOffset;
    }

    public override string GetBehaviourName() => "SpewGoldBehavior";
    
    public override void OnPickBehaviour(Pet pet)
    {
        Log.Debug("Applying SpewGoldBehavior");
        m_durationLeft = SPEW_DURATION;
        lastCoinSpewed = 0f;
        spewWaitTime = SPEW_DURATION / COINS_TO_SPEW;
        Log.Debug($"spewWaitTime: {spewWaitTime}");
    }

    public override void Update(Pet pet)
    {
        m_durationLeft -= Time.deltaTime;
        var currentTime = Time.time;
        if (currentTime > lastCoinSpewed + spewWaitTime)
        {
            Log.Debug("spewWaitTime expired! Spawning MidasCoin");
            lastCoinSpewed = currentTime;

            MidasCoin coin = new MidasCoin();
            var posToSpawn = pet.transform.position;
            GameObject linkedObject = Object.Instantiate(coin.GetModel(), posToSpawn + new Vector3(Random.value * 0.2f, 0.5f, Random.value * 0.2f), Quaternion.Euler(0.0f, 45f, 0.0f), CollectibleManager.instance.CollectibleParent);
            linkedObject.name = coin.GetName(lastCoinId++);
            coin.LinkedObject = linkedObject;
            coin.GoldValue = 10f;
            coin.transform.position = linkedObject.transform.position;

            // CollectibleManager.SpawnCollectible(nameof(MidasCoin), pet.transform.position, new double[]
            // {
            //     10f,
            //     0f,
            //     0f
            // },
            //     0f,
            //     0f
            // );

        }
    }
    
    // public override bool CanChangebehaviour(Pet pet) => base.CanChangebehaviour(pet);

    public override float GetPickWeight(Pet pet)
    {
        return pet.Player.HasBuff(BuffAPI.GetBuffID(nameof(LittleMidasBuff))) ? 0f : 100f;
    }
}