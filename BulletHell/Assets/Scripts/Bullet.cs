using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 50f;
    public float timeDestroy = 3f;

    void Start()
    {
        // Rota la bala 90 grados en el eje X para que visualmente esté orientada correctamente
        transform.Rotate(90f, 0f, 0f);

        // Inicializa el Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb != null) 
        {
            // Aplica la velocidad hacia adelante en función de la rotación del objeto (ya rotado en X)
            rb.velocity = transform.up * speed; // Usa 'up' para moverlo según la rotación ajustada
        }
        else
        {
            Debug.LogError("Rigidbody no encontrado en la bala.");
        }
        
        Destroy(gameObject, timeDestroy); // Destruye la bala después de 'timeDestroy' segundos
    }
}

