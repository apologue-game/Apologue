using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    //Specific identifications for throwables
    //Sickleman: 1

    public int specificID;
    public bool inUse = false;
    public bool hasDamaged = false;
    public bool stop = false;

    private void Start()
    {
        Throwables.AddToObjectPool(this);
    }
}
