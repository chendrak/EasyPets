using System;
using UnityEngine;

namespace EasyPets.Helpers;

public static class GravityHelper
{
    public static Vector3 GetRequiredForce(
        Vector3 startingPosition,
        Vector3 targetPosition,
        float gravity = 9.8f)
    {
        Vector3 vector3_1 = targetPosition - startingPosition;
        float y = Vector2.SignedAngle(Vector2.right, new Vector2(vector3_1.x, vector3_1.z));
        float magnitude = (vector3_1 with { y = 0.0f }).magnitude;
        Vector3 vector3_2 = Quaternion.Euler(0.0f, y, 0.0f) * vector3_1;
        float num1 = Vector2.SignedAngle(Vector2.right, new Vector2(vector3_2.x, vector3_2.y));
        double f = (num1 < 0.0 ? 45.0 : 90.0 - (90.0 - num1) * 0.5) * (Math.PI / 180.0);
        float num2 = Mathf.Cos((float) f);
        float num3 = Mathf.Sin((float) f);
        float a = (float) (2.0 * num2 * ((double) num3 * (double) magnitude - (double) num2 * (double) vector3_1.y));
        float num4 = gravity * magnitude * magnitude;
        if (Mathf.Approximately(a, 0.0f) || (double) a < 0.0)
            return new Vector3(0.0f, 0.0f, 0.0f);
        float num5 = Mathf.Sqrt(num4 / a);
        return Quaternion.Euler(0.0f, -y, 0.0f) * new Vector3(num2 * num5, num3 * num5, 0.0f);
    }

}