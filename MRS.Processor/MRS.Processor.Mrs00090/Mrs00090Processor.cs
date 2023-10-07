using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisExpMestMedicine;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisExpMest; 
using MOS.MANAGER.HisImpMest; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00090
{
    public class Mrs00090Processor : AbstractProcessor
    {
        Mrs00090Filter castFilter = null; 
        List<Mrs00090RDO> ListRdo = new List<Mrs00090RDO>(); 
        List<V_HIS_EXP_MEST> ListPrescription = new List<V_HIS_EXP_MEST>(); 
        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>(); 

        public Mrs00090Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00090Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                castFilter = ((Mrs00090Filter)this.reportFilter); 

                //LoadDataToRam(); 
                LoadDataToRam(); 
                result = true; 
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
            bool result = false; 
            try
            {
                ProcessListPrescription(); 
                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void ProcessListPrescription()
        {
            try
            {
                if (ListPrescription.Count > 0)
                {
                    CommonParam paramGet = new CommonParam(); 
                    int start = 0; 
                    int count = ListPrescription.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        List<V_HIS_EXP_MEST> hisPrescriptions = ListPrescription.Skip(start).Take(limit).ToList(); 

                        HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery(); 
                        expMediFilter.EXP_MEST_IDs = hisPrescriptions.Select(s => s.ID).ToList(); 
                        expMediFilter.IS_EXPORT = true; 
                        var hisExpMestMedis = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expMediFilter); 

                        HisImpMestViewFilterQuery mobaFilter = new HisImpMestViewFilterQuery(); 
                        mobaFilter.MOBA_EXP_MEST_IDs = hisPrescriptions.Select(s => s.ID).ToList(); 
                        if (hisExpMestMedis != null && hisExpMestMedis.Count > 0)
                        {
                            ProcessListExpMestMedicine(paramGet, hisExpMestMedis); 
                            ProcessMobaMedicine(paramGet, mobaFilter); 
                        }
                        else
                        {
                            Inventec.Common.Logging.LogSystem.Info("Lay du lieu HisExpMestMedicine theo exp_mest_id tu prescription that bai. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => expMediFilter), expMediFilter)); 
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu."); 
                    }
                    else
                    {
                        ListRdo = ProcessListRdo(); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdo.Clear(); 
            }
        }

        private void ProcessListExpMestMedicine(CommonParam paramGet, List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedis)
        {
            try
            {
                var Groups = hisExpMestMedis.GroupBy(g => new { g.MEDICINE_TYPE_ID, g.IMP_PRICE }).ToList(); 
                foreach (var group in Groups)
                {
                    List<V_HIS_EXP_MEST_MEDICINE> listSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>(); 
                    if (listSub != null && listSub.Count > 0)
                    {
                        Mrs00090RDO rdo = new Mrs00090RDO(); 
                        rdo.MEDICINE_TYPE_ID = listSub[0].MEDICINE_TYPE_ID; 
                        rdo.MEDICINE_TYPE_CODE = listSub[0].MEDICINE_TYPE_CODE; 
                        rdo.MEDICINE_TYPE_NAME = listSub[0].MEDICINE_TYPE_NAME; 
                        rdo.IMP_PRICE = listSub[0].IMP_PRICE; 
                        rdo.SERVICE_UNIT_NAME = listSub[0].SERVICE_UNIT_NAME; 
                        rdo.AMOUNT = listSub.Sum(s => s.AMOUNT); 
                        GetConcentraByMedicineTypeId(paramGet, rdo, listSub[0].MEDICINE_TYPE_ID); 
                        ListRdo.Add(rdo); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessMobaMedicine(CommonParam paramGet, HisImpMestViewFilterQuery mobaFilter)
        {
            try
            {
                List<V_HIS_IMP_MEST> hisMobaImpMests = new HisImpMestManager(paramGet).GetView(mobaFilter); 
                if (hisMobaImpMests != null && hisMobaImpMests.Count > 0)
                {
                    int start = 0; 
                    int count = hisMobaImpMests.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        List<V_HIS_IMP_MEST> listMoba = hisMobaImpMests.Skip(start).Take(limit).ToList(); 
                        HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery(); 
                        impMediFilter.IMP_MEST_IDs = listMoba.Select(s => s.ID).ToList(); 
                        impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
                        var hisImpMestMedis = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impMediFilter); 
                        if (hisImpMestMedis != null && hisImpMestMedis.Count > 0)
                        {
                            var Groups = hisImpMestMedis.GroupBy(g => new { g.MEDICINE_TYPE_ID, g.IMP_PRICE }).ToList(); 
                            foreach (var group in Groups)
                            {
                                List<V_HIS_IMP_MEST_MEDICINE> listSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>(); 
                                if (listSub != null && listSub.Count > 0)
                                {
                                    Mrs00090RDO rdo = new Mrs00090RDO(); 
                                    rdo.MEDICINE_TYPE_ID = listSub[0].MEDICINE_TYPE_ID; 
                                    rdo.MEDICINE_TYPE_CODE = listSub[0].MEDICINE_TYPE_CODE; 
                                    rdo.MEDICINE_TYPE_NAME = listSub[0].MEDICINE_TYPE_NAME; 
                                    rdo.IMP_PRICE = listSub[0].IMP_PRICE; 
                                    rdo.SERVICE_UNIT_NAME = listSub[0].SERVICE_UNIT_NAME; 
                                    rdo.AMOUNT = listSub.Sum(s => (-s.AMOUNT)); 
                                    GetConcentraByMedicineTypeId(paramGet, rdo, listSub[0].MEDICINE_TYPE_ID); 
                                    ListRdo.Add(rdo); 
                                }
                            }
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void GetConcentraByMedicineTypeId(CommonParam paramGet, Mrs00090RDO rdo, long MedicineTypeId)
        {
            try
            {
                var medicineType = ListMedicineType.SingleOrDefault(s => s.ID == MedicineTypeId); 
                if (medicineType != null)
                {
                    rdo.CONCENTRA = medicineType.CONCENTRA; 
                }
                else
                {
                    medicineType = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).GetViewById(MedicineTypeId); 
                    if (medicineType != null)
                    {
                        rdo.CONCENTRA = medicineType.CONCENTRA; 
                        ListMedicineType.Add(medicineType); 
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Info("Khong lay duoc medicineType bang medicineTypeId." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => MedicineTypeId), MedicineTypeId)); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private List<Mrs00090RDO> ProcessListRdo()
        {
            List<Mrs00090RDO> result = new List<Mrs00090RDO>(); 
            try
            {
                var Groups = ListRdo.GroupBy(g => new { g.MEDICINE_TYPE_ID, g.IMP_PRICE }).ToList(); 
                foreach (var group in Groups)
                {
                    List<Mrs00090RDO> listSub = group.ToList<Mrs00090RDO>(); 
                    if (listSub != null && listSub.Count > 0)
                    {
                        Mrs00090RDO rdo = new Mrs00090RDO(); 
                        rdo.MEDICINE_TYPE_ID = listSub[0].MEDICINE_TYPE_ID; 
                        rdo.MEDICINE_TYPE_CODE = listSub[0].MEDICINE_TYPE_CODE; 
                        rdo.MEDICINE_TYPE_NAME = listSub[0].MEDICINE_TYPE_NAME; 
                        rdo.SERVICE_UNIT_NAME = listSub[0].SERVICE_UNIT_NAME; 
                        rdo.CONCENTRA = listSub[0].CONCENTRA; 
                        rdo.IMP_PRICE = listSub[0].IMP_PRICE; 
                        rdo.AMOUNT = listSub.Sum(s => s.AMOUNT); 
                        result.Add(rdo); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = new List<Mrs00090RDO>(); 
            }
            return result; 
        }

        private void LoadDataToRam()
        {
            try
            {
                HisExpMestViewFilterQuery filter = new HisExpMestViewFilterQuery(); 
                if (castFilter.MEDI_STOCK_IDs != null && castFilter.MEDI_STOCK_IDs.Count > 0)
                {
                    filter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs; 
                }
                else
                {
                    filter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                }
                filter.FINISH_DATE_FROM = castFilter.TIME_FROM; 
                filter.FINISH_DATE_TO = castFilter.TIME_TO;
                if (castFilter.REQ_DEPARTMENT_ID!=null)
                {
                    filter.REQ_DEPARTMENT_ID = castFilter.REQ_DEPARTMENT_ID;
                }
                if (castFilter.REQ_DEPARTMENT_IDs != null)
                {
                    filter.REQ_DEPARTMENT_IDs = castFilter.REQ_DEPARTMENT_IDs;
                }
                filter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE; 
                ListPrescription = new HisExpMestManager().GetView(filter); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListPrescription = new List<V_HIS_EXP_MEST>(); 
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }

                ListRdo = ListRdo.OrderBy(o => o.MEDICINE_TYPE_NAME).ToList(); 
                objectTag.AddObjectData(store, "Report", ListRdo); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
