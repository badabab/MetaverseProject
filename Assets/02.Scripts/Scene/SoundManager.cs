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
        LobbyScene,//0
        LoadingScene,//1
        //마을
        VillageSceneTutorials, //2
        VillageScene, //3
        VillageLoadScene,//4
        //폴가이즈
        FallGuysScene,//5
        FallGuysWinScene,//6
        FallGuysDescriptionScene,//7
        //배틀타일
        BattleTileScene,//8
        BattleTileWinScene,//9
        BattleTileDescriptionScene,//10
        //타워클라임
        TowerClimbScene,//11
        TowerClimbWinScene,//12
        TowerClimbDescriptionScene,//13
        // 마을 테마용 사운드
        VillageFallGuys,//14
        VillageBattleTile,//15
        VillageTowerClimb,//16
        VillageHorrorGameClosed,//17
        VillageWarnigChicken,//18
    }
    public enum Sfx
    {
        // 0. 로비 버튼음 / 로비 타자음
        UI_LobbyButton,            // 0
        UI_LobbyWordButton,        // 1
        // 1. 마을 튜토리얼 Q 사운드
        UI_VillageTutorials,       // 2
        // 2. 마을 인터렉티브 오브젝트 사운드
        VillageInteractiveObjectRocket,      // 3
        VillageInteractiveObjectShip,        // 4
        VillageInteractiveObjectCannon,      // 5
        VillageInteractiveObjectWarnigChicken,// 6
        VillageInteractiveObjectJump,        // 7
        VillageInteractiveObjectFire,        // 8
        VillageInteractiveObjectExplosionFire,// 9
        // 3. 마을 상점/ 빌보드 판
        VillageCharacterChange,    // 10
        VillageBillboard,          // 11
        // 4. 마을 포탈 
        VillagePortal,             // 12
        // 5. 마을 상점 / 빌보드 판 UI / 1, 2번 UI / M지도 UI
        UI_VillageInformationButton,// 13
        UI_VillageMapButton,       // 14
        UI_Village1Button,         // 15
        UI_Village2Button,         // 16
        UI_VillageMButton,         // 17
        // 6. 타워클라임 포트키 /  타자음
        UI_TowerClimbPortKey,      // 18
        UI_TowerClimbPortKeyWrod,  // 19
        // 7. 타워클라임 게임오버UI / 승리UI
        UI_TowerClimbGameOver,     // 20
        UI_TowerClimbVictory,      // 21
        // 8. 폴가이즈 R버튼음
        UI_FallGuysRButton,        // 22
        // 9. 폴가이즈 카운트다운UI / 스테이지 이동UI 
        UI_FallGuysCount,          // 23
        UI_FallGuysStageMove,      // 24
        // 10. 폴가이즈 게임오버UI / 승리UI / 패배UI
        UI_FallGuysGameOver,       // 25
        UI_FallGuysWin,            // 26
        UI_FallGuysLose,           // 27
        // 11. 배틀타일 R버튼음
        UI_BattleTileRButton,      // 28
        // 12. 배틀타일 카운트다운UI
        UI_BattleTileCount,        // 29
        UI_BattleTileCount54321,   // 30
        // 13. 배틀타일 게임오버UI / 승리UI / 패배UI
        UI_BattleTileGameOver,     // 31
        UI_BattleTileWin,          // 32
        UI_BattleTileLose,         // 33
        // 14. 게임설명 UI타자음
        UI_GameexPlanationWord,    // 34
        // 15. 플레이어 사운드
        PlayerWalking,             // 35
        PlayerRun,                 // 36
        PlayerJump,                // 37
        PLayerDamages,             // 38
        PlayerNormalJump,          // 39
        PlayerRunningJump,         // 40
        PlayerPunch,               // 41
        PlayerFlyingKick,          // 42
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
