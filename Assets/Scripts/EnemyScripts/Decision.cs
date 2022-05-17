using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decision
{
    public int  Id { get; set; }
    //Conditions to make a decision
    public float BaseCooldown { get; set; }
    public float CurrentCooldown { get; set; }
    public float Range { get; set; }
    public bool InRange { get; set; }

    public Decision(int id, float cooldown)
    {
        this.Id = id;
        this.BaseCooldown = cooldown;
        this.CurrentCooldown = 0f;
        this.InRange = true;
    }

    public Decision(int id, float cooldown, float range)
    {
        this.Id = id;
        this.BaseCooldown = cooldown;
        this.CurrentCooldown = 0f;
        this.Range = range;
    }
}
