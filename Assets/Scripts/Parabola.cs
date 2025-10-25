using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabola : MonoBehaviour
{
    // Devuelve Vector3.zero si no hay soluci�n real con la velocidad dada.
    public static Vector3 CalculateLaunchVelocity(Vector3 origin, Vector3 target, float speed)
    {
        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);

        float y = toTarget.y;
        float xz = toTargetXZ.magnitude;

        if (xz < 0.0001f) return Vector3.zero; // evita divisi�n por 0

        float g = Mathf.Abs(Physics.gravity.y); // gravedad positiva
        float v2 = speed * speed;
        float v4 = v2 * v2;

        // discriminante de la ecuaci�n para el �ngulo
        float insideSqrt = v4 - g * (g * xz * xz + 2f * y * v2);

        if (insideSqrt < 0f)
            return Vector3.zero; // no hay soluci�n con esa velocidad

        float sqrt = Mathf.Sqrt(insideSqrt);

        // dos posibles �ngulos (alto y bajo)
        float angleLow = Mathf.Atan2(v2 - sqrt, g * xz);
        float angleHigh = Mathf.Atan2(v2 + sqrt, g * xz);

        // eleg� la trayectoria que quieras: high = arc alto, low = m�s directo
        float chosenAngle = angleHigh; // cambi� a angleLow si quer�s tiro m�s plano

        // direcci�n horizontal normalizada
        Vector3 dir = toTargetXZ.normalized;

        // componemos la velocidad inicial
        Vector3 velocity = dir * (speed * Mathf.Cos(chosenAngle)) + Vector3.up * (speed * Mathf.Sin(chosenAngle));
        return velocity;
    }
}
