using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_AudioSource;
    [SerializeField]
    private AudioClip m_ClipOnTop;
    [SerializeField]
    private AudioClip m_ClipClick;

    public void OnPointerOn()
    {
        if (m_AudioSource != null)
        {
            m_AudioSource.clip = m_ClipOnTop;
            m_AudioSource.Play();
        }
    }
    public void OnPointerClick()
    {
        if (m_AudioSource != null)
        {
            m_AudioSource.clip = m_ClipClick;
            m_AudioSource.Play();
        }
    }
}
