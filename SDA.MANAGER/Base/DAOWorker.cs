using Inventec.Common.Repository;

namespace SDA.MANAGER.Base
{
    static class DAOWorker
    {
        internal static SDA.DAO.SdaCommune.SdaCommuneDAO SdaCommuneDAO { get { return (SDA.DAO.SdaCommune.SdaCommuneDAO)Worker.Get<SDA.DAO.SdaCommune.SdaCommuneDAO>(); } }
        internal static SDA.DAO.SdaConfig.SdaConfigDAO SdaConfigDAO { get { return (SDA.DAO.SdaConfig.SdaConfigDAO)Worker.Get<SDA.DAO.SdaConfig.SdaConfigDAO>(); } }
        internal static SDA.DAO.SdaConfigApp.SdaConfigAppDAO SdaConfigAppDAO { get { return (SDA.DAO.SdaConfigApp.SdaConfigAppDAO)Worker.Get<SDA.DAO.SdaConfigApp.SdaConfigAppDAO>(); } }
        internal static SDA.DAO.SdaConfigAppUser.SdaConfigAppUserDAO SdaConfigAppUserDAO { get { return (SDA.DAO.SdaConfigAppUser.SdaConfigAppUserDAO)Worker.Get<SDA.DAO.SdaConfigAppUser.SdaConfigAppUserDAO>(); } }
        internal static SDA.DAO.SdaDistrict.SdaDistrictDAO SdaDistrictDAO { get { return (SDA.DAO.SdaDistrict.SdaDistrictDAO)Worker.Get<SDA.DAO.SdaDistrict.SdaDistrictDAO>(); } }
        internal static SDA.DAO.SdaEthnic.SdaEthnicDAO SdaEthnicDAO { get { return (SDA.DAO.SdaEthnic.SdaEthnicDAO)Worker.Get<SDA.DAO.SdaEthnic.SdaEthnicDAO>(); } }
        internal static SDA.DAO.SdaEventLog.SdaEventLogDAO SdaEventLogDAO { get { return (SDA.DAO.SdaEventLog.SdaEventLogDAO)Worker.Get<SDA.DAO.SdaEventLog.SdaEventLogDAO>(); } }
        internal static SDA.DAO.SdaGroup.SdaGroupDAO SdaGroupDAO { get { return (SDA.DAO.SdaGroup.SdaGroupDAO)Worker.Get<SDA.DAO.SdaGroup.SdaGroupDAO>(); } }
        internal static SDA.DAO.SdaGroupType.SdaGroupTypeDAO SdaGroupTypeDAO { get { return (SDA.DAO.SdaGroupType.SdaGroupTypeDAO)Worker.Get<SDA.DAO.SdaGroupType.SdaGroupTypeDAO>(); } }
        internal static SDA.DAO.SdaLanguage.SdaLanguageDAO SdaLanguageDAO { get { return (SDA.DAO.SdaLanguage.SdaLanguageDAO)Worker.Get<SDA.DAO.SdaLanguage.SdaLanguageDAO>(); } }
        internal static SDA.DAO.SdaLicense.SdaLicenseDAO SdaLicenseDAO { get { return (SDA.DAO.SdaLicense.SdaLicenseDAO)Worker.Get<SDA.DAO.SdaLicense.SdaLicenseDAO>(); } }
        internal static SDA.DAO.SdaNational.SdaNationalDAO SdaNationalDAO { get { return (SDA.DAO.SdaNational.SdaNationalDAO)Worker.Get<SDA.DAO.SdaNational.SdaNationalDAO>(); } }
        internal static SDA.DAO.SdaNotify.SdaNotifyDAO SdaNotifyDAO { get { return (SDA.DAO.SdaNotify.SdaNotifyDAO)Worker.Get<SDA.DAO.SdaNotify.SdaNotifyDAO>(); } }
        internal static SDA.DAO.SdaProvince.SdaProvinceDAO SdaProvinceDAO { get { return (SDA.DAO.SdaProvince.SdaProvinceDAO)Worker.Get<SDA.DAO.SdaProvince.SdaProvinceDAO>(); } }
        internal static SDA.DAO.SdaReligion.SdaReligionDAO SdaReligionDAO { get { return (SDA.DAO.SdaReligion.SdaReligionDAO)Worker.Get<SDA.DAO.SdaReligion.SdaReligionDAO>(); } }
        internal static SDA.DAO.SdaTranslate.SdaTranslateDAO SdaTranslateDAO { get { return (SDA.DAO.SdaTranslate.SdaTranslateDAO)Worker.Get<SDA.DAO.SdaTranslate.SdaTranslateDAO>(); } }
        internal static SDA.DAO.SdaTrouble.SdaTroubleDAO SdaTroubleDAO { get { return (SDA.DAO.SdaTrouble.SdaTroubleDAO)Worker.Get<SDA.DAO.SdaTrouble.SdaTroubleDAO>(); } }

    }
}
