
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

/// <summary>
/// Assigns or removes  "roleTag" role from local player when entering the portal.
/// </summary>
public class PlayerRoleAssignerPortal : UdonSharpBehaviour
{
    [Header("Properties")]
    [Tooltip("Tag that will identify this player as owner of the role.")]
    [SerializeField] string roleTag;

    [Tooltip("Maximum number of users that can own this role.")]
    [SerializeField] int MaxCapacity = 3; 
    
    [Header("Visuals")]
    [SerializeField] Text capacityLabel;
    [SerializeField] Text roleLabel;
    [SerializeField] GameObject graphic;
    [SerializeField] Material getRoleMat;
    [SerializeField] Material releaseRoleMat;
    
    [Header("ActionsTriggered by this role")]
    [SerializeField] UdonSharpBehaviour target;

    private int m_capacity;
    [UdonSynced] private int sync_capacity;


    void Start()
    {
        //TODO: CLEAN THIS PART 
        //CHECK LATE JOINERS CASE
        sync_capacity = MaxCapacity;
        m_capacity = MaxCapacity;
        capacityLabel.text ="N.Players to own role: " +m_capacity.ToString();

        roleLabel.text = "Get Rol: " + roleTag;
        SetMaterial(getRoleMat);
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player.IsValid() && player.isLocal)
        {
            //Release player role if owned.
            if (player.GetPlayerTag("role") == roleTag)
            {
                player.SetPlayerTag("role", null);
                roleLabel.text = "Get Rol: " + roleTag;
             
                SetMaterial(getRoleMat);


                if (target) target.SendCustomEvent("UndoAction");

                if (!Networking.IsOwner(this.gameObject)) Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
                sync_capacity = Mathf.Clamp(m_capacity + 1, 0, MaxCapacity);
                m_capacity = sync_capacity;
                capacityLabel.text = "N.Players to own role: " + m_capacity.ToString();
                RequestSerialization();
            }

            //Assign player role.
            else
            {
                player.SetPlayerTag("role", roleTag);
                roleLabel.text = "Release Rol: " + roleTag;

                SetMaterial(releaseRoleMat);

                if (target) target.SendCustomEvent("DoAction");

                if (!Networking.IsOwner(this.gameObject)) Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
                sync_capacity = Mathf.Clamp(m_capacity - 1, 0, MaxCapacity);
                m_capacity=sync_capacity;
                capacityLabel.text = "N.Players to own role: " + m_capacity.ToString();
                RequestSerialization();
            }

        }
    }

    public override void OnDeserialization()
    {
        m_capacity = sync_capacity;
        capacityLabel.text = m_capacity.ToString();
    }

    private void SetMaterial(Material material)
    {
        if (graphic == null) return;

        graphic.GetComponent<MeshRenderer>().material=material;
    }
}
