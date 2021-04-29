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
    private int currentFeedback;
    public SpriteRenderer[] feedbackImages;
    // [0] -> pointing to the floor
    // [1] -> pointing locked object (door)
    
    private void Start()
    {
        teleportationScript = GetComponent<Teleportation>();
        hand.gameObject.SetActive(false);
    }
    
    private void Update()
    {
        // TO DO: Quitar puntero cuando hace teleport
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
            
            Feedback();
        }
        else if (_gazedAtObject)
        {
            // No GameObject detected in front of the camera.
            _gazedAtObject.SendMessage("OnPointerExit");
            _gazedAtObject = null;
        }

        // Checks for screen touches.
        if (Google.XR.Cardboard.Api.IsTriggerPressed || Input.GetKeyDown(KeyCode.Space))
        {
            //_gazedAtObject?.SendMessage("OnPointerClick");
            
            TriggerPressed(currentFeedback);
            
            if (_gazedAtObject && !_pickedObject)
                PickUpObject();
            else if (_pickedObject)
                DropObject(_pickedObject);
        }
    }
    
    private void PickUpObject()
    {
        hand.gameObject.SetActive(true);
        _pickedObject = _gazedAtObject;
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

    private void Feedback()
    {
        switch (hit.transform.tag)
        {
            case "Floor":
                pointer.color = Color.green;
                feedbackImages[0].color = Color.white;
                feedbackImages[0].transform.position = hit.point;
                EnableFeedbackSpriteRenderer(0);
                break;
            case "Interactable":
                pointer.color = Color.green;
                EnableFeedbackSpriteRenderer(-1);
                break;
            case "Locked":
                pointer.color = Color.blue;
                feedbackImages[1].color = Color.white;
                feedbackImages[1].transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z - 0.1f);
                EnableFeedbackSpriteRenderer(1);
                break;
            default:
                pointer.color = Color.red;
                EnableFeedbackSpriteRenderer(-1);
                break;
        }
    }
    
    private void TriggerPressed(int feedback)
    {
        switch (feedback)
        {
            case 0: // floor
                teleportationScript.FadeOut(hit.point);
                pointer.color = Color.yellow;
                feedbackImages[0].color = Color.yellow;
                EnableFeedbackSpriteRenderer(0);
                break;
            case 1:
                pointer.color = Color.blue;
                EnableFeedbackSpriteRenderer(1);
                break;
            default:
                pointer.color = Color.red;
                EnableFeedbackSpriteRenderer(-1);
                break;
        }
    }

    private void EnableFeedbackSpriteRenderer(int newFeedback)
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
}
