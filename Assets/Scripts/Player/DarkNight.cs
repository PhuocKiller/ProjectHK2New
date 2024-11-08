using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkNight : PlayerController
{
    
    [SerializeField] public Transform normalAttackTransform, skill_1Transform, skill_2Transform, ultimateTransform;
    TickTimer timerSkill2;

    public override void Spawned()
    {
        base.Spawned();
       /* playerStat = new PlayerStat(maxHealth: 300, maxMana: 100, damage: 50, maxXP: 100, defend: 5, magicResistance: 0.2f,
            criticalChance: 0f, criticalDamage: 0f, moveSpeed: 300);*/
    }

    public override void NormalAttack(GameObject VFXEffect, float levelDamage, bool isPhysicDamage,
        bool isMakeStun = false, bool isMakeSlow = false, bool isMakeSilen = false, float timeTrigger = 0f, 
        float TimeEffect = 0f)
    {
        base.NormalAttack(VFXEffect, levelDamage, isPhysicDamage, timeTrigger: timeTrigger);
        StartCoroutine(DelaySpawnAttack(VFXEffect, levelDamage, isPhysicDamage, timeTrigger: timeTrigger));
    }
    IEnumerator DelaySpawnAttack(GameObject VFXEffect, float levelDamage, bool isPhysicDamage,
        bool isMakeStun = false, bool isMakeSlow = false, bool isMakeSilen = false, float timeTrigger = 0f,
        float TimeEffect = 0f)
    {
        yield return new WaitForSeconds(0.5f);
        Runner.Spawn(VFXEffect, normalAttackTransform.transform.position, normalAttackTransform.rotation, inputAuthority: Object.InputAuthority
     , onBeforeSpawned: (NetworkRunner runner, NetworkObject obj) =>
     {
         obj.GetComponent<DarkNight_Attack>().SetUp(normalAttackTransform, playerStat.b_damage, true, timeTrigger: timeTrigger);
     }
                        );
    }
    public override void Skill_1(GameObject VFXEffect, float levelDamage, bool isPhysicDamage,
        bool isMakeStun = false, bool isMakeSlow = false, bool isMakeSilen = false, float timeTrigger = 0f, float TimeEffect = 0f)
    {
        base.Skill_1(VFXEffect, levelDamage, isPhysicDamage, timeTrigger: timeTrigger);
        NetworkObject obj = Runner.Spawn(VFXEffect, skill_1Transform.position, skill_1Transform.rotation, Object.InputAuthority,
            onBeforeSpawned: (NetworkRunner runner, NetworkObject obj) =>
            {
                obj.GetComponent<DarkNight_Attack>().SetUp(skill_1Transform, levelDamage, true, timeTrigger: timeTrigger);
                StartCoroutine(DelaySkill_1_Collider(obj));
            });
    }
    IEnumerator DelaySkill_1_Collider(NetworkObject obj)
    {
        yield return new WaitForSeconds(0.5f);
        obj.GetComponent<CapsuleCollider>().enabled = true;
    }
    public override void Skill_2(GameObject VFXEffect, float levelDamage, bool isPhysicDamage,
        bool isMakeStun = false, bool isMakeSlow = false, bool isMakeSilen = false, float timeTrigger = 0f, float TimeEffect = 0f)
    {
        base.Skill_2(VFXEffect, levelDamage, isPhysicDamage,timeTrigger: timeTrigger);
        NetworkObject obj = Runner.Spawn(VFXEffect, skill_2Transform.position, skill_2Transform.rotation, Object.InputAuthority,
            onBeforeSpawned: (NetworkRunner runner, NetworkObject obj) =>
            {
                obj.GetComponent<DarkNight_Attack>().SetUp(skill_2Transform, levelDamage, true, timeTrigger: timeTrigger);
            });
    }
    public override void Ultimate(GameObject VFXEffect, float levelDamage, bool isPhysicDamage,
        bool isMakeStun = false, bool isMakeSlow = false, bool isMakeSilen = false, float timeTrigger = 0f, float TimeEffect = 0f)
    {
        base.Ultimate(VFXEffect, levelDamage, isPhysicDamage, timeTrigger: timeTrigger);
        NetworkObject obj = Runner.Spawn(VFXEffect, ultimateTransform.position, ultimateTransform.rotation, Object.InputAuthority,
            onBeforeSpawned: (NetworkRunner runner, NetworkObject obj) =>
            {
                obj.GetComponent<DarkNight_Attack>().SetUp(ultimateTransform, levelDamage, true, timeTrigger: timeTrigger);
                StartCoroutine(DelayUltimateCollider(obj));
            });
    }
    IEnumerator DelayUltimateCollider(NetworkObject obj)
    {
        yield return new WaitForSeconds(1f);
        obj.GetComponent<SphereCollider>().enabled = true;
    }
}

