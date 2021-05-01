using UnityEngine;

public class MessageImage : MonoBehaviour
{
    public SpriteRenderer image;
    public float maxAlpha;
    
    private void OnEnable()
    {
        LeanTween.value(image.gameObject, 0, maxAlpha, 0.5f).setOnUpdate((float val) =>
        {
            Color c = image.color;
            c.a = val;
            image.color = c;
        }).setOnComplete(() => Invoke(nameof(Quit), 1.5f));
    }

    private void Quit()
    {
        LeanTween.value(image.gameObject, maxAlpha, 0, 0.5f).setOnUpdate((float val) =>
        {
            Color c = image.color;
            c.a = val;
            image.color = c;
        }).setOnComplete(() => gameObject.SetActive(false));
    }
}
