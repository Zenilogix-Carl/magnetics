using System;

namespace Magnets
{
    public static class Constants
    {
        #region Dimensions and Units
        /// <summary>
        /// Multiply a value in inches by this to obtain a result in metres
        /// </summary>
        public const double MetresPerInch = 0.0254;

        /// <summary>
        /// Multiply a value in Tesla by this to obtain a result in Gauss
        /// </summary>
        public const double GaussPerTesla = 10000;

        /// <summary>
        /// Multiply a value in Gauss by this to obtain a result in Tesla
        /// </summary>
        public const double TeslaPerGauss = 0.0001;

        /// <summary>
        /// Multiply a value in degrees by this to obtain a result in radians
        /// </summary>
        public const double RadiansPerDegree = Math.PI / 180.0;

        /// <summary>
        /// Multiply a value in radians by this to obtain a result in degrees
        /// </summary>
        public const double DegreesPerRadian = 180.0 / Math.PI;
        #endregion

        #region Physical Properties
        /// <summary>
        /// Permeability of free space
        /// </summary>
        public const double Mu0 = 4 * Math.PI * 1e-7;
        #endregion
    }
}
