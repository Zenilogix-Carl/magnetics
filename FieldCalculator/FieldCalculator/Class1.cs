using System;

class MagneticDipoleCalculator
{
    // Constants
    const double MU_0 = 4 * Math.PI * 1e-7;  // Permeability of free space (T*in/A)

    // Compute magnetization from surface field strength (Tesla)
    static double ComputeMagnetization(double B_surface)
    {
        return B_surface / MU_0;
    }

    // Compute magnetic dipole field vector at any (x, y, z) location (in inches)
    static (double Bx, double By, double Bz) ComputeDipoleField(double x, double y, double z,
                                                                double mx, double my, double mz,
                                                                double x0, double y0, double z0)
    {
        // Compute position vector relative to magnet center
        double rx = x - x0;
        double ry = y - y0;
        double rz = z - z0;
        double r = Math.Sqrt(rx * rx + ry * ry + rz * rz);
        if (r == 0) return (0, 0, 0); // Avoid singularity at magnet center

        double factor = (MU_0 / (4 * Math.PI)) / Math.Pow(r, 3);
        double dotProduct = mx * rx + my * ry + mz * rz;
        double Bx = factor * (3 * dotProduct * rx - mx);
        double By = factor * (3 * dotProduct * ry - my);
        double Bz = factor * (3 * dotProduct * rz - mz);

        return (Bx, By, Bz);
    }

    // Compute total dipole field from two magnets at (x, y, z) in inches
    static (double Bx, double By, double Bz) ComputeTotalField(double x, double y, double z,
                                                               double B_surface1, double x1, double y1, double z1,
                                                               double mx1, double my1, double mz1,
                                                               double B_surface2, double x2, double y2, double z2,
                                                               double mx2, double my2, double mz2)
    {
        // Compute individual magnetizations
        double m1 = ComputeMagnetization(B_surface1);
        double m2 = ComputeMagnetization(B_surface2);

        // Compute individual dipole fields
        var (Bx1, By1, Bz1) = ComputeDipoleField(x, y, z, mx1 * m1, my1 * m1, mz1 * m1, x1, y1, z1);
        var (Bx2, By2, Bz2) = ComputeDipoleField(x, y, z, mx2 * m2, my2 * m2, mz2 * m2, x2, y2, z2);

        // Sum the field contributions from both magnets
        return (Bx1 + Bx2, By1 + By2, Bz1 + Bz2);
    }

    static void Main()
    {
        // Define positions of two magnets (in inches)
        double x1 = -19.69, y1 = 0, z1 = 0;  // Magnet 1 center (0.5 meters converted)
        double x2 = 19.69, y2 = 0, z2 = 0;   // Magnet 2 center (0.5 meters converted)

        // Define surface field strengths in Tesla
        double B_surface1 = 4600;  // Magnet 1 surface field strength
        double B_surface2 = 4600;  // Magnet 2 surface field strength

        // Define orientation of magnets (dipole moments normalized to unit vectors)
        double mx1 = 1, my1 = 0, mz1 = 0;  // Magnet 1 aligned with x-axis (North-South poles)
        double mx2 = 0, my2 = 1, mz2 = 0;  // Magnet 2 aligned with y-axis

        // Define point of evaluation (in inches)
        double px = 0.0, py = 11.81, pz = 3.94;  // Example point in space (converted from meters)

        // Compute total magnetic field at the point
        var (Bx, By, Bz) = ComputeTotalField(px, py, pz, B_surface1, x1, y1, z1, mx1, my1, mz1,
                                             B_surface2, x2, y2, z2, mx2, my2, mz2);

        // Output results
        Console.WriteLine($"Magnetic field at ({px:F2} in, {py:F2} in, {pz:F2} in):");
        Console.WriteLine($"  Bx = {Bx:E} T");
        Console.WriteLine($"  By = {By:E} T");
        Console.WriteLine($"  Bz = {Bz:E} T");
    }
}