using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;

public class ControladorFirebase : MonoBehaviour
{
    // Variables de Firebase
    FirebaseAuth auth;
    FirebaseFirestore db;
    FirebaseUser usuarioActual;

    void Start()
    {
        // Inicializar Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(tarea => {
            auth = FirebaseAuth.DefaultInstance;
            db = FirebaseFirestore.DefaultInstance;
            Debug.Log("Firebase preparado");
        });
    }

    // 1. FUNCIÓN PARA CREAR USUARIO Y LOGUEAR
    public void RegistrarYLoguear(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(tarea => {
            if (tarea.IsCompleted && !tarea.IsFaulted) {
                usuarioActual = tarea.Result.User;
                Debug.Log("Registrado: " + usuarioActual.Email);
                
                // Al registrar, creamos sus datos iniciales obligatorios
                GuardarDatosJugador(1, 0, "avatar_01");
            } else {
                // Si ya existe, intentamos solo loguear
                LoguearSolo(email, password);
            }
        });
    }

    void LoguearSolo(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(tarea => {
            if (tarea.IsCompleted && !tarea.IsFaulted) {
                usuarioActual = tarea.Result.User;
                Debug.Log("Logueado con éxito");
            }
        });
    }

    // 2. FUNCIÓN PARA GUARDAR LOS DATOS (Mundial)
    public void GuardarDatosJugador(int nivel, int monedas, string idAvatar)
    {
        if (usuarioActual == null) return;

        // Diccionario con lo que pide el ejercicio
        Dictionary<string, object> datos = new Dictionary<string, object>
        {
            { "Nivel", nivel },
            { "Monedas", monedas },
            { "AvatarID", idAvatar }
        };

        // Guardar en la colección "Usuarios" usando el ID único del login
        db.Collection("Usuarios").Document(usuarioActual.UserId).SetAsync(datos).ContinueWithOnMainThread(tarea => {
            if (tarea.IsCompleted) Debug.Log("Datos guardados en la nube");
        });
    }
}