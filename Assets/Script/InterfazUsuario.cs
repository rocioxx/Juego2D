using UnityEngine;
using TMPro;

public class InterfazUsuario : MonoBehaviour
{
    public FirebaseManager fbManager;
    public TMPro.TMP_InputField emailInput;
    public TMPro.TMP_InputField passwordInput;

    // Ya no necesitas los inputs si solo usas Google, pero puedes dejarlos
    public GameObject canvasDeLogin;

    public void EjecutarBotonGoogle()
    {
        // Llamamos a la nueva función de Google
        fbManager.LoginConGoogle();
    }

    public void DesactivarMenu()
    {
        if (canvasDeLogin != null)
        {
            canvasDeLogin.SetActive(false);
        }
    }
}