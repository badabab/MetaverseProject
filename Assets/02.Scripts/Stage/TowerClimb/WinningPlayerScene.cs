using Photon.Pun;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class WinningPlayerScene : MonoBehaviour
{
    private string _firstName;

    public Vector3 PlayerSpawn;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("FirstPlayerName"))
        {
            _firstName = (string)PhotonNetwork.CurrentRoom.CustomProperties["FirstPlayerName"];
            int characterIndex = PersonalManager.Instance.CheckCharacterIndex();

            PhotonNetwork.Instantiate(_firstName, PlayerSpawn, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AllPlayerCameraOff()
    {
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList.ToArray();
        Debug.Log("Player count: " + players.Length);
        foreach (Photon.Realtime.Player player in players)
        {

        }
    }
}
