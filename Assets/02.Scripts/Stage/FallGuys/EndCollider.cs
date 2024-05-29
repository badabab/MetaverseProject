using UnityEngine;

public class EndCollider : MonoBehaviour
{
    public Transform Start2, Start3;
    private CharacterController _characterController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _characterController = other.GetComponent<CharacterController>();
            if (gameObject.name == "End1")
            {
                Debug.Log("End1 도착");
                _characterController.enabled = false;
                other.transform.position = Start2.position;
                _characterController.enabled = true;
            }
            else if (gameObject.name == "End2")
            {
                Debug.Log("End2 도착");
                _characterController.enabled = false;
                other.transform.position = Start3.position;
                _characterController.enabled = true;
            }
            else if (gameObject.name == "End3")
            {
                Debug.Log("게임 끝");
            }
        }
    }
}
