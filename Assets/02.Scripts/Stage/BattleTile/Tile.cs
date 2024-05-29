using UnityEngine;
using Photon.Pun;

public class Tile : MonoBehaviourPun
{
    public Material[] Materials;
    private Renderer _renderer;

    // 임시
    public int PlayerNum = 1;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material = Materials[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_renderer.material != Materials[PlayerNum])
            {
                // 위에 꺼는 테스트, 포톤으로 바꾸면 아래 코드
                _renderer.material = Materials[PlayerNum];
                //photonView.RPC("ChangeMaterial", RpcTarget.AllBuffered, PlayerNum);
            }
        }
    }

    [PunRPC]
    private void ChangeMaterial(int playerNum)
    {
        if (playerNum >= 0 && playerNum < Materials.Length)
        {
            _renderer.material = Materials[playerNum];
            //Debug.Log("Material이 변경되었습니다: " + Materials[playerNum].name);
        }
    }
}
