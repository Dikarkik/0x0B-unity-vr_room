using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    // Rayacast
    private const float _maxDistance = 35;
    private const float _pickUpDistance = 0.5f;
    private GameObject _gazedAtObject = null;
    private GameObject _pickedObject = null;
    private RaycastHit hit;

    // Player
    public Transform hand;
    private Teleportation teleportationScript;
    
    // Feedback
    public SpriteRenderer pointer;
    private Color colorTransparent;
    private int currentFeedback;
    public SpriteRenderer[] feedbackImages;
    // [0] -> pointing to the floor
    // [1] -> pointing locked object (door)
    public GameObject goCloserMessage;
    
    private void Start()
    {
        teleportationScript = GetComponent<Teleportation>();
        hand.gameObject.SetActive(false);
        colorTransparent = new Color(0, 0, 0, 0);
    }
    
    private void Update()
    {
        // TO DO: _pickUpDistance para recoger object
        
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
            
            if (hit.collider)
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
        hand.gameObject.SetActive(false);
    }

    private void HitFeedback()
    {
        if (!hit.collider)
        {
            pointer.color = Color.red;
            SetFeedbackImage(-1);
            return;
        }
            
        switch (hit.transform.tag)
        {
            case "Floor":
                pointer.color = colorTransparent;
                feedbackImages[0].transform.position = hit.point;
                SetFeedbackImage(0);
                break;
            case "Interactable":
                pointer.color = Color.green;
                SetFeedbackImage(-1);
                break;
            case "Locked":
                pointer.color = Color.green;
                feedbackImages[1].transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z - 0.1f);
                SetFeedbackImage(1);
                break;
            default:
                pointer.color = Color.red;
                SetFeedbackImage(-1);
                break;
        }
    }
    
    private void SetFeedbackImage(int newFeedback)
    {
        if (newFeedback != currentFeedback)
        {
            currentFeedback = newFeedback;
            
            if (currentFeedback != -1)
                feedbackImages[currentFeedback].enabled = true;
            
            for (int i = 0; i < feedbackImages.Length; i++)
                if (i != currentFeedback)
                    feedbackImages[i].enabled = false;
        }
    }
    
    private void TriggerPressed()
    {
        switch (hit.transform.tag)
        {
            case "Floor": // floor
                teleportationScript.FadeOut(hit.point);
                break;
            case "Interactable":
                if (_pickedObject)
                    DropObject(_pickedObject);
                else
                {
                    if (hit.distance < 1.8f)
                        PickUpObject(_gazedAtObject);
                    else
                        goCloserMessage.SetActive(true);
                }
                break;
            case "Locked":
                break;
            default:
                DropObject(_pickedObject);
                break;
        }
    }
}
