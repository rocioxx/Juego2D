using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class MenuManager : MonoBehaviour
{
    // Esta función la llamará tu botón verde
    public void Jugar()
    {
        // Asegúrate de que "Nivel1" es el nombre EXACTO de tu escena de juego
        SceneManager.LoadScene("Nivel1");
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }
}