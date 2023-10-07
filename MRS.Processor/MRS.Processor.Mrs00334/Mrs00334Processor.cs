using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisEkip;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisRemuneration;
using MOS.MANAGER.HisEkipUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;

using MOS.MANAGER.HisInvoice;
using MOS.MANAGER.HisInvoiceDetail;

using MRS.MANAGER.Config;
using MOS.MANAGER.HisSereServMaty;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;

namespace MRS.Processor.Mrs00334
{
    public class Mrs00334Processor : AbstractProcessor
    {
        List<Mrs00334RDO> ListRdo = new List<Mrs00334RDO>();
        List<Mrs00334RDO> ListRdoMeMaDetail = new List<Mrs00334RDO>();

        List<HIS_SERE_SERV> listSereServs = new List<HIS_SERE_SERV>();
        List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERVICE> listService = new List<HIS_SERVICE>();
        List<HIS_SERE_SERV> listSereServMediMate = new List<HIS_SERE_SERV>();
        List<HIS_EKIP> listEkips = new List<HIS_EKIP>();
        List<V_HIS_EKIP_USER> listEkipUsers = new List<V_HIS_EKIP_USER>();
        List<HIS_REMUNERATION> listRemus = new List<HIS_REMUNERATION>();
        List<HIS_MEDICINE> listMedicine = new List<HIS_MEDICINE>();
        List<HIS_MATERIAL> listMaterial = new List<HIS_MATERIAL>();

        Mrs00334Filter castFilter = new Mrs00334Filter();

        public Mrs00334Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00334Filter);
        }

        protected override bool GetData()///
        {
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = ((Mrs00334Filter)this.reportFilter);

                // Danh sach yeu cau
                HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
                serviceReqFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;                        //EXECUTE_TIME_TO
                serviceReqFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;                    //EXECUTE_TIME_FROM
                serviceReqFilter.SERVICE_REQ_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT };
                serviceReqFilter.REQUEST_ROOM_IDs = castFilter.REQUEST_ROOM_IDs;
                if (castFilter.EXECUTE_ROOM_IDs != null)
                {
                    serviceReqFilter.EXECUTE_ROOM_IDs = castFilter.EXECUTE_ROOM_IDs;
                }
                if (castFilter.EXE_ROOM_IDs != null)
                {
                    serviceReqFilter.EXECUTE_ROOM_IDs = castFilter.EXE_ROOM_IDs;
                }
                listServiceReq = new HisServiceReqManager(paramGet).Get(serviceReqFilter);

                if (castFilter.LOGINNAMEs != null && castFilter.LOGINNAMEs.Count > 0)
                {
                    listServiceReq = listServiceReq.Where(o => castFilter.LOGINNAMEs.Contains(o.EXECUTE_LOGINNAME)).ToList();
                }


                //yeu cau
                var skip1 = 0;
                var treatmentIds = listServiceReq.Select(o => o.TREATMENT_ID).Distinct().ToList();
                while (treatmentIds.Count - skip1 > 0)
                {
                    var listIds = treatmentIds.Skip(skip1).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip1 = skip1 + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServFilterQuery SereServFilter = new HisSereServFilterQuery();
                    SereServFilter.TREATMENT_IDs = listIds;
                    SereServFilter.PATIENT_TYPE_IDs = castFilter.PATIENT_TYPE_IDs;
                    var listSereServSub = new HisSereServManager(paramGet).Get(SereServFilter);
                    if (listSereServSub != null)
                    {
                        listSereServs.AddRange(listSereServSub);
                    }
                }
                listSereServs = listSereServs.Where(o => listServiceReq.Exists(p => p.ID == o.SERVICE_REQ_ID)).ToList();
                // các dv pttt
                HisServiceFilterQuery svFilter = new HisServiceFilterQuery();
                listService = new HisServiceManager(param).Get(svFilter);
                var sereServIds = listSereServs.Select(o => o.ID).ToList();
                if (sereServIds.Count > 0)
                {
                    skip1 = 0;
                    while (sereServIds.Count - skip1 > 0)
                    {
                        var listIds = sereServIds.Skip(skip1).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip1 = skip1 + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        //tính vật tư Hao phí
                        HisSereServFilterQuery sereServMediMateFilter = new HisSereServFilterQuery();
                        sereServMediMateFilter.PARENT_IDs = listIds;
                        sereServMediMateFilter.TDL_SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT };
                        //sereServMediMateFilter.IS_EXPEND = true;
                        var listSereServMediMateSub = new HisSereServManager(paramGet).Get(sereServMediMateFilter);
                        if (listSereServMediMateSub != null)
                        {
                            listSereServMediMate.AddRange(listSereServMediMateSub);
                        }
                    }
                }
                var medicineIds = listSereServMediMate.Where(p => p.MEDICINE_ID.HasValue).Select(o => o.MEDICINE_ID.Value).Distinct().ToList();
                if (medicineIds.Count > 0)
                {
                    skip1 = 0;
                    while (medicineIds.Count - skip1 > 0)
                    {
                        var listIds = medicineIds.Skip(skip1).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip1 = skip1 + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        //thông tin thuốc
                        HisMedicineFilterQuery medicineFilter = new HisMedicineFilterQuery();
                        medicineFilter.IDs = listIds;
                        var listMedicineSub = new HisMedicineManager(paramGet).Get(medicineFilter);
                        if (listMedicineSub != null)
                        {
                            listMedicine.AddRange(listMedicineSub);
                        }
                    }
                }
                var materialIds = listSereServMediMate.Where(p => p.MATERIAL_ID.HasValue).Select(o => o.MATERIAL_ID.Value).Distinct().ToList();
                if (materialIds.Count > 0)
                {
                    skip1 = 0;
                    while (materialIds.Count - skip1 > 0)
                    {
                        var listIds = materialIds.Skip(skip1).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip1 = skip1 + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        //thông tin Vật  tư
                        HisMaterialFilterQuery materialFilter = new HisMaterialFilterQuery();
                        materialFilter.IDs = listIds;
                        var listMaterialSub = new HisMaterialManager(paramGet).Get(materialFilter);
                        if (listMaterialSub != null)
                        {
                            listMaterial.AddRange(listMaterialSub);
                        }
                    }
                }
                var serviceIds = listSereServs.Select(o => o.SERVICE_ID).Distinct().ToList();
                if (sereServIds.Count > 0)
                {
                    skip1 = 0;
                    while (serviceIds.Count - skip1 > 0)
                    {
                        var listIds = serviceIds.Skip(skip1).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip1 = skip1 + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        // giá phụ cấp cho mõi dịch vụ
                        HisRemunerationFilterQuery remuFilter = new HisRemunerationFilterQuery();
                        remuFilter.SERVICE_IDs = listIds;
                        var listRemusSub = new HisRemunerationManager(param).Get(remuFilter);
                        if (listRemusSub != null)
                        {
                            listRemus.AddRange(listRemusSub);
                        }
                        
                    }
                    if (castFilter.PTTT_GROUP_IDs != null && castFilter.PTTT_GROUP_IDs.Count > 0 && castFilter.PTTT_GROUP_IDs.Count < HisPtttGroupCFG.PTTT_GROUPs.Count)
                    {
                        listSereServs = listSereServs.Where(o => listService.Exists(p => p.ID == o.SERVICE_ID&&castFilter.PTTT_GROUP_IDs.Contains(p.PTTT_GROUP_ID??0))).ToList();
                    }
                }
                var ekipIds = listSereServs.Where(p => p.EKIP_ID.HasValue).Select(o => o.EKIP_ID.Value).Distinct().ToList();
                if (ekipIds.Count > 0)
                {
                    skip1 = 0;
                    while (ekipIds.Count - skip1 > 0)
                    {
                        var listIds = ekipIds.Skip(skip1).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip1 = skip1 + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        //ekip
                        HisEkipUserViewFilterQuery ekipUserViewFilter = new HisEkipUserViewFilterQuery();
                        ekipUserViewFilter.EKIP_IDs = listIds;
                        var listEkipUserSub = new HisEkipUserManager(paramGet).GetView(ekipUserViewFilter);
                        if (listEkipUserSub != null)
                        {
                            listEkipUsers.AddRange(listEkipUserSub);
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
                if (IsNotNullOrEmpty(listSereServs))
                {
                    int number = 0;
                    foreach (var sereServ in listSereServs)
                    {
                        number++;
                        HIS_SERVICE service = listService.FirstOrDefault(s => s.ID == sereServ.SERVICE_ID);
                        var listMediMates = listSereServMediMate.Where(s => s.PARENT_ID == sereServ.ID).ToList();
                        var req = listServiceReq.FirstOrDefault(o => o.ID == sereServ.SERVICE_REQ_ID) ?? new HIS_SERVICE_REQ();
                        var listEkipUser = listEkipUsers.Where(s => s.EKIP_ID == sereServ.EKIP_ID).ToList();
                        var rdo = new Mrs00334RDO();
                        rdo.NUMBER = number;
                        rdo.REQUEST_DEPARTMENT = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == req.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        rdo.SERE_SERV_ID = sereServ.ID;
                        rdo.SERVICE_TYPE_ID = sereServ.TDL_SERVICE_TYPE_ID;
                        var serviceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == sereServ.TDL_SERVICE_TYPE_ID);
                        if (serviceType != null)
                        {
                            rdo.SERVICE_TYPE_NAME = serviceType.SERVICE_TYPE_NAME;
                            rdo.SERVICE_TYPE_CODE = serviceType.SERVICE_TYPE_CODE;
                        }
                        rdo.PATIENT_CODE = req.TDL_PATIENT_CODE;
                        rdo.PATIENT_NAME = req.TDL_PATIENT_NAME;
                        rdo.YEAR = req.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                        rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME;
                        rdo.SERVICE_CODE = sereServ.TDL_SERVICE_CODE;
                        if (service != null)
                        {
                            rdo.PTTT_GROUP_NAME = (HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == service.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_NAME;
                        }
                        rdo.AMOUNT = sereServ.AMOUNT;
                        rdo.PRICE = sereServ.PRICE;
                        rdo.TOTAL_PRICE = sereServ.AMOUNT * sereServ.PRICE;
                        if (listMediMates != null)
                        {
                            rdo.MEDICINE_PRICE = listMediMates.Where(s => s.MEDICINE_ID != null).ToList().Sum(x => x.AMOUNT * medicinePrice(x.MEDICINE_ID.Value));
                            rdo.MATERIAL_PRICE = listMediMates.Where(s => s.MATERIAL_ID != null).ToList().Sum(x => x.AMOUNT * materialPrice(x.MATERIAL_ID.Value));
                        }
                        rdo.PTTT_PRICE = 0;
                        foreach (var ekipUser in listEkipUser)
                        {
                            var remu = listRemus.Where(s => s.SERVICE_ID == sereServ.SERVICE_ID && s.EXECUTE_ROLE_ID == ekipUser.EXECUTE_ROLE_ID).ToList();
                            if (IsNotNullOrEmpty(remu))
                                rdo.PTTT_PRICE += remu.FirstOrDefault().PRICE;
                        }

                        //if (sereServ.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
                        //    rdo.PTTT_PRICE = rdo.PTTT_PRICE * (decimal)0.3; 

                        ListRdo.Add(rdo);
                        AddToListMediMateDetail(rdo, listMediMates);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private decimal materialPrice(long materialId)
        {
            var material = listMaterial.FirstOrDefault(o => o.ID == materialId);
            return (material != null) ? material.IMP_PRICE : 0;
        }

        private decimal medicinePrice(long medicineId)
        {
            var medicine = listMedicine.FirstOrDefault(o => o.ID == medicineId);
            return (medicine != null) ? medicine.IMP_PRICE : 0;
        }

        private void AddToListMediMateDetail(Mrs00334RDO rdo, List<HIS_SERE_SERV> listMediMates)
        {
            try
            {
                if (listMediMates == null) return;
                foreach (var item in listMediMates)
                {
                    var rdo1 = new Mrs00334RDO();
                    rdo1.SERE_SERV_ID = rdo.SERE_SERV_ID;
                    rdo1.MEDI_MATE_TYPE_NAME = item.TDL_SERVICE_NAME;
                    rdo1.MEDI_MATE_TYPE_CODE = item.TDL_SERVICE_CODE;
                    rdo1.MEDI_MATE_AMOUNT = item.AMOUNT;
                    if (item.MEDICINE_ID != null)
                    {
                        rdo1.TOTAL_PRICE = item.AMOUNT * medicinePrice(item.MEDICINE_ID.Value);
                    }
                    if (item.MATERIAL_ID != null)
                    {
                        rdo1.TOTAL_PRICE = item.AMOUNT * materialPrice(item.MATERIAL_ID.Value);
                    }
                    ListRdoMeMaDetail.Add(rdo1);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                }

                dicSingleTag.Add("CREATE_TIME", "Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year);
                objectTag.AddObjectData(store, "Parent", ListRdo.GroupBy(o => o.SERVICE_TYPE_ID).Select(p => p.First()).OrderBy(q => q.SERVICE_TYPE_NAME).ToList());
                objectTag.AddObjectData(store, "Rdo", ListRdo.OrderBy(o => o.REQUEST_DEPARTMENT).ToList());
                objectTag.AddRelationship(store, "Parent", "Rdo", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID");

                objectTag.AddObjectData(store, "RdoMeMaDetail", ListRdoMeMaDetail.OrderBy(o => o.REQUEST_DEPARTMENT).ToList());
                objectTag.AddRelationship(store, "Rdo", "RdoMeMaDetail", "SERE_SERV_ID", "SERE_SERV_ID");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

}
