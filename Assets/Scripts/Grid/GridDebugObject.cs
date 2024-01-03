using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    private object gridObject;
    [SerializeField] private TextMeshPro textDebug;

    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;        
    }

    protected virtual void Update()
    {
        textDebug.text = gridObject.ToString();
    }
   
}
