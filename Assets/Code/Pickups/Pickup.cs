using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    UnityEvent<GameObject> onPickupEvent;

    public virtual void OnPickup(GameObject obj)
    {
        onPickupEvent.Invoke(obj);
    }
}
