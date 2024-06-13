using Photon.Pun;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class WinningPlayerScene : MonoBehaviour
{
    private string _firstName;

    public Vector3 PlayerSpawn;

    public TextMeshProUGUI WinningName;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("FirstPlayerName"))
        {
            _firstName = (string)PhotonNetwork.CurrentRoom.CustomProperties["FirstPlayerName"];
            int characterIndex = PersonalManager.Instance.CheckCharacterIndex();
            string nickname = PersonalManager.Instance.UserNameMach();

            if (_firstName == nickname)
            {
                string firstCharacter = $"Player {characterIndex}";
                PhotonNetwork.Instantiate(firstCharacter, PlayerSpawn, Quaternion.identity);
                WinningName.text = _firstName;
            }
            else
            {
                return;
            }
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
