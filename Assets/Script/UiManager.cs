using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class UIManager : MonoBehaviour
{
    // Arrastra aquí tu panel de opciones (el que tiene fondo negro)
    public GameObject optionsPanel;

    // Función para el botón del engranaje (Pausa el juego y abre menú)
    public void OptionsPanel()
    {
        Time.timeScale = 0; // Detiene el tiempo del juego
        optionsPanel.SetActive(true); // Muestra el panel
    }

    // Función para el botón "Reanudar" o "Volver" (Cierra menú y sigue jugando)
    public void Return()
    {
        Time.timeScale = 1; // Vuelve el tiempo a la normalidad
        optionsPanel.SetActive(false); // Oculta el panel
    }

    // Función para futuras opciones de sonido/gráficos
    public void AnotherOptions()
    {
        // Sound
        // Graphics
    }

    // Función para el botón "Menú Principal" o "Salir"
    public void GoMainMenu()
    {
        // IMPORTANTE: Asegúrate de que tu escena del menú se llame "MainMenu"
        // Si se llama diferente (ej: "MenuPrincipal"), cambia el texto de abajo.
        SceneManager.LoadScene("MainMenu");
    }
}