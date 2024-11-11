using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField] int gridSpacing = 100;
    [SerializeField] int numberOfAsteroidsOnAxis = 10;
    [SerializeField] GameObject asteroid;

    void Start()
    {
        PlaceAsteroids();
    }


    void PlaceAsteroids()
    {
        for(int x=0; x<numberOfAsteroidsOnAxis; x++)
        {
            for(int y=0; y<numberOfAsteroidsOnAxis; y++)
            {
                for(int z=0; z<numberOfAsteroidsOnAxis;z++)
                {
                    InstantiateAsteroid(x,y,z);
                }
            }
        }
    }
    void InstantiateAsteroid(int x, int y, int z)
    {   
        Instantiate(asteroid,new Vector3(
            transform.position.x + (x*gridSpacing) + AsteroidOffSet(),
            transform.position.y+ (y*gridSpacing)+ AsteroidOffSet(), 
            transform.position.z + (z*gridSpacing)+ AsteroidOffSet()),
            Quaternion.identity,transform);
    }

    float AsteroidOffSet()
    {
        return Random.Range(-gridSpacing/2f,gridSpacing/2f);
    }
}
