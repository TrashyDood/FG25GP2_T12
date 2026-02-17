using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{
     [Header("Parameter Change")]
    [SerializeField] private string parameterName;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals("Player"))
        {
            AudioManager.instance.SetMusicArea(parameterName, 1);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag.Equals("Player"))
        {
            AudioManager.instance.SetMusicArea(parameterName, 0);
        }
    }
}
