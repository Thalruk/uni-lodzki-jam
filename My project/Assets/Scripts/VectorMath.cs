using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorMath
{
    public static Vector2 Rotate(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
        float tx = v.x;
        float ty = v.y;
        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }

    public static float AngleBetween(Vector2 from, Vector2 to)
    {
        //float dot = Vector2.Dot(from.normalized, to.normalized);
        //return Mathf.Acos(dot) * Mathf.Rad2Deg;

        return Vector3.SignedAngle(from, to, Vector3.forward);
    }
}
