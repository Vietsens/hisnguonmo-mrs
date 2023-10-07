using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisServiceRetyCat;

namespace MRS.Processor.Mrs00338
{
    class Mrs00338Processor : AbstractProcessor
    {
        Mrs00338Filter castFilter = null;

        public Mrs00338Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        //List<HIS_TREATMENT> listTreatmentND = null; 
        List<HIS_TREATMENT> listTreatment = null;
        List<HIS_TRANSACTION> listTransaction = null;
        List<HIS_SERE_SERV> listSereServ = new List<HIS_SERE_SERV>();
        List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        List<string> serviceKSKIds = new List<string>();
        Mrs00338RDO RDO = new Mrs00338RDO();
        Dictionary<long, Mrs00338RDO> dicEndRoom = new Dictionary<long, Mrs00338RDO>();

        long Date_Time_To = 0;
        long TotalDate = 0;

        public override Type FilterType()
        {
            return typeof(Mrs00338Filter);
        }

        protected override bool GetData()
        {
            bool resutl = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00338Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00338: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                this.Date_Time_To = Convert.ToInt64(castFilter.TIME_TO.ToString().Substring(0, 8) + "000000");

                this.ProcessCalcuTotalData();

                HisServiceRetyCatViewFilterQuery srcFilter = new HisServiceRetyCatViewFilterQuery();
                srcFilter.REPORT_TYPE_CODE__EXACT = "MRS00338";
                listServiceRetyCat = new HisServiceRetyCatManager(paramGet).GetView(srcFilter);

                HisTreatmentFilterQuery feeLockFilter = new HisTreatmentFilterQuery();
                feeLockFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                feeLockFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                listTreatment = new ManagerSql().Get(feeLockFilter);
                if (listTreatment != null)
                {
                    listTreatment = listTreatment.Where(o => o.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList();
                }

                if (castFilter.BRANCH_ID != null)
                {
                    listTreatment = listTreatment.Where(o => o.BRANCH_ID == castFilter.BRANCH_ID).ToList();
                }

                HisTransactionFilterQuery tranFilter = new HisTransactionFilterQuery();
                tranFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                tranFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;

                listTransaction = new ManagerSql().Get(tranFilter);
                if (castFilter.BRANCH_ID != null)
                {
                    HisCashierRoomViewFilterQuery filter = new HisCashierRoomViewFilterQuery();
                    filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    var HisCashierRooms = new HisCashierRoomManager().GetView(filter);
                    if (HisCashierRooms != null)
                    {
                        listTransaction = listTransaction.Where(o => HisCashierRooms.Exists(p => p.ID == o.CASHIER_ROOM_ID && p.BRANCH_ID == castFilter.BRANCH_ID)).ToList();
                    }
                }

                List<long> listTreatmentId = new List<long>();

                if (IsNotNullOrEmpty(listTreatment))
                {
                    listTreatmentId = listTreatment.Select(s => s.ID).ToList();
                }

                if (IsNotNullOrEmpty(listTransaction))
                {
                    listTreatmentId.AddRange(listTransaction.Select(s => s.TREATMENT_ID ?? 0).ToList());
                }

                listTreatmentId = listTreatmentId.Distinct().ToList();
                if (listTreatmentId.Count > 0)
                {
                    HisSereServFilterQuery HisSereServFilter = new HisSereServFilterQuery();
                    HisSereServFilter.TREATMENT_IDs = listTreatmentId;
                    listSereServ.AddRange(new ManagerSql().Get(HisSereServFilter));
                    HisPatientTypeAlterFilterQuery HisPatientTypeAlterFilter = new HisPatientTypeAlterFilterQuery();
                    HisPatientTypeAlterFilter.TREATMENT_IDs = listTreatmentId;
                    listPatientTypeAlter.AddRange(new ManagerSql().Get(HisPatientTypeAlterFilter));
                }

                serviceKSKIds = HisServiceCFG.getList_SERVICE_CODE__KSK ?? new List<string>();

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00338:");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                resutl = false;
            }
            return resutl;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                //Chỉ thay đổi lại cấu trúc để tái sử dụng code, tránh lặp code thôi
                if (IsNotNullOrEmpty(listTreatment))
                {
                    foreach (var treatment in listTreatment)
                    {
                        List<HIS_SERE_SERV> sereServs = listSereServ.Where(o => o.TDL_TREATMENT_ID == treatment.ID).ToList();

                        this.ProcessDataDetailBhyt(treatment, sereServs, RDO);

                        ProcessDataPriceBhyt(treatment, sereServs, RDO);

                        //Các cột: Số lượt, Tổng chi phí, Bình quân phiếu, Bình quân thuốc,Tỷ lệ thuốc lấy dữ liệu tương ứng với từng phòng khám, từng buồng bệnh 1. Dành cho bệnh nhân BHYT
                        this.GroupByEndRoomWithBHYT(treatment, sereServs);
                    }
                }
                if (IsNotNullOrEmpty(listTransaction))
                {
                    foreach (var transaction in listTransaction)
                    {
                        var PatientTypeAlter = listPatientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).LastOrDefault(q => q.TREATMENT_ID == transaction.TREATMENT_ID);
                        if (PatientTypeAlter != null && PatientTypeAlter.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            continue;
                        }

                        List<HIS_SERE_SERV> hisSereServs = listSereServ.Where(o => o.TDL_TREATMENT_ID == transaction.TREATMENT_ID).ToList();

                        ProcessDataPriceND(transaction, hisSereServs, RDO);
                    }
                }

                CaculatorVirtualFieldInfo(RDO);

                foreach (var item in dicEndRoom.Keys)
                {
                    var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item) ?? new V_HIS_ROOM();
                    dicEndRoom[item].END_ROOM_NAME = room.ROOM_NAME;
                    dicEndRoom[item].ROOM_TYPE_ID = room.ROOM_TYPE_ID;
                    CaculatorVirtualFieldInfo(dicEndRoom[item]);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void CaculatorVirtualFieldInfo(Mrs00338RDO rdo)
        {
            if (rdo.TOTAL_TREAT_OUT_AMOUNT > 0)
            {
                rdo.AVEGARE_TREAT_PRICE = rdo.TOTAL_TREAT_PRICE / rdo.TOTAL_TREAT_OUT_AMOUNT;
                rdo.AVEGARE_TREAT_MEDI_PRICE = rdo.TOTAL_TREAT_MEDI_PRICE / rdo.TOTAL_TREAT_OUT_AMOUNT;
            }

            if (rdo.TOTAL_TREAT_IN_OUT_AMOUNT > 0)
            {
                rdo.AVEGARE_TREAT_IN_PRICE = rdo.TOTAL_TREAT_IN_PRICE / rdo.TOTAL_TREAT_IN_OUT_AMOUNT;
                rdo.AVEGARE_TREAT_IN_MEDI_PRICE = rdo.TOTAL_TREAT_IN_MEDI_PRICE / rdo.TOTAL_TREAT_IN_OUT_AMOUNT;
            }

            if (rdo.TOTAL_TREAT_OUT_OUT_AMOUNT > 0)
            {
                rdo.AVEGARE_TREAT_OUT_PRICE = rdo.TOTAL_TREAT_OUT_PRICE / rdo.TOTAL_TREAT_OUT_OUT_AMOUNT;
                rdo.AVEGARE_TREAT_OUT_MEDI_PRICE = rdo.TOTAL_TREAT_OUT_MEDI_PRICE / rdo.TOTAL_TREAT_OUT_OUT_AMOUNT;
            }

            if (rdo.TOTAL_TREAT_PRICE > 0)
            {
                rdo.TREAT_MEDI_PRICE_RATIO = 100 * (rdo.TOTAL_TREAT_MEDI_PRICE / rdo.TOTAL_TREAT_PRICE);
            }

            if (rdo.TOTAL_TREAT_IN_PRICE > 0)
            {
                rdo.TREAT_IN_MEDI_PRICE_RATIO = 100 * (rdo.TOTAL_TREAT_IN_MEDI_PRICE / rdo.TOTAL_TREAT_IN_PRICE);
            }

            if (rdo.TOTAL_TREAT_OUT_PRICE > 0)
            {
                rdo.TREAT_OUT_MEDI_PRICE_RATIO = 100 * (rdo.TOTAL_TREAT_OUT_MEDI_PRICE / rdo.TOTAL_TREAT_OUT_PRICE);
            }

            if (rdo.TOTAL_EXAM_OUT_AMOUNT > 0)
            {
                rdo.AVEGARE_EXAM_PRICE = rdo.TOTAL_EXAM_PRICE / rdo.TOTAL_EXAM_OUT_AMOUNT;
                rdo.AVEGARE_EXAM_MEDI_PRICE = rdo.TOTAL_EXAM_MEDI_PRICE / rdo.TOTAL_EXAM_OUT_AMOUNT;
            }

            if (rdo.TOTAL_EXAM_PRICE > 0)
            {
                rdo.EXAM_MEDI_PRICE_RATIO = 100 * (rdo.TOTAL_EXAM_MEDI_PRICE / rdo.TOTAL_EXAM_PRICE);
            }

            if (rdo.TOTAL_E11_OUT_AMOUNT > 0)
            {
                rdo.AVEGARE_E11_PRICE = rdo.TOTAL_E11_PRICE / rdo.TOTAL_E11_OUT_AMOUNT;
                rdo.AVEGARE_E11_MEDI_PRICE = rdo.TOTAL_E11_MEDI_PRICE / rdo.TOTAL_E11_OUT_AMOUNT;
            }

            if (rdo.TOTAL_E11_PRICE > 0)
            {
                rdo.E11_MEDI_PRICE_RATIO = 100 * (rdo.TOTAL_E11_MEDI_PRICE / rdo.TOTAL_E11_PRICE);
            }

            if (this.TotalDate > 0)
            {
                rdo.AVEGARE_TOTAL_HEIN_PRICE = rdo.TOTAL_HEIN_PRICE / this.TotalDate;
            }
        }

        private void GroupByEndRoomWithBHYT(HIS_TREATMENT treatment, List<HIS_SERE_SERV> sereServs)
        {
            if (!treatment.END_ROOM_ID.HasValue)
            {
                return;
            }
            if (treatment.TDL_PATIENT_TYPE_ID != MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
            {
                return;
            }
            if (treatment.FEE_LOCK_TIME < this.Date_Time_To)
            {
                return;
            }
            if (!dicEndRoom.ContainsKey(treatment.END_ROOM_ID.Value))
            {
                dicEndRoom[treatment.END_ROOM_ID.Value] = new Mrs00338RDO();
            }
            var ir = dicEndRoom[treatment.END_ROOM_ID.Value];

            //Các cột: Số lượt, Tổng chi phí, Bình quân phiếu, Bình quân thuốc,Tỷ lệ thuốc lấy dữ liệu tương ứng với từng phòng khám, từng buồng bệnh 1. Dành cho bệnh nhân BHYT
            ProcessDataDetailBhyt(treatment, sereServs, dicEndRoom[treatment.END_ROOM_ID.Value]);
        }

        void ProcessDataDetailBhyt(HIS_TREATMENT treatment,List<HIS_SERE_SERV> sereServs, Mrs00338RDO rdo)
        {
            if (treatment.TDL_PATIENT_TYPE_ID != MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
            {
                return;
            }
            if (treatment.FEE_LOCK_TIME >= this.Date_Time_To)
            {
                if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                {
                    rdo.TOTAL_TREAT_OUT_AMOUNT++;
                    rdo.TOTAL_TREAT_PRICE += (sereServs.Sum(o => o.VIR_TOTAL_PRICE ?? 0));
                    if (IsNotNullOrEmpty(sereServs))
                    {
                        rdo.TOTAL_TREAT_MEDI_PRICE += sereServs.Where(o => o.MEDICINE_ID.HasValue).ToList().Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    }

                    if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        rdo.TOTAL_TREAT_IN_OUT_AMOUNT++;
                        rdo.TOTAL_TREAT_IN_PRICE += (sereServs.Sum(o => o.VIR_TOTAL_PRICE ?? 0));
                        if (IsNotNullOrEmpty(sereServs))
                        {
                            rdo.TOTAL_TREAT_IN_MEDI_PRICE += sereServs.Where(o => o.MEDICINE_ID.HasValue).ToList().Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                        }
                    }
                    else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                    {
                        rdo.TOTAL_TREAT_OUT_OUT_AMOUNT++;
                        rdo.TOTAL_TREAT_OUT_PRICE += (sereServs.Sum(o => o.VIR_TOTAL_PRICE ?? 0));
                        if (IsNotNullOrEmpty(sereServs))
                        {
                            rdo.TOTAL_TREAT_OUT_MEDI_PRICE += sereServs.Where(o => o.MEDICINE_ID.HasValue).ToList().Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                        }
                    }
                }
                else
                {
                    rdo.TOTAL_EXAM_OUT_AMOUNT++;
                    rdo.TOTAL_EXAM_PRICE += (sereServs.Sum(o => o.VIR_TOTAL_PRICE ?? 0));
                    if (IsNotNullOrEmpty(sereServs))
                    {
                        rdo.TOTAL_EXAM_MEDI_PRICE += sereServs.Where(o => o.MEDICINE_ID.HasValue).ToList().Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    }

                    if (!string.IsNullOrEmpty(treatment.ICD_CODE) && Config.HisIcdCFG.HisIcdCODE_Extends != null && Config.HisIcdCFG.HisIcdCODE_Extends.Contains(treatment.ICD_CODE))
                    {
                        rdo.TOTAL_E11_OUT_AMOUNT++;
                        rdo.TOTAL_E11_PRICE += (sereServs.Sum(o => o.VIR_TOTAL_PRICE ?? 0));
                        if (IsNotNullOrEmpty(sereServs))
                        {
                            rdo.TOTAL_E11_MEDI_PRICE += sereServs.Where(o => o.MEDICINE_ID.HasValue).ToList().Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                        }
                    }
                }
                if (castFilter.ICD_DTD == treatment.ICD_CODE)
                {
                    rdo.TOTAL_E11_OUT_AMOUNT++;
                    rdo.TOTAL_E11_PRICE += (sereServs.Sum(o => o.VIR_TOTAL_PRICE ?? 0));
                }
            }

        }

        private void ProcessDataPriceBhyt(HIS_TREATMENT treatment, List<HIS_SERE_SERV> hisSereServs, Mrs00338RDO rdo)
        {
            if (IsNotNullOrEmpty(hisSereServs))
            {
                bool isKsk = hisSereServs.Exists(o => serviceKSKIds.Contains(o.TDL_SERVICE_CODE));
                bool iskhac = hisSereServs.Exists(o => listServiceRetyCat.Exists(p => p.SERVICE_ID == o.SERVICE_ID && this.castFilter.CATEGORY_CODE__KHAC != null && this.castFilter.CATEGORY_CODE__KHAC.Contains(p.CATEGORY_CODE)));

                List<HIS_SERE_SERV> ssxhh = hisSereServs.Where(o => listServiceRetyCat.Exists(p => p.SERVICE_ID == o.SERVICE_ID && this.castFilter.CATEGORY_CODE__XHH != null && this.castFilter.CATEGORY_CODE__XHH.Contains(p.CATEGORY_CODE))).ToList();
                List<HIS_SERE_SERV> sskhac = hisSereServs.Where(o => !listServiceRetyCat.Exists(p => p.SERVICE_ID == o.SERVICE_ID && this.castFilter.CATEGORY_CODE__XHH != null && this.castFilter.CATEGORY_CODE__XHH.Contains(p.CATEGORY_CODE))).ToList();

                if (treatment.FEE_LOCK_TIME.HasValue && treatment.FEE_LOCK_TIME.Value >= this.Date_Time_To)
                {
                    //Tổng thu BHYT+BỆNH NHÂN trong ngàyVIR_TOTAL_PATIENT_PRICE_BHYT ?? 0
                    rdo.TOTAL_PRICE_END_DATE += hisSereServs.Sum(o => o.VIR_TOTAL_PRICE ?? 0);

                    if (IsNotNullOrEmpty(sskhac))
                    {
                        //Tổng thu bhyt trong ngày
                        rdo.TOTAL_HEIN_PRICE_END_DATE += sskhac.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                        //Tổng thu Bệnh nhân trong ngày
                        rdo.TOTAL_PATIENT_PRICE_END_DATE += sskhac.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                        if (isKsk)
                        {
                            //khám sức khỏe
                            rdo.TOTAL_PATIENT_PRICE_KSK_END_DATE += sskhac.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                        }
                        else if (iskhac)
                        {
                            //khác
                            rdo.TOTAL_PATIENT_PRICE_KHAC_END_DATE += sskhac.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                        }
                        else
                        {
                            //Tổng thu bệnh nhân BHYT dịch vụ bảo hiểm trong ngày
                            rdo.TOTAL_PATIENT_PRICE_BHYT_END_DATE += sskhac.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);

                            //tách tiền BHYT theo diện điều trị
                            if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                rdo.TOTAL_PATIENT_PRICE_BHYT_END_DATE_TREAT_IN += sskhac.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                            }
                            else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
                            {
                                rdo.TOTAL_PATIENT_PRICE_BHYT_END_DATE_TREAT_OUT += sskhac.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                            }
                            else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                            {
                                rdo.TOTAL_PATIENT_PRICE_BHYT_END_DATE_EXAM += sskhac.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                            }

                            //Viện phí 
                            rdo.TOTAL_PATIENT_PRICE_FEE_END_DATE += sskhac.Where(o => o.PATIENT_TYPE_ID != MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0));
                            //Viện phí của bhyt
                            rdo.TOTAL_PATIENT_PRICE_FEE_OF_BHYT_END_DATE += sskhac.Where(o => o.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0));
                        }
                    }

                    //tiền các dịch vụ xã hội hóa
                    if (IsNotNullOrEmpty(ssxhh))
                    {
                        rdo.TOTAL_PRICE_XHH_END_DATE += ssxhh.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                        rdo.TOTAL_HEIN_PRICE_XHH_END_DATE += ssxhh.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                        rdo.TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE += ssxhh.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);

                        //tách tiền BHYT theo diện điều trị
                        if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                        {
                            rdo.TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_TREAT_IN += ssxhh.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                        }
                        else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
                        {
                            rdo.TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_TREAT_OUT += ssxhh.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                        }
                        else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                        {
                            rdo.TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_EXAM += ssxhh.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                        }

                        rdo.TOTAL_PATIENT_PRICE_FEE_XHH_END_DATE += ssxhh.Where(o => o.PATIENT_TYPE_ID != MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0));
                        rdo.TOTAL_PATIENT_PRICE_FEE_OF_BHYT_XHH_END_DATE += ssxhh.Where(o => o.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0));
                    }
                }

                //Tổng thu BHYT+BỆNH NHÂN trong ngày
                rdo.TOTAL_PRICE += hisSereServs.Sum(o => o.VIR_TOTAL_PRICE ?? 0);
                if (IsNotNullOrEmpty(sskhac))
                {
                    //Tổng thu bhyt trong ngày
                    rdo.TOTAL_HEIN_PRICE += sskhac.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    //Tổng thu Bệnh nhân trong ngày
                    rdo.TOTAL_PATIENT_PRICE += sskhac.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                    if (isKsk)
                    {
                        //khám sức khỏe
                        rdo.TOTAL_PATIENT_PRICE_KSK += sskhac.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                    }
                    else if (iskhac)
                    {
                        //khác
                        rdo.TOTAL_PATIENT_PRICE_KHAC += sskhac.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                    }
                    else
                    {
                        //Tổng thu bệnh nhân BHYT dịch vụ bảo hiểm trong ngày
                        rdo.TOTAL_PATIENT_PRICE_BHYT += sskhac.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);

                        //tách tiền BHYT theo diện điều trị
                        if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                        {
                            rdo.TOTAL_PATIENT_PRICE_BHYT_TREAT_IN += sskhac.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                        }
                        else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
                        {
                            rdo.TOTAL_PATIENT_PRICE_BHYT_TREAT_OUT += sskhac.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                        }
                        else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
                        {
                            rdo.TOTAL_PATIENT_PRICE_BHYT_EXAM += sskhac.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                        }

                        //Viện phí 
                        rdo.TOTAL_PATIENT_PRICE_FEE += sskhac.Where(o => o.PATIENT_TYPE_ID != MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0));
                        //Viện phí  của bhyt
                        rdo.TOTAL_PATIENT_PRICE_FEE_OF_BHYT += sskhac.Where(o => o.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0));
                    }
                }

                //tiền các dịch vụ xã hội hóa
                if (IsNotNullOrEmpty(ssxhh))
                {
                    rdo.TOTAL_PRICE_XHH += ssxhh.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    rdo.TOTAL_HEIN_PRICE_XHH += ssxhh.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    rdo.TOTAL_PATIENT_PRICE_BHYT_XHH += ssxhh.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);

                    //tách tiền BHYT theo diện điều trị
                    if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        rdo.TOTAL_PATIENT_PRICE_BHYT_XHH_TREAT_IN += ssxhh.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                    }
                    else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
                    {
                        rdo.TOTAL_PATIENT_PRICE_BHYT_XHH_TREAT_OUT += ssxhh.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                    }
                    else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                    {
                        rdo.TOTAL_PATIENT_PRICE_BHYT_XHH_EXAM += ssxhh.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                    }

                    rdo.TOTAL_PATIENT_PRICE_FEE_XHH += ssxhh.Where(o => o.PATIENT_TYPE_ID != MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0));
                    rdo.TOTAL_PATIENT_PRICE_FEE_OF_BHYT_XHH += ssxhh.Where(o => o.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0));
                }
            }
        }

        private void ProcessDataPriceND(HIS_TRANSACTION transaction, List<HIS_SERE_SERV> hisSereServs, Mrs00338RDO rdo)
        {
            if (IsNotNullOrEmpty(hisSereServs))
            {
                bool isKsk = hisSereServs.Exists(o => serviceKSKIds.Contains(o.TDL_SERVICE_CODE));
                bool iskhac = hisSereServs.Exists(o => listServiceRetyCat.Exists(p => p.SERVICE_ID == o.SERVICE_ID && this.castFilter.CATEGORY_CODE__KHAC != null && this.castFilter.CATEGORY_CODE__KHAC.Contains(p.CATEGORY_CODE)));

                List<HIS_SERE_SERV> ssxhh = hisSereServs.Where(o => listServiceRetyCat.Exists(p => p.SERVICE_ID == o.SERVICE_ID && this.castFilter.CATEGORY_CODE__XHH != null && this.castFilter.CATEGORY_CODE__XHH.Contains(p.CATEGORY_CODE))).ToList();
                List<HIS_SERE_SERV> sskhac = hisSereServs.Where(o => !listServiceRetyCat.Exists(p => p.SERVICE_ID == o.SERVICE_ID && this.castFilter.CATEGORY_CODE__XHH != null && this.castFilter.CATEGORY_CODE__XHH.Contains(p.CATEGORY_CODE))).ToList();
                var treatment = listTreatment.FirstOrDefault(o => o.ID == transaction.TREATMENT_ID);
                if (treatment == null) treatment = new HIS_TREATMENT();

                if (transaction.TRANSACTION_TIME >= this.Date_Time_To)
                {
                    //Tổng thu BHYT+BỆNH NHÂN trong ngày
                    rdo.TOTAL_PRICE_END_DATE += hisSereServs.Sum(o => o.VIR_TOTAL_PRICE ?? 0);

                    if (IsNotNullOrEmpty(sskhac))
                    {
                        //Tổng thu bhyt trong ngày
                        rdo.TOTAL_HEIN_PRICE_END_DATE += sskhac.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                        //Tổng thu Bệnh nhân trong ngày
                        rdo.TOTAL_PATIENT_PRICE_END_DATE += sskhac.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                        if (isKsk)
                        {
                            //khám sức khỏe
                            rdo.TOTAL_PATIENT_PRICE_KSK_END_DATE += sskhac.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                        }
                        else if (iskhac)
                        {
                            //khác
                            rdo.TOTAL_PATIENT_PRICE_KHAC_END_DATE += sskhac.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                        }
                        else
                        {
                            //Tổng thu bệnh nhân BHYT dịch vụ bảo hiểm trong ngày
                            rdo.TOTAL_PATIENT_PRICE_BHYT_END_DATE += sskhac.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);

                            //tách tiền BHYT theo diện điều trị
                            if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                rdo.TOTAL_PATIENT_PRICE_BHYT_END_DATE_TREAT_IN += sskhac.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                            }
                            else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
                            {
                                rdo.TOTAL_PATIENT_PRICE_BHYT_END_DATE_TREAT_OUT += sskhac.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                            }
                            else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                            {
                                rdo.TOTAL_PATIENT_PRICE_BHYT_END_DATE_EXAM += sskhac.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                            }

                            //Viện phí 
                            rdo.TOTAL_PATIENT_PRICE_FEE_END_DATE += sskhac.Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0));
                        }
                    }

                    //tiền các dịch vụ xã hội hóa
                    if (IsNotNullOrEmpty(ssxhh))
                    {
                        rdo.TOTAL_PRICE_XHH_END_DATE += ssxhh.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                        rdo.TOTAL_HEIN_PRICE_XHH_END_DATE += ssxhh.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                        rdo.TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE += ssxhh.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);

                        //tách tiền BHYT theo diện điều trị
                        if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                        {
                            rdo.TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_TREAT_IN += ssxhh.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                        }
                        else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
                        {
                            rdo.TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_TREAT_OUT += ssxhh.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                        }
                        else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                        {
                            rdo.TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_EXAM += ssxhh.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                        }

                        rdo.TOTAL_PATIENT_PRICE_FEE_XHH_END_DATE += ssxhh.Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0));
                    }
                }

                //Tổng thu BHYT+BỆNH NHÂN trong ngày
                rdo.TOTAL_PRICE += hisSereServs.Sum(o => o.VIR_TOTAL_PRICE ?? 0);

                if (IsNotNullOrEmpty(sskhac))
                {
                    //Tổng thu bhyt trong ngày
                    rdo.TOTAL_HEIN_PRICE += sskhac.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    //Tổng thu Bệnh nhân trong ngày
                    rdo.TOTAL_PATIENT_PRICE += sskhac.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                    if (isKsk)
                    {
                        //khám sức khỏe
                        rdo.TOTAL_PATIENT_PRICE_KSK += sskhac.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                    }
                    else if (iskhac)
                    {
                        //khác
                        rdo.TOTAL_PATIENT_PRICE_KHAC += sskhac.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                    }
                    else
                    {
                        //Tổng thu bệnh nhân BHYT dịch vụ bảo hiểm trong ngày
                        rdo.TOTAL_PATIENT_PRICE_BHYT += sskhac.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);

                        //tách tiền BHYT theo diện điều trị
                        if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                        {
                            rdo.TOTAL_PATIENT_PRICE_BHYT_TREAT_IN += sskhac.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                        }
                        else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
                        {
                            rdo.TOTAL_PATIENT_PRICE_BHYT_TREAT_OUT += sskhac.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                        }
                        else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
                        {
                            rdo.TOTAL_PATIENT_PRICE_BHYT_EXAM += sskhac.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                        }

                        //Viện phí 
                        rdo.TOTAL_PATIENT_PRICE_FEE += sskhac.Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0));
                    }
                }

                //tiền các dịch vụ xã hội hóa
                if (IsNotNullOrEmpty(ssxhh))
                {
                    rdo.TOTAL_PRICE_XHH += ssxhh.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    rdo.TOTAL_HEIN_PRICE_XHH += ssxhh.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    rdo.TOTAL_PATIENT_PRICE_BHYT_XHH += ssxhh.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);

                    //tách tiền BHYT theo diện điều trị
                    if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        rdo.TOTAL_PATIENT_PRICE_BHYT_XHH_TREAT_IN += ssxhh.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                    }
                    else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
                    {
                        rdo.TOTAL_PATIENT_PRICE_BHYT_XHH_TREAT_OUT += ssxhh.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                    }
                    else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                    {
                        rdo.TOTAL_PATIENT_PRICE_BHYT_XHH_EXAM += ssxhh.Sum(s => s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0);
                    }

                    rdo.TOTAL_PATIENT_PRICE_FEE_XHH += ssxhh.Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.PATIENT_TYPE_ID == 1 ? (s.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0));
                }
            }
        }

        void ProcessCalcuTotalData()
        {
            try
            {
                //Inventec.Common.DateTime.Calculation.DifferenceDate(castFilter.TIME_FROM, castFilter.TIME_TO); 
                var dtFrom = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_FROM);
                var dtTo = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_TO);
                if (dtFrom.HasValue && dtTo.HasValue)
                {
                    this.TotalDate = dtTo.Value.DayOfYear - dtFrom.Value.DayOfYear + 1;
                    Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => this.TotalDate), this.TotalDate));
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
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                dicSingleTag.Add("DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                dicSingleTag.Add("DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                objectTag.AddObjectData(store, "Child", this.listSereServ);
                objectTag.AddObjectData(store, "Parent", this.listSereServ.GroupBy(o => o.SERVICE_ID).Select(p => p.First()).ToList());
                objectTag.AddObjectData(store, "GrandParent", this.listSereServ.GroupBy(o => o.TDL_SERVICE_TYPE_ID).Select(p => p.First()).ToList());

                objectTag.AddObjectData(store, "EndRoom", this.dicEndRoom.Values.OrderBy(o=>o.ROOM_TYPE_ID).ToList());

                dicSingleTag.Add(ExtendSingleKey.TOTAL_TREAT_PRICE, RDO.TOTAL_TREAT_PRICE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_TREAT_OUT_AMOUNT, RDO.TOTAL_TREAT_OUT_AMOUNT);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_TREAT_MEDI_PRICE, RDO.TOTAL_TREAT_MEDI_PRICE);
                dicSingleTag.Add(ExtendSingleKey.TREAT_MEDI_PRICE_RATIO, RDO.TREAT_MEDI_PRICE_RATIO);
                dicSingleTag.Add(ExtendSingleKey.AVEGARE_TREAT_PRICE, RDO.AVEGARE_TREAT_PRICE);
                dicSingleTag.Add(ExtendSingleKey.AVEGARE_TREAT_MEDI_PRICE, RDO.AVEGARE_TREAT_MEDI_PRICE);

                dicSingleTag.Add(ExtendSingleKey.TOTAL_EXAM_PRICE, RDO.TOTAL_EXAM_PRICE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_EXAM_OUT_AMOUNT, RDO.TOTAL_EXAM_OUT_AMOUNT);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_EXAM_MEDI_PRICE, RDO.TOTAL_EXAM_MEDI_PRICE);
                dicSingleTag.Add(ExtendSingleKey.EXAM_MEDI_PRICE_RATIO, RDO.EXAM_MEDI_PRICE_RATIO);
                dicSingleTag.Add(ExtendSingleKey.AVEGARE_EXAM_PRICE, RDO.AVEGARE_EXAM_PRICE);
                dicSingleTag.Add(ExtendSingleKey.AVEGARE_EXAM_MEDI_PRICE, RDO.AVEGARE_EXAM_MEDI_PRICE);

                dicSingleTag.Add(ExtendSingleKey.TOTAL_E11_PRICE, RDO.TOTAL_E11_PRICE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_E11_OUT_AMOUNT, RDO.TOTAL_E11_OUT_AMOUNT);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_E11_MEDI_PRICE, RDO.TOTAL_E11_MEDI_PRICE);
                dicSingleTag.Add(ExtendSingleKey.E11_MEDI_PRICE_RATIO, RDO.E11_MEDI_PRICE_RATIO);
                dicSingleTag.Add(ExtendSingleKey.AVEGARE_E11_PRICE, RDO.AVEGARE_E11_PRICE);
                dicSingleTag.Add(ExtendSingleKey.AVEGARE_E11_MEDI_PRICE, RDO.AVEGARE_E11_MEDI_PRICE);

                dicSingleTag.Add(ExtendSingleKey.TOTAL_HEIN_PRICE, RDO.TOTAL_HEIN_PRICE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_HEIN_PRICE_END_DATE, RDO.TOTAL_HEIN_PRICE_END_DATE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE, RDO.TOTAL_PATIENT_PRICE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_BHYT, RDO.TOTAL_PATIENT_PRICE_BHYT);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_BHYT_END_DATE, RDO.TOTAL_PATIENT_PRICE_BHYT_END_DATE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_END_DATE, RDO.TOTAL_PATIENT_PRICE_END_DATE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PRICE, RDO.TOTAL_PRICE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PRICE_END_DATE, RDO.TOTAL_PRICE_END_DATE);
                dicSingleTag.Add(ExtendSingleKey.AVEGARE_TOTAL_HEIN_PRICE, RDO.AVEGARE_TOTAL_HEIN_PRICE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_KSK, RDO.TOTAL_PATIENT_PRICE_KSK);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_KSK_END_DATE, RDO.TOTAL_PATIENT_PRICE_KSK_END_DATE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_KHAC, RDO.TOTAL_PATIENT_PRICE_KHAC);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_FEE, RDO.TOTAL_PATIENT_PRICE_FEE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_KHAC_END_DATE, RDO.TOTAL_PATIENT_PRICE_KHAC_END_DATE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_FEE_END_DATE, RDO.TOTAL_PATIENT_PRICE_FEE_END_DATE);

                dicSingleTag.Add(ExtendSingleKey.TOTAL_PRICE_XHH, RDO.TOTAL_PRICE_XHH);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_HEIN_PRICE_XHH, RDO.TOTAL_HEIN_PRICE_XHH);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_BHYT_XHH, RDO.TOTAL_PATIENT_PRICE_BHYT_XHH);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_FEE_XHH, RDO.TOTAL_PATIENT_PRICE_FEE_XHH);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PRICE_XHH_END_DATE, RDO.TOTAL_PRICE_XHH_END_DATE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_HEIN_PRICE_XHH_END_DATE, RDO.TOTAL_HEIN_PRICE_XHH_END_DATE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE, RDO.TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_FEE_XHH_END_DATE, RDO.TOTAL_PATIENT_PRICE_FEE_XHH_END_DATE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_FEE_OF_BHYT_END_DATE, RDO.TOTAL_PATIENT_PRICE_FEE_OF_BHYT_END_DATE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_FEE_OF_BHYT, RDO.TOTAL_PATIENT_PRICE_FEE_OF_BHYT);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_FEE_OF_BHYT_XHH_END_DATE, RDO.TOTAL_PATIENT_PRICE_FEE_OF_BHYT_XHH_END_DATE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_FEE_OF_BHYT_XHH, RDO.TOTAL_PATIENT_PRICE_FEE_OF_BHYT_XHH);

                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_BHYT_END_DATE_EXAM, RDO.TOTAL_PATIENT_PRICE_BHYT_END_DATE_EXAM);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_BHYT_END_DATE_TREAT_IN, RDO.TOTAL_PATIENT_PRICE_BHYT_END_DATE_TREAT_IN);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_BHYT_END_DATE_TREAT_OUT, RDO.TOTAL_PATIENT_PRICE_BHYT_END_DATE_TREAT_OUT);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_BHYT_EXAM, RDO.TOTAL_PATIENT_PRICE_BHYT_EXAM);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_BHYT_TREAT_IN, RDO.TOTAL_PATIENT_PRICE_BHYT_TREAT_IN);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_BHYT_TREAT_OUT, RDO.TOTAL_PATIENT_PRICE_BHYT_TREAT_OUT);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_EXAM, RDO.TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_EXAM);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_TREAT_IN, RDO.TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_TREAT_IN);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_TREAT_OUT, RDO.TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_TREAT_OUT);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_BHYT_XHH_EXAM, RDO.TOTAL_PATIENT_PRICE_BHYT_XHH_EXAM);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_BHYT_XHH_TREAT_IN, RDO.TOTAL_PATIENT_PRICE_BHYT_XHH_TREAT_IN);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_PATIENT_PRICE_BHYT_XHH_TREAT_OUT, RDO.TOTAL_PATIENT_PRICE_BHYT_XHH_TREAT_OUT);

                dicSingleTag.Add(ExtendSingleKey.TOTAL_TREAT_IN_PRICE, RDO.TOTAL_TREAT_IN_PRICE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_TREAT_IN_OUT_AMOUNT, RDO.TOTAL_TREAT_IN_OUT_AMOUNT);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_TREAT_IN_MEDI_PRICE, RDO.TOTAL_TREAT_IN_MEDI_PRICE);
                dicSingleTag.Add(ExtendSingleKey.TREAT_IN_MEDI_PRICE_RATIO, RDO.TREAT_IN_MEDI_PRICE_RATIO);
                dicSingleTag.Add(ExtendSingleKey.AVEGARE_TREAT_IN_PRICE, RDO.AVEGARE_TREAT_IN_PRICE);
                dicSingleTag.Add(ExtendSingleKey.AVEGARE_TREAT_IN_MEDI_PRICE, RDO.AVEGARE_TREAT_IN_MEDI_PRICE);

                dicSingleTag.Add(ExtendSingleKey.TOTAL_TREAT_OUT_PRICE, RDO.TOTAL_TREAT_OUT_PRICE);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_TREAT_OUT_OUT_AMOUNT, RDO.TOTAL_TREAT_OUT_OUT_AMOUNT);
                dicSingleTag.Add(ExtendSingleKey.TOTAL_TREAT_OUT_MEDI_PRICE, RDO.TOTAL_TREAT_OUT_MEDI_PRICE);
                dicSingleTag.Add(ExtendSingleKey.TREAT_OUT_MEDI_PRICE_RATIO, RDO.TREAT_OUT_MEDI_PRICE_RATIO);
                dicSingleTag.Add(ExtendSingleKey.AVEGARE_TREAT_OUT_PRICE, RDO.AVEGARE_TREAT_OUT_PRICE);
                dicSingleTag.Add(ExtendSingleKey.AVEGARE_TREAT_OUT_MEDI_PRICE, RDO.AVEGARE_TREAT_OUT_MEDI_PRICE);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

    class ExtendSingleKey
    {
        internal const string DATE_TO_STR = "DATE_TO_STR";
        internal const string DATE_FROM_STR = "DATE_FROM_STR";
        internal const string TOTAL_TREAT_OUT_AMOUNT = "TOTAL_TREAT_OUT_AMOUNT";
        internal const string TOTAL_TREAT_PRICE = "TOTAL_TREAT_PRICE";
        internal const string AVEGARE_TREAT_PRICE = "AVEGARE_TREAT_PRICE";
        internal const string AVEGARE_TREAT_MEDI_PRICE = "AVEGARE_TREAT_MEDI_PRICE";
        internal const string TREAT_MEDI_PRICE_RATIO = "TREAT_MEDI_PRICE_RATIO";
        internal const string TOTAL_TREAT_MEDI_PRICE = "TOTAL_TREAT_MEDI_PRICE";
        internal const string TOTAL_EXAM_OUT_AMOUNT = "TOTAL_EXAM_OUT_AMOUNT";
        internal const string TOTAL_EXAM_PRICE = "TOTAL_EXAM_PRICE";
        internal const string AVEGARE_EXAM_PRICE = "AVEGARE_EXAM_PRICE";
        internal const string AVEGARE_EXAM_MEDI_PRICE = "AVEGARE_EXAM_MEDI_PRICE";
        internal const string EXAM_MEDI_PRICE_RATIO = "EXAM_MEDI_PRICE_RATIO";
        internal const string TOTAL_EXAM_MEDI_PRICE = "TOTAL_EXAM_MEDI_PRICE";
        internal const string TOTAL_E11_OUT_AMOUNT = "TOTAL_E11_OUT_AMOUNT";
        internal const string TOTAL_E11_PRICE = "TOTAL_E11_PRICE";
        internal const string AVEGARE_E11_PRICE = "AVEGARE_E11_PRICE";
        internal const string AVEGARE_E11_MEDI_PRICE = "AVEGARE_E11_MEDI_PRICE";
        internal const string E11_MEDI_PRICE_RATIO = "E11_MEDI_PRICE_RATIO";
        internal const string TOTAL_E11_MEDI_PRICE = "TOTAL_E11_MEDI_PRICE";
        internal const string TOTAL_HEIN_PRICE_END_DATE = "TOTAL_HEIN_PRICE_END_DATE";
        internal const string TOTAL_HEIN_PRICE = "TOTAL_HEIN_PRICE";
        internal const string AVEGARE_TOTAL_HEIN_PRICE = "AVEGARE_TOTAL_HEIN_PRICE";
        internal const string TOTAL_PATIENT_PRICE_END_DATE = "TOTAL_PATIENT_PRICE_END_DATE";
        internal const string TOTAL_PATIENT_PRICE_BHYT_END_DATE = "TOTAL_PATIENT_PRICE_BHYT_END_DATE";
        internal const string TOTAL_PATIENT_PRICE_ND_END_DATE = "TOTAL_PATIENT_PRICE_ND_END_DATE";
        internal const string TOTAL_PATIENT_PRICE = "TOTAL_PATIENT_PRICE";
        internal const string TOTAL_PATIENT_PRICE_BHYT = "TOTAL_PATIENT_PRICE_BHYT";
        internal const string TOTAL_PATIENT_PRICE_ND = "TOTAL_PATIENT_PRICE_ND";
        internal const string TOTAL_PRICE_END_DATE = "TOTAL_PRICE_END_DATE";
        internal const string TOTAL_PRICE = "TOTAL_PRICE";
        internal const string TOTAL_PATIENT_PRICE_KSK_END_DATE = "TOTAL_PATIENT_PRICE_KSK_END_DATE";
        internal const string TOTAL_PATIENT_PRICE_KSK = "TOTAL_PATIENT_PRICE_KSK";
        internal const string TOTAL_PATIENT_PRICE_KHAC = "TOTAL_PATIENT_PRICE_KHAC";
        internal const string TOTAL_PATIENT_PRICE_FEE = "TOTAL_PATIENT_PRICE_FEE";
        internal const string TOTAL_PATIENT_PRICE_KHAC_END_DATE = "TOTAL_PATIENT_PRICE_KHAC_END_DATE";
        internal const string TOTAL_PATIENT_PRICE_FEE_END_DATE = "TOTAL_PATIENT_PRICE_FEE_END_DATE";
        internal const string TOTAL_PRICE_XHH = "TOTAL_PRICE_XHH";
        internal const string TOTAL_HEIN_PRICE_XHH = "TOTAL_HEIN_PRICE_XHH";
        internal const string TOTAL_PATIENT_PRICE_BHYT_XHH = "TOTAL_PATIENT_PRICE_BHYT_XHH";
        internal const string TOTAL_PATIENT_PRICE_FEE_XHH = "TOTAL_PATIENT_PRICE_FEE_XHH";
        internal const string TOTAL_PRICE_XHH_END_DATE = "TOTAL_PRICE_XHH_END_DATE";
        internal const string TOTAL_HEIN_PRICE_XHH_END_DATE = "TOTAL_HEIN_PRICE_XHH_END_DATE";
        internal const string TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE = "TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE";
        internal const string TOTAL_PATIENT_PRICE_FEE_XHH_END_DATE = "TOTAL_PATIENT_PRICE_FEE_XHH_END_DATE";
        internal const string TOTAL_PATIENT_PRICE_FEE_OF_BHYT_END_DATE = "TOTAL_PATIENT_PRICE_FEE_OF_BHYT_END_DATE";
        internal const string TOTAL_PATIENT_PRICE_FEE_OF_BHYT = "TOTAL_PATIENT_PRICE_FEE_OF_BHYT";
        internal const string TOTAL_PATIENT_PRICE_FEE_OF_BHYT_XHH_END_DATE = "TOTAL_PATIENT_PRICE_FEE_OF_BHYT_XHH_END_DATE";
        internal const string TOTAL_PATIENT_PRICE_FEE_OF_BHYT_XHH = "TOTAL_PATIENT_PRICE_FEE_OF_BHYT_XHH";

        internal const string TOTAL_PATIENT_PRICE_BHYT_END_DATE_EXAM = "TOTAL_PATIENT_PRICE_BHYT_END_DATE_EXAM";
        internal const string TOTAL_PATIENT_PRICE_BHYT_END_DATE_TREAT_IN = "TOTAL_PATIENT_PRICE_BHYT_END_DATE_TREAT_IN";
        internal const string TOTAL_PATIENT_PRICE_BHYT_END_DATE_TREAT_OUT = "TOTAL_PATIENT_PRICE_BHYT_END_DATE_TREAT_OUT";
        internal const string TOTAL_PATIENT_PRICE_BHYT_EXAM = "TOTAL_PATIENT_PRICE_BHYT_EXAM";
        internal const string TOTAL_PATIENT_PRICE_BHYT_TREAT_IN = "TOTAL_PATIENT_PRICE_BHYT_TREAT_IN";
        internal const string TOTAL_PATIENT_PRICE_BHYT_TREAT_OUT = "TOTAL_PATIENT_PRICE_BHYT_TREAT_OUT";
        internal const string TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_EXAM = "TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_EXAM";
        internal const string TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_TREAT_IN = "TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_TREAT_IN";
        internal const string TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_TREAT_OUT = "TOTAL_PATIENT_PRICE_BHYT_XHH_END_DATE_TREAT_OUT";
        internal const string TOTAL_PATIENT_PRICE_BHYT_XHH_EXAM = "TOTAL_PATIENT_PRICE_BHYT_XHH_EXAM";
        internal const string TOTAL_PATIENT_PRICE_BHYT_XHH_TREAT_IN = "TOTAL_PATIENT_PRICE_BHYT_XHH_TREAT_IN";
        internal const string TOTAL_PATIENT_PRICE_BHYT_XHH_TREAT_OUT = "TOTAL_PATIENT_PRICE_BHYT_XHH_TREAT_OUT";

        internal const string TOTAL_TREAT_IN_PRICE = "TOTAL_TREAT_IN_PRICE";
        internal const string TOTAL_TREAT_IN_OUT_AMOUNT = "TOTAL_TREAT_IN_OUT_AMOUNT";
        internal const string TOTAL_TREAT_IN_MEDI_PRICE = "TOTAL_TREAT_IN_MEDI_PRICE";
        internal const string TREAT_IN_MEDI_PRICE_RATIO = "TREAT_IN_MEDI_PRICE_RATIO";
        internal const string AVEGARE_TREAT_IN_PRICE = "AVEGARE_TREAT_IN_PRICE";
        internal const string AVEGARE_TREAT_IN_MEDI_PRICE = "AVEGARE_TREAT_IN_MEDI_PRICE";

        internal const string TOTAL_TREAT_OUT_PRICE = "TOTAL_TREAT_OUT_PRICE";
        internal const string TOTAL_TREAT_OUT_OUT_AMOUNT = "TOTAL_TREAT_OUT_OUT_AMOUNT";
        internal const string TOTAL_TREAT_OUT_MEDI_PRICE = "TOTAL_TREAT_OUT_MEDI_PRICE";
        internal const string TREAT_OUT_MEDI_PRICE_RATIO = "TREAT_OUT_MEDI_PRICE_RATIO";
        internal const string AVEGARE_TREAT_OUT_PRICE = "AVEGARE_TREAT_OUT_PRICE";
        internal const string AVEGARE_TREAT_OUT_MEDI_PRICE = "AVEGARE_TREAT_OUT_MEDI_PRICE";
    }
    //#18475
    //- Hướng xử lý với BN khám sức khỏe : Lấy toàn bộ DVKT của BN ở PK5 vào mục KSK/ không cần quan tâm thông tin chỉ định ở đâu.
    //- Với các nội dung liên quan đến thu Viện phí trực tiếp trong báo cáo. Yêu cầu lấy theo điều kiện là tất cả DVKT thu phí đã được thanh toán ( không tính khóa viện phí).
    //- Mục thu khác: Lấy toàn bộ DVKT của BN ở phòng xử lý ảo vào mục thu khác/ không cần quan tâm thông tin chỉ định ở đâu.
    //- Thêm 4 dòng số liệu (chữ màu đỏ) trên file EXCEL mẫu gửi kèm . Nội dung của 4 dòng đó là lấy riêng ra các DVKT thu phí của đối tượng BHYT --> thêm FEE_OF_BHYT và chỉ add trong ProcessDataPriceBhyt
}
