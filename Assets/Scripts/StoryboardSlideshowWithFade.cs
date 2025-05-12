using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StoryboardSlideshowWithFade : MonoBehaviour
{
    public Sprite[] slides;
    public float delay = 3f;
    public float fadeDuration = 1f;
    public bool autoAdvance = true;
    public GameObject continueButton; // Button to show at the end

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
            canvasGroup.alpha = 1;
        }

        if (continueButton != null)
        {
            continueButton.SetActive(false); // hide initially
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
        // Don't go past the last slide
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

        // Move to next slide
        currentIndex++;
        imageComponent.sprite = slides[currentIndex];

        // Fade in
        yield return StartCoroutine(Fade(0f, 1f));
        isTransitioning = false;

        // If we're now on the last slide, show the button
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
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = end;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
