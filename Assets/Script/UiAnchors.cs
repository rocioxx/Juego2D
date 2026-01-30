using UnityEngine;
using UnityEditor;

public class UIAnchorAlignment
{
    // Define la opción de menú. Se añadirá directamente en el menú "Window"
    // El 'true' al final hace que la opción solo aparezca si hay un RectTransform seleccionado.
    [MenuItem("Window/UI Tools/Align Anchors to Corners", true)]
    private static bool ValidateAlignAnchors()
    {
        // Solo habilita la opción si hay un objeto seleccionado que tiene un RectTransform
        return Selection.activeGameObject != null &&
               Selection.activeGameObject.GetComponent<RectTransform>() != null &&
               Selection.activeGameObject.transform.parent != null &&
               Selection.activeGameObject.transform.parent.GetComponent<RectTransform>() != null;
    }

    // La función que se ejecuta al hacer clic en el menú
    [MenuItem("Window/UI Tools/Align Anchors to Corners", false, 50)]
    private static void AlignAnchorsToCorners()
    {
        RectTransform rt = Selection.activeGameObject.GetComponent<RectTransform>();
        RectTransform parentRT = rt.parent as RectTransform;

        // Comprobaciones de seguridad, aunque el validador [MenuItem] ya hace la mayor parte
        if (rt == null || parentRT == null)
        {
            Debug.LogError("Please select a UI element that is a child of another RectTransform.");
            return;
        }

        // Registrar la acción para poder deshacerla (Ctrl+Z)
        Undo.RecordObject(rt, "Align UI Anchors and Quad");

        // --- LÓGICA CLAVE ---

        // 1. Obtener las esquinas del RectTransform actual en coordenadas de mundo
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        // 2. Obtener las esquinas del RectTransform del padre en coordenadas de mundo
        Vector3[] parentCorners = new Vector3[4];
        parentRT.GetWorldCorners(parentCorners);

        // 3. Calcular el tamaño del padre en el mundo
        float parentWidth = parentCorners[2].x - parentCorners[0].x;
        float parentHeight = parentCorners[2].y - parentCorners[0].y;

        // 4. Calcular los Anchors Min/Max (coordenadas normalizadas de 0 a 1)

        // Anchor Min (esquina inferior izquierda)
        float anchorMinX = (corners[0].x - parentCorners[0].x) / parentWidth;
        float anchorMinY = (corners[0].y - parentCorners[0].y) / parentHeight;

        // Anchor Max (esquina superior derecha)
        float anchorMaxX = (corners[2].x - parentCorners[0].x) / parentWidth;
        float anchorMaxY = (corners[2].y - parentCorners[0].y) / parentHeight;

        // 5. Aplicar los nuevos Anchors
        rt.anchorMin = new Vector2(anchorMinX, anchorMinY);
        rt.anchorMax = new Vector2(anchorMaxX, anchorMaxY);

        // 6. Restablecer el Offset (Posición/Padding) a cero. 
        // ¡ESTO ES LO QUE HACE QUE SE CUADRE!
        // Al hacer esto, el objeto se estira para coincidir perfectamente con sus nuevos anchors.
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        // 7. Marcar el objeto como modificado
        EditorUtility.SetDirty(rt);

        Debug.Log("Anchors and RectTransform aligned for: " + rt.gameObject.name);
    }
}