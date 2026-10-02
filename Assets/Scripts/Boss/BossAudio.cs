using UnityEngine;

public class BossAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [Header("Dźwięki Bossa")] 
    [SerializeField] private AudioClip clubSwing;
    [SerializeField] private AudioClip bossGroan;
    [SerializeField] private AudioClip bossDeath;
    [SerializeField] private AudioClip bossGasp;
    [SerializeField] private AudioClip bossStep;
    [SerializeField] private AudioClip groundSlam;
    void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    

    public void PlaySwingSound()
    {
        PlayClipWithPitch(clubSwing, 0.95f, 1.05f);
    }

    public void PlayDeathSound()
    {
        PlayClipWithPitch(bossDeath, 0.95f, 1.05f);
    }

    public void PlayGaspSound()
    {
        PlayClipWithPitch(bossGasp, 0.9f, 1.1f);
    }

    public void PlayGroanSound()
    {
        PlayClipWithPitch(bossGroan, 0.9f, 1.1f);
    }
    private void PlayClipWithPitch(AudioClip clip, float minPitch, float maxPitch)
    {
        if (clip == null) return;
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(clip);
    }
    public void PlayStepSound()
    {
        PlayClipWithPitch(bossStep, 0.95f, 1.05f);
    }
    public void PlayGroundSlamSound()
    {
        PlayClipWithPitch(groundSlam, 0.95f, 1.05f);
    }
}
