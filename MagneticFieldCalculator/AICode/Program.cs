using System;
using System.Numerics;

public class CubicMagnetFieldCalculator
{
    private const double GaussToTesla = 1e-4;
    private double SurfaceFieldTesla;
    private Vector3 MagnetSize; // Magnet dimensions in meters
    private Vector2 MagnetPosition;

    public CubicMagnetFieldCalculator(double surfaceFieldGauss, Vector3 sizeInMeters, Vector2 position)
    {
        SurfaceFieldTesla = surfaceFieldGauss * GaussToTesla;
        MagnetSize = sizeInMeters;
        MagnetPosition = position;
    }

    public Vector2 CalculateFieldVector(Vector2 point)
    {
        var r = point - MagnetPosition;

        // Magnet half-dimensions
        double a = MagnetSize.X / 2, b = MagnetSize.Y / 2;

        // Compute field using an analytical approximation

        return new Vector2(
            (float)(SurfaceFieldTesla * (ComputeComponent(r.X, a) * ComputeComponent(r.Y, b))),
            (float)(SurfaceFieldTesla * (ComputeComponent(r.Y, b) * ComputeComponent(r.X, a))));
    }

    private double ComputeComponent(double r, double dimension)
    {
        return Math.Log(Math.Abs(r + dimension) / Math.Abs(r - dimension)); // Approximate field component
    }
}

class Program
{
    static void Main()
    {
        var magnetPosition = new Vector2(0, 0);
        var magnetSize = new Vector3(0.25f, 0.25f, 0.25f); // 0.25 inch cube in meters
        var calculator = new CubicMagnetFieldCalculator(4600, magnetSize, magnetPosition);


        var testPoint = new Vector2(0.002f, 0.002f); // Test point constrained to Z = 0
        var fieldVector = calculator.CalculateFieldVector(testPoint);

        Console.WriteLine($"Magnetic Field at {testPoint}: {fieldVector} Tesla");
    }
}

using System;
using System.Numerics;

public class MagneticFieldCalculator
{
    private const double GaussToTesla = 1e-4;
    private const double Mu0 = 4 * Math.PI * 1e-7; // Permeability of free space in T·m/A
    private double SurfaceFieldTesla;
    private Vector3 MagnetSize;
    private Vector3 MagnetPosition;

    public MagneticFieldCalculator(double surfaceFieldGauss, Vector3 sizeInMeters, Vector3 position)
    {
        SurfaceFieldTesla = surfaceFieldGauss * GaussToTesla;
        MagnetSize = sizeInMeters;
        MagnetPosition = position;
    }

    public Vector3 CalculateFieldVector(Vector3 point)
    {
        Vector3 r = point - MagnetPosition;
        double x = r.X, y = r.Y, z = r.Z;

        // Distance from the magnet center
        double distance = r.Length();

        // Magnet half-dimensions
        double a = MagnetSize.X / 2, b = MagnetSize.Y / 2, c = MagnetSize.Z / 2;

        // Hybrid model: Rectangular for near-field, dipole for mid-range
        double fieldScalingFactor = (distance < 0.005) ? ComputeRectangularField(x, y, a, b) : ComputeDipoleField(distance);

        Vector3 fieldVector = new Vector3((float)(SurfaceFieldTesla * fieldScalingFactor), 0, 0); // Poles along X-axis

        return fieldVector;
    }

    private double ComputeRectangularField(double x, double y, double a, double b)
    {
        // Approximate near-field solution using logarithmic correction
        return Math.Log(Math.Abs(x + a) / Math.Abs(x - a)) * Math.Log(Math.Abs(y + b) / Math.Abs(y - b));
    }

    private double ComputeDipoleField(double distance)
    {
        // Magnetic dipole approximation for mid-range distances
        return Mu0 / (4 * Math.PI * Math.Pow(distance, 3));
    }
}

class Program
{
    static void Main()
    {
        Vector3 magnetPosition = new Vector3(0, 0, 0);
        Vector3 magnetSize = new Vector3(0.00635f, 0.00635f, 0.00635f);
        MagneticFieldCalculator calculator = new MagneticFieldCalculator(4000, magnetSize, magnetPosition);

        Vector3 testPoint = new Vector3(0.005f, 0.002f, 0.0f); // Test point at Z = 0
        Vector3 fieldVector = calculator.CalculateFieldVector(testPoint);

        Console.WriteLine($"Magnetic Field at {testPoint}: {fieldVector} Tesla");
    }
}