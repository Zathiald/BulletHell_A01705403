using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetWall : MonoBehaviour
{
    public float boundaryRadius = 5.0f; // Ajusta este valor según el tamaño de tu planeta
    public Transform player; // Asegúrate de asignar esta variable en el inspector de Unity

    void Update()
    {
        // Calcula la distancia entre el jugador y el planeta
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // Si el jugador está demasiado cerca del planeta...
        if (distance < boundaryRadius)
        {
            // Calcula la dirección desde el jugador al planeta
            Vector3 fromPlanetToPlayer = player.transform.position - transform.position;

            // Normaliza la dirección (la convierte en una longitud de 1)
            fromPlanetToPlayer.Normalize();

            // Mueve al jugador de vuelta a la "pared invisible"
            player.transform.position = transform.position + (fromPlanetToPlayer * boundaryRadius);
        }
    }
}
