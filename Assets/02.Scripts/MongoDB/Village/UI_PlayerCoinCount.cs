using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerCoinCount : MonoBehaviour
{
    public TMP_Text PlayerCoinCount;

    private string playerId;

    private void Start()
    {
        playerId = PlayerPrefs.GetString("LoggedInId", "");

        UpdateCoinCount();
    }

    private void Update()
    {
        UpdateCoinCount();
    }

    private void UpdateCoinCount()
    {
        if (!string.IsNullOrEmpty(playerId))
        {
            int coinCount = PlayerPrefs.GetInt($"{playerId}_Coins", 0);
            PlayerCoinCount.text = coinCount.ToString();
        }
    }
}
