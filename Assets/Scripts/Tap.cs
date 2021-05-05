using UnityEngine;

public class Tap : MonoBehaviour
{
    private Vector3 mugPosition = new Vector3(4.98f, 1.319f, 14.714f);

    public void OnPointerClick()
    {
        // when the player does not have an object
        var obj = CameraRaycast.script.GetPickedObject();

        if (!obj)
        {
            CameraRaycast.script.alertMessage.GetComponent<MessageText>().SetText($"hold a mug is your hand");
            CameraRaycast.script.alertMessage.SetActive(true);
            return;
        }

        // when the object is not a mug
        var type = obj.GetComponent<Interactable>().objectType;
        
        if (type != ObjectType.Mug)
        {
            CameraRaycast.script.alertMessage.GetComponent<MessageText>().SetText($"invalid object");
            CameraRaycast.script.alertMessage.SetActive(true);
            return;
        }

        // when the object is a mug:
        // set the mug into the sink
        CameraRaycast.script.DisablePickedObject();
        obj.transform.position = mugPosition;
        obj.transform.rotation = Quaternion.Euler(0, -356.083f, 0);
        obj.transform.parent = transform.parent;
        obj.SetActive(true);
        // fill it with water
        var mugScript = obj.GetComponent<Mug>();
        mugScript.FillMug();
        obj.GetComponent<Collider>().enabled = true;
        obj.GetComponent<Interactable>().enabled = true;
    }
}
