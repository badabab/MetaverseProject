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
        Debug.Log("플레이어 초기화");
        _init = true;
        Vector3 spawnPoint = GetRandomSpawnPoint();
        Debug.Log($"스폰 위치: {spawnPoint}");

        int characterIndex = PersonalManager.Instance.CheckCharacterIndex();

        if (characterIndex <= 0)
        {
            characterIndex = PlayerSelection.Instance.SelectedCharacterIndex;
        }

        // Instantiate player
        PhotonNetwork.Instantiate($"Player {characterIndex}", spawnPoint, Quaternion.identity);

        string nickname = PlayerPrefs.GetString("LoggedInId");
        Debug.Log($"{nickname}");

        PlayerCanvasAbility.Instance.SetNickname(nickname);
        PlayerCanvasAbility.Instance.ShowMyNickname();
    }

    public Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, SpawnPoints.Count);
        return SpawnPoints[randomIndex].position;
    }
}
