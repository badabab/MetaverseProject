using MongoDB.Driver;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

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
            DontDestroyOnLoad(gameObject);
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
        if (type == PlayerType.Male)
        {
            int indexMale = Random.Range(13, 26);
            currentCharacter = Instantiate(SelectedCharacter[indexMale - 1], Vector3.zero, Quaternion.identity);
            SelectedCharacterIndex = indexMale;
            Debug.Log($"{SelectedCharacterIndex}");
            PersonalManager.Instance.UpdateCharacterIndex(SelectedCharacterIndex);
        }
        else
        {
            int indexFemale = Random.Range(1, 13);
            currentCharacter = Instantiate(SelectedCharacter[indexFemale - 1], Vector3.zero, Quaternion.identity);
            SelectedCharacterIndex = indexFemale;
            Debug.Log($"{SelectedCharacterIndex}");
            PersonalManager.Instance.UpdateCharacterIndex(SelectedCharacterIndex);
        }
        SelectedType = type;
        currentCharacter.gameObject.SetActive(true);
    }
    public void ReloadCharacter()
    {
        int characterIndex = PersonalManager.Instance.CheckCharacterIndex();

        if (characterIndex != -1)
        {
            if (currentCharacter != null)
            {
                Destroy(currentCharacter);
            }

            currentCharacter = Instantiate(SelectedCharacter[characterIndex - 1], Vector3.zero, Quaternion.identity);
            currentCharacter.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("캐릭터 인덱스를 가져오지 못했습니다.");
            return;
        }
    }
}
