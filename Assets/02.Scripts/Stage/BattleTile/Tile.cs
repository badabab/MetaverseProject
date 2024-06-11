using UnityEngine;
using Photon.Pun;

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
    }
}
