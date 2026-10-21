using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    
    [SerializeField] private AudioClip hoverSFX;
    [SerializeField] private AudioClip clickSFX;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponentInParent<AudioSource>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSFX);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSFX);
        }
    }
}
