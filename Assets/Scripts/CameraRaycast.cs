using UnityEngine;

public enum ObjectType
{
    None,
    BatteryCube,
    BatteryCylinder,
    Mug
}

public class CameraRaycast : MonoBehaviour
{
    // Access to this script
    public static CameraRaycast script;
    
    // Rayacast
    private const float MaxDistance = 35;
    private const float PickUpDistance = 3.5f;
    private GameObject _gazedAtObject = null;
    private GameObject _pickedObject = null;
    private RaycastHit hit;

    // Player
    public Transform hand;
    private Teleportation teleportationScript;
    
    // Feedback
    public SpriteRenderer pointer;
    public SpriteRenderer floorPointer;
    private Color colorTransparent;
    public GameObject goCloserMessage;
    public GameObject alertMessage;
    public GameObject fineMessage;
    
    private void Start()
    {
        script = this;
        teleportationScript = GetComponent<Teleportation>();
        colorTransparent = new Color(0, 0, 0, 0);
    }
    
    private void Update()
    {
        CheckHit();

        PointerFeedback();
        
        // Checks for screen touches.
        if (Google.XR.Cardboard.Api.IsTriggerPressed || Input.GetKeyDown(KeyCode.Space))
            TriggerPressed();
    }

    private void CheckHit()
    {
        // Casts ray towards camera's forward direction, to detect if a GameObject is being gazed at.
        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxDistance))
        {
            // GameObject detected in front of the camera.
            if (_gazedAtObject != hit.transform.gameObject)
            {
                _gazedAtObject?.SendMessage("OnPointerExit");

                // Save new GameObject.
                if (hit.transform.CompareTag("Interactable"))
                {
                    _gazedAtObject = hit.transform.gameObject;
                    _gazedAtObject.SendMessage("OnPointerEnter");
                }
                else
                    _gazedAtObject = null;
            }
        }
        else if (_gazedAtObject)
        {
            // No GameObject detected in front of the camera.
            _gazedAtObject.SendMessage("OnPointerExit");
            _gazedAtObject = null;
        }
    }

    private void PickUpObject(GameObject obj)
    {
        _pickedObject = obj;
        _pickedObject.GetComponent<Rigidbody>().useGravity = false;
        _pickedObject.transform.parent = hand;

        // Set position and rotation
        var type = obj.GetComponent<Interactable>().objectType;
        
        switch (type)
        {
            case ObjectType.Mug:
                _pickedObject.transform.localPosition = new Vector3(0.3683f, -0.1592f, -0.4095f);
                _pickedObject.transform.localRotation = Quaternion.Euler(-24.79f, -436.193f, -121.874f);
                break;
            default:
                _pickedObject.transform.localPosition = new Vector3(0, -1.3f, 0);
                _pickedObject.transform.localRotation = Quaternion.identity;
                break;
        }
    }

    public void DropObject(GameObject obj)
    {
        if (_pickedObject != obj)
            return;
        
        _pickedObject.GetComponent<Rigidbody>().useGravity = true;
        _pickedObject.transform.parent = null;
        _pickedObject = null;
    }

    public GameObject GetPickedObject()
    {
        return _pickedObject ? _pickedObject : null;
    }

    public void DisablePickedObject()
    {
        if (_pickedObject)
        {
            _pickedObject.GetComponent<Interactable>().enabled = false;
            _pickedObject.GetComponent<Collider>().enabled = false;
            _pickedObject.SetActive(false);
            _pickedObject = null;
        }
    }
    
    private void PointerFeedback()
    {
        switch (hit.transform.tag)
        {
            case "Floor":
                pointer.color = colorTransparent;
                floorPointer.color = Color.white;
                floorPointer.transform.position = hit.point;
                break;
            case "Interactable":
            case "InteractableStatic":
                pointer.color = Color.green;
                floorPointer.color = colorTransparent;
                break;
            default:
                pointer.color = Color.red;
                floorPointer.color = colorTransparent;
                break;
        }
    }

    private void TriggerPressed()
    {
        switch (hit.transform.tag)
        {
            case "Floor":
                teleportationScript.FadeOut(hit.point);
                break;
            case "Interactable":
                if (_pickedObject)
                    DropObject(_pickedObject);
                else
                {
                    if (hit.distance < PickUpDistance)
                        PickUpObject(_gazedAtObject);
                    else
                        goCloserMessage.SetActive(true);
                }
                break;
            case "InteractableStatic":
                if (hit.distance < PickUpDistance)
                    hit.transform.SendMessage("OnPointerClick");
                else
                    goCloserMessage.SetActive(true);
                break;
            default:
                if (_pickedObject)
                    DropObject(_pickedObject);
                break;
        }
    }
}
