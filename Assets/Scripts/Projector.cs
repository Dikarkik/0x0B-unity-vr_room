using UnityEngine;

public class Projector : MonoBehaviour
{
    private bool _isPartyTime = false;
    public Plant[] plants;
    public ParticleSystem particles;
    public GameObject exitGameButton;

    public void OnPointerClick()
    {
        // when is party time
        if (_isPartyTime)
        {
            particles.gameObject.SetActive(!particles.gameObject.activeSelf);
            return;
        }

        foreach (var plant in plants)
        {
            // when some plant is not fine
            if (!plant.isHealthy)
            {
                CameraRaycast.script.alertMessage.GetComponent<MessageText>().SetText($"healthy plants = party time");
                CameraRaycast.script.alertMessage.SetActive(true);
                return;
            }
        }
        
        // when plants are fine: start party time
        _isPartyTime = true;
        particles.gameObject.SetActive(!particles.gameObject.activeSelf);
        exitGameButton.SetActive(true);
    }
}
