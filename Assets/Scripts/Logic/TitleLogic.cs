using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TournamentSDKUnity;

public class TitleLogic : MonoBehaviour
{
    public Text versionText;

    public Text gameStartText;
    public Outline gameStartOutline;
    public Shadow gameStartShadow;

    public GameObject touchBlock;
    public GameObject NoneTouch;

    private Color startTextColor;

    private Coroutine startTextColorCorutine;

    public int cloudCount = 0;

    private void Awake()
    {
        versionText.text = $"Ver.{Application.version}";

        startTextColor = gameStartText.color;
        startTextColorCorutine = StartCoroutine(StartTextColor());

    }

    void SetNotiChannel(string notify)
    {
        if(notify.Contains("mute"))
        {
            MuteData muteData = JsonUtility.FromJson<MuteData>(notify);
            Debug.Log($"Mute = {muteData.mute}");

            SoundMgr.Instance.SetMute(muteData.mute);
        }
        else
        {
            Debug.Log($"No Case = {notify}");
        }
    }


    private async void Start()
    {
        var init = await TournamentUnitySDK.Instance.Init("dev", "57859d9618ab477eade234ef9da1a05d", SetNotiChannel);
        CloudGenerate();       
    }

    IEnumerator StartTextColor()
    {
        yield return null;

        bool isAdd = false;
        Color copyColor = startTextColor;
        Color effectColor = Color.black;

        while (true)
        {
            if (isAdd == false)
            {
                copyColor.a -= Time.deltaTime * 5;
                effectColor.a -= Time.deltaTime * 5;

                if (copyColor.a <= 0)
                    isAdd = true;
            }
            else if (isAdd == true)
            {
                copyColor.a += Time.deltaTime * 3;
                effectColor.a += Time.deltaTime * 3;

                if (copyColor.a >= 1)
                    isAdd = false;
            }

            gameStartText.color = copyColor;
            gameStartOutline.effectColor = effectColor;
            gameStartShadow.effectColor = effectColor;

            yield return null;
        }
    }

    private void CloudGenerate()
    {
        for (int i = 0; i < cloudCount; i++)
        {
            Cloud cloud = ObjectPoolMgr.Instance.Load<Cloud>(PoolObjectType.Cloud, $"Cloud{Random.Range(1,3)}");
            cloud.Initilize();
        }
    }

    //Web Token Request
    public async void InitWebToken()
    {
        NoneTouch.SetActive(true);
        touchBlock.SetActive(true);

        gameStartText.color = startTextColor;

        Debug.Log("GetToken");
        try
        {
            var token = await TournamentUnitySDK.Instance.RequestToken();
            UserManager.Instance.OnUser(token);
            OnToken(true);
        } catch(TournamentErrorException ex)
        {
            Debug.LogWarning($"[GetToken Error] :: {ex.code}, {ex.message}");
        }   
    }

    //Web Token Response
    private void OnToken(bool result)
    {
        if (result)
            NetworkMgr.Instance.OnConnect(GetStartData);

        else
            Debug.LogError("Token Is Null");
    }

    //Game Start Request
    private void GetStartData()
    {
        GameStartReqDto gameStartReqDto = new();
        NetworkMgr.Instance.RequestStartGame(gameStartReqDto);
        NoneTouch.SetActive(false);
    }
}
