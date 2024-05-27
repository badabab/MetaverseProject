using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    public static PlayerSelection Instance;
    public PlayerType SelectedType;
    public int SelectedCharacterIndex;

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
        if (type == PlayerType.Male)
        {
            int indexMale = Random.Range(13, 26);
            Instantiate(Resources.Load<GameObject>($"Player {indexMale}"), Vector3.zero, Quaternion.identity);
            SelectedCharacterIndex = indexMale;
        }
        else
        {
            int indexFemale = Random.Range(1, 13);
            Instantiate(Resources.Load<GameObject>($"Player {indexFemale}"), Vector3.zero, Quaternion.identity);
            SelectedCharacterIndex = indexFemale;
        }
    }
}
