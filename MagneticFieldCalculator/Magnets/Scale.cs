namespace Magnets
{
    public static class Scale
    {
        /// <summary>
        /// Multiply a value in inches by this to get a result in metres
        /// </summary>
        public const double MetresPerInch = 0.0254;

        /// <summary>
        /// Multiply a value in Tesla by this to get a result in Gauss
        /// </summary>
        public const double GaussPerTesla = 10000;

        /// <summary>
        /// Multiply a value in Gauss by this to get a result in Tesla
        /// </summary>
        public const double TeslaPerGauss = 0.0001;
    }
}
