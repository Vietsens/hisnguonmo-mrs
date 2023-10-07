using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartmentTran;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00159
{
    public class Mrs00159Processor : AbstractProcessor
    {
        List<Mrs00159RDO> _listSarReportMrs00159Rdos = new List<Mrs00159RDO>();
        List<V_HIS_DEPARTMENT_TRAN> listParent = new List<V_HIS_DEPARTMENT_TRAN>();
        List<TOTAL_DEPARTMENT> _listtotalDepartments = new List<TOTAL_DEPARTMENT>();
        Mrs00159Filter CastFilter;

        public Mrs00159Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00159Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                CastFilter = ((Mrs00159Filter)reportFilter);
                var paramGet = new CommonParam();
                CastFilter = (Mrs00159Filter)this.reportFilter;
                LogSystem.Debug("Bat dau lay du lieu filter MRS00159: " +
                    LogUtil.TraceData(LogUtil.GetMemberName(() => CastFilter), CastFilter));
                LogSystem.Info("Bat dau lay du lieu filter MRS00159 ===============================================================");

                //-------------------------------------------------------------------------------------------------- V_HIS_TREATMENT - hồ sơ khám chữa bệnh
                var listTreatmentViews = new List<V_HIS_TREATMENT>();
                var metyFilterTreatment = new HisTreatmentViewFilterQuery
                {
                    IS_OUT = true,
                    END_ROOM_IDs = CastFilter.ROOM_IDs //new List<long> { 25 }//
                };
                var listTreatments = new HisTreatmentManager(paramGet).GetView(metyFilterTreatment);
                //--------------------------------------------------------------------------------------------------V_HIS_DEPARTMENT_TRAN
                var listDepartmentTran = new List<V_HIS_DEPARTMENT_TRAN>();
                var listTreatmentIds = listTreatments.Select(s => s.ID).ToList();
                var skip = 0;
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listIds = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterDepartmentTran = new HisDepartmentTranViewFilterQuery
                    {
                        TREATMENT_IDs = listIds,
                        DEPARTMENT_IDs = CastFilter.DEPARTMENT_IDs,//Filter là để dùng chung cho tất cả các bảng, bảng nào cần cái nào thì tự gọi ra.
                        //new List<long> { 22 }
                    };
                    var departmentTran = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(metyFilterDepartmentTran);
                    listDepartmentTran.AddRange(departmentTran);
                }

                listTreatmentViews = listTreatments.Where(s => listDepartmentTran.Select(d => d.TREATMENT_ID).Contains(s.ID)).ToList();

                var listTreatmentIDs = listTreatmentViews.Select(s => s.ID).ToList(); // select V_HIS_TREATMENT (ID)

                //--------------------------------------------------------------------------------------------------V_HIS_PATIENT_TYPE_ALTER
                //var listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
                //while (listTreatmentIDs.Count - skip > 0)
                //{
                //    var listIds = listTreatmentIDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                var metyFilterPatientTypeAlter = new HisPatientTypeAlterViewFilterQuery
                {
                    PATIENT_TYPE_ID = CastFilter.PATIENT_TYPE_ID,
                    TREATMENT_IDs = listTreatmentIDs
                };
                var listPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(metyFilterPatientTypeAlter);

                //--------------------------------------------------------------------------------------------------V_HIS_SEVE_SERV
                var listServers = new List<V_HIS_SERE_SERV_2>();
                skip = 0;
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listIDs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterServer = new HisSereServView2FilterQuery
                    {
                        TREATMENT_IDs = listIDs,
                    };
                    var listServer = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView2(metyFilterServer);
                    listServers.AddRange(listServer);
                }
                //--------------------------------------------------------------------------------------------V_HIS_ICD

                var listSereServCodes = listServers.Where(s => !String.IsNullOrEmpty(s.ICD_CODE)).Select(s => s.ICD_CODE).ToList();
                var listICD = new List<HIS_ICD>();
                skip = 0;
                while (listSereServCodes.Count - skip > 0)
                {
                    var listCodes = listSereServCodes.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterICD = new HisIcdFilterQuery
                    {
                        ICD_CODEs = listCodes,
                    };
                    var listIcdView = new MOS.MANAGER.HisIcd.HisIcdManager(paramGet).Get(metyFilterICD);
                    listICD.AddRange(listIcdView);
                }

                //----------------------------------------------------------------------------------------------V_HIS_REPORT_TYPE_CAT
                var metyFilterReportTypeCat = new HisReportTypeCatFilterQuery
                {
                    REPORT_TYPE_CODE__EXACT = MRS.MANAGER.Config.HisReportTypeCatCFG.REPORT_TYPE_CODE_MRS00159
                };
                var listReportTypeCats = new MOS.MANAGER.HisReportTypeCat.HisReportTypeCatManager(paramGet).Get(metyFilterReportTypeCat);
                var listReportTypeCatIds = listReportTypeCats.Select(s => s.ID).ToList();
                //---------------------------------------------------------------------------------------------V_HIS_SERVICE_RETY_CAT
                var listServiceRetyCats = new List<V_HIS_SERVICE_RETY_CAT>();
                skip = 0;
                while (listReportTypeCatIds.Count - skip > 0)
                {
                    var listIds = listReportTypeCatIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterServiceRetyCat = new HisServiceRetyCatViewFilterQuery
                    {
                        REPORT_TYPE_CAT_IDs = listIds
                    };
                    var listServiceRetyCatViews = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(paramGet).GetView(metyFilterServiceRetyCat);
                    listServiceRetyCats.AddRange(listServiceRetyCatViews);
                    //var  = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(paramGet).GetView(metyFilterServiceRetyCat); 
                }
                //----------------------------------------------------------------------------------------------HIS_TRANSISTION
                //Thông tin giao dịch
                var metyFilterTransaction = new HisTransactionViewFilterQuery
                {
                    TREATMENT_IDs = listTreatmentIds,
                    HAS_SALL_TYPE = false,
                };
                var listTransactions = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(metyFilterTransaction);
                var listTransactionId = listTransactions.Select(s => s.ID).ToList();
                ProcessFilterData(listPatientTypeAlter, listTreatmentViews, listServers,
                     listTransactions, listServiceRetyCats, listICD, listDepartmentTran);
                result = true;

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
            var result = false;
            try
            {

                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessFilterData(List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter,
            List<V_HIS_TREATMENT> listTreatmentViews, List<V_HIS_SERE_SERV_2> listServers,
            List<V_HIS_TRANSACTION> listTransactions,
            List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCats, List<HIS_ICD> listICD, List<V_HIS_DEPARTMENT_TRAN> listDepartmentTran)
        {
            try
            {
                LogSystem.Info("Bat dau xu ly du lieu MRS00159 ===============================================================");
                //------------------------------------------------------------------------------------------Select PatientTypeAlter theo Treatment_Id
                var listPatientTypeAlterView = listPatientTypeAlter.Select(s => s.TREATMENT_ID).ToList();
                // department_tran - thong tin BN trong Khoa, lay theo department_id

                var listTreatments = new List<V_HIS_TREATMENT_NEW>();
                //gom nhóm theo Department_id đồng thời so sánh với patient_type_id
                var listDepart_ment_Trans = listDepartmentTran.Where(s => listPatientTypeAlter.Select(ss => ss.TREATMENT_ID).Contains(s.TREATMENT_ID)).GroupBy(s => s.DEPARTMENT_ID).ToList();
                foreach (var listDepart_ment_view in listDepart_ment_Trans)
                {
                    listParent.Add(listDepart_ment_view.First());
                    //select theo Treatment_id
                    var listTreatmentIds = listDepart_ment_view.Select(o => o.TREATMENT_ID).ToList();
                    var treatments = listTreatmentViews.Where(s => listTreatmentIds.Contains(s.ID)).ToList();

                    foreach (var treatment in treatments)
                    {
                        var listdepartmentTran = new V_HIS_TREATMENT_NEW
                        {
                            DEPARTMENT_ID = listDepart_ment_view.Key,
                            V_HIS_TREATMENT = treatment
                        };
                        listTreatments.Add(listdepartmentTran);
                    }
                }
                foreach (var listTreatment in listTreatments)
                {
                    // lấy dữ liệu cho so tien benh nhan phai thanh toan
                    var Amount_Serv = listServers.Where(s => s.TDL_TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID).Sum(s => s.VIR_TOTAL_PATIENT_PRICE);
                    //lấy dữ liệu cho so tien da thu tu benh nhan
                    var Amout_Bill = listTransactions.Where(s => s.TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID && s.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Sum(s => s.AMOUNT);
                    var Amount_Deposit = listTransactions.Where(s => s.TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID && s.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).Sum(s => s.AMOUNT);
                    var Amount_Repay = listTransactions.Where(s => s.TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID && s.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).Sum(s => s.AMOUNT);
                    var Amount_BHYT = listServers.Where(s => s.TDL_TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID).Sum(s => s.VIR_TOTAL_HEIN_PRICE);
                    //Tổng tiền Xét nghiệm
                    var xn = listServiceRetyCats.Where(s => s.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Select(s => s.SERVICE_ID);
                    var xetnghiem = listServers.Where(s => s.TDL_TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID && xn.Contains(s.SERVICE_ID)).Sum(s => s.PRICE);
                    // Tổng tiền CĐHA
                    var CDHA = listServiceRetyCats.Where(s => s.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).Select(s => s.SERVICE_ID);
                    var CĐHA = listServers.Where(s => s.TDL_TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID && CDHA.Contains(s.SERVICE_ID)).Sum(s => s.PRICE);
                    // Tổng tiền thuốc, DT
                    var Th_DT = listServiceRetyCats.Where(s => s.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Select(s => s.SERVICE_ID);
                    var THUOC_DT = listServers.Where(s => s.TDL_TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID && Th_DT.Contains(s.SERVICE_ID)).Sum(s => s.PRICE);
                    //Tổng tiền Máu
                    var mau = listServiceRetyCats.Where(s => s.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU).Select(s => s.SERVICE_ID);
                    var MAU = listServers.Where(s => s.TDL_TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID && mau.Contains(s.SERVICE_ID)).Sum(s => s.PRICE);
                    //Tổng tiền PTTT
                    var pttt = listServiceRetyCats.Where(s => s.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT || s.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).Select(s => s.SERVICE_ID);
                    var PTTT = listServers.Where(s => s.TDL_TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID && pttt.Contains(s.SERVICE_ID)).Sum(s => s.PRICE);
                    // Tổng tiền VTYT
                    var vtyt = listServiceRetyCats.Where(s => s.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Select(s => s.SERVICE_ID);
                    var VTYT = listServers.Where(s => s.TDL_TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID && vtyt.Contains(s.SERVICE_ID)).Sum(s => s.PRICE);
                    //// TỔng tiền DVKTC
                    //var dvktc = listServiceRetyCats.Where(s => s.H == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.).Select(s => s.SERVICE_ID); 
                    //var DrrrrrVKTC = listServers.Where(s => s.TDL_TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID && dvktc.Contains(s.SERVICE_ID)).ToList(); 
                    //var DVKTC = listServers.Where(s => s.TDL_TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID && dvktc.Contains(s.SERVICE_ID)).Sum(s => s.PRICE); 
                    //// Tổng tiền CATEGORY_CODE_159_ThuocK
                    //var thuoc_k = listServiceRetyCats.Where(s => s.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE).Select(s => s.SERVICE_ID); 
                    //var THUOC_K = listServers.Where(s => s.TDL_TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID && thuoc_k.Contains(s.SERVICE_ID)).Sum(s => s.PRICE); 
                    //Tổng tiền CATEGORY_CODE_159_TK
                    var tien_kham = listServiceRetyCats.Where(s => s.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Select(s => s.SERVICE_ID);
                    var TIEN_KHAM = listServers.Where(s => s.TDL_TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID && tien_kham.Contains(s.SERVICE_ID)).Sum(s => s.PRICE);
                    // Tổng tiền CATEGORY_CODE_159_TG
                    var tien_giuong = listServiceRetyCats.Where(s => s.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).Select(s => s.SERVICE_ID);
                    var TIEN_GIUONG = listServers.Where(s => s.TDL_TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID && tien_giuong.Contains(s.SERVICE_ID)).Sum(s => s.PRICE);
                    // Tổng tiền CATEGORY_CODE_159_CPVC
                    var cpvc = listServiceRetyCats.Where(s => s.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC).Select(s => s.SERVICE_ID);
                    var CPVC = listServers.Where(s => s.TDL_TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID && cpvc.Contains(s.SERVICE_ID)).Sum(s => s.PRICE);
                    //// Tổng tiền CATEGORY_CODE_159_Mien
                    //var mien = listServiceRetyCats.Where(s => s.REPORT_TYPE_CAT_ID == HisReportTypeCatCFG.CATEGORY_CODE_159_Mien).Select(s => s.SERVICE_ID); 
                    //var MIEN = listServers.Where(s => s.TDL_TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID && mien.Contains(s.SERVICE_ID)).Sum(s => s.PRICE); 

                    //---lấy theo điều kiện so sánh treatment_id và patient_type_alter
                    var listPatientType = listPatientTypeAlter.Where(s => s.TREATMENT_ID == listTreatment.V_HIS_TREATMENT.ID && s.HEIN_CARD_NUMBER != null).ToList();

                    var heinCardNumber = string.Empty;
                    var heinMediOrgCode = string.Empty;
                    if (listPatientType.Count > 0)
                    {
                        heinCardNumber = listPatientType.First().HEIN_CARD_NUMBER;
                        heinMediOrgCode = listPatientType.First().HEIN_MEDI_ORG_CODE;
                    }
                    Mrs00159RDO rdo = new Mrs00159RDO
                    {
                        PARENT_ID = listTreatment.DEPARTMENT_ID,//lay theo id
                        AMOUNT_BILL = Amout_Bill,
                        AMOUNT_DEPOSIT = Amount_Deposit,
                        AMOUNT_SERV = Amount_Serv,
                        AMOUNT_REPAY = Amount_Repay,
                        VIR_TOTAL_HEIN_PRICE = Amount_BHYT,
                        PATIENT_CODE = listTreatment.V_HIS_TREATMENT.TDL_PATIENT_CODE,
                        VIR_PATIENT_NAME = listTreatment.V_HIS_TREATMENT.TDL_PATIENT_NAME,
                        DOB = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listTreatment.V_HIS_TREATMENT.TDL_PATIENT_DOB),
                        GENDER_NAME = listTreatment.V_HIS_TREATMENT.TDL_PATIENT_GENDER_NAME,
                        HEIN_CARD_NUMBER = heinCardNumber.ToString(),
                        HEIN_MEDI_ORG_CODE = heinMediOrgCode.ToString(),
                        ICD_CODE = listTreatment.V_HIS_TREATMENT.ICD_CODE,
                        XN = xetnghiem,
                        CDHA = CĐHA,
                        THUOC = THUOC_DT,
                        PTTT = PTTT,
                        MAU = MAU,
                        VTYT = VTYT,
                        //DVKTC = DVKTC,
                        //THUOC_K = THUOC_K,
                        TIEN_KHAM = TIEN_KHAM,
                        TIEN_GIUONG = TIEN_GIUONG,
                        //MIEN = MIEN,
                        CREAT_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listTreatment.V_HIS_TREATMENT.CREATE_TIME.Value),//TH cần có giá trị thì .Value
                        TREATMENT_CODE = listTreatment.V_HIS_TREATMENT.TREATMENT_CODE,
                    };
                    _listSarReportMrs00159Rdos.Add(rdo);
                }
                var totalDepartments = _listSarReportMrs00159Rdos.GroupBy(s => s.PARENT_ID).ToList();
                foreach (var totalDepartment in totalDepartments)
                {
                    var xn = totalDepartment.Sum(s => s.XN);
                    var cdha = totalDepartment.Sum(s => s.CDHA);
                    var thuoc = totalDepartment.Sum(s => s.THUOC);
                    var mau = totalDepartment.Sum(s => s.MAU);
                    var pttt = totalDepartment.Sum(s => s.PTTT);
                    var vtyt = totalDepartment.Sum(s => s.VTYT);
                    var thuocK = totalDepartment.Sum(s => s.THUOC_K);
                    var tienKham = totalDepartment.Sum(s => s.TIEN_KHAM);
                    var tienGiuong = totalDepartment.Sum(s => s.TIEN_GIUONG);
                    var cpvc = totalDepartment.Sum(s => s.CPVC);
                    var mien = totalDepartment.Sum(s => s.MIEN);
                    var Amount_Serv = totalDepartment.Sum(s => s.AMOUNT_SERV);
                    var Amout_Bill = totalDepartment.Sum(s => s.AMOUNT_BILL);
                    var Amount_Deposit = totalDepartment.Sum(s => s.AMOUNT_DEPOSIT);
                    var Amount_Repay = totalDepartment.Sum(s => s.AMOUNT_REPAY);
                    var Amount_BHYT = totalDepartment.Sum(s => s.VIR_TOTAL_HEIN_PRICE);

                    var rdos = new TOTAL_DEPARTMENT
                    {
                        XN = xn,
                        CDHA = cdha,
                        THUOC = thuoc,
                        MAU = mau,
                        PTTT = pttt,
                        VTYT = vtyt,
                        THUOC_K = thuocK,
                        TIEN_KHAM = tienKham,
                        TIEN_GIUONG = tienGiuong,
                        CPVC = cpvc,
                        MIEN = mien,
                        AMOUNT_BILL = Amout_Bill,
                        AMOUNT_REPAY = Amount_Repay,
                        AMOUNT_DEPOSIT = Amount_Deposit,
                        AMOUNT_SERV = Amount_Serv,
                        VIR_TOTAL_HEIN_PRICE = Amount_BHYT
                    };
                    _listtotalDepartments.Add(rdos);
                }

                LogSystem.Info("Ket thuc xu ly du lieu MRS00159 ===============================================================");
            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                objectTag.AddObjectData(store, "Report", _listSarReportMrs00159Rdos);
                objectTag.AddObjectData(store, "TotalDepartment", _listtotalDepartments);
                objectTag.AddObjectData(store, "Parent", listParent);
                objectTag.AddRelationship(store, "Parent", "Report", "DEPARTMENT_ID", "PARENT_ID");
                objectTag.AddRelationship(store, "Parent", "TotalDepartment", "DEPARTMENT_ID", "PARENT_ID");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
