using UnityEngine;
using TMPro; // Necesario para los textos

public class MostrarPuntuacionFinal : MonoBehaviour
{
    public TextMeshProUGUI textoFinal; // Aquí arrastraremos el texto

    void Start()
    {
        // 1. Leemos la memoria global
        int total = PlayerPrefs.GetInt("TotalFrutasGuardadas", 0);

        // 2. Lo mostramos en el texto
        if (textoFinal != null)
        {
            textoFinal.text = "TOTAL FRUTAS: " + total;
        }
    }
}