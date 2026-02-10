using UnityEngine;
using FMODUnity;

public class FModEvents : MonoBehaviour
{
    [field: Header("Test SFX")]
    [field: SerializeField] public EventReference soundEventA { get; private set; }
    [field: SerializeField] public EventReference soundEventB { get; private set; }

    [field: Header("UI SFX")]
    [field: SerializeField] public EventReference buttonClick { get; private set; }
    [field: SerializeField] public EventReference levelUpFanfare { get; private set; }
    [field: SerializeField] public EventReference placeBodyPart { get; private set; }
    [field: SerializeField] public EventReference menuSplash { get; private set; }


    [field: Header("Pickups SFX")]
    [field: SerializeField] public EventReference playerConsumeExp { get; private set; }
    [field: SerializeField] public EventReference playerConsumeHp { get; private set; }
    [field: SerializeField] public EventReference playerPickupPickup { get; private set; }
    [field: SerializeField] public EventReference playerPickupKey { get; private set; }
    [field: SerializeField] public EventReference playerSuckTrunk { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerMovingAcrossSurface { get; private set; }
    [field: SerializeField] public EventReference playerIsHitMass { get; private set; }
    [field: SerializeField] public EventReference playerIsHitShell { get; private set; }
    [field: SerializeField] public EventReference playerBreakShell { get; private set; }
    [field: SerializeField] public EventReference playerRegenShell { get; private set; }
    [field: SerializeField] public EventReference playerDeath { get; private set; }

    [field: Header("Weapons SFX")]
    [field: SerializeField] public EventReference playerShootMouth { get; private set; }
    [field: SerializeField] public EventReference playerShotQuill { get; private set; }
    [field: SerializeField] public EventReference playerShootSnake { get; private set; }
    [field: SerializeField] public EventReference playerShootGiraffe { get; private set; }


    [field: Header("Enemy SFX")]
    [field: SerializeField] public EventReference enemyWalk { get; private set; }
    [field: SerializeField] public EventReference enemyAttack { get; private set; }
    [field: SerializeField] public EventReference enemyIsHit { get; private set; }
    [field: SerializeField] public EventReference enemyDeath { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference menuMusic { get; private set; }
    [field: SerializeField] public EventReference gameplayMusic { get; private set; }





    //Singleton setup
    public static FModEvents instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        { 
            Debug.LogError("Found more than one FMOD events script in the scene"); 
        }
        instance = this;
    }
}
