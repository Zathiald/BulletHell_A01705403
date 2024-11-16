using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour,IDamage
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
    public float health = 50f; 
    public AudioClip hitSound;
    private AudioSource audioSource;

    void Start()
    {
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

    void Die()
    {
        Debug.Log("Enemigo destruido");
        Destroy(gameObject);
    }
}
