using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioClip[] SandSounds;
    public AudioClip[] DirtSounds;
    public AudioClip[] MetalSounds;
    public AudioClip[] ConcreteSounds;
    public AudioClip[] WoodSounds;
    public bool stopAudioSource;
    public AudioSource audiosource;
    public LayerMask groundLayer;
    public AudioClip[] FootStepsSoundClips;

    //played during an animation event, specifically the frame in which the foot touches the ground
    //since we are using a blend tree for the movement, the left and right animations are naturally blended with the other animations
    //hence the events of the other animations can be used instead of adding some to 
    //the left and right anim
    private void PlayFootStepsSound()
    {
        //draws a line from player position straight down
        Vector3 p1 = transform.position;
        Vector3 p2 = Vector3.down + Vector3.forward/5;
        //a specified layer in inspected used for ground layer since we only want to 
        //detect the floor
        if (Physics.Raycast(p1, p2, out RaycastHit hit, 1f, groundLayer))
        {
            //retrieves tag from raycast
            string floorTag = hit.collider.gameObject.tag;
            //determines which soundclip to load in based on the tag of the object the raycast detected
            switch (floorTag)
            {
                case "Sand":
                    //Debug.Log("On sand floor!");
                    FootStepsSoundClips = SandSounds;
                    break;
                case "Dirt":
                    //Debug.Log("On dirt floor!");
                    FootStepsSoundClips = DirtSounds;
                    break;
                case "Metal":
                    //Debug.Log("On metal floor!");
                    FootStepsSoundClips = MetalSounds;
                    break;
                case "Concrete":
                    //Debug.Log("On concrete floor!");
                    FootStepsSoundClips = ConcreteSounds;
                    break;
                case "Wood":
                    //Debug.Log("On wood floor!");
                    FootStepsSoundClips = WoodSounds;
                    break;
                default:
                    //Debug.Log("On an unknown floor type!");
                    FootStepsSoundClips = null;
                    break;
            }
        }

        
        //prevents overlapping sounds by stopping the previous sound
        //being played before starting the next one
        if(stopAudioSource)
        {
            audiosource.Stop();
            stopAudioSource = false;
        }
        //sets variables for below based on the configurations 
        //through a random number from 0 to the max length of the array
        int randomIndex = Random.Range(0, FootStepsSoundClips.Length);
        // set the pitch of the audio source
        audiosource.pitch = Random.Range(0.8f, FootStepsSoundClips.Length/2);
        audiosource.volume = Random.Range(0.5f, 1f);
        //checks that the index is truly random in console
        Debug.Log(randomIndex);
        //loads in sound clip to be played
        AudioClip soundClip = FootStepsSoundClips[randomIndex];
        //plays sound
        audiosource.PlayOneShot(soundClip);
        stopAudioSource = true;
        
    }
    
}
