using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class MenuGameOver : MonoBehaviour
{
    // Esta función la llamará el botón de la flecha
    public void ReiniciarNivel()
    {
        // 1. Es MUY IMPORTANTE "descongelar" el tiempo antes de cambiar
        Time.timeScale = 1f;

        // 2. Cargamos la escena.
        // OPCIÓN A: Si quieres reiniciar el nivel en el que estás ahora mismo:
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // OPCIÓN B: Si quieres ir a una escena concreta llamada "Nivel1":
        // SceneManager.LoadScene("Nivel1"); 
    }

    // Esta función la llamará el botón de la puerta (Salir)
    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal"); // Asegúrate de poner el nombre real de tu menú
    }
}