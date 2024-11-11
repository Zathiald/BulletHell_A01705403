using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    Transform trans;
    Vector3 randomRotation;

    [SerializeField] float minScale = .8f;
    [SerializeField] float maxScale = 5f;
    [SerializeField] float minRotation = -100f;
    [SerializeField] float maxRotation = 100f;

    public float moveSpeed = 10f;
    void Awake() 
    {
        trans = transform;
    }
    // Start is called before the first frame update
    void Start()
    {

        trans.localScale = Vector3.one * Random.Range(minScale,maxScale);

        randomRotation.x = Random.Range(minRotation,maxRotation);
        randomRotation.y = Random.Range(minRotation,maxRotation);
        randomRotation.z = Random.Range(minRotation,maxRotation);
    }

    // Update is called once per frame
    void Update()
    {
        trans.Rotate(randomRotation * Time.deltaTime);

        trans.position += trans.forward * moveSpeed* Time.deltaTime;
        trans.position += trans.right * moveSpeed* Time.deltaTime + trans.up * moveSpeed* Time.deltaTime;
    }
}
