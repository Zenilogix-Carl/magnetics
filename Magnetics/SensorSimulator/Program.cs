using Magnets;
using System.Numerics;
using System;

namespace SensorSimulator
{
    /// <summary>
    /// Angle sensor simulator
    /// </summary>
    /// <remarks>
    /// This program simulates the behavior of a position sensor set up as follows:
    /// We define an X/Y plane and an axis of rotation around the Z axis.
    /// There are two identical cubic magnets which measure 0.25" on a side and have surface field of 4601 Gauss
    /// We place our magnets around the Z axis such that
    /// the X/Y plane cuts through the centers of the magnets, and
    /// the center of each magnet is 1.05" from the Z axis.
    /// One magnet has its north pole facing the Z axis, the other has its south pole facing the Z axis
    /// One magnet is centered on a radial line rotated +30 degrees from the X axis,
    /// the other centred on a radial line -30 from the X axis
    /// A sensor which detects the component of a B field perpendicular to its face is positioned in the X/Y plane
    /// 0.595" away from the Z axis and facing away from it.
    /// The sensor is moved along an arc always facing away from the Z axis and its output is measured at a series of points.
    /// The sensor output is a voltage; its sensitivity is 100mV/mT, and its quiescent point (null field) is 2.5V;
    /// values less than 2.5V represent a B pointing toward the face, a value greater than 2.5V represent a B pointing away. 
    /// </remarks>
    public class Program
    {
        private const int SweepAngleDegrees = 30;
        private const double MagnetCenterRadiusInches = 1.8;
        private const double MagnetSizeInches = 0.25;
        private const double MagnetSurfaceFieldGauss = 4601;

        private const double SensorRadiusInches = 1.3;

        private const int StepDegrees = 1;
        private const int HalfAngleDegrees = SweepAngleDegrees / 2;

        public static void Main()
        {
            var m1 = CreateMagnet(new Vector2((float)(MagnetCenterRadiusInches * Constants.MetresPerInch), 0).Rotate(HalfAngleDegrees), HalfAngleDegrees);
            var m2 = CreateMagnet(new Vector2((float)(MagnetCenterRadiusInches * Constants.MetresPerInch), 0).Rotate(-HalfAngleDegrees), 180 - HalfAngleDegrees);

            var sensor = new Drv5055A1();

            var sensorNominalPosition = new Vector2((float)(SensorRadiusInches * Constants.MetresPerInch), 0);
            var magnetToMagnetVector = m1.Position - m2.Position;
            var magnetCenterSpacingInches = Math.Sqrt(magnetToMagnetVector.X*magnetToMagnetVector.X + magnetToMagnetVector.Y*magnetToMagnetVector.Y) * Constants.InchesPerMeter;

            Console.WriteLine($"Magnet centers at radius {MagnetCenterRadiusInches:F2}\" center-to-center spacing {magnetCenterSpacingInches:F2}\" +/-{HalfAngleDegrees} deg. {MagnetSizeInches:F2}\" cube, {MagnetSurfaceFieldGauss} Gauss surface field");
            Console.WriteLine($"Sensor at radius {SensorRadiusInches:F3}\" {sensor.Sensitivity}mV/mT");

            Console.WriteLine($"Angle\tB(mT)\tV\tNon-Linear");
            for (var i = -HalfAngleDegrees; i <= HalfAngleDegrees; i += StepDegrees)
            {
                sensor.Position = sensorNominalPosition.Rotate(i);
                sensor.Orientation = i;
                var bSum = m1.B(sensor.Position) + m2.B(sensor.Position); // combined field at sensor location
                var bSensor = sensor.GetFieldComponent(bSum); // component of bSum sensed by sensor
                var v = sensor.GetOutput(bSum); // sensor output (V)
                Console.WriteLine($"{i}\t{bSensor * 1000:F1}\t{v:F2}\t{(sensor.IsInLinearRange(bSum)?"":"*")}"); // show field strength in mT
            }
        }

        /// <summary>
        /// Creates a simulation of a real-world 0.25" cubic Neodymium magnet having a 4601 Gauss surface field
        /// placed in a 2D space
        /// </summary>
        /// <param name="position">Where in our 2D co-ordinate system the magnet is placed</param>
        /// <param name="orientation">Magnet's orientation in our 2D space</param>
        /// <returns>The created magnet</returns>
        private static MagnetWithPosition2 CreateMagnet(Vector2 position, double orientation)
        {
            return new MagnetWithPosition2(new CubicMagnet((float)(MagnetSizeInches * Constants.MetresPerInch)){SurfaceField = MagnetSurfaceFieldGauss * Constants.TeslaPerGauss}, position, orientation);
        }
    }
}

