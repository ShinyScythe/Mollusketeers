using UnityEngine;
using PurrNet.StateMachine;
using NUnit.Framework;
using System.Collections.Generic;

public class PlayerSpawningState : StateNode
{
    [SerializeField] private PlayerHealth playerPrefab;
    [SerializeField] private List<Transform> spawnPoints = new();

    public override void Enter(bool asServer)
    {
        base.Enter(asServer);
        if (!asServer ) 
        {
            return;
        }

        var spawnedPlayers = new List<PlayerHealth>();

        int currentSpawnIndex = 0;
        foreach (var player in networkManager.players) 
        {
            var spawnPoint = spawnPoints[currentSpawnIndex];
            print($"Spawning player {player} at spawn {spawnPoint}");

            var newPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            print($"Giving ownership to {player}.");
            newPlayer.GiveOwnership(player);

            print($"Adding {newPlayer} to List of Spawned Players.");
            spawnedPlayers.Add(newPlayer);

            print($"Spawned Players: {spawnedPlayers.Count}.");

            currentSpawnIndex++;

            if (currentSpawnIndex >= spawnPoints.Count)
            {
                currentSpawnIndex = 0;
            }
                

        }
        print($"Moving to next state.");
        machine.Next(spawnedPlayers);
    }

    public override void Exit(bool asServer)
    {
        print("Exiting.");
        base.Exit(asServer);

    }
}
