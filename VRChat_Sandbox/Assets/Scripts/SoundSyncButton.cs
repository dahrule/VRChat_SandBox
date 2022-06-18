
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;

public class SoundSyncButton : UdonSharpBehaviour
{
    [SerializeField] AudioSource audioS;
    [SerializeField] Toggle toggle;
     bool doesLoop;

    private void Start()
    {
        if (toggle) doesLoop = toggle.isOn;
    }
    public void PlaySound()
    {
        SendCustomNetworkEvent(NetworkEventTarget.All, "SyncedAudio");
    }

    public void SyncedAudio()
    {
        audioS.loop = doesLoop;
        audioS.Play();
    }

    public void Toggle()
    {
        SendCustomNetworkEvent(NetworkEventTarget.All, "SyncedToggle");
        //SendCustomNetworkEvent(NetworkEventTarget.All, "SyncedToggle2");

    }

    public void SyncedToggle()
    {
        
        doesLoop = !doesLoop;
        if(doesLoop) toggle.isOn = true;
        else toggle.isOn = false;

    }
    public void SyncedToggle2()
    {
        


    }
}
