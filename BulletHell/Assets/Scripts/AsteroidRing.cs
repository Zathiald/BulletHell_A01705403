using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidRing : MonoBehaviour
{

    public GameObject[] stars;
    int entranceCheck = 0;
    void Start() 
    {
        foreach(GameObject star in stars)
        {
            star.SetActive(false);
        }
    }
    
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            entranceCheck++;
        
            foreach(GameObject star in stars)
            {
                star.SetActive(true);
            }

            if(entranceCheck==2)
            {
                foreach(GameObject star in stars)
                {
                    star.SetActive(false);
                }
                entranceCheck=0;
            }
        }
    }
}
