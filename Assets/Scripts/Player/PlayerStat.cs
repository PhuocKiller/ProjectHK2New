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
        UpdateBaseStat(level, multipleHealth, multipleMana, multipleDamage, multipleDefend,
            multipleMagicResistance, multipleCriticalChance, multipleCriticalDamage, multipleMoveSpeed, multipleAttackSpeed);
    }
    [Networked] public float b_maxHealth {  get; set; }
    public float multipleHealth;
    public float maxHealth;
    [Networked]  public float currentHealth { get; set; }
    [Networked] public float b_maxMana { get; set; }
    public float multipleMana;
    public float maxMana;
    [Networked]  public float currentMana { get; set; }
    [Networked] public int maxXP { get; set; }
    [Networked] public int currentXP { get; set; }
    [Networked] public float b_damage { get; set; }
    public float multipleDamage;
    public float damage;
    [Networked] public int b_defend { get; set; }
    public int multipleDefend;
    public int defend;
    [Networked]  public float b_magicResistance { get; set; }
    public float multipleMagicResistance;
    public float magicResistance;
    [Networked]  public float b_criticalChance { get; set; }
    public float multipleCriticalChance;
    public float criticalChance;
    [Networked]  public float b_criticalDamage { get; set; }
    public float multipleCriticalDamage;
    public float criticalDamage;
    [Networked]  public int b_moveSpeed { get; set; }
    public int multipleMoveSpeed;
    public int moveSpeed;
    [Networked] public int b_attackSpeed { get; set; }
    public int multipleAttackSpeed;
    public int attackSpeed;
    


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
        float multipleMagicResistance, float multipleCriticalChance, float multipleCriticalDamage,int multipleMoveSpeed,int multipleAttackSpeed)
    {
        b_maxHealth = 300 + (level-1) * multipleHealth; b_maxMana = 100 + (level - 1) * multipleMana;
        b_damage= 50 + (level - 1) * multipleDamage; b_defend= 5 + ((level - 1) * multipleDefend);
        b_magicResistance = 0.2f + (level - 1) * multipleMagicResistance;
        b_criticalChance=0+ (level - 1) * multipleCriticalChance; b_criticalDamage= 0+ (level - 1) * multipleCriticalDamage;
        b_moveSpeed=300+((level - 1) * multipleMoveSpeed);
        b_attackSpeed=100 + ((level - 1) * multipleAttackSpeed);
    }
    public void UpgradeLevel()
    {
        Debug.Log("vo dya");
        level++;
        UpdateBaseStat(level, multipleHealth, multipleMana, multipleDamage, multipleDefend,
            multipleMagicResistance, multipleCriticalChance, multipleCriticalDamage, multipleMoveSpeed, multipleAttackSpeed);
        currentHealth =b_maxHealth;
    }
}
