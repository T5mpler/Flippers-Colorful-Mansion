using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ActivityModifierScript : MonoBehaviour
{
    public List<MoveModifier> movementModList
    {
        get { return moveModifiers; }
    }
    public Vector3 TotalAdder
    {
        get
        {
            Vector3 v3 = Vector3.zero;
            foreach (MoveModifier moveMod in moveModifiers)
            {
                v3 += moveMod.adder;
            }
            return v3;
        }
    }
    public float TotalMultipler
    {
        get
        {
            float multipler = 1f;
            foreach (MoveModifier moveMod in moveModifiers)
            {
                multipler *= moveMod.multipler;
            }
            return multipler;
        }
    }
    List<MoveModifier> moveModifiers = new List<MoveModifier>();
}
