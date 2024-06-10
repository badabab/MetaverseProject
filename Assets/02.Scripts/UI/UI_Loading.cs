using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Loading : MonoBehaviour
{
    public GameObject LoadingImage;
    public float minY = 66f; // 최소 Y 위치
    public float maxY = 72f; // 최대 Y 위치
    public float speed = 1f; // 움직임 속도
    public float LoadingTime = 3f;// 로딩시간 

    public float rotationSpeed = 100f; // 회전 속도

    private GameObject _character;
    public GameObject[] SelectedCharacter;

    private void Start()
    {
        for (int i = 0; i < SelectedCharacter.Length; i++)
        {
            SelectedCharacter[i].gameObject.SetActive(false);
        }
        ShowCharacter();
    }
    void Update()
    {
        LoadingTime -= Time.deltaTime;


        _character.transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);

        if (LoadingTime <= 0)
        {
            
        }
    }

    void ShowCharacter()
    {

            // PingPong 함수 사용하여 부드러운 Y 위치 변경
            float newY = Mathf.PingPong(Time.time * speed, maxY - minY) + minY;
            // LoadingImage의 위치를 새로운 Y 값으로 설정
            Vector3 newPosition = new Vector3(LoadingImage.transform.position.x, newY, LoadingImage.transform.position.z);
            LoadingImage.transform.position = newPosition;
            // _character = Instantiate(SelectedCharacter[PlayerSelection.Instance.SelectedCharacterIndex - 1], newPosition, Quaternion.identity);

            int characterIndex = PersonalManager.Instance.CheckCharacterIndex();
            if (characterIndex != 0)
            {
                _character = SelectedCharacter[characterIndex - 1];
            }
            else
            {
                _character = SelectedCharacter[PlayerSelection.Instance.SelectedCharacterIndex - 1].gameObject;
            }

            _character.gameObject.SetActive(true);
        
    }
}
