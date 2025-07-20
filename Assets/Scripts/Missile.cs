using Unity.Netcode;
using UnityEngine;

public class Missile : NetworkBehaviour
{
    [SerializeField] private float speed = 500;
    [SerializeField] private float lifetime = 10;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        
        Destroy(gameObject, lifetime);

        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    private void OnCollisionEnter()
    {
        if (!IsServer) return;

        Destroy(gameObject);
    }
}