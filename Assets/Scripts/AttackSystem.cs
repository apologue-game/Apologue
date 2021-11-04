using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem
{
    public AttackSystem(int attackDamage, AttackType attackType)
    {
        AttackDamage = attackDamage;
        AttackMake = attackType;
    }

    public int AttackDamage { get; set; }

    public enum AttackType
    {
        normal,
        onlyParryable,
        special
    }

    public AttackType AttackMake { get; set; }
}
