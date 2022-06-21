
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;

/// <summary>
/// Manual Syncing of a group of lights.
/// !Set Synchronization Method to Manual in Editor.
/// </summary>
public class LightSyncController : UdonSharpBehaviour
{
    [SerializeField] string descriptor;
    [SerializeField] Text descriptionLabel;
    [SerializeField] GameObject[] targets;
    [UdonSynced] bool sync_isOn = false;

    void Start()
    {
        if (targets.Length == 0) return;
        foreach (var target in targets) target.SetActive(false);

        if (descriptionLabel == null) return;
        descriptionLabel.text = descriptor;
    }

    public override void Interact()
    {
        //Tranfer ownership of this game object to local player. This ensures everyone can change the synced state.
        if (!Networking.IsOwner(this.gameObject)) Networking.SetOwner(Networking.LocalPlayer, this.gameObject);

        Toggle();

        RequestSerialization();//Inform that synced variables had been changed (required with Manual Syncing only).
    }

    private void Toggle()
    {
        if (targets.Length == 0) return;
        sync_isOn = !targets[0].activeSelf; //set sync variable to the new  local state.
        foreach (var target in targets) target.SetActive(sync_isOn);//update local variable with synced state.
    }

    //Update synced variables for late joiners too.
    public override void OnDeserialization()
    {
        if (targets.Length == 0) return;
        foreach (var target in targets) target.SetActive(sync_isOn);
    }
}
