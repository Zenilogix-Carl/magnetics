using System;

class MagneticFieldCalculator
{
    static void Main()
    {
        // Constants
        double mu0 = 4 * Math.PI * 1e-7; // Permeability of free space (T·m/A)
        double Bs = 0.4601; // Surface field in Tesla
        double r = 0.25; // Distance from north pole in inches
        double magnetSize = 0.25; // Cube side length in inches

        // Convert inches to meters (1 inch = 0.0254 meters)
        double r_m = r * 0.0254;
        double magnetSize_m = magnetSize * 0.0254;

        // Approximate magnet volume (V = s³ for cube)
        double V_m = Math.Pow(magnetSize_m, 3);

        // Estimate magnetic moment (M = B_s * V)
        double M = Bs * V_m;

        // Magnetic field calculation (Dipole approximation)
        double Bz = (mu0 / (4 * Math.PI)) * (2 * M) / Math.Pow(r_m, 3);


        // Output result in Tesla
        Console.WriteLine($"Magnetic Field Vector at {r} inches: (0, 0, {(Bz * 1000)}) mTesla");
    }

    static void Main2()
    {
        // Constants
        double mu0 = 4 * Math.PI * 1e-7; // Permeability of free space (T·m/A)
        double Bs = 0.4601; // Surface field in Tesla
        double magnetSize = 0.25; // Cube side length in inches
        double r_surface = 1.0 / 8.0; // Distance from surface in inches

        // Convert inches to meters (1 inch = 0.0254 meters)
        double magnetSize_m = magnetSize * 0.0254;
        double r_m = r_surface * 0.0254;

        // Approximate magnet volume (V = s³ for cube)
        double V_m = Math.Pow(magnetSize_m, 3);

        // Estimate magnetic moment (M = B_s * V)
        double M = Bs * V_m;

        // Refined magnetic field calculation
        double Bz = (mu0 / (2 * Math.PI)) * (M / Math.Pow(r_m + magnetSize_m, 2));

        // Output result in Tesla
        Console.WriteLine($"Magnetic Field at {r_surface} inches: {Bz} Tesla");
    }

    static void Main3()
    {
        // Constants
        double mu0 = 4 * Math.PI * 1e-7; // Permeability of free space (T·m/A)
        double Bs = 0.4601; // Surface field in Tesla
        double magnetSize = 0.25; // Cube side length in inches
        double r_surface = 1.0 / 8.0; // Distance from surface in inches

        // Convert inches to meters
        double magnetSize_m = magnetSize * 0.0254;
        double r_m = r_surface * 0.0254;

        // Approximate magnet volume (V = s³ for cube)
        double V_m = Math.Pow(magnetSize_m, 3);

        // Estimate magnetic moment (M = B_s * V)
        double mz = Bs * V_m;

        // Define position (example: 1/8 inch away along x or y)
        double x_m = 1.0 / 8.0 * 0.0254; // Example X-position in meters
        double y_m = 1.0 / 8.0 * 0.0254; // Example Y-position in meters
        double z_m = r_m + magnetSize_m; // Distance from center along Z

        // Compute distance r from magnet center
        double r_total = Math.Sqrt(x_m * x_m + y_m * y_m + z_m * z_m);

        // Compute B-field components
        double Bx = (mu0 / (4 * Math.PI)) * ((3 * x_m * mz - mz * Math.Pow(r_total, 2)) / Math.Pow(r_total, 5));
        double By = (mu0 / (4 * Math.PI)) * ((3 * y_m * mz - mz * Math.Pow(r_total, 2)) / Math.Pow(r_total, 5));
        double Bz = (mu0 / (4 * Math.PI)) * ((3 * z_m * mz - mz * Math.Pow(r_total, 2)) / Math.Pow(r_total, 5));

        // Output result
        Console.WriteLine($"Magnetic Field Vector at ({x_m}, {y_m}, {z_m}) inches: ({Bx}, {By}, {Bz}) Tesla");
    }
}