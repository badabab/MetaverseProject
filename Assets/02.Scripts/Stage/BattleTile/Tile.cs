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
        if (BattleTileManager.Instance.CurrentGameState != GameState.Go)
        {
            return;
        }

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
        StartCoroutine(WaitAndAnimateTile(0.3f)); // 0.3초 대기 후 애니메이션 시작
    }

    private IEnumerator WaitAndAnimateTile(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // 0.3초 대기
        StartCoroutine(AnimateTile());
    }

    private IEnumerator AnimateTile()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 1.2f;
        Vector3 originalPosition = transform.localPosition;
        Vector3 targetPosition = new Vector3(originalPosition.x, 0, originalPosition.z); // y = 0으로 설정

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
        Vector3 finalPosition = new Vector3(originalPosition.x, -1, originalPosition.z); // y = -1으로 설정
        Vector3 finalScale = new Vector3(1, 1, 1); // 최종 크기를 (1, 1, 1)로 설정

        // 크기와 위치 원래대로
        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(targetScale, finalScale, elapsedTime / duration);
            transform.localPosition = Vector3.Lerp(targetPosition, finalPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = finalScale;
        transform.localPosition = finalPosition;
    }
}
