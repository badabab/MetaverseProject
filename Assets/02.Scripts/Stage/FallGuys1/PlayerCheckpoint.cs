using Photon.Pun;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCheckpoint : MonoBehaviourPunCallbacks
{
    public Vector3 CurrentCheckpoint { get; private set; }
    public float FallPoint = -10;
    private string[] _allowedScenes = { "FallGuysScene1", "FallGuysScene2", "FallGuysScene3" };

    private void Start()
    {
        CurrentCheckpoint = new Vector3(0, 5.5f, 110);
    }
    private void Update()
    {
        if (_allowedScenes.Contains(SceneManager.GetActiveScene().name))
        {
            CheckFall();
        }
    }

    private void CheckFall()
    {
        if (transform.position.y < FallPoint)
        {
            transform.position = CurrentCheckpoint;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            Checkpoint checkpoint = other.GetComponent<Checkpoint>();
            if (checkpoint != null)
            {
                Vector3 newCheckpoint = checkpoint.transform.position;
                photonView.RPC("UpdateCheckpoint", RpcTarget.All, newCheckpoint);
            }
        }
    }

    [PunRPC]
    public void UpdateCheckpoint(Vector3 checkpoint)
    {
        CurrentCheckpoint = checkpoint;
    }
}
