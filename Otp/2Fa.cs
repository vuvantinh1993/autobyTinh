using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autohana.Otp
{
    public static class _2Fa
    {
        public static string Lay2FaFB(string twoFactorAuthSeed)
        {
            var otpKeyBytes = Base32Encoding.ToBytes(twoFactorAuthSeed);
            // Instanticate the generator's class
            var totp = new Totp(otpKeyBytes);
            // Compute the 2FA code. Easy.
            var twoFactorCode = totp.ComputeTotp();
            return twoFactorCode;
        }
    }
}
