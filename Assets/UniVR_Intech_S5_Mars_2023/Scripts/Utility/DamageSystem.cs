using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Kinetic,
    Fire,
    Poison,
    Electricity,
    True
}

public enum ProtectionType
{
    Health,
    Armour,
    Shield,
    Invicibility
}

public enum DoTType
{
    Fire,
    Poison
}

public static class DamageSystem
{
    public const float fireProcTime = 1f;
    public const float poisonProcTime = 0.8f;
    public const float paralyseProcTime = 0.6f;

    public const float immunityFrameTime = 1.2f;

    public const float fireArmourPercent = 0.05f;

    public static float CalculateDamage(float rawDamage, float armour, DamageType damageType, ProtectionType protectionType)
    {
        if (protectionType == ProtectionType.Invicibility) return 0;
        if (damageType == DamageType.True) return rawDamage;

        switch (protectionType)
        {
            case ProtectionType.Health:
                switch (damageType)
                {
                    case DamageType.Kinetic:
                        return rawDamage;

                    case DamageType.Fire:
                        return (float)(rawDamage * 1.25f);

                    case DamageType.Poison:
                        return (float)(rawDamage * 1.33f);

                    case DamageType.Electricity:
                        return (float)(rawDamage * 0.75f);
                }
                break;

            case ProtectionType.Armour:
                switch (damageType)
                {
                    case DamageType.Kinetic:
                        return CalculateArmourDamage(rawDamage * 1.25f, armour);

                    case DamageType.Fire:
                        return CalculateArmourDamage(rawDamage * 0.75f, armour);

                    case DamageType.Poison:
                        return CalculateArmourDamage(rawDamage * 0.5f, armour);

                    case DamageType.Electricity:
                        return CalculateArmourDamage(rawDamage, armour);
                }
                break;

            case ProtectionType.Shield:
                switch (damageType)
                {
                    case DamageType.Kinetic:
                        return (float)(rawDamage);

                    case DamageType.Fire:
                        return (float)(rawDamage * 0.5f);

                    case DamageType.Poison:
                        return 0;

                    case DamageType.Electricity:
                        return (float)(rawDamage * 1.5f);
                }
                break;
        }
        Debug.LogError("Something went wrong when calculating damage :\n(" + rawDamage + " | " + armour + " | " + damageType.ToString() + " | " + protectionType.ToString() + ").");
        return 0;
    }

    public static float CalculateDoT(float health, float shield, DoTType dotType)
    {
        float effectiveHealthPool = 0;
        if (shield > 0)
        {
            effectiveHealthPool = shield;
        }
        else
        {
            effectiveHealthPool = health;
        }

        float multiplier = 0;
        switch (dotType)
        {
            case DoTType.Fire:
                multiplier = 0.05f;
                break;

            case DoTType.Poison:
                multiplier = 0.1f;
                break;

            default:
                Debug.LogError("Something went wrong when calculating DoT : \n(" + health + " | " + shield + " | " + dotType + ").");
                return 0;
        }
        return (float)(effectiveHealthPool * multiplier);
    }

    private static float CalculateArmourDamage(float damage, float armour)
    {
        return (float)(damage * (armour / (armour + 300)));
    }
}
