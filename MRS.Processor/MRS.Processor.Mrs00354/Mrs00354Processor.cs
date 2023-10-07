using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
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
using MOS.MANAGER.HisServiceRetyCat;

namespace MRS.Processor.Mrs00354
{
    class Mrs00354Processor : AbstractProcessor
    {
        Mrs00354Filter castFilter = null;

        List<Mrs00354RDO> listExamHeinRdo = new List<Mrs00354RDO>();
        List<Mrs00354RDO> listTreatOutHeinRdo = new List<Mrs00354RDO>();
        List<Mrs00354RDO> listTreatInHeinRdo = new List<Mrs00354RDO>();
        List<Mrs00354RDO> listTreatOutRdo = new List<Mrs00354RDO>();
        List<Mrs00354RDO> listTreatInRdo = new List<Mrs00354RDO>();

        Mrs00354RDO rdoKSK = new Mrs00354RDO();
        Mrs00354RDO rdoExam = new Mrs00354RDO();
        Dictionary<long, TongKhoaPhong> dicKhoa = new Dictionary<long, TongKhoaPhong>();
        Dictionary<long, TongKhoaPhong> dicPhong = new Dictionary<long, TongKhoaPhong>();
        Dictionary<long, TongKhoaPhong> dicKhoaYc = new Dictionary<long, TongKhoaPhong>();
        Dictionary<long, TongKhoaPhong> dicPhongYc = new Dictionary<long, TongKhoaPhong>();
        List<string> serviceCodeECGs = new List<string>();


        public Mrs00354Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        Dictionary<long, List<DataGet>> dicTreatSereServ = new Dictionary<long, List<DataGet>>();
        List<DataGet> listTreatment = new List<DataGet>();
        List<DataGet> listSereServ = new List<DataGet>();

        public override Type FilterType()
        {
            return typeof(Mrs00354Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00354Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00354" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                listSereServ = new ManagerSql().GetRdo(this.castFilter);
                //HisTreatmentViewFilterQuery treatFilter = new HisTreatmentViewFilterQuery();
                //treatFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                //treatFilter.OUT_TIME_TO = castFilter.TIME_TO;
                //treatFilter.IS_PAUSE = true;

                listTreatment = listSereServ.GroupBy(o => o.TDL_TREATMENT_ID ?? 0).Select(p => p.First()).ToList();
                if (castFilter.BRANCH_ID != null)
                {
                    listTreatment = listTreatment.Where(o => o.BRANCH_ID == castFilter.BRANCH_ID).ToList();
                }

                if (castFilter.BRANCH_IDs != null)
                {
                    listTreatment = listTreatment.Where(p => castFilter.BRANCH_IDs.Contains(p.BRANCH_ID ?? 0)).ToList();

                }

                if (castFilter.SERVICE_CODE__ECGs != null)
                {
                    serviceCodeECGs = castFilter.SERVICE_CODE__ECGs.Split(new char[] { ',' }).ToList();
                }
                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu bao cao MRS00354");
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
                if (IsNotNullOrEmpty(listTreatment))
                {

                    if (IsNotNullOrEmpty(listSereServ))
                    {
                        foreach (var item in listSereServ)
                        {
                            if (item.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.IS_NO_PAY == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                            {
                                continue;
                            }
                            //thêm khoa chỉ định
                            var reqDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.TDL_REQUEST_DEPARTMENT_ID);
                            if (reqDepartment != null)
                            {
                                item.REQUEST_DEPARTMENT_CODE = reqDepartment.DEPARTMENT_CODE;
                                item.REQUEST_DEPARTMENT_NAME = reqDepartment.DEPARTMENT_NAME;
                            }
                            //them khoa thực hiện
                            var exeDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.TDL_EXECUTE_DEPARTMENT_ID);
                            if (exeDepartment != null)
                            {
                                item.EXECUTE_DEPARTMENT_CODE = exeDepartment.DEPARTMENT_CODE;
                                item.EXECUTE_DEPARTMENT_NAME = exeDepartment.DEPARTMENT_NAME;
                            }
                            //thêm phòng chỉ định
                            var reqRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.TDL_REQUEST_ROOM_ID);
                            if (reqRoom != null)
                            {
                                item.REQUEST_ROOM_CODE = reqRoom.ROOM_CODE;
                                item.REQUEST_ROOM_NAME = reqRoom.ROOM_NAME;
                            }
                            //thêm phòng thực hiện
                            var exeRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.TDL_EXECUTE_ROOM_ID);
                            if (exeRoom != null)
                            {
                                item.EXECUTE_ROOM_CODE = exeRoom.ROOM_CODE;
                                item.EXECUTE_ROOM_NAME = exeRoom.ROOM_NAME;
                            }
                            //thêm khoa kết thúc
                            var endDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.END_DEPARTMENT_ID);
                            if (endDepartment != null)
                            {
                                item.END_DEPARTMENT_CODE = endDepartment.DEPARTMENT_CODE;
                                item.END_DEPARTMENT_NAME = endDepartment.DEPARTMENT_NAME;
                            }
                            //thêm phòng kết thúc
                            var endRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.END_ROOM_ID);
                            if (endRoom != null)
                            {
                                item.END_ROOM_CODE = endRoom.ROOM_CODE;
                                item.END_ROOM_NAME = endRoom.ROOM_NAME;
                            }

                            if (!dicTreatSereServ.ContainsKey(item.TDL_TREATMENT_ID ?? 0))
                                dicTreatSereServ[item.TDL_TREATMENT_ID ?? 0] = new List<DataGet>();
                            dicTreatSereServ[item.TDL_TREATMENT_ID ?? 0].Add(item);
                        }
                    }

                    this.ProcessDataDetail();
                    this.ProcessRdo();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        void ProcessDataDetail()
        {
            if (IsNotNullOrEmpty(listTreatment))
            {
                foreach (var treatment in listTreatment)
                {
                    var hisSereServs = dicTreatSereServ.ContainsKey(treatment.TDL_TREATMENT_ID ?? 0) ? dicTreatSereServ[treatment.TDL_TREATMENT_ID ?? 0] : new List<DataGet>();
                    //xử lý tạo chi phí khoa phòng
                    //if(split by request_department_room_id)
                    EndRoomDepartmentPrice(treatment, hisSereServs);
                    RoomDepartmentPrice(treatment, hisSereServs);

                    if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && treatment.TDL_PATIENT_TYPE_ID != MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        var ksk = hisSereServs.Where(o => MANAGER.Config.HisServiceCFG.getList_SERVICE_CODE__KSK != null && MANAGER.Config.HisServiceCFG.getList_SERVICE_CODE__KSK.Contains(o.TDL_SERVICE_CODE ?? "")).ToList();
                        if (IsNotNullOrEmpty(ksk))
                        {
                            rdoKSK.COUNT_TREATMENT++;
                            rdoKSK.TOTAL_PRICE += hisSereServs.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                            rdoKSK.TOTAL_PATIENT_PRICE += hisSereServs.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                        }
                        else
                        {
                            rdoExam.COUNT_TREATMENT++;
                            rdoExam.TOTAL_PRICE += hisSereServs.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                            rdoExam.TOTAL_PATIENT_PRICE += hisSereServs.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                        }
                    }
                    else
                    {
                        foreach (var item in hisSereServs)
                        {
                            if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                            {
                                item.TDL_REQUEST_ROOM_ID = item.TDL_EXECUTE_ROOM_ID;
                                item.REQUEST_ROOM_CODE = item.EXECUTE_ROOM_CODE;
                                item.REQUEST_ROOM_NAME = item.EXECUTE_ROOM_NAME;
                                item.TDL_REQUEST_DEPARTMENT_ID = item.TDL_EXECUTE_DEPARTMENT_ID;
                                item.REQUEST_DEPARTMENT_CODE = item.EXECUTE_DEPARTMENT_CODE;
                                item.REQUEST_DEPARTMENT_NAME = item.EXECUTE_DEPARTMENT_NAME;
                            }
                        }
                        if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                        {
                            var GroupByRequestRoom = hisSereServs.GroupBy(o => o.REQUEST_ROOM_CODE).ToList();
                            foreach (var item in GroupByRequestRoom)
                            {
                                List<DataGet> listSub = item.ToList<DataGet>();
                                Mrs00354RDO rdo = new Mrs00354RDO();
                                rdo.COUNT_TREATMENT = 1;
                                rdo.ROOM_ID = listSub.First().TDL_REQUEST_ROOM_ID ?? 0;
                                rdo.ROOM_CODE = listSub.First().REQUEST_ROOM_CODE;
                                rdo.ROOM_NAME = listSub.First().REQUEST_ROOM_NAME;
                                rdo.TOTAL_PRICE = listSub.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                                rdo.MEDICINE_PRICE = listSub.Where(o => o.MEDICINE_ID.HasValue).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                                rdo.TOTAL_PATIENT_PRICE_HEIN = listSub.Where(o => o.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                                rdo.TOTAL_PATIENT_PRICE = listSub.Where(o => o.PATIENT_TYPE_ID != MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                                listExamHeinRdo.Add(rdo);
                            }



                        }
                        else
                        {
                            var GroupByRequestDepartment = hisSereServs.GroupBy(o => o.REQUEST_DEPARTMENT_CODE).ToList();
                            foreach (var item in GroupByRequestDepartment)
                            {
                                List<DataGet> listSub = item.ToList<DataGet>();
                                Mrs00354RDO rdo = new Mrs00354RDO();
                                rdo.COUNT_TREATMENT = 1;
                                rdo.DEPARTMENT_ID = listSub.First().TDL_REQUEST_DEPARTMENT_ID ?? 0;
                                rdo.DEPARTMENT_CODE = listSub.First().REQUEST_DEPARTMENT_CODE;
                                rdo.DEPARTMENT_NAME = listSub.First().REQUEST_DEPARTMENT_NAME;
                                rdo.TOTAL_PRICE = listSub.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                                rdo.MEDICINE_PRICE = listSub.Where(o => o.MEDICINE_ID.HasValue).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                                rdo.TOTAL_PATIENT_PRICE_HEIN = listSub.Where(o => o.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                                rdo.TOTAL_PATIENT_PRICE = listSub.Where(o => o.PATIENT_TYPE_ID != MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);


                                if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                                {
                                    if (treatment.TDL_PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                    {

                                        listTreatOutHeinRdo.Add(rdo);
                                    }
                                    else
                                    {
                                        listTreatOutRdo.Add(rdo);
                                    }
                                }
                                else
                                {
                                    if (treatment.TDL_PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                    {

                                        listTreatInHeinRdo.Add(rdo);
                                    }
                                    else
                                    {
                                        listTreatInRdo.Add(rdo);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void EndRoomDepartmentPrice(DataGet treatment, List<DataGet> hisSereServs)
        {
            {
                if (!dicKhoa.ContainsKey(treatment.END_DEPARTMENT_ID ?? 0))
                {
                    dicKhoa[treatment.END_DEPARTMENT_ID ?? 0] = new TongKhoaPhong();
                    dicKhoa[treatment.END_DEPARTMENT_ID ?? 0].ID = treatment.END_DEPARTMENT_ID ?? 0;
                    dicKhoa[treatment.END_DEPARTMENT_ID ?? 0].Ma = treatment.END_DEPARTMENT_CODE;
                    dicKhoa[treatment.END_DEPARTMENT_ID ?? 0].Khoa = treatment.END_DEPARTMENT_NAME;
                    //dicKhoa[treatment.END_DEPARTMENT_ID??0].IsRoom = false;
                }
                AddPriceTo(treatment, hisSereServs.Where(o => o.TDL_TREATMENT_ID == treatment.TDL_TREATMENT_ID).ToList(), dicKhoa[treatment.END_DEPARTMENT_ID ?? 0]);
                AddAmountTo(treatment, dicKhoa[treatment.END_DEPARTMENT_ID ?? 0]);

                {

                    if (!dicPhong.ContainsKey(treatment.END_ROOM_ID ?? 0))
                    {
                        dicPhong[treatment.END_ROOM_ID ?? 0] = new TongKhoaPhong();
                        dicPhong[treatment.END_ROOM_ID ?? 0].ID = (treatment.END_DEPARTMENT_ID ?? 0);
                        dicPhong[treatment.END_ROOM_ID ?? 0].Ma = treatment.END_ROOM_CODE;
                        dicPhong[treatment.END_ROOM_ID ?? 0].Khoa = treatment.END_ROOM_NAME;
                        //dicPhong[treatment.END_ROOM_ID??0].IsRoom = true;
                    }
                    AddPriceTo(treatment, hisSereServs.Where(o => o.TDL_TREATMENT_ID == treatment.TDL_TREATMENT_ID).ToList(), dicPhong[treatment.END_ROOM_ID ?? 0]);
                    AddAmountTo(treatment, dicPhong[treatment.END_ROOM_ID ?? 0]);
                }

            }

        }

        private void RoomDepartmentPrice(DataGet treatment, List<DataGet> hisSereServs)
        {
            var groupByDepartment = hisSereServs.GroupBy(o => o.TDL_REQUEST_DEPARTMENT_ID ?? 0);
            foreach (var department in groupByDepartment)
            {
                if (!dicKhoaYc.ContainsKey(department.Key))
                {
                    dicKhoaYc[department.Key] = new TongKhoaPhong();
                    dicKhoaYc[department.Key].ID = department.Key;
                    dicKhoaYc[department.Key].Ma = department.First().REQUEST_DEPARTMENT_CODE;
                    dicKhoaYc[department.Key].Khoa = department.First().REQUEST_DEPARTMENT_NAME;
                    //dicKhoaYc[department.Key].IsRoom = false;
                }
                AddPriceTo(treatment, department.ToList(), dicKhoaYc[department.Key]);
                AddAmountTo(treatment, dicKhoaYc[department.Key]);

                var groupByRoom = department.GroupBy(o => o.TDL_REQUEST_ROOM_ID ?? 0);
                foreach (var item in groupByRoom)
                {

                    if (!dicPhongYc.ContainsKey(item.Key))
                    {
                        dicPhongYc[item.Key] = new TongKhoaPhong();
                        dicPhongYc[item.Key].ID = item.First().TDL_REQUEST_DEPARTMENT_ID ?? 0;
                        dicPhongYc[item.Key].Ma = item.First().REQUEST_ROOM_CODE;
                        dicPhongYc[item.Key].Khoa = item.First().REQUEST_ROOM_NAME;
                        //dicPhongYc[item.Key].IsRoom = true;
                    }
                    AddPriceTo(treatment, item.ToList(), dicPhongYc[item.Key]);
                    AddAmountTo(treatment, dicPhongYc[item.Key]);
                }

            }

        }

        private void AddAmountTo(DataGet treatment, TongKhoaPhong tong)
        {
            //noi tru
            if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
            {
                if (treatment.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    tong.SoLuot_NT_BH += 1;
                }
                else
                {
                    tong.SoLuot_NT_TP += 1;
                }

            }
            // ngoai tru
            if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
            {
                if (treatment.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    tong.SoLuot_NGT_BH += 1;
                }
                else
                {
                    tong.SoLuot_NGT_TP += 1;
                }

            }
            //kham
            if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
            {
                if (treatment.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    tong.SoLuot_KH_BH += 1;
                }
                else
                {
                    tong.SoLuot_KH_TP += 1;
                }
            }
            //ban ngay
            if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
            {
                if (treatment.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    tong.SoLuot_BNG_BH += 1;
                }
                else
                {
                    tong.SoLuot_BNG_TP += 1;
                }
            }

        }

        private void AddPriceTo(DataGet treatment, List<DataGet> item, TongKhoaPhong tong)
        {
            //noi tru
            if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
            {

                tong.Sotien_NT_BHTT += item.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                tong.Sotien_NT_BNTT += item.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                tong.Sotien_NT_TP += item.Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
            }
            // ngoai tru
            if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
            {

                tong.Sotien_NGT_BHTT += item.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                tong.Sotien_NGT_BNTT += item.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                tong.Sotien_NGT_TP += item.Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
            }
            //kham
            if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
            {

                tong.Sotien_KH_BHTT += item.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                tong.Sotien_KH_BNTT += item.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                tong.Sotien_KH_TP += item.Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
            }
            //ban ngay
            if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
            {

                tong.Sotien_BNG_BHTT += item.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                tong.Sotien_BNG_BNTT += item.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                tong.Sotien_BNG_TP += item.Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
            }
            tong.Sotien_DTT += item.Sum(s => s.BILL_PRICE ?? 0);
            tong.Sotien += item.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
            tong.Sotien_BHTT += item.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
            tong.Sotien_BNTT += item.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
            tong.Sotien_TP += item.Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));

            tong.Sotien_giuong += item.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).Sum(s => s.VIR_TOTAL_PRICE ?? 0);

            tong.Sotien_XN += item.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Sum(s => s.VIR_TOTAL_PRICE ?? 0);

            tong.Sotien_Thuoc += item.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(s => s.VIR_TOTAL_PRICE ?? 0);

            tong.Sotien_Vattu += item.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Sum(s => s.VIR_TOTAL_PRICE ?? 0);

            tong.Sotien_pttt += item.Where(p => !this.serviceCodeECGs.Contains(p.TDL_SERVICE_CODE)).Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).Sum(s => s.VIR_TOTAL_PRICE ?? 0);

            tong.Sotien_kham += item.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(s => s.VIR_TOTAL_PRICE ?? 0);

            tong.Sotien_NS += item.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS).Sum(s => s.VIR_TOTAL_PRICE ?? 0);

            tong.Sotien_sieuam += item.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).Sum(s => s.VIR_TOTAL_PRICE ?? 0);

            tong.Sotien_xq += item.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).Sum(s => s.VIR_TOTAL_PRICE ?? 0);

            tong.Sotien_DIENTIM += item.Where(o => this.serviceCodeECGs.Contains(o.TDL_SERVICE_CODE)).Sum(s => s.VIR_TOTAL_PRICE ?? 0);

            tong.Sotien_khac += item.Where(p => !this.serviceCodeECGs.Contains(p.TDL_SERVICE_CODE)).Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PHCN || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__AN).Sum(s => s.VIR_TOTAL_PRICE ?? 0);

            var groupSv = item.GroupBy(o => o.TDL_SERVICE_CODE ?? "NONE");
            if (tong.DIC_SV_TOTAL_PRICE == null)
            {
                tong.DIC_SV_TOTAL_PRICE = new Dictionary<string, decimal>();
            }
            foreach (var gr in groupSv)
            {
                if (tong.DIC_SV_TOTAL_PRICE.ContainsKey(gr.Key))
                {
                    tong.DIC_SV_TOTAL_PRICE[gr.Key] += gr.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                }
                else
                {

                    tong.DIC_SV_TOTAL_PRICE.Add(gr.Key, gr.Sum(s => s.VIR_TOTAL_PRICE ?? 0));
                }
            }
        }

        void ProcessRdo()
        {
            try
            {
                if (IsNotNullOrEmpty(listExamHeinRdo))
                {
                    Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => listExamHeinRdo.Count), listExamHeinRdo.Count));
                    listExamHeinRdo = listExamHeinRdo.GroupBy(o => o.ROOM_ID).Select(s => new Mrs00354RDO() { ROOM_ID = s.First().ROOM_ID, ROOM_CODE = s.First().ROOM_CODE, ROOM_NAME = s.First().ROOM_NAME, COUNT_TREATMENT = s.Sum(s1 => s1.COUNT_TREATMENT), TOTAL_PRICE = s.Sum(s2 => s2.TOTAL_PRICE), MEDICINE_PRICE = s.Sum(s3 => s3.MEDICINE_PRICE), TOTAL_PATIENT_PRICE_HEIN = s.Sum(s4 => s4.TOTAL_PATIENT_PRICE_HEIN), TOTAL_PATIENT_PRICE = s.Sum(s5 => s5.TOTAL_PATIENT_PRICE) }).ToList();
                }

                if (IsNotNullOrEmpty(listTreatInHeinRdo))
                {
                    listTreatInHeinRdo = listTreatInHeinRdo.GroupBy(o => o.DEPARTMENT_ID).Select(s => new Mrs00354RDO() { DEPARTMENT_ID = s.First().DEPARTMENT_ID, DEPARTMENT_CODE = s.First().DEPARTMENT_CODE, DEPARTMENT_NAME = s.First().DEPARTMENT_NAME, COUNT_TREATMENT = s.Sum(s1 => s1.COUNT_TREATMENT), TOTAL_PRICE = s.Sum(s2 => s2.TOTAL_PRICE), MEDICINE_PRICE = s.Sum(s3 => s3.MEDICINE_PRICE), TOTAL_PATIENT_PRICE_HEIN = s.Sum(s4 => s4.TOTAL_PATIENT_PRICE_HEIN), TOTAL_PATIENT_PRICE = s.Sum(s5 => s5.TOTAL_PATIENT_PRICE) }).ToList();
                }

                if (IsNotNullOrEmpty(listTreatInRdo))
                {
                    listTreatInRdo = listTreatInRdo.GroupBy(o => o.DEPARTMENT_ID).Select(s => new Mrs00354RDO() { DEPARTMENT_ID = s.First().DEPARTMENT_ID, DEPARTMENT_CODE = s.First().DEPARTMENT_CODE, DEPARTMENT_NAME = s.First().DEPARTMENT_NAME, COUNT_TREATMENT = s.Sum(s1 => s1.COUNT_TREATMENT), TOTAL_PRICE = s.Sum(s2 => s2.TOTAL_PRICE), MEDICINE_PRICE = s.Sum(s3 => s3.MEDICINE_PRICE), TOTAL_PATIENT_PRICE_HEIN = s.Sum(s4 => s4.TOTAL_PATIENT_PRICE_HEIN), TOTAL_PATIENT_PRICE = s.Sum(s5 => s5.TOTAL_PATIENT_PRICE) }).ToList();
                }

                if (IsNotNullOrEmpty(listTreatOutHeinRdo))
                {
                    listTreatOutHeinRdo = listTreatOutHeinRdo.GroupBy(o => o.DEPARTMENT_ID).Select(s => new Mrs00354RDO() { DEPARTMENT_ID = s.First().DEPARTMENT_ID, DEPARTMENT_CODE = s.First().DEPARTMENT_CODE, DEPARTMENT_NAME = s.First().DEPARTMENT_NAME, COUNT_TREATMENT = s.Sum(s1 => s1.COUNT_TREATMENT), TOTAL_PRICE = s.Sum(s2 => s2.TOTAL_PRICE), MEDICINE_PRICE = s.Sum(s3 => s3.MEDICINE_PRICE), TOTAL_PATIENT_PRICE_HEIN = s.Sum(s4 => s4.TOTAL_PATIENT_PRICE_HEIN), TOTAL_PATIENT_PRICE = s.Sum(s5 => s5.TOTAL_PATIENT_PRICE) }).ToList();
                }

                if (IsNotNullOrEmpty(listTreatOutRdo))
                {
                    listTreatOutRdo = listTreatOutRdo.GroupBy(o => o.DEPARTMENT_ID).Select(s => new Mrs00354RDO() { DEPARTMENT_ID = s.First().DEPARTMENT_ID, DEPARTMENT_CODE = s.First().DEPARTMENT_CODE, DEPARTMENT_NAME = s.First().DEPARTMENT_NAME, COUNT_TREATMENT = s.Sum(s1 => s1.COUNT_TREATMENT), TOTAL_PRICE = s.Sum(s2 => s2.TOTAL_PRICE), MEDICINE_PRICE = s.Sum(s3 => s3.MEDICINE_PRICE), TOTAL_PATIENT_PRICE_HEIN = s.Sum(s4 => s4.TOTAL_PATIENT_PRICE_HEIN), TOTAL_PATIENT_PRICE = s.Sum(s5 => s5.TOTAL_PATIENT_PRICE) }).ToList();
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
                dicSingleTag.Add("COUNT_TREATMENT_KSK", rdoKSK.COUNT_TREATMENT);
                dicSingleTag.Add("TOTAL_PRICE_KSK", rdoKSK.TOTAL_PRICE);
                dicSingleTag.Add("TOTAL_PATIENT_PRICE_KSK", rdoKSK.TOTAL_PATIENT_PRICE);
                dicSingleTag.Add("COUNT_TREATMENT_PK", rdoExam.COUNT_TREATMENT);
                dicSingleTag.Add("TOTAL_PRICE_PK", rdoExam.TOTAL_PRICE);
                dicSingleTag.Add("TOTAL_PATIENT_PRICE_PK", rdoExam.TOTAL_PATIENT_PRICE);
                objectTag.AddObjectData(store, "ReportExamHein", listExamHeinRdo);
                objectTag.AddObjectData(store, "ReportTreatOutHein", listTreatOutHeinRdo);
                objectTag.AddObjectData(store, "ReportTreatOut", listTreatOutRdo);
                objectTag.AddObjectData(store, "ReportTreatInHein", listTreatInHeinRdo);
                objectTag.AddObjectData(store, "ReportTreatIn", listTreatInRdo);
                if (castFilter.IS_SWAP_DEPARTMENT_AND_DEPARTMENTYC == true)
                {
                    objectTag.AddObjectData(store, "DepartmentYc", dicKhoa.Select(o => o.Value).ToList());
                    objectTag.AddObjectData(store, "RoomYc", dicPhong.Select(o => o.Value).OrderBy(o => o.ID).ToList());
                    objectTag.AddObjectData(store, "Department", dicKhoaYc.Select(o => o.Value).ToList());
                    objectTag.AddObjectData(store, "Room", dicPhongYc.Select(o => o.Value).ToList());
                }
                else
                {
                    objectTag.AddObjectData(store, "Department", dicKhoa.Select(o => o.Value).ToList());
                    objectTag.AddObjectData(store, "Room", dicPhong.Select(o => o.Value).OrderBy(o => o.ID).ToList());
                    objectTag.AddObjectData(store, "DepartmentYc", dicKhoaYc.Select(o => o.Value).ToList());
                    objectTag.AddObjectData(store, "RoomYc", dicPhongYc.Select(o => o.Value).ToList());
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
