using System;
using System.Collections.Generic;
using System.Text;

namespace ecloudvalley.Domain.Bill.Dtos
{
    public class UsageAmountDto
    {
        /// <summary>
        /// 第幾筆資料
        /// </summary>
        public int RowSort { get; set; }
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 使用日期
        /// </summary>
        public string UsageDate { get; set; }

        /// <summary>
        /// 使用金額-加總
        /// </summary>
        public decimal TotalUsageAmount { get; set; }

    }
}
