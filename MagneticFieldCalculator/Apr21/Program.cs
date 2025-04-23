using System;
using System.Numerics;

namespace Apr21;

class MagneticFieldCalculator
{
    const double Mu0 = 4 * Math.PI * 1e-7;

    public static Vector3 CalculateField(Vector3 point, Vector3 magnetCenter, Vector3 size, double surfaceField)
    {
        Vector3 field = Vector3.Zero;
        double magnetization = surfaceField / Mu0;

        // Loop over edges and vertices for enhanced accuracy
        for (int i = -1; i <= 1; i += 2)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                for (int k = -1; k <= 1; k += 2)
                {
                    Vector3 vertex = magnetCenter + new Vector3(i * size.X / 2, j * size.Y / 2, k * size.Z / 2);
                    Vector3 r = point - vertex;
                    double distance = r.Length();

                    if (distance < 1e-6) distance = 1e-6;

                    field += Vector3.Multiply((float)(magnetization / (distance * distance)), Vector3.Normalize(r));

                    // Edge corrections: additional weighting based on proximity to edges
                    Vector3 edgeFactor = new Vector3(i / size.X, j / size.Y, k / size.Z);
                    field += Vector3.Multiply((float)(magnetization / (distance * distance * 2)), edgeFactor);
                }
            }
        }

        return field;
    }

    static void Main()
    {
        Vector3 magnetCenter = new Vector3(0, 0, 0);
        Vector3 size = new Vector3(0.25F, 0.25F, 0.25F);
        double surfaceField = 4601.0/10000.0;

        Vector3 point = new Vector3(0.125F + 0.3F, 0, 0);
        Vector3 field = CalculateField(point, magnetCenter, size, surfaceField);

        Console.WriteLine($"Magnetic Field at ({point.X}, {point.Y}, {point.Z}): {field}");
    }
}