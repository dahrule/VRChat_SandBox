
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class OneShotSoundSync : UdonSharpBehaviour
{
    [SerializeField] AudioSource audioS;

    public override void Interact()
    {

        SendCustomNetworkEvent(NetworkEventTarget.All, "PlayAudio");
    }

    public void PlayAudio()
    {
        audioS.Play();
    }
}
