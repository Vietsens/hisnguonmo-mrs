using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMedicineType;
using Inventec.Common.FlexCellExport;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMestPeriodMety;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00003
{
    public class Mrs00003Processor : AbstractProcessor
    {
        Mrs00003Filter castFilter = null; 
        List<Mrs00003RDO> listRdo = new List<Mrs00003RDO>(); 
        private decimal beginAmount; 
        private decimal ERROR_BEGIN_AMOUNT = -1; 
        List<V_HIS_EXP_MEST_MEDICINE> listExp; 
        List<V_HIS_IMP_MEST_MEDICINE> listImp; 

        public Mrs00003Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00003Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                castFilter = ((Mrs00003Filter)this.reportFilter); 
                CommonParam getParam = new CommonParam(); 
                HisImpMestMedicineViewFilterQuery impFilter = new HisImpMestMedicineViewFilterQuery(); 
                impFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID; 
                impFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
                if (castFilter.MEDI_STOCK_PERIOD_ID != null)
                {
                    impFilter.MEDI_STOCK_PERIOD_ID = castFilter.MEDI_STOCK_PERIOD_ID.Value; 
                }
                else
                {
                    impFilter.HAS_MEDI_STOCK_PERIOD = false; 
                    if (castFilter.MEDI_STOCK_ID.HasValue)
                    {
                        impFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                    }
                    else
                    {
                        ///Loi code, neu khong gui thong tin ky thi phai gui thong tin kho
                        ///Co tinh gan kho "ao" de khong the xuat ra du lieu
                        impFilter.MEDI_STOCK_ID = -1; 
                        Inventec.Common.Logging.LogSystem.Error("Khong gui thong tin kho cung nhu thong tin ky. Kiem tra lai frontend."); 
                    }
                }
                listImp = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(getParam).GetView(impFilter); 

                HisExpMestMedicineViewFilterQuery expFilter = new HisExpMestMedicineViewFilterQuery(); 
                expFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID; 
                //expFilter.IN_EXECUTE = true; 
                expFilter.IS_EXPORT = true; 
                if (castFilter.MEDI_STOCK_PERIOD_ID.HasValue)
                {
                    expFilter.MEDI_STOCK_PERIOD_ID = castFilter.MEDI_STOCK_PERIOD_ID.Value; 
                }
                else
                {
                    expFilter.HAS_MEDI_STOCK_PERIOD = false; 
                    if (castFilter.MEDI_STOCK_ID.HasValue)
                    {
                        expFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                    }
                    else
                    {
                        ///Loi code, neu khong gui thong tin ky thi phai gui thong tin kho
                        ///Co tinh gan kho "ao" de khong the xuat ra du lieu
                        expFilter.MEDI_STOCK_ID = -1; 
                        Inventec.Common.Logging.LogSystem.Error("Khong gui thong tin kho cung nhu thong tin ky. Kiem tra lai frontend."); 
                    }
                }
                //expFilter.IN_EXECUTE = true; 
                listExp = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(getParam).GetView(expFilter); 


                if (getParam.HasException)
                {
                    result = false; 
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
                ProcessBeginAmount(); 

                List<Mrs00003RDO> listRdoTemp = new List<Mrs00003RDO>(); 
                foreach (var imp in listImp)
                {
                    Mrs00003RDO rdo = new Mrs00003RDO(imp); 
                    listRdoTemp.Add(rdo); 
                }
                foreach (var exp in listExp)
                {
                    Mrs00003RDO rdo = new Mrs00003RDO(exp); 
                    listRdoTemp.Add(rdo); 
                }

                listRdo = listRdoTemp.OrderBy(o => o.EXECUTE_TIME).ToList(); 
                decimal previousEndAmount = beginAmount; 
                foreach (var rdo in listRdo)
                {
                    rdo.CalculateAmount(previousEndAmount); 
                    previousEndAmount = rdo.END_AMOUNT; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void ProcessBeginAmount()
        {
            if (castFilter.MEDI_STOCK_PERIOD_ID.HasValue)
            {
                HIS_MEDI_STOCK_PERIOD period = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager().GetById(castFilter.MEDI_STOCK_PERIOD_ID.Value); 
                if (period != null)
                {
                    if (period.PREVIOUS_ID.HasValue)
                    {
                        SetBeginAmount(period.PREVIOUS_ID.Value); 
                    }
                    else
                    {
                        ///Ky dau tien
                        beginAmount = 0; 
                    }
                }
                else
                {
                    beginAmount = ERROR_BEGIN_AMOUNT; 
                    Logging("Khong xac dinh duoc period tu id truyen len. Kiem tra lai frontend.", LogType.Warn); 
                }
            }
            else
            {
                ///Khong co thong tin ky. NSD muon lay theo ky hien tai (chua chot) ==> Su dung MEDI_STOCK_ID.
                ///Truong hop MEDI_STOCK_ID null thi duoc hieu la truong hop loi cua client.
                if (castFilter.MEDI_STOCK_ID.HasValue)
                {
                    HIS_MEDI_STOCK_PERIOD period = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager().GetTheLast(castFilter.MEDI_STOCK_ID.Value); 
                    if (period != null)
                    {
                        SetBeginAmount(period.ID); 
                    }
                    else
                    {
                        ///Chua co ky nao
                        beginAmount = 0; 
                    }
                }
                else
                {
                    beginAmount = ERROR_BEGIN_AMOUNT; 
                    Logging("Khong xac dinh medi_stock_id. Kiem tra lai frontend.", LogType.Warn); 
                }
            }
        }

        private void SetBeginAmount(long periodId)
        {
            HisMestPeriodMetyFilterQuery mestPeriodMetyFilter = new HisMestPeriodMetyFilterQuery(); 
            mestPeriodMetyFilter.MEDI_STOCK_PERIOD_ID = periodId; 
            mestPeriodMetyFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID; 
            List<HIS_MEST_PERIOD_METY> listMestPeriodMety = new MOS.MANAGER.HisMestPeriodMety.HisMestPeriodMetyManager().Get(mestPeriodMetyFilter); 
            if (listMestPeriodMety != null && listMestPeriodMety.Count > 0)
            {

                ///Luon co 1 ban ghi neu nhu phan tong hop ky du lieu xu ly dung.
                ///Trong truong hop co > 1 ban ghi thi ghi log dong thoi set so dau ky = -1 de NSD nhan dien va thong bao loi nham xu ly
                if (listMestPeriodMety.Count == 1)
                {
                    beginAmount = listMestPeriodMety[0].BEGIN_AMOUNT; 
                }
                else
                {
                    beginAmount = ERROR_BEGIN_AMOUNT; 
                    Logging("Co nhieu hon 1 ban ghi co cung MEDICINE_TYPE_ID va MEDI_STOCK_PERIOD_ID trong bang HIS_MEST_PERIOD_METY. Kiem tra lai unique constraint va code phan tong hop chot ky du lieu.", LogType.Error); 
                }
            }
            else
            {
                ///Ky truoc do khong co du lieu ve loai thuoc
                beginAmount = 0; 
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            #region Cac the Single
            V_HIS_MEDICINE_TYPE medicineType = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager().GetViewById(castFilter.MEDICINE_TYPE_ID); 
            if (medicineType != null)
            {
                dicSingleTag.Add("MEDICINE_TYPE_CODE", medicineType.MEDICINE_TYPE_CODE); 
                dicSingleTag.Add("MEDICINE_TYPE_NAME", medicineType.MEDICINE_TYPE_NAME); 
                dicSingleTag.Add("SERVICE_UNIT_NAME", medicineType.SERVICE_UNIT_NAME); 
                dicSingleTag.Add("NATIONAL_NAME", medicineType.NATIONAL_NAME); 
                dicSingleTag.Add("MANUFACTURER_NAME", medicineType.MANUFACTURER_NAME); 
            }
            if (castFilter.MEDI_STOCK_PERIOD_ID.HasValue)
            {
                V_HIS_MEDI_STOCK_PERIOD period = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager().GetViewById(castFilter.MEDI_STOCK_PERIOD_ID.Value); 
                if (period != null)
                {
                    //dicSingleData.Add("MEDI_STOCK_PERIOD_CODE", period.MEDI_STOCK_PERIOD_CODE); 
                    dicSingleTag.Add("MEDI_STOCK_CODE", period.MEDI_STOCK_CODE); 
                    dicSingleTag.Add("MEDI_STOCK_NAME", period.MEDI_STOCK_NAME); 
                    if (period.CREATE_TIME.HasValue)
                    {
                        //if (period.PREVIOUS_CREATE_TIME.HasValue)
                        //{
                        //		dicSingleData.Add("MEDI_STOCK_PERIOD_FROM_TIME_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(period.PREVIOUS_CREATE_TIME.Value)); 
                        //}
                        dicSingleTag.Add("MEDI_STOCK_PERIOD_TO_TIME_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(period.CREATE_TIME.Value)); 
                    }
                }
            }
            else if (castFilter.MEDI_STOCK_ID.HasValue)
            {
                HIS_MEDI_STOCK mediStock = new MOS.MANAGER.HisMediStock.HisMediStockManager().GetById(castFilter.MEDI_STOCK_ID.Value); 
                dicSingleTag.Add("MEDI_STOCK_PERIOD_CODE", ""); 
                dicSingleTag.Add("MEDI_STOCK_CODE", mediStock.MEDI_STOCK_CODE); 
                dicSingleTag.Add("MEDI_STOCK_NAME", mediStock.MEDI_STOCK_NAME); 
                HIS_MEDI_STOCK_PERIOD period = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager().GetTheLast(castFilter.MEDI_STOCK_ID.Value); 
                if (period != null)
                {
                    dicSingleTag.Add("MEDI_STOCK_PERIOD_FROM_TIME_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(period.CREATE_TIME.Value)); 
                    System.DateTime now = System.DateTime.Now; 
                    dicSingleTag.Add("MEDI_STOCK_PERIOD_TO_TIME_STR", Inventec.Common.DateTime.Get.NowAsTimeString()); 
                }
            }
            #endregion

            objectTag.AddObjectData(store, "Report", listRdo); 
        }
    }
}
