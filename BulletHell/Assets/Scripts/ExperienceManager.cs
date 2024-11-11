using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceManager : MonoBehaviour
{
    [Header("Experience")]
    [SerializeField] AnimationCurve experienceCurve;

    int currentLevel, totalExperience;
    int previousLevelsExperience, nextLevelsExperience;

    [Header("Interface")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI experienceText;
    [SerializeField] Image experienceFill;

    public Collider playerCollider;
    public List<Collider> checkpoints;
    HashSet<Collider> visitedCheckpoints = new HashSet<Collider>();

    void Start()
    {
        UpdateLevel();
    }

    void Update()
    {
        // Define el radio y la posición de tu esfera de detección (puedes ajustar estos valores según sea necesario)
        float detectionRadius = 1.0f;
        Vector3 detectionPosition = playerCollider.transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(detectionPosition, detectionRadius, LayerMask.GetMask("Asteroid_Ring"));
        foreach (var hitCollider in hitColliders)
        {
            if (checkpoints.Contains(hitCollider) && !visitedCheckpoints.Contains(hitCollider))
            {
                Debug.Log("Punto de control alcanzado");
                visitedCheckpoints.Add(hitCollider);

                // Si el jugador ha pasado por todos los puntos de control
                if (visitedCheckpoints.Count == checkpoints.Count)
                {
                    AddExperience(5);
                    visitedCheckpoints.Clear(); // Reiniciar los puntos de control
                }
            }
        }
    }

    public void AddExperience(int amount)
    {
        totalExperience += amount;
        CheckForLevelUp();
        UpdateInterface();
    }

    void CheckForLevelUp()
    {
        if(totalExperience >= nextLevelsExperience)
        {
            currentLevel++;
            UpdateLevel();
        }
    }

    void UpdateLevel()
    {
        previousLevelsExperience = (int)experienceCurve.Evaluate(currentLevel);
        nextLevelsExperience = (int)experienceCurve.Evaluate(currentLevel + 1);
        UpdateInterface();
    }

    void UpdateInterface()
    {
        int start = totalExperience - previousLevelsExperience;
        int end = nextLevelsExperience - previousLevelsExperience; 

        levelText.text = currentLevel.ToString();
        experienceText.text = start + " exp / " + end + " exp";
        experienceFill.fillAmount = (float)start / (float)end;
    }
}
