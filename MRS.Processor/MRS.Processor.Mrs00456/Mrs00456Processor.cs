using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Treatment.DateTime;
using MOS.MANAGER.HisServiceMaty;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatient;


namespace MRS.Processor.Mrs00456
{
    //báo cáo hoạt động xét nghiệm

    class Mrs00456Processor : AbstractProcessor
    {
        Mrs00456Filter castFilter = null;
        List<Mrs00456RDO> listRdo = new List<Mrs00456RDO>();

        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>();
        List<V_HIS_SERVICE_MATY> listServiceMatys = new List<V_HIS_SERVICE_MATY>();
        Dictionary<long, HIS_TREATMENT> DicTreatment = new Dictionary<long, HIS_TREATMENT>();
        Dictionary<long, HIS_PATIENT> DicPatient = new Dictionary<long, HIS_PATIENT>();

        List<long> listServiceTBIDs = new List<long>();

        List<long> listServiceTBM = new List<long>();
        List<long> listServiceMDMC = new List<long>();
        List<long> listServiceNM = new List<long>();
        List<long> listServiceML = new List<long>();
        List<long> listServiceHIV = new List<long>();
        List<long> listServiceHbsAg = new List<long>();
        List<long> listServiceSR = new List<long>();

        List<long> listServiceSHM = new List<long>();
        List<long> listServiceNT = new List<long>();
        List<long> listServiceXND = new List<long>();

        List<long> listServiceVK = new List<long>();
        List<long> listServiceLAO = new List<long>();
        List<long> listServiceKST = new List<long>();

        public Mrs00456Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00456Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00456Filter)this.reportFilter;

                HisServiceRetyCatViewFilterQuery retyCastFilter = new HisServiceRetyCatViewFilterQuery();
                retyCastFilter.REPORT_TYPE_CODE__EXACT = "MRS00456";
                var listServiceRetCats = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(param).GetView(retyCastFilter);

                //listServiceTBIDs = listServiceRetCats.Where(w => w.CATEGORY_CODE == "456_TB").Select(s=>s.SERVICE_ID).ToList(); 
                //hh
                listServiceTBM = listServiceRetCats.Where(w => w.CATEGORY_CODE == "456_TBM").Select(s => s.SERVICE_ID).ToList();
                listServiceMDMC = listServiceRetCats.Where(w => w.CATEGORY_CODE == "456_MDMC").Select(s => s.SERVICE_ID).ToList();
                listServiceNM = listServiceRetCats.Where(w => w.CATEGORY_CODE == "456_NM").Select(s => s.SERVICE_ID).ToList();
                listServiceML = listServiceRetCats.Where(w => w.CATEGORY_CODE == "456_ML").Select(s => s.SERVICE_ID).ToList();
                listServiceHIV = listServiceRetCats.Where(w => w.CATEGORY_CODE == "456_HIV").Select(s => s.SERVICE_ID).ToList();
                listServiceHbsAg = listServiceRetCats.Where(w => w.CATEGORY_CODE == "456_HbsAg").Select(s => s.SERVICE_ID).ToList();
                listServiceSR = listServiceRetCats.Where(w => w.CATEGORY_CODE == "456_SR").Select(s => s.SERVICE_ID).ToList();
                //vs
                listServiceSHM = listServiceRetCats.Where(w => w.CATEGORY_CODE == "456_SHM").Select(s => s.SERVICE_ID).ToList();
                listServiceNT = listServiceRetCats.Where(w => w.CATEGORY_CODE == "456_NT").Select(s => s.SERVICE_ID).ToList();
                listServiceXND = listServiceRetCats.Where(w => w.CATEGORY_CODE == "456_XND").Select(s => s.SERVICE_ID).ToList();
                //vk
                listServiceVK = listServiceRetCats.Where(w => w.CATEGORY_CODE == "456_VK").Select(s => s.SERVICE_ID).ToList();
                listServiceLAO = listServiceRetCats.Where(w => w.CATEGORY_CODE == "456_LAO").Select(s => s.SERVICE_ID).ToList();
                listServiceKST = listServiceRetCats.Where(w => w.CATEGORY_CODE == "456_KST").Select(s => s.SERVICE_ID).ToList();

                var skip = 0;
                while (listServiceRetCats.Count - skip > 0)
                {
                    var listIds = listServiceRetCats.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisSereServViewFilterQuery sereServViewFilter = new HisSereServViewFilterQuery();
                    sereServViewFilter.INTRUCTION_DATE_FROM = castFilter.TIME_FROM;
                    sereServViewFilter.INTRUCTION_DATE_TO = castFilter.TIME_TO;
                    sereServViewFilter.SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN;
                    sereServViewFilter.SERVICE_IDs = listIds.Select(s => s.SERVICE_ID).ToList();
                    listSereServs = new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServViewFilter);
                }

                listSereServs = listSereServs.Where(w => w.IS_DELETE != 1 && w.IS_ACTIVE == 1).ToList();

                if (IsNotNullOrEmpty(listSereServs))
                {
                    skip = 0;
                    while (listSereServs.Count - skip > 0)
                    {
                        var listIds = listSereServs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisServiceMatyViewFilterQuery serviceMatyFilter = new HisServiceMatyViewFilterQuery();
                        serviceMatyFilter.SERVICE_IDs = listIds.Select(s => s.SERVICE_ID).ToList();
                        listServiceMatys.AddRange(new HisServiceMatyManager(param).GetView(serviceMatyFilter));
                    }
                }

                List<long> lstTreatmentId = listSereServs.Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                List<long> patientIds = new List<long>();

                skip = 0;
                while (lstTreatmentId.Count - skip > 0)
                {
                    var listId = lstTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                    treatmentFilter.IDs = listId;
                    var treat = new HisTreatmentManager(param).Get(treatmentFilter);
                    if (IsNotNullOrEmpty(treat))
                    {
                        patientIds.AddRange(treat.Select(s => s.PATIENT_ID).ToList());
                        foreach (var item in treat)
                        {
                            DicTreatment[item.ID] = item;
                        }
                    }
                }

                if (IsNotNullOrEmpty(patientIds))
                {
                    patientIds = patientIds.Distinct().ToList();

                    skip = 0;
                    while (patientIds.Count - skip > 0)
                    {
                        var listId = patientIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisPatientFilterQuery patientFilter = new HisPatientFilterQuery();
                        patientFilter.IDs = listId;
                        var patient = new HisPatientManager(param).Get(patientFilter);
                        if (IsNotNullOrEmpty(patient))
                        {
                            foreach (var item in patient)
                            {
                                DicPatient[item.ID] = item;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();

                foreach (var sereServ in listSereServs)
                {
                    var rdo = new Mrs00456RDO();
                    rdo.REQUEST_DEPARTMENT_ID = sereServ.TDL_REQUEST_DEPARTMENT_ID;
                    rdo.REQUEST_DEPARTMENT_NAME = sereServ.REQUEST_DEPARTMENT_NAME;
                    rdo.TREATMENT_ID = sereServ.TDL_TREATMENT_ID ?? 0;
                    #region hh
                    if (listServiceTBM != null && IsNotNullOrEmpty(listServiceTBM.Where(w => w == sereServ.SERVICE_ID).ToList()))
                    {
                        rdo.TOTAL_TBM = sereServ.AMOUNT;
                        rdo.TOTAL_HH = sereServ.AMOUNT;
                        rdo.TOTAL_PRICE_HH = sereServ.AMOUNT * sereServ.PRICE;
                        var tieuBan = listServiceMatys.Where(s => s.SERVICE_ID == sereServ.SERVICE_ID).ToList();
                        if (IsNotNullOrEmpty(tieuBan))
                            rdo.TOTAL_PATIENT_TBHH = tieuBan.First().EXPEND_AMOUNT;

                        SetInfoPlus(rdo, sereServ, 1);
                    }
                    else if (listServiceMDMC != null && IsNotNullOrEmpty(listServiceMDMC.Where(w => w == sereServ.SERVICE_ID).ToList()))
                    {
                        rdo.TOTAL_MDMC = sereServ.AMOUNT;
                        rdo.TOTAL_HH = sereServ.AMOUNT;
                        rdo.TOTAL_PRICE_HH = sereServ.AMOUNT * sereServ.PRICE;
                        var tieuBan = listServiceMatys.Where(s => s.SERVICE_ID == sereServ.SERVICE_ID).ToList();
                        if (IsNotNullOrEmpty(tieuBan))
                            rdo.TOTAL_PATIENT_TBHH = tieuBan.First().EXPEND_AMOUNT;

                        SetInfoPlus(rdo, sereServ, 1);
                    }
                    else if (listServiceNM != null && IsNotNullOrEmpty(listServiceNM.Where(w => w == sereServ.SERVICE_ID).ToList()))
                    {
                        rdo.TOTAL_NM = sereServ.AMOUNT;
                        rdo.TOTAL_HH = sereServ.AMOUNT;
                        rdo.TOTAL_PRICE_HH = sereServ.AMOUNT * sereServ.PRICE;
                        var tieuBan = listServiceMatys.Where(s => s.SERVICE_ID == sereServ.SERVICE_ID).ToList();
                        if (IsNotNullOrEmpty(tieuBan))
                            rdo.TOTAL_PATIENT_TBHH = tieuBan.First().EXPEND_AMOUNT;

                        SetInfoPlus(rdo, sereServ, 1);
                    }
                    else if (listServiceML != null && IsNotNullOrEmpty(listServiceML.Where(w => w == sereServ.SERVICE_ID).ToList()))
                    {
                        rdo.TOTAL_ML = sereServ.AMOUNT;
                        rdo.TOTAL_HH = sereServ.AMOUNT;
                        rdo.TOTAL_PRICE_HH = sereServ.AMOUNT * sereServ.PRICE;
                        var tieuBan = listServiceMatys.Where(s => s.SERVICE_ID == sereServ.SERVICE_ID).ToList();
                        if (IsNotNullOrEmpty(tieuBan))
                            rdo.TOTAL_PATIENT_TBHH = tieuBan.First().EXPEND_AMOUNT;

                        SetInfoPlus(rdo, sereServ, 1);
                    }
                    else if (listServiceHIV != null && IsNotNullOrEmpty(listServiceHIV.Where(w => w == sereServ.SERVICE_ID).ToList()))
                    {
                        rdo.TOTAL_HIV = sereServ.AMOUNT;
                        rdo.TOTAL_HH = sereServ.AMOUNT;
                        rdo.TOTAL_PRICE_HH = sereServ.AMOUNT * sereServ.PRICE;
                        var tieuBan = listServiceMatys.Where(s => s.SERVICE_ID == sereServ.SERVICE_ID).ToList();
                        if (IsNotNullOrEmpty(tieuBan))
                            rdo.TOTAL_PATIENT_TBHH = tieuBan.First().EXPEND_AMOUNT;

                        SetInfoPlus(rdo, sereServ, 1);
                    }
                    else if (listServiceHbsAg != null && IsNotNullOrEmpty(listServiceHbsAg.Where(w => w == sereServ.SERVICE_ID).ToList()))
                    {
                        rdo.TOTAL_HBSAG = sereServ.AMOUNT;
                        rdo.TOTAL_HH = sereServ.AMOUNT;
                        rdo.TOTAL_PRICE_HH = sereServ.AMOUNT * sereServ.PRICE;
                        var tieuBan = listServiceMatys.Where(s => s.SERVICE_ID == sereServ.SERVICE_ID).ToList();
                        if (IsNotNullOrEmpty(tieuBan))
                            rdo.TOTAL_PATIENT_TBHH = tieuBan.First().EXPEND_AMOUNT;

                        SetInfoPlus(rdo, sereServ, 1);
                    }
                    else if (listServiceSR != null && IsNotNullOrEmpty(listServiceSR.Where(w => w == sereServ.SERVICE_ID).ToList()))
                    {
                        rdo.TOTAL_SR = sereServ.AMOUNT;
                        rdo.TOTAL_HH = sereServ.AMOUNT;
                        rdo.TOTAL_PRICE_HH = sereServ.AMOUNT * sereServ.PRICE;
                        var tieuBan = listServiceMatys.Where(s => s.SERVICE_ID == sereServ.SERVICE_ID).ToList();
                        if (IsNotNullOrEmpty(tieuBan))
                            rdo.TOTAL_PATIENT_TBHH = tieuBan.First().EXPEND_AMOUNT;

                        SetInfoPlus(rdo, sereServ, 1);
                    }
                    #endregion
                    #region sh
                    if (listServiceSHM != null && IsNotNullOrEmpty(listServiceSHM.Where(w => w == sereServ.SERVICE_ID).ToList()))
                    {
                        rdo.TOTAL_SH = sereServ.AMOUNT;
                        rdo.TOTAL_PRICE_SH = sereServ.AMOUNT * sereServ.PRICE;
                        rdo.TOTAL_SHM = sereServ.AMOUNT;
                        rdo.TOTAL_PRICE_SHM = sereServ.AMOUNT * sereServ.PRICE;
                        var tieuBan = listServiceMatys.Where(s => s.SERVICE_ID == sereServ.SERVICE_ID).ToList();
                        if (IsNotNullOrEmpty(tieuBan))
                        {
                            rdo.TOTAL_PATIENT_TBSH = tieuBan.First().EXPEND_AMOUNT;
                            rdo.TOTAL_PATIENT_TBSHM = tieuBan.First().EXPEND_AMOUNT;
                        }

                        SetInfoPlus(rdo, sereServ, 2);
                    }
                    else if (listServiceNT != null && IsNotNullOrEmpty(listServiceNT.Where(w => w == sereServ.SERVICE_ID).ToList()))
                    {
                        rdo.TOTAL_SH = sereServ.AMOUNT;
                        rdo.TOTAL_PRICE_SH = sereServ.AMOUNT * sereServ.PRICE;
                        rdo.TOTAL_NT = sereServ.AMOUNT;
                        rdo.TOTAL_PRICE_NT = sereServ.AMOUNT * sereServ.PRICE;
                        var tieuBan = listServiceMatys.Where(s => s.SERVICE_ID == sereServ.SERVICE_ID).ToList();
                        if (IsNotNullOrEmpty(tieuBan))
                        {
                            rdo.TOTAL_PATIENT_TBSH = tieuBan.First().EXPEND_AMOUNT;
                            rdo.TOTAL_PATIENT_TBNT = tieuBan.First().EXPEND_AMOUNT;
                        }

                        SetInfoPlus(rdo, sereServ, 3);
                    }
                    else if (listServiceXND != null && IsNotNullOrEmpty(listServiceXND.Where(w => w == sereServ.SERVICE_ID).ToList()))
                    {
                        rdo.TOTAL_SH = sereServ.AMOUNT;
                        rdo.TOTAL_PRICE_SH = sereServ.AMOUNT * sereServ.PRICE;
                        rdo.TOTAL_XND = sereServ.AMOUNT;
                        rdo.TOTAL_PRICE_XND = sereServ.AMOUNT * sereServ.PRICE;
                        var tieuBan = listServiceMatys.Where(s => s.SERVICE_ID == sereServ.SERVICE_ID).ToList();
                        if (IsNotNullOrEmpty(tieuBan))
                        {
                            rdo.TOTAL_PATIENT_TBSH = tieuBan.First().EXPEND_AMOUNT;
                            rdo.TOTAL_PATIENT_TBXND = tieuBan.First().EXPEND_AMOUNT;
                        }

                        SetInfoPlus(rdo, sereServ, 4);
                    }
                    #endregion
                    #region vk
                    if (listServiceVK != null && IsNotNullOrEmpty(listServiceVK.Where(w => w == sereServ.SERVICE_ID).ToList()))
                    {
                        rdo.TOTAL_VK = sereServ.AMOUNT;
                        rdo.TOTAL_PRICE_VK = sereServ.AMOUNT * sereServ.PRICE;
                        var tieuBan = listServiceMatys.Where(s => s.SERVICE_ID == sereServ.SERVICE_ID).ToList();
                        if (IsNotNullOrEmpty(tieuBan))
                        {
                            rdo.TOTAL_PATIENT_TBVK = tieuBan.First().EXPEND_AMOUNT;
                        }

                        SetInfoPlus(rdo, sereServ, 5);
                    }
                    if (listServiceLAO != null && IsNotNullOrEmpty(listServiceLAO.Where(w => w == sereServ.SERVICE_ID).ToList()))
                    {
                        rdo.TOTAL_LAO = sereServ.AMOUNT;
                    }
                    else if (listServiceKST != null && IsNotNullOrEmpty(listServiceKST.Where(w => w == sereServ.SERVICE_ID).ToList()))
                    {
                        rdo.TOTAL_KST = sereServ.AMOUNT;
                    }
                    #endregion

                    listRdo.Add(rdo);
                }

                listRdo = listRdo.GroupBy(g => new { g.REQUEST_DEPARTMENT_ID, g.TREATMENT_ID }).Select(s => new Mrs00456RDO
                {
                    REQUEST_DEPARTMENT_ID = s.First().REQUEST_DEPARTMENT_ID,
                    REQUEST_DEPARTMENT_NAME = s.First().REQUEST_DEPARTMENT_NAME,
                    TREATMENT_ID = s.First().TREATMENT_ID,

                    //hh
                    TOTAL_PATIENT_TBHH = s.Sum(su => su.TOTAL_PATIENT_TBHH),
                    TOTAL_HH = s.Sum(su => su.TOTAL_HH),
                    TOTAL_PRICE_HH = s.Sum(su => su.TOTAL_PRICE_HH),
                    TOTAL_TBM = s.Sum(su => su.TOTAL_TBM),
                    TOTAL_MDMC = s.Sum(su => su.TOTAL_MDMC),
                    TOTAL_NM = s.Sum(su => su.TOTAL_NM),
                    TOTAL_ML = s.Sum(su => su.TOTAL_ML),
                    TOTAL_HIV = s.Sum(su => su.TOTAL_HIV),
                    TOTAL_HBSAG = s.Sum(su => su.TOTAL_HBSAG),
                    TOTAL_SR = s.Sum(su => su.TOTAL_SR),
                    TOTAL_HH_BHYT = s.First().TOTAL_HH_BHYT,
                    TOTAL_HH_NU = s.First().TOTAL_HH_NU,
                    TOTAL_HH_15 = s.First().TOTAL_HH_15,
                    TOTAL_HH_DTTS = s.First().TOTAL_HH_DTTS,

                    // sh
                    TOTAL_PATIENT_TBSH = s.Sum(su => su.TOTAL_PATIENT_TBSH),
                    TOTAL_PRICE_SH = s.Sum(su => su.TOTAL_PRICE_SH),
                    TOTAL_SH = s.Sum(su => su.TOTAL_SH),

                    TOTAL_PATIENT_TBSHM = s.Sum(su => su.TOTAL_PATIENT_TBSHM),
                    TOTAL_PRICE_SHM = s.Sum(su => su.TOTAL_PRICE_SHM),
                    TOTAL_SHM = s.Sum(su => su.TOTAL_SHM),
                    TOTAL_SHM_BHYT = s.First().TOTAL_SHM_BHYT,
                    TOTAL_SHM_NU = s.First().TOTAL_SHM_NU,
                    TOTAL_SHM_15 = s.First().TOTAL_SHM_15,
                    TOTAL_SHM_DTTS = s.First().TOTAL_SHM_DTTS,

                    TOTAL_PATIENT_TBNT = s.Sum(su => su.TOTAL_PATIENT_TBNT),
                    TOTAL_PRICE_NT = s.Sum(su => su.TOTAL_PRICE_NT),
                    TOTAL_NT = s.Sum(su => su.TOTAL_NT),
                    TOTAL_NT_BHYT = s.First().TOTAL_NT_BHYT,
                    TOTAL_NT_NU = s.First().TOTAL_NT_NU,
                    TOTAL_NT_15 = s.First().TOTAL_NT_15,
                    TOTAL_NT_DTTS = s.First().TOTAL_NT_DTTS,

                    TOTAL_PATIENT_TBXND = s.Sum(su => su.TOTAL_PATIENT_TBXND),
                    TOTAL_PRICE_XND = s.Sum(su => su.TOTAL_PRICE_XND),
                    TOTAL_XND = s.Sum(su => su.TOTAL_XND),
                    TOTAL_XND_BHYT = s.First().TOTAL_XND_BHYT,
                    TOTAL_XND_NU = s.First().TOTAL_XND_NU,
                    TOTAL_XND_15 = s.First().TOTAL_XND_15,
                    TOTAL_XND_DTTS = s.First().TOTAL_XND_DTTS,

                    // vk
                    TOTAL_PATIENT_TBVK = s.Sum(su => su.TOTAL_PATIENT_TBVK),
                    TOTAL_PRICE_VK = s.Sum(su => su.TOTAL_PRICE_VK),
                    TOTAL_VK = s.Sum(su => su.TOTAL_VK),
                    TOTAL_LAO = s.Sum(su => su.TOTAL_LAO),
                    TOTAL_KST = s.Sum(su => su.TOTAL_KST),
                    TOTAL_VK_BHYT = s.First().TOTAL_VK_BHYT,
                    TOTAL_VK_NU = s.First().TOTAL_VK_NU,
                    TOTAL_VK_15 = s.First().TOTAL_VK_15,
                    TOTAL_VK_DTTS = s.First().TOTAL_VK_DTTS
                }).ToList();

                listRdo = listRdo.GroupBy(g => g.REQUEST_DEPARTMENT_ID).Select(s => new Mrs00456RDO
                {
                    REQUEST_DEPARTMENT_ID = s.First().REQUEST_DEPARTMENT_ID,
                    REQUEST_DEPARTMENT_NAME = s.First().REQUEST_DEPARTMENT_NAME,

                    TOTAL_PATIENT = s.Count(),
                    // hh
                    TOTAL_PATIENT_HH = s.Where(w => w.TOTAL_TBM > 0 || w.TOTAL_MDMC > 0 || w.TOTAL_NM > 0 || w.TOTAL_ML > 0 || w.TOTAL_HIV > 0 || w.TOTAL_HBSAG > 0 || w.TOTAL_SR > 0).Count(),

                    TOTAL_PATIENT_TBHH = s.Sum(su => su.TOTAL_PATIENT_TBHH),
                    TOTAL_HH = s.Sum(su => su.TOTAL_HH),
                    TOTAL_PRICE_HH = s.Sum(su => su.TOTAL_PRICE_HH),
                    TOTAL_TBM = s.Sum(su => su.TOTAL_TBM),
                    TOTAL_MDMC = s.Sum(su => su.TOTAL_MDMC),
                    TOTAL_NM = s.Sum(su => su.TOTAL_NM),
                    TOTAL_ML = s.Sum(su => su.TOTAL_ML),
                    TOTAL_HIV = s.Sum(su => su.TOTAL_HIV),
                    TOTAL_HBSAG = s.Sum(su => su.TOTAL_HBSAG),
                    TOTAL_SR = s.Sum(su => su.TOTAL_SR),
                    TOTAL_HH_BHYT = s.Sum(su => su.TOTAL_HH_BHYT),
                    TOTAL_HH_NU = s.Sum(su => su.TOTAL_HH_NU),
                    TOTAL_HH_15 = s.Sum(su => su.TOTAL_HH_15),
                    TOTAL_HH_DTTS = s.Sum(su => su.TOTAL_HH_DTTS),

                    // sh
                    TOTAL_PATIENT_SH = s.Where(w => w.TOTAL_SH > 0).Count(),
                    TOTAL_PATIENT_SHM = s.Where(w => w.TOTAL_SHM > 0).Count(),
                    TOTAL_PATIENT_NT = s.Where(w => w.TOTAL_NT > 0).Count(),
                    TOTAL_PATIENT_XND = s.Where(w => w.TOTAL_XND > 0).Count(),

                    TOTAL_PATIENT_TBSH = s.Sum(su => su.TOTAL_PATIENT_TBSH),
                    TOTAL_PRICE_SH = s.Sum(su => su.TOTAL_PRICE_SH),
                    TOTAL_SH = s.Sum(su => su.TOTAL_SH),

                    TOTAL_PATIENT_TBSHM = s.Sum(su => su.TOTAL_PATIENT_TBSHM),
                    TOTAL_PRICE_SHM = s.Sum(su => su.TOTAL_PRICE_SHM),
                    TOTAL_SHM = s.Sum(su => su.TOTAL_SHM),
                    TOTAL_SHM_BHYT = s.Sum(su => su.TOTAL_SHM_BHYT),
                    TOTAL_SHM_NU = s.Sum(su => su.TOTAL_SHM_NU),
                    TOTAL_SHM_15 = s.Sum(su => su.TOTAL_SHM_15),
                    TOTAL_SHM_DTTS = s.Sum(su => su.TOTAL_SHM_DTTS),

                    TOTAL_PATIENT_TBNT = s.Sum(su => su.TOTAL_PATIENT_TBNT),
                    TOTAL_PRICE_NT = s.Sum(su => su.TOTAL_PRICE_NT),
                    TOTAL_NT = s.Sum(su => su.TOTAL_NT),
                    TOTAL_NT_BHYT = s.Sum(su => su.TOTAL_NT_BHYT),
                    TOTAL_NT_NU = s.Sum(su => su.TOTAL_NT_NU),
                    TOTAL_NT_15 = s.Sum(su => su.TOTAL_NT_15),
                    TOTAL_NT_DTTS = s.Sum(su => su.TOTAL_NT_DTTS),

                    TOTAL_PATIENT_TBXND = s.Sum(su => su.TOTAL_PATIENT_TBXND),
                    TOTAL_PRICE_XND = s.Sum(su => su.TOTAL_PRICE_XND),
                    TOTAL_XND = s.Sum(su => su.TOTAL_XND),
                    TOTAL_XND_BHYT = s.Sum(su => su.TOTAL_XND_BHYT),
                    TOTAL_XND_NU = s.Sum(su => su.TOTAL_XND_NU),
                    TOTAL_XND_15 = s.Sum(su => su.TOTAL_XND_15),
                    TOTAL_XND_DTTS = s.Sum(su => su.TOTAL_XND_DTTS),

                    // vk
                    TOTAL_PATIENT_VK = s.Where(w => w.TOTAL_VK > 0).Count(),

                    TOTAL_PATIENT_TBVK = s.Sum(su => su.TOTAL_PATIENT_TBVK),
                    TOTAL_PRICE_VK = s.Sum(su => su.TOTAL_PRICE_VK),
                    TOTAL_VK = s.Sum(su => su.TOTAL_VK),
                    TOTAL_LAO = s.Sum(su => su.TOTAL_LAO),
                    TOTAL_KST = s.Sum(su => su.TOTAL_KST),
                    TOTAL_VK_BHYT = s.Sum(su => su.TOTAL_VK_BHYT),
                    TOTAL_VK_NU = s.Sum(su => su.TOTAL_VK_NU),
                    TOTAL_VK_15 = s.Sum(su => su.TOTAL_VK_15),
                    TOTAL_VK_DTTS = s.Sum(su => su.TOTAL_VK_DTTS)
                }).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rdo"></param>
        /// <param name="sereServ"></param>
        /// <param name="type">1: HH |2: SHM |3: NT |4: XND |5: VK</param>
        private void SetInfoPlus(Mrs00456RDO rdo, V_HIS_SERE_SERV sereServ, int type)
        {
            try
            {
                if (rdo == null || sereServ == null) return;

                if (sereServ.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    if (type == 1)
                        rdo.TOTAL_HH_BHYT = 1;
                    else if (type == 2)
                        rdo.TOTAL_SHM_BHYT = 1;
                    else if (type == 3)
                        rdo.TOTAL_NT_BHYT = 1;
                    else if (type == 4)
                        rdo.TOTAL_XND_BHYT = 1;
                    else if (type == 5)
                        rdo.TOTAL_VK_BHYT = 1;
                }

                if (DicTreatment.ContainsKey(sereServ.TDL_TREATMENT_ID ?? 0))
                {
                    if (DicTreatment[sereServ.TDL_TREATMENT_ID ?? 0].TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        if (type == 1)
                            rdo.TOTAL_HH_NU = 1;
                        else if (type == 2)
                            rdo.TOTAL_SHM_NU = 1;
                        else if (type == 3)
                            rdo.TOTAL_NT_NU = 1;
                        else if (type == 4)
                            rdo.TOTAL_XND_NU = 1;
                        else if (type == 5)
                            rdo.TOTAL_VK_NU = 1;
                    }

                    if (Inventec.Common.DateTime.Calculation.Age(DicTreatment[sereServ.TDL_TREATMENT_ID ?? 0].TDL_PATIENT_DOB, DicTreatment[sereServ.TDL_TREATMENT_ID ?? 0].IN_TIME) < 15)
                    {
                        if (type == 1)
                            rdo.TOTAL_HH_15 = 1;
                        else if (type == 2)
                            rdo.TOTAL_SHM_15 = 1;
                        else if (type == 3)
                            rdo.TOTAL_NT_15 = 1;
                        else if (type == 4)
                            rdo.TOTAL_XND_15 = 1;
                        else if (type == 5)
                            rdo.TOTAL_VK_15 = 1;
                    }

                    if (DicPatient.ContainsKey(DicTreatment[sereServ.TDL_TREATMENT_ID ?? 0].PATIENT_ID))
                    {
                        string ethnic = DicPatient[DicTreatment[sereServ.TDL_TREATMENT_ID ?? 0].PATIENT_ID].ETHNIC_NAME;
                        if (!String.IsNullOrWhiteSpace(ethnic) && ethnic.Trim().ToLower() == "kinh")
                        {
                            rdo.TOTAL_HH_DTTS = 1;
                            if (type == 1)
                                rdo.TOTAL_HH_DTTS = 1;
                            else if (type == 2)
                                rdo.TOTAL_SHM_DTTS = 1;
                            else if (type == 3)
                                rdo.TOTAL_NT_DTTS = 1;
                            else if (type == 4)
                                rdo.TOTAL_XND_DTTS = 1;
                            else if (type == 5)
                                rdo.TOTAL_VK_DTTS = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);
                }

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(s => s.REQUEST_DEPARTMENT_NAME).ToList());
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
