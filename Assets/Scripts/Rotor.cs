using UnityEngine;

public delegate void OnRotorReady();

public class Rotor : MonoBehaviour
{
    // puzzle
    private int totalInPlace = 0;
    private ObjectType[] requiredObjects =
    { 
        ObjectType.BatteryCube,
        ObjectType.BatteryCylinder,
        ObjectType.BatteryCube
    };
    private Battery[] objectsInPlace =
    {
        null,
        null,
        null
    };
    public Transform[] objectPositions;
    public GameObject light;
    public GameObject screenMessage;
    public static event OnRotorReady onRotorReady;

    public void OnPointerClick()
    {
        // When the puzzle is done show the message "Working well"
        if (totalInPlace == requiredObjects.Length)
        {
            CameraRaycast.script.fineMessage.SetActive(true);
            return;
        }

        // When the player don't have an object: show the message "Batteries ?/?"
        var pickedObject = CameraRaycast.script.GetPickedObject();
        
        if (!pickedObject)
        {
            CameraRaycast.script.alertMessage.GetComponent<MessageText>().SetText($"batteries {totalInPlace}/{requiredObjects.Length}");
            CameraRaycast.script.alertMessage.SetActive(true);
            return;
        }

        // Check if the picked object is a required object
        var type = pickedObject.GetComponent<Interactable>().objectType;

        for (int i = 0; i < requiredObjects.Length; i++)
        {
            // If this is a required object and is still missing
            if (type == requiredObjects[i] && !objectsInPlace[i])
            {
                // Disable Collider and Interatable. And clean '_pickedObject' variable.
                CameraRaycast.script.DisablePickedObject();

                // Activate Battery
                pickedObject.transform.position = objectPositions[i].position;
                pickedObject.transform.rotation = objectPositions[i].rotation;
                pickedObject.transform.parent = transform.parent;
                objectsInPlace[i] = pickedObject.GetComponent<Battery>();
                objectsInPlace[i].enabled = true;
                objectsInPlace[i].SetColor();
                pickedObject.SetActive(true);
                totalInPlace++;
                
                // Puzzle Done, Activate Light, Turn on computer
                if (totalInPlace == requiredObjects.Length)
                {
                    light.SetActive(true);
                    screenMessage.SetActive(true);
                    CameraRaycast.script.fineMessage.GetComponent<MessageText>().SetText($"working well");
                    CameraRaycast.script.fineMessage.SetActive(true);
                    onRotorReady();
                }
                
                return;
            }
        }
        
        // When the object is not a required Object or isn't a missing object
        CameraRaycast.script.alertMessage.GetComponent<MessageText>().SetText($"invalid object");
        CameraRaycast.script.alertMessage.SetActive(true);
    }
}
