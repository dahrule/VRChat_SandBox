
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

/// <summary>
/// Manual Syncing of group of lights state.
/// </summary>
public class LightSync : UdonSharpBehaviour
{
    [SerializeField] GameObject[] targets;
    [UdonSynced] bool sync_isOn = false;

    void Start()
    {
        foreach (var target in targets) target.SetActive(false); 
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

        sync_isOn = !targets[0].activeSelf; //set sync variable to the new  local state.
        foreach (var target in targets) target.SetActive(sync_isOn);//update local variable with synced state.
    }

    //Reflect state for late joiners too.
    public override void OnDeserialization()
    {
        foreach (var target in targets) target.SetActive(sync_isOn);
    }
}
