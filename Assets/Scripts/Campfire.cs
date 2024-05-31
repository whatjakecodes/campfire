using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    
    [SerializeField] private Transform m_PivotTarget;

    private void Awake()
    {
        var pivotPosition = m_PivotTarget.transform.position;
        var offset = transform.position - pivotPosition;
        foreach (Transform child in transform)
            child.transform.position += offset;
        transform.position = pivotPosition;
    }

}
