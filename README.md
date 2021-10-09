伊雲谷面試試題:

1. 開發工具: Visual Studio 2019  
   (1) Dapper  
   (2) Swashbuckle: 產生API文件  
   (3) Mediator & CQRS  
2. 程式語言: .NET Core 3.1  
3. 資料庫: SQL Server 2019  
   (1) DB : TestDB  
   (2) File Name : AWS_Bill  
4. API測試方式: 使用 Visual Studio 2019 , 運用 Debug 工具 >> 選擇 IIS Express 後, 可以用 Postman 測試API  
5. API文件網址:  
   (1) https://localhost:44345/swagger  
   (2) https://localhost:44345/redoc/v1  
6. 使用 IIS Express + Postman測試API時, 要將 Postman的 Settings 裡 SSL certificate verification 勾選取消  
7. 專案架構:  
   (1) ecloudvalley.API : API  
   (2) ecloudvalley.Application : 應用層 (Service)  
   (3) ecloudvalley.Domain : 領域層 (資料庫SQL, CQRS)   
   (4) ecloudvalley.Infrastructure : 基礎設施層 (共用的程式, NuGet套件)   
   ![20111997ZUPRgzvmKT](https://user-images.githubusercontent.com/92206048/136656353-62b97834-3900-4c04-bc6e-ff16057440db.png)
