using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NpcOutlineRaycast : MonoBehaviour
{
    [Header("Outline Settings")]
    [SerializeField] private float reachDistance = 5f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private QuestManager QuestManager;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI talkText;
    
    private Outline currentOutline;

    private void Start()
    {
        if (talkText != null)
        {
            talkText.gameObject.SetActive(false);
        }
    }
    
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, reachDistance, interactableLayer))
        {
            Outline hitOutline = hit.collider.GetComponent<Outline>() ?? hit.collider.GetComponentInParent<Outline>();

            if (hitOutline != null)
            {
                if (currentOutline != hitOutline)
                {
                    DisableCurrentOutline();
                    currentOutline = hitOutline;
                    currentOutline.enabled = true;
                }

                if (talkText != null && !talkText.gameObject.activeSelf)
                {
                    talkText.gameObject.SetActive(true);
                }
                
                if(Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    bool isWizard = hitOutline.name.Contains("Wizard_NPC") || hit.transform.root.name.Contains("Wizard_NPC");
                    if (isWizard)
                    {
                        QuestManager.Instance.StartSlimeQuest();
                    }
                    
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

        if (talkText != null && talkText.gameObject.activeSelf)
        {
            talkText.gameObject.SetActive(false);
        }
    }
}
