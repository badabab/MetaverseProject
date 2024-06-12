using UnityEngine;

public class Particles : MonoBehaviour
{
    public GameObject Particle1;
    public GameObject Particle2;
    public GameObject Particle3;
    public GameObject Particle4;

    private void Start()
    {
        Particle1.SetActive(false);
        Particle2.SetActive(false);
        Particle3.SetActive(false);
        Particle4.SetActive(false);
    }
    private void Update()
    {
        BattleTilePlayer[] players = FindObjectsOfType<BattleTilePlayer>();
        foreach (BattleTilePlayer player in players)
        {
            switch (player.MyNum)
            {
                case 1:
                    FollowPlayer(Particle1, player.transform);
                    break;
                case 2:
                    FollowPlayer(Particle2, player.transform);
                    break;
                case 3:
                    FollowPlayer(Particle3, player.transform);
                    break;
                case 4:
                    FollowPlayer(Particle4, player.transform);
                    break;
            }
        }
    }

    private void FollowPlayer(GameObject particle, Transform playerTransform)
    {
        if (particle != null)
        {
            particle.transform.position = playerTransform.position;
            particle.SetActive(true);
        }
    }
}