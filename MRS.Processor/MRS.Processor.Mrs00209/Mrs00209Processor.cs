using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
using AutoMapper; 
using FlexCel.Report; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisDepartment; 
using MOS.MANAGER.HisExpMest; 
using MOS.MANAGER.HisExpMestMaterial; 
using MOS.MANAGER.HisExpMestMedicine; 
using MOS.MANAGER.HisIcd; 
using MOS.MANAGER.HisImpMestMaterial; 
using MOS.MANAGER.HisImpMestMedicine; 
using MOS.MANAGER.HisMaterialType; 
using MOS.MANAGER.HisMedicineType; 
using MOS.MANAGER.HisMediStock; 
using MOS.MANAGER.HisMediStockPeriod; 
using MOS.MANAGER.HisMestPeriodMate; 
using MOS.MANAGER.HisMestPeriodMedi; 
//using MOS.MANAGER.HisMobaImpMest; 
using MOS.MANAGER.HisPatient; 
using MOS.MANAGER.HisPatientType; 
using MOS.MANAGER.HisPatientTypeAlter; 
using MOS.MANAGER.HisReportTypeCat; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisService; 
using MOS.MANAGER.HisServiceRetyCat; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00209
{
    public class Mrs00209Processor : AbstractProcessor
    {
        private List<Mrs00209RDO> listMrs00209RDOs = new List<Mrs00209RDO>(); 
        CommonParam paramGet = new CommonParam(); 
        List<Mrs00209RDO> ListRdo = new List<Mrs00209RDO>(); 
        List<Mrs00209RDO> ListName = new List<Mrs00209RDO>(); 
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterialView = new List<V_HIS_EXP_MEST_MATERIAL>(); 
        List<V_HIS_IMP_MEST_MATERIAL> listMobaImpMestMaterialView = new List<V_HIS_IMP_MEST_MATERIAL>(); 
        List<MATERIAL_VIEW> listMaterialView1 = new List<MATERIAL_VIEW>(); 
        List<MATERIAL_VIEW> listMaterialView2 = new List<MATERIAL_VIEW>(); 
        List<MATERIAL_VIEW> listMaterialView = new List<MATERIAL_VIEW>(); 
        HIS_DEPARTMENT department = new HIS_DEPARTMENT(); 
        HIS_MEDI_STOCK medistock = new HIS_MEDI_STOCK(); 
        List<V_HIS_EXP_MEST> listHisExpMest = new List<V_HIS_EXP_MEST>(); 

        public Mrs00209Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00209Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            try
            {
                //get dữ liệu:
                if (((Mrs00209Filter)this.reportFilter).DEPARTMENT_ID != null)
                    department = new HisDepartmentManager(paramGet).GetById(((Mrs00209Filter)this.reportFilter).DEPARTMENT_ID??0); 
                if (((Mrs00209Filter)this.reportFilter).MEDI_STOCK_ID != null)
                    medistock = new HisMediStockManager(paramGet).GetById(((Mrs00209Filter)this.reportFilter).MEDI_STOCK_ID ?? 0); 

                var metyFilterHisExpMest = new HisExpMestViewFilterQuery
                {
                    FINISH_TIME_FROM = ((Mrs00209Filter)this.reportFilter).TIME_FROM,
                    FINISH_TIME_TO = ((Mrs00209Filter)this.reportFilter).TIME_TO,
                    REQ_DEPARTMENT_ID = ((Mrs00209Filter)this.reportFilter).DEPARTMENT_ID,
                    MEDI_STOCK_ID = ((Mrs00209Filter)this.reportFilter).MEDI_STOCK_ID,
                    EXP_MEST_TYPE_IDs = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT
                    }
                }; 

                listHisExpMest = new HisExpMestManager(paramGet).GetView(metyFilterHisExpMest); 

                var listExpMestIds = listHisExpMest.Select(s => s.ID).ToList(); 

                var skip = 0; 
                while (listExpMestIds.Count - skip > 0)
                {
                    var listIds = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    var metyFilterExpMestMaterial = new HisExpMestMaterialViewFilterQuery
                    {
                        EXP_MEST_IDs = listIds,
                        IS_EXPORT = true
                    }; 
                    //metyFilterExpMestMaterial.IN_EXECUTE = true; 
                    var expMestMaterialViews = new HisExpMestMaterialManager(paramGet).GetView(metyFilterExpMestMaterial); 
                    listExpMestMaterialView.AddRange(expMestMaterialViews); 
                }
                var metyFilterHisMobaImpMest = new HisImpMestViewFilterQuery
                {
                    MOBA_EXP_MEST_IDs = listExpMestIds
                }; 
                var listHisMobaImpMest = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(metyFilterHisMobaImpMest); 

                var listMobaImpMestIds = listHisMobaImpMest.Select(s => s.ID).ToList(); 

                skip = 0; 
                while (listMobaImpMestIds.Count - skip > 0)
                {
                    var listIds = listMobaImpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    var metyFilterMobaImpMestMaterial = new HisImpMestMaterialViewFilterQuery
                    {
                        IMP_MEST_IDs = listIds
                    }; 
                    var MobaImpMestMaterialViews = new HisImpMestMaterialManager(paramGet).GetView(metyFilterMobaImpMestMaterial); 
                    listMobaImpMestMaterialView.AddRange(MobaImpMestMaterialViews); 
                }

                listMaterialView1 = listExpMestMaterialView.Select(o =>
                    new MATERIAL_VIEW
                    {
                        MATERIAL_TYPE_CODE = o.MATERIAL_TYPE_CODE,
                        MATERIAL_TYPE_NAME = o.MATERIAL_TYPE_NAME,
                        PRICE = o.PRICE,
                        AMOUNT = o.AMOUNT,
                        EXP_MEST_ID = o.EXP_MEST_ID ?? 0,
                        MATERIAL_ID = o.MATERIAL_ID ?? 0
                    }).ToList(); 

                listMaterialView2 = listMobaImpMestMaterialView.Select(o =>
                    new MATERIAL_VIEW
                    {
                        MATERIAL_TYPE_CODE = o.MATERIAL_TYPE_CODE,
                        MATERIAL_TYPE_NAME = o.MATERIAL_TYPE_NAME,
                        PRICE = 1,
                        AMOUNT = o.AMOUNT * (-1),
                        EXP_MEST_ID = (listHisMobaImpMest.Where(q => q.ID == o.IMP_MEST_ID).First().MOBA_EXP_MEST_ID ?? 0),
                        MATERIAL_ID = o.MATERIAL_ID
                    }).ToList(); 

                listMaterialView.AddRange(listMaterialView1); 
                listMaterialView.AddRange(listMaterialView2); 

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
            bool result = false; 
            try
            {
                ListRdo.Clear(); 
                foreach (var material in listMaterialView)
                {
                    Mrs00209RDO rdo = new Mrs00209RDO(); 
                    {
                        rdo.MATERIAL_TYPE_CODE = material.MATERIAL_TYPE_CODE; 
                        rdo.MATERIAL_TYPE_NAME = material.MATERIAL_TYPE_NAME; 
                        rdo.PRICE = (long)material.PRICE; 
                        rdo.AMOUNT = material.AMOUNT; 
                        rdo.TOTAL_PRICE = (long)material.PRICE * material.AMOUNT; 
                        rdo.TDL_PATIENT_ID = (long)(listHisExpMest.Where(o => o.ID == material.EXP_MEST_ID).Select(p => p.TDL_PATIENT_ID).FirstOrDefault()); 
                        rdo.VIR_PATIENT_NAME = listHisExpMest.Where(o => o.ID == material.EXP_MEST_ID).Select(p => p.TDL_PATIENT_NAME).FirstOrDefault(); 
                        rdo.MATERIAL_ID = material.MATERIAL_ID; 
                    }
                    ListRdo.Add(rdo); 

                }

                if (IsNotNullOrEmpty(ListRdo))
                {
                    var GroupbyPatientIDs = ListRdo.GroupBy(o => o.TDL_PATIENT_ID).ToList(); 
                    ListRdo.Clear(); 
                    foreach (var groups in GroupbyPatientIDs)
                    {
                        var groupByMaterialID = groups.GroupBy(o => o.MATERIAL_ID).ToList(); 
                        foreach (var group in groupByMaterialID)
                        {
                            var listRdoSub = group.ToList<Mrs00209RDO>(); 
                            Mrs00209RDO rdo = new Mrs00209RDO()
                            {
                                MATERIAL_TYPE_CODE = listRdoSub.First().MATERIAL_TYPE_CODE,
                                MATERIAL_TYPE_NAME = listRdoSub.First().MATERIAL_TYPE_NAME,
                                PRICE = (long)listRdoSub.First().PRICE,
                                AMOUNT = group.Select(o => o.AMOUNT).Sum(),
                                TOTAL_PRICE = (long)listRdoSub.First().PRICE * group.Select(o => o.AMOUNT).Sum(),
                                TDL_PATIENT_ID = listRdoSub.First().TDL_PATIENT_ID,
                                VIR_PATIENT_NAME = listRdoSub.First().VIR_PATIENT_NAME,
                                MATERIAL_ID = listRdoSub.First().MATERIAL_ID
                            }; 
                            ListRdo.Add(rdo); 
                        }

                    }
                    ListRdo = ListRdo.Where(o => o.AMOUNT != 0).ToList(); 
                    ListName = ListRdo.GroupBy(o => o.TDL_PATIENT_ID).Select(p => p.First()).ToList(); 
                }

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
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00209Filter)this.reportFilter).TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00209Filter)this.reportFilter).TIME_TO)); 
            dicSingleTag.Add("DEPARTMENT_NAME", department.DEPARTMENT_NAME); 
            dicSingleTag.Add("MEDI_STOCK_NAME", medistock.MEDI_STOCK_NAME); 
            objectTag.AddObjectData(store, "Name", ListName); 
            objectTag.AddObjectData(store, "Material", ListRdo); 
            objectTag.AddRelationship(store, "Name", "Material", "TDL_PATIENT_ID", "TDL_PATIENT_ID"); 
        }
    }
}