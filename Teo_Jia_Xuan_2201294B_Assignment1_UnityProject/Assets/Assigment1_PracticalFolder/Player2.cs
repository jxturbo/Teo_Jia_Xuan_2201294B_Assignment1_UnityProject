using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public Animator PlayerAnim;
    public PlayerMovemnt playerMovemnt;
    public int swordSwing;
    public float idleTime;

    // Update is called once per frame
    void Update()
    {
        //checks if player is not moving and a idle animation has yet to play
        if(PlayerAnim.GetInteger("RandomIdle") == 0 && !playerMovemnt.moving && !PlayerAnim.GetBool("Attack") && !PlayerAnim.GetBool("Attack2") && !PlayerAnim.GetBool("Attack3"))
        {
            idleTime += Time.deltaTime;
        }
        //resets  idle timer since we just want the time to update when player is idle
        else if(playerMovemnt.moving)
        {
            idleTime = 0;
        }
        //checks for mouse click and if any other attack animations are not playing
        if (Input.GetMouseButtonDown(0) && !PlayerAnim.GetBool("Attack") && !PlayerAnim.GetBool("Attack2") && !PlayerAnim.GetBool("Attack3"))
        {
            idleTime = 0;
            //prevents player from moving while attacking
            playerMovemnt.enabled = false;
            swordSwing += 1;
            //switch statement used to cycle between animations and make it easier to read instead  
            //of using if statements
            switch (swordSwing)
            {
                case 1:
                    //triggers a bool that is used as the condition to transition to Attack animation
                    PlayerAnim.SetBool("Attack", true);
                    break;
                case 2:
                    PlayerAnim.SetBool("Attack2", true);
                    break;
                case 3:
                    PlayerAnim.SetBool("Attack3", true);
                    break;
                default:
                    break;
            }
              
        }
        //if player is idle for long enough, trigger the method to play it
        //player is idle so theyre not supposed to move
        else if(idleTime >= 3)
        {
            idleTime = 0;
            playerMovemnt.enabled = false;
            RandomIdleAnimation();
        }
    }
    //Animation events used to trigger this method at the end of the animation
    public void StopAttack()
    {   
        if(swordSwing >= 3)
        {
            //since the character is a knight,
            //a rest sequence is used where they sheathe their sword instead of a reload animation
            StartCoroutine("Rest");
        }
        else
        {
            //check which sword swing its on, disables all the bools to have it revert back to its movement animation
            //allows the player to move again
            Debug.Log(swordSwing);
            playerMovemnt.enabled = true;
            PlayerAnim.SetBool("Attack", false);
            PlayerAnim.SetBool("Attack2", false);
            PlayerAnim.SetBool("Attack3", false);
        }
    }
    public IEnumerator Rest()
    {
        //since theres no need to return from sheathe to movement, trigger is used
        PlayerAnim.SetTrigger("Sheathe");
        //transition used to allow for more fluid movement since using animation events here splits up the code 
        //into too many methods
        yield return new WaitForSeconds(1f);
        //same goes for unsheathe to movement
        PlayerAnim.SetTrigger("Unsheathe");
        yield return new WaitForSeconds(1f);
        //used to return to movement state and let the previous animation fully play out 
        //instead of immediately returning to the blend tree movement
        PlayerAnim.SetTrigger("Resume");
        //reset swordswing count to prevent player from 'resting' every sword swing after the third
        swordSwing = 0;
        playerMovemnt.enabled = true;
        PlayerAnim.SetBool("Attack", false);
        PlayerAnim.SetBool("Attack2", false);
        PlayerAnim.SetBool("Attack3", false);
    }
    private void RandomIdleAnimation()
    {
        //randomises the index to allow the idle animations to play randomly
        //int used to prevent the collection of too many bool variables
        //one float is better than eg: 5 bool var
        int randomIndex = Random.Range(1, 6);
        PlayerAnim.SetInteger("RandomIdle", randomIndex);
    }
    //uses animation events to deactivate the animation
    public void StopIdleAnimation()
    {
        PlayerAnim.SetInteger("RandomIdle", 0);
        playerMovemnt.enabled = true;
    }

}
