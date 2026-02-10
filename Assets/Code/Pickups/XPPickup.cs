using UnityEngine;

public class XPPickup : Pickup
{
    [SerializeField]
    uint value = 1;

    public override void OnPickup(GameObject obj)
    {
        Player player = obj.GetComponent<Player>();
        if (player == null)
            return;

        player.XPManager.CurrentXP += (int)value;
        Destroy(gameObject);
    }
}
