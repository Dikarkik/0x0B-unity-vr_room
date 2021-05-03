using UnityEngine;

public class Interactable : MonoBehaviour
{
    private CameraRaycast cameraRaycastScript;
    private Material material;
    private Color emissionColor;
    public ObjectType objectType;
    
    void Start()
    {
        cameraRaycastScript = FindObjectOfType<CameraRaycast>();
        
        // Emission Intensity
        // https://forum.unity.com/threads/setting-material-emission-intensity-in-script-to-match-ui-value.661624/
        material = GetComponent<Renderer>().material;
        material.EnableKeyword("_EMISSION");
        material.color = Color.white;
        emissionColor = material.GetColor("_Color");
        emissionColor *= Mathf.Pow(2.0F, -0.8f - (0.4169F));
    }

    public void OnPointerEnter()
    {
        material.color = Color.yellow;
        material.SetColor("_EmissionColor", emissionColor);
    }

    public void OnPointerExit()
    {
        material.color = Color.white;
        material.SetColor("_EmissionColor", Color.black);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        cameraRaycastScript.DropObject(gameObject);
    }
}
