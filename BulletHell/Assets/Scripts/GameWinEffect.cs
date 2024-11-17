using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameWinEffect : MonoBehaviour
{
    public Image fadeImage;             // Imagen negra que se usa para oscurecer la pantalla
    public TextMeshProUGUI gameOverText; // Texto "GAME OVER" (usa Text si no estás usando TextMeshPro)
    public float fadeDuration = 1.5f;   // Duración del efecto de oscurecimiento
    public float textMoveDuration = 2f; // Duración de la animación de movimiento del texto

    private void Start()
    {
        // Asegúrate de que el fadeImage y el gameOverText empiecen ocultos
        fadeImage.color = new Color(0, 0, 0, 0);
        gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, 0);
    }

    public void TriggerGameWin()
    {
        StartCoroutine(FadeToBlackAndShowText());
    }

    private IEnumerator FadeToBlackAndShowText()
    {
        // Paso 1: Desvanecer la pantalla a negro
        float fadeElapsed = 0f;
        while (fadeElapsed < fadeDuration)
        {
            fadeElapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeElapsed / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Paso 2: Mostrar y mover el texto "GAME OVER" desde abajo
        Vector3 startPosition = new Vector3(gameOverText.rectTransform.position.x, -100f, gameOverText.rectTransform.position.z); // Justo fuera de la pantalla, abajo
        Vector3 endPosition = new Vector3(gameOverText.rectTransform.position.x, Screen.height / 2, gameOverText.rectTransform.position.z); // Centro de la pantalla

        gameOverText.rectTransform.position = startPosition; // Asegúrate de que el texto comience fuera de la pantalla

        float textElapsed = 0f;
        while (textElapsed < textMoveDuration)
        {
            textElapsed += Time.deltaTime;
            float t = textElapsed / textMoveDuration;

            // Interpolar la posición y el color del texto
            gameOverText.rectTransform.position = Vector3.Lerp(startPosition, endPosition, t);
            gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, t);

            yield return null;
        }
    }
}
