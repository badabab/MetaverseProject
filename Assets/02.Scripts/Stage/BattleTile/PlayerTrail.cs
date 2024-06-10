using UnityEngine;
using Photon.Pun;

public class PlayerTrail : MonoBehaviour
{
    private Transform player;
    private BattleTilePlayer playerScript;
    private TrailRenderer trailRenderer;

    void Start()
    {
        foreach (var p in FindObjectsOfType<BattleTilePlayer>())
        {
            if (p.GetComponent<PhotonView>().IsMine)
            {
                player = p.transform;
                playerScript = p;
                break;
            }
        }

        if (player == null || playerScript == null)
        {
            Debug.LogError("Local player not found.");
            return;
        }

        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer == null)
        {
            Debug.LogError("Trail Renderer component not found.");
            return;
        }

        // 트레일 렌더러 색상설정
        //trailRenderer.startColor = playerScript.playerColor;
        //trailRenderer.endColor = playerScript.playerColor;
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position;
        }
    }
}
