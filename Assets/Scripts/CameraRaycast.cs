using UnityEngine;

public enum ObjectType
{
    None,
    BatteryCube,
    BatteryCylinder,
}

public class CameraRaycast : MonoBehaviour
{
    // Rayacast
    private const float _maxDistance = 35;
    private const float _pickUpDistance = 3.5f;
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
    public GameObject actionRequiredMessage;
    public GameObject workingWellMessage;
    
    private void Start()
    {
        teleportationScript = GetComponent<Teleportation>();
        hand.gameObject.SetActive(false);
        colorTransparent = new Color(0, 0, 0, 0);
    }
    
    private void Update()
    {
        // Casts ray towards camera's forward direction, to detect if a GameObject is being gazed at.
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
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
        
        HitFeedback();
        
        // Checks for screen touches.
        if (Google.XR.Cardboard.Api.IsTriggerPressed || Input.GetKeyDown(KeyCode.Space))
        {
            //_gazedAtObject?.SendMessage("OnPointerClick");
            TriggerPressed();
        }
    }
    
    private void PickUpObject(GameObject obj)
    {
        hand.gameObject.SetActive(true);
        _pickedObject = obj;
        _pickedObject.GetComponent<Rigidbody>().useGravity = false;
        _pickedObject.transform.parent = hand;
        _pickedObject.transform.localPosition = new Vector3(0, -1.3f, 0);
        _pickedObject.transform.localRotation = Quaternion.identity;
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
    
    private void HitFeedback()
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
                    if (hit.distance < _pickUpDistance)
                        PickUpObject(_gazedAtObject);
                    else
                        goCloserMessage.SetActive(true);
                }
                break;
            case "InteractableStatic":
                if (hit.distance < _pickUpDistance)
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
