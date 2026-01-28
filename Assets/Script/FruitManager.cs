using UnityEngine;
using TMPro;

public class FruitManager : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    private int totalFruits;
    public int collectedFruits = 0;

    void Start()
    {
        // CAMBIO IMPORTANTE:
        // Usamos FindObjectsByType para buscar en TODA la escena, no solo en los hijos.
        // Así puedes poner las frutas donde quieras.
        FruitCollected[] todasLasFrutas = Object.FindObjectsByType<FruitCollected>(FindObjectsSortMode.None);

        totalFruits = todasLasFrutas.Length;

        // Debug para comprobar si las ha encontrado
        Debug.Log("Frutas encontradas al inicio: " + totalFruits);

        UpdateUI();
    }

    public void AddFruit()
    {
        collectedFruits++;
        UpdateUI();

        if (collectedFruits >= totalFruits)
        {
            Debug.Log("¡Has recogido todas las frutas!");
        }
    }

    void UpdateUI()
    {
        if (counterText != null)
        {
            counterText.text = "Frutas: " + collectedFruits + " / " + totalFruits;
        }
    }
}