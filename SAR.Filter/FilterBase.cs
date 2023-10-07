
using System.Collections.Generic;
namespace SAR.Filter
{
    public class FilterBase
    {
        protected static readonly long NEGATIVE_ID = -1;

        /// <summary>
        /// Truong sap xep (mac dinh: MODIFY_TIME)
        /// </summary>
        public string ORDER_FIELD { get; set; }
        /// <summary>
        /// Chieu sap xep (DESC/ASC, mac dinh DESC)
        /// </summary>
        public string ORDER_DIRECTION { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public long? ID { get; set; }
        /// <summary>
        /// Trang thai kich hoat
        /// </summary>
        public short? IS_ACTIVE { get; set; }
        /// <summary>
        /// Thoi gian tao (tu)
        /// </summary>
        public long? CREATE_TIME_FROM { get; set; }
        public long? CREATE_TIME_FROM__GREATER { get; set; }
        /// <summary>
        /// Thoi gian tao (den)
        /// </summary>
        public long? CREATE_TIME_TO { get; set; }
        public long? CREATE_TIME_TO__LESS { get; set; }
        /// <summary>
        /// Thoi gian sua (tu)
        /// </summary>
        public long? MODIFY_TIME_FROM { get; set; }
        public long? MODIFY_TIME_FROM__GREATER { get; set; }
        /// <summary>
        /// Thoi gian sua (den)
        /// </summary>
        public long? MODIFY_TIME_TO { get; set; }
        public long? MODIFY_TIME_TO__LESS { get; set; }
        /// <summary>
        /// Nguoi tao
        /// </summary>
        public string CREATOR { get; set; }
        /// <summary>
        /// Nguoi sua
        /// </summary>
        public string MODIFIER { get; set; }
        /// <summary>
        /// Don vi quan ly
        /// </summary>
        public string GROUP_CODE { get; set; }
        /// <summary>
        /// Tu khoa tim kiem
        /// </summary>
        public string KEY_WORD { get; set; }
        /// <summary>
        /// Danh sach cac Id
        /// </summary>
        public List<long> IDs { get; set; }
    }
}
