using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DamageType
{
    Neutral,
    Fire,
    Poison,
    Paralyse,
    True
}

public class Entity : MonoBehaviour
{
    public string name;

    [Range(0, Mathf.Infinity)]  // Base Health amount.
    public float baseHealth = 300;
    [Range(0, Mathf.Infinity)]  // Base Armour amount.
    public float baseArmour = 225;
    [Range(0, Mathf.Infinity)]  // Base Shield capacity.
    public float baseShield = 450;
    [Range(0, Mathf.Infinity)]  // Amount of ticks to wait before shield starts to regen.
    public int baseTickShieldRegenDelay = 5;
    [Range(0, 1)]               // % of total shields to regen in 1 second.
    public float baseShieldRegen = 0.25f;

    [Range(0, Mathf.Infinity)]  // Movement speed multiplier.
    public float baseSpeed = 1;
    [Range(0, Mathf.Infinity)]  // Jump height multiplier.
    public float baseJumpHeight = 1;

    [Range(0, Mathf.Infinity)]  // Base Damage output.
    public float baseAttackDamage = 50;
    [Range(0, Mathf.Infinity)]  // Attack Speed multiplier.
    public float baseAttackSpeed = 1;
    [Range(0, Mathf.Infinity)]  // Chance do crit.
    public float baseCritChance = 0;
    [Range(0, Mathf.Infinity)]  // Damage Multipler.
    public float baseCritDamage = 2;

    [Range(1, Mathf.Infinity)]  // Damage Multipler for Fire element.
    public float baseFireDamage;
    [Range(1, Mathf.Infinity)]  // Damage Multipler for Poison element.
    public float basePoisonDamage;
    [Range(1, Mathf.Infinity)]  // Damage Multipler for Paralyse element.
    public float baseParalyseDamage;

    private float health;
    [Range(0, Mathf.Infinity)]
    private float armour;
    [Range(0, Mathf.Infinity)]
    private float shield;
    [Range(0, Mathf.Infinity)]
    private float speed;
    [Range(0, Mathf.Infinity)]
    private float jumpHeight;

    [Range(0, Mathf.Infinity)]
    private float removedArmour;
    private bool isShiedlRegenActive = true;
    [Range(0, Mathf.Infinity)]
    private int ticksToShieldRegen = 0;

    private bool isOnFire = false;
    [Range(0, Mathf.Infinity)]
    private float fireDamage = 0;
    [Range(0, Mathf.Infinity)]
    private float fireTimeLeft = 0;
    private bool isPoisoned = false;
    [Range(0, Mathf.Infinity)]
    private float poisonDamage = 0;
    [Range(0, Mathf.Infinity)]
    private float poisonTimeLeft = 0;
    private bool isParalysed = false;
    [Range(0, Mathf.Infinity)]
    private float paralysedTimeLeft = 0;

    [Range(0, Mathf.Infinity)]
    private float timeCount = 0;
    private bool isDead = false;

    private void Start()
    {
        health = baseHealth;
        armour = baseArmour;
        shield = baseShield;
        speed = baseSpeed;
        jumpHeight = baseJumpHeight;
    }

    public void Update()
    {
        if (isDead) return;

        // Check if player is dead.
        if (health <= 0)
        {
            isDead = true;
            return;
        }

        // Check if a tick event should trigger. Should trigger every 1 second.
        bool tickEvent = false;
        timeCount += Time.deltaTime;
        if (timeCount >= 1)
        {
            tickEvent = true;
            timeCount -= 1;
        }

        if (isShiedlRegenActive)
        {
            if (shield < baseShield)
            {
                float shieldToRegen = baseShield * baseShieldRegen * Time.deltaTime;
                if ((shieldToRegen + shield) >= baseShield)
                {
                    shield = baseShield;
                }
                else
                {
                    shield += shieldToRegen;
                }
            }
        }
        else
        {
            if (tickEvent)
            {
                ticksToShieldRegen++;
                if (ticksToShieldRegen >= baseTickShieldRegenDelay)
                {
                    ticksToShieldRegen = 0;
                    isShiedlRegenActive = true;
                }
            }
        }

        // Fire damage ticks neutral damage but reduces armour over time. Resets armour upon end of duration.
        if (isOnFire) 
        {
            if (tickEvent)
            {
                TakeDamage(fireDamage * Time.deltaTime, DamageType.Neutral);
                if (shield <= 0)
                {
                    float armourReduction = (armour * 5) / 100;
                    RemoveArmour(armourReduction);
                    removedArmour += (armourReduction);
                }
            }

            fireTimeLeft -= Time.deltaTime;
            if (fireTimeLeft <= 0)
            {
                fireTimeLeft = 0;
                fireDamage = 0;
                isOnFire = false;
                AddArmour(removedArmour);
                removedArmour = 0;
                return;
            }
        }

        // Poison damage ticks true damage ignoring armour but not shields.
        if (isPoisoned) 
        {
            if (tickEvent)
            {
                TakeDamage(poisonDamage * Time.deltaTime, DamageType.True);
            }

            poisonTimeLeft -= Time.deltaTime;
            if (poisonTimeLeft <= 0)
            {
                poisonTimeLeft = 0;
                poisonDamage = 0;
                isPoisoned = false;
                return;
            }
        }

        // Parlyse reduces effective player speed. Resets speed upon end of duration.
        if (isParalysed) 
        {
            paralysedTimeLeft -= Time.deltaTime;
            if (paralysedTimeLeft <= 0)
            {
                paralysedTimeLeft = 0;
                speed = baseSpeed;
                jumpHeight = baseJumpHeight;
                isParalysed = false;
                return;
            }
        }
    }

    public void TakeDamage(float damage, DamageType type)
    {
        bool isTrueDamage = false;
        switch (type)
        {
            case DamageType.Fire :
                fireDamage += damage;
                isOnFire = true;
                if (fireTimeLeft < 10)
                {
                    fireTimeLeft += 1;
                }
                break;

            case DamageType.Poison :
                if (shield <= 0)
                {
                    poisonDamage += damage;
                    isPoisoned = true;
                    if (poisonTimeLeft < 10)
                    {
                        poisonTimeLeft += 1;
                    }
                }
                break;

            case DamageType.Paralyse :
                if (shield <= 0)
                {
                    isParalysed = true;
                    if (paralysedTimeLeft < 10)
                    {
                        paralysedTimeLeft += 1;
                    }
                    speed = baseSpeed * 0.25f;
                    jumpHeight = baseJumpHeight * 0.25f;
                }
                break;

            case DamageType.True :
                isTrueDamage = true;
                break;
        }

        CalculateDamage(damage, isTrueDamage);
    }

    private void CalculateDamage(float damage, bool isTrueDamage)
    {
        isShiedlRegenActive = false;
        ticksToShieldRegen = 0;

        if (damage < 0)
        {
            Debug.LogWarning("Cannot take negative damage.");
            return;
        }

        if (!isTrueDamage)
        {
            if (shield >= 0)
            {
                if (shield >= damage)
                {
                    shield -= damage;
                }
                else
                {
                    damage -= shield;
                    shield = 0;
                }
            }
        }

        if (isTrueDamage)
        {
            health -= damage;
        }
        else
        {
            // Damage reduction is base off of Warframe's calculation.
            // Example 300ar value reduces by 50%, 900ar value reduces by 75%.
            health -= damage * (armour / (armour + 300));
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
            removedArmour += amount;
            armour -= amount;
        }
        else
        {
            removedArmour += armour;
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