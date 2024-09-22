using Microsoft.EntityFrameworkCore;
using Kartu_Stock.Models;
using Microsoft.Data.SqlClient;

namespace Kartu_Stock.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<TransactionModel> Transactions { get; set; }
        public DbSet<ProductModel> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionModel>()
                .HasKey(s => s.TransactionID);
            modelBuilder.Entity<ProductModel>()
                .HasKey(s => s.ProductID);
        }

        public List<TransactionModel> GetTransactions(string StartDate, string EndDate, string ProductID)
        {
            var StartDateParam = new SqlParameter("@StartDate", StartDate);
            var EndDateParam = new SqlParameter("@EndDate", EndDate);
            var ProductIDParam = new SqlParameter("@ProductID", ProductID);
            return Transactions
                .FromSqlRaw("Exec GetTransactionsByDateAndProductID @StartDate, @EndDate, @ProductID", StartDateParam, EndDateParam, ProductIDParam)
                .AsNoTracking()
                .ToList();
        }

        public List<ProductModel> GetProducts()
        {
            return Products
                .FromSqlRaw("select COALESCE(purchase.ProductID, sales.ProductID) ProductID, product.Name ProductName from [dbo].[PurchaseDetail] purchase full outer join [dbo].[SalesDetail] sales on sales.ProductID = purchase.ProductID left join [dbo].[Product] product on COALESCE(purchase.ProductID, sales.ProductID) = product.ProductID group by COALESCE(purchase.ProductID, sales.ProductID), product.Name;")
                .AsNoTracking()
                .ToList();
        }
    }
}
