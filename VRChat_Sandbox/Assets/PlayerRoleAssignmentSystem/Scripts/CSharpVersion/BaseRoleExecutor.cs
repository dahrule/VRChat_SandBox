using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseRoleExecutor : MonoBehaviour
{
    [SerializeField] protected GameObject[] targets;

    protected GameObject graphicsContainer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public abstract void DoAction();
    public abstract void UndoAction();
}