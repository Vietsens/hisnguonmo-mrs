using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisImpMest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisExpMestType;
using MOS.MANAGER.HisMediStock;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisImpMestType;
using Inventec.Common.Logging;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MRS.MANAGER.Base;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MRS.MANAGER.Core.MrsReport.RDO;
using ACS.EFMODEL.DataModels;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisTreatmentBedRoom;

namespace MRS.Processor.Mrs00382
{
    class Mrs00382Processor : AbstractProcessor
    {
        Mrs00382Filter mrs00382Filter = new Mrs00382Filter();
        List<V_HIS_EXP_MEST> listExp = new List<V_HIS_EXP_MEST>();
        Dictionary<long, V_HIS_EXP_MEST> dicExp = new Dictionary<long, V_HIS_EXP_MEST>();
        List<V_HIS_IMP_MEST> listImp = new List<V_HIS_IMP_MEST>();
        List<HIS_EXP_MEST_TYPE> listExpType = new List<HIS_EXP_MEST_TYPE>();
        List<HIS_IMP_MEST_TYPE> listImpType = new List<HIS_IMP_MEST_TYPE>();
        List<Mrs00382AgeRDO> listRdoAge = new List<Mrs00382AgeRDO>();
        //cả tạo và xuất trong thời gian báo cáo
        List<V_HIS_EXP_MEST_MEDICINE> listMedicineAll = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listMaterialAll = new List<V_HIS_EXP_MEST_MATERIAL>();

        //xuất trong thời gian báo cáo
        List<V_HIS_EXP_MEST_MEDICINE> listMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
        //tạo trong thời gian báo cáo và là đơn phòng khám
        List<V_HIS_EXP_MEST_MEDICINE> listMedicineDpk = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listMaterialDpk = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<HIS_SERVICE> listService = new List<HIS_SERVICE>();
        List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>();
        Dictionary<long, List<HIS_SERE_SERV>> dicSereServ = new Dictionary<long, List<HIS_SERE_SERV>>();
        List<ParentService> listParentService = new List<ParentService>();
        List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<Mrs00382RDO> listRdo = new List<Mrs00382RDO>();
        List<Mrs00382RDO> listRdoDpkGroupEMT = new List<Mrs00382RDO>();
        List<Mrs00382RDO> listRdoExpMest = new List<Mrs00382RDO>();
        List<Mrs00382RDO> listRdoExpMestNew = new List<Mrs00382RDO>();
        List<Mrs00382RDO> listRdoExpMestType = new List<Mrs00382RDO>();
        List<Mrs00382RDO> listRdoDetailExp = new List<Mrs00382RDO>();
        List<Mrs00382RDO> listRdoDetailDpk = new List<Mrs00382RDO>();
        List<Mrs00382RDO> listForPatientType = new List<Mrs00382RDO>();
        List<Mrs00382RDO> listForCabinet = new List<Mrs00382RDO>();
        //danh sách phiếu xuất tổng hợp
        List<HIS_EXP_MEST> listAggrExpMest = new List<HIS_EXP_MEST>();
        List<HIS_TREATMENT_BED_ROOM> listBedLog = new List<HIS_TREATMENT_BED_ROOM>();
        List<HIS_BED_ROOM> listBedRoom = new List<HIS_BED_ROOM>();
        List<HIS_BED> listBed = new List<HIS_BED>();

        List<MATERIAL_REUSABLING> listIsReusabling = new List<MATERIAL_REUSABLING>();

        List<HIS_MATERIAL_TYPE> listMaterialType = new List<HIS_MATERIAL_TYPE>();

        Dictionary<long, Mrs00382RDO> dicData = new Dictionary<long, Mrs00382RDO>();

        List<string> ListTempType = new List<string>();

        public long? PatientTypeId__BHYT = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
        public long? PatientTypeId__FEE = HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
        public long? PatientTypeId__IS_FREE = HisPatientTypeCFG.PATIENT_TYPE_ID__IS_FREE;

        public Mrs00382Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00382Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                //Lựa chọn dữ liệu cần xuất
                GetTempTypes();
                this.mrs00382Filter = (Mrs00382Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu .mrs00382..: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => mrs00382Filter), mrs00382Filter));

                //lấy dữ liệu xuất theo thời gian hoàn thành
                listExp = GetExpMestDone(mrs00382Filter);

                //thêm xuất theo thời gian tạo
                var listExpCreate = GetExpMestCreate(mrs00382Filter) ?? new List<V_HIS_EXP_MEST>();
                listExp.AddRange(listExpCreate);

                listExp = listExp.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                Inventec.Common.Logging.LogSystem.Info("Lay them cac phieu xuat tao trong thoi gian bao cao done: " + listExp.Count);

                //lọc theo các điều kiện lọc khác
                listExp = FilterOther(listExp, mrs00382Filter);

                //loại nhập xuất
                listExpType = new HisExpMestTypeManager().Get(new HisExpMestTypeFilterQuery());
                listImpType = new HisImpMestTypeManager().Get(new HisImpMestTypeFilterQuery());

                CommonParam paramGet = new CommonParam();
                if (listExp != null)
                {
                    //chi tiết thuốc xuất
                    List<long> expIdHasDetail = new List<long>();
                    listMedicineAll = GetExme(listExp, mrs00382Filter, ref expIdHasDetail);
                    Inventec.Common.Logging.LogSystem.Info("Lay chi tiet thuoc xuat trong phieu xuat done: " + listMedicineAll.Count);

                    listMaterialAll = GetExma(listExp, mrs00382Filter, ref expIdHasDetail);
                    Inventec.Common.Logging.LogSystem.Info("Lay chi tiet vat tu xuat trong phieu xuat done: " + listMaterialAll.Count);
                    //lọc lại phiếu xuất theo chi tiết xuất
                    var expID = expIdHasDetail.Distinct().ToList();
                    listExp = listExp.Where(o => expID.Contains(o.ID)).ToList();

                    //thêm dữ liệu phiếu xuất vào dictionary
                    dicExp = listExp.GroupBy(g => g.ID).ToDictionary(p => p.Key, q => q.First());

                    //get du lieu treatment cua phieu xuat
                    listTreatment = GetTreatment(listExp, mrs00382Filter);
                    Inventec.Common.Logging.LogSystem.Info("Lay danh sach ho so dieu tri done: " + listTreatment.Count);

                    //get du lieu serviceReq cua phieu xuat
                    listServiceReq = GetServiceReq(listExp);
                    Inventec.Common.Logging.LogSystem.Info("Lay danh sach y lenh cua thuoc vat tu done: " + listServiceReq.Count);

                    //get du lieu sereserv cua phieu xuat
                    dicSereServ = GetSereServ(listServiceReq);
                    Inventec.Common.Logging.LogSystem.Info("Lay danh sach bang ke cua thuoc vat tu done: " + dicSereServ.Values.Count);

                    //get du lieu phieu tong hop
                    listAggrExpMest = GetAggr(listExp);
                    Inventec.Common.Logging.LogSystem.Info("Lay danh sach phieu tong hop done: " + listAggrExpMest.Count);

                    //get du lieu tra thuoc
                    listImpMestMedicine = GetImme(listMedicineAll, mrs00382Filter);
                    Inventec.Common.Logging.LogSystem.Info("Lay danh sach phieu nhap tra thuoc done: " + listImpMestMedicine.Count);

                    //get du lieu tra vat tu
                    listImpMestMaterial = GetImma(listMaterialAll, mrs00382Filter);
                    Inventec.Common.Logging.LogSystem.Info("Lay danh sach phieu nhap tra vat tu done: " + listImpMestMedicine.Count);

                    var impMestIds = listImpMestMedicine.Select(p => p.IMP_MEST_ID).Distinct().ToList();
                    impMestIds.AddRange(listImpMestMaterial.Select(p => p.IMP_MEST_ID).Distinct().ToList());
                    impMestIds = impMestIds.Distinct().ToList();

                    //get du lieu phieu tra
                    listImp = GetImp(impMestIds);
                    Inventec.Common.Logging.LogSystem.Info("Lay danh sach phieu nhap tra done: " + listImp.Count);

                    //get du lieu buong benh cua benh nhan dung thuoc
                    listBedLog = GetTreatmentBedRoom(listTreatment);
                    Inventec.Common.Logging.LogSystem.Info("Lay danh sach lich su giuong done: " + listBedLog.Count);
                }

                //dịch vụ cha
                listParentService = GetParentService();

                //danh sách giường
                listBed = GetBed();

                //danh sách vật tư
                listMaterialType = GetMaty();

                //thông tin tái sử dụng
                listIsReusabling = GetIsReusabling(mrs00382Filter);
            }

            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<MATERIAL_REUSABLING> GetIsReusabling(Mrs00382Filter _mrs00382Filter)
        {
            List<MATERIAL_REUSABLING> result = new List<MATERIAL_REUSABLING>();
            string sql = @"select
MATERIAL_ID,
(case when min(imp_time)>{0} then 2 else 1 end) IS_REUSABLING
from v_his_imp_mest_material 
where 1=1
and imp_mest_stt_id=5
and imp_mest_type_id in (17)
and imp_time <{1}
and material_type_id in (select id from his_material_type where is_reusable=1)
group by
material_id";
            CommonParam paramGet = new CommonParam();
            result = new MOS.DAO.Sql.SqlDAO().GetSql<MATERIAL_REUSABLING>(paramGet, string.Format(sql, _mrs00382Filter.TIME_FROM, _mrs00382Filter.TIME_TO));
            return result;
        }

        private List<HIS_BED> GetBed()
        {
            string queryBed = "select * from his_bed";
            return new MOS.DAO.Sql.SqlDAO().GetSql<HIS_BED>(queryBed);
        }

        private List<HIS_MATERIAL_TYPE> GetMaty()
        {
            return new HisMaterialTypeManager().Get(new HisMaterialTypeFilterQuery());
        }

        private List<ParentService> GetParentService()
        {
            string queryParentService = "select (case when pr.service_code is null then 'NONE' else pr.service_code end) as service_code, (case when pr.service_name is null then 'NHÓM KHÁC' else pr.service_name end) as service_name, sv.service_code as CHILD_SERVICE_CODE from his_service pr join his_service sv on pr.id = sv.parent_id";
            return new MOS.DAO.Sql.SqlDAO().GetSql<ParentService>(queryParentService); new MOS.DAO.Sql.SqlDAO().GetSql<ParentService>(queryParentService);
        }

        private List<HIS_TREATMENT_BED_ROOM> GetTreatmentBedRoom(List<HIS_TREATMENT> _listTreatment)
        {
            List<HIS_TREATMENT_BED_ROOM> result = new List<HIS_TREATMENT_BED_ROOM>();
            int skip = 0;
            var treatmentIds = _listTreatment.Select(p => p.ID).Distinct().ToList();

            while (treatmentIds.Count - skip > 0)
            {
                var listIds = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();

                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                HisTreatmentBedRoomFilterQuery tbrFilter = new HisTreatmentBedRoomFilterQuery();
                tbrFilter.TREATMENT_IDs = listIds;
                var listTbrSub = new HisTreatmentBedRoomManager().Get(tbrFilter);
                if (listTbrSub != null)
                {
                    result.AddRange(listTbrSub);
                }
            }
            return result;
        }

        private List<V_HIS_IMP_MEST> GetImp(List<long> _impMestIds)
        {
            List<V_HIS_IMP_MEST> result = new List<V_HIS_IMP_MEST>();
            int skip = 0;
            while (_impMestIds.Count - skip > 0)
            {
                var listIds = _impMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();

                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                HisImpMestViewFilterQuery impFilter = new HisImpMestViewFilterQuery();
                impFilter.IDs = listIds;
                var listImpSub = new HisImpMestManager().GetView(impFilter);
                if (listImpSub != null)
                {
                    result.AddRange(listImpSub);
                }
            }

            return result;
        }

        private List<V_HIS_IMP_MEST_MATERIAL> GetImma(List<V_HIS_EXP_MEST_MATERIAL> _listMaterialAll, Mrs00382Filter mrs00382Filter)
        {
            List<V_HIS_IMP_MEST_MATERIAL> result = new List<V_HIS_IMP_MEST_MATERIAL>();
            int skip = 0;
            var materialIds = _listMaterialAll.Select(p => p.MATERIAL_TYPE_ID).Distinct().ToList();
            while (materialIds.Count - skip > 0)
            {
                var listIds = materialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();

                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                HisImpMestMaterialViewFilterQuery impFilter = new HisImpMestMaterialViewFilterQuery();
                impFilter.MATERIAL_TYPE_IDs = listIds;
                impFilter.IMP_TIME_FROM = mrs00382Filter.TIME_FROM;
                impFilter.IMP_TIME_TO = mrs00382Filter.TIME_TO;

                var listImpSub = new HisImpMestMaterialManager().GetView(impFilter);
                if (listImpSub != null)
                {
                    result.AddRange(listImpSub);
                }
            }
            result = result.Where(p => p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DMTL
                                                              || p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL
                                                              || p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL
                                                              || p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL
                                                              || p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL).ToList();
            return result;
        }

        private List<V_HIS_IMP_MEST_MEDICINE> GetImme(List<V_HIS_EXP_MEST_MEDICINE> _listMedicineAll, Mrs00382Filter _mrs00382Filter)
        {
            List<V_HIS_IMP_MEST_MEDICINE> result = new List<V_HIS_IMP_MEST_MEDICINE>();
            int skip = 0;
            var medicineIds = _listMedicineAll.Select(p => p.MEDICINE_TYPE_ID).Distinct().ToList();
            while (medicineIds.Count - skip > 0)
            {
                var listIds = medicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();

                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                HisImpMestMedicineViewFilterQuery impFilter = new HisImpMestMedicineViewFilterQuery();
                impFilter.MEDICINE_TYPE_IDs = listIds;
                impFilter.IMP_TIME_FROM = _mrs00382Filter.TIME_FROM;
                impFilter.IMP_TIME_TO = _mrs00382Filter.TIME_TO;

                var listImpSub = new HisImpMestMedicineManager().GetView(impFilter);
                if (listImpSub != null)
                {
                    result.AddRange(listImpSub);
                }
            }
            result = result.Where(p => p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DMTL
                                                              || p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL
                                                              || p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL
                                                              || p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL
                                                              || p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL).ToList();
            return result;


        }

        private List<HIS_EXP_MEST> GetAggr(List<V_HIS_EXP_MEST> _listExp)
        {
            List<HIS_EXP_MEST> result = new List<HIS_EXP_MEST>();
            var skip = 0;
            var aggrExpMestId = _listExp.Select(p => p.AGGR_EXP_MEST_ID ?? 0).Distinct().ToList();
            //thông tin phiếu tổng hợp của các phiếu xuất
            while (aggrExpMestId.Count - skip > 0)
            {
                var listIds = aggrExpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();

                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
                expMestFilter.IDs = listIds;
                var listExpMestSub = new HisExpMestManager().Get(expMestFilter);
                if (listExpMestSub != null)
                {
                    result.AddRange(listExpMestSub);
                }
            }
            return result;
        }

        private Dictionary<long, List<HIS_SERE_SERV>> GetSereServ(List<HIS_SERVICE_REQ> _listServiceReq)
        {
            Dictionary<long, List<HIS_SERE_SERV>> result = new Dictionary<long, List<HIS_SERE_SERV>>();
            List<HIS_SERE_SERV> ss = new List<HIS_SERE_SERV>();
            int skip = 0;
            var serviceReqID = _listServiceReq.Select(p => p.ID).Distinct().ToList();
            //thông tin sere_serv của các phiếu xuất
            while (serviceReqID.Count - skip > 0)
            {
                var listIds = serviceReqID.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();

                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                HisSereServFilterQuery ssFilter = new HisSereServFilterQuery();
                ssFilter.SERVICE_REQ_IDs = listIds;
                if ((ListTempType.Count == 0 || ListTempType.Contains("MediMate") || ListTempType.Contains("SereServ")))
                {
                    var listSereServSub = new HisSereServManager().Get(ssFilter);
                    if (listSereServSub != null)
                    {
                        ss.AddRange(listSereServSub);
                    }

                }

            }
            result = ss.GroupBy(o => o.SERVICE_REQ_ID ?? 0).ToDictionary(p => p.Key, q => q.ToList());
            return result;
        }

        private List<HIS_SERVICE_REQ> GetServiceReq(List<V_HIS_EXP_MEST> _listExp)
        {
            List<HIS_SERVICE_REQ> result = new List<HIS_SERVICE_REQ>();
            int skip = 0;
            var serviceReqID = _listExp.Select(p => p.SERVICE_REQ_ID ?? 0).Distinct().ToList();
            //thông tin sere_serv của các phiếu xuất
            while (serviceReqID.Count - skip > 0)
            {
                var listIds = serviceReqID.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();

                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                HisServiceReqFilterQuery srFilter = new HisServiceReqFilterQuery();
                srFilter.IDs = listIds;
                if (ListTempType.Count == 0 || !ListTempType.Contains("Report"))
                {
                    var listServiceReqSub = new HisServiceReqManager().Get(srFilter);
                    if (listServiceReqSub != null)
                    {
                        result.AddRange(listServiceReqSub);
                    }

                }

            }
            return result;
        }

        private List<HIS_TREATMENT> GetTreatment(List<V_HIS_EXP_MEST> _listExp, Mrs00382Filter mrs00382Filter)
        {
            List<HIS_TREATMENT> result = new List<HIS_TREATMENT>();
            var treatmentId = _listExp.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
            var skip = 0;
            //thông tin hồ sơ điều trị của các phiếu xuất
            while (treatmentId.Count - skip > 0)
            {
                var listIds = treatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();

                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                HisTreatmentFilterQuery treatFilter = new HisTreatmentFilterQuery();
                treatFilter.IDs = listIds;
                var listTreatmentSub = new HisTreatmentManager().Get(treatFilter);
                if (listTreatmentSub != null)
                {
                    result.AddRange(listTreatmentSub);
                }
            }
            return result;
        }

        private List<V_HIS_EXP_MEST_MEDICINE> GetExme(List<V_HIS_EXP_MEST> _listExp, Mrs00382Filter _mrs00382Filter, ref List<long> expIdHasDetail)
        {
            List<V_HIS_EXP_MEST_MEDICINE> result = new List<V_HIS_EXP_MEST_MEDICINE>();
            var expID = _listExp.Select(o => o.ID).Distinct().ToList();
            var skip = 0;
            while (expID.Count - skip > 0)
            {
                var listIds = expID.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();

                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                HisExpMestMedicineViewFilterQuery medicineFilter = new HisExpMestMedicineViewFilterQuery();
                medicineFilter.EXP_MEST_IDs = listIds;
                if (_mrs00382Filter.INPUT_DATA_ID_MEMA_TYPEs != null && !_mrs00382Filter.INPUT_DATA_ID_MEMA_TYPEs.Contains(1))
                {
                    medicineFilter.ID = 0;
                }
                if (_mrs00382Filter.SERVICE_TYPE_IDs != null && !_mrs00382Filter.SERVICE_TYPE_IDs.Contains(6))
                {
                    medicineFilter.ID = 0;
                }
                if (_mrs00382Filter.SERVICE_IDs != null)
                {
                    medicineFilter.SERVICE_IDs = _mrs00382Filter.SERVICE_IDs; //dịch vụ
                }
                //chi tiết thuốc đã xuất trong phiếu
                var listMedicineSub = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager().GetView(medicineFilter);

                if (_mrs00382Filter.MEDICINE_GROUP_IDs != null)
                {
                    listMedicineSub = listMedicineSub.Where(p => _mrs00382Filter.MEDICINE_GROUP_IDs.Contains(p.MEDICINE_GROUP_ID ?? 0)).ToList(); //nhóm thuốc
                }

                if (_mrs00382Filter.SS_PATIENT_TYPE_IDs != null)
                {
                    listMedicineSub = listMedicineSub.Where(p => _mrs00382Filter.SS_PATIENT_TYPE_IDs.Contains(p.PATIENT_TYPE_ID ?? 0)).ToList(); //đối tượng thanh toán
                }

                if (_mrs00382Filter.OTHER_PAY_SOURCE_IDs != null)
                {
                    listMedicineSub = listMedicineSub.Where(p => _mrs00382Filter.OTHER_PAY_SOURCE_IDs.Contains(p.OTHER_PAY_SOURCE_ID ?? 0)).ToList(); //nguồn chi trả
                }
                if (listMedicineSub != null)
                {
                    expIdHasDetail.AddRange(listMedicineSub.Select(o => o.EXP_MEST_ID ?? 0).Distinct().ToList());
                    result.AddRange(listMedicineSub);
                }
            }
            return result;
        }

        private List<V_HIS_EXP_MEST_MATERIAL> GetExma(List<V_HIS_EXP_MEST> _listExp, Mrs00382Filter _mrs00382Filter, ref List<long> expIdHasDetail)
        {
            List<V_HIS_EXP_MEST_MATERIAL> result = new List<V_HIS_EXP_MEST_MATERIAL>();
            var expID = _listExp.Select(o => o.ID).Distinct().ToList();
            var skip = 0;
            while (expID.Count - skip > 0)
            {
                var listIds = expID.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();

                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                HisExpMestMaterialViewFilterQuery materialFilter = new HisExpMestMaterialViewFilterQuery();
                materialFilter.EXP_MEST_IDs = listIds;

                if (_mrs00382Filter.INPUT_DATA_ID_MEMA_TYPEs != null && !_mrs00382Filter.INPUT_DATA_ID_MEMA_TYPEs.Contains(2))
                {
                    materialFilter.ID = 0;
                }
                if (_mrs00382Filter.SERVICE_TYPE_IDs != null && !_mrs00382Filter.SERVICE_TYPE_IDs.Contains(7))
                {
                    materialFilter.ID = 0;
                }
                if (_mrs00382Filter.SERVICE_IDs != null)
                {
                    materialFilter.SERVICE_IDs = _mrs00382Filter.SERVICE_IDs; //dịch vụ
                }
                if (_mrs00382Filter.CHEMICAL_SUBSTANCE_TYPE_IDs != null)
                {
                    materialFilter.MATERIAL_TYPE_IDs = _mrs00382Filter.CHEMICAL_SUBSTANCE_TYPE_IDs;
                }
                if (_mrs00382Filter.SERVICE_IDs != null)
                {
                    materialFilter.SERVICE_IDs = _mrs00382Filter.SERVICE_IDs; //dịch vụ
                }

                //chi tiết vật tư đã xuất trong phiếu xuất
                var listMaterialSub = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager().GetView(materialFilter);

                if (_mrs00382Filter.SS_PATIENT_TYPE_IDs != null)
                {
                    listMaterialSub = listMaterialSub.Where(p => _mrs00382Filter.SS_PATIENT_TYPE_IDs.Contains(p.PATIENT_TYPE_ID ?? 0)).ToList(); ///đối tượng thanh toán
                }

                if (_mrs00382Filter.OTHER_PAY_SOURCE_IDs != null)
                {
                    listMaterialSub = listMaterialSub.Where(p => _mrs00382Filter.OTHER_PAY_SOURCE_IDs.Contains(p.OTHER_PAY_SOURCE_ID ?? 0)).ToList(); //nguồn chi trả
                }
                if (listMaterialSub != null)
                {
                    expIdHasDetail.AddRange(listMaterialSub.Select(o => o.EXP_MEST_ID ?? 0).Distinct().ToList());
                    result.AddRange(listMaterialSub);
                }

            }
            return result;
        }

        private List<V_HIS_EXP_MEST> FilterOther(List<V_HIS_EXP_MEST> _listExp, Mrs00382Filter _mrs00382Filter)
        {
            List<V_HIS_EXP_MEST> result = _listExp;
            if (_mrs00382Filter.DOCTOR_LOGINNAMEs != null)
            {
                result = result.Where(p => _mrs00382Filter.DOCTOR_LOGINNAMEs.Contains(p.REQ_LOGINNAME)).ToList(); //bác sỹ kê đơn
            }
            if (!string.IsNullOrWhiteSpace(_mrs00382Filter.PATIENT_NAMEs))
            {
                result = result.Where(p => p.TDL_PATIENT_NAME.Contains(_mrs00382Filter.PATIENT_NAMEs)).ToList(); //bệnh nhân
            }
            if (_mrs00382Filter.PATIENT_TYPE_IDs != null)
            {
                result = result.Where(p => _mrs00382Filter.PATIENT_TYPE_IDs.Contains(p.TDL_PATIENT_TYPE_ID ?? 0)).ToList(); //đối tượng bệnh nhân
            }

            if (_mrs00382Filter.EXP_MEST_REASON_IDs != null)
            {
                result = result.Where(p => _mrs00382Filter.EXP_MEST_REASON_IDs.Contains(p.EXP_MEST_REASON_ID ?? 0)).ToList(); //đối tượng bệnh nhân
            }

            if (_mrs00382Filter.INPUT_DATA_ID_BSCS_TYPE != null)
            {
                if (_mrs00382Filter.INPUT_DATA_ID_BSCS_TYPE == (short)1) // thêm bổ sung cơ số
                {
                    result = result.Where(p => _mrs00382Filter.EXP_MEST_TYPE_IDs != null && _mrs00382Filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK) || p.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK || p.CHMS_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__ADDITION).ToList();
                }
                if (_mrs00382Filter.INPUT_DATA_ID_BSCS_TYPE == (short)2)// bỏ bổ sung cơ số
                {
                    result = result.Where(p => _mrs00382Filter.EXP_MEST_TYPE_IDs != null && _mrs00382Filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK) && p.CHMS_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__ADDITION || p.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK).ToList();
                }
                if (_mrs00382Filter.INPUT_DATA_ID_BSCS_TYPE == (short)3) //chỉ lấy bổ sung cơ số
                {
                    result = result.Where(p => p.CHMS_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__ADDITION).ToList();
                }
            }
            return result;
        }

        private List<V_HIS_EXP_MEST> GetExpMestCreate(Mrs00382Filter _mrs00382Filter)
        {
            List<V_HIS_EXP_MEST> result = new List<V_HIS_EXP_MEST>();
            HisExpMestViewFilterQuery expFilter = new HisExpMestViewFilterQuery();
            if (_mrs00382Filter.IMP_MEDI_STOCK_IDs != null) { expFilter.IMP_MEDI_STOCK_IDs = _mrs00382Filter.IMP_MEDI_STOCK_IDs; }
            if (_mrs00382Filter.MEDI_STOCK_ID != null) { expFilter.MEDI_STOCK_ID = _mrs00382Filter.MEDI_STOCK_ID; }
            if (_mrs00382Filter.EXP_MEDI_STOCK_IDs != null) { expFilter.MEDI_STOCK_IDs = _mrs00382Filter.EXP_MEDI_STOCK_IDs; }
            if (_mrs00382Filter.MEDI_STOCK_CABINET_IDs != null) { expFilter.MEDI_STOCK_IDs = _mrs00382Filter.MEDI_STOCK_CABINET_IDs; }
            if (_mrs00382Filter.DEPARTMENT_IDs != null) { expFilter.REQ_DEPARTMENT_IDs = _mrs00382Filter.DEPARTMENT_IDs; }
            if (_mrs00382Filter.EXAM_ROOM_IDs != null) { expFilter.REQ_ROOM_IDs = _mrs00382Filter.EXAM_ROOM_IDs; }
            if (_mrs00382Filter.REQ_ROOM_IDs != null) { expFilter.REQ_ROOM_IDs = _mrs00382Filter.REQ_ROOM_IDs; }
            expFilter.EXP_MEST_TYPE_IDs = _mrs00382Filter.EXP_MEST_TYPE_IDs ?? new List<long>()
                {
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP,
                };
            if (_mrs00382Filter.EXP_MEST_TYPE_ID != null)
            {
                expFilter.EXP_MEST_TYPE_IDs = new List<long>() { _mrs00382Filter.EXP_MEST_TYPE_ID ?? 0 };
            }
            if (_mrs00382Filter.INPUT_DATA_ID_BSCS_TYPE == 1 || _mrs00382Filter.INPUT_DATA_ID_BSCS_TYPE == 3)
            {
                expFilter.EXP_MEST_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK);
            }
            //expFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
            expFilter.CREATE_TIME_FROM = _mrs00382Filter.TIME_FROM;
            expFilter.CREATE_TIME_TO = _mrs00382Filter.TIME_TO;

            result = new HisExpMestManager().GetView(expFilter);
            return result;
        }

        private List<V_HIS_EXP_MEST> GetExpMestDone(Mrs00382Filter _mrs00382Filter)
        {
            List<V_HIS_EXP_MEST> result = new List<V_HIS_EXP_MEST>();
            HisExpMestViewFilterQuery expFilter = new HisExpMestViewFilterQuery();
            if (_mrs00382Filter.IMP_MEDI_STOCK_IDs != null) { expFilter.IMP_MEDI_STOCK_IDs = _mrs00382Filter.IMP_MEDI_STOCK_IDs; }
            if (_mrs00382Filter.MEDI_STOCK_ID != null) { expFilter.MEDI_STOCK_ID = _mrs00382Filter.MEDI_STOCK_ID; }
            if (_mrs00382Filter.EXP_MEDI_STOCK_IDs != null) { expFilter.MEDI_STOCK_IDs = _mrs00382Filter.EXP_MEDI_STOCK_IDs; }
            if (_mrs00382Filter.MEDI_STOCK_CABINET_IDs != null) { expFilter.MEDI_STOCK_IDs = _mrs00382Filter.MEDI_STOCK_CABINET_IDs; }
            if (_mrs00382Filter.DEPARTMENT_IDs != null) { expFilter.REQ_DEPARTMENT_IDs = _mrs00382Filter.DEPARTMENT_IDs; }
            if (_mrs00382Filter.EXAM_ROOM_IDs != null) { expFilter.REQ_ROOM_IDs = _mrs00382Filter.EXAM_ROOM_IDs; }
            expFilter.EXP_MEST_TYPE_IDs = _mrs00382Filter.EXP_MEST_TYPE_IDs ?? new List<long>()
                {
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP,
                };
            if (_mrs00382Filter.EXP_MEST_TYPE_ID != null)
            {
                expFilter.EXP_MEST_TYPE_IDs = new List<long>() { _mrs00382Filter.EXP_MEST_TYPE_ID ?? 0 };
            }
            if (_mrs00382Filter.INPUT_DATA_ID_BSCS_TYPE == 1 || _mrs00382Filter.INPUT_DATA_ID_BSCS_TYPE == 3)
            {
                expFilter.EXP_MEST_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK);
            }
            if (_mrs00382Filter.REQ_ROOM_IDs != null) { expFilter.REQ_ROOM_IDs = _mrs00382Filter.REQ_ROOM_IDs; }
            expFilter.FINISH_TIME_FROM = _mrs00382Filter.TIME_FROM;
            expFilter.FINISH_TIME_TO = _mrs00382Filter.TIME_TO;

            //các phiếu xuất hoàn thành trong thời gian yêu cầu
            result = new HisExpMestManager().GetView(expFilter);
            Inventec.Common.Logging.LogSystem.Info("Lay cac phieu xuat trong thoi gian bao cao done: " + result.Count);
            return result;
        }

        private void GetTempTypes()
        {
            try
            {
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_TEMP_TYPEs") && this.dicDataFilter["KEY_GROUP_TEMP_TYPEs"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_TEMP_TYPEs"].ToString()))
                {
                    var temptypes = this.dicDataFilter["KEY_GROUP_TEMP_TYPEs"].ToString();
                    if (!string.IsNullOrWhiteSpace(temptypes))
                    {
                        ListTempType = temptypes.Split(',').ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                listMaterialDpk = listMaterialAll.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK && o.CREATE_TIME >= mrs00382Filter.TIME_FROM && o.CREATE_TIME <= mrs00382Filter.TIME_TO).ToList();
                listMedicineDpk = listMedicineAll.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK && o.CREATE_TIME >= mrs00382Filter.TIME_FROM && o.CREATE_TIME <= mrs00382Filter.TIME_TO).ToList();
                listMaterial = listMaterialAll.Where(o => o.IS_EXPORT == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.EXP_TIME >= mrs00382Filter.TIME_FROM && o.EXP_TIME <= mrs00382Filter.TIME_TO).ToList();
                listMedicine = listMedicineAll.Where(o => o.IS_EXPORT == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.EXP_TIME >= mrs00382Filter.TIME_FROM && o.EXP_TIME <= mrs00382Filter.TIME_TO).ToList();

                //Xử lý theo đối tượng
                var listExpMedi = listMedicine.Where(p => p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK ||
                                                              p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT ||
                                                              p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT ||
                                                              p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP).ToList();

                var listExpMate = listMaterial.Where(p => p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK ||
                                                          p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT ||
                                                          p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT ||
                                                          p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP).ToList();

                //tach doi tuong bn
                ProcessPatientType(listExpMedi, listExpMate, listImpMestMedicine, listImpMestMaterial, listImp);

                //xử lý có gộp
                ProcessMediMate(listMaterial, listMedicine);

                //xử lý theo phiếu xuất
                ProcessExpMestType(listExp.Where(o => o.FINISH_TIME >= mrs00382Filter.TIME_FROM && o.FINISH_TIME <= mrs00382Filter.TIME_TO).ToList());

                //xử lý theo loại xuất của thuốc, vật tư
                ProcessExpType(listMedicine, listMaterial);

                //xử lý chi tiết
                ProcessExpMestCode(listMaterial, listMedicine, listMaterialDpk, listMedicineDpk);
                //this.LoadMobaImpMest(listExpMestId); //LAM SAU

                //xử lý gộp thuốc xuất ra theo tuổi của bệnh nhân
                var listCheck = listMedicine.Where(p => p.TDL_TREATMENT_ID != null).ToList();
                if (listCheck != null)
                {
                    ProcessExpMestMedicineByAge(listCheck);

                }

                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData("listForCabinet_03", listForCabinet));
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessMediMate(List<V_HIS_EXP_MEST_MATERIAL> listMaterial, List<V_HIS_EXP_MEST_MEDICINE> listMedicine)
        {
            //có gộp

            string keyGroupExp = "{0}_{1}";
            if (this.dicDataFilter.ContainsKey("KEY_GROUP_EXP") && this.dicDataFilter["KEY_GROUP_EXP"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_EXP"].ToString()))
            {
                keyGroupExp = this.dicDataFilter["KEY_GROUP_EXP"].ToString();
            }

            //gộp thuốc và vật tư theo lô, đơn giá
            if (IsNotNullOrEmpty(listMedicine) && (ListTempType.Count == 0 || ListTempType.Contains("Report")))
            {
                var rsGroup = listMedicine.GroupBy(p => string.Format(keyGroupExp, p.MEDICINE_ID, (p.PRICE ?? 0) * (1 + (p.VAT_RATIO ?? 0)), p.MEDICINE_TYPE_ID, p.IMP_PRICE * (1 + p.IMP_VAT_RATIO), p.PATIENT_TYPE_ID, dicExp.ContainsKey(p.EXP_MEST_ID ?? 0) ? dicExp[p.EXP_MEST_ID ?? 0].EXP_MEST_REASON_ID : 0, dicExp.ContainsKey(p.EXP_MEST_ID ?? 0) ? dicExp[p.EXP_MEST_ID ?? 0].TDL_PATIENT_TYPE_ID : 0, p.EXP_MEST_ID)).ToList();
                foreach (var itemGr in rsGroup)
                {
                    Mrs00382RDO ado = new Mrs00382RDO();
                    //if (ado.AMOUT == null)
                    //    ado.AMOUT = 0; 
                    Map(itemGr.First(), ado);
                    ado.AMOUNT = itemGr.Sum(p => p.AMOUNT);
                    var expType = listExpType.FirstOrDefault(p => p.ID == itemGr.FirstOrDefault().EXP_MEST_TYPE_ID);
                    if (expType != null)
                    {
                        ado.EXP_MEST_TYPE_CODE = expType.EXP_MEST_TYPE_CODE;
                        ado.EXP_MEST_TYPE_NAME = expType.EXP_MEST_TYPE_NAME;
                    }

                    //thêm thông tin lý do xuất và đối tượng bệnh nhân
                    if (dicExp.ContainsKey(itemGr.First().EXP_MEST_ID ?? 0))
                    {
                        ado.TDL_PATIENT_TYPE_ID = dicExp[itemGr.First().EXP_MEST_ID ?? 0].TDL_PATIENT_TYPE_ID;
                        ado.TDL_PATIENT_TYPE_NAME = dicExp[itemGr.First().EXP_MEST_ID ?? 0].PATIENT_TYPE_NAME;
                        ado.TDL_EXP_MEST_REASON_ID = dicExp[itemGr.First().EXP_MEST_ID ?? 0].EXP_MEST_REASON_ID;
                        ado.TDL_EXP_MEST_REASON_NAME = dicExp[itemGr.First().EXP_MEST_ID ?? 0].EXP_MEST_REASON_NAME;
                        ado.TDL_EXP_MEST_REASON_CODE = dicExp[itemGr.First().EXP_MEST_ID ?? 0].EXP_MEST_REASON_CODE;
                        ado.IMP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == dicExp[itemGr.First().EXP_MEST_ID ?? 0].IMP_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                        ado.IMP_MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == dicExp[itemGr.First().EXP_MEST_ID ?? 0].IMP_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                    }

                    if (ado.EXP_PRICE == null)
                        ado.EXP_PRICE = 0;
                    ado.EXP_PRICE = (itemGr.FirstOrDefault().PRICE ?? 0) * (1 + (itemGr.FirstOrDefault().VAT_RATIO ?? 0));
                    //if (ado.IMP_PRICE == null)
                    //    ado.IMP_PRICE = 0; 
                    ado.IMP_PRICE = (itemGr.FirstOrDefault().IMP_PRICE) * (1 + (itemGr.FirstOrDefault().IMP_VAT_RATIO));
                    ado.MEDI_MATE_TYPE_CODE = itemGr.FirstOrDefault().MEDICINE_TYPE_CODE;
                    ado.MEDI_MATE_TYPE_NAME = itemGr.FirstOrDefault().MEDICINE_TYPE_NAME;
                    ado.CONCENTRA = itemGr.FirstOrDefault().CONCENTRA;
                    ado.PACKAGE_NUMBER = itemGr.FirstOrDefault().PACKAGE_NUMBER;
                    ado.SERVICE_UNIT_NAME = itemGr.FirstOrDefault().SERVICE_UNIT_NAME;
                    ado.ACTIVE_INGR_BHYT_NAME = itemGr.FirstOrDefault().ACTIVE_INGR_BHYT_NAME;
                    ado.NATIONAL_NAME = itemGr.FirstOrDefault().NATIONAL_NAME;
                    ado.MANUFACTURER_NAME = itemGr.FirstOrDefault().MANUFACTURER_NAME;
                    ado.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(itemGr.FirstOrDefault().EXP_TIME ?? 0);
                    ado.EXP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == itemGr.FirstOrDefault().MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                    ado.EXP_MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == itemGr.FirstOrDefault().MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                    //if (ado.TOTAL_PRICE == null)
                    //    ado.TOTAL_PRICE = 0; 
                    ado.TOTAL_PRICE = itemGr.Sum(p => p.AMOUNT * (itemGr.FirstOrDefault().PRICE ?? 0) * (1 + (itemGr.FirstOrDefault().VAT_RATIO ?? 0)));
                    ado.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(itemGr.FirstOrDefault().EXPIRED_DATE ?? 0);
                    //var impPrice = Medicine.Where(x => x.ID == itemGr.First().MEDICINE_ID).FirstOrDefault();
                    //if (impPrice != null)
                    {
                        ado.PRICE_MEDI_MATE = (itemGr.FirstOrDefault().IMP_PRICE) * (1 + (itemGr.FirstOrDefault().IMP_VAT_RATIO));
                    }

                    listRdo.Add(ado);
                }


            }
            if (IsNotNullOrEmpty(listMaterial) && (ListTempType.Count == 0 || ListTempType.Contains("Report")))
            {
                var rsGroup = listMaterial.GroupBy(p => string.Format(keyGroupExp, p.MATERIAL_ID, (p.PRICE ?? 0) * (1 + (p.VAT_RATIO ?? 0)), p.MATERIAL_TYPE_ID, p.IMP_PRICE * (1 + p.IMP_VAT_RATIO), p.PATIENT_TYPE_ID, dicExp.ContainsKey(p.EXP_MEST_ID ?? 0) ? dicExp[p.EXP_MEST_ID ?? 0].EXP_MEST_REASON_ID : 0, dicExp.ContainsKey(p.EXP_MEST_ID ?? 0) ? dicExp[p.EXP_MEST_ID ?? 0].TDL_PATIENT_TYPE_ID : 0, p.EXP_MEST_ID)).ToList();
                foreach (var itemGr in rsGroup)
                {
                    Mrs00382RDO ado = new Mrs00382RDO();
                    //if (ado.AMOUT == null)
                    //    ado.AMOUT = 0; 
                    Map(itemGr.First(), ado);
                    var expType = listExpType.FirstOrDefault(p => p.ID == itemGr.FirstOrDefault().EXP_MEST_TYPE_ID);
                    if (expType != null)
                    {
                        ado.EXP_MEST_TYPE_CODE = expType.EXP_MEST_TYPE_CODE;
                        ado.EXP_MEST_TYPE_NAME = expType.EXP_MEST_TYPE_NAME;
                    }

                    //thêm thông tin lý do xuất và đối tượng bệnh nhân
                    if (dicExp.ContainsKey(itemGr.First().EXP_MEST_ID ?? 0))
                    {
                        ado.TDL_PATIENT_TYPE_ID = dicExp[itemGr.First().EXP_MEST_ID ?? 0].TDL_PATIENT_TYPE_ID;
                        ado.TDL_PATIENT_TYPE_NAME = dicExp[itemGr.First().EXP_MEST_ID ?? 0].PATIENT_TYPE_NAME;
                        ado.TDL_EXP_MEST_REASON_ID = dicExp[itemGr.First().EXP_MEST_ID ?? 0].EXP_MEST_REASON_ID;
                        ado.TDL_EXP_MEST_REASON_NAME = dicExp[itemGr.First().EXP_MEST_ID ?? 0].EXP_MEST_REASON_NAME;
                        ado.TDL_EXP_MEST_REASON_CODE = dicExp[itemGr.First().EXP_MEST_ID ?? 0].EXP_MEST_REASON_CODE;
                        ado.IMP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == dicExp[itemGr.First().EXP_MEST_ID ?? 0].IMP_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                        ado.IMP_MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == dicExp[itemGr.First().EXP_MEST_ID ?? 0].IMP_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                    }

                    ado.AMOUNT = itemGr.Sum(p => p.AMOUNT);
                    if (ado.EXP_PRICE == null)
                        ado.EXP_PRICE = 0;
                    ado.EXP_PRICE = (itemGr.FirstOrDefault().PRICE ?? 0) * (1 + (itemGr.FirstOrDefault().VAT_RATIO ?? 0));
                    //if (ado.IMP_PRICE == null)
                    //    ado.IMP_PRICE = 0; 
                    ado.IMP_PRICE = (itemGr.FirstOrDefault().IMP_PRICE) * (1 + (itemGr.FirstOrDefault().IMP_VAT_RATIO));
                    ado.MEDI_MATE_TYPE_CODE = itemGr.FirstOrDefault().MATERIAL_TYPE_CODE;
                    ado.MEDI_MATE_TYPE_NAME = itemGr.FirstOrDefault().MATERIAL_TYPE_NAME;
                    ado.CONCENTRA = "";
                    ado.PACKAGE_NUMBER = itemGr.FirstOrDefault().PACKAGE_NUMBER;
                    ado.SERVICE_UNIT_NAME = itemGr.FirstOrDefault().SERVICE_UNIT_NAME;
                    ado.NATIONAL_NAME = itemGr.FirstOrDefault().NATIONAL_NAME;
                    ado.MANUFACTURER_NAME = itemGr.FirstOrDefault().MANUFACTURER_NAME;
                    ado.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(itemGr.FirstOrDefault().EXP_TIME ?? 0);
                    ado.SERVICE_UNIT_NAME = itemGr.FirstOrDefault().SERVICE_UNIT_NAME;
                    ado.EXP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == itemGr.FirstOrDefault().MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                    ado.EXP_MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == itemGr.FirstOrDefault().MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;

                    //if (ado.TOTAL_PRICE == null)
                    //    ado.TOTAL_PRICE = 0; 
                    ado.TOTAL_PRICE = itemGr.Sum(p => p.AMOUNT * (p.PRICE ?? 0) * (1 + (p.VAT_RATIO ?? 0)));
                    ado.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(itemGr.FirstOrDefault().EXPIRED_DATE ?? 0);
                    //var impPrice = Material.Where(x => x.ID == itemGr.First().MATERIAL_ID).FirstOrDefault();
                    //if (impPrice != null)
                    {
                        ado.PRICE_MEDI_MATE = (itemGr.FirstOrDefault().IMP_PRICE) * (1 + (itemGr.FirstOrDefault().IMP_VAT_RATIO));
                    }

                    //trạng thái tái sử dụng
                    if (listIsReusabling != null)
                    {
                        var IsReusabling = listIsReusabling.FirstOrDefault(o => o.MATERIAL_ID == itemGr.FirstOrDefault().MATERIAL_ID && o.IS_REUSABLING >= 1);
                        if (IsReusabling != null)
                        {
                            ado.TOTAL_REUSABLE_EXP = itemGr.Sum(p => p.AMOUNT * (p.PRICE ?? 0) * (1 + (p.VAT_RATIO ?? 0)));
                        }
                    }
                    listRdo.Add(ado);
                }
            }

            //gộp đơn phòng khám theo lô, đơn giá và loại xuất
            if (IsNotNullOrEmpty(listMedicineDpk) && (ListTempType.Count == 0 || ListTempType.Contains("Detail2")))
            {
                var rsGroup1 = listMedicineDpk.GroupBy(p => string.Format("{0}_{1}_{2}", p.MEDICINE_ID, (p.PRICE ?? 0) * (1 + (p.VAT_RATIO ?? 0)), p.EXP_MEST_TYPE_ID)).ToList();
                foreach (var itemGr1 in rsGroup1)
                {
                    Mrs00382RDO ado = new Mrs00382RDO();
                    //if (ado.AMOUT == null)
                    //    ado.AMOUT = 0; 
                    ado.AMOUNT = itemGr1.Sum(p => p.AMOUNT);
                    if (ado.EXP_PRICE == null)
                        ado.EXP_PRICE = 0;
                    ado.EXP_PRICE = (itemGr1.FirstOrDefault().PRICE ?? 0) * (1 + (itemGr1.FirstOrDefault().VAT_RATIO ?? 0));
                    ado.EXP_MEST_TYPE_ID = itemGr1.FirstOrDefault().EXP_MEST_TYPE_ID;
                    //if (ado.IMP_PRICE == null)
                    //    ado.IMP_PRICE = 0; 
                    ado.IMP_PRICE = (itemGr1.FirstOrDefault().IMP_PRICE) * (1 + (itemGr1.FirstOrDefault().IMP_VAT_RATIO));
                    ado.MEDI_MATE_TYPE_CODE = itemGr1.FirstOrDefault().MEDICINE_TYPE_CODE;
                    ado.MEDI_MATE_TYPE_NAME = itemGr1.FirstOrDefault().MEDICINE_TYPE_NAME;
                    ado.PACKAGE_NUMBER = itemGr1.FirstOrDefault().PACKAGE_NUMBER;
                    ado.SERVICE_UNIT_NAME = itemGr1.FirstOrDefault().SERVICE_UNIT_NAME;
                    ado.ACTIVE_INGR_BHYT_NAME = itemGr1.FirstOrDefault().ACTIVE_INGR_BHYT_NAME;
                    //if (ado.TOTAL_PRICE == null)
                    //    ado.TOTAL_PRICE = 0; 
                    ado.TOTAL_PRICE = ado.AMOUNT * (itemGr1.FirstOrDefault().PRICE ?? 0) * (1 + (itemGr1.FirstOrDefault().VAT_RATIO ?? 0));
                    ado.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(itemGr1.FirstOrDefault().EXPIRED_DATE ?? 0);
                    var expMestIds = itemGr1.Select(p => p.EXP_MEST_ID).ToList();
                    //listExpMestId.AddRange(expMestIds); 
                    //var impPrice = Medicine.Where(x => x.ID == itemGr1.First().MEDICINE_ID).FirstOrDefault();
                    //if (impPrice != null)
                    {
                        ado.PRICE_MEDI_MATE = (itemGr1.FirstOrDefault().IMP_PRICE) * (1 + (itemGr1.FirstOrDefault().IMP_VAT_RATIO));
                    }
                    listRdoDpkGroupEMT.Add(ado);
                }
            }
            if (IsNotNullOrEmpty(listMaterialDpk) && (ListTempType.Count == 0 || ListTempType.Contains("Detail2")))
            {
                var rsGroup1 = listMaterialDpk.GroupBy(p => string.Format("{0}_{1}_{2}", p.MATERIAL_ID, (p.PRICE ?? 0) * (1 + (p.VAT_RATIO ?? 0)), p.EXP_MEST_TYPE_ID)).ToList();
                foreach (var itemGr1 in rsGroup1)
                {
                    Mrs00382RDO ado = new Mrs00382RDO();
                    //if (ado.AMOUT == null)
                    //    ado.AMOUT = 0; 

                    ado.AMOUNT = itemGr1.Sum(p => p.AMOUNT);
                    if (ado.EXP_PRICE == null)
                        ado.EXP_PRICE = 0;
                    ado.EXP_PRICE = (itemGr1.FirstOrDefault().PRICE ?? 0) * (1 + (itemGr1.FirstOrDefault().VAT_RATIO ?? 0));
                    //if (ado.IMP_PRICE == null)
                    //    ado.IMP_PRICE = 0; 
                    ado.IMP_PRICE = (itemGr1.FirstOrDefault().IMP_PRICE) * (1 + (itemGr1.FirstOrDefault().IMP_VAT_RATIO));
                    ado.EXP_MEST_TYPE_ID = itemGr1.FirstOrDefault().EXP_MEST_TYPE_ID;
                    ado.MEDI_MATE_TYPE_CODE = itemGr1.FirstOrDefault().MATERIAL_TYPE_CODE;
                    ado.MEDI_MATE_TYPE_NAME = itemGr1.FirstOrDefault().MATERIAL_TYPE_NAME;
                    ado.PACKAGE_NUMBER = itemGr1.FirstOrDefault().PACKAGE_NUMBER;
                    ado.SERVICE_UNIT_NAME = itemGr1.FirstOrDefault().SERVICE_UNIT_NAME;
                    ado.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(itemGr1.FirstOrDefault().EXPIRED_DATE ?? 0);
                    //if (ado.TOTAL_PRICE == null)
                    //    ado.TOTAL_PRICE = 0; 
                    ado.TOTAL_PRICE = ado.AMOUNT * (itemGr1.FirstOrDefault().PRICE ?? 0) * (1 + (itemGr1.FirstOrDefault().VAT_RATIO ?? 0));
                    var expMestIds = itemGr1.Select(p => p.EXP_MEST_ID).ToList();
                    //var impPrice = Material.Where(x => x.ID == itemGr1.First().MATERIAL_ID).FirstOrDefault();
                    //if (impPrice != null)
                    {
                        ado.PRICE_MEDI_MATE = (itemGr1.FirstOrDefault().IMP_PRICE) * (1 + (itemGr1.FirstOrDefault().IMP_VAT_RATIO));
                    }
                    //listExpMestId.AddRange(expMestIds); 

                    //trạng thái tái sử dụng
                    if (listIsReusabling != null)
                    {
                        var IsReusabling = listIsReusabling.FirstOrDefault(o => o.MATERIAL_ID == itemGr1.FirstOrDefault().MATERIAL_ID && o.IS_REUSABLING >= 1);
                        if (IsReusabling != null)
                        {
                            ado.TOTAL_REUSABLE_EXP = itemGr1.Sum(p => p.AMOUNT * (p.PRICE ?? 0) * (1 + (p.VAT_RATIO ?? 0)));
                        }
                    }
                    listRdoDpkGroupEMT.Add(ado);
                }
            }
        }

        private void Map(V_HIS_EXP_MEST_MEDICINE item, Mrs00382RDO ado)
        {
            ado.ACTIVE_INGR_BHYT_CODE = item.ACTIVE_INGR_BHYT_CODE;
            ado.ACTIVE_INGR_BHYT_NAME = item.ACTIVE_INGR_BHYT_NAME;
            ado.AFTERNOON = item.AFTERNOON;
            ado.AGGR_EXP_MEST_ID = item.AGGR_EXP_MEST_ID;
            ado.AMOUNT = item.AMOUNT;
            ado.APPROVAL_DATE = item.APPROVAL_DATE;
            ado.APPROVAL_LOGINNAME = item.APPROVAL_LOGINNAME;
            ado.APPROVAL_TIME = item.APPROVAL_TIME;
            ado.APPROVAL_USERNAME = item.APPROVAL_USERNAME;
            ado.BCS_REQ_AMOUNT = item.BCS_REQ_AMOUNT;
            ado.BID_ID = item.BID_ID;
            ado.BID_NAME = item.BID_NAME;
            ado.BID_NUMBER = item.BID_NUMBER;
            ado.BK_AMOUNT = item.BK_AMOUNT;
            ado.BREATH_SPEED = item.BREATH_SPEED;
            ado.BREATH_TIME = item.BREATH_TIME;
            ado.BYT_NUM_ORDER = item.BYT_NUM_ORDER;
            ado.CK_IMP_MEST_MEDICINE_ID = item.CK_IMP_MEST_MEDICINE_ID;
            ado.CONCENTRA = item.CONCENTRA;
            ado.CONVERT_RATIO = item.CONVERT_RATIO;
            ado.CONVERT_UNIT_CODE = item.CONVERT_UNIT_CODE;
            ado.CONVERT_UNIT_NAME = item.CONVERT_UNIT_NAME;
            ado.CREATE_TIME = item.CREATE_TIME;
            ado.CREATOR = item.CREATOR;
            ado.DAY_COUNT = item.DAY_COUNT;
            ado.DESCRIPTION = item.DESCRIPTION;
            ado.DISCOUNT = item.DISCOUNT;
            ado.EVENING = item.EVENING;
            ado.EXP_DATE = item.EXP_DATE;
            ado.EXP_LOGINNAME = item.EXP_LOGINNAME;
            ado.EXP_MEST_CODE = item.EXP_MEST_CODE;
            ado.EXP_MEST_ID = item.EXP_MEST_ID;
            ado.EXP_MEST_METY_REQ_ID = item.EXP_MEST_METY_REQ_ID;
            ado.EXP_MEST_STT_ID = item.EXP_MEST_STT_ID;
            ado.EXP_MEST_TYPE_ID = item.EXP_MEST_TYPE_ID;

            ado.EXP_TIME = item.EXP_TIME;
            ado.EXP_USERNAME = item.EXP_USERNAME;
            ado.EXPEND_TYPE_ID = item.EXPEND_TYPE_ID;
            ado.EXPIRED_DATE = item.EXPIRED_DATE;
            ado.GROUP_CODE = item.GROUP_CODE;
            ado.HTU_ID = item.HTU_ID;
            ado.HTU_NAME = item.HTU_NAME;
            ado.ID = item.ID;
            ado.IMP_PRICE = item.IMP_PRICE;
            ado.IMP_TIME = item.IMP_TIME;
            ado.IMP_VAT_RATIO = item.IMP_VAT_RATIO;
            ado.INTERNAL_PRICE = item.INTERNAL_PRICE;
            ado.IS_ACTIVE = item.IS_ACTIVE;
            ado.IS_ALLOW_ODD = item.IS_ALLOW_ODD;
            ado.IS_CREATED_BY_APPROVAL = item.IS_CREATED_BY_APPROVAL;
            ado.IS_DELETE = item.IS_DELETE;
            ado.IS_EXPEND = item.IS_EXPEND;
            ado.IS_EXPORT = item.IS_EXPORT;
            ado.IS_FUNCTIONAL_FOOD = item.IS_FUNCTIONAL_FOOD;
            ado.IS_NOT_PRES = item.IS_NOT_PRES;
            ado.IS_OUT_HOSPITAL = item.IS_OUT_HOSPITAL;
            ado.IS_OUT_PARENT_FEE = item.IS_OUT_PARENT_FEE;
            ado.IS_USE_CLIENT_PRICE = item.IS_USE_CLIENT_PRICE;
            ado.IS_USED = item.IS_USED;
            ado.MANUFACTURER_CODE = item.MANUFACTURER_CODE;
            ado.MANUFACTURER_ID = item.MANUFACTURER_ID;
            ado.MANUFACTURER_NAME = item.MANUFACTURER_NAME;
            ado.MATERIAL_NUM_ORDER = item.MATERIAL_NUM_ORDER;
            ado.MEDI_STOCK_ID = item.MEDI_STOCK_ID;
            ado.MEDI_STOCK_PERIOD_ID = item.MEDI_STOCK_PERIOD_ID;
            ado.MEDICINE_BYT_NUM_ORDER = item.MEDICINE_BYT_NUM_ORDER;
            ado.MEDICINE_GROUP_CODE = item.MEDICINE_GROUP_CODE;
            ado.MEDICINE_GROUP_ID = item.MEDICINE_GROUP_ID;
            ado.MEDICINE_GROUP_NAME = item.MEDICINE_GROUP_NAME;
            ado.MEDICINE_GROUP_NUM_ORDER = item.MEDICINE_GROUP_NUM_ORDER;
            ado.MEDICINE_ID = item.MEDICINE_ID;
            ado.MEDICINE_NUM_ORDER = item.MEDICINE_NUM_ORDER;
            ado.MEDICINE_REGISTER_NUMBER = item.MEDICINE_REGISTER_NUMBER;
            ado.MEDICINE_TCY_NUM_ORDER = item.MEDICINE_TCY_NUM_ORDER;
            ado.MEDICINE_TYPE_CODE = item.MEDICINE_TYPE_CODE;
            ado.MEDICINE_TYPE_ID = item.MEDICINE_TYPE_ID;
            ado.MEDICINE_TYPE_NAME = item.MEDICINE_TYPE_NAME;
            ado.MEDICINE_TYPE_NUM_ORDER = item.MEDICINE_TYPE_NUM_ORDER;
            ado.MEDICINE_USE_FORM_CODE = item.MEDICINE_USE_FORM_CODE;
            ado.MEDICINE_USE_FORM_ID = item.MEDICINE_USE_FORM_ID;
            ado.MEDICINE_USE_FORM_NAME = item.MEDICINE_USE_FORM_NAME;
            ado.MEDICINE_USE_FORM_NUM_ORDER = item.MEDICINE_USE_FORM_NUM_ORDER;
            ado.MEMA_GROUP_ID = item.MEMA_GROUP_ID;
            ado.MODIFIER = item.MODIFIER;
            ado.MODIFY_TIME = item.MODIFY_TIME;
            ado.MORNING = item.MORNING;
            ado.NATIONAL_NAME = item.NATIONAL_NAME;
            ado.NOON = item.NOON;
            ado.NUM_ORDER = item.NUM_ORDER;
            ado.OTHER_PAY_SOURCE_ID = item.OTHER_PAY_SOURCE_ID;
            ado.OTHER_PAY_SOURCE_CODE = item.OTHER_PAY_SOURCE_CODE;
            ado.OTHER_PAY_SOURCE_NAME = item.OTHER_PAY_SOURCE_NAME;
            ado.PACKAGE_NUMBER = item.PACKAGE_NUMBER;
            ado.PATIENT_TYPE_CODE = item.PATIENT_TYPE_CODE;
            ado.PATIENT_TYPE_ID = item.PATIENT_TYPE_ID;
            ado.PATIENT_TYPE_NAME = item.PATIENT_TYPE_NAME;
            ado.PREVIOUS_USING_COUNT = item.PREVIOUS_USING_COUNT;
            ado.PRICE = item.PRICE ?? 0;
            ado.RECORDING_TRANSACTION = item.RECORDING_TRANSACTION;
            ado.REGISTER_NUMBER = item.REGISTER_NUMBER;
            ado.REQ_DEPARTMENT_ID = item.REQ_DEPARTMENT_ID;
            ado.REQ_ROOM_ID = item.REQ_ROOM_ID;
            ado.REQ_USER_TITLE = item.REQ_USER_TITLE;
            ado.SERE_SERV_PARENT_ID = item.SERE_SERV_PARENT_ID;
            ado.SERVICE_CONDITION_ID = item.SERVICE_CONDITION_ID;
            ado.SERVICE_ID = item.SERVICE_ID;
            ado.SERVICE_UNIT_CODE = item.SERVICE_UNIT_CODE;
            ado.SERVICE_UNIT_ID = item.SERVICE_UNIT_ID;
            ado.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
            ado.SPEED = item.SPEED;
            ado.SUPPLIER_CODE = item.SUPPLIER_CODE;
            ado.SUPPLIER_ID = item.SUPPLIER_ID;
            ado.SUPPLIER_NAME = item.SUPPLIER_NAME;
            //ado.TAX_RATIO = item.TAX_RATIO;
            ado.TCY_NUM_ORDER = item.TCY_NUM_ORDER;
            ado.TDL_AGGR_EXP_MEST_ID = item.TDL_AGGR_EXP_MEST_ID;
            ado.TDL_INTRUCTION_TIME = item.TDL_INTRUCTION_TIME;
            ado.TDL_MEDI_STOCK_ID = item.TDL_MEDI_STOCK_ID;
            ado.TDL_MEDICINE_TYPE_ID = item.TDL_MEDICINE_TYPE_ID;
            ado.TDL_PRES_REQ_USER_TITLE = item.TDL_PRES_REQ_USER_TITLE;
            ado.TDL_SERVICE_REQ_ID = item.TDL_SERVICE_REQ_ID;
            ado.TDL_TREATMENT_ID = item.TDL_TREATMENT_ID;
            ado.TDL_VACCINATION_ID = item.TDL_VACCINATION_ID;
            ado.TH_AMOUNT = item.TH_AMOUNT;
            ado.TUTORIAL = item.TUTORIAL;
            ado.USE_ORIGINAL_UNIT_FOR_PRES = item.USE_ORIGINAL_UNIT_FOR_PRES;
            ado.USE_TIME_TO = item.USE_TIME_TO;
            ado.VACCINATION_RESULT_ID = item.VACCINATION_RESULT_ID;
            ado.VAT_RATIO = item.VAT_RATIO ?? 0;
            ado.VIR_PRICE = item.VIR_PRICE;
            //var impPrice = Medicine.Where(x => x.ID == item.MEDICINE_ID).FirstOrDefault();
            //       if (impPrice != null)
            {
                ado.PRICE_MEDI_MATE = item.IMP_PRICE * (1 + (item.IMP_VAT_RATIO));
            }

        }

        private void Map(V_HIS_EXP_MEST_MATERIAL item, Mrs00382RDO ado)
        {
            //ado.ACTIVE_INGR_BHYT_CODE = item.ACTIVE_INGR_BHYT_CODE;
            //ado.ACTIVE_INGR_BHYT_NAME = item.ACTIVE_INGR_BHYT_NAME;
            //ado.AFTERNOON = item.AFTERNOON;
            ado.AGGR_EXP_MEST_ID = item.AGGR_EXP_MEST_ID;
            ado.AMOUNT = item.AMOUNT;
            ado.APPROVAL_DATE = item.APPROVAL_DATE;
            ado.APPROVAL_LOGINNAME = item.APPROVAL_LOGINNAME;
            ado.APPROVAL_TIME = item.APPROVAL_TIME;
            ado.APPROVAL_USERNAME = item.APPROVAL_USERNAME;
            ado.BCS_REQ_AMOUNT = item.BCS_REQ_AMOUNT;
            ado.BID_ID = item.BID_ID;
            ado.BID_NAME = item.BID_NAME;
            ado.BID_NUMBER = item.BID_NUMBER;
            ado.BK_AMOUNT = item.BK_AMOUNT;
            //ado.BREATH_SPEED = item.BREATH_SPEED;
            //ado.BREATH_TIME = item.BREATH_TIME;
            //ado.BYT_NUM_ORDER = item.BYT_NUM_ORDER;
            ado.CK_IMP_MEST_MEDICINE_ID = item.CK_IMP_MEST_MATERIAL_ID;
            //ado.CONCENTRA = item.CONCENTRA;
            ado.CONVERT_RATIO = item.CONVERT_RATIO;
            ado.CONVERT_UNIT_CODE = item.CONVERT_UNIT_CODE;
            ado.CONVERT_UNIT_NAME = item.CONVERT_UNIT_NAME;
            ado.CREATE_TIME = item.CREATE_TIME;
            ado.CREATOR = item.CREATOR;
            //ado.DAY_COUNT = item.DAY_COUNT;
            ado.DESCRIPTION = item.DESCRIPTION;
            ado.DISCOUNT = item.DISCOUNT;
            //ado.EVENING = item.EVENING;
            ado.EXP_DATE = item.EXP_DATE;
            ado.EXP_LOGINNAME = item.EXP_LOGINNAME;
            ado.EXP_MEST_CODE = item.EXP_MEST_CODE;
            ado.EXP_MEST_ID = item.EXP_MEST_ID;
            //ado.EXP_MEST_METY_REQ_ID = item.EXP_MEST_METY_REQ_ID;
            ado.EXP_MEST_STT_ID = item.EXP_MEST_STT_ID;
            ado.EXP_MEST_TYPE_ID = item.EXP_MEST_TYPE_ID;
            ado.EXP_TIME = item.EXP_TIME;
            ado.EXP_USERNAME = item.EXP_USERNAME;
            ado.EXPEND_TYPE_ID = item.EXPEND_TYPE_ID;
            ado.EXPIRED_DATE = item.EXPIRED_DATE;
            ado.GROUP_CODE = item.GROUP_CODE;
            //ado.HTU_ID = item.HTU_ID;
            //ado.HTU_NAME = item.HTU_NAME;
            ado.ID = item.ID;
            ado.IMP_PRICE = item.IMP_PRICE;
            ado.IMP_TIME = item.IMP_TIME;
            ado.IMP_VAT_RATIO = item.IMP_VAT_RATIO;
            ado.INTERNAL_PRICE = item.INTERNAL_PRICE;
            ado.IS_ACTIVE = item.IS_ACTIVE;
            //ado.IS_ALLOW_ODD = item.IS_ALLOW_ODD;
            ado.IS_CREATED_BY_APPROVAL = item.IS_CREATED_BY_APPROVAL;
            ado.IS_DELETE = item.IS_DELETE;
            ado.IS_EXPEND = item.IS_EXPEND;
            ado.IS_EXPORT = item.IS_EXPORT;
            //ado.IS_FUNCTIONAL_FOOD = item.IS_FUNCTIONAL_FOOD;
            ado.IS_NOT_PRES = item.IS_NOT_PRES;
            ado.IS_OUT_HOSPITAL = item.IS_OUT_HOSPITAL;
            ado.IS_OUT_PARENT_FEE = item.IS_OUT_PARENT_FEE;
            ado.IS_USE_CLIENT_PRICE = item.IS_USE_CLIENT_PRICE;
            ado.IS_USED = item.IS_USED;
            ado.MANUFACTURER_CODE = item.MANUFACTURER_CODE;
            ado.MANUFACTURER_ID = item.MANUFACTURER_ID;
            ado.MANUFACTURER_NAME = item.MANUFACTURER_NAME;
            ado.MATERIAL_NUM_ORDER = item.MATERIAL_NUM_ORDER;
            ado.MEDI_STOCK_ID = item.MEDI_STOCK_ID;
            ado.MEDI_STOCK_PERIOD_ID = item.MEDI_STOCK_PERIOD_ID;
            //ado.MEDICINE_BYT_NUM_ORDER = item.MEDICINE_BYT_NUM_ORDER;
            //ado.MEDICINE_GROUP_CODE = item.MEDICINE_GROUP_CODE;
            //ado.MEDICINE_GROUP_ID = item.MEDICINE_GROUP_ID;
            //ado.MEDICINE_GROUP_NAME = item.MEDICINE_GROUP_NAME;
            //ado.MEDICINE_GROUP_NUM_ORDER = item.MEDICINE_GROUP_NUM_ORDER;
            ado.MEDICINE_ID = item.MATERIAL_ID;
            ado.MEDICINE_NUM_ORDER = item.MEDICINE_NUM_ORDER;
            //ado.MEDICINE_REGISTER_NUMBER = item.MEDICINE_REGISTER_NUMBER;
            //ado.MEDICINE_TCY_NUM_ORDER = item.MEDICINE_TCY_NUM_ORDER;
            ado.MEDICINE_TYPE_CODE = item.MATERIAL_TYPE_CODE;
            ado.MEDICINE_TYPE_ID = item.MATERIAL_TYPE_ID;
            ado.MEDICINE_TYPE_NAME = item.MATERIAL_TYPE_NAME;
            //ado.MEDICINE_TYPE_NUM_ORDER = item.MEDICINE_TYPE_NUM_ORDER;
            //ado.MEDICINE_USE_FORM_CODE = item.MEDICINE_USE_FORM_CODE;
            //ado.MEDICINE_USE_FORM_ID = item.MEDICINE_USE_FORM_ID;
            //ado.MEDICINE_USE_FORM_NAME = item.MEDICINE_USE_FORM_NAME;
            //ado.MEDICINE_USE_FORM_NUM_ORDER = item.MEDICINE_USE_FORM_NUM_ORDER;
            ado.MEMA_GROUP_ID = item.MEMA_GROUP_ID;
            ado.MODIFIER = item.MODIFIER;
            ado.MODIFY_TIME = item.MODIFY_TIME;
            //ado.MORNING = item.MORNING;
            ado.NATIONAL_NAME = item.NATIONAL_NAME;
            //ado.NOON = item.NOON;
            ado.NUM_ORDER = item.NUM_ORDER;
            ado.OTHER_PAY_SOURCE_ID = item.OTHER_PAY_SOURCE_ID;
            ado.OTHER_PAY_SOURCE_CODE = item.OTHER_PAY_SOURCE_CODE;
            ado.OTHER_PAY_SOURCE_NAME = item.OTHER_PAY_SOURCE_NAME;
            ado.PACKAGE_NUMBER = item.PACKAGE_NUMBER;
            ado.PATIENT_TYPE_CODE = item.PATIENT_TYPE_CODE;
            ado.PATIENT_TYPE_ID = item.PATIENT_TYPE_ID;
            ado.PATIENT_TYPE_NAME = item.PATIENT_TYPE_NAME;
            //ado.PREVIOUS_USING_COUNT = item.PREVIOUS_USING_COUNT;
            ado.PRICE = item.PRICE ?? 0;
            ado.RECORDING_TRANSACTION = item.RECORDING_TRANSACTION;
            //ado.REGISTER_NUMBER = item.REGISTER_NUMBER;
            ado.REQ_DEPARTMENT_ID = item.REQ_DEPARTMENT_ID;
            ado.REQ_ROOM_ID = item.REQ_ROOM_ID;
            ado.REQ_USER_TITLE = item.REQ_USER_TITLE;
            ado.SERE_SERV_PARENT_ID = item.SERE_SERV_PARENT_ID;
            ado.SERVICE_CONDITION_ID = item.SERVICE_CONDITION_ID;
            ado.SERVICE_ID = item.SERVICE_ID;
            ado.SERVICE_UNIT_CODE = item.SERVICE_UNIT_CODE;
            ado.SERVICE_UNIT_ID = item.SERVICE_UNIT_ID;
            ado.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
            //ado.SPEED = item.SPEED;
            ado.SUPPLIER_CODE = item.SUPPLIER_CODE;
            ado.SUPPLIER_ID = item.SUPPLIER_ID;
            ado.SUPPLIER_NAME = item.SUPPLIER_NAME;
            //ado.TAX_RATIO = item.TAX_RATIO;
            //ado.TCY_NUM_ORDER = item.TCY_NUM_ORDER;
            ado.TDL_AGGR_EXP_MEST_ID = item.TDL_AGGR_EXP_MEST_ID;
            ado.TDL_INTRUCTION_TIME = item.TDL_INTRUCTION_TIME;
            ado.TDL_MEDI_STOCK_ID = item.TDL_MEDI_STOCK_ID;
            ado.TDL_MEDICINE_TYPE_ID = item.TDL_MATERIAL_TYPE_ID;
            ado.TDL_PRES_REQ_USER_TITLE = item.TDL_PRES_REQ_USER_TITLE;
            ado.TDL_SERVICE_REQ_ID = item.TDL_SERVICE_REQ_ID;
            ado.TDL_TREATMENT_ID = item.TDL_TREATMENT_ID;
            //ado.TDL_VACCINATION_ID = item.TDL_VACCINATION_ID;
            ado.TH_AMOUNT = item.TH_AMOUNT;
            ado.TUTORIAL = item.TUTORIAL;
            ado.USE_ORIGINAL_UNIT_FOR_PRES = item.USE_ORIGINAL_UNIT_FOR_PRES;
            //ado.USE_TIME_TO = item.USE_TIME_TO;
            //ado.VACCINATION_RESULT_ID = item.VACCINATION_RESULT_ID;
            ado.VAT_RATIO = item.VAT_RATIO ?? 0;
            ado.VIR_PRICE = item.VIR_PRICE;
            //var impPrice = Material.Where(x => x.ID == item.MATERIAL_ID).FirstOrDefault();
            //if (impPrice != null)
            {
                ado.PRICE_MEDI_MATE = item.IMP_PRICE * (1 + (item.IMP_VAT_RATIO));
            }
        }

        private void ProcessExpMestCode(List<V_HIS_EXP_MEST_MATERIAL> listExpMaterial, List<V_HIS_EXP_MEST_MEDICINE> listExpMedicine, List<V_HIS_EXP_MEST_MATERIAL> listMaterialDpk, List<V_HIS_EXP_MEST_MEDICINE> listMedicineDpk)
        {
            string query = "select * from his_medi_stock";
            List<HIS_MEDI_STOCK> listStock = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MEDI_STOCK>(query) ?? new List<HIS_MEDI_STOCK>();
            //Không phân loại theo loại xuất

            //chi tiết thuốc vật tư thực xuất
            LogSystem.Info("ListTemp: " + string.Join(",", ListTempType));
            if (IsNotNullOrEmpty(listExpMedicine) && (ListTempType.Count == 0 || ListTempType.Contains("ExpMest") || ListTempType.Contains("ExpMestCode") || ListTempType.Contains("Detail")))
            {

                foreach (var itemGr in listExpMedicine)
                {

                    var expType = listExpType.FirstOrDefault(o => o.ID == itemGr.EXP_MEST_TYPE_ID) ?? new HIS_EXP_MEST_TYPE();
                    var aggrExpMest = listAggrExpMest.FirstOrDefault(o => o.ID == itemGr.AGGR_EXP_MEST_ID);

                    var treatment = listTreatment.FirstOrDefault(p => p.ID == itemGr.TDL_TREATMENT_ID);
                    var serviceReq = listServiceReq.FirstOrDefault(p => p.ID == itemGr.TDL_SERVICE_REQ_ID);


                    Mrs00382RDO ado = new Mrs00382RDO();
                    //if (ado.AMOUT == null)
                    //    ado.AMOUT = 0; 

                    ado.TYPE = "THUỐC";
                    ado.AMOUNT = itemGr.AMOUNT;
                    if (ado.EXP_PRICE == null)
                        ado.EXP_PRICE = 0;
                    ado.EXP_PRICE = (itemGr.PRICE ?? 0) * (1 + (itemGr.VAT_RATIO ?? 0));

                    ado.EXP_MEST_TYPE_CODE = expType.EXP_MEST_TYPE_CODE;
                    ado.EXP_MEST_TYPE_NAME = expType.EXP_MEST_TYPE_NAME;
                    //if (ado.IMP_PRICE == null)
                    //    ado.IMP_PRICE = 0; 
                    ado.EXP_MEST_TYPE_ID = itemGr.EXP_MEST_TYPE_ID;
                    ado.EXP_MEST_CODE = itemGr.EXP_MEST_CODE;
                    if (aggrExpMest != null)
                    {
                        ado.TDL_AGGR_EXP_MEST_CODE = aggrExpMest.EXP_MEST_CODE;

                    }
                    else
                    {
                        ado.TDL_AGGR_EXP_MEST_CODE = itemGr.EXP_MEST_CODE;
                    }

                    //var impPrice = Medicine.Where(x => x.ID == itemGr.MEDICINE_ID).FirstOrDefault();
                    //if (impPrice!=null)
                    {
                        ado.PRICE_MEDI_MATE = itemGr.IMP_PRICE * (1 + (itemGr.IMP_VAT_RATIO));
                    }

                    ado.IMP_PRICE = (itemGr.IMP_PRICE) * (1 + (itemGr.IMP_VAT_RATIO));
                    ado.MEDI_MATE_TYPE_CODE = itemGr.MEDICINE_TYPE_CODE;
                    ado.MEDI_MATE_TYPE_NAME = itemGr.MEDICINE_TYPE_NAME;
                    ado.PACKAGE_NUMBER = itemGr.PACKAGE_NUMBER;
                    ado.SERVICE_UNIT_NAME = itemGr.SERVICE_UNIT_NAME;
                    ado.ACTIVE_INGR_BHYT_CODE = itemGr.ACTIVE_INGR_BHYT_CODE;
                    ado.ACTIVE_INGR_BHYT_NAME = itemGr.ACTIVE_INGR_BHYT_NAME;
                    ado.CONCENTRA = itemGr.CONCENTRA;
                    ado.MEDICINE_USE_FORM_NAME = itemGr.MEDICINE_USE_FORM_NAME;
                    ado.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(itemGr.EXP_TIME ?? 0);
                    ado.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(itemGr.EXPIRED_DATE ?? 0);
                    ado.MANUFACTURER_NAME = itemGr.MANUFACTURER_NAME;
                    ado.STATUS = itemGr.EXP_MEST_STT_ID;
                    ado.TUTORIAL = itemGr.TUTORIAL;
                    //if (ado.TOTAL_PRICE == null)
                    //    ado.TOTAL_PRICE = 0; 
                    ado.TOTAL_PRICE = ado.AMOUNT * (itemGr.PRICE ?? 0) * (1 + (itemGr.VAT_RATIO ?? 0));
                    ado.TOTAL_IMP_PRICE = ado.AMOUNT * itemGr.IMP_PRICE * (1 + (itemGr.IMP_VAT_RATIO));
                    ado.SERVICE_NAME = itemGr.MEDICINE_TYPE_NAME;


                    if (treatment != null)
                    {
                        ado.TREATMENT_CODE = treatment.TREATMENT_CODE;
                        ado.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                        ado.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                        ado.PATIENT_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                        ado.FEE_LOCK_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.FEE_LOCK_TIME ?? 0);
                        ado.TDL_TREATMENT_TYPE_ID = treatment.TDL_TREATMENT_TYPE_ID ?? 0;
                        ado.PATIENT_DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
                        ado.ICD_CODE = treatment.ICD_CODE;
                        ado.ICD_NAME = treatment.ICD_NAME;
                        var bedLog = listBedLog.FirstOrDefault(p => p.TREATMENT_ID == treatment.ID);
                        if (bedLog != null)
                        {
                            var bed = listBed.FirstOrDefault(p => p.ID == bedLog.BED_ID);
                            if (bed != null)
                            {
                                ado.BED_NAME = bed.BED_NAME;
                            }
                        }
                    }
                    ado.REQ_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(p => p.ID == itemGr.REQ_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;

                    ado.IS_CABINET = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == itemGr.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).IS_CABINET ?? 0;
                    ado.EXP_MEDI_STOCK_CODE = itemGr.MEDI_STOCK_CODE;
                    ado.EXP_MEDI_STOCK_NAME = itemGr.MEDI_STOCK_NAME;
                    //LogSystem.Info("EXP_MEDI_STOCK_NAME: " + itemGr.MEDI_STOCK_NAME);
                    //LogSystem.Info("EXP_MEDI_STOCK_NAME: " + (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == itemGr.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME);
                    if (serviceReq != null)
                    {
                        ado.TDL_INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(serviceReq.INTRUCTION_TIME);
                        ado.REQUEST_USERNAME = serviceReq.REQUEST_USERNAME;
                    }


                    ado.PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(p => p.ID == itemGr.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                    //listExpMestId.AddRange(expMestIds); 

                    listRdoDetailExp.Add(ado);
                }


            }
            if (IsNotNullOrEmpty(listExpMaterial) && (ListTempType.Count == 0 || ListTempType.Contains("ExpMest") || ListTempType.Contains("ExpMestCode") || ListTempType.Contains("Detail")))
            {

                foreach (var itemGr in listExpMaterial)
                {
                    var expType = listExpType.FirstOrDefault(o => o.ID == itemGr.EXP_MEST_TYPE_ID) ?? new HIS_EXP_MEST_TYPE();
                    var aggrExpMest = listAggrExpMest.FirstOrDefault(o => o.ID == itemGr.AGGR_EXP_MEST_ID);

                    var treatment = listTreatment.FirstOrDefault(p => p.ID == itemGr.TDL_TREATMENT_ID);
                    var serviceReq = listServiceReq.FirstOrDefault(p => p.ID == itemGr.TDL_SERVICE_REQ_ID);

                    var materialType = listMaterialType.FirstOrDefault(p => p.ID == itemGr.MATERIAL_TYPE_ID);

                    Mrs00382RDO ado = new Mrs00382RDO();

                    //if (ado.AMOUT == null)
                    //    ado.AMOUT = 0; 
                    if (materialType != null && materialType.IS_CHEMICAL_SUBSTANCE != null && materialType.IS_CHEMICAL_SUBSTANCE == 1)
                    {
                        ado.TYPE = "HÓA CHẤT";
                    }
                    else
                    {
                        ado.TYPE = "VẬT TƯ";
                    }
                    ado.AMOUNT = itemGr.AMOUNT;
                    if (ado.EXP_PRICE == null)
                        ado.EXP_PRICE = 0;
                    ado.EXP_PRICE = (itemGr.PRICE ?? 0) * (1 + (itemGr.VAT_RATIO ?? 0));
                    ado.EXP_MEST_TYPE_CODE = expType.EXP_MEST_TYPE_CODE;
                    ado.EXP_MEST_TYPE_NAME = expType.EXP_MEST_TYPE_NAME;
                    //if (ado.IMP_PRICE == null)
                    //    ado.IMP_PRICE = 0; 
                    ado.EXP_MEST_CODE = itemGr.EXP_MEST_CODE;
                    if (aggrExpMest != null)
                    {
                        ado.TDL_AGGR_EXP_MEST_CODE = aggrExpMest.EXP_MEST_CODE;

                    }
                    else
                    {
                        ado.TDL_AGGR_EXP_MEST_CODE = itemGr.EXP_MEST_CODE;
                    }
                    //var impPrice = Material.Where(x => x.ID == itemGr.MATERIAL_ID).FirstOrDefault();
                    //if (impPrice != null)
                    {
                        ado.PRICE_MEDI_MATE = itemGr.IMP_PRICE * (1 + (itemGr.IMP_VAT_RATIO));
                    }

                    ado.IMP_PRICE = (itemGr.IMP_PRICE) * (1 + (itemGr.IMP_VAT_RATIO));
                    ado.MEDI_MATE_TYPE_CODE = itemGr.MATERIAL_TYPE_CODE;
                    ado.MEDI_MATE_TYPE_NAME = itemGr.MATERIAL_TYPE_NAME;
                    ado.PACKAGE_NUMBER = itemGr.PACKAGE_NUMBER;
                    ado.SERVICE_UNIT_NAME = itemGr.SERVICE_UNIT_NAME;
                    ado.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(itemGr.EXP_TIME ?? 0);
                    ado.EXP_MEST_TYPE_ID = itemGr.EXP_MEST_TYPE_ID;
                    ado.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(itemGr.EXPIRED_DATE ?? 0);
                    ado.MANUFACTURER_NAME = itemGr.MANUFACTURER_NAME;
                    //if (ado.TOTAL_PRICE == null)
                    //    ado.TOTAL_PRICE = 0; 
                    ado.TOTAL_PRICE = ado.AMOUNT * (itemGr.PRICE ?? 0) * (1 + (itemGr.VAT_RATIO ?? 0));
                    ado.TOTAL_IMP_PRICE = ado.AMOUNT * itemGr.IMP_PRICE * (1 + (itemGr.IMP_VAT_RATIO));

                    ado.SERVICE_NAME = itemGr.MATERIAL_TYPE_NAME;

                    if (treatment != null)
                    {
                        ado.TREATMENT_CODE = treatment.TREATMENT_CODE;
                        ado.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                        ado.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                        ado.PATIENT_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                        ado.FEE_LOCK_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.FEE_LOCK_TIME ?? 0);
                        ado.TDL_TREATMENT_TYPE_ID = treatment.TDL_TREATMENT_TYPE_ID ?? 0;
                        ado.PATIENT_DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
                        ado.ICD_CODE = treatment.ICD_CODE;
                        ado.ICD_NAME = treatment.ICD_NAME;
                        var bedLog = listBedLog.FirstOrDefault(p => p.TREATMENT_ID == treatment.ID);
                        if (bedLog != null)
                        {
                            var bed = listBed.FirstOrDefault(p => p.ID == bedLog.BED_ID);
                            if (bed != null)
                            {
                                ado.BED_NAME = bed.BED_NAME;
                            }
                        }

                    }
                    ado.REQ_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(p => p.ID == itemGr.REQ_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;

                    ado.IS_CABINET = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == itemGr.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).IS_CABINET ?? 0;
                    ado.EXP_MEDI_STOCK_CODE = itemGr.MEDI_STOCK_CODE;
                    ado.EXP_MEDI_STOCK_NAME = itemGr.MEDI_STOCK_NAME;

                    if (serviceReq != null)
                    {
                        ado.TDL_INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(serviceReq.INTRUCTION_TIME);
                        ado.REQUEST_USERNAME = serviceReq.REQUEST_USERNAME;
                    }


                    ado.PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(p => p.ID == itemGr.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;

                    //listExpMestId.AddRange(expMestIds); 

                    //trạng thái tái sử dụng
                    if (listIsReusabling != null)
                    {
                        var IsReusabling = listIsReusabling.FirstOrDefault(o => o.MATERIAL_ID == itemGr.MATERIAL_ID && o.IS_REUSABLING >= 1);
                        if (IsReusabling != null)
                        {
                            ado.TOTAL_REUSABLE_EXP = itemGr.AMOUNT * (itemGr.PRICE ?? 0) * (1 + (itemGr.VAT_RATIO ?? 0));
                        }
                    }
                    listRdoDetailExp.Add(ado);
                }
            }

            //chi tiết thuốc vật tư đơn phòng khám đã tạo
            if (IsNotNullOrEmpty(listMedicineDpk) && (ListTempType.Count == 0 || ListTempType.Contains("Detail0") || ListTempType.Contains("Detail1")))
            {

                foreach (var itemGr in listMedicineDpk)
                {

                    var expType = listExpType.FirstOrDefault(o => o.ID == itemGr.EXP_MEST_TYPE_ID) ?? new HIS_EXP_MEST_TYPE();
                    var aggrExpMest = listAggrExpMest.FirstOrDefault(o => o.ID == itemGr.AGGR_EXP_MEST_ID);

                    var serviceReq = listServiceReq.FirstOrDefault(p => p.ID == itemGr.TDL_SERVICE_REQ_ID);
                    var treatment = listTreatment.FirstOrDefault(p => p.ID == itemGr.TDL_TREATMENT_ID);
                    Mrs00382RDO ado = new Mrs00382RDO();
                    //if (ado.AMOUT == null)
                    //    ado.AMOUT = 0; 
                    ado.AMOUNT = itemGr.AMOUNT;
                    if (ado.EXP_PRICE == null)
                        ado.EXP_PRICE = 0;
                    ado.EXP_PRICE = (itemGr.PRICE ?? 0) * (1 + (itemGr.VAT_RATIO ?? 0));
                    ado.EXP_MEST_TYPE_CODE = expType.EXP_MEST_TYPE_CODE;
                    ado.EXP_MEST_TYPE_NAME = expType.EXP_MEST_TYPE_NAME;
                    //var impPrice = Medicine.Where(x => x.ID == itemGr.MEDICINE_ID).FirstOrDefault();
                    //if (impPrice != null)
                    {
                        ado.PRICE_MEDI_MATE = itemGr.IMP_PRICE * (1 + (itemGr.IMP_VAT_RATIO));
                    }
                    //if (ado.IMP_PRICE == null)
                    //    ado.IMP_PRICE = 0; 
                    ado.EXP_MEST_TYPE_ID = itemGr.EXP_MEST_TYPE_ID;
                    ado.EXP_MEST_CODE = itemGr.EXP_MEST_CODE;
                    if (aggrExpMest != null)
                    {
                        ado.TDL_AGGR_EXP_MEST_CODE = aggrExpMest.EXP_MEST_CODE;
                    }
                    else
                    {
                        ado.TDL_AGGR_EXP_MEST_CODE = itemGr.EXP_MEST_CODE;
                    }
                    ado.IMP_PRICE = (itemGr.IMP_PRICE) * (1 + (itemGr.IMP_VAT_RATIO));
                    ado.MEDI_MATE_TYPE_CODE = itemGr.MEDICINE_TYPE_CODE;
                    ado.MEDI_MATE_TYPE_NAME = itemGr.MEDICINE_TYPE_NAME;
                    ado.PACKAGE_NUMBER = itemGr.PACKAGE_NUMBER;
                    ado.SERVICE_UNIT_NAME = itemGr.SERVICE_UNIT_NAME;
                    ado.ACTIVE_INGR_BHYT_NAME = itemGr.ACTIVE_INGR_BHYT_NAME;
                    ado.CONCENTRA = itemGr.CONCENTRA;
                    ado.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(itemGr.EXP_TIME ?? 0);
                    ado.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(itemGr.EXPIRED_DATE ?? 0);
                    ado.MANUFACTURER_NAME = itemGr.MANUFACTURER_NAME;
                    ado.STATUS = itemGr.EXP_MEST_STT_ID;
                    ado.EXP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.Where(p => p.ID == itemGr.MEDI_STOCK_ID).FirstOrDefault() ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                    //if (ado.TOTAL_PRICE == null)
                    //    ado.TOTAL_PRICE = 0; 
                    ado.TOTAL_PRICE = ado.AMOUNT * (itemGr.PRICE ?? 0) * (1 + (itemGr.VAT_RATIO ?? 0));
                    ado.TOTAL_IMP_PRICE = ado.AMOUNT * itemGr.IMP_PRICE * (1 + (itemGr.IMP_VAT_RATIO));

                    ado.SERVICE_NAME = itemGr.MEDICINE_TYPE_NAME;
                    if (serviceReq != null)
                    {
                        ado.REQUEST_USERNAME = serviceReq.REQUEST_USERNAME;
                    }


                    if (treatment != null)
                    {
                        ado.TREATMENT_CODE = treatment.TREATMENT_CODE;
                        ado.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                        ado.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                    ado.PRICE = (itemGr.PRICE??0) * (1 + (itemGr.VAT_RATIO??0));
                        ado.PATIENT_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                        ado.FEE_LOCK_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.FEE_LOCK_TIME ?? 0);
                    }
                    ado.PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(p => p.ID == itemGr.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                    //listExpMestId.AddRange(expMestIds); 

                    listRdoDetailDpk.Add(ado);
                }


            }
            if (IsNotNullOrEmpty(listMaterialDpk) && (ListTempType.Count == 0 || ListTempType.Contains("Detail0") || ListTempType.Contains("Detail1")))
            {

                foreach (var itemGr in listMaterialDpk)
                {
                    var expType = listExpType.FirstOrDefault(o => o.ID == itemGr.EXP_MEST_TYPE_ID) ?? new HIS_EXP_MEST_TYPE();
                    var aggrExpMest = listAggrExpMest.FirstOrDefault(o => o.ID == itemGr.AGGR_EXP_MEST_ID);
                    var serviceReq = listServiceReq.FirstOrDefault(p => p.ID == itemGr.TDL_SERVICE_REQ_ID);
                    var treatment = listTreatment.FirstOrDefault(p => p.ID == itemGr.TDL_TREATMENT_ID);
                    Mrs00382RDO ado = new Mrs00382RDO();
                    //if (ado.AMOUT == null)
                    //    ado.AMOUT = 0; 
                    ado.AMOUNT = itemGr.AMOUNT;
                    if (ado.EXP_PRICE == null)
                        ado.EXP_PRICE = 0;
                    ado.EXP_PRICE = (itemGr.PRICE ?? 0) * (1 + (itemGr.VAT_RATIO ?? 0));
                    ado.PRICE = (itemGr.PRICE??0) * (1 + (itemGr.VAT_RATIO??0));
                    ado.EXP_MEST_TYPE_CODE = expType.EXP_MEST_TYPE_CODE;
                    ado.EXP_MEST_TYPE_NAME = expType.EXP_MEST_TYPE_NAME;
                    //if (ado.IMP_PRICE == null)
                    //    ado.IMP_PRICE = 0; 
                    ado.EXP_MEST_CODE = itemGr.EXP_MEST_CODE;
                    if (aggrExpMest != null)
                    {
                        ado.TDL_AGGR_EXP_MEST_CODE = aggrExpMest.EXP_MEST_CODE;
                    }
                    else
                    {
                        ado.TDL_AGGR_EXP_MEST_CODE = itemGr.EXP_MEST_CODE;
                    }
                    //var impPrice = Material.Where(x => x.ID == itemGr.MATERIAL_ID).FirstOrDefault();
                    //if (impPrice != null)
                    {
                        ado.PRICE_MEDI_MATE = itemGr.IMP_PRICE * (1 + (itemGr.IMP_VAT_RATIO));
                    }
                    ado.IMP_PRICE = (itemGr.IMP_PRICE) * (1 + (itemGr.IMP_VAT_RATIO));
                    ado.MEDI_MATE_TYPE_CODE = itemGr.MATERIAL_TYPE_CODE;
                    ado.MEDI_MATE_TYPE_NAME = itemGr.MATERIAL_TYPE_NAME;
                    ado.PACKAGE_NUMBER = itemGr.PACKAGE_NUMBER;
                    ado.SERVICE_UNIT_NAME = itemGr.SERVICE_UNIT_NAME;
                    ado.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(itemGr.EXP_TIME ?? 0);
                    ado.EXP_MEST_TYPE_ID = itemGr.EXP_MEST_TYPE_ID;
                    ado.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(itemGr.EXPIRED_DATE ?? 0);
                    ado.MANUFACTURER_NAME = itemGr.MANUFACTURER_NAME;
                    ado.EXP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.Where(p => p.ID == itemGr.MEDI_STOCK_ID).FirstOrDefault() ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                    //if (ado.TOTAL_PRICE == null)
                    //    ado.TOTAL_PRICE = 0; 
                    ado.TOTAL_PRICE = ado.AMOUNT * (itemGr.PRICE ?? 0) * (1 + (itemGr.VAT_RATIO ?? 0));
                    ado.TOTAL_IMP_PRICE = ado.AMOUNT * itemGr.IMP_PRICE * (1 + (itemGr.IMP_VAT_RATIO));
                    ado.SERVICE_NAME = itemGr.MATERIAL_TYPE_NAME;
                    if (serviceReq != null)
                    {
                        ado.REQUEST_USERNAME = serviceReq.REQUEST_USERNAME;
                    }

                    if (treatment != null)
                    {
                        ado.TREATMENT_CODE = treatment.TREATMENT_CODE;
                        ado.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                        ado.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                        ado.PATIENT_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                        ado.FEE_LOCK_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.FEE_LOCK_TIME ?? 0);
                    }
                    ado.PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(p => p.ID == itemGr.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;

                    //listExpMestId.AddRange(expMestIds); 

                    //trạng thái tái sử dụng
                    if (listIsReusabling != null)
                    {
                        var IsReusabling = listIsReusabling.FirstOrDefault(o => o.MATERIAL_ID == itemGr.MATERIAL_ID && o.IS_REUSABLING >= 1);
                        if (IsReusabling != null)
                        {
                            ado.TOTAL_REUSABLE_EXP = itemGr.AMOUNT * (itemGr.PRICE ?? 0) * (1 + (itemGr.VAT_RATIO ?? 0));
                        }
                    }
                    listRdoDetailDpk.Add(ado);
                }
            }

        }

        private void ProcessExpMestType(List<V_HIS_EXP_MEST> listExp)
        {
            if (IsNotNullOrEmpty(listExp) && (ListTempType.Count == 0 || ListTempType.Contains("ExpType")))
            {
                var dicMedicine = listMedicine.GroupBy(g => g.EXP_MEST_ID??0).ToDictionary(p => p.Key, q => q.ToList());
                var dicMaterial = listMaterial.GroupBy(g => g.EXP_MEST_ID??0).ToDictionary(p => p.Key, q => q.ToList());
                foreach (var item in listExp)
                {
                    var medis = dicMedicine.ContainsKey(item.ID) ? dicMedicine[item.ID] : new List<V_HIS_EXP_MEST_MEDICINE>();
                    var mates = dicMaterial.ContainsKey(item.ID) ? dicMaterial[item.ID] : new List<V_HIS_EXP_MEST_MATERIAL>();
                    var medi = medis.Where(p => p.EXP_MEST_ID == item.ID).Sum(s => s.AMOUNT * (s.PRICE ?? s.IMP_PRICE) * (1 + (s.VAT_RATIO ?? s.IMP_VAT_RATIO)));
                    var mate = mates.Where(p => p.EXP_MEST_ID == item.ID).Sum(s => s.AMOUNT * (s.PRICE ?? s.IMP_PRICE) * (1 + (s.VAT_RATIO ?? s.IMP_VAT_RATIO)));

                    Mrs00382RDO rdo = new Mrs00382RDO();
                    rdo.EXP_MEST_TYPE_CODE = item.EXP_MEST_TYPE_CODE;
                    rdo.EXP_MEST_TYPE_NAME = item.EXP_MEST_TYPE_NAME;
                    rdo.EXP_MEDI_STOCK_CODE = item.MEDI_STOCK_CODE;
                    rdo.EXP_MEDI_STOCK_NAME = item.MEDI_STOCK_NAME;
                    rdo.AMOUNT = 1;
                    //if (medi != null)
                    //{
                    rdo.MEDICINE_PRICE = medi;
                    //}

                    //if (mate != null)
                    //{
                    rdo.MATERIAL_PRICE = mate;
                    //}

                    rdo.TOTAL_PRICE = (rdo.MEDICINE_PRICE ?? 0) + (rdo.MATERIAL_PRICE ?? 0);

                    //trạng thái tái sử dụng
                    if (listIsReusabling != null && mates.Count>0)
                    {
                        rdo.TOTAL_REUSABLE_EXP = mates.Where(o => listIsReusabling.Exists(q => q.MATERIAL_ID == o.MATERIAL_ID && q.IS_REUSABLING >= 1)).Sum(p => p.AMOUNT * (p.PRICE ?? 0) * (1 + (p.VAT_RATIO ?? 0)));
                    }
                    if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK || item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS)
                    {
                        rdo.IMP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == item.IMP_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                        rdo.IMP_MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == item.IMP_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                    }
                    listRdoExpMest.Add(rdo);
                    listRdoExpMestNew.Add(rdo);
                }
                listRdoExpMest = listRdoExpMest.GroupBy(p => new { p.EXP_MEST_TYPE_CODE, p.EXP_MEDI_STOCK_CODE, p.IMP_MEDI_STOCK_CODE }).Select(
                        o => new Mrs00382RDO
                        {
                            EXP_MEST_TYPE_CODE = o.First().EXP_MEST_TYPE_CODE,
                            EXP_MEST_TYPE_NAME = o.First().EXP_MEST_TYPE_NAME,
                            EXP_MEDI_STOCK_CODE = o.First().EXP_MEDI_STOCK_CODE,
                            EXP_MEDI_STOCK_NAME = o.First().EXP_MEDI_STOCK_NAME,
                            IMP_MEDI_STOCK_CODE = o.First().IMP_MEDI_STOCK_CODE,
                            IMP_MEDI_STOCK_NAME = o.First().IMP_MEDI_STOCK_NAME,
                            EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(o.First().EXP_TIME ?? 0),
                            AMOUNT = o.Sum(q => q.AMOUNT),
                            MEDICINE_PRICE = o.Sum(q => q.MEDICINE_PRICE),
                            MATERIAL_PRICE = o.Sum(q => q.MATERIAL_PRICE),
                            TOTAL_PRICE = o.Sum(q => q.TOTAL_PRICE)
                        }).ToList();
            }
        }

        private void ProcessPatientType(List<V_HIS_EXP_MEST_MEDICINE> listMedi, List<V_HIS_EXP_MEST_MATERIAL> listMate, List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine, List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial, List<V_HIS_IMP_MEST> listImp)
        {
            try
            {

                //region thuốc
                List<Mrs00382RDO> listSubMediMate = new List<Mrs00382RDO>();

                string keyGroupExp = "{2}_{3}";
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_EXP") && this.dicDataFilter["KEY_GROUP_EXP"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_EXP"].ToString()))
                {
                    keyGroupExp = this.dicDataFilter["KEY_GROUP_EXP"].ToString();
                }
                if (listMedi != null && (ListTempType.Count == 0 || ListTempType.Contains("MediMate") || ListTempType.Contains("Cabinet")))
                {
                    foreach (var item in listMedi)
                    {
                        var parent = listParentService.FirstOrDefault(p => p.CHILD_SERVICE_CODE == item.MEDICINE_TYPE_CODE);
                        var listSereServSub = dicSereServ.ContainsKey(item.TDL_SERVICE_REQ_ID ?? 0) ? dicSereServ[item.TDL_SERVICE_REQ_ID ?? 0] : new List<HIS_SERE_SERV>();
                        var sereServ = listSereServSub.FirstOrDefault(p => p.EXP_MEST_MEDICINE_ID == item.ID);

                        Mrs00382RDO rdo = new Mrs00382RDO();

                        if (parent != null)
                        {
                            rdo.PARENT_MEDI_MATE_TYPE_CODE = !string.IsNullOrEmpty(parent.SERVICE_CODE) ? parent.SERVICE_CODE : "NONE";
                            rdo.PARENT_MEDI_MATE_TYPE_NAME = !string.IsNullOrEmpty(parent.SERVICE_CODE) ? parent.SERVICE_NAME : "NHÓM KHÁC";
                        }
                        rdo.TYPE = "THUỐC";
                        rdo.IS_CABINET = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == item.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).IS_CABINET ?? 0;
                        rdo.EXP_MEST_TYPE_ID = item.EXP_MEST_TYPE_ID;
                        rdo.MEDI_MATE_TYPE_CODE = item.MEDICINE_TYPE_CODE;
                        rdo.MEDI_MATE_TYPE_NAME = item.MEDICINE_TYPE_NAME;
                        rdo.ACTIVE_INGR_BHYT_CODE = item.ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = item.ACTIVE_INGR_BHYT_NAME;
                        rdo.CONCENTRA = item.CONCENTRA;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.MEDICINE_TYPE_ID = item.MEDICINE_TYPE_ID;
                        rdo.EXP_MEST_ID = item.EXP_MEST_ID;
                        rdo.IMP_PRICE = item.IMP_PRICE;
                        rdo.PRICE = item.PRICE ?? 0;
                        rdo.IMP_VAT_RATIO = item.IMP_VAT_RATIO;
                        rdo.VAT_RATIO = item.VAT_RATIO ?? 0;
                        rdo.BID_ID = item.BID_ID;
                        rdo.BID_NUMBER = item.BID_NUMBER;
                        rdo.BID_NAME = item.BID_NAME;
                        rdo.AMOUNT = item.AMOUNT;
                        rdo.TOTAL_PRICE = item.AMOUNT * (item.PRICE ?? 0) * (1 + (item.VAT_RATIO ?? 0));
                        rdo.BHYT_EXP_AMOUNT = item.PATIENT_TYPE_ID == PatientTypeId__BHYT ? item.AMOUNT : 0;
                        rdo.VP_EXP_AMOUNT = item.PATIENT_TYPE_ID == PatientTypeId__FEE ? item.AMOUNT : 0;
                        rdo.FREE_EXP_AMOUNT = item.PATIENT_TYPE_ID == PatientTypeId__IS_FREE ? item.AMOUNT : 0;
                        if (sereServ != null)
                        {
                            if (sereServ.IS_EXPEND == 1)
                            {
                                rdo.EXPEND_AMOUNT = sereServ.AMOUNT;
                            }
                            rdo.REQ_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(p => p.ID == sereServ.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;

                            var treatment = listTreatment.FirstOrDefault(p => p.ID == sereServ.TDL_TREATMENT_ID) ?? new HIS_TREATMENT();
                            rdo.TREATMENT_TYPE_CODE = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(p => p.ID == treatment.TDL_TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_CODE;
                            rdo.TREATMENT_TYPE_NAME = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(p => p.ID == treatment.TDL_TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME;
                            rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                            rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                            var bedLog = listBedLog.FirstOrDefault(p => p.TREATMENT_ID == treatment.ID) ?? new HIS_TREATMENT_BED_ROOM();
                            var bed = listBed.FirstOrDefault(p => p.ID == bedLog.BED_ID);
                            if (bed != null)
                            {
                                rdo.BED_NAME = bed.BED_NAME;

                            }
                            var impMestMedi = listImpMestMedicine.Where(o => o.TH_EXP_MEST_MEDICINE_ID == item.ID).ToList();
                            if (impMestMedi != null)
                            {
                                foreach (var impMedi in impMestMedi)
                                {
                                    rdo.RETURN_AMOUNT += impMedi.AMOUNT;
                                    if (impMedi.IMP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL)
                                {
                                    if (treatment.TDL_PATIENT_TYPE_ID == PatientTypeId__BHYT)
                                    {
                                        rdo.BHYT_RETURN_AMOUNT += impMedi.AMOUNT;
                                    }
                                    else if (treatment.TDL_PATIENT_TYPE_ID == PatientTypeId__FEE)
                                    {
                                        rdo.VP_RETURN_AMOUNT += impMedi.AMOUNT;
                                    }
                                    else if (treatment.TDL_PATIENT_TYPE_ID == PatientTypeId__IS_FREE)
                                    {
                                        rdo.FREE_RETURN_AMOUNT += impMedi.AMOUNT;
                                    }
                                }
                                }

                            }
                        }

                        listSubMediMate.Add(rdo);
                    }
                }
                //endregion

                //region vật tư
                if (listMate != null && (ListTempType.Count == 0 || ListTempType.Contains("MediMate") || ListTempType.Contains("Cabinet")))
                {
                    foreach (var item in listMate)
                    {
                        var parent = listParentService.FirstOrDefault(p => p.CHILD_SERVICE_CODE == item.MATERIAL_TYPE_CODE);
                        var listSereServSub = dicSereServ.ContainsKey(item.TDL_SERVICE_REQ_ID ?? 0) ? dicSereServ[item.TDL_SERVICE_REQ_ID ?? 0] : new List<HIS_SERE_SERV>();
                        var sereServ = listSereServSub.FirstOrDefault(p => p.EXP_MEST_MATERIAL_ID == item.ID);

                        Mrs00382RDO rdo = new Mrs00382RDO();

                        if (parent != null)
                        {
                            rdo.PARENT_MEDI_MATE_TYPE_CODE = !string.IsNullOrEmpty(parent.SERVICE_CODE) ? parent.SERVICE_CODE : "NONE";
                            rdo.PARENT_MEDI_MATE_TYPE_NAME = !string.IsNullOrEmpty(parent.SERVICE_CODE) ? parent.SERVICE_NAME : "NHÓM KHÁC";
                        }
                        rdo.TYPE = "VẬT TƯ";
                        rdo.IS_CABINET = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == item.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).IS_CABINET ?? 0;
                        rdo.EXP_MEST_TYPE_ID = item.EXP_MEST_TYPE_ID;
                        rdo.MEDI_MATE_TYPE_CODE = item.MATERIAL_TYPE_CODE;
                        rdo.MEDI_MATE_TYPE_NAME = item.MATERIAL_TYPE_NAME;
                        rdo.ACTIVE_INGR_BHYT_CODE = "";
                        rdo.ACTIVE_INGR_BHYT_NAME = "";
                        rdo.CONCENTRA = "";
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.MEDICINE_TYPE_ID = item.MATERIAL_TYPE_ID;
                        rdo.EXP_MEST_ID = item.EXP_MEST_ID;
                        rdo.IMP_PRICE = item.IMP_PRICE;
                        rdo.PRICE = item.PRICE??0;
                        rdo.IMP_VAT_RATIO = item.IMP_VAT_RATIO;
                        rdo.VAT_RATIO = item.VAT_RATIO??0;
                        rdo.BID_ID = item.BID_ID;
                        rdo.BID_NUMBER = item.BID_NUMBER;
                        rdo.BID_NAME = item.BID_NAME;
                        rdo.AMOUNT = item.AMOUNT;
                        rdo.TOTAL_PRICE = item.AMOUNT * (item.PRICE ?? 0) * (1 + (item.VAT_RATIO ?? 0));
                        rdo.BHYT_EXP_AMOUNT = item.PATIENT_TYPE_ID == PatientTypeId__BHYT ? item.AMOUNT : 0;
                        rdo.VP_EXP_AMOUNT = item.PATIENT_TYPE_ID == PatientTypeId__FEE ? item.AMOUNT : 0;
                        rdo.FREE_EXP_AMOUNT = item.PATIENT_TYPE_ID == PatientTypeId__IS_FREE ? item.AMOUNT : 0;
                        if (sereServ != null)
                        {
                            if (sereServ.IS_EXPEND == 1)
                            {
                                rdo.EXPEND_AMOUNT = sereServ.AMOUNT;
                            }
                            rdo.REQ_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(p => p.ID == sereServ.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;

                            var treatment = listTreatment.FirstOrDefault(p => p.ID == sereServ.TDL_TREATMENT_ID) ?? new HIS_TREATMENT();
                            rdo.TREATMENT_TYPE_CODE = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(p => p.ID == treatment.TDL_TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_CODE;
                            rdo.TREATMENT_TYPE_NAME = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(p => p.ID == treatment.TDL_TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME;
                            rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                            rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                            var bedLog = listBedLog.FirstOrDefault(p => p.TREATMENT_ID == treatment.ID) ?? new HIS_TREATMENT_BED_ROOM();
                            var bed = listBed.FirstOrDefault(p => p.ID == bedLog.BED_ID);
                            if (bed != null)
                            {
                                rdo.BED_NAME = bed.BED_NAME;

                            }
                            var impMestMate = listImpMestMaterial.Where(o => o.TH_EXP_MEST_MATERIAL_ID == item.ID).ToList();
                            if (impMestMate != null)
                            {
                                foreach (var impMedi in impMestMate)
                                {
                                    rdo.RETURN_AMOUNT += impMedi.AMOUNT;
                                    if (impMedi.IMP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL)
                                    {
                                        if (treatment.TDL_PATIENT_TYPE_ID == PatientTypeId__BHYT)
                                        {
                                            rdo.BHYT_RETURN_AMOUNT += impMedi.AMOUNT;
                                        }
                                        else if (treatment.TDL_PATIENT_TYPE_ID == PatientTypeId__FEE)
                                        {
                                            rdo.VP_RETURN_AMOUNT += impMedi.AMOUNT;
                                        }
                                        else if (treatment.TDL_PATIENT_TYPE_ID == PatientTypeId__IS_FREE)
                                        {
                                            rdo.FREE_RETURN_AMOUNT += impMedi.AMOUNT;
                                        }
                                    }
                                }

                            }
                        }

                        //trạng thái tái sử dụng
                        if (listIsReusabling != null)
                        {
                            var IsReusabling = listIsReusabling.FirstOrDefault(o => o.MATERIAL_ID == item.MATERIAL_ID && o.IS_REUSABLING >=1);
                            if (IsReusabling != null)
                            {
                                rdo.TOTAL_REUSABLE_EXP = item.AMOUNT * (item.PRICE??0)*(1 + (item.VAT_RATIO ?? 0));
                            }
                        }
                        listSubMediMate.Add(rdo);
                    }
                }
                //endregion
                var cabinets = listSubMediMate.GroupBy(p => new {p.TYPE, p.MEDICINE_TYPE_ID, p.IMP_PRICE, p.IMP_VAT_RATIO, p.TREATMENT_CODE, p.BED_NAME, p.REQ_ROOM_NAME, p.IS_CABINET }).Select(p => p.First()).ToList();
                if (listForCabinet == null) listForCabinet = new List<Mrs00382RDO>();
                foreach (var item in cabinets)
                {
                    listForCabinet.Add((Mrs00382RDO)item.Clone());
                }
                foreach (var item in listForCabinet)
                {
                    var subMediMate = listSubMediMate.Where(o =>o.TYPE == item.TYPE && o.MEDICINE_TYPE_ID == item.MEDICINE_TYPE_ID && o.IMP_PRICE == item.IMP_PRICE && o.IMP_VAT_RATIO == item.IMP_VAT_RATIO && o.TREATMENT_CODE == item.TREATMENT_CODE && o.BED_NAME == item.BED_NAME && o.REQ_ROOM_NAME == item.REQ_ROOM_NAME && o.IS_CABINET == item.IS_CABINET).ToList();
                    item.AMOUNT = subMediMate.Sum(s => s.AMOUNT);
                    item.TOTAL_PRICE = subMediMate.Sum(s => s.TOTAL_PRICE);
                    item.TOTAL_REUSABLE_EXP = subMediMate.Sum(s => s.TOTAL_REUSABLE_EXP);
                    item.BHYT_EXP_AMOUNT = subMediMate.Sum(s => s.BHYT_EXP_AMOUNT);
                    item.VP_EXP_AMOUNT = subMediMate.Sum(s => s.VP_EXP_AMOUNT);
                    item.FREE_EXP_AMOUNT = subMediMate.Sum(s => s.FREE_EXP_AMOUNT);
                    item.EXPEND_AMOUNT = subMediMate.Sum(s => s.EXPEND_AMOUNT);
                    item.RETURN_AMOUNT = subMediMate.Sum(s => s.RETURN_AMOUNT);
                    item.BHYT_RETURN_AMOUNT = subMediMate.Sum(s => s.BHYT_RETURN_AMOUNT);
                    item.VP_RETURN_AMOUNT = subMediMate.Sum(s => s.VP_RETURN_AMOUNT);
                    item.FREE_RETURN_AMOUNT = subMediMate.Sum(s => s.FREE_RETURN_AMOUNT);
                }
                var forPatientType = listSubMediMate.GroupBy(p => new { p.TYPE, p.MEDICINE_TYPE_ID, p.IMP_PRICE, p.IMP_VAT_RATIO }).Select(p => p.First()).ToList();
                if (listForPatientType == null) listForPatientType = new List<Mrs00382RDO>();
                foreach (var item in cabinets)
                {
                    listForPatientType.Add((Mrs00382RDO)item.Clone());
                }
                foreach (var item in listForPatientType)
                {
                    var subMediMate = listSubMediMate.Where(o => o.TYPE == item.TYPE && o.MEDICINE_TYPE_ID == item.MEDICINE_TYPE_ID && o.IMP_PRICE == item.IMP_PRICE && o.IMP_VAT_RATIO == item.IMP_VAT_RATIO).ToList();
                    item.AMOUNT = subMediMate.Sum(s => s.AMOUNT);
                    item.TOTAL_PRICE = subMediMate.Sum(s => s.TOTAL_PRICE);
                    item.TOTAL_REUSABLE_EXP = subMediMate.Sum(s => s.TOTAL_REUSABLE_EXP);
                    item.BHYT_EXP_AMOUNT = subMediMate.Sum(s => s.BHYT_EXP_AMOUNT);
                    item.VP_EXP_AMOUNT = subMediMate.Sum(s => s.VP_EXP_AMOUNT);
                    item.FREE_EXP_AMOUNT = subMediMate.Sum(s => s.FREE_EXP_AMOUNT);
                    item.EXPEND_AMOUNT = subMediMate.Sum(s => s.EXPEND_AMOUNT);
                    item.RETURN_AMOUNT = subMediMate.Sum(s => s.RETURN_AMOUNT);
                    item.BHYT_RETURN_AMOUNT = subMediMate.Sum(s => s.BHYT_RETURN_AMOUNT);
                    item.VP_RETURN_AMOUNT = subMediMate.Sum(s => s.VP_RETURN_AMOUNT);
                    item.FREE_RETURN_AMOUNT = subMediMate.Sum(s => s.FREE_RETURN_AMOUNT);
                }
                if (listForPatientType != null)
                {
                    listForPatientType = listForPatientType.OrderBy(p => p.PARENT_MEDI_MATE_TYPE_CODE).ThenBy(p => p.MEDI_MATE_TYPE_NAME).ToList();
                }
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
            }
        }

        private void ProcessExpType(List<V_HIS_EXP_MEST_MEDICINE> listMedi, List<V_HIS_EXP_MEST_MATERIAL> listMate)
        {
            try
            {
                List<Mrs00382RDO> listMediSub = new List<Mrs00382RDO>();
                List<Mrs00382RDO> listMateSub = new List<Mrs00382RDO>();

                string keyGroupExp = "{0}_{1}";
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_EXP") && this.dicDataFilter["KEY_GROUP_EXP"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_EXP"].ToString()))
                {
                    keyGroupExp = this.dicDataFilter["KEY_GROUP_EXP"].ToString();
                }
                if (listMedi != null && (ListTempType.Count == 0 || ListTempType.Contains("ExpMestType")))
                {
                    foreach (var medi in listMedi)
                    {
                        var item = listExp.FirstOrDefault(p => p.ID == medi.EXP_MEST_ID) ?? new V_HIS_EXP_MEST();
                        var imp = listImpMestMedicine.FirstOrDefault(p => p.TH_EXP_MEST_MEDICINE_ID == medi.EXP_MEST_ID && p.MEDICINE_TYPE_ID == medi.MEDICINE_TYPE_ID);
                        Mrs00382RDO rdo = new Mrs00382RDO();

                        if (imp != null)
                        {
                            rdo.IMP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(imp.IMP_TIME ?? 0);
                            rdo.IMP_MEST_CODE = imp.IMP_MEST_CODE;
                            rdo.IMP_MEDI_STOCK_CODE_NEW = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == imp.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE ?? "NONE";
                            rdo.IMP_MEDI_STOCK_NAME_NEW = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == imp.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME ?? "NONE";
                        }
                        if (medi.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT)
                        {
                            rdo.NOT_DTT = "X";
                        }
                        else
                        {
                            rdo.NOT_DTT = "";
                        }

                        rdo.EXP_MEST_CODE = item.EXP_MEST_CODE;
                        rdo.EXP_MEST_TYPE_CODE = item.EXP_MEST_TYPE_CODE;
                        rdo.EXP_MEST_TYPE_NAME = item.EXP_MEST_TYPE_NAME;
                        rdo.IS_CABINET = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == medi.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).IS_CABINET ?? 0;
                        rdo.EXP_MEDI_STOCK_CODE = item.MEDI_STOCK_CODE;
                        rdo.EXP_MEDI_STOCK_NAME = item.MEDI_STOCK_NAME;
                        rdo.IMP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == item.IMP_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE ?? "";
                        rdo.IMP_MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == item.IMP_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME ?? "";
                        rdo.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(medi.EXP_TIME ?? 0);
                        rdo.MEDI_MATE_TYPE_CODE = medi.MEDICINE_TYPE_CODE;
                        rdo.MEDI_MATE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                        rdo.CONCENTRA = medi.CONCENTRA;
                        rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                        rdo.PRICE = medi.PRICE ?? 0;
                        rdo.IMP_PRICE = medi.IMP_PRICE;
                        rdo.IMP_VAT_RATIO = medi.IMP_VAT_RATIO;
                        rdo.VAT_RATIO = medi.VAT_RATIO ?? 0;
                        rdo.AMOUNT = medi.AMOUNT;
                        rdo.TOTAL_PRICE = medi.AMOUNT * (medi.PRICE ?? medi.IMP_PRICE) * (1 + (medi.VAT_RATIO ?? medi.IMP_VAT_RATIO));
                        //rdo.MEDICINE_PRICE = medi.Sum(s => s.AMOUNT * (s.PRICE ?? 0) * (1 + (s.VAT_RATIO ?? 0)));
                        rdo.TYPE = "THUỐC";
                        listMediSub.Add(rdo);
                    }
                }
                listRdoExpMestType.AddRange(listMediSub);

                if (listMate != null)
                {
                    foreach (var mate in listMate)
                    {
                        var item = listExp.FirstOrDefault(p => p.ID == mate.EXP_MEST_ID) ?? new V_HIS_EXP_MEST();
                        var materialType = listMaterialType.FirstOrDefault(p => p.ID == mate.MATERIAL_TYPE_ID) ?? new HIS_MATERIAL_TYPE();
                        var imp = listImpMestMaterial.FirstOrDefault(p => p.TH_EXP_MEST_MATERIAL_ID == mate.EXP_MEST_ID && p.MATERIAL_TYPE_ID == mate.MATERIAL_TYPE_ID);
                        Mrs00382RDO rdo = new Mrs00382RDO();
                        if (imp != null)
                        {
                            rdo.IMP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(imp.IMP_TIME ?? 0);
                            rdo.IMP_MEST_CODE = imp.IMP_MEST_CODE;
                            rdo.IMP_MEDI_STOCK_CODE_NEW = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == imp.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE ?? "NONE";
                            rdo.IMP_MEDI_STOCK_NAME_NEW = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == imp.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME ?? "NONE";
                        }
                        if (mate.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT)
                        {
                            rdo.NOT_DTT = "X";
                        }
                        else
                        {
                            rdo.NOT_DTT = "";
                        }
                        rdo.EXP_MEST_CODE = item.EXP_MEST_CODE;
                        rdo.EXP_MEST_TYPE_CODE = item.EXP_MEST_TYPE_CODE;
                        rdo.EXP_MEST_TYPE_NAME = item.EXP_MEST_TYPE_NAME;
                        rdo.IS_CABINET = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == mate.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).IS_CABINET ?? 0;
                        rdo.EXP_MEDI_STOCK_CODE = item.MEDI_STOCK_CODE;
                        rdo.EXP_MEDI_STOCK_NAME = item.MEDI_STOCK_NAME;
                        rdo.IMP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == item.IMP_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE ?? "";
                        rdo.IMP_MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == item.IMP_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME ?? "";
                        rdo.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(mate.EXP_TIME ?? 0);
                        rdo.MEDI_MATE_TYPE_CODE = mate.MATERIAL_TYPE_CODE;
                        rdo.MEDI_MATE_TYPE_NAME = mate.MATERIAL_TYPE_NAME;
                        rdo.CONCENTRA = "";
                        rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                        rdo.PRICE = mate.PRICE ?? 0;
                        rdo.IMP_PRICE = mate.IMP_PRICE;
                        rdo.IMP_VAT_RATIO = mate.IMP_VAT_RATIO;
                        rdo.VAT_RATIO = mate.VAT_RATIO ?? 0;
                        rdo.AMOUNT = mate.AMOUNT;
                        rdo.TOTAL_PRICE = mate.AMOUNT * (mate.PRICE ?? mate.IMP_PRICE) * (1 + (mate.VAT_RATIO ?? mate.IMP_VAT_RATIO));
                        //rdo.MATERIAL_PRICE = mate.Sum(s => s.AMOUNT * (s.PRICE ?? 0) * (1 + (s.VAT_RATIO ?? 0)));
                        if (materialType.IS_CHEMICAL_SUBSTANCE != null && materialType.IS_CHEMICAL_SUBSTANCE == 1)
                        {
                            rdo.TYPE = "HÓA CHẤT";
                        }
                        else
                        {
                            rdo.TYPE = "VẬT TƯ";
                        }


                        //trạng thái tái sử dụng
                        if (listIsReusabling != null)
                        {
                            var IsReusabling = listIsReusabling.FirstOrDefault(o => o.MATERIAL_ID == mate.MATERIAL_ID && o.IS_REUSABLING >= 1);
                            if (IsReusabling != null)
                            {
                                rdo.TOTAL_REUSABLE_EXP = mate.AMOUNT * (mate.PRICE ?? 0) * (1 + (mate.VAT_RATIO ?? 0));
                            }
                        }
                        listMateSub.Add(rdo);
                    }
                }
                listRdoExpMestType.AddRange(listMediSub);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
            }
        }
        public List<Mrs00382RDO> GroupAggrExpMest(List<Mrs00382RDO> listRdo)
        {

            List<Mrs00382RDO> listR = new List<Mrs00382RDO>();
            try
            {
                var group = listRdo.GroupBy(x => new { x.TDL_AGGR_EXP_MEST_CODE, x.MEDI_MATE_TYPE_CODE, x.EXP_TIME }).ToList();
                foreach (var item in group)
                {
                    var expType = listExpType.FirstOrDefault(o => o.ID == item.First().EXP_MEST_TYPE_ID) ?? new HIS_EXP_MEST_TYPE();
                    var aggrExpMest = listAggrExpMest.FirstOrDefault(o => o.ID == item.First().AGGR_EXP_MEST_ID);
                    var serviceReq = listServiceReq.FirstOrDefault(p => p.ID == item.First().TDL_SERVICE_REQ_ID);
                    Mrs00382RDO rdo = new Mrs00382RDO();
                    rdo.EXP_MEST_CODE = item.First().TDL_AGGR_EXP_MEST_CODE;
                    rdo.TDL_AGGR_EXP_MEST_ID = item.First().TDL_AGGR_EXP_MEST_ID;
                    rdo.TDL_AGGR_EXP_MEST_CODE = item.First().TDL_AGGR_EXP_MEST_CODE;
                    rdo.AMOUNT = item.Sum(x => x.AMOUNT);
                    rdo.ACTIVE_INGR_BHYT_CODE = item.First().ACTIVE_INGR_BHYT_CODE;
                    rdo.ACTIVE_INGR_BHYT_NAME = item.First().ACTIVE_INGR_BHYT_NAME;
                    rdo.MEDICINE_USE_FORM_NAME = item.First().MEDICINE_USE_FORM_NAME;
                    if (rdo.EXP_PRICE == null)
                        rdo.EXP_PRICE = 0;
                    rdo.EXP_PRICE = (item.First().PRICE) * (1 + (item.First().VAT_RATIO));
                    rdo.EXP_TIME_STR = item.Where(x => x.EXP_TIME_STR != null).First().EXP_TIME_STR;
                    rdo.EXP_TIME = item.First().EXP_TIME;
                    rdo.PRICE_MEDI_MATE = item.First().PRICE_MEDI_MATE;
                    rdo.MEDI_MATE_TYPE_CODE = item.First().MEDI_MATE_TYPE_CODE;
                    rdo.EXP_MEST_TYPE_CODE = expType.EXP_MEST_TYPE_CODE;
                    rdo.EXP_MEST_TYPE_NAME = expType.EXP_MEST_TYPE_NAME;
                    rdo.MEDI_MATE_TYPE_NAME = item.Where(x => x.MEDI_MATE_TYPE_NAME != null).First().MEDI_MATE_TYPE_NAME;
                    rdo.PACKAGE_NUMBER = item.First().PACKAGE_NUMBER;
                    rdo.SERVICE_UNIT_NAME = item.First().SERVICE_UNIT_NAME;
                    rdo.CONCENTRA = item.First().CONCENTRA;
                    rdo.IMP_PRICE = item.First().IMP_PRICE;
                    //rdo.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.First().EXP_TIME ?? 0);
                    rdo.EXPIRED_DATE_STR = string.Join(";", item.Where(x => x.EXPIRED_DATE_STR != null).Select(p => p.EXPIRED_DATE_STR).ToList());
                    rdo.EXPIRED_DATE = item.First().EXPIRED_DATE;
                    rdo.MANUFACTURER_NAME = item.First().MANUFACTURER_NAME;
                    rdo.STATUS = rdo.EXP_MEST_STT_ID;
                    //if (ado.TOTAL_PRICE == null)
                    //    ado.TOTAL_PRICE = 0; 
                    rdo.TOTAL_PRICE = rdo.AMOUNT * (item.First().PRICE) * (1 + (item.First().VAT_RATIO));
                    rdo.TOTAL_IMP_PRICE = rdo.AMOUNT * item.First().IMP_PRICE * (1 + (item.First().IMP_VAT_RATIO));
                    rdo.SERVICE_NAME = item.First().MEDICINE_TYPE_NAME;
                    listR.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return listR;

        }

        private void ProcessExpMestMedicineByAge(List<V_HIS_EXP_MEST_MEDICINE> listMedi)
        {
            try
            {
                List<Mrs00382AgeRDO> listAgeRdo = new List<Mrs00382AgeRDO>();

                if (listMedi != null && (ListTempType.Count == 0 || ListTempType.Contains("GroupByAge")))
                {
                    foreach (var each in listMedi)
                    {
                        var treatment = listTreatment.FirstOrDefault(p => p.ID == each.TDL_TREATMENT_ID);
                        Mrs00382AgeRDO ageRdo = new Mrs00382AgeRDO();
                        ageRdo.MEDICINE_TYPE_ID = each.MEDICINE_TYPE_ID;
                        ageRdo.MEDICINE_TYPE_CODE = each.MEDICINE_TYPE_CODE;
                        ageRdo.MEDICINE_TYPE_NAME = each.MEDICINE_TYPE_NAME;
                        ageRdo.SERVICE_UNIT_NAME = each.SERVICE_UNIT_NAME;
                        ageRdo.CONCENTRA = each.CONCENTRA;
                        ageRdo.IMP_PRICE = each.IMP_PRICE;
                        ageRdo.EXP_PRICE = each.PRICE;
                        ageRdo.IMP_VAT_RATIO = each.IMP_VAT_RATIO;
                        ageRdo.VAT_RATIO = each.VAT_RATIO;
                        ageRdo.AMOUNT = each.AMOUNT;
                        ageRdo.VAT_RATIO = each.VAT_RATIO;
                        ageRdo.IMP_VAT_RATIO = each.IMP_VAT_RATIO;
                        ageRdo.REQUEST_ROOM_ID = each.REQ_ROOM_ID;
                        ageRdo.REQUEST_DEPARTMENT_ID = each.REQ_DEPARTMENT_ID;
                        if (treatment != null)
                        {
                            ageRdo.TDL_PATIENT_AGE = RDOCommon.CalculateAge(treatment.TDL_PATIENT_DOB) ?? 0;
                        }
                        listAgeRdo.Add(ageRdo);
                    }


                }

                if (listAgeRdo != null && (ListTempType.Count == 0 || ListTempType.Contains("GroupByAge")))
                {
                    var group = listAgeRdo.GroupBy(p => new { p.MEDICINE_TYPE_ID, p.IMP_PRICE, p.REQUEST_DEPARTMENT_ID, p.REQUEST_ROOM_ID }).ToList();
                    foreach (var item in group)
                    {
                        List<Mrs00382AgeRDO> listSub = item.ToList<Mrs00382AgeRDO>();
                        Mrs00382AgeRDO rdo = new Mrs00382AgeRDO();
                        rdo.MEDICINE_TYPE_CODE = listSub[0].MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = listSub[0].MEDICINE_TYPE_NAME + " " + listSub[0].CONCENTRA;
                        rdo.SERVICE_UNIT_NAME = listSub[0].SERVICE_UNIT_NAME;
                        rdo.REQ_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(p => p.ID == listSub[0].REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                        rdo.REQ_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(p => p.ID == listSub[0].REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        rdo.REQ_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == listSub[0].REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                        rdo.REQ_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == listSub[0].REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        rdo.EXP_PRICE =listSub[0].EXP_PRICE;
                        rdo.VAT_RATIO = listSub[0].VAT_RATIO;
                        rdo.IMP_PRICE = listSub[0].IMP_PRICE ;
                        rdo.IMP_VAT_RATIO = listSub[0].IMP_VAT_RATIO;
                        rdo.UNDER_6_AMOUNT = listSub.Where(p => p.TDL_PATIENT_AGE < 6).Sum(p => p.AMOUNT);
                        rdo.FROM_6_TO_15_AMOUNT = listSub.Where(p => p.TDL_PATIENT_AGE >= 6 && p.TDL_PATIENT_AGE < 15).Sum(p => p.AMOUNT);
                        rdo.MORE_THAN_15_AMOUNT = listSub.Where(p => p.TDL_PATIENT_AGE >= 15).Sum(p => p.AMOUNT);
                        listRdoAge.Add(rdo);
                    }
                }
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("EXP_MEST_TYPE_NAME", string.Join(" - ", (new HisExpMestTypeManager().Get(new HisExpMestTypeFilterQuery() { ID = mrs00382Filter.EXP_MEST_TYPE_ID }) ?? new List<HIS_EXP_MEST_TYPE>()).Select(o => o.EXP_MEST_TYPE_NAME).ToList()));
                if (mrs00382Filter.MEDI_STOCK_ID != null)
                {
                    dicSingleTag.Add("MEDI_STOCK_NAME", new HisMediStockManager().Get(new HisMediStockFilterQuery() { ID = mrs00382Filter.MEDI_STOCK_ID }).FirstOrDefault().MEDI_STOCK_NAME);
                }
                else
                {
                    string a = "NOTHING";
                    dicSingleTag.Add("MEDI_STOCK_NAME", a);
                }
                List<Mrs00382RDO> listRdoDetailExp1 = new List<Mrs00382RDO>();

                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(mrs00382Filter.TIME_FROM));
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(mrs00382Filter.TIME_TO));
                dicSingleTag.Add("TOTAL_COUNT", listRdo.Select(p => p.MEDI_MATE_TYPE_CODE).Distinct().Count());

                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(p => p.MEDI_MATE_TYPE_NAME).ToList());
                objectTag.AddObjectData(store, "ExpType", listRdoExpMest.Where(p => p.AMOUNT > 0).OrderBy(p => p.EXP_MEST_TYPE_CODE).ThenBy(p => p.EXP_MEDI_STOCK_CODE).ThenBy(p => p.IMP_MEDI_STOCK_CODE).ToList());
                if (mrs00382Filter.IS_GROUP == true && (ListTempType.Count == 0 || ListTempType.Contains("ExpMest") || ListTempType.Contains("ExpMestCode") || ListTempType.Contains("Detail")))
                {
                    listRdoDetailExp = GroupAggrExpMest(listRdoDetailExp);
                    objectTag.AddObjectData(store, "ExpMest", listRdoDetailExp.OrderBy(p => p.EXP_MEST_CODE).ToList());
                }
                else
                {
                    objectTag.AddObjectData(store, "ExpMest", listRdoDetailExp.OrderBy(p => p.EXP_MEST_CODE).ToList());
                }

                objectTag.AddObjectData(store, "ExpMestCode", listRdoDetailExp.GroupBy(p => p.EXP_MEST_CODE).Select(p => p.First()).Distinct().ToList());
                objectTag.AddRelationship(store, "ExpMestCode", "ExpMest", "EXP_MEST_CODE", "EXP_MEST_CODE");

                objectTag.AddObjectData(store, "Detail", listRdoDetailExp.OrderBy(p => p.EXP_TIME_STR).ThenBy(p => p.SERVICE_NAME).ThenBy(p => p.MEDI_MATE_TYPE_NAME).ToList());
                objectTag.AddObjectData(store, "NgoaiTru", listRdoDetailExp.Where(p => p.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || p.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).OrderBy(p => p.EXP_TIME_STR).ThenBy(p => p.SERVICE_NAME).ThenBy(p => p.MEDI_MATE_TYPE_NAME).ToList());
                if (mrs00382Filter.IS_FEE_LOCK == true)
                {
                    objectTag.AddObjectData(store, "Detail0", listRdoDetailDpk.Where(p => !string.IsNullOrEmpty(p.FEE_LOCK_TIME_STR) && p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK && (p.STATUS == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE || p.STATUS == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__EXECUTE)).OrderBy(p => p.EXP_TIME_STR).ThenBy(p => p.SERVICE_NAME).ThenBy(p => p.MEDI_MATE_TYPE_NAME).ToList());
                    string a = "Đã khóa viện phí";
                    dicSingleTag.Add("IS_LOCK", a);
                }
                else
                {
                    objectTag.AddObjectData(store, "Detail0", listRdoDetailDpk.Where(p => string.IsNullOrEmpty(p.FEE_LOCK_TIME_STR) && p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK && (p.STATUS == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE || p.STATUS == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__EXECUTE)).OrderBy(p => p.EXP_TIME_STR).ThenBy(p => p.SERVICE_NAME).ThenBy(p => p.MEDI_MATE_TYPE_NAME).ToList());
                    string a = "Chưa khóa viện phí";
                    dicSingleTag.Add("IS_LOCK", a);
                }



                objectTag.AddObjectData(store, "Detail1", listRdoDetailDpk.Where(p => p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK && p.STATUS == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE).OrderBy(p => p.EXP_MEST_CODE).ThenBy(p => p.MEDI_MATE_TYPE_CODE).ToList());
                objectTag.AddObjectData(store, "Detail2", listRdoDpkGroupEMT.Where(p => p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK).OrderBy(p => p.MEDI_MATE_TYPE_NAME).ToList());
                objectTag.AddObjectData(store, "GroupByAge", listRdoAge.OrderBy(p => p.MEDICINE_TYPE_NAME).ToList());
                objectTag.AddObjectData(store, "ExpMestType", listRdoExpMestType.OrderBy(p => p.MEDI_MATE_TYPE_NAME).ToList());
                objectTag.AddObjectData(store, "SereServ", dicSereServ.Values.ToList());
                objectTag.AddObjectData(store, "Cabinet", listForCabinet);
                objectTag.AddObjectData(store, "MediMate", listForPatientType.Where(p => p.BHYT_EXP_AMOUNT > 0 || p.VP_EXP_AMOUNT > 0 || p.FREE_EXP_AMOUNT > 0 || p.EXPEND_AMOUNT > 0).ToList());
                objectTag.AddObjectData(store, "ExpGroupByCode", listRdoExpMestType.Where(p => p.IS_CABINET == 1 && p.NOT_DTT == "X" && p.IMP_MEDI_STOCK_NAME_NEW != "NONE").ToList());
                //Thêm key điều kiện lọc
                if (mrs00382Filter.PATIENT_CLASSIFY_IDs != null)
                {
                    string query = string.Format("select * from his_patient_classify where id in ({0})", string.Join(",", mrs00382Filter.PATIENT_CLASSIFY_IDs));
                    List<HIS_PATIENT_CLASSIFY> listSub = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PATIENT_CLASSIFY>(query) ?? new List<HIS_PATIENT_CLASSIFY>();

                    dicSingleTag.Add("PATIENT_CLASSIFY_NAMEs", string.Join(";", listSub.Select(p => p.PATIENT_CLASSIFY_NAME).Distinct().ToList()));
                }

                if (mrs00382Filter.EXP_MEST_REASON_IDs != null)
                {
                    string query = string.Format("select * from his_exp_mest_reason where id in ({0})", string.Join(",", mrs00382Filter.EXP_MEST_REASON_IDs));
                    List<HIS_EXP_MEST_REASON> listSub = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_EXP_MEST_REASON>(query) ?? new List<HIS_EXP_MEST_REASON>();

                    dicSingleTag.Add("EXP_MEST_REASON_NAMEs", string.Join(";", listSub.Select(p => p.EXP_MEST_REASON_NAME).Distinct().ToList()));
                }

                if (mrs00382Filter.DEPARTMENT_IDs != null)
                {
                    string query = string.Format("select * from his_department where id in ({0})", string.Join(",", mrs00382Filter.DEPARTMENT_IDs));
                    List<HIS_DEPARTMENT> listSub = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_DEPARTMENT>(query) ?? new List<HIS_DEPARTMENT>();

                    dicSingleTag.Add("DEPARTMENT_NAMEs", string.Join(";", listSub.Select(p => p.DEPARTMENT_NAME).Distinct().ToList()));
                }

                if (mrs00382Filter.PATIENT_TYPE_IDs != null)
                {
                    string query = string.Format("select * from his_patient_type where id in ({0})", string.Join(",", mrs00382Filter.PATIENT_TYPE_IDs));
                    List<HIS_PATIENT_TYPE> listSub = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PATIENT_TYPE>(query) ?? new List<HIS_PATIENT_TYPE>();

                    dicSingleTag.Add("PATIENT_TYPE_NAMEs", string.Join(";", listSub.Select(p => p.PATIENT_TYPE_NAME).Distinct().ToList()));
                }

                if (mrs00382Filter.DOCTOR_LOGINNAMEs != null)
                {
                    string query = string.Format("select * from acs_user where loginname in ('{0}')", string.Join("','", mrs00382Filter.DOCTOR_LOGINNAMEs));
                    List<ACS_USER> listSub = new MOS.DAO.Sql.MyAppContext().GetSql<ACS_USER>(query) ?? new List<ACS_USER>();

                    dicSingleTag.Add("DOCTOR_USERNAMEs", string.Join(";", listSub.Select(p => p.USERNAME).Distinct().ToList()));
                }

                if (mrs00382Filter.INPUT_DATA_ID_MEMA_TYPEs != null)
                {
                    string memaType = "";
                    List<string> listMema = new List<string>();
                    foreach (var item in mrs00382Filter.INPUT_DATA_ID_MEMA_TYPEs)
                    {
                        if (item == 1)
                        {
                            memaType = "Thuốc";
                        }
                        else
                        {
                            memaType = "Vật tư";
                        }
                        listMema.Add(memaType);
                    }
                    dicSingleTag.Add("MEMA_TYPEs", string.Join(";", listMema));
                }

                if (mrs00382Filter.MEDICINE_GROUP_IDs != null)
                {
                    string query = string.Format("select * from his_medicine_group where id in ({0})", string.Join(",", mrs00382Filter.MEDICINE_GROUP_IDs));
                    List<HIS_MEDICINE_GROUP> listSub = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MEDICINE_GROUP>(query) ?? new List<HIS_MEDICINE_GROUP>();

                    dicSingleTag.Add("MEDICINE_GROUP_NAMEs", string.Join(";", listSub.Select(p => p.MEDICINE_GROUP_NAME).Distinct().ToList()));
                }

                if (mrs00382Filter.IMP_MEDI_STOCK_IDs != null)
                {
                    string query = string.Format("select * from his_medi_stock where id in ({0})", string.Join(",", mrs00382Filter.IMP_MEDI_STOCK_IDs));
                    List<HIS_MEDI_STOCK> listSub = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MEDI_STOCK>(query) ?? new List<HIS_MEDI_STOCK>();

                    dicSingleTag.Add("IMP_MEDI_STOCK_NAMEs", string.Join(";", listSub.Select(p => p.MEDI_STOCK_NAME).Distinct().ToList()));
                }

                if (mrs00382Filter.EXP_MEDI_STOCK_IDs != null)
                {
                    string query = string.Format("select * from his_medi_stock where id in ({0})", string.Join(",", mrs00382Filter.EXP_MEDI_STOCK_IDs));
                    List<HIS_MEDI_STOCK> listSub = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MEDI_STOCK>(query) ?? new List<HIS_MEDI_STOCK>();

                    dicSingleTag.Add("EXP_MEDI_STOCK_NAMEs", string.Join(";", listSub.Select(p => p.MEDI_STOCK_NAME).Distinct().ToList()));
                }

                if (mrs00382Filter.EXP_MEST_TYPE_IDs != null)
                {
                    string query = string.Format("select * from his_exp_mest_type where id in ({0})", string.Join(",", mrs00382Filter.EXP_MEST_TYPE_IDs));
                    List<HIS_EXP_MEST_TYPE> listSub = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_EXP_MEST_TYPE>(query) ?? new List<HIS_EXP_MEST_TYPE>();

                    dicSingleTag.Add("EXP_MEST_TYPE_NAMEs", string.Join(";", listSub.Select(p => p.EXP_MEST_TYPE_NAME).Distinct().ToList()));
                }

                if (mrs00382Filter.EXP_MEST_TYPE_ID != null)
                {
                    string query = string.Format("select * from his_exp_mest_type where id = {0}", mrs00382Filter.EXP_MEST_TYPE_ID);
                    List<HIS_EXP_MEST_TYPE> listSub = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_EXP_MEST_TYPE>(query) ?? new List<HIS_EXP_MEST_TYPE>();

                    dicSingleTag.Add("EXP_MEST_TYPE_NAME", listSub.First().EXP_MEST_TYPE_NAME);
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
