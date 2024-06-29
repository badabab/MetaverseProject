using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarnigChickenFire : MonoBehaviour
{
    private PhotonView photonView;
    public TMP_Text Warnig;
    public List<GameObject> MissileFire; // 미사일 발사 위치들
    public List<GameObject> Missiles; // 미사일 오브젝트 리스트
    private float MissilesAngle = 180f;
    private int collisionCount = 0;
    private bool isConnected = false; // PhotonNetwork 연결 상태 확인용

    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        // PhotonNetwork가 연결되어 있는지 확인
        if (PhotonNetwork.IsConnected)
        {
            isConnected = true;
        }
        else
        {
            // 연결되어 있지 않으면, 연결을 시도하거나 오프라인 모드로 전환
            PhotonNetwork.ConnectUsingSettings(); // 또는 PhotonNetwork.OfflineMode = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collisionCount++;
            HandleCollision();
        }
    }

    private void HandleCollision()
    {
        switch (collisionCount)
        {
            case 1:
                if (isConnected)
                photonView.RPC("SyncWarnigText", RpcTarget.AllBuffered, "I warned you");
                SoundManager.instance.PlaySfx(SoundManager.Sfx.VillageInteractiveObjectWarningChicken1);
                Warnig.text = "I warned you"; // 연결되어 있지 않은 경우 로컬에서만 설정
                break;
            case 2:
                if (isConnected)
                photonView.RPC("SyncWarnigText", RpcTarget.AllBuffered, "You");
                SoundManager.instance.PlaySfx(SoundManager.Sfx.VillageInteractiveObjectWarningChicken2);
                Warnig.text = "You";
                break;
            case 3:
                if (isConnected)
                photonView.RPC("SyncWarnigText", RpcTarget.AllBuffered, "Regret");
                SoundManager.instance.PlaySfx(SoundManager.Sfx.VillageInteractiveObjectWarningChicken3);
                Warnig.text = "Regret";
                break;
            case 4:
                if (isConnected)
                photonView.RPC("SyncWarnigText", RpcTarget.AllBuffered, "3");
                SoundManager.instance.PlaySfx(SoundManager.Sfx.VillageInteractiveObjectWarningChicken4);
                Warnig.text = "3";
                break;
            case 5:
                if (isConnected)
                photonView.RPC("SyncWarnigText", RpcTarget.AllBuffered, "2");
                SoundManager.instance.PlaySfx(SoundManager.Sfx.VillageInteractiveObjectWarningChicken5);
                Warnig.text = "2";
                break;
            case 6:
                if (isConnected)
                photonView.RPC("SyncWarnigText", RpcTarget.AllBuffered, "1");
                SoundManager.instance.PlaySfx(SoundManager.Sfx.VillageInteractiveObjectWarningChicken6);
                Warnig.text = "1";
                break;
            case 7:
                StartCoroutine(LaunchMissilesRandomly());
                break;
            default:
                ResetWarnings();
                break;
        }
    }

    [PunRPC]
    private void SyncWarnigText(string text)
    {
        Warnig.text = text;
    }

    private IEnumerator LaunchMissilesRandomly()
    {
        SoundManager.instance.PlaySfx(SoundManager.Sfx.VillageInteractiveObjectWarningChicken7);
        if (isConnected)
            photonView.RPC("SyncWarnigText", RpcTarget.AllBuffered, "");

        float randomDelay = Random.Range(0f, 5f);
        yield return new WaitForSeconds(randomDelay);

        foreach (GameObject firePosition in MissileFire)
        {
            int randomMissileIndex = Random.Range(0, Missiles.Count);
            Instantiate(Missiles[randomMissileIndex], firePosition.transform.position, Quaternion.Euler(0, 0, MissilesAngle));
        }

        collisionCount = 0;
        ResetWarnings();
    }

    private void ResetWarnings()
    {
        if (isConnected)
            photonView.RPC("SyncWarnigText", RpcTarget.AllBuffered, "");
        else
            Warnig.text = "";
    }
}