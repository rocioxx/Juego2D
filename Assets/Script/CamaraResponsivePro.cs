using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CamaraResponsivePro : MonoBehaviour
{
    private Camera cam;

    [Header("Resolución de Diseño")]
    public float anchoDiseno = 16f;
    public float altoDiseno = 9f;

    [Header("Zoom de la Cámara")]
    public float sizeOriginal = 1.6f;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        AjustarCamara();
    }

    void LateUpdate()
    {
        AjustarCamara();
    }

    void AjustarCamara()
    {
        if (cam == null) return;

        float relacionAspectoActual = (float)Screen.width / Screen.height;
        float relacionAspectoDiseno = anchoDiseno / altoDiseno;

        // NUEVA LÓGICA: En lugar de alejar la cámara y crear bandas,
        // mantenemos el tamaño fijo o hacemos que encaje de forma que llene la pantalla.
        if (relacionAspectoActual < relacionAspectoDiseno)
        {
            // Mantenemos el zoom original para que el suelo y el techo tapen el fondo amarillo
            cam.orthographicSize = sizeOriginal;
        }
        else
        {
            // Si la pantalla es extremadamente ancha, ajustamos sutilmente
            float diferencia = relacionAspectoDiseno / relacionAspectoActual;
            cam.orthographicSize = sizeOriginal;
        }
    }
}
