using UnityEngine;

public class XPPickup : Pickup
{
    public override void OnPickup(GameObject obj)
    {
        Destroy(gameObject);
    }
}
