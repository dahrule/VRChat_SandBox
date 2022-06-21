
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

/// <summary>
/// Assigns or removes a role from local player when entering the portal.
/// !Set Synchronization Method to Manual in Editor.
/// </summary>
public class PlayerRoleAssigner : UdonSharpBehaviour
{
    #region Variables
    [Header("Properties")]
    [Tooltip("Role that the portal assigns.")]
    [SerializeField] string portalRole;

    [Tooltip("Maximum number of users that can own this role.")]
    [SerializeField] int MaxCapacity = 0;

    //TODELETE 
    /*[Header("UI")] 
    [SerializeField] Text capacityLabel;
    [SerializeField] Text roleLabel;
    */
    [Header("Graphics")]
    [SerializeField] GameObject portalGraphic;
    [SerializeField] Material getRoleMat;
    [SerializeField] Material releaseRoleMat;

    [Header("ActionsTriggered by this role")]
    [SerializeField] UdonSharpBehaviour ActionTarget;

    [Header("UI")]
    [SerializeField] UdonSharpBehaviour[] UITargets;

    private int m_capacity;
    [UdonSynced] private int sync_capacity;
    private bool m_playerOwnsRole = false;
    private bool m_portalEnabled;
    #endregion

    #region Built-in Methods
    void Start()
    {
        m_capacity = MaxCapacity;
        sync_capacity = m_capacity;

        SetPortalMaterial();

        SetPortalAvailability();

        UpdateUI();
    }
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (!(player.IsValid() && player.isLocal)) return;

        //Release player role if owned.
        if (player.GetPlayerTag("role") == portalRole) m_playerOwnsRole = false;
        //Assign player role otherwise.
        else m_playerOwnsRole = true;

        //Set player role tag
        player.SetPlayerTag("role", m_playerOwnsRole ? portalRole : null);

        //Update portal's capacity
        m_capacity = m_playerOwnsRole ? m_capacity-- : m_capacity++;
        
        SyncVariables();

        SetPortalMaterial();

        SetPortalAvailability();

        UpdateUI();

        //Send message to reflect role consequences. The Do/UndoAction methods must be public members of the ActionTarget script reference.
        string message = m_playerOwnsRole ? "DoAction" : "UndoAction";
        if (ActionTarget) ActionTarget.SendCustomEvent(message);

        RequestSerialization();
    }

    private void SyncVariables()
    {
        //Update synced variables
        if (!Networking.IsOwner(this.gameObject)) Networking.SetOwner(Networking.LocalPlayer, this.gameObject); //set script's ownership to local player
        sync_capacity = Mathf.Clamp(m_capacity, 0, MaxCapacity);
        //Sync local variables
        m_capacity = sync_capacity;
    }

    //Update synced variables for late joiners too.
    public override void OnDeserialization()
    {
        //Update local variable with synced values.
        m_capacity = sync_capacity;

        SetPortalAvailability();

        UpdateUI();
    }
    #endregion

    #region Custom Methods
    private void SetPortalMaterial()
    {
        if (portalGraphic == null) return;

        portalGraphic.GetComponent<MeshRenderer>().material= m_playerOwnsRole ? releaseRoleMat: getRoleMat;  
    }
    private void UpdateUI()
    {
        if (UITargets==null || UITargets.Length==0) return;

        string action = m_playerOwnsRole ? "Release" : "Get";
        string message = m_portalEnabled ? string.Format("{0} rol: {1}.  Capacity: {2}  ", action, portalRole, m_capacity): ".…………….Out-of-use………………….";
        

        foreach(var target in UITargets)
        {
            target.SetProgramVariable("originalMessage", message);
        }
        

        //if (roleLabel == null || capacityLabel == null) return; //TODELETE 

        //roleLabel.text = string.Format("{0} Rol: {1}", action, portalRole); //TODELETE
        //capacityLabel.text = "Portal capacity: " + m_capacity.ToString(); //TODELETE
    }
    private void SetPortalAvailability()
    {
        if (portalGraphic == null) return;

        //Enable portal if capacity is not saturated or player owns role.
        m_portalEnabled = m_capacity > 0 || m_playerOwnsRole;

        portalGraphic.SetActive(m_portalEnabled); //hide/show graphic
        this.GetComponent<Collider>().enabled=m_portalEnabled; //enable/disable interactions with portal
    }
    #endregion
}
