using UnityEngine;
using FMODUnity;
using System.Collections.Generic;
using Unity.VisualScripting;

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
    [field: SerializeField] public EventReference enemyIsHit { get; private set; }
    [field: SerializeField] public EventReference enemyDeathRat { get; private set; }
    [field: SerializeField] public EventReference enemyDeathPenguin { get; private set; }
    [field: SerializeField] public EventReference enemyDeathMonkey { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference menuMusic { get; private set; }
    [field: SerializeField] public EventReference gameplayMusic { get; private set; }

    Dictionary<string, EventReference> sounds = new();

    public EventReference GetSound(string soundName) =>
        sounds[soundName];

    //Singleton setup
    public static FModEvents instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        sounds.AddRange(new KeyValuePair<string, EventReference>[]
        {
            new("ButtonClick", buttonClick), //?
            new("LevelUp", levelUpFanfare),                             //done
            new("PlaceBodyPart", placeBodyPart), //?
            new("MenuSplash", menuSplash), //?
            new("ConsumeXP", playerConsumeExp),                         //done
            new("ConsumeHP", playerConsumeHp),                          //done
            new("Pickup", playerPickupPickup), //where?
            new("PickupKey", playerPickupKey),                          //wont be in game
            new("SuckTrunk", playerSuckTrunk),                          //wont be in game
            new("PlayerMove", playerMovingAcrossSurface),               //done
            new("MassHit", playerIsHitMass),                            //done
            new("ShellHit", playerIsHitShell), //
            new("ShellBreak", playerBreakShell), //
            new("ShellRegen", playerRegenShell), //
            new("PlayerDeath", playerDeath),                            //needs variable input
            new("MouthAttack", playerShootMouth), //
            new("QuillAttack", playerShotQuill), //
            new("SnakeShot", playerShootSnake), //                      
            new("GiraffeAttack", playerShootGiraffe),                   //wont be in game

            new("EnemyMove", enemyWalk), //e
            new("EnemyHit", enemyIsHit), //e
            new("EnemyDeathRat", enemyDeathRat), //e
            new("EnemyDeathPenguin", enemyDeathPenguin), //e
            new("EnemyDeathMonkey", enemyDeathMonkey), //e
            new("MenuMusic", menuMusic),   //what scene is correct?                             
            new("GameMusic", gameplayMusic)                             //done
        });
    }
}






// - parameters to insert into oneshot sounds on play -         (this is not needed)
// player death current size
// level up current size
// consume current size


// - Continuous sound changes -                                     
//menu music transition on GAMEPLAY = true update of variable           (needs to be hooked up in the menu scene)

//gameplay music pause                                                  (is hardcoded?)
//gameplay music intensity                                              (is hardcoded)
//gameplay music current health                                         (is hardcoded)
//player sound current health                                           (is hardcoded)
//player sound current speed                                            (is hardcoded but needs a variable)
//I can change music based on area                                      (done)
//I can change movement sound based on movement                         (done)


// - hook up my volume sliders to the GUI sliders                       (needs to be hooked up in the menu scene)
//put my script on a slider parent
//drop down into the slider subcomponent, onValueChanged, drag in its own parent, select onvaluechanged function
//repeat for all sliders

//somehow save the settings after a scene transition?



//Designer feedback for level design. Center stuff. Lookup. Ask Noah.

//make audio manager a persistant object between scenes if possible. Trigger other sounds like music.