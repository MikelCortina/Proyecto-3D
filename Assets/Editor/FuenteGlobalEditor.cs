using UnityEngine;
using UnityEditor;
using TMPro;

public class FuenteGlobalEditor : EditorWindow
{
    private TMP_FontAsset nuevaFuente;

    [MenuItem("Herramientas/Cambiar Fuente en Escena")]
    public static void MostrarVentana()
    {
        GetWindow<FuenteGlobalEditor>("Cambiar Fuente");
    }

    void OnGUI()
    {
        GUILayout.Label("Cambiar fuente en todos los textos de la escena", EditorStyles.boldLabel);
        nuevaFuente = (TMP_FontAsset)EditorGUILayout.ObjectField("Nueva Fuente", nuevaFuente, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("Aplicar fuente a todos los textos"))
        {
            if (nuevaFuente == null)
            {
                EditorUtility.DisplayDialog("Error", "Por favor asigna una fuente.", "OK");
                return;
            }

            CambiarFuenteEnEscena(nuevaFuente);
        }
    }

    private void CambiarFuenteEnEscena(TMP_FontAsset fuente)
    {
        int contador = 0;

        // Buscar todos los textos TextMeshProUGUI
        foreach (var texto in FindObjectsOfType<TextMeshProUGUI>(true))
        {
            Undo.RecordObject(texto, "Cambiar Fuente");
            texto.font = fuente;
            EditorUtility.SetDirty(texto);
            contador++;
        }

        // Buscar también textos 3D TextMeshPro
        foreach (var texto in FindObjectsOfType<TextMeshPro>(true))
        {
            Undo.RecordObject(texto, "Cambiar Fuente");
            texto.font = fuente;
            EditorUtility.SetDirty(texto);
            contador++;
        }

        Debug.Log($"Fuente cambiada en {contador} objetos de texto.");
    }
}
