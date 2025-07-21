using Unity.Netcode;
using UnityEngine;

public class Missile : NetworkBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float lifetime = 10;

    public override void OnNetworkSpawn()
    {
        Destroy(gameObject, lifetime);

        if (!IsServer) return;

        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    private void OnCollisionEnter()
    {
        if (!IsServer) return;

        Destroy(gameObject);
    }
}