using System;

class MagneticFieldCalculator
{
    // Constants
    const double MU_0 = 4 * Math.PI * 1e-7;  // Permeability of free space (T*in/A)
    const double DEG_TO_RAD = Math.PI / 180.0; // Conversion factor (degrees to radians)
    const double RAD_TO_DEG = 180.0 / Math.PI; // Conversion factor (radians to degrees)

    // Compute magnetization from surface field strength (Tesla)
    static double ComputeMagnetization(double B_surface)
    {
        return B_surface / MU_0;
    }

    // Convert polar coordinates to Cartesian coordinates
    static (double x, double y) ConvertPolarToCartesian(double r, double theta)
    {
        double thetaRad = theta * DEG_TO_RAD;
        return (r * Math.Cos(thetaRad), r * Math.Sin(thetaRad));
    }

    // Convert Cartesian coordinates to polar (magnitude and angle)
    static (double r, double theta) ConvertCartesianToPolar(double x, double y)
    {
        double r = Math.Sqrt(x * x + y * y);
        double theta = Math.Atan2(y, x) * RAD_TO_DEG;
        return (r, theta);
    }

    // Convert dipole orientation from polar to Cartesian vector
    static (double mx, double my) ConvertDipoleOrientation(double magnitude, double angle)
    {
        double angleRad = angle * DEG_TO_RAD;
        return (magnitude * Math.Cos(angleRad), magnitude * Math.Sin(angleRad));
    }

    // Compute magnetic field considering cubic magnet size
    static (double Bx, double By) ComputeCubicMagnetField(double x, double y,
                                                          double mx, double my,
                                                          double theta, double size)
    {
        // Convert magnet position from polar to Cartesian
        var (x0, y0) = ConvertPolarToCartesian(size / 2, theta);

        // Compute position vector relative to magnet center
        double rx = x - x0;
        double ry = y - y0;
        double r = Math.Sqrt(rx * rx + ry * ry);

        // Ensure calculation doesn't break near the magnet
        if (r < size / 2) r = size / 2; // Adjust minimum possible distance

        // Factor including cubic magnet correction
        double factor = (MU_0 / (4 * Math.PI)) / Math.Pow(r, 3);
        double dotProduct = mx * rx + my * ry;
        double Bx = factor * (3 * dotProduct * rx - mx);
        double By = factor * (3 * dotProduct * ry - my);

        return (Bx, By);
    }

    // Compute total field from two equal cubic magnets at a given evaluation point
    static (double B_r, double theta_B) ComputeTotalField(double r_eval, double theta_eval,
                                                          double B_surface, double radius, double theta1, double angle1,
                                                          double theta2, double angle2, double size)
    {
        // Convert evaluation point from polar to Cartesian
        var (x_eval, y_eval) = ConvertPolarToCartesian(r_eval, theta_eval);

        // Compute magnetization (same for both magnets)
        double m = ComputeMagnetization(B_surface);

        // Convert dipole orientations from polar to Cartesian
        var (mx1, my1) = ConvertDipoleOrientation(m, angle1);
        var (mx2, my2) = ConvertDipoleOrientation(m, angle2);

        // Compute individual cubic magnet fields (both magnets share the same radius)
        var (Bx1, By1) = ComputeCubicMagnetField(x_eval, y_eval, mx1, my1, theta1, size);
        var (Bx2, By2) = ComputeCubicMagnetField(x_eval, y_eval, mx2, my2, theta2, size);

        // Sum the field contributions from both magnets
        double Bx_total = Bx1 + Bx2;
        double By_total = By1 + By2;

        // Convert final result to polar form (magnitude and angle)
        var (B_r, theta_B) = ConvertCartesianToPolar(Bx_total, By_total);
        return (B_r, theta_B);
    }

    static void Main()
    {
        // Define common properties of both magnets
        double B_surface = 4600;  // Surface field strength in Tesla
        double size = 2.0;        // Magnet size in inches
        double radius = 19.69;    // Common radius for both magnets

        // Define positions (only angles vary since radius is fixed)
        double theta1 = 0;    // Magnet 1 at angle 0° (aligned with x-axis)
        double theta2 = 90;   // Magnet 2 at angle 90° (aligned with y-axis)

        // Define dipole orientation angles (degrees)
        double angle1 = 0;   // Magnet 1 dipole moment pointing along x-axis
        double angle2 = 90;  // Magnet 2 dipole moment pointing along y-axis

        // Define evaluation point in polar coordinates (radius in inches, angle in degrees)
        double r_eval = 11.81, theta_eval = 45; // Example point at 11.81 inches, 45°

        // Compute total magnetic field at the given polar coordinates
        var (B_r, theta_B) = ComputeTotalField(r_eval, theta_eval, B_surface, radius, theta1, angle1,
                                               theta2, angle2, size);

        // Output results
        Console.WriteLine($"Magnetic field at (r = {r_eval:F2} in, θ = {theta_eval:F2}°):");
        Console.WriteLine($"  B_r = {B_r:E} T (Magnitude)");
        Console.WriteLine($"  θ_B = {theta_B:F2}° (Direction)");
    }
}