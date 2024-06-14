using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarnigChickenFire : MonoBehaviour
{
    public TMP_Text Warnig;
    public List<GameObject> MissileFire; // 미사일 발사 위치들
    public List<GameObject> Missiles; // 미사일 오브젝트 리스트

    private int collisionCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView photonView = other.GetComponent<PhotonView>();

            if (photonView != null && photonView.IsMine)
            {
                collisionCount++;
                HandleCollision();
            }
        }
    }

    private void HandleCollision()
    {
        switch (collisionCount)
        {
            case 1:
                Warnig.text = "I warned you";
                break;
            case 2:
                Warnig.text = "You";
                break;
            case 3:
                Warnig.text = "Regret";
                break;
            case 4:
                Warnig.text = "3";
                break;
            case 5:
                Warnig.text = "2";
                break;
            case 6:
                Warnig.text = "1";
                break;
            case 7:
                LaunchMissiles();
                break;
            default:
                ResetWarnings();
                break;
        }
    }

    private void LaunchMissiles()
    {
        Warnig.text = "";

        for (int i = 0; i < 2; i++)
        {
            int randomMissileIndex = Random.Range(0, Missiles.Count);
            int randomFireIndex = Random.Range(0, MissileFire.Count);

            GameObject missile = Instantiate(Missiles[randomMissileIndex], MissileFire[randomFireIndex].transform.position, Quaternion.identity);
        }

        collisionCount = 0;
        ResetWarnings();
    }

    private void ResetWarnings()
    {
        Warnig.text = "";
    }
}
