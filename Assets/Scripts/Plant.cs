using UnityEngine;

public class Plant : MonoBehaviour
{
    public Material healthyMaterial;
    public Material thirstyMaterial;
    public bool isHealthy = true;
    private MeshRenderer renderer;
    public PlantType type;

    public enum PlantType
    {
        Bush,
        Leaves
    }
    
    void Start() => renderer = GetComponent<MeshRenderer>();

    public void OnPointerClick()
    {
        // when the plant is fine
        if (isHealthy)
        {
            CameraRaycast.script.fineMessage.GetComponent<MessageText>().SetText($"healthy plant");
            CameraRaycast.script.fineMessage.SetActive(true);
            return;
        }
        
        // when the player does not have an object and the plant is thirsty
        var obj = CameraRaycast.script.GetPickedObject();

        if (!obj && !isHealthy)
        {
            CameraRaycast.script.alertMessage.GetComponent<MessageText>().SetText($"thirsty plant");
            CameraRaycast.script.alertMessage.SetActive(true);
            return;
        }

        // when the player have an invalid object
        var interactableScript = obj.GetComponent<Interactable>();

        if (interactableScript.objectType != ObjectType.Mug)
        {
            CameraRaycast.script.alertMessage.GetComponent<MessageText>().SetText($"invalid object");
            CameraRaycast.script.alertMessage.SetActive(true);
            return;
        }
        
        // when the player have a mug without water
        var mugScript = obj.GetComponent<Mug>();

        if (!mugScript.GetFillState())
        {
            CameraRaycast.script.alertMessage.GetComponent<MessageText>().SetText($"empty mug");
            CameraRaycast.script.alertMessage.SetActive(true);
            return;
        }

        // when the player have a mug with water
        mugScript.EmptyMug();
        WaterThePlant();
    }

    public void WaterThePlant()
    {
        isHealthy = true;
        Material[] mats = renderer.materials;

        switch (type)
        {
            case PlantType.Bush:
                mats[0] = healthyMaterial;
                break;
            case PlantType.Leaves:
                mats[2] = healthyMaterial;
                break;
        }

        renderer.materials = mats;
    }
}
