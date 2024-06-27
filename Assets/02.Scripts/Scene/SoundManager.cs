using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] BgmClips;
    public float BgmVolume = 1;
    AudioSource BgmPlayer;

    public AudioClip[] SfxClips;
    public float SfxVolume;
    public int Channels;
    AudioSource[] SfxPlayer;
    int channelIndex;

    public enum Bgm
    {
        // 로비
        LobbyScene,                // 0
        // 마을 튜토리얼
        VillageSceneTutorials,     // 1
        // 마을
        VillageScene,              // 2
        // 폴가이즈
        FallGuysScene,             // 3
        // 배틀타일
        BattleTileScene,           // 4
        // 타워클라임
        TowerClimbScene,           // 5
        // 마을 내 공포사운드
        HorrorGameClosed,//6
        // 승리 
        Win,//7
        // 씬이동
        SceneMove,//8

    }
    public enum Sfx
    {
        // 0. 로비 버튼음
        UI_LobbyButtonQTutorialsButton,      // 0
        // 1. 마을 인터렉티브 오브젝트 사운드
        VillageInteractiveObjectRocket,      // 1
        VillageInteractiveObjectShip,        // 2
        VillageInteractiveObjectCannon,      // 3
        VillageInteractiveObjectWarningChicken1, // 4
        VillageInteractiveObjectWarningChicken2, // 5
        VillageInteractiveObjectWarningChicken3, // 6
        VillageInteractiveObjectWarningChicken4, // 7
        VillageInteractiveObjectWarningChicken5, // 8
        VillageInteractiveObjectWarningChicken6, // 9
        VillageInteractiveObjectWarningChicken7, // 10
        VillageInteractiveObjectJump,        // 11
        VillageInteractiveObjectBall,        // 12
        VillageInteractiveObjectPowerJump,        // 13
        // 2. 마을 상점/ 빌보드 판
        VillageCharacterChangeBillboard,     // 14
        // 3. 마을 포탈
        VillagePortal,                       // 15
        // 4. 1, 2번 UI / M지도 UI / 상점 코인 사운드
        UI_Village12MButton,                 // 16
        UI_VillageCharacterCoinSound,        // 17
        // 5. 모든 게임 / 게임오버UI / 승리UI / 패배UI / 카운트다운
        UI_WinVictory,                       // 18
        UI_Lose,                             // 19
        UI_GameOver,                         // 20
        UI_Count,                            // 21
        // 6. 스테이지 이동UI / 코인 획득
        UI_FallGuysStageMove,                // 22
        UI_FallGuysCoinSound,                // 23
        // 7. R버튼음
        UI_RButton,                          // 24
        // 8. 플레이어 사운드
        PlayerWalking,                       // 25
        PlayerRun,                           // 26
        PlayerJump,                          // 27
        PlayerDamages,                       // 28
        PlayerRunningJump,                   // 29
        PlayerPunch,                         // 30
        PlayerFlyingKick,                    // 31
        //승리
        Win,                                //32
        // 이동씬
        SceneMove,                          // 33
        // 준비, 출발

    }

    public static SoundManager instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        Init();
    }
    private void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        BgmPlayer = bgmObject.AddComponent<AudioSource>();
        BgmPlayer.playOnAwake = false;
        BgmPlayer.loop = true;
        BgmPlayer.volume = BgmVolume;

        GameObject SfxObject = new GameObject("SfxPlayer");
        SfxObject.transform.parent = transform;
        SfxPlayer = new AudioSource[Channels];

        for (int i = 0; i < SfxPlayer.Length; i++)
        {
            SfxPlayer[i] = SfxObject.AddComponent<AudioSource>();
            SfxPlayer[i].playOnAwake = false;
            SfxPlayer[i].volume = SfxVolume;
        }
    }
    public void PlayBgm(Bgm bgm)
    {
        BgmPlayer.clip = BgmClips[(int)bgm];
        BgmPlayer.Play();
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < SfxPlayer.Length; i++)
        {
            int loopIndex = (i + channelIndex) % SfxPlayer.Length;

            if (SfxPlayer[loopIndex].isPlaying)
            {
                continue;
            }
            channelIndex = loopIndex;
            SfxPlayer[loopIndex].clip = SfxClips[(int)sfx];
            SfxPlayer[loopIndex].Play();
            break;
        }
    }
    public void StopBgm()
    {
        if (BgmPlayer.isPlaying)
        {
            BgmPlayer.Stop();
        }
    }
    public void StopSfx(Sfx sfx)
    {
        for (int i = 0; i < SfxPlayer.Length; i++)
        {
            if (SfxPlayer[i].clip == SfxClips[(int)sfx])
            {
                SfxPlayer[i].Stop();
            }
        }
    }
}
