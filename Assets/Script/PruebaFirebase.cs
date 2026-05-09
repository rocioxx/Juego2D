using UnityEngine;
using Firebase.Analytics;

public partial class PruebaFirebase : MonoBehaviour
{
    void Start()
    {
        // Esto inicializa Firebase y envía un evento de "juego_abierto"
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAppOpen);
            Debug.Log("¡Firebase dice: Conexión establecida!");
        });
    }
}
