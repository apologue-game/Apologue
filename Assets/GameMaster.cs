using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    //singleton
    public static GameMaster gameMaster;

    public GameObject karasuPlayerPrefab;
    public Transform respawnLocation;

    private void Start()
    {
        if (gameMaster == null)
        {
            gameMaster = this;
        }
    }

    public void RespawnPlayer()
    {
        //Instantiate(karasuPlayerPrefab, respawnLocation.position, respawnLocation.rotation);
    }
    
    public static void KillPlayer(KarasuEntity karasuEntity)
    {
        //Destroy(karasuEntity.gameObject);
        karasuEntity.gameObject.transform.position = gameMaster.respawnLocation.position;
        //gameMaster.RespawnPlayer();
    }
}
