using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
using AutoMapper;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMediStock;
//using MOS.MANAGER.HisMobaImpMest; 
//using MOS.MANAGER.HisPrescription; 
using SAR.EFMODEL.DataModels;
using SAR.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisExpMestType;
using MOS.MANAGER.HisTreatmentType;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisTreatmentEndType;

namespace MRS.Processor.Mrs00208
{
    public class Mrs00208Processor : AbstractProcessor
    {

        CommonParam paramGet = new CommonParam();
        List<Mrs00208RDO> ListParent = new List<Mrs00208RDO>();
        List<Mrs00208RDO> ListTreatment = new List<Mrs00208RDO>();
        List<Mrs00208RDO> ListRdo = new List<Mrs00208RDO>();
        Mrs00208Filter filter = new Mrs00208Filter();
        //List<V_HIS_EXP_MEST> listExpMest = new List<V_HIS_EXP_MEST>(); 
        //List<V_HIS_IMP_MEST> ListMobaImpMest = new List<V_HIS_IMP_MEST>();
        //List<V_HIS_EXP_MEST> ListPrescription = new List<V_HIS_EXP_MEST>();
        //List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        //List<V_HIS_IMP_MEST_MATERIAL> ListImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
        //List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterialView = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_MATERIAL_TYPE> listMaterialType = new List<V_HIS_MATERIAL_TYPE>();

        //loại phiếu xuất
        Dictionary<long, HIS_EXP_MEST_TYPE> dicExpMestType = new Dictionary<long, HIS_EXP_MEST_TYPE>();

        //Diện điều trị
        Dictionary<long, HIS_TREATMENT_TYPE> dicTreatmentType = new Dictionary<long, HIS_TREATMENT_TYPE>();

        //Đối tượng bệnh nhân
        Dictionary<long, HIS_PATIENT_TYPE> dicPatientType = new Dictionary<long, HIS_PATIENT_TYPE>();

        //Khoa yêu cầu
        Dictionary<long, HIS_DEPARTMENT> dicDepartment = new Dictionary<long, HIS_DEPARTMENT>();

        //Phòng
        Dictionary<long, V_HIS_ROOM> dicRoom = new Dictionary<long, V_HIS_ROOM>();

        //Kho
        Dictionary<long, V_HIS_MEDI_STOCK> dicMediStock = new Dictionary<long, V_HIS_MEDI_STOCK>();

        //Loại kết thúc
        Dictionary<long, HIS_TREATMENT_END_TYPE> dicTreatmentEndType = new Dictionary<long, HIS_TREATMENT_END_TYPE>();


        public Mrs00208Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00208Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                filter = (Mrs00208Filter)this.reportFilter;
                var paramGet = new CommonParam();
                //get dữ liệu:
                ListRdo = new ManagerSql().Get(filter);

                HisMaterialTypeViewFilterQuery medicineTypeFilter = new HisMaterialTypeViewFilterQuery();
                medicineTypeFilter.ID = filter.MATERIAL_TYPE_ID;
                var medicineTypes = new HisMaterialTypeManager().GetView(medicineTypeFilter);
                if (medicineTypes != null)
                {
                    listMaterialType = medicineTypes;
                }
                //loại phiếu xuất
                GetExpMestType();

                //Diện điều trị
                GetTreatmentType();

                //Đối tượng bệnh nhân
                GetPatientType();

                //Khoa yêu cầu
                GetDepartment();

                //Phòng
                GetRoom();

                //Kho
                GetMediStock();

                //Loại kết thúc
                GetTreatmentEndType();

                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        private void GetExpMestType()
        {
            var listExpMestType = new HisExpMestTypeManager().Get(new HisExpMestTypeFilterQuery());
            if (listExpMestType != null)
            {
                dicExpMestType = listExpMestType.ToDictionary(s=>s.ID,s=>s);
            }
        }

        private void GetTreatmentType()
        {
            var listTreatmentType = new HisTreatmentTypeManager().Get(new HisTreatmentTypeFilterQuery());
            if (listTreatmentType != null)
            {
                dicTreatmentType = listTreatmentType.ToDictionary(s => s.ID, s => s);
            }
        }

        private void GetPatientType()
        {
            var listPatientType = new HisPatientTypeManager().Get(new HisPatientTypeFilterQuery());
            if (listPatientType != null)
            {
                dicPatientType = listPatientType.ToDictionary(s => s.ID, s => s);
            }
        }

        private void GetDepartment()
        {
            if (HisDepartmentCFG.DEPARTMENTs != null)
            {
                dicDepartment = HisDepartmentCFG.DEPARTMENTs.ToDictionary(s => s.ID, s => s);
            }
        }

        private void GetRoom()
        {
            if (HisRoomCFG.HisRooms != null)
            {
                dicRoom = HisRoomCFG.HisRooms.ToDictionary(s => s.ID, s => s);
            }
        }

        private void GetMediStock()
        {
            if (HisMediStockCFG.HisMediStocks != null)
            {
                dicMediStock = HisMediStockCFG.HisMediStocks.ToDictionary(s => s.ID, s => s);
            }
        }

        private void GetTreatmentEndType()
        {
            var listTreatmentEndType = new HisTreatmentEndTypeManager().Get(new HisTreatmentEndTypeFilterQuery());
            if (listTreatmentEndType != null)
            {
                dicTreatmentEndType = listTreatmentEndType.ToDictionary(s => s.ID, s => s);
            }
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                foreach (var item in ListRdo)
                {
                    item.EXP_MEST_TYPE_CODE = dicExpMestType.ContainsKey(item.EXP_MEST_TYPE_ID) ? dicExpMestType[item.EXP_MEST_TYPE_ID].EXP_MEST_TYPE_CODE : "";
                    item.EXP_MEST_TYPE_NAME = dicExpMestType.ContainsKey(item.EXP_MEST_TYPE_ID) ? dicExpMestType[item.EXP_MEST_TYPE_ID].EXP_MEST_TYPE_NAME : "";

                    item.TREATMENT_TYPE_CODE = dicTreatmentType.ContainsKey(item.TREATMENT_TYPE_ID) ? dicTreatmentType[item.TREATMENT_TYPE_ID].TREATMENT_TYPE_CODE : "";
                    item.TREATMENT_TYPE_NAME = dicTreatmentType.ContainsKey(item.TREATMENT_TYPE_ID) ? dicTreatmentType[item.TREATMENT_TYPE_ID].TREATMENT_TYPE_NAME : "";

                    item.PATIENT_TYPE_CODE = dicPatientType.ContainsKey(item.TDL_PATIENT_TYPE_ID) ? dicPatientType[item.TDL_PATIENT_TYPE_ID].PATIENT_TYPE_CODE : "";
                    item.PATIENT_TYPE_NAME = dicPatientType.ContainsKey(item.TDL_PATIENT_TYPE_ID) ? dicPatientType[item.TDL_PATIENT_TYPE_ID].PATIENT_TYPE_NAME : "";

                    item.REQUEST_DEPARTMENT_CODE = dicDepartment.ContainsKey(item.REQ_DEPARTMENT_ID) ? dicDepartment[item.REQ_DEPARTMENT_ID].DEPARTMENT_CODE : "";
                    item.REQUEST_DEPARTMENT_NAME = dicDepartment.ContainsKey(item.REQ_DEPARTMENT_ID) ? dicDepartment[item.REQ_DEPARTMENT_ID].DEPARTMENT_NAME : "";

                    item.REQUEST_ROOM_CODE = dicRoom.ContainsKey(item.REQ_ROOM_ID) ? dicRoom[item.REQ_ROOM_ID].ROOM_CODE : "";
                    item.REQUEST_ROOM_NAME = dicRoom.ContainsKey(item.REQ_ROOM_ID) ? dicRoom[item.REQ_ROOM_ID].ROOM_NAME : "";

                    item.MEDI_STOCK_CODE = dicMediStock.ContainsKey(item.MEDI_STOCK_ID) ? dicMediStock[item.MEDI_STOCK_ID].MEDI_STOCK_CODE : "";
                    item.MEDI_STOCK_NAME = dicMediStock.ContainsKey(item.MEDI_STOCK_ID) ? dicMediStock[item.MEDI_STOCK_ID].MEDI_STOCK_NAME : "";

                    item.TREATMENT_END_TYPE_CODE = dicTreatmentEndType.ContainsKey(item.TREATMENT_END_TYPE_ID) ? dicTreatmentEndType[item.TREATMENT_END_TYPE_ID].TREATMENT_END_TYPE_CODE : "";
                    item.TREATMENT_END_TYPE_NAME = dicTreatmentEndType.ContainsKey(item.TREATMENT_END_TYPE_ID) ? dicTreatmentEndType[item.TREATMENT_END_TYPE_ID].TREATMENT_END_TYPE_NAME : "";
                }
                ListRdo = ListRdo.Where(p => p.AMOUNT_TRUST > 0).OrderBy(o => o.EXP_TIME).ToList();
                ListParent = ListRdo.Count > 0 ? ListRdo.GroupBy(o => o.PATIENT_ID).Select(p => p.First()).ToList() : new List<Mrs00208RDO>();
                ListTreatment = ListRdo.Count > 0 ? ListRdo.GroupBy(o => o.TDL_TREATMENT_ID).Select(p => p.First()).ToList() : new List<Mrs00208RDO>();
                //}
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00208Filter)this.reportFilter).TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00208Filter)this.reportFilter).TIME_TO));
            if (this.filter.DEPARTMENT_ID != null)
            {
                dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == this.filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
            }
            if (this.filter.MEDI_STOCK_ID != null)
            {
                dicSingleTag.Add("MEDI_STOCK_NAME", (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == this.filter.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME);
            }
            if (this.filter.MATERIAL_TYPE_ID != null)
            {
                dicSingleTag.Add("MATERIAL_TYPE_CODE", (listMaterialType.FirstOrDefault(o => o.ID == this.filter.MATERIAL_TYPE_ID) ?? new V_HIS_MATERIAL_TYPE()).MATERIAL_TYPE_CODE);
                dicSingleTag.Add("MATERIAL_TYPE_NAME", (listMaterialType.FirstOrDefault(o => o.ID == this.filter.MATERIAL_TYPE_ID) ?? new V_HIS_MATERIAL_TYPE()).MATERIAL_TYPE_NAME);
                dicSingleTag.Add("SERVICE_UNIT_NAME", (listMaterialType.FirstOrDefault(o => o.ID == this.filter.MATERIAL_TYPE_ID) ?? new V_HIS_MATERIAL_TYPE()).SERVICE_UNIT_NAME);
                dicSingleTag.Add("CONCENTRA", (listMaterialType.FirstOrDefault(o => o.ID == this.filter.MATERIAL_TYPE_ID) ?? new V_HIS_MATERIAL_TYPE()).CONCENTRA);
            }
            ListRdo = ListRdo.OrderBy(o => o.PATIENT_ID).ToList();
            objectTag.AddObjectData(store, "Name", ListParent);
            objectTag.AddObjectData(store, "Treatment", ListTreatment);

            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "Parent", ListParent);
            objectTag.AddRelationship(store, "Parent", "Report", "PATIENT_ID", "PATIENT_ID");

            objectTag.AddObjectData(store, "ExpMest", ListRdo);
            objectTag.AddRelationship(store, "Name", "ExpMest", "PATIENT_ID", "PATIENT_ID");

            objectTag.AddObjectData(store, "ExpMestDate", ListRdo.GroupBy(o => new { o.EXP_MEST_ID, o.INTRUCTION_DATE }).Select(p => p.First()).ToList());

            objectTag.AddObjectData(store, "Date", ListRdo.GroupBy(o => o.INTRUCTION_DATE).Select(p => p.First()).OrderBy(o => o.INTRUCTION_DATE).ToList());

            objectTag.AddObjectData(store, "PatientType", ListRdo.GroupBy(o => o.TDL_PATIENT_TYPE_CODE).Select(p => p.First()).ToList());

            objectTag.AddObjectData(store, "Categorys", ListRdo.GroupBy(o => o.CATEGORY_CODE).Select(p => p.First()).ToList());
            var categorys = ListRdo.Where(q => !string.IsNullOrEmpty(q.CATEGORY_CODE)).GroupBy(o => o.CATEGORY_CODE).Select(p => p.First()).ToList();
            for (int i = 0; i < 20; i++)
            {
                Mrs00208RDO category = new Mrs00208RDO();
                if (categorys.Count > 0 && categorys.Count > i)
                {
                    category = categorys[i];
                }
                dicSingleTag.Add(string.Format("CATEGORY_NAME_{0}", i + 1), category.CATEGORY_NAME);
                dicSingleTag.Add(string.Format("CATEGORY_CODE_{0}", i + 1), category.CATEGORY_CODE);

                objectTag.AddObjectData(store, string.Format("MaterialTypes{0}", i + 1), ListRdo.Where(q => !string.IsNullOrEmpty(category.CATEGORY_CODE) && q.CATEGORY_CODE == category.CATEGORY_CODE).GroupBy(o => o.MATERIAL_TYPE_ID).Select(p => p.First()).ToList());

                objectTag.AddObjectData(store, string.Format("RequestUsers{0}", i + 1), ListRdo.Where(q => !string.IsNullOrEmpty(category.CATEGORY_CODE) && q.CATEGORY_CODE == category.CATEGORY_CODE).GroupBy(o => o.REQUEST_LOGINNAME).Select(p => p.First()).ToList());
            }

            objectTag.AddObjectData(store, "MaterialTypes", ListRdo.GroupBy(o => o.MATERIAL_TYPE_ID).Select(p => p.First()).ToList());

            objectTag.AddObjectData(store, "RequestUsers", ListRdo.GroupBy(o => o.REQUEST_LOGINNAME).Select(p => p.First()).ToList());

        }
    }
}