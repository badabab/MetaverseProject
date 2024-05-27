using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Loading : MonoBehaviour
{
    public GameObject LoadingImage;
    public float minY = 66f; // 최소 Y 위치
    public float maxY = 72f; // 최대 Y 위치
    public float speed = 1f; // 움직임 속도
    public float LoadingTime = 3f;// 로딩시간 
    public GameObject CharacterSpin;
    public float rotationSpeed = 100f; // 회전 속도

    void Update()
    {
        LoadingTime -= Time.deltaTime;
        // PingPong 함수 사용하여 부드러운 Y 위치 변경
        float newY = Mathf.PingPong(Time.time * speed, maxY - minY) + minY;

        // LoadingImage의 위치를 새로운 Y 값으로 설정
        Vector3 newPosition = new Vector3(LoadingImage.transform.position.x, newY, LoadingImage.transform.position.z);
        LoadingImage.transform.position = newPosition;

        CharacterSpin.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        if (LoadingTime <= 0)
        {
            SceneManager.LoadScene("VillageScene");
        }

    }
}
