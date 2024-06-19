using Photon.Pun;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class PlayerSelection : MonoBehaviour
{
    public static PlayerSelection Instance;
    public PlayerType SelectedType;
    public int SelectedCharacterIndex;
    private GameObject currentCharacter;

    public GameObject[] SelectedCharacter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        for (int i = 0; i < SelectedCharacter.Length; i++)
        {
            SelectedCharacter[i].gameObject.SetActive(false);
        }
    }

    public void CharacterSelection(PlayerType type)
    {
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }
        int index;
        if (type == PlayerType.Male)
        {
            index = Random.Range(13, 26);
        }
        else
        {
            index = Random.Range(1, 13);
        }
        currentCharacter = SelectedCharacter[index - 1];
        SelectedCharacterIndex = index;

        Debug.Log($"{SelectedCharacterIndex}");
        PersonalManager.Instance.UpdateCharacterIndex(SelectedCharacterIndex);

        Hashtable characterindex = new Hashtable { { "CharacterIndex", SelectedCharacterIndex } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(characterindex);

        SelectedType = type;
        currentCharacter.gameObject.SetActive(true);
    }
    public void ReloadCharacter()
    {
        int characterIndex = PersonalManager.Instance.CheckCharacterIndex();
        Hashtable characterindex = new Hashtable { { "CharacterIndex", characterIndex } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(characterindex);
        if (characterIndex != -1)
        {
            if (currentCharacter != null)
            {
                Destroy(currentCharacter);
            }

            currentCharacter = SelectedCharacter[characterIndex - 1];
            currentCharacter.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("캐릭터 인덱스를 가져오지 못했습니다.");
            return;
        }
    }
}
