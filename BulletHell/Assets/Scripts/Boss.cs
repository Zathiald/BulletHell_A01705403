using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour, IDamage
{
    public float rotationSpeed = 100f;
    public float rotationDuration = 5f;
    public float decelerationTime = 1f;
    public float tiltAngle = -10f;
    public float tiltSpeed = 2f;
    public float blinkDuration = 3f;
    public Color blinkColor = Color.red;
    public float blinkInterval = 1f;
    private Color originalColor;
    public Renderer objectRenderer;
    private float currentRotationSpeed;
    private bool isAnimating = false;
    private bool isRotating = false;
    public float health = 50f;
    private float maxHealth = 50f;
    public AudioClip hitSound;
    private AudioSource audioSource;

    [Header("Firepoints and Prefabs")]
    public Transform[] firePoints1;
    public Transform[] firePoints2;
    public Transform[] firePoints3;
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;

    public float fireIntervalPrefab1 = 2f; // Intervalo para prefab1
    public float fireFramePrefab2And3 = 0.5f; // Intervalo entre disparos de prefab2 y prefab3

    void Start()
    {
        isAnimating = false;
        isRotating = false;
        maxHealth = health;

        if (objectRenderer != null)
            originalColor = objectRenderer.material.color;

        currentRotationSpeed = rotationSpeed;
        StartCoroutine(SpinAndAnimate());
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = hitSound;
        audioSource.playOnAwake = false;
    }

    private IEnumerator SpinAndAnimate()
    {
        while (true)
        {
            yield return SpinObject();
            yield return TiltObject();
            yield return BlinkAndReset();
        }
    }

    private IEnumerator SpinObject()
    {
        isAnimating = true;
        isRotating = true;
        StartCoroutine(FireContinuously(firePoints1, prefab1, fireIntervalPrefab1)); // Prefab1
        StartCoroutine(ManageFirePrefab2And3()); // Prefab2 y Prefab3
        float timer = 0f;

        while (timer < rotationDuration)
        {
            timer += Time.deltaTime;

            if (timer >= rotationDuration - decelerationTime)
                currentRotationSpeed = Mathf.Lerp(rotationSpeed, 0, (timer - (rotationDuration - decelerationTime)) / decelerationTime);

            transform.Rotate(Vector3.up, currentRotationSpeed * Time.deltaTime);
            yield return null;
        }

        currentRotationSpeed = rotationSpeed;
        isAnimating = false;
    }

    private IEnumerator TiltObject()
    {
        isAnimating = true;
        isRotating = false;
        Quaternion targetRotation = Quaternion.Euler(tiltAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
            yield return null;
        }

        isAnimating = false;
    }

    private IEnumerator BlinkAndReset()
    {
        isAnimating = true;
        isRotating = false;
        float blinkTimer = 0f;
        bool isVisible = true;

        while (blinkTimer < blinkDuration)
        {
            blinkTimer += blinkInterval;
            isVisible = !isVisible;

            if (objectRenderer != null)
                objectRenderer.material.color = isVisible ? blinkColor : originalColor;

            yield return new WaitForSeconds(blinkInterval);
        }

        if (objectRenderer != null)
            objectRenderer.material.color = originalColor;

        yield return ReturnToDefaultPosition();
        isAnimating = false;
    }

    private IEnumerator ReturnToDefaultPosition()
    {
        Quaternion defaultRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        while (Quaternion.Angle(transform.rotation, defaultRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultRotation, tiltSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Enemigo Boss dañado. Vida restante: " + health);

        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator FireContinuously(Transform[] firePoints, GameObject prefab, float interval)
    {
        while (true)
        {
            if (isRotating)
            {
                yield return new WaitForSeconds(interval);
                FireFromPoints(firePoints, prefab);
            }
            else
            {
                yield return null; // Espera a que el objeto esté girando
            }
        }
    }

    private IEnumerator ManageFirePrefab2And3()
    {
        bool isFiringPrefab2 = false;
        bool isFiringPrefab3 = false;

        while (true)
        {
            if (health <= 0.75f * maxHealth && !isFiringPrefab2)
            {
                isFiringPrefab2 = true;
                StartCoroutine(FireContinuously(firePoints2, prefab2, fireFramePrefab2And3));
            }

            if (health <= 0.5f * maxHealth && !isFiringPrefab3)
            {
                isFiringPrefab3 = true;
                StartCoroutine(FireContinuously(firePoints3, prefab3, fireFramePrefab2And3));
            }

            yield return null;
        }
    }


    private void FireFromPoints(Transform[] firePoints, GameObject prefab)
    {
        foreach (var firePoint in firePoints)
        {
            if (firePoint != null && prefab != null)
            {
                Instantiate(prefab, firePoint.position, firePoint.rotation);
            }
        }
    }

    void Die()
    {
        Debug.Log("Enemigo destruido");
        Destroy(gameObject);
    }
}
