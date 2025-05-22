using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ColorBlindEffect : MonoBehaviour
{
    public Shader colorBlindShader;
    private Material _material;

    void Start()
    {
        if (colorBlindShader == null)
        {
            Debug.LogError("Falta asignar el shader al script.");
            enabled = false;
            return;
        }

        if (!colorBlindShader.isSupported)
        {
            Debug.LogError("El shader no es compatible con esta plataforma.");
            enabled = false;
            return;
        }

        _material = new Material(colorBlindShader);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_material != null)
            Graphics.Blit(source, destination, _material);
        else
            Graphics.Blit(source, destination);
    }
}
