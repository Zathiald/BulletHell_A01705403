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
    private float boostTime = 1f; // Agrega esta línea
    private bool isBoosting = false; // Agrega esta línea
    public bool canMove = true;


    // Start is called before the first frame update
    void Start()
    {   
        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        if(!canMove){
            return;
        }
        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;

        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance,1f);

        rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Roll"),rollAcceleration * Time.deltaTime);

        transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollInput* rollSpeed * Time.deltaTime,Space.Self);

        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed,Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAcceleration * Time.deltaTime);
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed,Input.GetAxisRaw("Horizontal") * strafeSpeed, strafeAcceleration * Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed,Input.GetAxisRaw("Hover") * hoverSpeed, hoverAcceleration * Time.deltaTime);

        transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
        transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed * Time.deltaTime);

        if(Input.GetButtonDown("Action"))
        {
           Boost(true);
        }

        if(Input.GetButtonUp("Action"))
        {
            Boost(false);
        }
    }
    void Boost(bool state)
    {
        if(state)
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
}
