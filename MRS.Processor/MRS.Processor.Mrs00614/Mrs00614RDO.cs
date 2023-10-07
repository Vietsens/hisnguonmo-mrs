using Inventec.Common.Repository;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00614
{

    public class Mrs00614RDO : HIS_VITAMIN_A
    {
        public string BRANCH_NAME { get; set; }
        public decimal? T1_AMOUNT { get; set; }
        public decimal? T2_AMOUNT { get; set; }
        public decimal? T3_AMOUNT { get; set; }
        public decimal? Q1_AMOUNT { get; set; }
        public decimal? T4_AMOUNT { get; set; }
        public decimal? T5_AMOUNT { get; set; }
        public decimal? T61_AMOUNT { get; set; }
        public decimal? T62_AMOUNT { get; set; }
        public decimal? T63_AMOUNT { get; set; }
        public decimal? T64_AMOUNT { get; set; }
        public decimal? Q2_AMOUNT { get; set; }
        public decimal? T7_AMOUNT { get; set; }
        public decimal? T8_AMOUNT { get; set; }
        public decimal? T9_AMOUNT { get; set; }
        public decimal? Q3_AMOUNT { get; set; }
        public decimal? T10_AMOUNT { get; set; }
        public decimal? T11_AMOUNT { get; set; }
        public decimal? T121_AMOUNT { get; set; }
        public decimal? T122_AMOUNT { get; set; }
        public decimal? T123_AMOUNT { get; set; }
        public decimal? T124_AMOUNT { get; set; }
        public decimal? Q4_AMOUNT { get; set; }

        public Mrs00614RDO() { }
    }
}
