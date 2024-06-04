using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerOptionAbility : MonoBehaviourPun
{
    public void Pause()
    {
        if (photonView.IsMine)
        {
            Time.timeScale = 0f;
        }
    }

    public void Continue()
    {
        if (photonView.IsMine)
        {
            Time.timeScale = 1f;
        }
    }

    [PunRPC]
    public void TeleportToVillage()
    {
        if (photonView.IsMine)
        {
            SceneManager.LoadScene("VillageScene");
        }
    }
}
