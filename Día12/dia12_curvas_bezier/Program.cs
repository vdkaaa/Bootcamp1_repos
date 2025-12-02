using System;
using System.Numerics;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Día 12: Curvas Bézier ===\n");

        #region Kata1 - Bézier lineal (equivalente a Lerp)

        Vector2 p0 = new Vector2(0, 0);
        Vector2 p1 = new Vector2(10, 0);

        Console.WriteLine("Kata1: Bézier lineal entre (0,0) y (10,0)");
        for (float t = 0; t <= 1.001f; t += 0.25f)
        {
            Vector2 point = BezierLinear(p0, p1, t);
            Console.WriteLine($"t={t:F2} -> {point}");
        }

        Console.WriteLine();
        #endregion

        #region Kata2 - Bézier cuadrática con De Casteljau

        // P0 = inicio, P1 = control, P2 = final
        Vector2 q0 = new Vector2(0, 0);
        Vector2 q1 = new Vector2(5, 5);   // punto de control que levanta el arco
        Vector2 q2 = new Vector2(10, 0);

        Console.WriteLine("Kata2: Bézier cuadrática (0,0) -> (5,5) -> (10,0)");
        for (float t = 0; t <= 1.001f; t += 0.25f)
        {
            Vector2 point = BezierQuadratic(q0, q1, q2, t);
            Console.WriteLine($"t={t:F2} -> {point}");
        }

        Console.WriteLine();
        #endregion

        #region Kata3 - Muestrear muchos puntos de la cuadrática (trayectoria)

        Console.WriteLine("Kata3: Muestreo fino de la curva cuadrática");
        int steps = 10;
        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps;
            Vector2 point = BezierQuadratic(q0, q1, q2, t);
            Console.WriteLine($"t={t:F2} -> {point}");
        }

        Console.WriteLine();
        #endregion

        #region Kata4 - Bézier cúbica

        // Cúbica con dos puntos de control
        Vector2 c0 = new Vector2(0, 0);
        Vector2 c1 = new Vector2(3, 6);
        Vector2 c2 = new Vector2(7, 6);
        Vector2 c3 = new Vector2(10, 0);

        Console.WriteLine("Kata4: Bézier cúbica con 2 controles");
        for (float t = 0; t <= 1.001f; t += 0.2f)
        {
            Vector2 point = BezierCubic(c0, c1, c2, c3, t);
            Console.WriteLine($"t={t:F2} -> {point}");
        }

        Console.WriteLine();
        #endregion

        #region Mini-Challenge - PathFollower sobre una curva Bezier (arco tipo proyectil)

        Console.WriteLine("Mini-Challenge: seguidor sobre trayectoria Bézier cuadrática\n");

        // Imagina un proyectil que sale de (0,0), sube, y cae en (10,0)
        Vector2 start = new Vector2(0, 0);
        Vector2 control = new Vector2(5, 8);  // altura del arco
        Vector2 end = new Vector2(10, 0);

        float totalTime = 2.0f;     // 2 segundos de “vuelo”
        int frames = 20;            // simulamos 20 pasos
        float dt = totalTime / frames;

        float elapsed = 0f;
        for (int i = 0; i <= frames; i++)
        {
            float t = elapsed / totalTime;
            if (t > 1f) t = 1f;

            Vector2 position = BezierQuadratic(start, control, end, t);
            Console.WriteLine(
                $"time={elapsed:F2}s (t={t:F2}) -> pos={position}");

            elapsed += dt;
        }

        Console.WriteLine("\nFin Día 12.");
        #endregion
    }

    // ----------------------
    // Helpers
    // ----------------------

    static float Lerp(float a, float b, float t)
        => a + (b - a) * t;

    static Vector2 LerpVector2(Vector2 a, Vector2 b, float t)
        => new Vector2(
            Lerp(a.X, b.X, t),
            Lerp(a.Y, b.Y, t)
        );

    // Bézier lineal: idéntico a un Lerp entre dos posiciones
    static Vector2 BezierLinear(Vector2 p0, Vector2 p1, float t)
    {
        return LerpVector2(p0, p1, t);
    }

    // Bézier cuadrática usando De Casteljau:
    // 1) q0 = Lerp(p0, p1, t)
    // 2) q1 = Lerp(p1, p2, t)
    // 3) B(t) = Lerp(q0, q1, t)
    static Vector2 BezierQuadratic(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        Vector2 q0 = LerpVector2(p0, p1, t);
        Vector2 q1 = LerpVector2(p1, p2, t);
        return LerpVector2(q0, q1, t);
    }

    // Bézier cúbica con De Casteljau:
    // q0 = Lerp(p0, p1, t)
    // q1 = Lerp(p1, p2, t)
    // q2 = Lerp(p2, p3, t)
    // r0 = Lerp(q0, q1, t)
    // r1 = Lerp(q1, q2, t)
    // B(t) = Lerp(r0, r1, t)
    static Vector2 BezierCubic(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        Vector2 q0 = LerpVector2(p0, p1, t);
        Vector2 q1 = LerpVector2(p1, p2, t);
        Vector2 q2 = LerpVector2(p2, p3, t);

        Vector2 r0 = LerpVector2(q0, q1, t);
        Vector2 r1 = LerpVector2(q1, q2, t);

        return LerpVector2(r0, r1, t);
    }
}
