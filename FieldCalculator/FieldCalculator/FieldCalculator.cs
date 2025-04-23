using System;

class MagneticFieldCalculator
{
    // Constants
    const double MU_0 = 4 * Math.PI * 1e-7;  // Permeability of free space (T*in/A)
    const double DEG_TO_RAD = Math.PI / 180.0; // Conversion factor (degrees to radians)

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
        var (x0, y0) = ConvertPolarToCartesian(size / 2, theta);

        double rx = x - x0;
        double ry = y - y0;
        double r = Math.Sqrt(rx * rx + ry * ry);
        if (r < size / 2) r = size / 2;

        double factor = (MU_0 / (4 * Math.PI)) / Math.Pow(r, 3);
        double dotProduct = mx * rx + my * ry;
        double Bx = factor * (3 * dotProduct * rx - mx);
        double By = factor * (3 * dotProduct * ry - my);

        return (Bx, By);
    }

    // Compute total field strength component aligned with theta_eval
    static double ComputeFieldAlignedWithTheta(double r_eval, double theta_eval,
                                               double B_surface, double radius, double theta1, double size)
    {
        double theta2 = -theta1;
        double angle1 = theta1;
        double angle2 = theta2 + 180.0;

        var (x_eval, y_eval) = ConvertPolarToCartesian(r_eval, theta_eval);

        double m = ComputeMagnetization(B_surface);
        var (mx1, my1) = ConvertDipoleOrientation(m, angle1);
        var (mx2, my2) = ConvertDipoleOrientation(m, angle2);

        var (Bx1, By1) = ComputeCubicMagnetField(x_eval, y_eval, mx1, my1, theta1, size);
        var (Bx2, By2) = ComputeCubicMagnetField(x_eval, y_eval, mx2, my2, theta2, size);

        double Bx_total = Bx1 + Bx2;
        double By_total = By1 + By2;

        // Project total field onto theta_eval direction
        double theta_eval_rad = theta_eval * DEG_TO_RAD;
        double B_aligned = Bx_total * Math.Cos(theta_eval_rad) + By_total * Math.Sin(theta_eval_rad);

        return B_aligned;
    }

    static void Main()
    {
        double B_surface = 4600;
        double size = .25;
        double radius = 1;
        double theta1 = 30;
        double r_eval = (double)9/16;

        for (double theta_eval = -30; theta_eval < 30; theta_eval+=5)
        {
            double B_aligned = ComputeFieldAlignedWithTheta(r_eval, theta_eval, B_surface, radius, theta1, size);

            Console.WriteLine($"{theta_eval:F2}°: {B_aligned:E}");
        }
    }
}