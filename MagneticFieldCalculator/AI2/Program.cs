using System;


/*
 c# code to compute the magnetic field vector around a rectangular magnet of a specified size and surface field,
having poles aligned to the long axis, which is coincident with the x axis. 
use rectangular magnet model and consider only locations in x y plane. 
use inches for linear dimension and gauss for field strength
 */
namespace AI2
{
    class MagneticFieldCalculator
    {
        // Constants
        const double MU_0 = 4 * Math.PI * 1e-7; // Permeability of free space (T·m/A)
        const double GAUSS_TO_TESLA = 1e-4;     // Conversion factor from Gauss to Tesla
        const double INCH_TO_METER = 0.0254;    // Conversion factor from inches to meters

        // Magnet properties (User-defined)
        static double surfaceField_Gauss = 500; // Surface field strength in Gauss
        static double length_Inches = 4;        // Length of the rectangular magnet in inches
        static double width_Inches = 2;         // Width of the magnet in inches

        // Computed properties
        static double M = surfaceField_Gauss * GAUSS_TO_TESLA / MU_0; // Magnetization (A/m)
        static double a_x = (length_Inches / 2) * INCH_TO_METER;      // Half-length in meters
        static double a_y = (width_Inches / 2) * INCH_TO_METER;       // Half-width in meters

        static void Main()
        {
            // Test point in the x-y plane (in inches)
            double x_inches = 5;
            double y_inches = 3;

            // Convert test point to meters
            double x = x_inches * INCH_TO_METER;
            double y = y_inches * INCH_TO_METER;

            // Compute the magnetic field
            (double Bx, double By) = ComputeMagneticField(x, y);
            Console.WriteLine($"Magnetic Field at ({x_inches} in, {y_inches} in): Bx = {Bx} T, By = {By} T");
        }

        static (double, double) ComputeMagneticField(double x, double y)
        {
            double Bx = 0, By = 0;

            // Sum contributions from magnetized faces along the x-axis
            for (int sx = -1; sx <= 1; sx += 2)
            {
                for (int sy = -1; sy <= 1; sy += 2)
                {
                    double xp = x - sx * a_x; // Position relative to magnet face
                    double yp = y - sy * a_y; // Position relative to magnet face
                    double rSquared = xp * xp + yp * yp;
                    double rCubed = Math.Pow(rSquared, 1.5);

                    if (rCubed != 0)
                    {
                        Bx += MU_0 * M * xp / (4 * Math.PI * rCubed); // Field component along x-axis
                        By += MU_0 * M * yp / (4 * Math.PI * rCubed); // Field component along y-axis
                    }
                }
            }

            return (Bx, By);
        }
    }
}
