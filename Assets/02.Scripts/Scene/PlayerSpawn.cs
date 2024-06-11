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
    private void Init()
    {
        _init = true;
        Vector3 spawnPoint = GetRandomSpawnPoint();
        Debug.Log($"스폰 위치: {spawnPoint}");
        int characterIndex = PersonalManager.Instance.CheckCharacterIndex();
        string characterPrefab = characterIndex <= 0 ? $"Player {PlayerSelection.Instance.SelectedCharacterIndex}" : $"Player {characterIndex}";

        GameObject playerObject = PhotonNetwork.Instantiate(characterPrefab, spawnPoint, Quaternion.identity);
        Debug.Log(playerObject.name);
    }

    public Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, SpawnPoints.Count);
        return SpawnPoints[randomIndex].position;
    }

}
