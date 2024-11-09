using System;
using System.ComponentModel;
using Fusion;
using UnityEngine;

public class PlayerStat: NetworkBehaviour
{
    [Networked]
    public int level {  get; set; }
    private void OnValidate()
    {
       // OnLevelChanged();
    }
    public void OnLevelChanged()
    {
        /*UpdateBaseStat(level, multipleHealth, multipleMana, multipleDamage, multipleDefend,
            multipleMagicResistance, multipleCriticalChance, multipleCriticalDamage, multipleMoveSpeed, multipleAttackSpeed);*/
    }
    
    [Networked] public float b_maxHealth {  get; set; }
    [Networked] public float currentHealth { get; set; }
    [Networked] public float b_maxMana { get; set; }
    [Networked] public float currentMana { get; set; }
    [Networked] public int maxXP { get; set; }
    [Networked] public int currentXP { get; set; }
    [Networked] public float b_damage { get; set; }
    [Networked] public int b_defend { get; set; }
    [Networked] public float b_magicResistance { get; set; }
    [Networked] public float b_magicAmpli { get; set; }
    [Networked] public float b_criticalChance { get; set; }
    [Networked] public float b_criticalDamage { get; set; }
    [Networked] public int b_moveSpeed { get; set; }
    [Networked] public int b_attackSpeed { get; set; }
    [Header("Full Stat")]
    public float maxHealth;
    public float maxMana;
    public float damage;
    public int defend;
    public float magicResistance;
    public float magicAmpli;
    public float criticalChance;
    public float criticalDamage;
    public int moveSpeed;
    public int attackSpeed;
    [Space(1)]
    [Header("Multiple Stat")]
    public float multipleHealth;
    public float multipleMana;
    public float multipleDamage;
    public int multipleDefend;
    public float multipleMagicResistance;
    public float multipleMagicAmpli;
    public float multipleCriticalChance;
    public float multipleCriticalDamage;
    public int multipleMoveSpeed;
    public int multipleAttackSpeed;
    


    public PlayerStat(float maxHealth, float maxMana, float damage, int maxXP, int defend,
       float magicResistance, float criticalChance, float criticalDamage, int moveSpeed)
    {
        this.level = 1;
        this.b_maxHealth = maxHealth;currentHealth=maxHealth;
        this.b_maxMana = maxMana;this.currentMana = maxMana;
        this.maxXP = maxXP;this.currentXP = 0;
        this.b_damage = damage;this.b_defend = defend; this.b_magicResistance = magicResistance;
        this.b_criticalChance = criticalChance; this.b_criticalDamage = criticalDamage;
        this.b_moveSpeed = moveSpeed;
    }
    public void UpdateBaseStat(int level, float multipleHealth, float multipleMana, float multipleDamage, int multipleDefend,
        float multipleMagicResistance, float multipleMagicAmpli,
        float multipleCriticalChance,float multipleCriticalDamage,int multipleMoveSpeed,int multipleAttackSpeed)
    {
        b_maxHealth = 300 + (level-1) * multipleHealth; b_maxMana = 100 + (level - 1) * multipleMana;
        maxXP = 100 + (level - 1) * (level - 1) * 50;
        b_damage = 50 + (level - 1) * multipleDamage; b_defend= 5 + ((level - 1) * multipleDefend);
        b_magicResistance = 0.2f + (level - 1) * multipleMagicResistance; b_magicAmpli = 0 + (level - 1) * multipleMagicAmpli;
        b_criticalChance =0+ (level - 1) * multipleCriticalChance; b_criticalDamage= 0+ (level - 1) * multipleCriticalDamage;
        b_moveSpeed=300+((level - 1) * multipleMoveSpeed);
        b_attackSpeed=100 + ((level - 1) * multipleAttackSpeed);
    }
    public void UpgradeLevel()
    {
        level++;
        UpdateBaseStat(level, multipleHealth, multipleMana, multipleDamage, multipleDefend,
            multipleMagicResistance,multipleMagicAmpli, 
            multipleCriticalChance, multipleCriticalDamage, multipleMoveSpeed, multipleAttackSpeed);
        UpdateFullStat();
        currentHealth=maxHealth;
        currentMana=maxMana;
    }

    private void UpdateFullStat()
    {
        maxHealth = b_maxHealth; maxMana = b_maxMana;
        damage = b_damage; defend = b_defend;
        magicResistance = b_magicResistance;
        criticalChance = b_criticalChance; criticalDamage = b_criticalDamage;
        moveSpeed = b_moveSpeed; attackSpeed = b_attackSpeed;
    }
}
