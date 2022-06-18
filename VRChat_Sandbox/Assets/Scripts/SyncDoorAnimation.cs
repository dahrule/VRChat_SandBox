
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class SyncDoorAnimation : UdonSharpBehaviour
{
    public Animator animator;
    public AudioSource audioSource;

    public override void Interact()
    {
        SendCustomNetworkEvent(NetworkEventTarget.All, "ToggleDoorState");
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if(Networking.IsMaster) //Check if  this client is master (user with longest time inside the world)
        {
            if(animator.GetBool("Open")) SendCustomNetworkEvent(NetworkEventTarget.All, "OpenDoor");

            else SendCustomNetworkEvent(NetworkEventTarget.All, "CloseDoor");
        }
    }

    public void ToggleDoorState()
    {
        animator.SetBool("Open", !animator.GetBool("Open"));
        audioSource.Play();

    }
    public void OpenDoor()
    {
        animator.SetBool("Open", true);
    }
    public void CloseDoor()
    {
        animator.SetBool("Open", false);
    }

    


}
