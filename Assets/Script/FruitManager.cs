using UnityEngine;
using TMPro; // ¡Importante! Para que reconozca el texto

public class FruitManager : MonoBehaviour
{
    public TextMeshProUGUI counterText; // Aquí arrastraremos el texto luego
    private int totalFruits;
    public int collectedFruits = 0;

    void Start()
    {
        // Esto busca TODOS los componentes FruitCollected que haya dentro, 
        // sin importar si están en carpetas o subcarpetas.
        totalFruits = GetComponentsInChildren<FruitCollected>().Length;
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
        counterText.text = "Frutas: " + collectedFruits + " / " + totalFruits;
    }
}