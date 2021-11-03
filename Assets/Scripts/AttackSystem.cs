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
        blockable,
        parryable,
        onlyParryable,
        none
    }

    public AttackType AttackMake { get; set; }
}
