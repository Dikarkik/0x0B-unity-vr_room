using UnityEngine;

public class Battery : MonoBehaviour
{
    private Material material;
    private Color emissionColor;

    private void Start()
    {
        this.enabled = false;
        material = GetComponent<Renderer>().material;
        material.EnableKeyword("_EMISSION");
    }

    public void SetColor()
    {
        material.color = Color.green;
        emissionColor = material.GetColor("_Color");
        emissionColor *= Mathf.Pow(2.0F, -0.8f - (0.4169F));
        material.SetColor("_EmissionColor", emissionColor);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 50f);
    }
}
