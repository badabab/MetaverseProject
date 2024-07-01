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

        int index = GetRandomCharacterIndex(type);
        if (index < 1 || index > SelectedCharacter.Length)
        {
            Debug.LogError($"Invalid character index: {index}");
            return;
        }

        SelectedCharacterIndex = index;
        currentCharacter = Instantiate(SelectedCharacter[index - 1]);
        currentCharacter.SetActive(true);

        Debug.Log($"SelectedCharacterIndex: {SelectedCharacterIndex}");
        PersonalManager.Instance.UpdateCharacterIndex(SelectedCharacterIndex);

        Hashtable characterindex = new Hashtable { { "CharacterIndex", SelectedCharacterIndex } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(characterindex);

        SelectedType = type;
    }
    private int GetRandomCharacterIndex(PlayerType type)
    {
        if (type == PlayerType.Male)
        {
            return Random.Range(13, 26);
        }
        else
        {
            return Random.Range(1, 13);
        }
    }
    public void ReloadCharacter()
    {
        int characterIndex = PersonalManager.Instance.CheckCharacterIndex();
        Debug.Log($"{characterIndex}");
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
