using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public SpriteRenderer image;
    public Transform cameraTransform;
    private Vector3 newPosition;
    
    private void Start()
    {
        // Start in black
        Color c = image.color;
        c.a = 1;
        image.color = c;
        
        FadeOut();
    }

    public void FadeIn(Vector3 position)
    {
        newPosition.x = position.x;
        newPosition.y = position.y + 1.8f;
        newPosition.z = position.z;
        
        LeanTween.value(image.gameObject, 0, 1, 1f).setOnUpdate((float val) =>
        {
            Color c = image.color;
            c.a = val;
            image.color = c;
        }).setOnComplete(SetNewPosition);
    }

    private void SetNewPosition()
    {
        cameraTransform.position = newPosition;
        FadeOut();
    }

    private void FadeOut()
    {
        LeanTween.value(image.gameObject, 1, 0, 1f).setOnUpdate((float val) =>
        {
            Color c = image.color;
            c.a = val;
            image.color = c;
        });
    }
}
