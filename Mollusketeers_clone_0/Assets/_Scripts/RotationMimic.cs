using PurrNet;
using Unity.Cinemachine;
using UnityEngine;

public class RotationMimic : NetworkBehaviour
{
    [SerializeField] private Transform mimicObject;

    protected override void OnSpawned(bool asServer)
    {
        base.OnSpawned(asServer);

        enabled = isOwner;
    }
    private void Update()
    {
        if (!mimicObject)
            return;

        transform.rotation = mimicObject.rotation;
    }
}
