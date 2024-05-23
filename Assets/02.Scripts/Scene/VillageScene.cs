using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class VillageScene : MonoBehaviourPunCallbacks
{
    public static VillageScene Instance { get; private set; }
    public List<Transform> SpawnPoints;

    private bool _init = false;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (!_init)
        {
            Init();
        }
    }
    public override void OnJoinedRoom()
    {
        if (!_init)
        {
            Init();
        }
    }

    private void Init()
    {
        _init = true;
        PhotonNetwork.Instantiate("Player", GetRandomSpawnPoint(), Quaternion.identity);
    }

    public Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, SpawnPoints.Count);
        return SpawnPoints[randomIndex].position;
    }
}
