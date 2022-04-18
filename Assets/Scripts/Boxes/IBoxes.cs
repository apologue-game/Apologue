using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoxes
{
    public float boxMass { get; set; }
    public float pushForce { get; set; }
    //lightOrHeavy = true -> light attack
    //lightOrHeavy = false -> heavy attack
    public void MoveOrDestroy(bool lightOrHeavy);

    enum BoxType
    {
        wooden,
        reinforcedWooden,
        metal
    }

    BoxType boxType { get; set; }
} 
