using System.Collections;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class TilePrefab : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color moveColor;
    [SerializeField] private Color moveSelectedColor;
    [SerializeField] private Color attackRangeColor;
    [SerializeField] private Color attackTargetColor;

    public UnityEvent OnUnitStep;
    public UnityEvent OnUnitLand;

    public UnityEvent OnTileClicked;
    public UnityEvent<TileSD> OnTileClickedRef;

    public UnityEvent OnTileHovered;
    public UnityEvent<TileSD> OnTileHoveredRef;

    public UnityEvent OnTileUnHovered;
    public UnityEvent<TileSD> OnTileUnHoveredRef;

    private Color startcolor;
    private TileSD refTileSD;



    public SpriteRenderer SpriteRenderer { get => spriteRenderer; }
    public TileSD RefTileSD { get => refTileSD; }

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
        OnTileClickedRef?.Invoke(refTileSD);
        OnTileClicked?.Invoke();
    }

    private void OnMouseUp()
    {
        spriteRenderer.color = startcolor;
        Debug.Log(gameObject.name);
    }


    private void OnMouseEnter()
    {
        OnTileHovered?.Invoke();
        OnTileHoveredRef?.Invoke(RefTileSD);
    }

    private void OnMouseExit()
    {
        OnTileUnHovered?.Invoke();
        OnTileUnHoveredRef?.Invoke(RefTileSD);
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
        while (counter < 2)
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
        MoveColor();
        OnTileHovered.AddListener(MovementOverlay);
        OnTileUnHovered.AddListener(MoveColor);
    }
    public void SetAttackMode()
    {
        AttackColor();
      /*  OnTileHovered.AddListener(TargetOverlay);
        OnTileUnHovered.AddListener(AttackColor);*/
    }

    private void AttackColor()
    {
        SpriteRenderer.color = attackRangeColor;
    }

    private void MoveColor()
    {
        SpriteRenderer.color = moveColor;
    }

    public void TargetOverlay()
    {
        SpriteRenderer.color = attackTargetColor;
    }

    public void MovementOverlay()
    {
        SpriteRenderer.color = moveSelectedColor;
    }

    public void ResetOverlay()
    {
        SpriteRenderer.color = startcolor;
/*        OnTileHovered.RemoveListener(TargetOverlay);
        OnTileUnHovered.RemoveListener(AttackColor);*/
        OnTileHovered.RemoveListener(MovementOverlay);
        OnTileUnHovered.RemoveListener(MoveColor);
    }
}
