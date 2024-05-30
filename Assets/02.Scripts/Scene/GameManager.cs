using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action<string, int> OnCoinsAdded;// 임시코드(이윤석)

    private Dictionary<string, Personal> playerData = new Dictionary<string, Personal>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 플레이어 데이터를 초기화하거나 불러오기
    public void InitializePlayerData(string playerId, string playerName)
    {
        if (!playerData.ContainsKey(playerId))
        {
            playerData[playerId] = new Personal
            {
                Name = playerName,
                Coins = 0, // 기본 코인
                CharacterIndex = 0, // 기본 캐릭터
            };
        }
    }
    // 승리 시 코인 추가
    public void AddCoinsToWinner(string winnerId, int coinsToAdd)
    {
        if (playerData.ContainsKey(winnerId))
        {
            playerData[winnerId].Coins += coinsToAdd;
            Debug.Log($"Added {coinsToAdd} coins to {playerData[winnerId].Name}. Total Coins: {playerData[winnerId].Coins}");

            OnCoinsAdded?.Invoke(winnerId, playerData[winnerId].Coins);// 임시코드(이윤석)
        }
        else
        {
            Debug.LogError("Winner ID not found in player data.");
        }
    }
}
