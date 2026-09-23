using UnityEngine;
using UnityEngine.LowLevelPhysics2D;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    
   [Header("Audio Source")]
   [SerializeField] private AudioSource audioSource;

   [Header("Combat SFX")] 
   [SerializeField] private AudioClip bowSound;
   [SerializeField] private AudioClip swordSwingSound;
   [SerializeField] private AudioClip swordImpactSound;
   [SerializeField] private AudioClip hurtSound;
   [SerializeField] private AudioClip stepSound;
   [Header("Footsteps Settings")]
   [SerializeField] private float stepCooldown = 0.18f; 
   [SerializeField] private CharacterController characterController;
   private float lastStepTime;
    void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayBowSound()
    {
        PlayClipWithPitch(bowSound, 0.95f, 1.05f);
    }
    public void PlaySwordSound()
    {
        PlayClipWithPitch(swordSwingSound, 0.95f, 1.05f);
    }

    public void PlaySwordImpactSound()
    {
        PlayClipWithPitch(swordImpactSound, 0.95f, 1.05f);
    }
    private void PlayClipWithPitch(AudioClip clip, float minPitch, float maxPitch)
    {
        if (clip == null) return;
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(clip);
    }

    public void PlayStepSound()
    {
        
        bool isMoving = false;
        if (Keyboard.current != null)
        {
            isMoving = Keyboard.current.wKey.isPressed ||
                       Keyboard.current.sKey.isPressed ||
                       Keyboard.current.aKey.isPressed ||
                       Keyboard.current.dKey.isPressed;
        }

        if (!isMoving) return; 

        if (characterController != null && !characterController.isGrounded) return;
        
        if (Time.time - lastStepTime < stepCooldown) return;
        if (stepSound == null) return;

        lastStepTime = Time.time;
        PlayClipWithPitch(stepSound, 0.95f, 1.05f);
    }
    public void PlayHurtSound()
    {
        PlayClipWithPitch(hurtSound, 0.95f, 1.05f);
    }
}
