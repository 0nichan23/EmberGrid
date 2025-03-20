using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class UnitSelector : MonoBehaviour
{

    [SerializeField] private Unit unit;

    private void OnMouseDown()
    {
        GameManager.Instance.SelectionManager.SelectUnit(unit);
    }




}
