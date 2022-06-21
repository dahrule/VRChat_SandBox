
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

/// <summary>
/// Defines the consequences of owning the role: DoorOpener
/// Consequences: Players who own the role can open/close target doors.
///!Set Synchronization Method to Manual in Editor.
/// </summary>
public class DoorOpenerRoleExecutor : UdonSharpBehaviour
{
    [SerializeField] GameObject [] targets;

    //private GameObject graphicsContainer;

    void Start()
    {
        if (targets.Length == 0) return;

        foreach (var target in targets)
        {
            SetTargetState(target, false);
        }
    }

    //Response to event sent by PlayerRoleAssigenr class.
    public void DoAction()
    {
        if (targets.Length == 0) return;

        foreach (var target in targets)
        {
            SetTargetState(target, true);
        }
    }

    //Response to event sent by PlayerRoleAssigenr class.
    public void UndoAction()
    {
        if (targets.Length == 0) return;

        foreach (var target in targets)
        {
            SetTargetState(target, false);
        }
    }

    private void SetTargetState(GameObject target, bool state)
    {
        if (target == null) return;

        //Disable interaction
        target.GetComponent<Collider>().enabled = state;

        //Turn interactable invisible 
        //graphicsContainer = target.transform.GetChild(0).gameObject;
        //graphicsContainer.SetActive(state);
    }
}
