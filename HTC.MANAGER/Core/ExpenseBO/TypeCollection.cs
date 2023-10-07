using HTC.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace HTC.MANAGER.Core.ExpenseBO
{
    partial class TypeCollection
    {
        internal static readonly List<Type> HtcExpense = new List<Type>() { typeof(HTC_EXPENSE), typeof(List<HTC_EXPENSE>) };
        internal static readonly List<Type> HtcExpenseType = new List<Type>() { typeof(HTC_EXPENSE_TYPE), typeof(List<HTC_EXPENSE_TYPE>) };
    }
}
