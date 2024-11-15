using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> ufos;
    public List<GameObject> whiplashes;
    public List<GameObject> tornadoes;
    public List<GameObject> focusObjects; // Lista de objetos especiales de enfoque
    public CinemachineVirtualCamera virtualCamera; // Referencia a la Cinemachine Virtual Camera
    public Transform player; // Referencia al jugador

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

                yield return new WaitForSeconds(0.5f); // Espera un momento antes de destruir el objeto

                // Vuelve a enfocar al jugador
                virtualCamera.LookAt = player;
                
                // Avanza al siguiente objeto de enfoque en la lista
                currentFocusIndex++;
            }
        }
    }
}
