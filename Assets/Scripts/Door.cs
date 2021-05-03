using UnityEngine;

public class Door : MonoBehaviour
{
    private CameraRaycast cameraRaycastScript;
    private bool isLocked = true;
    private Animator animator;

    void Start()
    {
        cameraRaycastScript = FindObjectOfType<CameraRaycast>();
        Rotor.onRotorReady += UnlockDoor;
        animator = transform.parent.GetComponent<Animator>();
    }

    public void UnlockDoor() => isLocked = false;

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen is touched.
    /// </summary>
    public void OnPointerClick()
    {
        if (!isLocked)
        {
            Debug.Log("abrir puerta");
            animator.SetBool("character_nearby", true);
        }
        else
        {
            cameraRaycastScript.actionRequiredMessage.GetComponent<MessageText>().SetText($"door locked");
            cameraRaycastScript.actionRequiredMessage.SetActive(true);
        }
    }
}
