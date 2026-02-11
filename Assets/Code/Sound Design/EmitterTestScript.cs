using UnityEngine;

public class EmitterTestScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        AudioManager.instance.PlayOneShot(FModEvents.instance.playerPickupKey, this.transform.position);
    }
}
