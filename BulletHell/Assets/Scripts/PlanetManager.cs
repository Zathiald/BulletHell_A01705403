using System.Collections;
using UnityEngine;
using Cinemachine;

public class PlanetManager : MonoBehaviour
{
    public GameObject canvasButton;
    
    public GameObject canvasVisit;

    public GameObject mercuryCanvas;

    public GameObject venusCanvas;

    public GameObject earthCanvas;

    public GameObject moonCanvas;

    public GameObject marsCanvas;

    public GameObject jupiterCanvas;

    public GameObject saturnCanvas;

    public GameObject uranusCanvas;

    public GameObject neptuneCanvas;

    public GameObject plutoCanvas;

    public ShipController playerMovement;

    public float rotationSpeed = 5f;
    public CinemachineVirtualCamera playerCamera;  // Asegúrate de asignar la cámara del jugador en el inspector
    public Transform playerFocusPoint; // Punto focal del jugador
    public float planetCameraDistance = 5f; // Distancia deseada de la cámara al planeta
    public float playerCameraFOV = 60f;

    private float originalFOV = 60f;

    private Transform nearestPlanet; // Almacena la referencia al planeta más cercano
    private bool visitCanvasActivated = false;

    void Start()
    {
        canvasVisit.SetActive(false);
        mercuryCanvas.SetActive(false);
        venusCanvas.SetActive(false);
        earthCanvas.SetActive(false);
        moonCanvas.SetActive(false);
        marsCanvas.SetActive(false);
        jupiterCanvas.SetActive(false);
        saturnCanvas.SetActive(false);
        uranusCanvas.SetActive(false);
        neptuneCanvas.SetActive(false);
        plutoCanvas.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer==LayerMask.NameToLayer("Planet"))
        {
            nearestPlanet = other.transform;
            Debug.Log("Player entered planet area");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer==LayerMask.NameToLayer("Planet"))
        {
            nearestPlanet = null; // Cuando salimos del área, eliminamos la referencia al planeta más cercano
            Debug.Log("Player exited planet area");
        }
    }

    IEnumerator SwitchToPlanetCamera()
    {   
        // Desactivar canvas del botón y activar canvas de visita
        canvasButton.SetActive(false);
        canvasVisit.SetActive(true);
        visitCanvasActivated = true;

        // Cambiar el objetivo de seguimiento de la cámara de Cinemachine al planeta más cercano
        playerCamera.LookAt = nearestPlanet;

        // Ajustar la distancia de la cámara al planeta
        playerCamera.m_Lens.FieldOfView = playerCameraFOV;

        playerMovement.canMove = false;

        if(nearestPlanet.CompareTag("Mercury")) {
            mercuryCanvas.SetActive(true);
        }

        if(nearestPlanet.CompareTag("Venus")) {
            venusCanvas.SetActive(true);
        }

        if(nearestPlanet.CompareTag("Earth")) {
            earthCanvas.SetActive(true);
        }

        if(nearestPlanet.CompareTag("Moon")) {
            moonCanvas.SetActive(true);
        }

        if(nearestPlanet.CompareTag("Mars")) {
            marsCanvas.SetActive(true);
        }

        if(nearestPlanet.CompareTag("Jupiter")) {
            jupiterCanvas.SetActive(true);
        }

        if(nearestPlanet.CompareTag("Saturn")) {
            saturnCanvas.SetActive(true);
        }

        if(nearestPlanet.CompareTag("Uranus")) {
            uranusCanvas.SetActive(true);
        }

        if(nearestPlanet.CompareTag("Neptune")) {
            neptuneCanvas.SetActive(true);
        }

        if(nearestPlanet.CompareTag("Pluto")) {
            plutoCanvas.SetActive(true);
        }

        // Esperar un frame para que Cinemachine pueda realizar la transición
        yield return null;
    }

    IEnumerator SwitchToPlayerCamera()
    {   
        // Desactivar canvas de visita
        canvasVisit.SetActive(false);
        visitCanvasActivated = false;
        mercuryCanvas.SetActive(false);
        venusCanvas.SetActive(false);
        earthCanvas.SetActive(false);
        moonCanvas.SetActive(false);
        marsCanvas.SetActive(false);
        jupiterCanvas.SetActive(false);
        saturnCanvas.SetActive(false);
        uranusCanvas.SetActive(false);
        neptuneCanvas.SetActive(false);
        plutoCanvas.SetActive(false);

        // Restaurar el objetivo de seguimiento de la cámara de Cinemachine al jugador
        playerCamera.LookAt = playerFocusPoint;
        playerCamera.Follow = playerFocusPoint;

        // Restaurar la distancia de la cámara al jugador
        playerCamera.m_Lens.OrthographicSize = playerCamera.m_Lens.OrthographicSize;
        playerCamera.m_Lens.FieldOfView = originalFOV;

        playerMovement.canMove = true;

        // Esperar un frame para que Cinemachine pueda realizar la transición
        yield return null;
    }

    void Update()
    {
        if (nearestPlanet != null && !visitCanvasActivated)
        {
            canvasButton.SetActive(true);
            Debug.Log("Canvas activated");
            if (Input.GetKeyDown(KeyCode.V))
            {
                StartCoroutine(SwitchToPlanetCamera());
            }
        }
        if (nearestPlanet == null && visitCanvasActivated)
        {
            StartCoroutine(SwitchToPlayerCamera());
            Debug.Log("Visit Canvas deactivated");
        }
        else if (nearestPlanet == null && !visitCanvasActivated && canvasButton.activeSelf)
        {
            canvasButton.SetActive(false);
            Debug.Log("Button Canvas deactivated");
        }

        // Añadir lógica para seguir al jugador cuando se presiona la tecla 'X'
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(SwitchToPlayerCamera());
            Debug.Log("Switching back to Player Camera");
        }
    }
}
