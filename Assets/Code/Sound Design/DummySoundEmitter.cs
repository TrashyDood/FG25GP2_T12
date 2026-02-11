using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;

[RequireComponent(typeof(StudioEventEmitter))]

public class DummySoundEmitter : MonoBehaviour
{
    private EventInstance audio;
    private StudioEventEmitter emitter;
    private bool shouldPlaySound;

    private void Start()
    {
        audio = AudioManager.instance.CreateEventInstance(FModEvents.instance.soundEventA); //for creating an effect that can be started and stopped. I guess...
        emitter = AudioManager.instance.InitializeEventEmitter(FModEvents.instance.soundEventB, this.gameObject);
        emitter.Play(); //for effects that are always on.
    }
    private void OnTriggerEnter(Collider other)
    {
        AudioManager.instance.PlayOneShot(FModEvents.instance.playerPickupKey, this.transform.position);
        shouldPlaySound = true;
    }
    private void OnTriggerExit(Collider other)
    {
        shouldPlaySound = false;
    }

    private void UpdateSound()
    {
        if (shouldPlaySound)
        {
            //Get the playback state
            PLAYBACK_STATE playbackState;
            audio.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                audio.start();
                AudioManager.instance.PlayOneShot(FModEvents.instance.soundEventA, this.transform.position);
            }
        }
        else
        {
            //audio.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }



}
