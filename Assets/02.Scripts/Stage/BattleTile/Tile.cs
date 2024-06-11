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
        _renderer.material = Materials[playerNum];
        StartCoroutine(AnimateTile());
    }

    private IEnumerator AnimateTile()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 1.2f;
        Vector3 originalPosition = transform.localPosition;
        Vector3 targetPosition = originalPosition + new Vector3(0, 0.5f, 0);

        float duration = 0.2f;
        float elapsedTime = 0f;

        // 크기와 위치 확대
        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        transform.localPosition = targetPosition;

        elapsedTime = 0f;

        // 크기와 위치 원래대로
        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / duration);
            transform.localPosition = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        transform.localPosition = originalPosition;
    }
}
