using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isLocked = true;
    private Animator animator;

    void Start()
    {
        CameraRaycast.script = FindObjectOfType<CameraRaycast>();
        Rotor.OnRotorReady += UnlockDoor;
        animator = transform.parent.GetComponent<Animator>();
    }

    public void UnlockDoor() => isLocked = false;

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen is touched.
    /// </summary>
    public void OnPointerClick()
    {
        if (!isLocked)
            animator.SetBool("character_nearby", true);
        else
        {
            CameraRaycast.script.alertMessage.GetComponent<MessageText>().SetText($"locked door");
            CameraRaycast.script.alertMessage.SetActive(true);
        }
    }
}
