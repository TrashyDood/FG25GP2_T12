using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{
     [Header("Parameter Change")]
    [SerializeField] private string parameterName;
    [SerializeField] private float parameterValue;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals("Player"))
        {
            Debug.Log("Hit a player");
            AudioManager.instance.SetMusicArea(parameterName, parameterValue);
        }
    }
}
