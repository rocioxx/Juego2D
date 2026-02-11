using UnityEngine;
using TMPro;

public class FruitManager : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    private int totalFruits;
    public int collectedFruits = 0;

    void Start()
    {
        // Buscamos todas las frutas
        FruitCollected[] todasLasFrutas = Object.FindObjectsByType<FruitCollected>(FindObjectsSortMode.None);

        totalFruits = todasLasFrutas.Length;
        UpdateUI();

        // --- CÓDIGO DE DETECTIVE ---
        Debug.Log("🔍 HE ENCONTRADO " + totalFruits + " OBJETOS CON SCRIPT DE FRUTA:");

        foreach (FruitCollected fruta in todasLasFrutas)
        {
            // Esto escribirá el nombre del objeto en la consola
            Debug.Log(" -> " + fruta.gameObject.name);

            // PISTA EXTRA: Nos dice si es hijo de alguien (para encontrarlo mejor)
            if (fruta.transform.parent != null)
            {
                Debug.Log("      (Está dentro de: " + fruta.transform.parent.name + ")");
            }
        }
        Debug.Log("-----------------------------------");
    }
    public void AddFruit()
    {
        // 1. Sumamos al contador local (el de la pantalla de juego)
        collectedFruits++;
        UpdateUI();

        // 2. --- NUEVO: GUARDAR EN LA MEMORIA GLOBAL ---
        // Recuperamos cuántas llevábamos guardadas de antes
        int totalGlobal = PlayerPrefs.GetInt("TotalFrutasGuardadas", 0);

        // Le sumamos la nueva
        totalGlobal = totalGlobal + 1;

        // Guardamos el nuevo total en la memoria
        PlayerPrefs.SetInt("TotalFrutasGuardadas", totalGlobal);
        PlayerPrefs.Save();
        // ----------------------------------------------

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
