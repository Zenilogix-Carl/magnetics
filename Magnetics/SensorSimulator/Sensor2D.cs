using System.Numerics;
using Magnets;

namespace SensorSimulator
{
    /// <summary>
    /// Idealized sensor operating in 2D space
    /// </summary>
    public class Sensor2D
    {
        /// <summary>
        /// Gets or sets sensor position in 2D space
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets sensor orientation in degrees. Zero aligns face with X axis
        /// </summary>
        public double Orientation { get; set; }

        /// <summary>
        /// Gets or sets sensitivity (mV/mT)
        /// </summary>
        public double Sensitivity { get; set; }

        /// <summary>
        /// Gets or sets sensor quiescent (null field) output value (V)
        /// </summary>
        public double QuiescentOutput { get; set; }

        /// <summary>
        /// Gets or sets minimum output value (V)
        /// </summary>
        public double OutputMinimum { get; set; }

        /// <summary>
        /// Gets or sets maximum output value (V)
        /// </summary>
        public double OutputMaximum { get; set; }

        /// <summary>
        /// Computes the output value for an input B vector in 2D space
        /// </summary>
        /// <param name="input">B vector (Tesla)</param>
        /// <returns>Output value (V)</returns>
        /// <remarks>Measures the input B field vector and returns a sensor output</remarks>
        public double GetOutput(Vector2 input)
        {
            var v = GetFieldComponent(input) * Sensitivity + QuiescentOutput;
            return (v < OutputMinimum) ? OutputMinimum : ((v > OutputMaximum) ? OutputMaximum : v);
        }

        /// <summary>
        /// Gets the component of the input B field projected on the X axis (Tesla)
        /// </summary>
        /// <param name="input">B vector (Tesla)</param>
        /// <returns>B field component along sensor's X axis (Tesla)</returns>
        public double GetFieldComponent(Vector2 input)
        {
            return input.Rotate(-Orientation).X;
        }
    }
}
