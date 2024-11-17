using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float forwardSpeed = 25f, strafeSpeed = 10f, hoverSpeed = 5f;
    public float lookRateSpeed = 90f;
    public float tiltAmount = 45;
    public float rollSpeed = 90f, rollAcceleration = 3.5f, boostSpeed = 10f;
    private float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;
    private float forwardAcceleration = 2.5f, strafeAcceleration = 2f, hoverAcceleration = 2f;
    private float rollInput;
    private Vector2 lookInput, screenCenter, mouseDistance;
    public ParticleSystem trail;
    public ParticleSystem circle;
    private float boostTime = 1f;
    private bool isBoosting = false;
    public bool canMove = true;

    // Variables de disparo
    public GameObject bulletPrefab;
    public Transform[] firePoints;
    public float fireRate = 0.5f;
    private float fireCooldown = 0f;

    // Clip de audio de disparo
    public AudioClip fireSound;
    public AudioClip hitSound;
    public AudioClip gameOverSound;
    private AudioSource audioSource;
    public AudioSource backgroundMusic;

    // Variables de vida del jugador
    public float playerHealth = 100f;
    public HealthBarController healthBarController;
    public Renderer playerRenderer;
    public Material originalMaterial;

    void Start()
    {
        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;
        Cursor.lockState = CursorLockMode.Confined;
        healthBarController.TakeDamage(playerHealth);

        // Configura el componente de audio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = fireSound;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }

        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;

        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

        rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Roll"), rollAcceleration * Time.deltaTime);

        transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);

        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAcceleration * Time.deltaTime);
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, Input.GetAxisRaw("Horizontal") * strafeSpeed, strafeAcceleration * Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, Input.GetAxisRaw("Hover") * hoverSpeed, hoverAcceleration * Time.deltaTime);

        transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
        transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Action"))
        {
            Boost(true);
        }

        if (Input.GetButtonUp("Action"))
        {
            Boost(false);
        }

        // Controla el disparo y el tiempo de espera entre ráfagas
        fireCooldown -= Time.deltaTime;
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(1)) && fireCooldown <= 0f)
        {
            FireAllBullets();
            fireCooldown = fireRate;
        }
    }

    void FireAllBullets()
    {
        foreach (Transform firePoint in firePoints)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }

        // Reproduce el sonido de disparo
        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }

    void Boost(bool state)
    {
        if (state)
        {
            trail.Play();
            circle.Play();
        }
        else
        {
            trail.Stop();
            circle.Stop();
        }
        trail.GetComponent<TrailRenderer>().emitting = state;
        activeForwardSpeed = activeForwardSpeed * boostSpeed;
    }

    public void TakeDamage(float damage, Color color)
    {
        Debug.Log("Danio recibido");
        playerHealth -= damage;
        healthBarController.TakeDamage(damage);
        Debug.Log("Jugador dañado. Vida restante: " + playerHealth);

        if (playerRenderer != null)
        {
            playerRenderer.materials[1].SetColor("_Color", color);
            StartCoroutine(ResetColor());
        }

        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (playerHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.5f);

        if (playerRenderer != null && originalMaterial != null)
        {
            Material[] materials = playerRenderer.materials;
            materials[1] = originalMaterial;
            playerRenderer.materials = materials;
        }
    }

    void Die()
    {
        Debug.Log("Jugador muerto");
        Destroy(gameObject);

        if (backgroundMusic != null)
        {
            // Detener la música de fondo
            backgroundMusic.Stop();
        }

        if (audioSource != null && gameOverSound != null)
        {
            // Reproducir el sonido de "Game Over" en el AudioSource para efectos de sonido
            backgroundMusic.PlayOneShot(gameOverSound);
        }

        GameObject.FindObjectOfType<GameOverEffect>().TriggerGameOver();
    }
}

