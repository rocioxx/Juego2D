using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;

public class FirebaseManager : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseFirestore db;
    FirebaseUser usuarioActual;

    // REFERENCIA A LA INTERFAZ
    private InterfazUsuario interfaz;

    void Start()
    {
        // Buscamos el componente de interfaz en el mismo objeto
        interfaz = GetComponent<InterfazUsuario>();

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(tarea => {
            auth = FirebaseAuth.DefaultInstance;
            db = FirebaseFirestore.DefaultInstance;
            Debug.Log("Firebase preparado");
        });
    }

    public void LoginORegistro(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(tarea => {
            if (tarea.IsCompleted && !tarea.IsFaulted) {
                usuarioActual = tarea.Result.User;
                Debug.Log("Nuevo usuario registrado: " + usuarioActual.Email);
                
                // ES NUEVO: Creamos datos iniciales y cerramos menú
                GuardarDatosJugador(1, 0, "avatar_01");
                interfaz.DesactivarMenu();
            } else {
                // YA EXISTE: Intentamos loguear y recuperar sus datos
                LoguearYRecuperar(email, password);
            }
        });
    }

    void LoguearYRecuperar(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(tarea => {
            if (tarea.IsCompleted && !tarea.IsFaulted) {
                usuarioActual = tarea.Result.User;
                Debug.Log("Bienvenido de nuevo: " + usuarioActual.Email);
                
                // CARGAMOS sus datos en lugar de borrarlos
                CargarDatosJugador();
                
                // OCULTAR MENÚ AL ENTRAR
                interfaz.DesactivarMenu();
            } else {
                Debug.LogError("Error al loguear (posible contraseña mal): " + tarea.Exception);
            }
        });
    }

    public void CargarDatosJugador()
    {
        if (usuarioActual == null) return;

        // Vamos a la nube a buscar el documento del usuario
        db.Collection("Usuarios").Document(usuarioActual.UserId).GetSnapshotAsync().ContinueWithOnMainThread(tarea => {
            if (tarea.IsCompleted && !tarea.IsFaulted) {
                DocumentSnapshot snapshot = tarea.Result;
                
                if (snapshot.Exists) {
                    // Extraemos los datos guardados
                    int nivel = System.Convert.ToInt32(snapshot.GetValue<int>("Nivel"));
                    int monedas = System.Convert.ToInt32(snapshot.GetValue<int>("Monedas"));
                    string avatar = snapshot.GetValue<string>("AvatarID");

                    Debug.Log("DATOS CARGADOS: Nivel " + nivel + ", Monedas " + monedas);
                    
                    // Aquí es donde dirías a tu juego: Jugador.nivel = nivel;
                }
            } else {
                Debug.LogWarning("El usuario no tiene datos previos guardados.");
            }
        });
    }

    public void GuardarDatosJugador(int nivel, int monedas, string idAvatar)
    {
        if (usuarioActual == null) return;

        Dictionary<string, object> datos = new Dictionary<string, object>
        {
            { "Nivel", nivel },
            { "Monedas", monedas },
            { "AvatarID", idAvatar }
        };

        db.Collection("Usuarios").Document(usuarioActual.UserId).SetAsync(datos).ContinueWithOnMainThread(tarea => {
            if (tarea.IsCompleted) Debug.Log("Datos guardados en la nube");
        });
    }
}