# Welcome to Kartu Stock

By Dyah Achwatiningrum

## Database Configuration

This is my database configuration by default

- Server name = localhost
- Authentication = Windows Authentication
- Encryption = Optional
- Database = jmp

You can change it in `Kartu Stock/appsettings.json`
```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=jmp;Trusted_Connection=True;TrustServerCertificate=True;"
  }
```

## Seeding Data and Store Procedure

Open SQL file in `Kartu Stock/SQL` folder. Run this files on jmp database with this order

1. `initialData.sql` for create tables dan input initial data
2. `jmp.GetTransactionsByDateAndProductID.sql` for create stored procedure

## Open Project and Run it

1. Open Visual Studio and choose `Open a project or solution`
2. Open `Kartu Stock.sln`
3. If you haven't installed EntityFrameworkCore, run this prompt on Package Manager Console

```bash
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
```

4. Go to `Debug > Start Without Debugging`
5. A browser will be opening the project