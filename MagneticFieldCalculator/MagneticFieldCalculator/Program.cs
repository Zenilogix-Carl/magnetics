using System;

namespace MagneticFieldCalculator
{
    class Program
    {
        private const double GaussPerTesla = 10000.0; // 1T = 10000 Gauss

        static void Main()
        {
            var size = .25;
            var bSurface = 4601.0 /* Gauss */ / GaussPerTesla; // field in Tesla
            var sweep = 60.0;
            var step = 5.0;
            var halfAngle = sweep/2;
            var magnetRadius = 1;
            var sensorDistanceFromOrigin = 9.0/16.0;
            var separation = .3 + (1.0/64.0);

            var magnetSeparation = magnetRadius * Math.Sin(halfAngle * Math.PI / 180.0) * 2.0;

            var m0 = new CubicMagnet(new Vector(), bSurface, size);

            var m1 = new CubicMagnet(new Vector { Distance = magnetRadius, Angle = halfAngle }, bSurface, size)
            {
                Angle = halfAngle
            };

            var m2 = new CubicMagnet(new Vector { Distance = magnetRadius, Angle = -halfAngle }, bSurface, size)
            {
                Angle = 180-halfAngle
            };

            var sensorLocation = new Vector { X = /*sensorDistanceFromOrigin*/ magnetRadius - (separation + size / 2) };

            for (int x = -20; x <= 20; x++)
            {
                var field = m0.DipoleField(new Vector { X = x, Y = 0.0 });
                Console.WriteLine($"{x} {field.X} {field.Y} {field.Magnitude * 1000} {field.Angle:F0}");
            }

            Console.WriteLine($"Magnet-to-magnet distance: {magnetSeparation:F2}");

            for (var angle = -halfAngle; angle <= halfAngle; angle += step)
            {
                m1.Position.Distance = magnetRadius;
                m1.Position.Angle = angle - halfAngle;
                m1.Angle = m1.Position.Angle;

                m2.Position.Distance = magnetRadius;
                m2.Position.Angle = angle + halfAngle;
                m2.Angle = m2.Position.Angle + 180;

                var field1 = m1.DipoleField(sensorLocation);
                var field2 = m2.DipoleField(sensorLocation);

                var field = field1 + field2; // Tesla

                Console.WriteLine($"{angle:F0} {field.X * 1000}"); // milliTesla
            }
        }
    }
}

