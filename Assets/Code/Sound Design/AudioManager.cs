using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Dynamic;

public class AudioManager : MonoBehaviour
{
    public bool inGamePlayScene;

    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;
    [Range(0, 1)]
    public float ambianceVolume = 1;
    [Range(0, 1)]
    public float SFXvolume = 1;

    private Bus masterBus;
    private Bus musicBus;
    private Bus ambianceBus;
    private Bus sfxBus;


    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;
    private EventInstance ambienceEventInstance;
    private EventInstance musicEventInstance;
    private EventInstance musicMenuEventInstance;
    public static AudioManager instance { get; private set; }

    private float healthPercentage;

    //Singleton setup
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        inGamePlayScene = true;

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        ambianceBus = RuntimeManager.GetBus("bus:/Ambiance");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {
        InitializeAmbiance(FModEvents.instance.playerMovingAcrossSurface);
        InitializeMusic(FModEvents.instance.gameplayMusic);
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        ambianceBus.setVolume(ambianceVolume);
        sfxBus.setVolume(SFXvolume);

        healthPercentage = GameManager.Instance.Player.CurrentHealth / GameManager.Instance.Player.MaxHealth;

        if (inGamePlayScene)
        {
            SetAmbienceParameter("healthPercentage", healthPercentage);
            SetAmbienceParameter("speed", 10);

            musicEventInstance.setParameterByName("DAMAGE", 1 - healthPercentage);
            musicEventInstance.setParameterByName("intensity", GameManager.Instance.GameTimeProgress*4 + 1);
            musicEventInstance.setParameterByName("PAUSE", 0); //what is reference?
        }
    }


    //This is a function for starting an ambiance track
    public void InitializeAmbiance(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateEventInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }


    //This is a function for starting the music track
    public void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateEventInstance(musicEventReference);
        musicEventInstance.start();
    }

    //This is a function for starting the menu music track
    public void InitializeMenuMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateEventInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void InitializeAndPlayDeathSound()
    {
        EventInstance deathS = CreateEventInstance(FModEvents.instance.playerDeath);
        deathS.setParameterByName("playerSize", 30);
        deathS.start();
    }

    //This is a function for changing the parameters of the constantly playing sound
    public void SetAmbienceParameter(string parameterName, float parameterValue)
    {
        ambienceEventInstance.setParameterByName(parameterName, parameterValue);
    }

    public void SetWalkParameter(float parameterValue)
    {
        ambienceEventInstance.setParameterByName("isWalking", parameterValue);
    }

    //This is a function for setting the area sounds of the gameplay music
    public void SetMusicArea(string parameterName, float parameterValue)
    {
        //reset previous area sounds
        musicEventInstance.setParameterByName("Faculty", 0);
        musicEventInstance.setParameterByName("Forest", 0);
        musicEventInstance.setParameterByName("Jungle", 0);
        musicEventInstance.setParameterByName("Desert", 0);

        musicEventInstance.setParameterByName(parameterName, (float) parameterValue);
    }

    public void SetMusicPause(float parameterValue)
    {
         musicEventInstance.setParameterByName("PAUSE", (float) parameterValue);
    }

    public void SwitchScene()
    {
        inGamePlayScene = !inGamePlayScene;

        if (inGamePlayScene) 
        { 
            InitializeMusic(FModEvents.instance.gameplayMusic);
        }
        else
        {
            InitializeMenuMusic(FModEvents.instance.menuMusic);
        }
    }

    //A function for spawning a sound that is played and is then destroyed. Called from anywhere.
    public void PlayOneShot(EventReference sound, Vector3 worldPos) //Plays a sound once
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void PlayOneShotSterio(string soundName) //Plays a sound once without world coords
    {
        EventReference sound = FModEvents.instance.GetSound(soundName);
        RuntimeManager.PlayOneShot(sound);
    }

    public void PlayDeathSound() //Plays a sound once without world coords
    {
        int playerSize = GameManager.Instance.Player.Body.Count;

        RuntimeManager.PlayOneShot(FModEvents.instance.playerDeath);
    }

    
    //A function for spawning an instance of a sound that will play until its done playing or is cleared.
    public EventInstance CreateEventInstance(EventReference eventReferance) //Plays a looping sound until stopped
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReferance);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }


    //A function for initializing gameobjects that has FMOD emitters on them. Called from that object to add it to list.
    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    public void CleanUp() 
    {
    //Stop and release any created event instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();

        }
    //stop all eventemitters
        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }
    public void OnDestroy()
    {
        CleanUp();
    }

}
