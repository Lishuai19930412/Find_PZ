using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZ : MonoBehaviour
{
    private Animator m_Atr;
    private float m_SayHelloTimer;
    private float m_WaitToSayHello;

    private void Awake()
    {
        m_Atr = GetComponent<Animator>();
        m_WaitToSayHello = 0.5f;
    }
    private void Update()
    {
        if (m_SayHelloTimer < m_WaitToSayHello)
        {
            m_SayHelloTimer += Time.deltaTime;

            if (m_SayHelloTimer >= m_WaitToSayHello)
            {
                SayHello();
            }
        }
    }

    public void SayHello()
    {
        m_Atr.SetTrigger("ToWaving");
        m_SayHelloTimer = 0f;
        m_WaitToSayHello = Random.Range(4f, 10f);
    }
}
