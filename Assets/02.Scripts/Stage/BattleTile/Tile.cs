using UnityEngine;
using Photon.Pun;
using System.Collections;

public class Tile : MonoBehaviourPunCallbacks
{
    public Material[] Materials;
    private Renderer _renderer;

    private BattleTilePlayer _player;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material = Materials[0]; // 원본 Material을 사용
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            _player = other.GetComponent<BattleTilePlayer>();
            if (_renderer.material != Materials[_player.MyNum])
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("ChangeMaterial", RpcTarget.AllBuffered, _player.MyNum);
                }
                else
                {
                    photonView.RPC("RequestMaterialChange", RpcTarget.MasterClient, _player.MyNum);
                }
            }
        }
    }

    [PunRPC]
    private void RequestMaterialChange(int playerNum)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ChangeMaterial", RpcTarget.AllBuffered, playerNum);
        }
    }

    [PunRPC]
    private void ChangeMaterial(int playerNum)
    {
        StartCoroutine(ChangeMaterialCoroutine(Materials[playerNum]));
    }

    private IEnumerator ChangeMaterialCoroutine(Material targetMaterial)
    {
        Material startMaterial = _renderer.material;
        float duration = 1.0f; // 전환 시간 (초)
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _renderer.material.Lerp(startMaterial, targetMaterial, elapsedTime / duration);
            yield return null;
        }

        _renderer.material = targetMaterial; // 최종 Material을 설정
    }
}
