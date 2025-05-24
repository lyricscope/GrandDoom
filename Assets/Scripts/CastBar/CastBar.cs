using System;
using System.Collections;
using TMPro;
using UnityEngine;

[Flags]
public enum CastBarAnimation
{ 
    none = 0,
    size = 1,
    opacity = 2,
}

public class CastBar : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Transform uiContainer;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float defaultWidth = 250f;
    [SerializeField] private float defaultHeight = 75f; 

    public Action OnCastStart; 
    public Action OnCastComplete;
    public Action OnCastCanceled;

    private bool isCasting;
    private float startCastTime;
    private float castDuration;
    private GameObject castBarObject;
    private CanvasGroup canvasGroup;
    private RectTransform castBarTransform;
    private RectTransform castBarFillTransform;
    private TextMeshProUGUI castBarLabel;
    private CastBarAnimation animateIn;
    private CastBarAnimation animateOut;

    private void Start()
    {
        // If no container is defined, grab the first canvas
        if (!uiContainer)
            uiContainer = FindObjectOfType<Canvas>().transform;

        // Spawn the visual
        castBarObject = Instantiate(prefab, uiContainer.transform);
        castBarTransform = castBarObject.GetComponent<RectTransform>();
        canvasGroup = castBarTransform.GetComponent<CanvasGroup>();
        castBarFillTransform = castBarTransform.GetChild(0).GetComponent<RectTransform>();
        castBarLabel = castBarTransform.GetChild(1).GetComponent<TextMeshProUGUI>();

        Hide();
    }

    private void Update()
    {
        if (isCasting)
            UpdateCastBar();
    }
    private void UpdateCastBar()
    {
        float completionRatio = (Time.time - startCastTime) / castDuration;
        castBarFillTransform.localScale = new Vector3(completionRatio, 1, 1);

        if (completionRatio >= 1)
        { 
            castBarFillTransform.localScale = Vector3.one;
            CompleteCast();
        }
    }

    public bool CanCast()
    {
        // Verify if we can cast, through your own logic
        return true;
    }
    public void StartCast(float time)
    {
        StartCast(time, "", defaultWidth);
    }
    public void StartCast(float time, string label)
    {
        StartCast(time, label, defaultWidth);
    }
    public void StartCast(float time, string label, float customWidth)
    {
        StartCast(time, label, customWidth, CastBarAnimation.none);
    }
    public void StartCast(float time, string label, CastBarAnimation animateIn = CastBarAnimation.none, CastBarAnimation animateOut = CastBarAnimation.none)
    {
        StartCast(time, label, defaultWidth, animateIn, animateOut);
    }
    public void StartCast(float time, string label, float customWidth, CastBarAnimation animateIn = CastBarAnimation.none, CastBarAnimation animateOut = CastBarAnimation.none)
    {
        if (!CanCast())
        {
            Debug.Log("CastBar::CanCast->false... Unable to cast right now...");
            return;
        }

        // Show the cast bar, register params
        OnCastStart?.Invoke();
        this.animateIn = animateIn;
        this.animateOut = animateOut;
        Show(animateIn);
        isCasting = true;
        castDuration = time;

        // Change the size of the cast bar, prior to the cast
        castBarTransform.sizeDelta = new Vector2(customWidth, defaultHeight);

        // Reset the fill bar
        castBarFillTransform.localScale = new Vector3(0, 1, 1);

        // Register the start cast time
        startCastTime = Time.time;

        // Write the Label on the cast bar
        castBarLabel.text = label;
    }
    private void CompleteCast()
    {
        Hide(animateOut);

        isCasting = false;
        OnCastComplete?.Invoke();
    }
    private void CancelCast()
    {
        Hide();

        isCasting = false;
        OnCastCanceled?.Invoke();
    }
    
    // Visuals
    private void Show(CastBarAnimation inAnimation = CastBarAnimation.none, float duration = 0.25f)
    {
        StopAllCoroutines();

        //Instant
        if (inAnimation == CastBarAnimation.none)
        {
            canvasGroup.alpha = 1;
            castBarObject.SetActive(true);
        }

        if ((inAnimation & CastBarAnimation.opacity) != 0)
            StartCoroutine(OpacityAnimation(true, duration));
        if ((inAnimation & CastBarAnimation.size) != 0)
            StartCoroutine(SizeAnimation(true, duration));
    }
    private void Hide(CastBarAnimation outAnimation = CastBarAnimation.none, float duration = 1f)
    {
        StopAllCoroutines();

        //Instant
        if (outAnimation == CastBarAnimation.none)
        {
            canvasGroup.alpha = 0;
            castBarObject.SetActive(false);
        }

        if ((outAnimation & CastBarAnimation.opacity) != 0)
            StartCoroutine(OpacityAnimation(false, duration));
        if ((outAnimation & CastBarAnimation.size) != 0)
            StartCoroutine(SizeAnimation(false, duration));
    }

    // Animation Coroutine
    private IEnumerator OpacityAnimation(bool animateIn, float time)
    {
        // Fade In
        if (animateIn)
        {
            castBarObject.SetActive(true);

            for (float i = 0; i < time; i += Time.deltaTime)
            {
                canvasGroup.alpha = (i / time);
                yield return null;
            }

            canvasGroup.alpha = 1;
        }
        // Fade out
        else
        { 
            for (float i = 0; i < time; i += Time.deltaTime)
            {
                canvasGroup.alpha = 1 - (i / time);
                yield return null;
            }

            canvasGroup.alpha = 0;
            castBarObject.SetActive(false);
        }

        yield return null;
    }
    private IEnumerator SizeAnimation(bool animateIn, float time)
    {
        canvasGroup.alpha = 1;

        // Fade In
        if (animateIn)
        {
            castBarObject.SetActive(true);
            castBarTransform.sizeDelta = new Vector2(0, defaultHeight);

            for (float i = 0; i < time; i += Time.deltaTime)
            {
                castBarTransform.sizeDelta = new Vector2((i / time) * defaultWidth, defaultHeight);
                yield return null;
            }
        }
        // Fade out
        else
        {
            castBarTransform.sizeDelta = new Vector2(defaultWidth, defaultHeight);

            for (float i = 0; i < time; i += Time.deltaTime)
            {
                castBarTransform.sizeDelta = new Vector2((1 - (i / time)) * defaultWidth, defaultHeight);
                yield return null;
            }

            castBarObject.SetActive(false);
        }

        yield return null;
    }
}
