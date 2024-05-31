using UnityEngine;
using Photon.Pun;

public class Tile : MonoBehaviourPun
{
    public Material[] Materials;
    private Renderer _renderer;

    private BattleTilePlayer _player;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material = Materials[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.GetComponent<BattleTilePlayer>();
            if (_renderer.material != Materials[_player.MyNum])
            {
                //_renderer.material = Materials[_player.MyNum];      // 테스트용
                photonView.RPC("ChangeMaterial", RpcTarget.AllBuffered, _player.MyNum);   // 포톤
            }
        }
    }

    [PunRPC]
    private void ChangeMaterial(int playerNum)
    {
        _renderer.material = Materials[playerNum];
    }
}
