using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkNight_Attack : NetworkBehaviour
{
    private Vector3 direction;
    private NetworkRigidbody rb;

    private List<Collider> collisions = new List<Collider>();

    private TickTimer timer;
    public float timerDespawn;
    public float damage;
    public override void Spawned()
    {
        base.Spawned();
        collisions.Clear();
        rb = GetComponent<NetworkRigidbody>();
        if (HasStateAuthority && HasInputAuthority)
        {
            timer = TickTimer.CreateFromSeconds(Runner, timerDespawn);
        }
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (HasStateAuthority && timer.Expired(Runner)
            )
        {
            Runner.Despawn(Object);
        }

    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HasStateAuthority
            && other.gameObject.layer == 7
            && other.gameObject.GetComponent<NetworkObject>().HasStateAuthority == false
            && collisions.Count == 0
            /* && (other.gameObject.GetComponent<CharacterController>().GetCurrentState() == 0
                   ||
               other.gameObject.GetComponent<CharacterController>().GetCurrentState() == 1)*/
            )
        {
            collisions.Add(other);
            other.gameObject.GetComponent<ICanTakeDamage>().ApplyDamage(damage, Object.InputAuthority,
                () =>
                {
                  //  Runner.Despawn(Object);
                }
                );
        }
    }
}
