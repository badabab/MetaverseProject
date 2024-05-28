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
            currentCharacter = Instantiate(Resources.Load<GameObject>($"Player {indexMale}"), Vector3.zero, Quaternion.identity);
            SelectedCharacterIndex = indexMale;
            Debug.Log($"{SelectedCharacterIndex}");
            PersonalManager.Instance.UpdateCharacterIndex(SelectedCharacterIndex);
        }
        else
        {
            int indexFemale = Random.Range(1, 13);
            currentCharacter = Instantiate(Resources.Load<GameObject>($"Player {indexFemale}"), Vector3.zero, Quaternion.identity);
            SelectedCharacterIndex = indexFemale;
            Debug.Log($"{SelectedCharacterIndex}");
            PersonalManager.Instance.UpdateCharacterIndex(SelectedCharacterIndex);
        }
        SelectedType = type;
    }
}
