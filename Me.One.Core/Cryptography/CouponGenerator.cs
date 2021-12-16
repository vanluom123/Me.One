using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Me.One.Core.Cryptography
{
    public class CouponGenerator
    {
        private readonly int _bitCount;
        private readonly ulong _bitMask;
        private readonly int _bits;
        private readonly CouponCryptographyInformation _info;

        public CouponGenerator(CouponCryptographyInformation info)
        {
            _info = info;
            var num = info.Alphatbet.Length - 1;
            do
            {
                ++_bits;
                num /= 2;
            } while (num > 0);

            _bitCount = _bits * info.CouponLength;
            _bitMask = (ulong) ((1 << (_bitCount / 2)) - 1);
        }

        public IEnumerable<string> GenerateCoupons(ulong start, ulong numberOfCode)
        {
            for (ulong num = 0; num < numberOfCode; ++num)
                yield return GenerateCoupon(start + num);
        }

        public string GenerateCoupon(ulong num)
        {
            return CouponCode(Crypt(num));
        }

        public ulong TotalCodes()
        {
            var num = (ulong) Math.Pow(_info.Alphatbet.Length, _info.CouponLength);
            return num != 0UL ? num : ulong.MaxValue;
        }

        private ulong RoundFunction(ulong number)
        {
            return ((number * (ulong) _info.SecretNumber3 + _info.SecretNumber2) ^ _info.SecretNumber) & _bitMask;
        }

        private ulong Crypt(ulong number)
        {
            var num1 = number >> (_bitCount / 2);
            var number1 = number & _bitMask;
            for (var index = 0; index < _info.Round; ++index)
            {
                var num2 = (long) (num1 ^ RoundFunction(number1));
                num1 = number1;
                number1 = (ulong) num2;
            }

            return num1 | (number1 << (_bitCount / 2));
        }

        public static ulong CalculateMaximumTotalCode(int lengthCoupon)
        {
            return lengthCoupon > 12 ? ulong.MaxValue : (ulong) Math.Pow(32.0, lengthCoupon);
        }

        public static string CreateAlphabet(int lengCoupon, ulong numberCoupon)
        {
            var list = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToList();
            var str = "";
            var num1 = 0;
            var num2 = (int) Math.Ceiling(Math.Pow(numberCoupon, 1.0 / lengCoupon)) - 1;
            do
            {
                ++num1;
                num2 /= 2;
            } while (num2 > 0);

            var num3 = 1 << num1;
            var random = new Random();
            for (var index1 = 0; index1 < num3; ++index1)
            {
                var index2 = random.Next(list.Count);
                str += list[index2].ToString();
                list.RemoveAt(index2);
            }

            return str;
        }

        private string CouponCode(ulong number)
        {
            var stringBuilder = new StringBuilder();
            for (var index = 0; index < _info.CouponLength; ++index)
            {
                stringBuilder.Append(_info.Alphatbet[(int) number & ((1 << _bits) - 1)]);
                number >>= _bits;
            }

            return stringBuilder.ToString();
        }

        public ulong CodeFromCoupon(string coupon)
        {
            ulong number = 0;
            for (var index = 0; index < _info.CouponLength; ++index)
                number |= (ulong) _info.Alphatbet.IndexOf(coupon[index]) << (_bits * index);
            return Crypt(number);
        }
    }
}