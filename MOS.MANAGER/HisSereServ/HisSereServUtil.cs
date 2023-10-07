using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOS.MANAGER.HisSereServ
{
    class HisSereServUtil
    {
        //Luu du thua du lieu
        public static void SetTdl(HIS_SERE_SERV sereServ, HIS_SERVICE_REQ serviceReq)
        {
            if (sereServ != null && serviceReq != null)
            {
                HIS_DEPARTMENT executeDepartment = HisDepartmentCFG.DATA.Where(o => o.ID == serviceReq.EXECUTE_DEPARTMENT_ID).FirstOrDefault();
                sereServ.TDL_PATIENT_ID = serviceReq.TDL_PATIENT_ID;
                sereServ.TDL_EXECUTE_DEPARTMENT_ID = serviceReq.EXECUTE_DEPARTMENT_ID;
                sereServ.TDL_EXECUTE_ROOM_ID = serviceReq.EXECUTE_ROOM_ID;
                sereServ.TDL_REQUEST_DEPARTMENT_ID = serviceReq.REQUEST_DEPARTMENT_ID;
                sereServ.TDL_REQUEST_LOGINNAME = serviceReq.REQUEST_LOGINNAME;
                sereServ.TDL_REQUEST_ROOM_ID = serviceReq.REQUEST_ROOM_ID;
                sereServ.TDL_REQUEST_USERNAME = serviceReq.REQUEST_USERNAME;
                sereServ.TDL_SERVICE_REQ_CODE = serviceReq.SERVICE_REQ_CODE;
                sereServ.TDL_TREATMENT_CODE = serviceReq.TDL_TREATMENT_CODE;
                sereServ.TDL_TREATMENT_ID = serviceReq.TREATMENT_ID;
                sereServ.TDL_INTRUCTION_TIME = serviceReq.INTRUCTION_TIME;
                sereServ.TDL_INTRUCTION_DATE = sereServ.TDL_INTRUCTION_TIME - sereServ.TDL_INTRUCTION_TIME % 1000000;
                sereServ.TDL_EXECUTE_BRANCH_ID = executeDepartment.BRANCH_ID;

                V_HIS_SERVICE service = HisServiceCFG.DATA_VIEW != null ? HisServiceCFG.DATA_VIEW.Where(o => o.ID == sereServ.SERVICE_ID).FirstOrDefault() : null;
                if (service != null)
                {
                    sereServ.TDL_HEIN_ORDER = service.HEIN_ORDER;
                    sereServ.TDL_HEIN_SERVICE_BHYT_CODE = service.HEIN_SERVICE_BHYT_CODE;
                    sereServ.TDL_HEIN_SERVICE_BHYT_NAME = service.HEIN_SERVICE_BHYT_NAME;
                    sereServ.TDL_HEIN_SERVICE_TYPE_ID = service.HEIN_SERVICE_TYPE_ID;
                    sereServ.TDL_SERVICE_CODE = service.SERVICE_CODE;
                    sereServ.TDL_SPECIALITY_CODE = service.SPECIALITY_CODE;
                    sereServ.TDL_SERVICE_NAME = service.SERVICE_NAME;
                    sereServ.TDL_SERVICE_TYPE_ID = service.SERVICE_TYPE_ID;
                    sereServ.TDL_SERVICE_UNIT_ID = service.SERVICE_UNIT_ID;
                    sereServ.TDL_ACTIVE_INGR_BHYT_CODE = service.ACTIVE_INGR_BHYT_CODE;
                    sereServ.TDL_ACTIVE_INGR_BHYT_NAME = service.ACTIVE_INGR_BHYT_NAME;
                }
            }
        }

        //Luu du thua du lieu
        public static void SetTdl(HIS_SERE_SERV sereServ, long patientId, HIS_SERVICE_REQ serviceReq, V_HIS_MEDICINE medicine)
        {
            HisSereServUtil.SetTdl(sereServ, serviceReq);
            if (medicine != null)
            {
                sereServ.TDL_MEDICINE_BID_NUM_ORDER = medicine.TDL_BID_NUM_ORDER;
                sereServ.TDL_MEDICINE_CONCENTRA = medicine.CONCENTRA;
                sereServ.TDL_MEDICINE_PACKAGE_NUMBER = medicine.PACKAGE_NUMBER;
                sereServ.TDL_MEDICINE_REGISTER_NUMBER = medicine.REGISTER_NUMBER;
            }
        }

        public static void CopyInformation(HIS_SERE_SERV old, HIS_SERE_SERV tmp)
        {
            old.ADD_PRICE = tmp.ADD_PRICE;
            old.AMOUNT = tmp.AMOUNT;
            old.BLOOD_ID = tmp.BLOOD_ID;
            old.DISCOUNT = tmp.DISCOUNT;
            old.EKIP_ID = tmp.EKIP_ID;
            old.EXECUTE_TIME = tmp.EXECUTE_TIME;
            old.HEIN_APPROVAL_ID = tmp.HEIN_APPROVAL_ID;
            old.HEIN_CARD_NUMBER = tmp.HEIN_CARD_NUMBER;
            old.HEIN_LIMIT_PRICE = tmp.HEIN_LIMIT_PRICE;
            old.HEIN_LIMIT_RATIO = tmp.HEIN_LIMIT_RATIO;
            old.HEIN_NORMAL_PRICE = tmp.HEIN_NORMAL_PRICE;
            old.HEIN_PRICE = tmp.HEIN_PRICE;
            old.HEIN_RATIO = tmp.HEIN_RATIO;
            old.INVOICE_ID = tmp.INVOICE_ID;
            old.IS_EXPEND = tmp.IS_EXPEND;
            old.IS_NO_EXECUTE = tmp.IS_NO_EXECUTE;
            old.IS_NO_PAY = tmp.IS_NO_PAY;
            old.IS_OUT_PARENT_FEE = tmp.IS_OUT_PARENT_FEE;
            old.IS_SPECIMEN = tmp.IS_SPECIMEN;
            old.JSON_PATIENT_TYPE_ALTER = tmp.JSON_PATIENT_TYPE_ALTER;
            old.MATERIAL_ID = tmp.MATERIAL_ID;
            old.MEDICINE_ID = tmp.MEDICINE_ID;
            old.ORIGINAL_PRICE = tmp.ORIGINAL_PRICE;
            old.PARENT_ID = tmp.PARENT_ID;
            old.PATIENT_TYPE_ID = tmp.PATIENT_TYPE_ID;
            old.PARENT_ID = tmp.PARENT_ID;
            old.PRICE = tmp.PRICE;
            old.TDL_INTRUCTION_TIME = tmp.TDL_INTRUCTION_TIME;
            old.TDL_INTRUCTION_DATE = tmp.TDL_INTRUCTION_DATE;
            old.SERVICE_ID = tmp.SERVICE_ID;
            old.SERVICE_REQ_ID = tmp.SERVICE_REQ_ID;
            old.TDL_TREATMENT_ID = tmp.TDL_TREATMENT_ID;
            old.VAT_RATIO = tmp.VAT_RATIO;
            old.OVERTIME_PRICE = tmp.OVERTIME_PRICE;
            old.IS_ADDITION = tmp.IS_ADDITION;
            old.IS_DELETE = tmp.IS_DELETE;
            old.IS_ACTIVE = tmp.IS_ACTIVE;
        }
    }
}
