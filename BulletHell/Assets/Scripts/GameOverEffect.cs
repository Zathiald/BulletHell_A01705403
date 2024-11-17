using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Solo si estás usando TextMeshPro

public class GameOverEffect : MonoBehaviour
{
    public Image fadeImage;               // Imagen negra que se usa para oscurecer la pantalla
    public TextMeshProUGUI gameOverText;   // Texto "GAME OVER" (usa Text si no estás usando TextMeshPro)
    public Button restartButton;           // Botón para reiniciar el juego
    public TextMeshProUGUI restartButtonText; // Texto dentro del botón
    public float fadeDuration = 1.5f;      // Duración del efecto de oscurecimiento
    public float textMoveDuration = 2f;    // Duración de la animación de movimiento del texto
    public float buttonFadeDuration = 1.5f; // Duración del fade del botón
    public float buttonMoveDuration = 1f;  // Duración del movimiento del botón
    public Vector3 buttonStartPosition;    // Posición inicial del botón
    public Vector3 buttonEndPosition;      // Posición final del botón (más abajo)

    private void Start()
    {
        // Asegúrate de que el fadeImage y el gameOverText empiecen ocultos
        fadeImage.color = new Color(0, 0, 0, 0);
        gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, 0);
        
        // Hacer que el botón y su texto comiencen invisibles
        restartButton.GetComponent<Image>().color = new Color(restartButton.GetComponent<Image>().color.r, restartButton.GetComponent<Image>().color.g, restartButton.GetComponent<Image>().color.b, 0);
        restartButtonText.color = new Color(restartButtonText.color.r, restartButtonText.color.g, restartButtonText.color.b, 0);

        // Desactivar la interacción del botón al inicio
        restartButton.interactable = false;

        // Guardamos la posición inicial del botón
        buttonStartPosition = restartButton.GetComponent<RectTransform>().position;
        // Define la nueva posición final del botón (más abajo)
        buttonEndPosition = new Vector3(buttonStartPosition.x, buttonStartPosition.y - 100f, buttonStartPosition.z); // Ajusta el valor de -100f según lo que necesites
    }

    public void TriggerGameOver()
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

        // Paso 2: Mostrar y mover el texto "GAME OVER" desde arriba
        Vector3 startPosition = gameOverText.rectTransform.position;
        Vector3 endPosition = new Vector3(startPosition.x, Screen.height / 2, startPosition.z); // Centro de la pantalla

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

        // Paso 3: Hacer que el botón y su texto se desvanezcan mientras se mueve hacia abajo
        float buttonFadeElapsed = 0f;
        float buttonMoveElapsed = 0f;
        while (buttonFadeElapsed < buttonFadeDuration || buttonMoveElapsed < buttonMoveDuration)
        {
            buttonFadeElapsed += Time.deltaTime;
            buttonMoveElapsed += Time.deltaTime;

            // Calcula el alpha del fade para el botón
            float buttonAlpha = Mathf.Clamp01(buttonFadeElapsed / buttonFadeDuration);

            // Interpolación para mover el botón hacia abajo
            float moveProgress = Mathf.Clamp01(buttonMoveElapsed / buttonMoveDuration);
            Vector3 buttonNewPosition = Vector3.Lerp(buttonStartPosition, buttonEndPosition, moveProgress);

            // Fade de la imagen del botón
            restartButton.GetComponent<Image>().color = new Color(restartButton.GetComponent<Image>().color.r, restartButton.GetComponent<Image>().color.g, restartButton.GetComponent<Image>().color.b, buttonAlpha);

            // Fade del texto del botón
            restartButtonText.color = new Color(restartButtonText.color.r, restartButtonText.color.g, restartButtonText.color.b, buttonAlpha);

            // Mover el botón a su nueva posición
            restartButton.GetComponent<RectTransform>().position = buttonNewPosition;

            yield return null;
        }

        // Hacer que el botón sea interactivo después del fade
        restartButton.interactable = true;
    }

    public void RestartGame()
    {
        // Lógica para cargar la escena "FirstPart"
        UnityEngine.SceneManagement.SceneManager.LoadScene("FirstPart");
    }
}
