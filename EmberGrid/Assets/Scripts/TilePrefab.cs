using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class TilePrefab : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color moveColor;
    [SerializeField] private Color attackRangeColor;
    [SerializeField] private Color attackTargetColor;

    public UnityEvent OnUnitStep;
    public UnityEvent OnUnitLand;
    public UnityEvent<TileSD> OnTileClicked;
    public UnityEvent OnTileHovered;
    public UnityEvent OnTileUnHovered;
    private Color startcolor;
    private TileSD refTileSD;



    public SpriteRenderer SpriteRenderer { get => spriteRenderer; }


    public void CacheSD(TileSD givenSD)
    {
        refTileSD = givenSD;
    }

    private void Start()
    {
        startcolor = spriteRenderer.color;
    }

    private void OnMouseDown()
    {
        spriteRenderer.color = new Color(startcolor.r, startcolor.g, startcolor.b, 0.33f);
        OnTileClicked?.Invoke(refTileSD);
    }

    private void OnMouseUp()
    {
        spriteRenderer.color = startcolor;
        Debug.Log(gameObject.name);
    }


    private void OnMouseEnter()
    {
        OnTileHovered?.Invoke();   
    }

    private void OnMouseExit()
    {
        OnTileUnHovered?.Invoke();
    }

    public void RedBlink()
    {
        StartCoroutine(BlinkColor(startcolor, Color.red));
    }

    public void BlackBlink()
    {
        StartCoroutine(BlinkColor(startcolor, Color.black));
    }

    private IEnumerator BlinkColor(Color start, Color end)
    {
        float counter = 0;
        while (counter < 1)
        {
            counter += Time.deltaTime;
            
            float t = Mathf.PingPong(counter, 1f);
            SpriteRenderer.color = Color.Lerp(start, end, t);
            yield return null;
        }
        SpriteRenderer.color = start;
    }

    public void SetMoveMode()
    {
        SpriteRenderer.color = moveColor;
    }
    public void SetAttackMode()
    {
        AttackColor();
        OnTileHovered.AddListener(TargetOverlay);
        OnTileUnHovered.AddListener(AttackColor);
    }

    private void AttackColor()
    {
        SpriteRenderer.color = attackRangeColor;
    }

    public void TargetOverlay()
    {
        Debug.Log("Hovered");
        SpriteRenderer.color = attackTargetColor;
    }

   
    public void ResetOverlay()
    {
        SpriteRenderer.color = startcolor;
        OnTileHovered.RemoveListener(TargetOverlay);
        OnTileUnHovered.RemoveListener(AttackColor);
    }
}
