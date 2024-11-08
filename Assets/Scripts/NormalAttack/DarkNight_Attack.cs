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
    public float timerDespawn, timeEffect;
    public float damage;
    public bool isPhysicDamage, isMakeStun, isMakeSlow, isMakeSilen, isDestroyWhenCollider;
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
    public void SetUp(Transform parentObject, float levelDamage, bool isPhysicDamage,
        bool isMakeStun = false, bool isMakeSlow = false, bool isMakeSilen = false, float timeTrigger = 0f,
        float timeEffect = 0f, bool isDestroyWhenCollider = false)
    {
        transform.SetParent(parentObject);
        damage = levelDamage;
        this.isPhysicDamage = isPhysicDamage;
        this.isMakeStun = isMakeStun;
        this.isMakeSlow = isMakeSlow;
        this.isMakeSilen = isMakeSilen;
        this.timeEffect = timeEffect;
        this.isDestroyWhenCollider = isDestroyWhenCollider;
        timerDespawn = timeTrigger;
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
            other.gameObject.GetComponent<ICanTakeDamage>().ApplyDamage(damage, isPhysicDamage, Object.InputAuthority,
                () =>
                {
                  if(isDestroyWhenCollider)
                    {
                        Runner.Despawn(Object);
                    }

                }
                );
            other.gameObject.GetComponent<ICanTakeDamage>().ApplyEffect(Object.InputAuthority,
                callback:() =>
                {
                }
                );
        }
    }
}
