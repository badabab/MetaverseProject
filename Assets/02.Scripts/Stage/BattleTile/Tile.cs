using UnityEngine;
using Photon.Pun;

public class Tile : MonoBehaviourPun
{
    public GameObject Black;
    public GameObject Red;
    public GameObject Yellow;
    public GameObject Blue;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeColor(Color.blue);
        }
    }

    public void ChangeColor(Color newColor)
    {
        photonView.RPC("RPC_ChangeColor", RpcTarget.AllBuffered, newColor.r, newColor.g, newColor.b, newColor.a);
    }

    [PunRPC]
    private void RPC_ChangeColor(float r, float g, float b, float a)
    {
        Color newColor = new Color(r, g, b, a);
        
    }
}
