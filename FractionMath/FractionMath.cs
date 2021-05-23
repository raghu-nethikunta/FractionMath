using System;

namespace FractionMath
{
    public class FractionMath : IEquatable<FractionMath>, IComparable<FractionMath>
    {
        private FractionMath _reciprocal;

        /// <summary>
        /// The sign of the fraction, -1 or 1.
        /// </summary>
        public int Sign { get; }

        /// <summary>
        /// The denominator for the fraction.
        /// </summary>
        public int Denominator { get; } = 1;

        /// <summary>
        /// The improper numerator for the fraction. See <see cref="ProperNumerator" /> for the proper numerator.
        /// </summary>
        public int Numerator { get; }

        /// <summary>
        /// The proper numerator for the fraction. See <see cref="Numerator" /> for the improper numerator.
        /// </summary>
        public int ProperNumerator { get; }

        /// <summary>
        /// Get the reciprocal fraction.
        /// </summary>
        public FractionMath Reciprocal
        {
            get
            {
                if (_reciprocal != null)
                    return _reciprocal;

                _reciprocal = new FractionMath(Sign * Denominator, Numerator);
                return _reciprocal;
            }
        }

        /// <summary>
        /// Create a new instance of <see cref="FractionMath" /> with a numerator and denominator.
        /// </summary>
        /// <remarks>
        /// When creating an instance of <see cref="FractionMath" /> it will always be in its simplest form.
        /// </remarks>
        /// <param name="numerator">Numerator for the fraction.</param>
        /// <param name="denominator">Denominator for the fraction.</param>
        public FractionMath(int numerator, int denominator = 1)
        {
            if (denominator == 0)
                throw new ArgumentException(nameof(denominator));

            Sign = numerator < 0 ^ denominator < 0 ? -1 : 1;

            Numerator = Math.Abs(numerator);
            Denominator = Math.Abs(denominator);

            var gcd = CommonMath.GreatestCommonDivisor(Numerator, Denominator);
            Numerator /= gcd;
            Denominator /= gcd;

            if (Denominator == 1)
                ProperNumerator = 0;
            else
                ProperNumerator = Numerator < Denominator ? Numerator : Numerator % Denominator;
        }

        /// <summary>
        /// Test for equality to another instance of <see cref="FractionMath" />.
        /// </summary>
        /// <param name="other">The instance to test equality against.</param>
        /// <returns>True if equal, otherwise false.</returns>
        public bool Equals(FractionMath other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Sign == other.Sign
                && Numerator == other.Numerator
                && Denominator == other.Denominator;
        }

        /// <summary>
        /// Test for equality against another object.
        /// </summary>
        /// <remarks>
        /// This tests against integers and strings representing integers as well.
        /// </remarks>
        /// <param name="obj">The object to test equality against.</param>
        /// <returns>True if equal, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (Convert.ToInt32(obj) is int wholeNumber)
                return Equals(new FractionMath(wholeNumber));

            if (obj is string fractionString && int.TryParse(fractionString, out int parsedInt))
                return Equals(new FractionMath(parsedInt));

            if (obj.GetType() != typeof(FractionMath))
                return false;

            return Equals(obj as FractionMath);
        }

        public static FractionMath operator +(FractionMath left, FractionMath right)
        {
            var newDenominator = CommonMath.LeastCommonMultiple(left.Denominator, right.Denominator);
            var newNumerator = left.Sign * left.Numerator * newDenominator / left.Denominator
                + right.Sign * right.Numerator * newDenominator / right.Denominator;

            return new FractionMath(newNumerator, newDenominator);
        }

        public static FractionMath operator -(FractionMath left, FractionMath right)
        {
            var newDenominator = CommonMath.LeastCommonMultiple(left.Denominator, right.Denominator);
            var newNumerator = left.Sign * left.Numerator * newDenominator / left.Denominator
                - right.Sign * right.Numerator * newDenominator / right.Denominator;

            return new FractionMath(newNumerator, newDenominator);
        }

        public static FractionMath operator *(FractionMath left, FractionMath right)
        {
            return new FractionMath(left.Sign * left.Numerator * right.Sign * right.Numerator, left.Denominator * right.Denominator);
        }
        public static FractionMath operator /(FractionMath left, FractionMath right)
        {
            return left * right.Reciprocal;
        }

        public static explicit operator int(FractionMath other)
        {
            return other.Sign * other.Numerator / other.Denominator;
        }

        public static implicit operator FractionMath(int number)
        {
            return new FractionMath(number);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 17;
                hashCode = hashCode * 223 + Numerator;
                hashCode = hashCode * 223 + Denominator;

                return hashCode;
            }
        }

        public override string ToString()
        {
            var whole = Numerator / Denominator;
            var sign = Sign < 0 ? "-" : string.Empty;

            if (ProperNumerator == 0)
                return $"{sign}{whole}";

            if (whole == 0)
                return $"{sign}{Numerator}/{Denominator}";
            else
                return $"{sign}{whole}{"_"}{ProperNumerator}/{Denominator}";
        }

        /// <summary>
        /// Compare the <see cref="FractionMath" /> with another fraction.
        /// </summary>
        /// <param name="other">The <see cref="FractionMath" /> to compare against.</param>
        /// <returns>-1 if it is less, 0 if equal, 1 if it is greater.</returns>
        public int CompareTo(FractionMath other)
        {
            var lcm = CommonMath.LeastCommonMultiple(Denominator, other.Denominator);
            var leftNumerator = Numerator * lcm / Denominator;
            var rightNumerator = other.Numerator * lcm / other.Denominator;

            return leftNumerator.CompareTo(rightNumerator);
        }

        public static FractionMath GetFraction(string s)
        {
            int numerator = 0;
            int denominator = 0;
            if (s.Contains("_"))
            {
                string[] mixedNumbers = s.Split(new char[] { '_' });

                numerator = int.Parse(mixedNumbers[1].Substring(0, 1));
                denominator = int.Parse(mixedNumbers[1].Substring(mixedNumbers[1].Length - 1, 1));
                return new FractionMath(numerator, denominator) + int.Parse(mixedNumbers[0]); // 3 3/4


            }
            numerator = int.Parse(s.Substring(0, 1));
            denominator = int.Parse(s.Substring(s.Length - 1, 1));
            return new FractionMath(numerator, denominator);  // 1/2
        }
    }
}