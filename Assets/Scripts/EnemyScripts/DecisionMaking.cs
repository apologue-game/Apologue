using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionMaking
{
    System.Random rnd = new System.Random();
    int randomNumber = 0;
    public List<Decision> Decisions;
    public Decision madeDecision;

    public DecisionMaking()
    {

    }

    public DecisionMaking(List<Decision> decisions)
    {
        this.Decisions = decisions;
    }

    public Decision DecisionCalculation()
    {
        randomNumber = rnd.Next(0, Decisions.Count);
        madeDecision = Decisions[randomNumber];

        if (Time.time < madeDecision.currentCooldown)
        {
            return null;
        }

        return madeDecision;
    }
}