using UnityEngine;
using TMPro;

public class FruitManager : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    private int totalFruits;
    public int collectedFruits = 0;

    void Start()
    {
        // === CAMBIO CLAVE AQUÍ ===
        // Usamos FindObjectsInactive.Include para obligar a Unity a contar 
        // también las frutas que están desactivadas dentro de las cajas.
        FruitCollected[] todasLasFrutas = Object.FindObjectsByType<FruitCollected>(
            FindObjectsInactive.Include, 
            FindObjectsSortMode.None
        );

        totalFruits = todasLasFrutas.Length;
        UpdateUI();

        // --- CÓDIGO DE DETECTIVE ---
        Debug.Log("🔍 HE ENCONTRADO " + totalFruits + " OBJETOS CON SCRIPT DE FRUTA (INCLUYENDO ESCONDIDAS):");

        foreach (FruitCollected fruta in todasLasFrutas)
        {
            Debug.Log(" -> " + fruta.gameObject.name);

            if (fruta.transform.parent != null)
            {
                Debug.Log("      (Está dentro de: " + fruta.transform.parent.name + ")");
            }
        }
        Debug.Log("-----------------------------------");
    }

    public void AddFruit()
    {
        // 1. Sumamos al contador local
        collectedFruits++;
        UpdateUI();

        // 2. GUARDAR EN LA MEMORIA GLOBAL
        int totalGlobal = PlayerPrefs.GetInt("TotalFrutasGuardadas", 0);
        totalGlobal = totalGlobal + 1;
        PlayerPrefs.SetInt("TotalFrutasGuardadas", totalGlobal);
        PlayerPrefs.Save();

        // 3. Comprobar si acabamos el nivel
        if (collectedFruits >= totalFruits)
        {
            Debug.Log("¡Todas las frutas recogidas!");
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