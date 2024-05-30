using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerCoinCount : MonoBehaviour
{
    public TMP_Text PlayerCoinCount;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;

        gameManager.OnCoinsAdded += UpdateCoinCount;
    }

    // GameManager의 AddCoinsToWinner 이벤트에 연결된 UI 업데이트 함수
    private void UpdateCoinCount(string playerId, int newCoinCount)
    {
        // PlayerCoinCount에 새로운 코인 수를 표시
        PlayerCoinCount.text = newCoinCount.ToString();
    }
}
