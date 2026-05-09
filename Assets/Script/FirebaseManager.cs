using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;
using Google;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Diagnostics;

public class FirebaseManager : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseFirestore db;
    FirebaseUser usuarioActual;
    string redirectUri = "http://localhost:51772/";

    [Header("Configuración Google")]
    public string webClientId = "548132711242-2tsnj7somntaq5c8hv19d5r41b7jaig4.apps.googleusercontent.com";

    private InterfazUsuario interfaz;

    void Start()
    {
        interfaz = GetComponent<InterfazUsuario>();

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(tarea => {
            auth = FirebaseAuth.DefaultInstance;
            db = FirebaseFirestore.DefaultInstance;
            UnityEngine.Debug.Log("Firebase preparado");
        });
    }

    public void LoginConGoogle()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        UnityEngine.Debug.Log("Iniciando Login en Windows...");
        _ = LoginWindows(); // Ejecutamos la tarea asíncrona
#elif UNITY_ANDROID
            UnityEngine.Debug.Log("Iniciando Google Sign-In en Android...");
            IniciarLoginAndroid();
#endif
    }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    private async Task LoginWindows()
    {
        // 1. Configuración del "Oído" (Servidor Local)
        string redirectUri = "http://localhost:51772/";
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(redirectUri);
        listener.Start();

        // 2. Construcción de la URL y apertura del navegador
        string authEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        string scope = "email%20profile";
        string authRequest = $"{authEndpoint}?response_type=code&scope={scope}&redirect_uri={redirectUri}&client_id={webClientId}";

        Application.OpenURL(authRequest);
        UnityEngine.Debug.Log("Esperando respuesta en el navegador...");

        // 3. Espera asíncrona del código de Google
        HttpListenerContext contexto = await listener.GetContextAsync();
        string codigo = contexto.Request.QueryString.Get("code");

        // 4. Respuesta amigable al usuario en el navegador
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes("<html><body>¡Autenticación completada! Puedes volver a Mr. Rana.</body></html>");
        contexto.Response.ContentLength64 = buffer.Length;
        contexto.Response.OutputStream.Write(buffer, 0, buffer.Length);
        contexto.Response.Close();
        listener.Stop();

        if (!string.IsNullOrEmpty(codigo))
        {
            UnityEngine.Debug.Log("Código recibido: " + codigo);
            // El siguiente paso lógico será intercambiar este 'codigo' por un Token
            // ¿Quieres que veamos cómo hacer ese intercambio con una petición web?
        }
    }
#endif

    private void IniciarLoginAndroid()
    {
#if UNITY_ANDROID
        GoogleSignInConfiguration configuration = new GoogleSignInConfiguration {
            WebClientId = webClientId,
            RequestIdToken = true
        };
        GoogleSignIn.Configuration = configuration;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread<GoogleSignInUser>(tarea => {
            if (tarea.IsFaulted) {
                UnityEngine.Debug.LogError("Error en Google Sign-In Android: " + tarea.Exception);
            } else if (tarea.IsCompleted) {
                EntrarEnFirebaseConGoogle(tarea.Result.IdToken);
            }
        });
#endif
    }

    void EntrarEnFirebaseConGoogle(string idToken)
    {
        Credential credencial = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credencial).ContinueWithOnMainThread(tarea => {
            if (tarea.IsFaulted || tarea.IsCanceled)
            {
                UnityEngine.Debug.LogError("Error al conectar con Firebase: " + tarea.Exception);
                return;
            }

            if (tarea.IsCompleted)
            {
                usuarioActual = auth.CurrentUser;
                UnityEngine.Debug.Log("✅ Login Exitoso: " + usuarioActual.Email);
                CargarDatosJugador();
                if (interfaz != null) interfaz.DesactivarMenu();
            }
        });
    }

    // --- FUNCIONES DE FIRESTORE ---
    public void CargarDatosJugador()
    {
        if (usuarioActual == null) return;
        db.Collection("Usuarios").Document(usuarioActual.UserId).GetSnapshotAsync().ContinueWithOnMainThread(tarea => {
            if (tarea.IsCompleted && !tarea.IsFaulted)
            {
                DocumentSnapshot snapshot = tarea.Result;
                if (snapshot.Exists)
                {
                    int nivel = System.Convert.ToInt32(snapshot.GetValue<int>("Nivel"));
                    UnityEngine.Debug.Log("Nivel recuperado: " + nivel);
                }
                else
                {
                    GuardarDatosJugador(1, 0, "avatar_01");
                }
            }
        });
    }

    public void GuardarDatosJugador(int nivel, int monedas, string idAvatar)
    {
        if (usuarioActual == null) return;
        Dictionary<string, object> datos = new Dictionary<string, object> {
            { "Nivel", nivel }, { "Monedas", monedas }, { "AvatarID", idAvatar }
        };
        db.Collection("Usuarios").Document(usuarioActual.UserId).SetAsync(datos);
    }
}