using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class TilePrefab : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer;

    public UnityEvent OnUnitStep;
    public UnityEvent OnUnitLand;
    //<Unit>
    private Color startcolor;

    private void Start()
    {
        startcolor = spriteRenderer.color;
    }

    private void OnMouseDown()
    {
        spriteRenderer.color = new Color(startcolor.r, startcolor.g, startcolor.b, 0.33f);
    }

    private void OnMouseUp()
    {
        spriteRenderer.color = startcolor;
        Debug.Log(gameObject.name);
    }
}
