using System;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    // Feedback
    public SpriteRenderer pointer;
    public SpriteRenderer[] feedbackImages;
    private int currentFeedback;
    
    // Rayacast
    private const float _maxDistance = 35;
    private GameObject _gazedAtObject = null;
    private RaycastHit hit;

    private Teleportation teleportationScript;

    private void Start()
    {
        teleportationScript = GetComponent<Teleportation>();
    }

    /// Update is called once per frame.
    private void Update()
    {
        // PONER UN CANCLICK PARA CUANDO LE DOY PRESS SE MUESTRE AMARILLO
        
        
        // Casts ray towards camera's forward direction, to detect if a GameObject is being gazed at.
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
        {
            // GameObject detected in front of the camera.
            if (_gazedAtObject != hit.transform.gameObject)
            {
                // New GameObject.
                //_gazedAtObject?.SendMessage("OnPointerExit");

                if (hit.transform.CompareTag("Interactable"))
                {
                    _gazedAtObject = hit.transform.gameObject;
                    //_gazedAtObject.SendMessage("OnPointerEnter");
                }
                else
                    _gazedAtObject = null;
            }
            
            PointerFeedback();
        }
        else
        {
            // No GameObject detected in front of the camera.
            //_gazedAtObject?.SendMessage("OnPointerExit");
            _gazedAtObject = null;
        }

        // Checks for screen touches.
        if (Google.XR.Cardboard.Api.IsTriggerPressed || Input.GetKeyDown(KeyCode.Space))
        {
            _gazedAtObject?.SendMessage("OnPointerClick");
            TriggerPressed(currentFeedback);
        }
    }

    private void PointerFeedback()
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
            case 0:
                teleportationScript.FadeIn(hit.point);
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
