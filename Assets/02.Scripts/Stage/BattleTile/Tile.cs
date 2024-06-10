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
        _renderer.material = new Material(Materials[0]); // 원본 Material을 변경하지 않기 위해 새로운 인스턴스를 생성
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            _player = other.GetComponent<BattleTilePlayer>();
            if (_renderer.material.color != Materials[_player.MyNum].color)
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
        StartCoroutine(ChangeColorCoroutine(Materials[playerNum].color));
    }

    private IEnumerator ChangeColorCoroutine(Color targetColor)
    {
        Color startColor = Color.white;
        Color currentColor = _renderer.material.color;
        float duration = 1.0f; // 전환 시간 (초)
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentColor = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            _renderer.material.color = currentColor;
            yield return null;
        }

        _renderer.material.color = targetColor; // 최종 색상을 설정
    }
}
