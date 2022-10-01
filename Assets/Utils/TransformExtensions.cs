using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static void LookAt2D(this Transform transform, Vector3 target)
    {
        transform.right = target - transform.position;
    }
}
