using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void OnPointerClick()
    {
        Application.Quit();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 6f);
    }
}
