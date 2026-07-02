using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource uiAudioSource;
    
    [Header("Hover Clip")]
    [SerializeField] private AudioClip hoverClip;

    [Header("Click Clip")]
    [SerializeField] private AudioClip clickClip;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayClickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();
    }

    public void OnSelect(BaseEventData eventData)
    {
        PlayHoverSound();
    }

    private void PlayHoverSound()
    {
        if (hoverClip != null && uiAudioSource != null)
            uiAudioSource.PlayOneShot(hoverClip);
    }

    private void PlayClickSound()
    {
        if (clickClip != null && uiAudioSource != null)
            uiAudioSource.PlayOneShot(clickClip);
    }
}