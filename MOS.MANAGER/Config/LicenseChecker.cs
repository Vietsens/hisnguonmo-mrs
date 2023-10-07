using Inventec.Common.HardDrive;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.MANAGER.Base;
using MOS.MANAGER.ConsumerManager;
using SDA.EFMODEL.DataModels;
using SDA.Filter;
using System;
using System.Configuration;

namespace MOS.MANAGER.Config
{
    public class LicenseChecker : BusinessBase
    {
        private const string decryptRsaKey = "<RSAKeyValue><Modulus>vx1J3M/TcESXJw2Jv8G43ZUHdFy26qS+r5Bsfx2QOQmPgn0P5WxmIh6zMKlBB0s/Wuhrqo5mqchmPWg7JUkViijz8sEpmxl8EkMxiYDWWDfY9w5JXgp28Kz7WpMDihhHdmWDUo5VoMkwO41Yn8F94Gx02JyFgKSsJNulGomRyU8=</Modulus><Exponent>AQAB</Exponent><P>/UjErI/thMB6dS+OfH6pfUCJLQuMkIIWkFenX+qHlSpqgaIrdARGXbobXT8JNU4Ow2lU8H7y/4cj8FBYIwv74Q==</P><Q>wSnfTac/C8kQJyqzkxakDA2tIFSR1oblF5r9bWYKF7l2CGt3spoWWnkT2CTd5dUcL64GMO1MI4C3RkUrFwfrLw==</Q><DP>goJBXQ1YfMPVFZbom3uEh9Z+GGjQCBIP4FZaFxE4xYKymJNgQoqFW3wu53A4pW/QKZ6XtsgjG8mdmhE8KOre4Q==</DP><DQ>Rv5zwx+gDV3VoP+RxpLAsmloYwSSIU2s75MYZ9fkB7ozRn7xHGSDqLbtcziBkdJUrLWCMwNUQ4wwUBRl9O4FIw==</DQ><InverseQ>qRX7IyU28TuSAeUDyGkAE/fpH2kkwdy6cyENy8GioPTb5VSA2WtPUFQR25KOC32uxaz/UteEfxlhJlpo1K0Yiw==</InverseQ><D>lbePHqHdBU1mkFyFtig/966BOYJMdcN78rOUm3yAeH5p45Kvuk5SMJaWUB+35svnJQOBHQsTmBx7I84cz5fSe5zcEee/EiLMxMJKUhiCGKkl390AnS8du94mtafcDcqrKEHVjwUUSAl7A4mAGIuR8irSt7OQ1IWIO31ImoPWnUE=</D></RSAKeyValue>";
        private const string decryptAesKey = "᷻臏閳囀�䛹捺⷏첯瀞穸䡷퐕멟韦湘";
        private const string decryptAesIv = "졻엃菱萯⇨蹋件";
        private const string APP_CODE = "MOS";
        private static bool validLicense = false;

        public static bool ValidLicense
        {
            get
            {
                return validLicense;
            }
        }

        public static void Check()
        {
            try
            {
                string getLastLicenseUri = ConfigurationManager.AppSettings["Inventec.SdaConsumer.GetLastLicense.Uri"];
                string cardmac = NetworkCard.GetMacAddress();
                string puk = ConfigurationManager.AppSettings["License.PUK"];
                long now = Inventec.Common.DateTime.Get.Now().Value;

                LogSystem.Debug("cardmac:" + cardmac);

                if (String.IsNullOrWhiteSpace(cardmac) || String.IsNullOrWhiteSpace(puk) || now == 0)
                {
                    throw new Exception("snhdd, puk hoac time ko hop le. cardmac:" + cardmac + "; puk:" + puk + "; now:" + now);
                }

                SdaLicenseFilter filter = new SdaLicenseFilter();
                filter.Time = now;
                filter.PUK = puk;
                filter.SN_HDD = cardmac;
                filter.APP_CODE = APP_CODE;

                var aro = ApiConsumerStore.SdaConsumer.Get<ApiResultObject<SDA_LICENSE>>(getLastLicenseUri, null, filter);

                if (aro == null || aro.Data == null)
                {
                    LogSystem.Warn("puk:" + puk);
                    LogSystem.Warn("now:" + now);
                    throw new LicenseNotFoundException("Khong lay duoc thong tin license tu server.");
                }

                SDA_LICENSE license = aro.Data;
                string licenseDecrypt = Inventec.Common.LicenseCrypto.Execute.Decrypt(license.LICENSE, decryptRsaKey, decryptAesKey, decryptAesIv);
                if (String.IsNullOrEmpty(licenseDecrypt))
                {
                    throw new LicenseWrongDataException("Khong decrypt duoc license. licenseDecrypt:" + licenseDecrypt);
                }

                string[] licenseParam = licenseDecrypt.Split(',');
                if (licenseParam.Length < 5)
                {
                    throw new LicenseWrongDataException("So luong param khong hop le (<5). licenseDecrypt:" + licenseDecrypt);
                }

                ///Kiem tra thoi han hieu luc cung nhu thong tin so voi license (tranh fake thong tin khi lay license)
                if (now > long.Parse(licenseParam[3]))
                {
                    throw new LicenseExpireException("licenseDecrypt:" + licenseDecrypt);
                }
                if (puk != licenseParam[1])
                {
                    throw new LicenseWrongDataException("PUK: " + puk + ". licenseDecrypt:" + licenseDecrypt);
                }
                if (cardmac != licenseParam[2])
                {
                    throw new LicenseWrongDataException("snhdd: " + cardmac + ". licenseDecrypt:" + licenseDecrypt);
                }
                if (APP_CODE != licenseParam[0])
                {
                    throw new LicenseWrongDataException("APP_CODE: " + APP_CODE + ". licenseDecrypt:" + licenseDecrypt);
                }
                validLicense = true;
                LogSystem.Info("Kiem tra License: hop le");
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                validLicense = false;
            }
        }

        class LicenseWrongDataException : Exception
        {
            internal LicenseWrongDataException() { }
            internal LicenseWrongDataException(string message) : base("Sai du lieu license. " + message) { }
        }

        class LicenseNotFoundException : Exception
        {
            internal LicenseNotFoundException() { }
            internal LicenseNotFoundException(string message) : base("Khong tim thay license. " + message) { }
        }

        class LicenseExpireException : Exception
        {
            internal LicenseExpireException() { }
            internal LicenseExpireException(string message) : base("License het hieu luc. " + message) { }
        }
    }
}
