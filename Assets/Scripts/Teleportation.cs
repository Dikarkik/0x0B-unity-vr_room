using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public SpriteRenderer image;
    public Transform player;
    private Vector3 newPosition = Vector3.zero;
    
    private void Start()
    {
        // Fade in
        Color c = image.color;
        c.a = 1;
        image.color = c;
        FadeIn();
    }

    private void FadeIn()
    {
        LeanTween.value(image.gameObject, 1, 0, 1f).setOnUpdate((float val) =>
        {
            Color c = image.color;
            c.a = val;
            image.color = c;
        });
    }
    
    public void FadeOut(Vector3 position)
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
        player.position = newPosition;
        FadeIn();
    }
}
