using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager_MultiPlayerPrefab : NetworkManager
{
    public GameObject playerPrefab2;
    public List<GameObject> Players = new List<GameObject>();
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if (conn.connectionId == NetworkServer.serverHostId) /*PLAYER CHOOSES CLASS A*/
        {
            //SPAWN THAT PREFAB
            Debug.Log("IS HOST!");
            var player = (GameObject)GameObject.Instantiate(playerPrefab);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
            Players.Add(player);
        }
        else  /*PLAYER CHOOSES CLASS B*/
        {
            //SPAWN THAT PREFAB
            var player = (GameObject)GameObject.Instantiate(playerPrefab2);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
            Players.Add(player);
        }
    }
    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);
        if(Players.Contains(player.gameObject))
        {
            Players.Remove(player.gameObject);
        }
    }
}
