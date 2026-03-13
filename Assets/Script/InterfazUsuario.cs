using UnityEngine;
using TMPro;

public class InterfazUsuario : MonoBehaviour 
{
    public FirebaseManager fbManager;
    public TMP_InputField emailInput;
    public TMP_InputField passInput;

    // ARRASTRA TU CANVAS AQUÍ EN EL INSPECTOR
    public GameObject canvasDeLogin; 

    public void EjecutarBoton() {
        fbManager.LoginORegistro(emailInput.text, passInput.text);
    }

    // Función que apaga el menú
    public void DesactivarMenu() {
        if (canvasDeLogin != null) {
            canvasDeLogin.SetActive(false);
        }
    }
}