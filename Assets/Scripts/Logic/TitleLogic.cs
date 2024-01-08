using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleLogic : MonoBehaviour
{
    public Text versionText;

    public Text gameStartText;
    public Outline gameStartOutline;
    public Shadow gameStartShadow;

    public GameObject touchBlock;

    private Color startTextColor;

    private Coroutine startTextColorCorutine;

    public int cloudCount = 0;

    private void Awake()
    {
        versionText.text = $"Ver.{Application.version}";

        startTextColor = gameStartText.color;
        startTextColorCorutine = StartCoroutine(StartTextColor());
    }

    private void Start()
    {
        WebNetworkMgr.Instance.InitTargetGame(() =>
        {
            Debug.Log("Loding False");
            WebNetworkMgr.Instance.SetLoading(false);

            CloudGenerate();
        });
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
    public void InitWebToken()
    {
        touchBlock.SetActive(true);

        gameStartText.color = startTextColor;

        Debug.Log("GetToken");
        WebNetworkMgr.Instance.InitNetwork(OnToken);
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
    }
}
