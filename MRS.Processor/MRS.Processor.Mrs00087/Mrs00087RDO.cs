using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisPatient;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00087
{
    class Mrs00087RDO
    {
        public long CREATE_TIME { get;  set;  }
        public string CREATE_DATE_STR { get;  set;  }
        public string TRANSACTION_CODE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }

        public decimal AMOUNT { get;  set;  }
        public decimal TEST_PRICE { get;  set;  }
        public decimal DIIM_FUEX_PRICE { get;  set;  }
        public decimal MEDI_PRICE { get;  set;  }
        public decimal SURG_MISU_PRICE { get;  set;  }
        public decimal BLOOD_PRICE { get;  set;  }
        public decimal MATE_PRICE { get;  set;  }
        public decimal OTHER_PRICE { get;  set;  }

        public Mrs00087RDO(V_HIS_TRANSACTION Bill)
        {
            try
            {
                TRANSACTION_CODE = Bill.TRANSACTION_CODE; 
                PATIENT_CODE = Bill.TDL_PATIENT_CODE; 
                VIR_PATIENT_NAME = Bill.TDL_PATIENT_NAME; 
                AMOUNT = Bill.AMOUNT;
                CREATE_TIME = Bill.TRANSACTION_TIME;
                CREATE_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(Bill.TRANSACTION_TIME); 
                ProcessDetailPrice(Bill); 
                ProcessAddressPatient(Bill); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessAddressPatient(V_HIS_TRANSACTION data)
        {
            try
            {
                V_HIS_PATIENT patient = new MOS.MANAGER.HisPatient.HisPatientManager().GetViewById(data.TDL_PATIENT_ID ?? 0); 
                if (patient != null)
                {
                    VIR_ADDRESS = patient.VIR_ADDRESS; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessDetailPrice(V_HIS_TRANSACTION data)
        {
            try
            {
                HisSereServBillViewFilterQuery filterSereServBill = new HisSereServBillViewFilterQuery(); 
                filterSereServBill.BILL_ID = data.ID; 
                var listSereServBillSub = new MOS.MANAGER.HisSereServBill.HisSereServBillManager(new CommonParam()).GetView(filterSereServBill); 

                HisSereServViewFilterQuery filter = new HisSereServViewFilterQuery(); 
                filter.IDs = listSereServBillSub.Select(o => o.SERE_SERV_ID).ToList(); 
                var hisSereServs = new MOS.MANAGER.HisSereServ.HisSereServManager().GetView(filter); 
                if (hisSereServs != null && hisSereServs.Count > 0)
                {
                    foreach (var sereServ in hisSereServs)
                    {
                        if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN)// Config.IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN)
                        {
                            TEST_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0; 
                        }
                        else if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA
                            /* Config.IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA*/ ||
                            sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN
                            /*Config.IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN*/ ||
                            sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA)
                        //Config.IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA)
                        {
                            DIIM_FUEX_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0; 
                        }
                        else if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC)
                        //Config.IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM)
                        {
                            MEDI_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0; 
                        }
                        else if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT
                            //Config.IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT ||
                            || sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT
                            //Config.IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT 
                            || sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS)
                        //Config.IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS)
                        {
                            SURG_MISU_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0; 
                        }
                        else if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT)
                        //Config.IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM)
                        {
                            MATE_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0; 
                        }
                        else
                        {
                            OTHER_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
