using System;
using System.Collections.Generic;
using System.Text;

namespace ecloudvalley.Domain.Bill.Dtos
{
    public class UnblendedCostDto
    {
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 未混和成本-加總
        /// </summary>
        public decimal TotalUnblendedCost { get; set; }
    }
}
