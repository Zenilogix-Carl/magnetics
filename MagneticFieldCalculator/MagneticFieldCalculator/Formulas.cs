using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MagneticFieldCalculator
{
    // From https://www.e-magnetica.pl/doku.php/calculator/field_of_cuboid_magnet_or_rectangular_solenoid


    class Formulas
    {
        public double mu0; // permeability of free space (H/m)

        public void Hx(double br /*remanence (T)*/ , double a /*cubic magnet half-length*/, Vector3 position)
        {
            var h0 = br/mu0; // magnetic field strength
            var hx = (h0 / (8 * Math.PI)) *
                     Summation((i, j, k) =>
                     {
                         var rijk = Rijk(a, position, i, j, k);
                         return SignFunc(i + j + k) * Math.Log(
                             (rijk - (position.Y + a * SignFunc(j + 1)))
                             / (rijk + (position.Y + a * SignFunc(j + 1))));
                     });
        }

        public void Hy(double br /*remanence (T)*/ , double a /*cubic magnet half-length*/, Vector3 position)
        {
            var h0 = br / mu0; // magnetic field strength
            var hy = (h0 / (8 * Math.PI)) *
                     Summation((i, j, k) =>
                     {
                         var rijk = Rijk(a, position, i, j, k);
                         return SignFunc(i + j + k) * Math.Log(
                             (rijk - (position.X + a * SignFunc(j + 1)))
                             / (rijk + (position.X + a * SignFunc(j + 1))));
                     });
        }

        public void Hz(double br /*remanence (T)*/ , double a /*cubic magnet half-length*/, Vector3 position)
        {
            var h0 = br / mu0; // magnetic field strength
            var hy = (h0 / (4 * Math.PI)) *
                     Summation((i, j, k) =>
                     {
                         var rijk = Rijk(a, position, i, j, k);
                         return SignFunc(i + j + k + 1) *
                                (Math.Atan(((position.X + a * SignFunc(i+1))*(position.Z + a * SignFunc(k+1)))/(rijk * (position.Y + a * SignFunc(j+1)))) 
                                 + Math.Atan(((position.Y + a * SignFunc(j + 1))*(position.Z + a * SignFunc(k+1)))/(rijk * (position.X + a * SignFunc(i+1)))));
                     });
        }

        double Summation(Func<int,int,int,double> func)
        {
            double sum = 0;

            for (var i = 0; i <= 1; i++)
            {
                for (var j = 0; j <= 1; j++)
                {
                    for (var k = 0; k <= 1; k++)
                    {
                        sum += func(i, j, k);
                    }
                }
            }
        }

        double Squared(double a)
        {
            return a * a;
        }

        double Rijk(double a, Vector3 position, int i, int j, int k)
        {
            return Math.Sqrt(
                Squared(position.X + a * SignFunc(i + 1))
                +Squared(position.Y + a * SignFunc(j + 1))
                +Squared(position.Z + a * SignFunc(k + 1)));
        }

        double SignFunc(int i)
        {
            return i % 2 == 0 ? 1 : -1;
        }
    }
}
