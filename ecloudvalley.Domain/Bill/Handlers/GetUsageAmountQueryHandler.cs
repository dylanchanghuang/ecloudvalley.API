using Dapper;
using ecloudvalley.Domain.Bill.Dtos;
using ecloudvalley.Domain.Bill.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ecloudvalley.Domain.Bill.Handlers
{
    public class GetUsageAmountQueryHandler : IRequestHandler<GetUsageAmountQuery, IDictionary<string, object>>
    {
        private static readonly string _testConnectionStrings = ConfigProvider.SQLSettings.TestConnectionStrings;
        private readonly ILogger<GetUsageAmountQueryHandler> _logger;

        public GetUsageAmountQueryHandler(ILogger<GetUsageAmountQueryHandler> logger)
        {
            _logger = logger;
        }

        public Task<IDictionary<string, object>> Handle(GetUsageAmountQuery request, CancellationToken cancellationToken)
        {
            //Query Model
            IDictionary<string, object> list = null;
            List<UsageAmountDto> dto = null;
            //資料-開始筆數
            int RowStart = ((request.CurrentPage - 1) * request.PageSize) + 1;
            //資料-結束筆數
            int RowEnd = request.CurrentPage * request.PageSize;
            string strSQL = @"
;WITH CTE AS
(
	SELECT DISTINCT ProductName
	FROM dbo.AWS_Bill
	WHERE UsageAccountId=@UsageAccountId
)
SELECT ROW_NUMBER() OVER (ORDER BY ProductName ASC) AS RowSort, ProductName
INTO #TEMP_Data
FROM CTE

SELECT TOP 1 RowSort AS TotalCount FROM #TEMP_Data ORDER BY RowSort DESC;
SELECT * INTO #TEMP_Page FROM #TEMP_Data WHERE RowSort BETWEEN @RowStart AND @RowEnd;

SELECT t.RowSort, b.ProductName, CONVERT(varchar(10), b.UsageStartDate, 111) AS UsageDate, SUM(b.UsageAmount) AS TotalUsageAmount
FROM dbo.AWS_Bill AS b
INNER JOIN #TEMP_Page AS t ON t.ProductName=b.ProductName
WHERE UsageAccountId=@UsageAccountId 
GROUP BY t.RowSort, b.ProductName, CONVERT(varchar(10), b.UsageStartDate, 111)
ORDER BY t.RowSort, b.ProductName, CONVERT(varchar(10), b.UsageStartDate, 111)

DROP TABLE #TEMP_Data;
DROP TABLE #TEMP_Page;";
            //參數設定
            var paramMaster = new DynamicParameters();
            paramMaster.Add("@UsageAccountId", request.UsageAccountId);
            paramMaster.Add("@RowStart", RowStart);
            paramMaster.Add("@RowEnd", RowEnd);

            using (SqlConnection conn = new SqlConnection(_testConnectionStrings))
            {
                var MultiResult = conn.QueryMultiple(strSQL, paramMaster);
                int totalCount = MultiResult.Read<int>().FirstOrDefault(); //總筆數
                if (totalCount > 0)
                {
                    dto = MultiResult.Read<UsageAmountDto>().ToList();
                    if (dto != null && dto.Count > 0)
                    {
                        list = new Dictionary<string, object>();
                        var nameList = dto.Select(m => new { m.RowSort, m.ProductName }).Distinct();
                        foreach (var item in nameList)
                        {
                            var data = dto.Select(x => new { x.ProductName, x.UsageDate, x.TotalUsageAmount })
                                          .Where(x => x.ProductName == item.ProductName)
                                          .ToDictionary(row => row.UsageDate, row => row.TotalUsageAmount);
                            list.Add(item.ProductName, data);
                        }
                    }
                }
            }

            return Task.FromResult(list);
        }
    }
}
