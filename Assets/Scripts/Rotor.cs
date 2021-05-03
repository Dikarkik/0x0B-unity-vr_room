using UnityEngine;
public delegate void OnRotorReady();

public class Rotor : MonoBehaviour
{
    private CameraRaycast cameraRaycastScript;
    
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
    
    void Start()
    {
        cameraRaycastScript = FindObjectOfType<CameraRaycast>();
    }
    
    public void OnPointerClick()
    {
        // When the puzzle is done show the message "Working well"
        if (totalInPlace == requiredObjects.Length)
        {
            cameraRaycastScript.workingWellMessage.SetActive(true);
            return;
        }

        // When the player do not have any object in hand show the message "Batteries ?/?"
        var pickedObject = cameraRaycastScript.GetPickedObject();
        
        if (!pickedObject)
        {
            cameraRaycastScript.actionRequiredMessage.GetComponent<MessageText>().SetText($"batteries {totalInPlace}/{requiredObjects.Length}");
            cameraRaycastScript.actionRequiredMessage.SetActive(true);
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
                cameraRaycastScript.DisablePickedObject();

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
                    cameraRaycastScript.workingWellMessage.SetActive(true);
                    onRotorReady();
                }
                
                return;
            }
        }
        
        // When the object is not a required Object or isn't a missing object
        cameraRaycastScript.actionRequiredMessage.GetComponent<MessageText>().SetText($"invalid object");
        cameraRaycastScript.actionRequiredMessage.SetActive(true);
    }
}
