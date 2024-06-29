using UnityEngine;

public class Particles : MonoBehaviour
{
    public GameObject Particle1;
    public GameObject Particle2;
    public GameObject Particle3;
    public GameObject Particle4;

    private void Start()
    {
        // Particle GameObject들을 초기화하고 비활성화 상태로 설정
        Particle1.SetActive(false);
        Particle2.SetActive(false);
        Particle3.SetActive(false);
        Particle4.SetActive(false);
    }

    // 각 파티클 위치 설정 메서드
    public void SetParticlePosition(int particleNum, Vector3 position)
    {
        GameObject particle = GetParticleObject(particleNum);
        if (particle != null)
        {
            particle.transform.position = position;
            particle.SetActive(true);
        }
    }

    // 플레이어 번호에 따라 해당하는 파티클 GameObject 반환
    private GameObject GetParticleObject(int particleNum)
    {
        switch (particleNum)
        {
            case 1:
                return Particle1;
            case 2:
                return Particle2;
            case 3:
                return Particle3;
            case 4:
                return Particle4;
            default:
                Debug.LogError("Invalid particle number: " + particleNum);
                return null;
        }
    }
}
