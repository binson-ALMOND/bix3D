using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    private PlayerCombat playerCombat;
    void Start()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
    }

    public void OnAttackHit()
    {
        if (playerCombat != null)
        {
            playerCombat.OnAttackHit();
        }
    }

    public void OnShootArrow()
    {
        if (playerCombat != null)
        {
            playerCombat.OnShootArrow();
        }
    }
}
