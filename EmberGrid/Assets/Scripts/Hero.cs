using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Unit
{
    protected override void Events()
    {
        base.Events();
        OnSelected.AddListener(() => GameManager.Instance.UIManager.ActionBar.Setup(this));
    }
}
