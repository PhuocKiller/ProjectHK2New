using System.ComponentModel;
using UnityEngine;

public class PlayerStat: MonoBehaviour
{
    public int level;
    private void OnValidate()
    {
        OnLevelChanged();
    }
    public void OnLevelChanged()
    {
        UpdateBaseStat(level, multipleHealth, multipleMana, multipleDamage, multipleDefend,
            multipleMagicResistance, multipleCriticalChance, multipleCriticalDamage, multipleMoveSpeed, multipleAttackSpeed);
    }
    public float b_maxHealth, multipleHealth;
    public float maxHealth;
    public float currentHealth;
    public float b_maxMana, multipleMana;
    public float maxMana;
    public float currentMana;
    public int maxXP;
    public int currentXP;
    public float b_damage, multipleDamage;
    public float damage;
    public int b_defend;public int multipleDefend;
    public int defend;
    public float b_magicResistance, multipleMagicResistance;
    public float magicResistance;
    public float b_criticalChance, multipleCriticalChance;
    public float criticalChance;
    public float b_criticalDamage, multipleCriticalDamage;
    public float criticalDamage;
    public int b_moveSpeed;public int multipleMoveSpeed;
    public int moveSpeed;
    public int b_attackSpeed; public int multipleAttackSpeed;
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
