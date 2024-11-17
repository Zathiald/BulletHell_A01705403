using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement; // Para cargar el siguiente nivel

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> ufos;
    public List<GameObject> whiplashes;
    public List<GameObject> tornadoes;
    public List<GameObject> focusObjects; // Lista de objetos especiales de enfoque
    public CinemachineVirtualCamera virtualCamera; // Referencia a la Cinemachine Virtual Camera
    public Transform player; // Referencia al jugador
    public GameObject cinematicObjectsParent; // Grupo de objetos para la cinemática
    public float cinematicDuration = 3f; // Duración de la cinemática
    public string nextLevelName; // Nombre del siguiente nivel

    private int currentFocusIndex = 0; // Índice para controlar el objeto de enfoque actual

    void Start()
    {
        // Activa solo los UFOs al inicio
        ActivateEnemies(ufos, true);
        ActivateEnemies(whiplashes, false);
        ActivateEnemies(tornadoes, false);
    }

    void Update()
    {
        // Comprueba si todos los UFOs han sido destruidos
        if (ufos.Count == 0 && whiplashes.Count > 0 && !whiplashes[0].activeSelf)
        {
            StartCoroutine(FocusAndDestroyObject());
            ActivateEnemies(whiplashes, true);
        }

        // Comprueba si todos los Whiplash han sido destruidos
        if (whiplashes.Count == 0 && tornadoes.Count > 0 && !tornadoes[0].activeSelf)
        {
            StartCoroutine(FocusAndDestroyObject());
            ActivateEnemies(tornadoes, true);
        }

        // Comprueba si todos los enemigos han sido destruidos
        if (ufos.Count == 0 && whiplashes.Count == 0 && tornadoes.Count == 0)
        {
            StartCoroutine(PlayCinematicAndLoadNextLevel());
        }
    }

    private void ActivateEnemies(List<GameObject> enemies, bool activate)
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(activate);
            }
        }
    }

    public void OnEnemyDestroyed(GameObject enemy)
    {
        // Elimina el enemigo de la lista correspondiente
        if (ufos.Contains(enemy))
        {
            ufos.Remove(enemy);
        }
        else if (whiplashes.Contains(enemy))
        {
            whiplashes.Remove(enemy);
        }
        else if (tornadoes.Contains(enemy))
        {
            tornadoes.Remove(enemy);
        }
    }

    private IEnumerator FocusAndDestroyObject()
    {
        if (currentFocusIndex < focusObjects.Count && virtualCamera != null)
        {
            GameObject focusObject = focusObjects[currentFocusIndex];
            if (focusObject != null)
            {
                // Cambia el LookAt de la Cinemachine Virtual Camera para mirar al objeto especial
                virtualCamera.LookAt = focusObject.transform;
                yield return new WaitForSeconds(1f); // Espera un momento antes de destruir el objeto

                // Destruye el objeto especial
                Destroy(focusObject);
                yield return new WaitForSeconds(0.5f);

                // Vuelve a enfocar al jugador
                virtualCamera.LookAt = player;

                // Avanza al siguiente objeto de enfoque en la lista
                currentFocusIndex++;
            }
        }
    }

    private IEnumerator PlayCinematicAndLoadNextLevel()
    {
        if (cinematicObjectsParent != null)
        {
            // Velocidad del descenso
            float speed = 3f;
            Vector3 startPosition = cinematicObjectsParent.transform.position;
            Vector3 targetPosition = new Vector3(
                startPosition.x,
                -1000.00f, // Cambia aquí el objetivo en Y
                startPosition.z
            );

            // Mueve los objetos hacia abajo hasta que alcancen el destino
            while (cinematicObjectsParent.transform.position.y > targetPosition.y + 0.01f)
            {
                cinematicObjectsParent.transform.position = Vector3.MoveTowards(
                    cinematicObjectsParent.transform.position,
                    targetPosition,
                    speed * Time.deltaTime
                );
                yield return null; // Espera hasta el siguiente frame
            }

            // Forzar la posición exacta para asegurar precisión
            cinematicObjectsParent.transform.position = targetPosition;

            // Carga la siguiente escena de manera asíncrona
            if (!string.IsNullOrEmpty(nextLevelName))
            {
                Debug.Log($"Cargando la siguiente escena: {nextLevelName}");

                // Inicia la carga asíncrona de la siguiente escena
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextLevelName);
                asyncLoad.allowSceneActivation = false; // No activar la escena hasta que termine el proceso

                // Mientras la escena se está cargando, espera a que termine
                while (!asyncLoad.isDone)
                {
                    // Si la carga está completada, activamos la escena
                    if (asyncLoad.progress >= 0.9f)
                    {
                        Debug.Log("Escena cargada, esperando para activarla.");
                        // Puedes mostrar un mensaje o una barra de carga si lo deseas aquí
                        yield return new WaitForSeconds(0.5f); // Opcional: espera para dar tiempo al jugador a ver el estado final

                        // Activamos la nueva escena
                        asyncLoad.allowSceneActivation = true;
                    }

                    yield return null; // Espera hasta el siguiente frame
                }
            }
        }
        else
        {
            Debug.LogError("cinematicObjectsParent es nulo. Asegúrate de asignarlo correctamente.");
        }
    }


}
