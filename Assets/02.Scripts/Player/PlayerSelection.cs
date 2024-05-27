using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    public static PlayerSelection Instance;
    public PlayerType SelectedType;
    public int SelectedCharacterIndex;
    private GameObject currentCharacter;
    public Personal personal;
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
            PersonalManager.Instance.UpdateCharacterIndex(personal.Name, SelectedCharacterIndex);
        }
        else
        {
            int indexFemale = Random.Range(1, 13);
            currentCharacter = Instantiate(Resources.Load<GameObject>($"Player {indexFemale}"), Vector3.zero, Quaternion.identity);
            SelectedCharacterIndex = indexFemale;
            PersonalManager.Instance.UpdateCharacterIndex(personal.Name, SelectedCharacterIndex);
        }
        SelectedType = type;
    }
}
