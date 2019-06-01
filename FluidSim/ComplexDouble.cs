using System;
using System.Collections.Generic;
using System.Text;

namespace LiquidSim {
    using System;
    using System.Numerics;

    public class ComplexDouble {
        public double R;
        public double I;

        public static readonly ComplexDouble i = new ComplexDouble(0, 1);

        public ComplexDouble() {
        }

        public ComplexDouble(double r) {
            this.R = r;
        }

        public ComplexDouble(double r, double i) {
            this.R = r;
            this.I = i;
        }

        public double Abs() {
            return Magnitude();
        }

        public double Power() {
            return R * R + I * I;
        }

        public double Power2() {
            return Math.Abs(R) + Math.Abs(I);
        }

        public double Power2Root() {
            return Math.Sqrt(Power2());
        }

        public ComplexDouble Conjugate() {
            return new ComplexDouble(R, -I);
        }

        public double Magnitude() {
            return Math.Sqrt(Power());
        }

        public static ComplexDouble operator +(ComplexDouble n1, ComplexDouble n2) {
            return new ComplexDouble(n1.R + n2.R, n1.I + n2.I);
        }

        public static ComplexDouble operator +(ComplexDouble n1, double n2) {
            return new ComplexDouble(n1.R + n2, n1.I);
        }

        public static ComplexDouble operator +(double n1, ComplexDouble n2) {
            return new ComplexDouble(n1 + n2.R, n2.I);
        }

        public static ComplexDouble operator -(ComplexDouble n1, ComplexDouble n2) {
            return new ComplexDouble(n1.R - n2.R, n1.I - n2.I);
        }

        public static ComplexDouble operator -(ComplexDouble n1, double n2) {
            return new ComplexDouble(n1.R - n2, n1.I);
        }

        public static ComplexDouble operator -(double n1, ComplexDouble n2) {
            return new ComplexDouble(n1 - n2.R, n2.I);
        }

        public static ComplexDouble operator *(ComplexDouble n1, ComplexDouble n2) {
            return new ComplexDouble(n1.R * n2.R - n1.I * n2.I, n1.I * n2.R + n2.I * n1.R);
        }

        public static ComplexDouble operator *(ComplexDouble n1, double n2) {
            return new ComplexDouble(n1.R * n2, n1.I * n2);
        }

        public static ComplexDouble operator *(double n1, ComplexDouble n2) {
            return new ComplexDouble(n1 * n2.R, n1 * n2.I);
        }

        public static ComplexDouble operator /(ComplexDouble n1, ComplexDouble n2) {
            ComplexDouble c2 = n2.Conjugate();
            n1 *= c2;
            n2 *= c2;
            return n1 / n2.R;
        }

        public static ComplexDouble operator /(ComplexDouble n1, double n2) {
            return new ComplexDouble(n1.R / n2, n1.I / n2);
        }

        public static implicit operator ComplexDouble(double n) {
            return new ComplexDouble(n);
        }

        public override string ToString() {
            if(I >= 0) {
                return $"{R:N2} + {I:N2}i";
            } else {
                return $"{R:N2} - {Math.Abs(I):N2}i";
            }
        }
    }
}
