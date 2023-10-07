using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisBlood;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisImpMestBlood;
using MOS.MANAGER.HisExpMestBlood;
using MOS.MANAGER.HisBloodAbo;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00428
{
    class Mrs00428Processor : AbstractProcessor
    {
        Mrs00428Filter castFilter = null;
        List<Mrs00428RDO> listRdo = new List<Mrs00428RDO>();
        List<Mrs00428RDO> listBloodType = new List<Mrs00428RDO>();

        List<BLOOD_ABO> listRdoABO = new List<BLOOD_ABO>();

        List<V_HIS_EXP_MEST_BLOOD> listExpMestBloods = new List<V_HIS_EXP_MEST_BLOOD>();
        List<V_HIS_IMP_MEST_BLOOD> listImpMestBloods = new List<V_HIS_IMP_MEST_BLOOD>();
        List<HIS_TREATMENT> listTreatments = new List<HIS_TREATMENT>();
        List<HIS_BLOOD_ABO> listBloodABOs = new List<HIS_BLOOD_ABO>();

        public string MEDI_STOCK_NAME = "";

        public Mrs00428Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00428Filter);
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);
                }

                dicSingleTag.Add("MEDI_STOCK_NAME", MEDI_STOCK_NAME);

                int i = 1;
                foreach (var abo in listRdoABO)
                {
                    dicSingleTag.Add("EXP_BLOOD_ABO_" + i, abo.BLOOD_ABO_CODE);
                    dicSingleTag.Add("IMP_BLOOD_ABO_" + i, abo.BLOOD_ABO_CODE);
                    i++;
                }

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Rdo", listBloodType.OrderBy(o=>o.TRANSFER_IN_MEDI_ORG_CODE!=null).ToList());
                objectTag.AddObjectData(store, "Report", listRdo);

                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00428Filter)this.reportFilter;

                listBloodABOs = new MOS.MANAGER.HisBloodAbo.HisBloodAboManager(param).Get(new HisBloodAboFilterQuery());

                HisMediStockFilterQuery mediStockFilter = new HisMediStockFilterQuery();
                mediStockFilter.ID = castFilter.MEDI_STOCK_ID;
                var listMediStocks = new MOS.MANAGER.HisMediStock.HisMediStockManager(param).Get(mediStockFilter);

                if (IsNotNullOrEmpty(listMediStocks))
                {
                    MEDI_STOCK_NAME = listMediStocks.First().MEDI_STOCK_NAME;
                }

                HisExpMestBloodViewFilterQuery expMestBloodViewFilter = new HisExpMestBloodViewFilterQuery();
                expMestBloodViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMestBloodViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                expMestBloodViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                listExpMestBloods.AddRange(new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(param).GetView(expMestBloodViewFilter));

                //HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery(); 
                //expMestFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                //expMestFilter.EXP_TIME_FROM = castFilter.TIME_FROM; 
                //expMestFilter.EXP_TIME_TO = castFilter.TIME_TO; 
                //var listExpMests = new MOS.MANAGER.HisExpMest.HisExpMestManager(param).Get(expMestFilter); 

                //var skip = 0; 
                //while (listExpMests.Count - skip > 0)
                //{
                //    var listIds = listExpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                //    HisExpMestBloodViewFilterQuery expMestBloodViewFilter = new HisExpMestBloodViewFilterQuery();   
                //    expMestBloodViewFilter.EXP_MEST_IDs = listIds.Select(s => s.ID).ToList(); 
                //    listExpMestBloods.AddRange(new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(param).GetView(expMestBloodViewFilter); 
                //}

                listExpMestBloods = listExpMestBloods.ToList();

                HisImpMestFilterQuery impMestFilter = new HisImpMestFilterQuery();
                impMestFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMestFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMestFilter.IMP_TIME_TO = castFilter.TIME_TO;
                var listImpMests = new MOS.MANAGER.HisImpMest.HisImpMestManager(param).Get(impMestFilter);

                var skip = 0;
                while (listImpMests.Count - skip > 0)
                {
                    var listIds = listImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisImpMestBloodViewFilterQuery impMestBloodViewFilter = new HisImpMestBloodViewFilterQuery();
                    impMestBloodViewFilter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList();
                    listImpMestBloods.AddRange(new MOS.MANAGER.HisImpMestBlood.HisImpMestBloodManager(param).GetView(impMestBloodViewFilter));
                }
                //ho so dieu tri
                if (listExpMestBloods != null)
                {
                    GetTreatment(listExpMestBloods);
                   
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetTreatment(List<V_HIS_EXP_MEST_BLOOD> listExpMestBloods)
        {
            var treatmentIds = listExpMestBloods.Where(p => p.TDL_TREATMENT_ID > 0).Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();

            var skip = 0;
            while (treatmentIds.Count - skip > 0)
            {
                var listIds = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                treatmentFilter.IDs = listIds;
                listTreatments.AddRange(new HisTreatmentManager(param).Get(treatmentFilter));
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();

                int i = 1;
                foreach (var abo in listBloodABOs.OrderBy(o => o.BLOOD_ABO_CODE).ToList())
                {
                    var bloodABO = new BLOOD_ABO();
                    bloodABO.ID = abo.ID;
                    bloodABO.BLOOD_ABO_CODE = abo.BLOOD_ABO_CODE;
                    bloodABO.EXP_BLOOD_ABO_TAG = "EXP_BLOOD_ABO_" + i;
                    bloodABO.IMP_BLOOD_ABO_TAG = "IMP_BLOOD_ABO_" + i;
                    i++;
                    listRdoABO.Add(bloodABO);
                }

                if (IsNotNullOrEmpty(listExpMestBloods))
                {
                    Dictionary<long, HIS_TREATMENT> dicTreatment = listTreatments.ToDictionary(p => p.ID);
                    foreach (var blood in listExpMestBloods)
                    {
                        var rdo = new Mrs00428RDO();

                        Inventec.Common.Mapper.DataObjectMapper.Map<MOS.EFMODEL.DataModels.V_HIS_EXP_MEST_BLOOD>(rdo, blood);
                        rdo.BLOOD_TYPE_ID = blood.BLOOD_TYPE_ID;
                        rdo.BLOOD_TYPE_CODE = blood.BLOOD_TYPE_CODE;
                        rdo.BLOOD_TYPE_NAME = blood.BLOOD_TYPE_NAME;
                        rdo.REQ_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == blood.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                        rdo.REQ_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == blood.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        rdo.REQ_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == blood.REQ_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                        rdo.REQ_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == blood.REQ_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        if (blood.TDL_TREATMENT_ID != null && dicTreatment != null && dicTreatment.ContainsKey(blood.TDL_TREATMENT_ID??0))
                        {
                            rdo.TRANSFER_IN_MEDI_ORG_CODE = dicTreatment[blood.TDL_TREATMENT_ID ?? 0].TRANSFER_IN_MEDI_ORG_CODE;
                            rdo.TRANSFER_IN_MEDI_ORG_NAME = dicTreatment[blood.TDL_TREATMENT_ID ?? 0].TRANSFER_IN_MEDI_ORG_NAME;
                        }

                        rdo.AMOUNT = 1;
                        var abo = listRdoABO.Where(s => s.ID == blood.BLOOD_ABO_ID).ToList();
                        if (IsNotNullOrEmpty(abo))
                        {
                            System.Reflection.PropertyInfo piBloodABO = typeof(Mrs00428RDO).GetProperty(abo.First().EXP_BLOOD_ABO_TAG);
                            piBloodABO.SetValue(rdo, Convert.ToDecimal(1));
                        }

                        listRdo.Add(rdo);
                    }
                }

                if (IsNotNullOrEmpty(listImpMestBloods))
                {
                    foreach (var blood in listImpMestBloods)
                    {
                        var rdo = new Mrs00428RDO();
                        Inventec.Common.Mapper.DataObjectMapper.Map<MOS.EFMODEL.DataModels.V_HIS_IMP_MEST_BLOOD>(rdo, blood);
                        rdo.BLOOD_TYPE_ID = blood.BLOOD_TYPE_ID;
                        rdo.BLOOD_TYPE_CODE = blood.BLOOD_TYPE_CODE;
                        rdo.BLOOD_TYPE_NAME = blood.BLOOD_TYPE_NAME;
                        rdo.REQ_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == blood.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                        rdo.REQ_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == blood.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        rdo.REQ_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == blood.REQ_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                        rdo.REQ_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == blood.REQ_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        rdo.AMOUNT_NHAP = 1;
                        if (blood.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DMTL)
                        {
                            rdo.AMOUNT = -1;
                        }    
                        var abo = listRdoABO.Where(s => s.ID == blood.BLOOD_ABO_ID).ToList();
                        if (IsNotNullOrEmpty(abo))
                        {
                            System.Reflection.PropertyInfo piBloodABO = typeof(Mrs00428RDO).GetProperty(abo.First().IMP_BLOOD_ABO_TAG);
                            piBloodABO.SetValue(rdo, -Convert.ToDecimal(1));
                        }

                        listRdo.Add(rdo);
                    }
                }

                string KeyGroupExp = "{0}";
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_EXP") && this.dicDataFilter["KEY_GROUP_EXP"] != null)
                {
                    KeyGroupExp = this.dicDataFilter["KEY_GROUP_EXP"].ToString();
                }
                listBloodType = listRdo.GroupBy(o=> string.Format(KeyGroupExp, o.BLOOD_TYPE_ID, o.PACKAGE_NUMBER, o.REQ_DEPARTMENT_ID, o.BLOOD_ABO_ID,o.BLOOD_RH_ID, o.SUPPLIER_ID,o.IMP_PRICE, o.IMP_VAT_RATIO, o.MEDI_STOCK_ID,o.TRANSFER_IN_MEDI_ORG_CODE)).Select(s => new Mrs00428RDO
                {
                    BLOOD_TYPE_ID = s.First().BLOOD_TYPE_ID,
                    BLOOD_TYPE_CODE = s.First().BLOOD_TYPE_CODE,
                    BLOOD_TYPE_NAME = s.First().BLOOD_TYPE_NAME,
                    IMP_PRICE = s.First().IMP_PRICE,
                    PACKAGE_NUMBER = s.First().PACKAGE_NUMBER,
                    IMP_VAT_RATIO = s.First().IMP_VAT_RATIO,
                    MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                    REQ_DEPARTMENT_ID = s.First().REQ_DEPARTMENT_ID,
                    REQ_DEPARTMENT_CODE = s.First().REQ_DEPARTMENT_CODE,
                    REQ_DEPARTMENT_NAME = s.First().REQ_DEPARTMENT_NAME,
                    REQ_ROOM_CODE = s.First().REQ_ROOM_CODE,
                    REQ_ROOM_NAME = s.First().REQ_ROOM_NAME,
                    TRANSFER_IN_MEDI_ORG_CODE = s.First().TRANSFER_IN_MEDI_ORG_CODE,
                    TRANSFER_IN_MEDI_ORG_NAME = s.First().TRANSFER_IN_MEDI_ORG_NAME,
                    VOLUME = s.First().VOLUME,
                    EXP_MEST_TYPE_ID = s.First().EXP_MEST_TYPE_ID,
                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                    BLOOD_ABO_ID = s.First().BLOOD_ABO_ID,
                    BLOOD_ABO_CODE = s.First().BLOOD_ABO_CODE,
                    BLOOD_RH_ID = s.First().BLOOD_RH_ID,
                    BLOOD_RH_CODE = s.First().BLOOD_RH_CODE,
                    SUPPLIER_ID = s.First().SUPPLIER_ID,
                    SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                    SUPPLIER_CODE = s.First().SUPPLIER_CODE,

                    EXP_BLOOD_ABO_1 = s.Sum(su => su.EXP_BLOOD_ABO_1),
                    EXP_BLOOD_ABO_2 = s.Sum(su => su.EXP_BLOOD_ABO_2),
                    EXP_BLOOD_ABO_3 = s.Sum(su => su.EXP_BLOOD_ABO_3),
                    EXP_BLOOD_ABO_4 = s.Sum(su => su.EXP_BLOOD_ABO_4),
                    EXP_BLOOD_ABO_5 = s.Sum(su => su.EXP_BLOOD_ABO_5),
                    EXP_BLOOD_ABO_6 = s.Sum(su => su.EXP_BLOOD_ABO_6),
                    EXP_BLOOD_ABO_7 = s.Sum(su => su.EXP_BLOOD_ABO_7),

                    IMP_BLOOD_ABO_1 = s.Sum(su => su.IMP_BLOOD_ABO_1),
                    IMP_BLOOD_ABO_2 = s.Sum(su => su.IMP_BLOOD_ABO_2),
                    IMP_BLOOD_ABO_3 = s.Sum(su => su.IMP_BLOOD_ABO_3),
                    IMP_BLOOD_ABO_4 = s.Sum(su => su.IMP_BLOOD_ABO_4),
                    IMP_BLOOD_ABO_5 = s.Sum(su => su.IMP_BLOOD_ABO_5),
                    IMP_BLOOD_ABO_6 = s.Sum(su => su.IMP_BLOOD_ABO_6),
                    IMP_BLOOD_ABO_7 = s.Sum(su => su.IMP_BLOOD_ABO_7),
                    AMOUNT_BH = s.Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(su => su.AMOUNT),
                    AMOUNT_VP = s.Where(o => o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(su => su.AMOUNT),
                    AMOUNT_TRA = s.Where(o => o.AMOUNT < 0).Sum(su => -su.AMOUNT),
                    AMOUNT_NHAP = s.Sum(su => su.AMOUNT_NHAP),
                    AMOUNT = s.Sum(su => su.AMOUNT)
                }).ToList();

                listRdoABO = listRdoABO.OrderBy(s => s.BLOOD_ABO_CODE).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }


    }
}
