using UnityEngine;
using UnityEngine.InputSystem;

public class NpcOutlineRaycast : MonoBehaviour
{
    [SerializeField] private float reachDistance = 5f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private QuestManager QuestManager;
    private Outline currentOutline;
    
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, reachDistance, interactableLayer))
        {
            Outline hitOutline = hit.collider.GetComponent<Outline>();

            if (hitOutline != null)
            {
                if (currentOutline != hitOutline)
                {
                    DisableCurrentOutline();
                    currentOutline = hitOutline;
                    currentOutline.enabled = true;
                }
                if(Keyboard.current.eKey.wasPressedThisFrame)
                {
                    QuestManager.Instance.StartSlimeQuest();
                }
                return;
            }
        }
        
        DisableCurrentOutline();
    }


    private void DisableCurrentOutline()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }
    }
}
