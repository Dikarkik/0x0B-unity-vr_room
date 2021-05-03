using System;
using TMPro;
using UnityEngine;

public class MessageText : MonoBehaviour
{
    public TextMeshPro text;
    public float maxAlpha;
    
    private void OnEnable()
    {
        LeanTween.value(text.gameObject, 0, maxAlpha, 0.5f).setOnUpdate((float val) =>
        {
            Color c = text.color;
            c.a = val;
            text.color = c;
        }).setOnComplete(() => Invoke(nameof(Quit), 1.5f));
    }
    
    private void Quit()
    {
        LeanTween.value(text.gameObject, maxAlpha, 0, 0.5f).setOnUpdate((float val) =>
        {
            Color c = text.color;
            c.a = val;
            text.color = c;
        }).setOnComplete(() => gameObject.SetActive(false));
    }

    public void SetText(String newText)
    {
        text.text = newText;
    }
}
