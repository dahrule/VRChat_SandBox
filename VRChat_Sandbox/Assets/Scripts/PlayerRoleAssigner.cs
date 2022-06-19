
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
    [Tooltip("Role that the portal provides.")]
    [SerializeField] string portalRole;

    [Tooltip("Maximum number of users that can own this role.")]
    [SerializeField] int MaxCapacity = 0;

    [Header("UI")]
    [SerializeField] Text capacityLabel;
    [SerializeField] Text roleLabel;

    [Header("Graphics")]
    [SerializeField] GameObject portalGraphic;
    [SerializeField] Material getRoleMat;
    [SerializeField] Material releaseRoleMat;

    [Header("ActionsTriggered by this role")]
    [SerializeField] UdonSharpBehaviour ActionTarget;

    private int m_capacity;
    [UdonSynced] private int sync_capacity;
    private bool m_playerOwnsRole = false;
    #endregion

    #region Built-in Methods
    void Start()
    {
        m_capacity = MaxCapacity;
        sync_capacity = m_capacity;

        UpdateUI();

        SetPortalMaterial();

        SetPortalAvailability();
    }
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (!(player.IsValid() && player.isLocal)) return;

        //Release player role if owned.
        if (player.GetPlayerTag("role") == portalRole) m_playerOwnsRole = false;
        //Assign player role otherwise.
        else m_playerOwnsRole = true;

        //Update portal's capacity
        m_capacity = m_playerOwnsRole ? m_capacity-- : m_capacity++;
        //Update synced variables
        if (!Networking.IsOwner(this.gameObject)) Networking.SetOwner(Networking.LocalPlayer, this.gameObject); //set script's ownership to local player
        sync_capacity = Mathf.Clamp(m_capacity, 0, MaxCapacity);
        //Synced local variables
        m_capacity = sync_capacity;

        //Set player role tag
        player.SetPlayerTag("role", m_playerOwnsRole ? portalRole : null);

        SetPortalMaterial();

        SetPortalAvailability();

        UpdateUI();

        //Send message to reflect role consequences.
        string message = m_playerOwnsRole ? "DoAction" : "UndoAction";
        if (ActionTarget) ActionTarget.SendCustomEvent(message);

        RequestSerialization();
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
        string action = m_playerOwnsRole ? "Release" : "Get";
        string role= portalRole;

        roleLabel.text = string.Format("{0} Rol: {1}", action, role);
        capacityLabel.text = "Portal capacity: " + m_capacity.ToString();
    }
    private void SetPortalAvailability()
    {
        //Enable portal if capacity is not saturated or player owns role.
        bool enabled = m_capacity > 0 || m_playerOwnsRole;

        portalGraphic.SetActive(enabled); //hide/show graphic
        this.GetComponent<Collider>().enabled=enabled; //enable/disable interactions with portal
    }
    #endregion
}
