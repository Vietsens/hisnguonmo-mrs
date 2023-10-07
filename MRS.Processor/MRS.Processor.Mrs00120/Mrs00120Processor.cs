using MOS.MANAGER.HisMilitaryRank;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatient;
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

namespace MRS.Processor.Mrs00120
{
    public class Mrs00120Processor : AbstractProcessor
    {
        Mrs00120Filter castFilter = null; 
        List<Mrs00120RDO> ListRdo = new List<Mrs00120RDO>(); 
        Dictionary<long, V_HIS_PATIENT> dicPatient = new Dictionary<long, V_HIS_PATIENT>(); 
        List<V_HIS_TREATMENT> ListTreatment; 

        public Mrs00120Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00120Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                this.castFilter = (Mrs00120Filter)this.reportFilter; 
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_TREATMENT, MRS00120 Filter. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                HisTreatmentViewFilterQuery treatFilter = new HisTreatmentViewFilterQuery(); 
                treatFilter.CREATE_TIME_FROM = castFilter.TIME_FROM; 
                treatFilter.CREATE_TIME_TO = castFilter.TIME_TO; 
                ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatFilter); 
                var listTreatmentId = ListTreatment.Select(o => o.PATIENT_ID).Distinct().ToList(); 
                if (IsNotNullOrEmpty(listTreatmentId)) dicPatient = GetPatient(listTreatmentId).ToDictionary(o => o.ID); 
                if (!paramGet.HasException)
                {
                    result = true; 
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_TREATMENT MRS00120." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatFilter), treatFilter)); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private List<V_HIS_PATIENT> GetPatient(List<long> Ids)
        {
            List<V_HIS_PATIENT> result = new List<V_HIS_PATIENT>(); 
            try
            {
                var skip = 0; 
                while (Ids.Count - skip > 0)
                {
                    var listIDs = Ids.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisPatientViewFilterQuery filterPatient = new HisPatientViewFilterQuery(); 
                    filterPatient.IDs = listIDs; 
                    var listPatientSub = new MOS.MANAGER.HisPatient.HisPatientManager().GetView(filterPatient); 
                    result.AddRange(listPatientSub); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return new List<V_HIS_PATIENT>(); 
            }
            return result; 
        }

        protected override bool ProcessData()
        {
            bool result = false; 
            try
            {
                ProcessListTreatment(ListTreatment); 
                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private V_HIS_PATIENT pat(V_HIS_TREATMENT o)
        {
            V_HIS_PATIENT result = new V_HIS_PATIENT(); 
            try
            {
                if (dicPatient.ContainsKey(o.PATIENT_ID))
                {
                    result = dicPatient[o.PATIENT_ID]; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return new V_HIS_PATIENT(); 
            }
            return result; 
        }

        private void ProcessListTreatment(List<V_HIS_TREATMENT> ListTreatment)
        {
            try
            {
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    ListTreatment = ListTreatment.Where(o => pat(o).WORK_PLACE_ID.HasValue && pat(o).MILITARY_RANK_ID.HasValue).ToList(); 
                    var Groups = ListTreatment.GroupBy(g => pat(g).WORK_PLACE_ID).ToList(); 
                    foreach (var group in Groups)
                    {
                        var listGroup = group.ToList<V_HIS_TREATMENT>(); 
                        Mrs00120RDO rdo = new Mrs00120RDO(); 
                        rdo.WORK_PLACE_ID = pat(listGroup.First()).WORK_PLACE_ID.Value; 
                        rdo.WORK_PLACE_CODE = pat(listGroup.First()).WORK_PLACE_CODE; 
                        rdo.WORK_PLACE_NAME = pat(listGroup.First()).WORK_PLACE_NAME; 

                        if (IsNotNullOrEmpty(MRS.MANAGER.Config.HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__GENERAL))
                        {
                            var listSub = listGroup.Where(o => MRS.MANAGER.Config.HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__GENERAL.Contains(pat(o).MILITARY_RANK_ID.Value)).ToList(); 
                            rdo.AMOUNT_GENERAL = (IsNotNullOrEmpty(listSub)) ? listSub.Count : 0; 

                        }
                        if (IsNotNullOrEmpty(MRS.MANAGER.Config.HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__3AND4SLASH))
                        {
                            var listSub = listGroup.Where(o => MRS.MANAGER.Config.HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__3AND4SLASH.Contains(pat(o).MILITARY_RANK_ID.Value)).ToList(); 
                            rdo.AMOUNT_3AND4SLASH = (IsNotNullOrEmpty(listSub)) ? listSub.Count : 0; 
                        }
                        if (IsNotNullOrEmpty(MRS.MANAGER.Config.HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__1AND2SLASH))
                        {
                            var listSub = listGroup.Where(o => MRS.MANAGER.Config.HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__1AND2SLASH.Contains(pat(o).MILITARY_RANK_ID.Value)).ToList(); 
                            rdo.AMOUNT_1AND2SLASH = (IsNotNullOrEmpty(listSub)) ? listSub.Count : 0; 
                        }
                        if (IsNotNullOrEmpty(MRS.MANAGER.Config.HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__LIEUTENANT))
                        {
                            var listSub = listGroup.Where(o => MRS.MANAGER.Config.HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__LIEUTENANT.Contains(pat(o).MILITARY_RANK_ID.Value)).ToList(); 
                            rdo.AMOUNT_HSQCS = (IsNotNullOrEmpty(listSub)) ? listSub.Count : 0; 
                        }
                        if (IsNotNullOrEmpty(MRS.MANAGER.Config.HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__HSQCS))
                        {
                            var listSub = listGroup.Where(o => MRS.MANAGER.Config.HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__HSQCS.Contains(pat(o).MILITARY_RANK_ID.Value)).ToList(); 
                            rdo.AMOUNT_HSQCS = listSub.Count; 
                        }
                        ListRdo.Add(rdo); 
                    }
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
                #region Cac the Single
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }
                #endregion
                objectTag.AddObjectData(store, "WorkPlaces", ListRdo); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
