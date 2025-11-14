using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionEffect : ScriptableObject
{
    public abstract void InvokeEffect(Unit target, Unit User);

    public abstract void InvokeDisplayEffect(Unit target, Unit User);
}
