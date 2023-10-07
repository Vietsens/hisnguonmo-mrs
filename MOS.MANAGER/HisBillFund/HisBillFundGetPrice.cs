using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
//using MOS.LibraryHein.HcmPoorFund;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatment;
//using MOS.MANAGER.Token;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBillFund
{
    partial class HisBillFundGet : BusinessBase
    {
        //internal decimal? GetPriceForPoorFund(long treatmentId)
        //{
        //    try
        //    {
        //        HIS_PATIENT_TYPE_ALTER patientTypeAlter = new HisPatientTypeAlterGet().GetLastByTreatmentId(treatmentId);
        //        if (patientTypeAlter != null && patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
        //        {
        //            HisSereServFilterQuery filter = new HisSereServFilterQuery();
        //            filter.IS_EXPEND = false;
        //            filter.HAS_EXECUTE = true;
        //            filter.TREATMENT_ID = treatmentId;

        //            List<HIS_SERE_SERV> hisSereServs = new HisSereServGet().Get(filter);
        //            if (IsNotNullOrEmpty(hisSereServs))
        //            {
        //                //HIS_BRANCH branch = new TokenManager(param).GetBranch();
        //                PoorFundPriceCalculator calculator = new PoorFundPriceCalculator(branch.HEIN_PROVINCE_CODE, HcmPoorFundCFG.VCN_ACCEPT_SERVICE_IDS, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
        //                return calculator.GetPaidAmount(hisSereServs, patientTypeAlter.HNCODE, patientTypeAlter.HEIN_CARD_NUMBER);
        //            }   
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogSystem.Error(ex);
        //        param.HasException = true;
        //        return null;
        //    }
        //}

    }
}
