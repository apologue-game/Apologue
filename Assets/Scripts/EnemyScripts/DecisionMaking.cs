using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionMaking
{
    System.Random rnd = new System.Random();
    int randomNumber = 0;
    public List<Decision> Decisions;
    public Decision madeDecision;
    Decision[] abilitiesWithRange;

    public DecisionMaking()
    {

    }

    public DecisionMaking(List<Decision> decisions)
    {
        this.Decisions = decisions;
    }

    public Decision DecisionCalculation_Cooldown()
    {
        randomNumber = rnd.Next(0, Decisions.Count);
        madeDecision = Decisions[randomNumber];

        if (Time.time < madeDecision.CurrentCooldown)
        {
            return null;
        }

        return madeDecision;
    }

    public Decision DecisionCalculation_Cooldown_Range(float currentDistance)
    {
        foreach (Decision decision in Decisions)
        {
            if (decision.Range > 0)
            {
                decision.InRange = false;
                if (decision.Range > currentDistance)
                {
                    decision.InRange = true;
                }
            }
        }
        randomNumber = rnd.Next(0, Decisions.Count);
        Debug.Log("RND: " + randomNumber);
        madeDecision = Decisions[randomNumber];
        Debug.Log("DECISION: " + madeDecision.Id);
        if (Time.time < madeDecision.CurrentCooldown || !madeDecision.InRange)
        {
            return null;
        }

        return madeDecision;
    }
}