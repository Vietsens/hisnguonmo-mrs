using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00612
{
    public class DateDifference
    {
        // Khởi tạo 1 mảng lưu số ngày của các tháng
        // riêng tháng 2 chưa biết có nhuận hay không nên để -1
        private int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        // ngày bắt đầu
        private DateTime fromDate;

        // ngày kết thúc
        private DateTime toDate;

        private int year;
        private int month;
        private int day;
        private int week;

        // Khởi tạo contructor và gán những giá trị cho ngày bắt đầu và kết thúc.
        public DateDifference(DateTime d1, DateTime d2)
        {
            if (d1 > d2)
            {
                this.fromDate = d2;
                this.toDate = d1;
            }
            else
            {
                this.fromDate = d1;
                this.toDate = d2;
            }

        }

        //Hàm tính số năm, tháng, tuần, ngày
        public void DateDiff()
        {
            // ban đầu gán bằng 0
            int tmp = 0;

            // nếu ngày của tháng bắt đầu lớn hơn ngày của tháng kết thúc
            // tmp gán bằng số ngày của tháng trước tháng kết thúc
            if (this.fromDate.Day > this.toDate.Day)
            {
                tmp = this.monthDay[this.toDate.Month - 2];

            }
            // nếu tmp = -1 (tức tháng 2)
            // xem tháng đó của năm kết thúc có phải là tháng nhuận hay không?
            // sau đó gán tmp bằng số ngày của tháng đó
            if (tmp == -1)
            {
                if (DateTime.IsLeapYear(this.toDate.Year))
                {
                    tmp = 29;
                }
                else
                {
                    tmp = 28;
                }
            }
            // Tính số ngày chưa chia cho tuần
            if (tmp != 0)
            {
                if (this.fromDate.Day > tmp)
                {
                    day = this.toDate.Day;
                }
                else
                {
                    day = (this.toDate.Day + tmp) - this.fromDate.Day;
                }
                tmp = 1; // sau khi tính xong ở đây thì tmp phải nhận giá trị là 1
            }
            else
            {
                day = this.toDate.Day - this.fromDate.Day;
                // ở đây không cần gán lại giá trị tmp vì nó vẫn bằng 0
            }
            // Tính số tuần, và số ngày sau khi chia cho tuần.
            if (day >= 7)
            {
                week = day / 7;
                day = day - week * 7;
            }
            // tính số tháng
            if ((this.fromDate.Month + tmp) > this.toDate.Month)
            {
                this.month = (this.toDate.Month + 12) - (this.fromDate.Month + tmp);
                tmp = 1;
            }
            else
            {
                this.month = (this.toDate.Month) - (this.fromDate.Month + tmp);
                tmp = 0;
            }

            // tính số năm

            this.year = this.toDate.Year - (this.fromDate.Year + tmp);
        }

        public override string ToString()
        {
            //return base.ToString();
            return this.year + " Year(s), " + this.month + " month(s), " + week + "week(s) " + this.day + " day(s)";
        }

        public int Years
        {
            get
            {
                return this.year;
            }
        }

        public int Months
        {
            get
            {
                return this.month;
            }
        }

        public int Weeks
        {
            get
            {
                return this.week;
            }
        }

        public int Days
        {
            get
            {
                return this.day;
            }
        }
    }
}
