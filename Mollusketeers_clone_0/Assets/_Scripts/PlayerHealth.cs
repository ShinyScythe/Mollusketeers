using NUnit.Framework;
using PurrNet;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private SyncVar<int> health = new(100);
    [SerializeField] private int selfLayer, otherLayer;

    public Action<PlayerHealth> OnDeath_Server;
    public int Health => health;

    protected override void OnSpawned()
    {
        base.OnSpawned();

        var actualLayer = isOwner ? selfLayer : otherLayer; // Sets layer depending on ownership
        SetLayerRecursive(gameObject, actualLayer);

        if (isOwner)
        {
            health.onChanged += OnHealthChanged;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        health.onChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int newHealth)
    {
        InstanceHandler.GetInstance<MainGameView>().UpdateHealth(newHealth);
    }

    private void SetLayerRecursive(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform t in obj.transform)
        {
            SetLayerRecursive(t.gameObject, layer);
        }
    }
    
    
    
    [ServerRpc(requireOwnership:false)]
    public void ChangeHealth(int amount)
    {
        health.value += amount;

        if (health.value <= 0)
        {
            OnDeath_Server?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
