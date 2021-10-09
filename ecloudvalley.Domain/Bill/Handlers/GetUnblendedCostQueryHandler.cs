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
    public class GetUnblendedCostQueryHandler : IRequestHandler<GetUnblendedCostQuery, IDictionary<string, decimal>>
    {
        private static readonly string _testConnectionStrings = ConfigProvider.SQLSettings.TestConnectionStrings;
        private readonly ILogger<GetUnblendedCostQueryHandler> _logger;

        public GetUnblendedCostQueryHandler(ILogger<GetUnblendedCostQueryHandler> logger)
        {
            _logger = logger;
        }

        public Task<IDictionary<string, decimal>> Handle(GetUnblendedCostQuery request, CancellationToken cancellationToken)
        {
            //Query Model
            IDictionary<string, decimal> list = null;
            //List<UnblendedCostDto> results = null;
            //資料-開始筆數
            int RowStart = ((request.CurrentPage -1) * request.PageSize) +1;
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

;WITH CTE_Page AS
(
	SELECT * 
	FROM #TEMP_Data
	WHERE RowSort BETWEEN @RowStart AND @RowEnd
)
SELECT b.[ProductName], SUM(b.UnblendedCost) AS TotalUnblendedCost
FROM dbo.AWS_Bill AS b
INNER JOIN CTE_Page AS t ON t.ProductName=b.ProductName
WHERE UsageAccountId=@UsageAccountId 
GROUP BY b.ProductName

DROP TABLE #TEMP_Data;";
            //參數設定
            var paramMaster = new DynamicParameters();
            paramMaster.Add("@UsageAccountId", request.UsageAccountId);
            paramMaster.Add("@RowStart", RowStart);
            paramMaster.Add("@RowEnd", RowEnd);

            using (SqlConnection conn = new SqlConnection(_testConnectionStrings))
            {

                //results = conn.Query<UnblendedCostDto>(strSql).ToList();
                var MultiResult = conn.QueryMultiple(strSQL, paramMaster);
                int totalCount = MultiResult.Read<int>().FirstOrDefault(); //總筆數
                if (totalCount > 0)
                {
                    list = MultiResult.Read<UnblendedCostDto>().ToDictionary(row => row.ProductName, row => row.TotalUnblendedCost);
                }
            }

            return Task.FromResult(list);
        }
    }
}
