using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviourPunCallbacks
{
    public List<Transform> SpawnPoints;
    private bool _init = false;

    private void Start()
    {
        if (!_init)
        {
            Init();
        }
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("방에 입장했습니다.");
        if (!_init)
        {
            Init();
        }
    }

    private void Init()
    {
        _init = true;
        Vector3 spawnPoint = GetRandomSpawnPoint();
        //Debug.Log($"스폰 위치: {spawnPoint}");

        PhotonNetwork.Instantiate($"Player {PlayerSelection.Instance.SelectedCharacterIndex}", spawnPoint, Quaternion.identity);
    }
    public Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, SpawnPoints.Count);
        return SpawnPoints[randomIndex].position;
    }
}
