using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameEventListener))]
public class Entity : MonoBehaviour
{
    public string name;


    [Min(0)]    // Base Health amount.
    public float baseHealth = 300;
    [Min(0)]    // Base Armour amount.
    public float baseArmour = 225;
    [Min(0)]    // Base Shield capacity.
    public float baseShield = 450;
    [Min(0)]    // Amount of ticks to wait before shield starts to regen.
    public int baseTickShieldRegenDelay = 5;
    [Min(0)]    // % of total shields to regen in 1 second.
    public float baseShieldRegen = 0.25f;

    [Min(0)]    // Movement speed multiplier.
    public float baseSpeed = 1;
    [Min(0)]    // Jump height multiplier.
    public float baseJumpHeight = 1;

    [Min(0)]    // Base Damage output.
    public float baseAttackDamage = 50;
    [Min(0)]    // Attack Speed multiplier.
    public float baseAttackSpeed = 1;
    [Min(0)]    // Chance do crit.
    public float baseCritChance = 0;
    [Min(0)]    // Damage Multipler.
    public float baseCritDamage = 2;

    [Min(0)]    // Damage Multipler for Fire element.
    public float baseFireDamage = 1;
    [Min(0)]    // Damage Multipler for Poison element.
    public float basePoisonDamage = 1;
    [Min(0)]    // Damage Multipler for Paralyse element.
    public float baseParalyseDamage = 1;

    [Min(0)]
    private float health;
    [Min(0)]
    private float armour;
    [Min(0)]
    private float shield;
    [Min(0)]
    private float speed;
    [Min(0)]
    private float jumpHeight;

    private bool isShiedlRegenActive = true;
    [Min(0)]
    private int ticksToShieldRegen = 0;

    private bool isImmune = false;
    [Min(0)]
    private float immunityTimeLeft = 0;

    [Range(0, 1)]
    private float fireProcArmourMultiplier = 0;

    private bool isOnFire = false;
    [Min(0)]
    private float fireTimeLeft = 0;
    private bool isPoisoned = false;
    [Min(0)]
    private float poisonTimeLeft = 0;
    private bool isParalysed = false;
    [Min(0)]
    private float paralysedTimeLeft = 0;

    private bool isDead = false;

    private void Awake()
    {
        health = baseHealth;
        armour = baseArmour;
        shield = baseShield;
        speed = baseSpeed;
        jumpHeight = baseJumpHeight;
    }

    public void TickUpdate(Component sender, object data)
    {
        if (sender is not TimeTickSystem || data is not int) return;

        if (isShiedlRegenActive)
        {
            if (shield < baseShield)
            {
                float regenAmount = (float)(baseShield * baseShieldRegen * TimeTickSystem.TICK_TIMER_MAX);
                if (shield + regenAmount >= baseShield)
                {
                    shield = baseShield;
                }
                else
                {
                    shield += regenAmount;
                }
            }
        }
        else
        {
            ticksToShieldRegen++;
            if (ticksToShieldRegen >= baseTickShieldRegenDelay)
            {
                isShiedlRegenActive = true;
                ticksToShieldRegen = 0;
            }
        }

        if (isOnFire)
        {
            TakeDamage(DamageSystem.CalculateDoT(health, shield, DoTType.Fire), DamageType.Fire, false);

            fireTimeLeft -= TimeTickSystem.TICK_TIMER_MAX;
            if (fireTimeLeft <= 0)
            {
                fireProcArmourMultiplier = 1;
                isOnFire = false;
                fireTimeLeft = 0;
            }
        }

        if (isPoisoned)
        {
            
            TakeDamage(DamageSystem.CalculateDoT(health, shield, DoTType.Poison), DamageType.Poison, false);

            poisonTimeLeft -= TimeTickSystem.TICK_TIMER_MAX;
            if (poisonTimeLeft <= 0)
            {
                isPoisoned = false;
                poisonTimeLeft = 0;
            }
        }

        if (isParalysed)
        {
            ticksToShieldRegen = 0;

            paralysedTimeLeft -= TimeTickSystem.TICK_TIMER_MAX;
            if (paralysedTimeLeft <= 0)
            {
                isParalysed = false;
                paralysedTimeLeft = 0;
            }
        }

        if (isImmune)
        {
            immunityTimeLeft -= TimeTickSystem.TICK_TIMER_MAX;
            if (immunityTimeLeft <= 0)
            {
                isImmune = false;
                immunityTimeLeft = 0;
            }
        }
    }

    public void TakeDamage(float damage, DamageType type)
    {
        TakeDamage(damage, type, true);
    }

    private void TakeDamage(float rawDamage, DamageType type, bool isProc)
    {
        if (isImmune) return;

        if (isProc) // Only proc from direct damage and not Dot proc damage.
        {
            switch (type)
            {
                case DamageType.Fire:
                    if (shield > 0) break; // Only proc when shields are down.
                    float calculatedArmour = fireProcArmourMultiplier - DamageSystem.fireArmourPercent;
                    if (calculatedArmour > 0)
                    {
                        fireProcArmourMultiplier = calculatedArmour;
                    }
                    else
                    {
                        fireProcArmourMultiplier = 0;
                    }  
                    isOnFire = true;
                    fireTimeLeft += DamageSystem.fireProcTime;
                    break;

                case DamageType.Poison:
                    if (shield > 0) break; // Only proc when shields are down.
                    isPoisoned = true;
                    poisonTimeLeft += DamageSystem.poisonProcTime;
                    break;

                case DamageType.Electricity:
                    isParalysed = true;
                    paralysedTimeLeft += DamageSystem.paralyseProcTime;
                    break;
            }
        }

        float calculatedDamage = 0f;

        if (shield > 0) // Calculate shield damage.
        {
            calculatedDamage = DamageSystem.CalculateDamage(rawDamage, 0, type, ProtectionType.Shield);
            if (calculatedDamage >= shield)
            {
                isImmune = true;
                immunityTimeLeft = DamageSystem.immunityFrameTime;
            }
            else
            {
                shield -= calculatedDamage;
            }
            return;
        }

        ProtectionType protectionType = ProtectionType.Health;
        if (armour > 0 && fireProcArmourMultiplier != 0) // Calculate health damage with or without armour.
        {
            protectionType = ProtectionType.Armour;
        }
        calculatedDamage = DamageSystem.CalculateDamage(rawDamage, armour * fireProcArmourMultiplier, type, protectionType);
        if (calculatedDamage >= health)
        {
            health = 0;
            isDead = true;
        }
        else
        {
            health -= calculatedDamage;
        }
    }

    public void RegenHealth(float amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot heal negative amount.");
            return;
        }

        if ((amount + health) >= baseHealth)
        {
            health = baseHealth;
        }
        else
        {
            health += amount;
        }
    }

    public void AddArmour(float amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot add negative armour amount.");
            return;
        }

        if ((amount + armour) >= baseArmour)
        {
            armour = baseArmour;
        }
        else
        {
            armour += amount;
        }
    }

    public void RemoveArmour(float amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot remove negative armour amount.");
            return;
        }

        if (armour >= amount)
        {
            armour -= amount;
        }
        else
        {
            armour = 0;
        }
    }

    public float GetHealth()
    {
        return health;
    }
    public float GetArmour()
    {
        return armour;
    }
    public float GetShield()
    {
        return shield;
    }
    public bool IsDead()
    {
        return isDead;
    }
}