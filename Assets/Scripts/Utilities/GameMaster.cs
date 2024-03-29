using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameMaster : MonoBehaviour
{
    //Singleton
    public static GameMaster gameMaster;

    public GameObject karasuPlayerPrefab;
    public Transform respawnLocation;

    public FixedJoint2D fixedJoint2D;

    //Enemy identification needed for destroying spawn locations object
    public static int enemyID = 0;

    int objectCounterInArrayToDestroy = 0;

    private void Start()
    {
        if (gameMaster == null)
        {
            gameMaster = this;
        }
    }

    public static void KillPlayer(KarasuEntity karasuEntity)
    {
        if (gameMaster.respawnLocation != null)
        {
            karasuEntity.transform.position = gameMaster.respawnLocation.position;
        }
        karasuEntity.GetComponent<FixedJoint2D>().enabled = false;
        //else
        //{
            
        //    GameObject[] checkPointArray = GameObject.FindGameObjectsWithTag("Respawn");
        //    gameMaster.closestCheckpoint = checkPointArray[0].transform;
        //    for (int i = 0; i < checkPointArray.Length; i++)
        //    {
        //        if (checkPointArray[i].transform.position.x - karasuEntity.transform.position.x < gameMaster.closestCheckpoint.position.x - karasuEntity.transform.position.x)
        //        {
        //            gameMaster.closestCheckpoint = checkPointArray[i].transform;
        //        }
        //    }
        //    karasuEntity.transform.position = gameMaster.closestCheckpoint.position;
        //}
        //gameMaster.fixedJoint2D.enabled = false;
    }

    public static void DestroyGameObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public static void DestroyGameObjects(GameObject[] gameObject)
    {
        gameMaster.StartCoroutine(gameMaster.DestroyGameObjectsWithDelay(gameObject));
    }

    IEnumerator DestroyGameObjectsWithDelay(GameObject[] gameObject)
    {
        yield return new WaitForSeconds(0.1f);
        DestroyGameObject(gameObject[objectCounterInArrayToDestroy]);
        objectCounterInArrayToDestroy++;
        if (objectCounterInArrayToDestroy < gameObject.Length)
        {
            StartCoroutine(DestroyGameObjectsWithDelay(gameObject));
        }
        else
        {
            objectCounterInArrayToDestroy = 0;
        }
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
