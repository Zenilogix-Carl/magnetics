using System;
using System.Numerics;

namespace SensorSimulator
{
    public class Drv5055A1 : Sensor2D
    {
        /// <summary>
        /// Limits of linear range (+/- mT)
        /// </summary>
        public double LinearRange { get; protected set; }

        public Drv5055A1()
        {
            OutputMinimum = 0.1;
            OutputMaximum = 4.9;
            QuiescentOutput = 2.5;
            Sensitivity = 100;

            LinearRange = 21;
        }

        /// <summary>
        /// Returns true if field is in linear range, false otherwise
        /// </summary>
        /// <param name="field">field vector (Tesla)</param>
        /// <returns></returns>
        public bool IsInLinearRange(Vector2 field) => Math.Abs(GetFieldComponent(field)) * 1000 < LinearRange;
    }
}
