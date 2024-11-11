using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField]float movementSpeed = 50f;
    [SerializeField]float turnSpeed = 60f;
    Transform trans;

    // Start is called before the first frame update
    void Awake()
    {
        trans = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Turn();
        Thrust();
    }

    void Turn()
    {
        float yaw = turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float hover = turnSpeed * Time.deltaTime * Input.GetAxis("Hover");
        float roll = turnSpeed * Time.deltaTime * Input.GetAxis("Roll");
        trans.Rotate(hover,yaw,roll);
    }

    void Thrust()
    {
        if(Input.GetAxis("Vertical")>0)
        {
            trans.position += trans.forward * movementSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        }
    }
}
