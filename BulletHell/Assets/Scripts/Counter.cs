using UnityEngine;
using TMPro;

public class Counter : MonoBehaviour
{
    public GameObject prefabToCount;
    public TextMeshProUGUI countText;
    public string customMessage = "Cantidad en pantalla: {0}";  // Mensaje personalizado

    void Update()
    {
        int count = CountInstancesOfPrefab(prefabToCount);
        UpdateText(count);
    }

    // Método para actualizar el texto del TextMeshProUGUI con formato personalizado
    void UpdateText(int count)
    {
        countText.text = string.Format(customMessage, count);  // Usa string.Format para el mensaje
    }

    int CountInstancesOfPrefab(GameObject prefab)
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == prefab.name + "(Clone)")
            {
                count++;
            }
        }
        return count;
    }
}

