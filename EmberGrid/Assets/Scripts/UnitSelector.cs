using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class UnitSelector : MonoBehaviour
{
    [SerializeField] private Unit unit;
    private bool selectable;
    public bool Selectable { get => selectable; set => selectable = value; }

    private void OnMouseDown()
    {
        if (selectable)
        GameManager.Instance.SelectionManager.SelectUnit(unit);
    }




}
