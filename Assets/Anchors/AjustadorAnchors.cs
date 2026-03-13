using UnityEngine;
using UnityEditor;

public class AjustadorAnchors : MonoBehaviour
{
    // Esto crea el menú desplegable arriba en Unity
    [MenuItem("Herramientas UI/Ajustar Anchors a los Vértices %u")]
    public static void Ajustar()
    {
        // Cogemos todos los objetos que tengas seleccionados en la jerarquía
        GameObject[] selecciones = Selection.gameObjects;

        foreach (GameObject go in selecciones)
        {
            RectTransform t = go.GetComponent<RectTransform>();
            RectTransform p = go.transform.parent.GetComponent<RectTransform>();

            if (t == null || p == null) continue;

            // Registro para poder hacer "Undo" (Deshacer) si nos equivocamos
            Undo.RecordObject(t, "Ajustar Anchors");

            Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / p.rect.width,
                                                t.anchorMin.y + t.offsetMin.y / p.rect.height);
            Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / p.rect.width,
                                                t.anchorMax.y + t.offsetMax.y / p.rect.height);

            t.anchorMin = newAnchorsMin;
            t.anchorMax = newAnchorsMax;
            t.offsetMin = t.offsetMax = new Vector2(0, 0);
        }
    }
}