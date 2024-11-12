using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 50f;
    public float timeDestroy = 3f;

    // Start is called before the first frame update
    void Start()
    {
        // Inicializa el Rigidbody
        rb = GetComponent<Rigidbody>();
        
        if (rb != null) 
        {
            rb.velocity = transform.forward * speed; // Aplica la velocidad hacia adelante
        }
        else
        {
            Debug.LogError("Rigidbody no encontrado en la bala.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, timeDestroy); // Destruye la bala después de 'timeDestroy' segundos
    }
}
