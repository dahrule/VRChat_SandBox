
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;

/// <summary>
/// Syncing of sound
/// </summary>
public class OneShotSoundSyncController : UdonSharpBehaviour
{   
    [SerializeField] string descriptor;
    [SerializeField] Text descriptionLabel;
    [SerializeField] AudioSource audioS;


    private void Start()
    {
        if (descriptionLabel == null) return;
        descriptionLabel.text = descriptor;
    }
    public override void Interact()
    {
       SendCustomNetworkEvent(NetworkEventTarget.All, "PlayAudio");
    }

    public void PlayAudio()
    {
        audioS.Play();
    }
}
