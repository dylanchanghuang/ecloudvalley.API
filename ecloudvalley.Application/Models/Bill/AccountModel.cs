using System;
using System.Collections.Generic;
using System.Text;

namespace ecloudvalley.Application.Models.Bill
{
    public class AccountModel
    {
        /// <summary>
        /// 使用者帳號Id
        /// </summary>
        public long UsageAccountId { get; set; }
        /// <summary>
        /// 第幾頁 (預設第1頁)
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 每頁幾筆資料 (預設每頁10筆)
        /// </summary>
        public int PageSize { get; set; }
    }
}
