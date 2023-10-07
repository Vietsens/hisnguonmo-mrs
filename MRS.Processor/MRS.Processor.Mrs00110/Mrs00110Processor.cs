using MOS.MANAGER.HisImpMestStt;
using FlexCel.Report; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisExpMest; 
using MOS.MANAGER.HisExpMestMedicine; 
using MOS.MANAGER.HisImpMest; 
using MOS.MANAGER.HisImpMestMedicine; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using Inventec.Common.Repository;
using System.Reflection; 

namespace MRS.Processor.Mrs00110
{
    public class Mrs00110Processor : AbstractProcessor
    {
        Mrs00110Filter castFilter = null; 
        List<Mrs00110RDO> ListRdo = new List<Mrs00110RDO>(); 
        List<Mrs00110RDO> ListExpMestDate = new List<Mrs00110RDO>(); 
        List<V_HIS_EXP_MEST> ListExpMest; 

        public Mrs00110Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00110Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                castFilter = ((Mrs00110Filter)this.reportFilter); 

                CommonParam paramGet = new CommonParam(); 

                HisExpMestViewFilterQuery expFilter = new HisExpMestViewFilterQuery(); 
                expFilter.FINISH_DATE_FROM = castFilter.TIME_FROM; 
                expFilter.FINISH_DATE_TO = castFilter.TIME_TO; 
                expFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE; //
                expFilter.EXP_MEST_TYPE_IDs = new List<long>()
                {
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK
                }; 
                ListExpMest = new HisExpMestManager(paramGet).GetView(expFilter); 

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
                ProcessListData(ListExpMest); 
                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void ProcessListData(List<V_HIS_EXP_MEST> ListExpMest)
        {
            try
            {
                CommonParam paramGet = new CommonParam(); 
                ProcessListDepaExpMest(paramGet, ListExpMest); 
                if (!paramGet.HasException)
                {
                    if (!paramGet.HasException)
                    {
                        ProcessListExpDate(); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdo.Clear(); 
                ListExpMestDate.Clear(); 
            }
        }

        // tong hop du lieu xuat su dung cho khoa phong
        private void ProcessListDepaExpMest(CommonParam paramGet, List<V_HIS_EXP_MEST> ListExpMest)
        {
            try
            {
                if (IsNotNullOrEmpty(ListExpMest))
                {
                    ListExpMest = ListExpMest.OrderBy(o => o.FINISH_TIME).ToList(); 
                    List<Mrs00110RDO> listRdo = new List<Mrs00110RDO>(); 
                    int start = 0; 
                    int count = ListExpMest.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        List<V_HIS_EXP_MEST> hisExpMests = ListExpMest.Skip(start).Take(limit).ToList(); 
                        HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery(); 
                        expMediFilter.EXP_MEST_IDs = hisExpMests.Select(s => s.ID).ToList(); 
                        expMediFilter.IS_EXPORT = true; // IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE; 
                        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new HisExpMestMedicineManager(paramGet).GetView(expMediFilter); 
                        if (!paramGet.HasException)
                        {
                            ProcessListExpMestMedicine(hisExpMests, listExpMestMedicine, listRdo); 
                            ProcessImpMoveBackMedicine(hisExpMests, listRdo); 
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                    if (!paramGet.HasException)
                    {
                        listRdo = listRdo.Where(o => o.TOTAL_AMOUNT > 0).ToList(); 
                        if (IsNotNullOrEmpty(listRdo))
                        {
                            ListRdo.AddRange(listRdo); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessListExpMestMedicine(List<V_HIS_EXP_MEST> hisExpMests, List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine, List<Mrs00110RDO> listRdo)
        {
            try
            {
                if (IsNotNullOrEmpty(listExpMestMedicine))
                {
                    foreach (var item in hisExpMests)
                    {
                        var hisExpMestMedicines = listExpMestMedicine.Where(o => o.EXP_MEST_ID == item.ID).ToList(); 
                        if (IsNotNullOrEmpty(hisExpMestMedicines))
                        {
                            var Groups = hisExpMestMedicines.GroupBy(g => g.MEDICINE_TYPE_ID).ToList(); 
                            foreach (var group in Groups)
                            {
                                List<V_HIS_EXP_MEST_MEDICINE> listSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>(); 
                                Mrs00110RDO rdo = new Mrs00110RDO(listSub); 
                                rdo.SetExpMest(item); 
                                listRdo.Add(rdo); 
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessImpMoveBackMedicine(List<V_HIS_EXP_MEST> hisExpMests, List<Mrs00110RDO> listRdo)
        {
            try
            {
                HisImpMestViewFilterQuery mobaFilter = new HisImpMestViewFilterQuery(); 
                mobaFilter.MOBA_EXP_MEST_IDs = hisExpMests.Select(s => s.ID).ToList(); 
                List<V_HIS_IMP_MEST> hisMobaImpMests = new HisImpMestManager().GetView(mobaFilter); 
                if (IsNotNullOrEmpty(hisMobaImpMests))
                {
                    int start = 0; 
                    int count = hisMobaImpMests.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        List<V_HIS_IMP_MEST> mobaImpMests = hisMobaImpMests.Skip(start).Take(limit).ToList(); 
                        HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery(); 
                        impMediFilter.IMP_MEST_IDs = mobaImpMests.Select(s => s.ID).ToList(); 
                        impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; // Config.HisImpMestSttCFG.IMP_MEST_STT_ID__IMPORTED; 
                        List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicines = new HisImpMestMedicineManager().GetView(impMediFilter); 
                        ProcessDetailImpMobaMedicine(mobaImpMests, hisImpMestMedicines, listRdo); 
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

        //Xu ly chung
        private void ProcessDetailImpMobaMedicine(List<V_HIS_IMP_MEST> mobaImpMests, List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicines, List<Mrs00110RDO> listRdo)
        {
            try
            {
                if (IsNotNullOrEmpty(hisImpMestMedicines))
                {
                    foreach (var moba in mobaImpMests)
                    {
                        var impMestMedis = hisImpMestMedicines.Where(o => o.IMP_MEST_ID == moba.ID).ToList(); 
                        var Groups = impMestMedis.GroupBy(g => g.MEDICINE_TYPE_ID).ToList(); 
                        foreach (var group in Groups)
                        {
                            List<V_HIS_IMP_MEST_MEDICINE> listSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>(); 
                            var rdo = listRdo.FirstOrDefault(f => f.EXP_MEST_ID == moba.ID && f.MEDICINE_TYPE_ID == listSub.First().MEDICINE_TYPE_ID); 
                            if (IsNotNull(rdo))
                            {
                                rdo.TOTAL_AMOUNT = rdo.TOTAL_AMOUNT - listSub.Sum(s => s.AMOUNT); 
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessListExpDate()
        {
            try
            {
                if (IsNotNullOrEmpty(ListRdo))
                {
                    ListExpMestDate = ListRdo.GroupBy(g => g.EXP_DATE).Select(s => new Mrs00110RDO { EXP_DATE = s.First().EXP_DATE, EXP_DATE_STR = s.First().EXP_DATE_STR, TOTAL_AMOUNT = s.Sum(s1 => s1.TOTAL_AMOUNT) }).ToList(); 
                    ListExpMestDate = ListExpMestDate.OrderBy(o => o.EXP_DATE).ToList(); 
                    ListRdo = ListRdo.OrderBy(o => o.EXP_DATE).ThenBy(t => t.DEPARTMENT_ID).ThenBy(b => b.MEDI_STOCK_ID).ThenBy(y => y.EXP_MEST_ID).ToList(); 
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
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }

                objectTag.AddObjectData(store, "ExpMestDates", ListExpMestDate);
                objectTag.AddObjectData(store, "ExpMests", ListRdo);
                objectTag.AddObjectData(store, "TreatmentExpMests", ListRdo.OrderBy(o => o.TDL_PATIENT_FIRST_NAME).ToList());
                objectTag.AddRelationship(store, "ExpMestDates", "ExpMests", "EXP_DATE", "EXP_DATE");
                objectTag.AddRelationship(store, "ExpMestDates", "TreatmentExpMests", "EXP_DATE", "EXP_DATE"); 
                objectTag.SetUserFunction(store, "FuncSameTitleColDepart", new CustomerFuncMergeDepartmentData()); 
                objectTag.SetUserFunction(store, "FuncSameTitleColStock", new CustomerFuncMergeMediStockData());
                objectTag.SetUserFunction(store, "FuncSameTitleColCode", new CustomerFuncMergeExpMestData());
                objectTag.SetUserFunction(store, "FuncRownumberExpMest", new RDOCustomerFuncManyRownumberData());
                objectTag.SetUserFunction(store, "FuncSameTitleColTreatmentCode", new CustomerFuncMergeExpMestData());
                objectTag.SetUserFunction(store, "FuncSameTitleColPatientName", new CustomerFuncMergeExpMestData()); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }

    class CustomerFuncMergeDepartmentData : TFlexCelUserFunction
    {
        long ExpDate; 
        long DepartmentId; 

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 0)
                throw new ArgumentException("Bad parameter count in call to Merge() user-defined function"); 
            bool result = false; 
            try
            {
                long expDate = Convert.ToInt64(parameters[0]); 
                long depaId = Convert.ToInt64(parameters[1]); 

                if (expDate > 0 && depaId > 0)
                {
                    if (DepartmentId == depaId && ExpDate == expDate)
                    {
                        return true; 
                    }
                    else
                    {
                        ExpDate = expDate; 
                        DepartmentId = depaId; 
                        return false; 
                    }
                }
            }
            catch (Exception)
            {
            }
            return result; 
        }
    }

    class CustomerFuncMergeMediStockData : TFlexCelUserFunction
    {
        long ExpDate; 
        long DepartmentId; 
        long MediStock_Id; 
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 0)
                throw new ArgumentException("Bad parameter count in call to Merge() user-defined function"); 

            bool result = false; 
            try
            {
                long exp_date = Convert.ToInt64(parameters[0]); 
                long department_id = Convert.ToInt64(parameters[1]); 
                long medi_stock_id = Convert.ToInt64(parameters[2]); 
                if (exp_date > 0 && department_id > 0 && medi_stock_id > 0)
                {
                    if (ExpDate == exp_date && DepartmentId == department_id && MediStock_Id == medi_stock_id)
                    {
                        return true; 
                    }
                    else
                    {
                        ExpDate = exp_date; 
                        DepartmentId = department_id; 
                        MediStock_Id = medi_stock_id; 
                        return false; 
                    }
                }
            }
            catch (Exception)
            {

            }
            return result; 
        }
    }

    class CustomerFuncMergeExpMestData : TFlexCelUserFunction
    {
        long ExpDate; 
        long DepartmentId; 
        long MediStockId; 
        long ExpMestId; 
        public override object Evaluate(object[] parameters)
        {
            bool result = false; 
            try
            {
                long exp_date = Convert.ToInt64(parameters[0]); 
                long department_id = Convert.ToInt64(parameters[1]); 
                long medi_stock_id = Convert.ToInt64(parameters[2]); 
                long exp_mest_id = Convert.ToInt64(parameters[3]); 
                if (exp_date > 0 && department_id > 0 && medi_stock_id > 0 && exp_mest_id > 0)
                {
                    if (ExpDate == exp_date && DepartmentId == department_id && MediStockId == medi_stock_id && ExpMestId == exp_mest_id)
                    {
                        return true; 
                    }
                    else
                    {
                        ExpDate = exp_date; 
                        DepartmentId = department_id; 
                        MediStockId = medi_stock_id; 
                        ExpMestId = exp_mest_id; 
                        return false; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return result; 
        }
    }

    class RDOCustomerFuncManyRownumberData : TFlexCelUserFunction
    {
        long ExpDate; 
        int num_order = 0; 

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 
            try
            {
                long exp_date = Convert.ToInt64(parameters[0]); 
                if (exp_date > 0)
                {
                    if (ExpDate == exp_date)
                    {
                        num_order += 1; 
                    }
                    else
                    {
                        ExpDate = exp_date; 
                        num_order = 1; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return num_order; 
        }
    }
}
