/*
 c# code to compute the magnetic field vector around a cubic magnet of a specified
size and surface field, having poles and faces aligned on x axis
using rectangular magnet model
consider only locations in x y plane.
consider distances as close as 1/2 magnet width. 
use inches for linear dimension and gauss for field strength
   
 */
namespace AI3
{
    using System;

    class MagneticFieldCalculator
    {
        // Constants
        const double MU_0 = 4 * Math.PI * 1e-7; // Permeability of free space (T·m/A)
        const double GAUSS_TO_TESLA = 1e-4;     // Conversion factor from Gauss to Tesla
        const double INCH_TO_METER = 0.0254;    // Conversion factor from inches to meters

        // Magnet properties (User-defined)
        static double surfaceField_Gauss = 500; // Surface field strength in Gauss
        static double cubeSize_Inches = 2;      // Side length of the cubic magnet in inches

        // Computed properties
        static double M = surfaceField_Gauss * GAUSS_TO_TESLA / MU_0; // Magnetization (A/m)
        static double a = (cubeSize_Inches / 2) * INCH_TO_METER;      // Half-length of cube in meters

        static void Main()
        {
            // Test point in the x-y plane (in inches)
            double x_inches = 1;
            double y_inches = 1; // Closer than ½ magnet width

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

            // Compute contributions from magnetized faces
            for (int sx = -1; sx <= 1; sx += 2)
            {
                for (int sy = -1; sy <= 1; sy += 2)
                {
                    double xp = x - sx * a; // Position relative to magnet face
                    double yp = y - sy * a; // Position relative to magnet face
                    double rSquared = xp * xp + yp * yp;
                    double rCubed = Math.Pow(rSquared, 1.5);

                    if (rCubed != 0)
                    {
                        // Apply logarithmic correction for close distances
                        double correctionFactor = 1 + Math.Log(1 + (a / Math.Sqrt(rSquared)));
                        Bx += correctionFactor * MU_0 * M * xp / (4 * Math.PI * rCubed);
                        By += correctionFactor * MU_0 * M * yp / (4 * Math.PI * rCubed);
                    }
                }
            }

            return (Bx, By);
        }
    }

    class MagneticFieldCalculator2
    {
        // Constants
        const double MU_0 = 4 * Math.PI * 1e-7; // Permeability of free space (T·m/A)
        const double GAUSS_TO_TESLA = 1e-4;     // Conversion factor from Gauss to Tesla
        const double INCH_TO_METER = 0.0254;    // Conversion factor from inches to meters

        // Magnet properties
        static double surfaceField_Gauss = 500; // Surface field strength in Gauss
        static double cubeSize_Inches = 2;      // Side length of the cubic magnet in inches

        // Computed properties
        static double M = surfaceField_Gauss * GAUSS_TO_TESLA / MU_0; // Magnetization (A/m)
        static double a = (cubeSize_Inches / 2) * INCH_TO_METER;      // Half-length of cube in meters

        static void Main()
        {
            // Test point in the x-y plane (in inches)
            double x_inches = 1, y_inches = 1; // Close to magnet

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

            // Inline loop structure for polarity-specific contributions
            foreach (int sx in new[] { -1, 1 })
                foreach (int sy in new[] { -1, 1 })
                {
                    double xp = x - sx * a;
                    double yp = y - sy * a;
                    double rSquared = xp * xp + yp * yp;
                    double rCubed = Math.Pow(rSquared, 1.5);

                    if (rCubed != 0)
                    {
                        // Determine polarity effect: north (+), south (-)
                        int poleSign = (sx == 1) ? 1 : -1; // North (+) right, South (-) left

                        double correctionFactor = 1 + Math.Log(1 + (a / Math.Sqrt(rSquared)));
                        Bx += poleSign * correctionFactor * MU_0 * M * xp / (4 * Math.PI * rCubed);
                        By += poleSign * correctionFactor * MU_0 * M * yp / (4 * Math.PI * rCubed);
                    }
                }

            return (Bx, By);
        }
    }
}