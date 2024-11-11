using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offSet;   
    private Vector3 currentVelocity = Vector3.zero; 
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime;

    private void Awake() 
    {
        offSet = transform.position - target.position;
    }

    private void LateUpdate() 
    {
        Vector3 targetPosition = target.position + offSet;
        transform.position = Vector3.SmoothDamp(transform.position,targetPosition, ref currentVelocity, smoothTime);
    }
}
