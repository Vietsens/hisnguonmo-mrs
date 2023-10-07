using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisAccountBook;
using System;
using System.Collections.Generic;
using System.Linq;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;

using MRS.MANAGER.Config;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisInvoice;

namespace MRS.Processor.Mrs00644
{
    public class Mrs00644Processor : AbstractProcessor 
    {
        CommonParam paramGet = new CommonParam();

        private List<Mrs00644RDO> mrs00644RDOSereServs = new List<Mrs00644RDO>();
        private List<Mrs00644RDO> Mrs00644RDODepartments = new List<Mrs00644RDO>();
        private decimal TotalAmount = 0, testFuexPriceSum = 0, diimPriceSum = 0, medicinePriceSum = 0, bloodPriceSum = 0, surgMisuPriceSum = 0, materialPriceSum = 0, examPriceSum = 0, otherPriceSum = 0, tranPriceSum = 0, bedPriceSum = 0, totalPriceSum = 0, totalHeinPriceSum = 0, totalPrice100Sum = 0, totalPrice5Sum = 0, totalPrice20Sum = 0, totalPrice0Sum = 0, patientPricePaySum = 0, endoPriceSum = 0, suimPriceSum = 0;
        Mrs00644Filter filter = null;
        List<HIS_INVOICE_CANCEL> listInvoiceCancel = new List<HIS_INVOICE_CANCEL>();
        List<SAR_PRINT_LOG_D> listPrintLog = new List<SAR_PRINT_LOG_D>();
        public Mrs00644Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00644Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00644Filter)reportFilter);
            var result = true;
            try
            {
                mrs00644RDOSereServs = new List<Mrs00644RDO>();
                if (filter.HAS_SERE_SERV == true || filter.HAS_SERE_SERV == null)
                {
                    var InvoiceIn = new ManagerSql().GetSereServInvoice(filter);
                    if (InvoiceIn != null)
                    {
                        mrs00644RDOSereServs.AddRange(InvoiceIn);
                    }
                }
                if (filter.HAS_SERE_SERV == false || filter.HAS_SERE_SERV == null)
                {
                    var InvoiceOut = new ManagerSql().GetInvoiceOut(filter);
                    if (InvoiceOut != null)
                    {
                        mrs00644RDOSereServs.AddRange(InvoiceOut);
                    }
                }
                listPrintLog = new ManagerSql().GetPrintLog(filter);
                listInvoiceCancel = new ManagerSql().GetInvoiceCancel(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                ProcessTreatmentPrice();
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessTreatmentPrice()
        {
            try
            {
                foreach (var item in mrs00644RDOSereServs)
                {
                    item.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.IN_TIME);
                    item.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.OUT_TIME);
                    item.CREATE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.CREATE_TIME);

                    if (item.END_DEPARTMENT_ID != null)
                    {
                        item.END_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.END_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                    }

                    item.DOB_YEAR = (this.CalcuatorAge(item.DOB) ?? 0).ToString();
                }
                this.Mrs00644RDODepartments.AddRange(this.mrs00644RDOSereServs);
                this.Mrs00644RDODepartments = this.Mrs00644RDODepartments.GroupBy(o => o.END_DEPARTMENT_ID).Select(o => o.First()).ToList();
                foreach (var item in listInvoiceCancel)
                {
                    var printCount = listPrintLog.Where(p => p.UNIQUE_CODE == ProcessUniqueCodeData(item)).ToList();
                    item.PRINT_COUNT = printCount != null && printCount.Count > 0 ? printCount.Max(o => o.NUM_ORDER) : 1;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private string ProcessUniqueCodeData(HIS_INVOICE_CANCEL r)
        {
            string result = "";
            try
            {
                //sửa UniqueCode thêm id của phiếu đang dùng ko phải id của phiếu bị hủy
                if (r.REUSE_ID.HasValue)
                    result = String.Format("{0}_{1}_{2}_{3}_{4}", "Mps000115", r.TEMPLATE_CODE, r.SYMBOL_CODE, r.VIR_NUM_ORDER, r.REUSE_ID);
            }
            catch (Exception ex)
            {
                result = "";
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private int? CalcuatorAge(long DOB)
        {
            int? AGE = null;
            try
            {
                int? tuoi = RDOCommon.CalculateAge(DOB);
                if (tuoi >= 0)
                {
                    AGE = (tuoi >= 1) ? tuoi : 1;
                }
                return AGE;
            }
            catch (Exception ex)
            {
                return null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00644Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00644Filter)reportFilter).TIME_FROM ?? 0));
            }
            if (((Mrs00644Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00644Filter)reportFilter).TIME_TO ?? 0));
            }
            dicSingleTag.Add("TEST_FUEX_PRICE_SUM", this.testFuexPriceSum);
            dicSingleTag.Add("DIIM_PRICE_SUM", this.diimPriceSum);
            dicSingleTag.Add("MEDICINE_PRICE_SUM", this.medicinePriceSum);
            dicSingleTag.Add("BLOOD_PRICE_SUM", this.bloodPriceSum);
            dicSingleTag.Add("SURGMISU_PRICE_SUM", this.surgMisuPriceSum);
            dicSingleTag.Add("MATERIAL_PRICE_SUM", this.materialPriceSum);
            dicSingleTag.Add("EXAM_PRICE_SUM", this.examPriceSum);
            dicSingleTag.Add("TRAN_PRICE_SUM", this.tranPriceSum);
            dicSingleTag.Add("BED_PRICE_SUM", this.bedPriceSum);
            dicSingleTag.Add("ENDO_PRICE_SUM", this.endoPriceSum);
            dicSingleTag.Add("SUIM_PRICE_SUM", this.suimPriceSum);
            dicSingleTag.Add("OTHER_PRICE_SUM", this.otherPriceSum);
            dicSingleTag.Add("TOTAL_PRICE_SUM", this.totalPriceSum);
            dicSingleTag.Add("TOTAL_HEIN_PRICE_SUM", this.totalHeinPriceSum);
            dicSingleTag.Add("TOTAL_PRICE_100_SUM", this.totalPrice100Sum);
            dicSingleTag.Add("TOTAL_PRICE_5_SUM", this.totalPrice5Sum);
            dicSingleTag.Add("TOTAL_PRICE_20_SUM", this.totalPrice20Sum);
            dicSingleTag.Add("TOTAL_PRICE_0_SUM", this.totalPrice0Sum);
            dicSingleTag.Add("PATIENT_PRICE_PAY_SUM", this.patientPricePaySum);

            bool exportSuccess = true;
            //if (!IsNotNullOrEmpty(this.filter.DEPARTMENT_IDs))
            //{
            //    foreach (var item in Mrs00644RDODepartments)
            //    {
            //        item.REQUEST_DEPARTMENT_NAME = ""; 
            //    }
            //}

            objectTag.AddObjectData(store, "Department", Mrs00644RDODepartments);
            objectTag.AddObjectData(store, "SereServ", this.mrs00644RDOSereServs.OrderBy(o => o.VIR_NUM_ORDER).ToList());
            objectTag.AddRelationship(store, "Department", "SereServ", "END_DEPARTMENT_ID", "END_DEPARTMENT_ID");
            objectTag.AddObjectData(store, "NumOrder", this.listInvoiceCancel.GroupBy(o => new { o.INVOICE_BOOK_ID, o.VIR_NUM_ORDER }).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "Invoice", this.listInvoiceCancel.OrderBy(o => o.VIR_NUM_ORDER).ToList());
            objectTag.AddRelationship(store, "NumOrder", "Invoice", new string[] { "VIR_NUM_ORDER", "INVOICE_BOOK_ID" }, new string[] { "VIR_NUM_ORDER", "INVOICE_BOOK_ID" });

            exportSuccess = exportSuccess && store.SetCommonFunctions();
        }

    }
}
