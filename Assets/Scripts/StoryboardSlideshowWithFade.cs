using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class StoryboardSlideshowWithFade : MonoBehaviour
{
    public Sprite[] slides;
    public string[] slideTexts; // Text per slide
    public float delay = 3f;
    public float fadeDuration = 1f;
    public bool autoAdvance = true;
    public GameObject continueButton;

    public TMP_Text StoryText; // Reference to the UI Text element

    private Image imageComponent;
    private CanvasGroup canvasGroup;
    private int currentIndex = 0;
    private bool isTransitioning = false;

    void Start()
    {
        imageComponent = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (slides.Length > 0)
        {
            imageComponent.sprite = slides[0];
            if (StoryText != null && slideTexts.Length > 0)
                StoryText.text = slideTexts[0];
            canvasGroup.alpha = 1;
        }

        if (continueButton != null)
        {
            continueButton.SetActive(false);
        }

        if (autoAdvance)
        {
            StartCoroutine(AutoAdvanceSlides());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTransitioning)
        {
            StartCoroutine(FadeToNextSlide());
        }
    }

    IEnumerator AutoAdvanceSlides()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (!isTransitioning)
                yield return FadeToNextSlide();
        }
    }

    IEnumerator FadeToNextSlide()
    {
        if (currentIndex >= slides.Length - 1)
        {
            if (continueButton != null)
            {
                continueButton.SetActive(true);
            }
            yield break;
        }

        isTransitioning = true;

        // Fade out
        yield return StartCoroutine(Fade(1f, 0.2f));

        // Update image and text
        currentIndex++;
        imageComponent.sprite = slides[currentIndex];

        if (StoryText != null && currentIndex < slideTexts.Length)
        {
            StoryText.text = slideTexts[currentIndex];
        }

        // Fade in
        yield return StartCoroutine(Fade(0.2f, 1f));
        isTransitioning = false;

        if (currentIndex == slides.Length - 1 && continueButton != null)
        {
            continueButton.SetActive(true);
        }
    }

    IEnumerator Fade(float start, float end)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(start, end, t);
            yield return null;
        }
        canvasGroup.alpha = end;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
