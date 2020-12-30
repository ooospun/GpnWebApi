# Заметки о работа с базой данных на проекте
## Чтобы сделать скаффолд
   
+ Открой Tools -> NuGet Package Manager -> Package Manager Console

+ Выбери `Default Project` -> `GpnWebApi`

+ Выполни 
Scaffold-DbContext Name=GpnDB Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -ContextDir EF -Context GpnContext -Force

## Чтобы сделать копию бд
+ открыть cmd в каталоге C:\Program Files\PostgreSQL\13\bin>
+ pg_dump -U postgres -W studentdb >studentdb.dmp