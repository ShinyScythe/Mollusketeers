using PurrNet.StateMachine;
using System.Collections;
using UnityEngine;

public class WaitForPlayerState : StateNode
{
    [SerializeField] private int minPlayers = 2;

    public override void Enter(bool asServer)
    {
        base.Enter(asServer);

        StartCoroutine(WaitForPlayers());
    }

    private IEnumerator WaitForPlayers()
    {
        print("Waiting for players..");
        while (networkManager.playerCount < minPlayers)
        {
            yield return null;
        }
        machine.Next();
    }
}
