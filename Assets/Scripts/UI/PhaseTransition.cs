using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PhaseTransition : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup transitionPanel;
    [SerializeField] private TextMeshProUGUI phaseText;
    
    [Header("Animation Settings")]
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float displayDuration = 1.0f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    
    private void Start()
    {
        if (transitionPanel != null)
            transitionPanel.alpha = 0f;
    }

    public IEnumerator ShowPhaseTransition(string phaseName)
    {
        // Start music transition early for smoother experience
        AudioManager.Instance.ChangeBGMForPhase(phaseName);
        
        // Set text
        phaseText.text = phaseName;
        
        // Fade in
        yield return FadePanel(0f, 1f, fadeInDuration);
        
        // Hold
        yield return new WaitForSeconds(displayDuration);
        
        // Fade out
        yield return FadePanel(1f, 0f, fadeOutDuration);
    }

    private IEnumerator FadePanel(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transitionPanel.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }
        
        transitionPanel.alpha = endAlpha;
    }
}