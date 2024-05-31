using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalMgr : MonoBehaviour
{
    [SerializeField] List<Transform> m_CityPoint;
    [SerializeField] GameObject m_EffectPbf;

    [SerializeField] Camera m_CameraMain;
    [SerializeField] Transform m_CameraFocus;
    [SerializeField] Player m_Player;
    [SerializeField] PZ m_PZ;

    [SerializeField] Canvas m_CanvasMain;
    [SerializeField] RectTransform m_RT_MG;
    [SerializeField] RectTransform m_RT_PZ;
    [SerializeField] RectTransform m_Dialog;

    private List<string> m_CityEnter = new List<string>(3);
    private string m_PZCity;
    private float m_DialogTimer;
    private float m_DialogShowTimer;

    private static string[] m_Speak = new string[]
    {
        "我现在在xx了，赶紧来接我，真的。",
        "今晚约了个xx本地的妹子，现在在xx。",
        "我现在在xx了，今晚约36D那个。",
        "今晚在xx。",
        "只有有空的时候都可以约见啊，我现在在xx。",
        "有空就xx见面啊。",
        "我回xx了。",
        "我在xx啊。"
    };

    private static Dictionary<string, string> m_P2Z = new Dictionary<string, string>()
    {
        { "ChaoZhou","潮州" },
        { "DongGuan","东莞" },
        { "FoShan","佛山" },
        { "GuangZhou","广州" },
        { "HeYuan","河源" },
        { "HuiZhou","惠州" },
        { "JiangMen","江门" },
        { "JieYang","揭阳" },
        { "MaoMing","茂名" },
        { "MeiZhou","梅州" },
        { "QingYuan","清远" },
        { "ShanTou","汕头" },
        { "ShanWei","汕尾" },
        { "ShaoGuan","韶关" },
        { "ShenZhen","深圳" },
        { "YangJiang","阳江" },
        { "YunFu","云浮" },
        { "ZhanJiang","湛江" },
        { "ZhaoQing","肇庆" },
        { "ZhongShan","中山" },
        { "ZhuHai","珠海" }
    };

    private void Start()
    {
        m_CityEnter.Add("FoShan");
        m_Player.transform.position = m_CityPoint.Find(s => s.name.Equals("FoShan")).position;

        m_PZCity = "GuangZhou";
        m_PZ.transform.position = m_CityPoint.Find(s => s.name.Equals("GuangZhou")).position;
        ShowDialog();

        m_Player.onCityEnter += (ctName) => 
        {
            if (m_CityEnter.Contains(ctName) == false)
            {
                m_CityEnter.Add(ctName);

                if (m_CityEnter.Contains(m_PZCity) == true)
                {
                    GameObject effect = Instantiate(m_EffectPbf);
                    effect.transform.localScale = Vector3.one * 5;
                    effect.transform.position = m_PZ.transform.position + new Vector3(0, 1f, 0);
                    StartCoroutine(DelayToDestroy(effect, 3));
                    List<Transform> nextCityList = m_CityPoint.FindAll(s => m_CityEnter.Contains(s.name) == false);
                    int nextCityID = Random.Range(0, nextCityList.Count);
                    m_PZCity = nextCityList[nextCityID].name;
                    m_PZ.transform.position = nextCityList[nextCityID].position;
                    m_PZ.SayHello();
                    ShowDialog();
                }
            }
        };

        m_Player.onCityExit += (ctName) =>
        {
            if (m_CityEnter.Contains(ctName) == true)
            {
                if (m_CityEnter.Count == 1)
                {
                    m_Player.transform.position = m_CityPoint.Find(s => s.name.Equals(ctName)).position;
                }
                else
                {
                    m_CityEnter.Remove(ctName);
                }
            }
        };
    }
    private void Update()
    {
        if (m_DialogTimer < 7)
        {
            m_DialogTimer += Time.deltaTime;

            if(m_DialogTimer >= 7)
            {
                m_DialogTimer = 7;
                m_DialogShowTimer = 0;
                m_Dialog.gameObject.SetActive(false);
            }
        }
        else
        {
            m_DialogShowTimer += Time.deltaTime;

            if(m_DialogShowTimer >= 3.5f)
            {
                ShowDialog();
            }
        }
    }
    private void LateUpdate()
    {
        m_CameraFocus.position = m_Player.CameraFocus;

        float w_h = (Screen.width / (float)Screen.height) / (1920f / 1080f);

        Vector3 mg_p = m_CameraMain.WorldToScreenPoint(m_Player.transform.position - new Vector3(0, 0.65f, 0));
        m_RT_MG.anchoredPosition = new Vector2(mg_p.x * (1920f / Screen.width) * w_h, mg_p.y * (1080f / Screen.height));

        Vector3 pz_p = m_CameraMain.WorldToScreenPoint(m_PZ.transform.position - new Vector3(0, 0.65f, 0));
        m_RT_PZ.anchoredPosition = new Vector2(pz_p.x * (1920f / Screen.width) * w_h, pz_p.y * (1080f / Screen.height));

        Vector3 dl_p = m_CameraMain.WorldToScreenPoint(m_PZ.transform.position + new Vector3(0, 2.2f, 0));
        m_Dialog.anchoredPosition = new Vector2(dl_p.x * (1920f / Screen.width) * w_h, dl_p.y * (1080f / Screen.height));

    }
    private IEnumerator DelayToDestroy(GameObject g, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(g);
    }
    private void ShowDialog()
    {
        m_Dialog.gameObject.SetActive(true);
        m_Dialog.GetComponentInChildren<Text>().text = m_Speak[Random.Range(0, m_Speak.Length)].Replace("xx", $"<color=red>{m_P2Z[m_PZCity]}</color>");
        m_DialogTimer = 0;
        m_DialogShowTimer = 0;
    }
}
