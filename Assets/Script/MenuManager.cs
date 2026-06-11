using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;

public class MenuManager : MonoBehaviour
{
    public void Jugar()
    {
        // 1. SI ESTÁS EN EL EDITOR DE UNITY (PC), TE DEJAMOS PASAR PARA PROBAR
#if UNITY_EDITOR
        Debug.Log("🎮 Modo Editor detectado: Saltando bloqueo de Firebase para pruebas de desarrollo.");
        CargarNivel1();
        return;
#endif

        // 2. LOGICA REAL PARA EL JUEGO EN ANDROID / DISPOSITIVOS
        if (FirebaseAuth.DefaultInstance != null && FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            Debug.Log("✅ Usuario detectado en Firebase. Cargando el juego...");
            CargarNivel1();
        }
        else
        {
            Debug.LogWarning("⛔ ¡Bloqueado! Debes iniciar sesión con Google antes de poder jugar.");
        }
    }

    // He creado esta función cortita para no repetir código
    private void CargarNivel1()
    {
        PlayerPrefs.DeleteKey("TotalFrutasGuardadas");
        Time.timeScale = 1f; // Aseguramos que el tiempo corre normal
        SceneManager.LoadScene("Nivel1");
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }
}