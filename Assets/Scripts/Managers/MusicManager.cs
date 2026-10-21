using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource explorationSource;
    [SerializeField] private AudioSource bossSource;
    [SerializeField] private AudioClip victoryJingle;

    [Range(0f, 1f)] [SerializeField] private float targetVolume = 0.211f;
    [SerializeField] private float delayAfterJingle = 0.5f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (explorationSource != null)
        {
            explorationSource.loop = true;
            explorationSource.volume = targetVolume;
            if (!explorationSource.isPlaying) explorationSource.Play();
        }

        if (bossSource != null)
        {
            bossSource.loop = true;
            bossSource.volume = targetVolume; 
            bossSource.Stop();                
        }
    }

    public void SwitchToBossMusic()
    {
        if (explorationSource != null)
        {
            explorationSource.Pause();
        }

        if (bossSource != null)
        {
            bossSource.volume = targetVolume; 
            bossSource.time = 0f;             
            bossSource.Play();
        }
    }

    public void SwitchToExplorationMusic()
    {
        if (bossSource != null)
        {
            bossSource.Stop();
        }

        if (explorationSource != null)
        {
            explorationSource.time = 0f;
            explorationSource.Play();
        }
    }

    public void TriggerVictory()
    {
        StartCoroutine(VictorySequenceRoutine());
    }

    private IEnumerator VictorySequenceRoutine()
    {
        if (bossSource != null)
        {
            bossSource.Stop();
        }

        if (victoryJingle != null && bossSource != null)
        {
            bossSource.PlayOneShot(victoryJingle);
            yield return new WaitForSeconds(victoryJingle.length + delayAfterJingle);
        }

        if (explorationSource != null)
        {
            explorationSource.time = 0f;
            explorationSource.Play();
        }
    }
}