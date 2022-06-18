
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

/// <summary>
/// Enables/Disables & shows/hides interactables for local players with fxroles.
/// </summary>
public class FxRoleConfigurator : UdonSharpBehaviour
{
    [SerializeField] GameObject[] targets;

    private GameObject graphicsContainer;

    private void Start()
    {
        if (targets.Length == 0) return;

        foreach (var target in targets)
        {
            SetTargetState(target, false);
        }
    }

    public void DoAction()
    {
       if (targets.Length == 0) return;

       foreach(var target in targets)
        {
            SetTargetState(target, true);
        }
    }
    public void UndoAction()
    {
        if (targets.Length == 0) return;

        foreach (var target in targets)
        {
            SetTargetState(target,false);
        }
    }

    private void SetTargetState(GameObject target, bool state)
    {
        //Disable interaction
        target.GetComponent<Collider>().enabled = state;

        //Turn interactable invisible 
        graphicsContainer = target.transform.GetChild(0).gameObject;
        graphicsContainer.SetActive(state);
    }
}
