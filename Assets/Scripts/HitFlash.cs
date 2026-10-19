using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HitFlash : MonoBehaviour
{
    [Header("Hit Flash")] 
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    private readonly List<Color> originalColors  = new List<Color>();
    private readonly List<Material> materials = new List<Material>();
    private Coroutine flashRoutine;

    private void Awake()
    {
        if (renderers == null || renderers.Length == 0)
        {
            renderers = GetComponentsInChildren<Renderer>();
        }

        foreach (Renderer r in renderers)
        {
            foreach (Material m in r.materials)
            {
                materials.Add(m);
                originalColors.Add(m.color);
            }
        }
    }
    private IEnumerator FlashEffect()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            if (materials[i] != null)
            {
                materials[i].color = hitColor;
            }
        }
        yield return new WaitForSeconds(flashDuration);

        ResetColors();
        flashRoutine = null;
    }
    public void Flash()
    {
        if (materials.Count == 0) return;
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashEffect());
    }

    private void ResetColors()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            if (materials[i] != null)
            {
                materials[i].color = originalColors[i];
            }
        }
    }

    private void OnDisable()
    {
        ResetColors();
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
            flashRoutine = null;
        }
    }
}
