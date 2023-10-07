using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00725
{
	internal class Mrs00725Filter
	{
		public long TIME_FROM { get; set; }

		public long TIME_TO { get; set; }
        
		public List<long> CURRENTBRANCH_DEPARTMENT_IDs { get; set; }

		public long? SERVICE_REQ_STT_ID { get; set; }

		public List<long> TREATMENT_TYPE_IDs { get; set; } //diện điều trị

        public List<long> EXACT_EXECUTE_ROOM_IDs { get; set; } //phòng thực hiện

        public bool? TAKE_ALL { get; set; }

        public List<long> REQUEST_DEPARTMENT_IDs { get; set; } //khoa chỉ định
        public List<long> PATIENT_TYPE_IDs { get; set; } //đối tượng dịch vụ
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; } //đối tượng bệnh nhân

        public List<long> SERVICE_TYPE_IDs { get; set; } //loại dịch vụ
        public List<long> PARENT_SERVICE_IDs { get; set; } //dịch vụ cha
        public long? INPUT_DATA_ID_TIME_TYPE { get; set; } //loại thời gian: 1.Vào viện; 2. Ra viện; 3. Chỉ định; Mặc định chỉ định
        public List<long> REQUEST_ROOM_IDs { get; set; } //phong chi dinh
        public long? INPUT_DATA_ID_FINANCE_TYPE { get; set; } //Duyệt tài chính: 1.Đã duyệt; 2. Chưa duyệt;
    }
}
