# ������� � ������ � ����� ������ �� �������
## ����� ������� ��������
   
+ ������ Tools -> NuGet Package Manager -> Package Manager Console

+ ������ `Default Project` -> `GpnWebApi`

+ ������� 
Scaffold-DbContext Name=GpnDB Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -ContextDir EF -Context GpnContext -Force

## ����� ������� ����� ��
+ ������� cmd � �������� C:\Program Files\PostgreSQL\13\bin>
+ pg_dump -U postgres -W studentdb >studentdb.dmp