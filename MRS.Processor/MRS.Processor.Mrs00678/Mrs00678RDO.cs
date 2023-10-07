using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using MRS.MANAGER.SarReport.RDO;
using MOS.Filter;
using MOS.MANAGER.HisIcd;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;

namespace MRS.Processor.Mrs00678
{
    class Mrs00678RDO : D_HIS_SERVICE_REQ
    {
        //public static int i = 0;
        V_HIS_EXECUTE_ROOM currentExecuteRoom { get; set; }

        public string VIR_ADDRESS { get; set; }
        public string TRANSFER_IN_MEDI_ORG_CODE { get; set; }//Nơi chuyển bệnh nhân đến

        public string TRANSFER_IN_MEDI_ORG_NAME { get; set; }//Nơi chuyển bệnh nhân đến
        public string PATIENT_TYPE_NAME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }//Phòng thực hiện
        public string IS_BHYT { get; set; }// Là đối tượng bệnh nhân bảo hiểm
        public string IS_THUPHI { get; set; } // là bệnh nhân thu phí
        public string IS_FREE { get; set; } // là bệnh nhân viện phí
        public string IS_NOBHYT { get; set; }// Không là BN bảo hiểm
        //public string FEMALE_YEAR { get; set; }//năm sinh nữ
        //public string MALE_YEAR { get; set; }//năm sinh nam
        public string HEIN_CARD_NUMBER { get; set; }// mã thẻ bh
        public string MEDI_ORG_NAME { get; set; }// nơi khám bệnh ban đầu
        //public string DOB_STR { get; set; }//ngày sinh
        //public int? AGE_MALE { get; set; }//tuổi nam
        //public int? AGE_FEMALE { get; set; }//tuổi nữ
        //public int AGE { get; set; }//tuổi nữ
        public string ICD_NAME_TEXT { get; set; }//Chẩn đoán tuyến dưới
        public string ICD_NAMEs { get; set; }//chẩn đoán chính
        public string ICD_NAME_FULL { get; set; }
        public string ICD_SUB { get; set; }//Chẩn đoán phụ
        public string PATIENT_TYPE_GROUP_SERVICE_TICK { get; set; }//Tích đối tượng dịch vụ
        public string PATIENT_TYPE_GROUP_FREE_TICK { get; set; }//Tích đối tượng miễn phí
        public string PATIENT_TYPE_GROUP_HEIN_TICK { get; set; }//Tích đối tượng bảo hiểm
        public string EXSR_FINISH_TYPE_HOSPITALIZED_IN_TICK { get; set; }//Tích xử lí nhập viện nội trú
        public string EXSR_FINISH_TYPE_HOSPITALIZED_OUT_TICK { get; set; }// tích xử lí điều trị ngoại trú
        public string EXSR_FINISH_TYPE_HOME_TICK { get; set; }//Tích cấp toa cho về
        public string EXSR_FINISH_TYPE_TRANSPORT_TICK { get; set; }//Tích kết thúc chuyển viện
        public string EXSR_FINISH_TYPE_TRANSPORT_UP_TICK { get; set; }//Tích kết thúc chuyển viện tuyến trên
        public string EXSR_FINISH_TYPE_TRANSPORT_DOWN_TICK { get; set; }//Tích kết thúc chuyển viện tuyến dưới
        public string EXSR_FINISH_TYPE_TRANSPORT_EQUAL_TICK { get; set; }//Tích kết thúc chuyển viện cùng tuyến
        public string HAS_MISU_SERVICE_REQ_TICK { get; set; }// tích có làm thủ thuật
        public string MISU_SERVICE_TYPE_NAMEs { get; set; }
        public string IS_SPECIALITY_TICK { get; set; }
        public string IS_EMERGENCY_TICK { get; set; }
        public string IS_MOV_ROOM_TICK { get; set; }
        public int? EXECUTE_TIME { get; set; }
        //public string INTRUCTION_TIME_STR { get; set; }
        public string MEDICINE_TYPE_NAMEs { get; set; }
        public string SERVICE_NAMEs { get; set; }
        public string APPOINTMENT_TICK { get; set; }
        public string DEATH_TICK { get; set; }
       // public string TDL_PATIENT_ETHNIC_NAME { get; set; }


        public V_HIS_TREATMENT_4 treatment { get; set; }
        public Mrs00678RDO(D_HIS_SERVICE_REQ data, V_HIS_TREATMENT_4 treatmentSub, List<HIS_SERE_SERV> listSereServ, List<HIS_DEPARTMENT> listDepartment,Dictionary<long, V_HIS_EXECUTE_ROOM> dicRoom, long patientTypeBhyt, long patientTypeFree, List<HIS_PATIENT_TYPE> listPatientType, List<SALE_MEDICINE> ListSaleExpMestMedicine, Mrs00678Filter filter, List<D_HIS_SERVICE_REQ> listServiceReq, Dictionary<long, HIS_SERVICE> dicService)
        {
            try
            {

                //LogSystem.Info("" + i);
                //i += 1;

                treatment = treatmentSub ?? new V_HIS_TREATMENT_4();
                var sereServ = listSereServ.Where(o => o.TDL_TREATMENT_ID == data.TREATMENT_ID).ToList();
                var serviceReq = listServiceReq.Where(o => o.TREATMENT_ID == data.TREATMENT_ID).ToList();
                var saleMedicine = ListSaleExpMestMedicine.Where(o => o.TREATMENT_ID == data.TREATMENT_ID).ToList();
                if (filter.IS_MERGE_EXAM_ROOM != true)
                {
                    sereServ = sereServ.Where(o => data.EXECUTE_ROOM_ID == o.TDL_REQUEST_ROOM_ID).ToList();
                    saleMedicine = saleMedicine.Where(o => data.EXECUTE_ROOM_ID == o.REQUEST_ROOM_ID).ToList();
                }
                else if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                {
                    sereServ = sereServ.Where(o => IsExam(o.TDL_REQUEST_ROOM_ID, dicRoom)).ToList();
                    saleMedicine = saleMedicine.Where(o => IsExam(o.REQUEST_ROOM_ID, dicRoom)).ToList();
                }
                //System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<D_HIS_SERVICE_REQ>();
                //foreach (var item in pi)
                //{
                //    item.SetValue(this, (item.GetValue(data)));
                //}
                //thêm tên dịch vụ khám
                if (!string.IsNullOrWhiteSpace(data.TDL_SERVICE_IDS))
                {

                    string[] sv = data.TDL_SERVICE_IDS.Split(',');

                    long svId = 0;
                    if (sv.Length == 1)
                    {
                        long.TryParse(sv[0], out svId);
                    }
                    if (dicService.ContainsKey(svId))
                    {
                        this.EXAM_SERVICE_CODE = dicService[svId].SERVICE_CODE;
                        this.EXAM_SERVICE_NAME = dicService[svId].SERVICE_NAME;
                    }
                }
                this.ID = data.ID;
                this.EXECUTE_LOGINNAME = data.EXECUTE_LOGINNAME;
                this.EXECUTE_ROOM_ID = data.EXECUTE_ROOM_ID;
                this.EXECUTE_USERNAME = data.EXECUTE_USERNAME;
                this.FINISH_TIME = data.FINISH_TIME;
                this.ICD_CAUSE_CODE = data.ICD_CAUSE_CODE;
                this.ICD_CAUSE_NAME = data.ICD_CAUSE_NAME;
                this.ICD_CODE = data.ICD_CODE;
                this.ICD_NAME = data.ICD_NAME;
                this.ICD_SUB_CODE = data.ICD_SUB_CODE;
                this.ICD_TEXT = data.ICD_TEXT;
                this.INTRUCTION_DATE = data.INTRUCTION_DATE;
                this.INTRUCTION_TIME = data.INTRUCTION_TIME;
                this.IS_EMERGENCY = data.IS_EMERGENCY;
                this.IS_ELDER = (treatment.TDL_PATIENT_DOB<treatment.IN_TIME-600000000000)?(short)1:(short)0;
                this.IS_TREATIN = (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU||treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY) ? (short)1 : (short)0;
                this.IS_MAIN_EXAM = data.IS_MAIN_EXAM;
                this.REQUEST_DEPARTMENT_ID = data.REQUEST_DEPARTMENT_ID;
                this.REQUEST_LOGINNAME = data.REQUEST_LOGINNAME;
                this.REQUEST_ROOM_ID = data.REQUEST_ROOM_ID;
                this.REQUEST_USERNAME = data.REQUEST_USERNAME;
                this.SERVICE_REQ_CODE = data.SERVICE_REQ_CODE;
                this.SERVICE_REQ_STT_ID = data.SERVICE_REQ_STT_ID;
                this.SERVICE_REQ_TYPE_ID = data.SERVICE_REQ_TYPE_ID;
                this.SESSION_CODE = data.SESSION_CODE;
                this.SICK_DAY = data.SICK_DAY;
                this.START_TIME = data.START_TIME;
                this.TDL_HEIN_CARD_NUMBER = data.TDL_HEIN_CARD_NUMBER;
                this.TDL_HEIN_MEDI_ORG_CODE = data.TDL_HEIN_MEDI_ORG_CODE;
                this.TDL_HEIN_MEDI_ORG_NAME = data.TDL_HEIN_MEDI_ORG_NAME;
                this.TDL_PATIENT_ADDRESS = data.TDL_PATIENT_ADDRESS;
                this.TDL_PATIENT_CAREER_NAME = data.TDL_PATIENT_CAREER_NAME;
                this.TDL_PATIENT_CODE = data.TDL_PATIENT_CODE;
                this.TDL_PATIENT_COMMUNE_CODE = data.TDL_PATIENT_COMMUNE_CODE;
                this.TDL_PATIENT_DISTRICT_CODE = data.TDL_PATIENT_DISTRICT_CODE;
                this.TDL_PATIENT_DOB = data.TDL_PATIENT_DOB;
                this.TDL_PATIENT_GENDER_ID = data.TDL_PATIENT_GENDER_ID;
                this.TDL_PATIENT_ID = data.TDL_PATIENT_ID;
                this.TDL_PATIENT_MILITARY_RANK_NAME = data.TDL_PATIENT_MILITARY_RANK_NAME;
                this.TDL_PATIENT_NAME = data.TDL_PATIENT_NAME;
                this.TDL_PATIENT_ETHNIC_NAME = treatment.TDL_PATIENT_ETHNIC_NAME;
                this.TDL_PATIENT_NATIONAL_NAME = data.TDL_PATIENT_NATIONAL_NAME;
                this.TDL_PATIENT_PROVINCE_CODE = data.TDL_PATIENT_PROVINCE_CODE;
                this.TDL_PATIENT_WORK_PLACE = data.TDL_PATIENT_WORK_PLACE;
                this.TDL_PATIENT_WORK_PLACE_NAME = data.TDL_PATIENT_WORK_PLACE_NAME;
                this.TDL_TREATMENT_CODE = data.TDL_TREATMENT_CODE;
                this.TDL_TREATMENT_TYPE_ID = data.TDL_TREATMENT_TYPE_ID;
                this.TREATMENT_ID = data.TREATMENT_ID;
                this.TREATMENT_TYPE_ID = data.TREATMENT_TYPE_ID;
                this.TDL_PATIENT_PHONE = data.TDL_PATIENT_PHONE;
                this.TDL_PATIENT_CAREER_NAME = data.TDL_PATIENT_CAREER_NAME;                
                this.ICD_NAME_FULL = ProcessIcdSub(data);
                SetExtendField(this, sereServ, saleMedicine, dicRoom, patientTypeBhyt, patientTypeFree, listPatientType);
                //Nếu tích gộp các phòng khám sẽ lấy lượt khám ở phòng xử lí cuối cùng, thông tin bác sĩ khám, ICD và chỉ định điều trị ở các phòng lại với nhau

              //  var department = dicDepartment.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID);
                //if (!string.IsNullOrWhiteSpace(data.END_DEPARTMENT_ID))
                //{

                var department = listDepartment.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID);
                if (department != null)
                {
                    END_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                    END_DEPARTMENT_ID = department.ID;
                }
               // }
               

                if (filter.IS_MERGE_EXAM_ROOM == true)
                {
                    if (data.SERVICE_REQ_STT_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT)
                    {
                        data.IS_DELETE = 1;
                        return;
                    }
                    if (treatment.IN_ROOM_ID != data.EXECUTE_ROOM_ID && (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU))
                    {
                        data.IS_DELETE = 1;
                        return;
                    }
                    if (treatment.END_ROOM_ID != data.EXECUTE_ROOM_ID && treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && treatment.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        data.IS_DELETE = 1;
                        return;
                    }
                    data.ICD_CODE = string.Join("/", serviceReq.Where(o => o.TREATMENT_ID == data.TREATMENT_ID).Select(p => (p.ICD_CODE ?? "") + ": " + (p.ICD_NAME ?? "")).Distinct().ToList());
                    data.ICD_NAME = "";
                    data.ICD_SUB_CODE = string.Join(";", serviceReq.Where(o => o.TREATMENT_ID == data.TREATMENT_ID && o.ICD_SUB_CODE != null).Select(p => (p.ICD_SUB_CODE)).Distinct().ToList());
                    data.ICD_TEXT = string.Join(";", serviceReq.Where(o => o.TREATMENT_ID == data.TREATMENT_ID && o.ICD_TEXT != null).Select(p => (p.ICD_TEXT)).Distinct().ToList());
                    data.EXECUTE_USERNAME = string.Join(";", serviceReq.Where(o => o.TREATMENT_ID == data.TREATMENT_ID && o.EXECUTE_USERNAME != null).Select(p => (p.EXECUTE_USERNAME)).Distinct().ToList());
                    
                    
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        private string ProcessIcdSub(D_HIS_SERVICE_REQ data)
        {
            string icdText = String.Empty;
            
            try
            {
                icdText = (data.ICD_CODE ?? "") + "\n" + (data.ICD_TEXT ?? "");
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return icdText;
        }
        private bool IsExam(long requestRoomId, Dictionary<long, V_HIS_EXECUTE_ROOM> dicRoom)
        {
            return dicRoom.ContainsKey(requestRoomId) && dicRoom[requestRoomId].IS_EXAM == 1;
        }

        private void SetExtendField(D_HIS_SERVICE_REQ data, List<HIS_SERE_SERV> listSereServ, List<SALE_MEDICINE> saleMedicine, Dictionary<long, V_HIS_EXECUTE_ROOM> dicRoom, long patientTypeBhyt, long patientTypeFree, List<HIS_PATIENT_TYPE> listPatientType)
        {
            this.VIR_ADDRESS = this.treatment.TDL_PATIENT_ADDRESS;
            this.TRANSFER_IN_MEDI_ORG_NAME = treatment.TRANSFER_IN_MEDI_ORG_NAME;
            this.TRANSFER_IN_MEDI_ORG_CODE = treatment.TRANSFER_IN_MEDI_ORG_CODE;
            this.PATIENT_TYPE_NAME = (listPatientType.FirstOrDefault(o => o.ID == treatment.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;

            this.TickPatientType(treatment.TDL_PATIENT_TYPE_ID ?? 0, patientTypeBhyt, patientTypeFree);
            ExecuteRoom(data, dicRoom);
            ProcessHeinCard(treatment.TDL_PATIENT_TYPE_ID ?? 0, treatment.TDL_HEIN_CARD_NUMBER, treatment.TDL_HEIN_MEDI_ORG_NAME, patientTypeBhyt, patientTypeFree);
            if (this.currentExecuteRoom != null)
            {

                this.EXECUTE_ROOM_NAME = this.currentExecuteRoom.EXECUTE_ROOM_NAME;
                this.TickEmergency(this.currentExecuteRoom.IS_EMERGENCY);
                this.TickSpeciality(this.currentExecuteRoom.IS_SPECIALITY);
            }

            this.TickTreatmentEndType(treatment);

            long? tranPatiFormId = 0;
            if (this.treatment != null)
            {
                tranPatiFormId = this.treatment.TRAN_PATI_FORM_ID;
            }
            if (treatment.END_ROOM_ID == data.EXECUTE_ROOM_ID)
            {
                this.TickTranPatiForm(tranPatiFormId);
            }
            this.TickTreatmentType(treatment, treatment.TDL_TREATMENT_TYPE_ID);

            ICD_NAME_TEXT = ProcessIcdText(this.treatment);
            ICD_SUB = ProcessIcdSub(this.treatment);
            TRANSFER_IN_ICD_CODE = ProcessTransferInIcdCode(this.treatment);
            TRANSFER_IN_ICD_NAME = ProcessTransferInIcdName(this.treatment);
            ICD_NAMEs = (!String.IsNullOrWhiteSpace(treatment.ICD_NAME) ? treatment.ICD_NAME : treatment.ICD_TEXT);
            
            if (data.START_TIME.HasValue && data.FINISH_TIME.HasValue)
            {
                EXECUTE_TIME = Inventec.Common.DateTime.Calculation.DifferenceTime(data.START_TIME.Value, data.FINISH_TIME.Value, Inventec.Common.DateTime.Calculation.UnitDifferenceTime.MINUTE);
            }
            else if (data.FINISH_TIME.HasValue && !data.START_TIME.HasValue)
            {
                LogSystem.Error("Yeu cau kham co thoi gian ket thuc nhung khong co thoi gian bat dau.Id=" + data.ID + ".");
            }

            ProcessSereServ(data, listSereServ);
            THUOC_BAN = ProcessSaleMedicine(saleMedicine);
        }

        private void ExecuteRoom(D_HIS_SERVICE_REQ data, Dictionary<long, V_HIS_EXECUTE_ROOM> dicRoom)
        {
            try
            {
                this.currentExecuteRoom = dicRoom.ContainsKey(data.EXECUTE_ROOM_ID) ? dicRoom[data.EXECUTE_ROOM_ID] : new V_HIS_EXECUTE_ROOM();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }



        private void TickPatientType(long patientTypeId, long patientTypeIdBhyt, long patientTypeFree)
        {
            try
            {
                if (patientTypeId != null)
                {

                    if (patientTypeId == patientTypeIdBhyt)
                    {
                        PATIENT_TYPE_GROUP_HEIN_TICK = RDOConstant.TickSymbol;
                    }
                    else if (patientTypeFree == patientTypeId)
                    {
                        PATIENT_TYPE_GROUP_FREE_TICK = RDOConstant.TickSymbol;
                    }
                    else
                    {
                        PATIENT_TYPE_GROUP_SERVICE_TICK = RDOConstant.TickSymbol;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void TickEmergency(short? isEmergency)
        {
            try
            {
                if (isEmergency.HasValue && isEmergency.Value == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)// IMSys.DbConfig.HIS_RS.HIS_EXECUTE_ROOM.IS_EMERGENCY__TRUE)
                {
                    IS_EMERGENCY_TICK = RDOConstant.TickSymbol;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        private void TickSpeciality(short? isSpeciality)
        {
            try
            {
                if (isSpeciality.HasValue && isSpeciality.Value == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)// IMSys.DbConfig.HIS_RS.HIS_EXECUTE_ROOM.IS_SPECIALITY__TRUE)
                {
                    IS_SPECIALITY_TICK = RDOConstant.TickSymbol;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void TickTreatmentEndType(V_HIS_TREATMENT_4 treatment)
        {
            try
            {
                if (treatment != null)
                {
                    if (IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV == treatment.TREATMENT_END_TYPE_ID)
                    {
                        EXSR_FINISH_TYPE_HOME_TICK = RDOConstant.TickSymbol;
                    }
                    else if (IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__HEN == treatment.TREATMENT_END_TYPE_ID)
                    {
                        APPOINTMENT_TICK = RDOConstant.TickSymbol;
                    }
                    else if (IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET == treatment.TREATMENT_END_TYPE_ID)
                    {
                        DEATH_TICK = RDOConstant.TickSymbol;
                    }
                    else if (IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__KHAC == treatment.TREATMENT_END_TYPE_ID)
                    {
                        OTHER_TICK = RDOConstant.TickSymbol;
                    }
                    else if (IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__TRON == treatment.TREATMENT_END_TYPE_ID)
                    {
                        TRON_TICK = RDOConstant.TickSymbol;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void TickTranPatiForm(long? tranPatiFormId)
        {
            try
            {
                if (tranPatiFormId.HasValue && tranPatiFormId > 0)
                {
                    EXSR_FINISH_TYPE_TRANSPORT_TICK = RDOConstant.TickSymbol;
                    if (MRS.MANAGER.Config.HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__EQUAL == tranPatiFormId.Value)
                    {
                        EXSR_FINISH_TYPE_TRANSPORT_EQUAL_TICK = RDOConstant.TickSymbol;
                    }
                    else if (MRS.MANAGER.Config.HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__UP_DOWN == tranPatiFormId.Value)
                    {
                        EXSR_FINISH_TYPE_TRANSPORT_DOWN_TICK = RDOConstant.TickSymbol;
                    }
                    else if (MRS.MANAGER.Config.HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NEXT == tranPatiFormId.Value || MRS.MANAGER.Config.HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NON_NEXT == tranPatiFormId.Value)
                    {
                        EXSR_FINISH_TYPE_TRANSPORT_UP_TICK = RDOConstant.TickSymbol;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        //benh nhan kham nhieu phong cho nhap vien dtri phong nao thi tick hien thi tai phong do
        private void TickTreatmentType(V_HIS_TREATMENT_4 treatment, long? treatmentTypeId)
        {
            try
            {
                if (treatmentTypeId.HasValue && treatmentTypeId > 0)
                {
                    if (IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU == treatmentTypeId.Value && treatment.CLINICAL_IN_TIME != null && treatment.IN_ROOM_ID == this.currentExecuteRoom.ROOM_ID)
                    {
                        EXSR_FINISH_TYPE_HOSPITALIZED_IN_TICK = RDOConstant.TickSymbol;
                    }
                    else if (IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU == treatmentTypeId.Value && treatment.CLINICAL_IN_TIME != null && treatment.IN_ROOM_ID == this.currentExecuteRoom.ROOM_ID)
                    {
                        EXSR_FINISH_TYPE_HOSPITALIZED_OUT_TICK = RDOConstant.TickSymbol;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private string ProcessIcdText(V_HIS_TREATMENT_4 treatment)
        {
            string icdText = String.Empty;
            try
            {

                icdText = treatment.TRANSFER_IN_ICD_NAME;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }

            return icdText;
        }

        private string ProcessIcdSub(V_HIS_TREATMENT_4 treatment)
        {
            string icdText = String.Empty;
            try
            {
                List<string> Codes = new List<string>();
                if (treatment != null && treatment.ICD_SUB_CODE != null)
                {
                    Codes = treatment.ICD_SUB_CODE.Split(';').Where(o => !string.IsNullOrWhiteSpace(o)).ToList();
                }

                List<string> Names = new List<string>();
                if (treatment != null && treatment.ICD_TEXT != null)
                {
                    Names = treatment.ICD_TEXT.Split(';').Where(o => !string.IsNullOrWhiteSpace(o)).ToList();
                }
                if (Codes.Count == Names.Count)
                {
                    for (int i = 0; i < Codes.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(Codes[i]))
                        {
                            Codes[i] = Codes[i] + ": " + Names[i];
                        }
                    }
                    icdText = string.Join("/", Codes.Distinct().ToList());
                }
                else
                {
                    icdText = (treatment.ICD_SUB_CODE ?? "") + ":" + (treatment.ICD_TEXT ?? "");
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }

            return icdText;
        }

        private string ProcessTransferInIcdName(V_HIS_TREATMENT_4 treatment)
        {
            string icdText = String.Empty;
            try
            {
                icdText = treatment.TRANSFER_IN_ICD_NAME;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }

            return icdText;
        }

        private string ProcessTransferInIcdCode(V_HIS_TREATMENT_4 treatment)
        {
            string icdText = String.Empty;
            try
            {
                icdText = treatment.TRANSFER_IN_ICD_CODE;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }

            return icdText;
        }

        private void ProcessHeinCard(long PatientTypeId, string HeinCardNumber, string HeinMediOrgName, long patientTypeBhyt, long patientTypeFree)
        {
            try
            {
                if (PatientTypeId == patientTypeBhyt)
                {
                    IS_BHYT = "X";

                    if (HeinCardNumber != null)
                    {
                        HEIN_CARD_NUMBER = RDOCommon.GenerateHeinCardSeparate(HeinCardNumber);
                        MEDI_ORG_NAME = HeinMediOrgName;
                    }
                }
                else
                {
                    IS_NOBHYT = "X";
                    if (PatientTypeId == patientTypeFree)
                    {
                        IS_FREE = "X";
                    }
                    else
                    {
                        IS_THUPHI = "X";
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessSereServ(D_HIS_SERVICE_REQ data, List<HIS_SERE_SERV> listSereServ)
        {
            var sereServ = listSereServ.Where(o => o.TDL_TREATMENT_ID == data.TREATMENT_ID).ToList();

            if (sereServ != null)
            {
                var misuSereServ = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).ToList();
                if (misuSereServ.Count > 0)
                {
                    HAS_MISU_SERVICE_REQ_TICK = RDOConstant.TickSymbol;
                    MISU_SERVICE_TYPE_NAMEs = string.Join(";", misuSereServ.GroupBy(q => q.SERVICE_ID).Select(o => o.First().TDL_SERVICE_NAME + ": " + o.Sum(s => s.AMOUNT)).ToList());
                }
                var mediSereServ = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).ToList();
                if (mediSereServ.Count > 0)
                {
                    MEDICINE_TYPE_NAMEs = string.Join(";", mediSereServ.GroupBy(q => q.SERVICE_ID).Select(o => o.First().TDL_SERVICE_NAME + ": " + o.Sum(s => s.AMOUNT) + " ").ToList());
                }
                AN = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__AN);
                CDHA = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA);
                G = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G);
                GPBL = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL);
                THUOC = StrMediMate(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC);
                VT = StrMediMate(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT);
                KHAC = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC);
                MAU = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU);
                NS = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS);
                PHCN = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PHCN);
                PT = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                SA = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA);
                TDCN = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN);
                TT = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                XN = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN);

                var serviceNames = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC && o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).ToList();
                if (serviceNames.Count > 0)
                {
                    SERVICE_NAMEs = string.Join(";", serviceNames.GroupBy(q => q.SERVICE_ID).Select(o => o.First().TDL_SERVICE_NAME + ": " + o.Sum(s => s.AMOUNT)).ToList());
                }
            }
        }

        private string StrService(List<MOS.EFMODEL.DataModels.HIS_SERE_SERV> sereServ, long serviceType)
        {
            string result = "";
            try
            {
                var sss = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == serviceType).ToList();
                if (sss.Count > 0)
                {
                    result = string.Join(";", sss.GroupBy(q => q.SERVICE_ID).Select(o => o.First().TDL_SERVICE_NAME).ToList()) + "\r\n";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string StrMediMate(List<MOS.EFMODEL.DataModels.HIS_SERE_SERV> sereServ, long serviceType)
        {
            string result = "";
            try
            {
                var sss = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == serviceType).ToList();
                if (sss.Count > 0)
                {
                    result = string.Join(";", sss.GroupBy(q => q.SERVICE_ID).Select(o => o.First().TDL_SERVICE_NAME + ": " + o.Sum(s => s.AMOUNT)).ToList()) + "\r\n";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string ProcessSaleMedicine(List<SALE_MEDICINE> saleMedicine)
        {
            string result = "";
            try
            {
                if (saleMedicine != null && saleMedicine.Count > 0)
                {
                    result = string.Join(";", saleMedicine.GroupBy(q => q.MEDICINE_TYPE_NAME).Select(o => o.First().MEDICINE_TYPE_NAME + ": " + o.Sum(s => s.AMOUNT)).ToList()) + "\r\n";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        public string AN { get; set; }

        public string CDHA { get; set; }

        public string G { get; set; }

        public string GPBL { get; set; }

        public string THUOC { get; set; }

        public string VT { get; set; }

        public string KHAC { get; set; }

        public string MAU { get; set; }

        public string NS { get; set; }

        public string PHCN { get; set; }

        public string PT { get; set; }

        public string SA { get; set; }

        public string TDCN { get; set; }

        public string TT { get; set; }

        public string XN { get; set; }

        public string THUOC_BAN { get; set; }

        public string TRANSFER_IN_ICD_NAME { get; set; }

        public string TRANSFER_IN_ICD_CODE { get; set; }

        public string OTHER_TICK { get; set; }

        public string TRON_TICK { get; set; }

        public string EXAM_SERVICE_NAME { get; set; }

        public string EXAM_SERVICE_CODE { get; set; }

        public short? IS_ELDER { get; set; }

        public short? IS_TREATIN { get; set; }

        public string TDL_PATIENT_ETHNIC_NAME { get; set; }
    }
    public class D_HIS_SERVICE_REQ
    {
        public long? END_DEPARTMENT_ID { get; set; }
        public string END_DEPARTMENT_NAME { get; set; }
        public long EXECUTE_DEPARTMENT_ID { get; set; }
        public string EXECUTE_LOGINNAME { get; set; }
        public long EXECUTE_ROOM_ID { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public long? FINISH_TIME { get; set; }
        public string HOSPITALIZATION_REASON { get; set; }
        public string ICD_CAUSE_CODE { get; set; }
        public string ICD_CAUSE_NAME { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_NAME { get; set; }
        public string ICD_SUB_CODE { get; set; }
        public string ICD_TEXT { get; set; }
        public long ID { get; set; }
        public long INTRUCTION_DATE { get; set; }
        public long INTRUCTION_TIME { get; set; }
        public short? IS_ACTIVE { get; set; }
        public short? IS_DELETE { get; set; }
        public short? IS_EMERGENCY { get; set; }
        public short? IS_HOME_PRES { get; set; }
        public short? IS_KIDNEY { get; set; }
        public short? IS_MAIN_EXAM { get; set; }
        public short? IS_NO_EXECUTE { get; set; }
        public long? NUM_ORDER { get; set; }
        public long? PARENT_ID { get; set; }
        public long? PREVIOUS_SERVICE_REQ_ID { get; set; }
        public long? PRIORITY { get; set; }
        public long REQUEST_DEPARTMENT_ID { get; set; }
        public string REQUEST_LOGINNAME { get; set; }
        public long REQUEST_ROOM_ID { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public string SERVICE_REQ_CODE { get; set; }
        public long SERVICE_REQ_STT_ID { get; set; }
        public long SERVICE_REQ_TYPE_ID { get; set; }
        public string SESSION_CODE { get; set; }
        public long? SICK_DAY { get; set; }
        public long? START_TIME { get; set; }
        public string SUBCLINICAL { get; set; }
        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public string TDL_HEIN_MEDI_ORG_CODE { get; set; }
        public string TDL_HEIN_MEDI_ORG_NAME { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }
        public string TDL_PATIENT_PHONE { get; set; }
        public string TDL_PATIENT_CAREER_NAME { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public string TDL_PATIENT_COMMUNE_CODE { get; set; }
        public string TDL_PATIENT_DISTRICT_CODE { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public string TDL_PATIENT_FIRST_NAME { get; set; }
        public long? TDL_PATIENT_GENDER_ID { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public long TDL_PATIENT_ID { get; set; }
        public string TDL_PATIENT_MILITARY_RANK_NAME { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string TDL_PATIENT_NATIONAL_NAME { get; set; }
        public string TDL_PATIENT_PROVINCE_CODE { get; set; }
        public string TDL_PATIENT_WORK_PLACE { get; set; }
        public string TDL_PATIENT_WORK_PLACE_NAME { get; set; }
        public string TDL_TREATMENT_CODE { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public long TREATMENT_ID { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public string TDL_SERVICE_IDS { get; set; }
        
    }
}
