using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerCoinCount : MonoBehaviour
{
    public TMP_Text PlayerCoinCount;

    private void Start()
    {
        UpdateCoinCount();
    }

    private void Update()
    {
        UpdateCoinCount();
    }

    private void UpdateCoinCount()
    {
        int coin = PersonalManager.Instance.CheckCoins();
        PlayerCoinCount.text = coin.ToString();
        /*if (!string.IsNullOrEmpty(playerId))
        {
            int coinCount = PlayerPrefs.GetInt($"{playerId}_Coins", 0);
            PlayerCoinCount.text = coinCount.ToString();
        }*/
    }
}
