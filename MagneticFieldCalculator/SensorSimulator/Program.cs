using Magnets;
using System.Numerics;
using System;

namespace SensorSimulator
{
    public class Program
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
        public static void Main()
        {
            var m1 = CreateMagnet(new Vector2((float)(1.05 * Constants.MetresPerInch), 0).Rotate(30), 30);
            var m2 = CreateMagnet(new Vector2((float)(1.05 * Constants.MetresPerInch), 0).Rotate(-30), 180 - 30);

            var sensorNominalPosition = new Vector2((float)(0.595 * Constants.MetresPerInch), 0);

            Console.WriteLine($"Angle, B(mT) V");
            for (var i = -30; i <= 30; i += 5)
            {
                var sensorPosition = sensorNominalPosition.Rotate(i);
                var bT = (m1.B(sensorPosition) + m2.B(sensorPosition)).Rotate(-i);
                var bmT = bT * 1000;
                var v = (bT.X * 100) + 2.5; // sensor model; 100mV/mT quiescent at Vcc/2, Vcc = 5V
                Console.WriteLine($"{i}, {bmT.X:F1} {v:F2}");
            }
        }

        /// <summary>
        /// Creates a magnet representative of a real-world 0.25" cubic Neodymium magnet having a 4601 Gauss surface field
        /// placed in a 2D space
        /// </summary>
        /// <param name="position">Where in our 2D co-ordinate system the magnet is placed</param>
        /// <param name="orientation">Magnet's orientation in our 2D space</param>
        /// <returns>The created magnet</returns>
        private static MagnetWithPosition2 CreateMagnet(Vector2 position, double orientation)
        {
            return new MagnetWithPosition2(new CubicMagnet((float)(0.25 * Constants.MetresPerInch)), position)
            {
                Orientation = orientation,
                SurfaceField = 4601 * Constants.TeslaPerGauss
            };
        }
    }
}

