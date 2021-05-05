using UnityEngine;

public class Mug : MonoBehaviour
{
    private bool isFill = false;
    public GameObject water;

    public bool GetFillState() => isFill;

    public void FillMug()
    {
        isFill = true;
        water.SetActive(true);
    }
    
    public void EmptyMug()
    {
        isFill = false;
        water.SetActive(false);
    }
}
