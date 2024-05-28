using Photon.Pun;
using Photon.Realtime;
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
        Debug.Log("빌리지 방에 입장했습니다.");
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
            if (UI_Lobby.SelectedType == PlayerType.Female)
            {
                PhotonNetwork.Instantiate($"Player {PlayerSelection.Instance.SelectedCharacterIndex}", spawnPoint, Quaternion.identity);
                string nickname = PlayerPrefs.GetString("LoggedInId");
                Debug.Log($"{nickname}");
                PlayerCanvasAbility.Instance.SetNickname(nickname);
            }
            if (UI_Lobby.SelectedType == PlayerType.Male)
            {
                PhotonNetwork.Instantiate($"Player {PlayerSelection.Instance.SelectedCharacterIndex}", spawnPoint, Quaternion.identity);
                string nickname = PlayerPrefs.GetString("LoggedInId");
                Debug.Log($"{nickname}");
                PlayerCanvasAbility.Instance.SetNickname(nickname);
            }
        }
        else
        {
            PhotonNetwork.Instantiate($"Player {characterIndex}", spawnPoint, Quaternion.identity);
            string nickname = PlayerPrefs.GetString("LoggedInId");
            PlayerCanvasAbility.Instance.NicknameTextUI.text = nickname;
            Debug.Log($"{nickname}");
        }
    }
    public Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, SpawnPoints.Count);
        return SpawnPoints[randomIndex].position;
    }
}
