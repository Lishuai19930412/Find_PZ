using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] Transform m_CameraFocus;

    private Animator m_Atr;
    private Tweener m_Twr;
    private Vector3 m_lastV;

    public Vector3 CameraFocus
    {
        get
        {
            return m_CameraFocus.position;
        }
    }
    public UnityAction<string> onCityEnter;
    public UnityAction<string> onCityExit;

    private void Awake()
    {
        m_Atr = GetComponent<Animator>();
    }
    private void Update()
    {
        Vector3 v = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), 1);
        transform.position += v * Time.deltaTime * 4f;

        if (m_lastV != v)
        {
            Quaternion tagQuat = v.magnitude > 0 ? Quaternion.LookRotation(v, transform.up) : transform.rotation;
            m_Twr?.Kill();
            m_Twr = transform.DORotateQuaternion(tagQuat, 0.07f).SetEase(Ease.Linear);
            m_lastV = v;
        }

        bool running = v.magnitude > 0 ? true : false;
        m_Atr.SetBool("running", running);
        m_Atr.speed = running == true ? 0.5f + 0.7f * v.magnitude : 1;
    }
    public void OnTriggerEnter(Collider other)
    {
        onCityEnter?.Invoke(other.name);
    }
    public void OnTriggerExit(Collider other)
    {
        onCityExit?.Invoke(other.name);
    }
}
