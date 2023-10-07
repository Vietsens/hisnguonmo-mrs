using Inventec.Common.Repository;

namespace TYT.MANAGER.Base
{
    static class DAOWorker
    {
        internal static TYT.DAO.TytDeath.TytDeathDAO TytDeathDAO { get { return (TYT.DAO.TytDeath.TytDeathDAO)Worker.Get<TYT.DAO.TytDeath.TytDeathDAO>(); } }
        internal static TYT.DAO.TytFetusAbortion.TytFetusAbortionDAO TytFetusAbortionDAO { get { return (TYT.DAO.TytFetusAbortion.TytFetusAbortionDAO)Worker.Get<TYT.DAO.TytFetusAbortion.TytFetusAbortionDAO>(); } }
        internal static TYT.DAO.TytFetusBorn.TytFetusBornDAO TytFetusBornDAO { get { return (TYT.DAO.TytFetusBorn.TytFetusBornDAO)Worker.Get<TYT.DAO.TytFetusBorn.TytFetusBornDAO>(); } }
        internal static TYT.DAO.TytFetusExam.TytFetusExamDAO TytFetusExamDAO { get { return (TYT.DAO.TytFetusExam.TytFetusExamDAO)Worker.Get<TYT.DAO.TytFetusExam.TytFetusExamDAO>(); } }
        internal static TYT.DAO.TytGdsk.TytGdskDAO TytGdskDAO { get { return (TYT.DAO.TytGdsk.TytGdskDAO)Worker.Get<TYT.DAO.TytGdsk.TytGdskDAO>(); } }
        internal static TYT.DAO.TytHiv.TytHivDAO TytHivDAO { get { return (TYT.DAO.TytHiv.TytHivDAO)Worker.Get<TYT.DAO.TytHiv.TytHivDAO>(); } }
        internal static TYT.DAO.TytKhh.TytKhhDAO TytKhhDAO { get { return (TYT.DAO.TytKhh.TytKhhDAO)Worker.Get<TYT.DAO.TytKhh.TytKhhDAO>(); } }
        internal static TYT.DAO.TytMalaria.TytMalariaDAO TytMalariaDAO { get { return (TYT.DAO.TytMalaria.TytMalariaDAO)Worker.Get<TYT.DAO.TytMalaria.TytMalariaDAO>(); } }
        internal static TYT.DAO.TytNerves.TytNervesDAO TytNervesDAO { get { return (TYT.DAO.TytNerves.TytNervesDAO)Worker.Get<TYT.DAO.TytNerves.TytNervesDAO>(); } }
        internal static TYT.DAO.TytTuberculosis.TytTuberculosisDAO TytTuberculosisDAO { get { return (TYT.DAO.TytTuberculosis.TytTuberculosisDAO)Worker.Get<TYT.DAO.TytTuberculosis.TytTuberculosisDAO>(); } }
        internal static TYT.DAO.TytUninfect.TytUninfectDAO TytUninfectDAO { get { return (TYT.DAO.TytUninfect.TytUninfectDAO)Worker.Get<TYT.DAO.TytUninfect.TytUninfectDAO>(); } }
        internal static TYT.DAO.TytUninfectIcd.TytUninfectIcdDAO TytUninfectIcdDAO { get { return (TYT.DAO.TytUninfectIcd.TytUninfectIcdDAO)Worker.Get<TYT.DAO.TytUninfectIcd.TytUninfectIcdDAO>(); } }
        internal static TYT.DAO.TytUninfectIcdGroup.TytUninfectIcdGroupDAO TytUninfectIcdGroupDAO { get { return (TYT.DAO.TytUninfectIcdGroup.TytUninfectIcdGroupDAO)Worker.Get<TYT.DAO.TytUninfectIcdGroup.TytUninfectIcdGroupDAO>(); } }

    }
}
