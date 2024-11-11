using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Skybox))]
public class SkyboxSetter : MonoBehaviour
{
    [SerializeField] List<Material> _skyboxMaterials;

    Skybox skybox;

    void Awake() 
    {
        skybox = GetComponent<Skybox>();
    }

    void OnEnable() 
    {
        ChangeSkybox(0);
        
    }

    void ChangeSkybox(int skyBox)
    {
        if(skybox != null && skyBox >=0 && skyBox <= _skyboxMaterials.Count)
        {
            skybox.material = _skyboxMaterials[skyBox];
        }
    }
}
