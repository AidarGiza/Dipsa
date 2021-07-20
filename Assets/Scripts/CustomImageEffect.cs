using UnityEngine;

[ExecuteInEditMode]
public class CustomImageEffect : MonoBehaviour
{

    public Material effectMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, effectMaterial);
    }

}
