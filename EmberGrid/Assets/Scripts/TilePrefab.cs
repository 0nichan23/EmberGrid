using System.Collections;
using System.Runtime.CompilerServices;
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

    public SpriteRenderer SpriteRenderer { get => spriteRenderer; }

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

}
