using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AttackSystem
{
    public enum AttackType
    {
        normal,
        blockable,
        parryable,
        onlyBlockable,
        onlyParryable
    }
}
