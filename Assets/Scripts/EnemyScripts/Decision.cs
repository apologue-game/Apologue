using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decision
{
    public int  id { get; set; }
    public float baseCooldown { get; set; }
    public float currentCooldown { get; set; }

    public Decision(int id, float cooldown)
    {
        this.id = id;
        this.baseCooldown = cooldown;
        this.currentCooldown = 0f;
    }
}
