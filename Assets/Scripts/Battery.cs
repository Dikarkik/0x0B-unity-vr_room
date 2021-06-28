using UnityEngine;

public class Battery : MonoBehaviour
{
    private Material material;
    private Color emissionColor;
    private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");
    private static readonly int ColorId = Shader.PropertyToID("_Color");

    private void Start()
    {
        this.enabled = false;
        material = GetComponent<Renderer>().material;
        material.EnableKeyword("_EMISSION");
    }

    public void SetColor()
    {
        material.color = Color.green;
        emissionColor = material.GetColor(ColorId);
        emissionColor *= Mathf.Pow(2.0F, -0.8f - (0.4169F));
        material.SetColor(EmissionColorId, emissionColor);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 50f);
    }
}
