
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

public class LightSyncSwitch : UdonSharpBehaviour
{
    [SerializeField] Light lightC;
    [SerializeField] AudioSource audioS;

    [UdonSynced] bool syncLightEnabled;
    [UdonSynced] float syncSwitchRotation;

    private BoxCollider interactionCol;

    void Start()
    {
        //Disable interaction with light switch for everyone but the owner of this object.
        interactionCol = GetComponent<BoxCollider>();
        if (!Networking.IsOwner(this.gameObject))
        {
            interactionCol.enabled = false;
            //TODO: Display unusable/not available icon
        }
        
        

        UpdateSyncs();
    }

    public override void Interact()
    {
        SendCustomEvent("Toggle");
    }

    public void Toggle()
    {
        lightC.enabled = !lightC.enabled;
        this.transform.localEulerAngles = new Vector3(lightC.enabled ? 45 : 0, transform.localRotation.y, transform.localRotation.z);
        if (audioS) audioS.Play();

        UpdateSyncs();
        
    }

    private void UpdateSyncs()
    {
       //Update synced variables if owner
       //if(Networking.IsOwner(this.gameObject))
       syncLightEnabled = lightC.enabled;
       syncSwitchRotation= this.transform.localEulerAngles.x;


        RequestSerialization();

    }

    private void SetSwitchRotation(bool lightState)
    {
        float switchRot = lightState ? 45 : 0;
        //this.transform.localEulerAngles = new Vector3(switchRot, transform.localRotation.y, transform.localRotation.z);
    }

    //Fired when network data is received
    public override void OnDeserialization()
    {
        //Update clients with synced data.
        lightC.enabled = syncLightEnabled;
        this.transform.localEulerAngles = new Vector3(syncSwitchRotation, transform.localRotation.y, transform.localRotation.z);
        
    }

    
     

}
