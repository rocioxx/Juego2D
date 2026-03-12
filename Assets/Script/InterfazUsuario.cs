using UnityEngine;
using TMPro;

public class InterfazUsuario : MonoBehaviour // <--- ESTO ES LO MÁS IMPORTANTE
{
    public FirebaseManager fbManager;
    public TMP_InputField emailInput;
    public TMP_InputField passInput;

    public void EjecutarBoton() {
        fbManager.LoginORegistro(emailInput.text, passInput.text);
    }
}