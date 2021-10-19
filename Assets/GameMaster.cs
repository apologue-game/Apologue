using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    //singleton
    public static GameMaster gameMaster;

    public GameObject karasuPlayerPrefab;
    public Transform respawnLocation;

    //Enemy identification needed for destroying spawn locations object
    public static int enemyID = 0;

    private void Start()
    {
        if (gameMaster == null)
        {
            gameMaster = this;
        }
    }

    public static void KillPlayer(KarasuEntity karasuEntity)
    {
        karasuEntity.gameObject.transform.position = gameMaster.respawnLocation.position;
    }

    public static void DestroyGameObject(GameObject gameObject)
    {
        if (gameObject.name == "Soldier0_SpawnLocation")
        {
            Debug.Log("Destroyed spawn location");
        }
        Destroy(gameObject);
    }

    public static void InstantiateGameObject(GameObject gameObject)
    {
        Instantiate(gameObject);
    }

    public static class Utilities
    {
        public static bool IsFloatInRange(float min, float max, float valueToCheck)
        {
            if (valueToCheck >= min && valueToCheck <= max)
            {
                return true;
            }
            return false;
        }
    }

}
