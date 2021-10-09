using ecloudvalley.Domain.Common.CQRS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecloudvalley.Domain.Bill.Queries
{
    public class GetUnblendedCostQuery : BaseQuery, IRequest<IDictionary<string, decimal>>
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
