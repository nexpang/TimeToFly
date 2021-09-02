# TimeToFly
<img src = "http://ggm.gondr.net/image/users/37/portfolio/135_screenshot_06.png" width="150%" height="150%">

Smartin App Challenge Development Part Projects<br/>
STAC2021에 결선진출한 거안사위 디럭스의 후속작인 TIME TO FLY입니다

***

### 게임 영상
[능력 소개 영상](https://youtu.be/KK-rLM9FDqw)

***

### 플레이 방식
<img src = "https://media.discordapp.net/attachments/798813285037899786/882212733272096798/unknown.png" width="150%" height="150%">

#### 일시정지, 이동, 점프, 능력 버튼 및 타이머

***

### 소스코드 예시
player ability ex) Ability_FutureCreate.cs<br/>
플레이어 능력 예) 미래예지 파일명.cs

```
    void RecordPlayer()
    {
        if (isAbilityEnable)
        {
            if (!isRecording)
            {
                isRecording = true;
                isFuturePlay = false;
                RecordNumber_XY.Clear();
                RecordNumber_Sprite.Clear();
                RecordNumber_SpriteFlipX.Clear();
            }
        }
        else
        {
            isRecording = false;
        }

        if (isRecording)
        {
            recordTime = Time.time;
            recordTime = (float)Math.Round(recordTime * 100) / 100;
            //Debug.Log("recordTime : " + recordTime + ", recordDelay : " + recordDelay);
            if (recordTime >= recordDelay)
            {
                RecordNumber_XY.Add(new Vector2(playerRb.transform.position.x, playerRb.transform.position.y));
                RecordNumber_Sprite.Add(playerAn.GetComponent<SpriteRenderer>().sprite);
                RecordNumber_SpriteFlipX.Add(playerAn.GetComponent<SpriteRenderer>().flipX);

                recordDelay = recordTime + 0.03f;
                recordTime = 0f;
            }
        }
    }
    public void ResetPlayer()
    {
        isAbilityEnable = false;
        isFuturePlay = true;

        //능력 시간 초기화
        currentTime = abilityDefaultTime;

        // DEAD에서 바꾼다.
        GameManager.Instance.player.playerState = PlayerState.NORMAL;

        // 시계 UI 사라지게
        DOTween.To(() => clockUI.GetComponent<CanvasGroup>().alpha, value => clockUI.GetComponent<CanvasGroup>().alpha = value, 0f, 2f).OnComplete(() =>
        {
            clockUI.SetActive(false);
        });

        // 파란색 닭에서 다시 기본 닭으로!
        playerAn.GetComponent<SpriteRenderer>().color = Color.white;

        // 카메라 글리치 효과 초기화
        GlitchEffect.Instance.colorIntensity = 0;
        GlitchEffect.Instance.flipIntensity = 0;
        GlitchEffect.Instance.intensity = 0;

        //trail Effect 남기고
        effect.transform.SetParent(null);
        effect.transform.position = playerRb.transform.position;

        // 자고있는 닭 상태로 다시 돌아간다.
        playerRb.transform.position = sleepPlayer.transform.position;
        playerAn.GetComponent<SpriteRenderer>().flipX = sleepPlayer.GetComponent<SpriteRenderer>().flipX;
        sleepPlayer.GetComponent<SleepingPlayer>().BubbleAwake();
        sleepPlayer.transform.position = new Vector3(-18, 10, 0);
        playerRb.GetComponent<Rigidbody2D>().simulated = true;

        // 현재로 돌아가는 화면 이펙트
        abilityEffectAnim.SetTrigger("OrangeT");

        // 트랩들 리셋
        foreach (ResetAbleTrap trap in traps)
        {
            trap.Reset();
        }

        // 소리 바꾸고
        GameManager.Instance.SetAudio(audioSource, Audio_presentEnter, 1);
        GameManager.Instance.SetAudio(bgAudioSource, GameManager.Instance.curChapterInfo.chapterBGM, GameManager.Instance.defaultBGMvolume, true);
    }
```

PoolManager.cs

```
public class PoolManager
{    
    private static Dictionary<string, IPool> poolDic = new Dictionary<string, IPool>();

    public static void CreatePool<T>(GameObject prefab, Transform parent, int count = 5) where T : MonoBehaviour
    {
        Type t = typeof(T);

        ObjectPooling<T> pool = new ObjectPooling<T>(prefab, parent, count);
        
        poolDic.Add(t.ToString(), pool);
    }

    public static T GetItem<T>() where T : MonoBehaviour
    {
        Type t = typeof(T);
        ObjectPooling<T> pool = (ObjectPooling<T>)poolDic[t.ToString()];
        return pool.GetOrCreate();
    }

    public static void ResetPool()
    {
        poolDic.Clear();
    }
}
```
