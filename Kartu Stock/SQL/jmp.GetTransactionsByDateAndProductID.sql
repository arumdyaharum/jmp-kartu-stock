CREATE PROCEDURE GetTransactionsByDateAndProductID
	@StartDate VARCHAR(12),
	@EndDate VARCHAR(12),
	@ProductID VARCHAR(225)
AS
BEGIN
with SalesData as (
    select sales.SalesID, sales.Date as Date, sales.No, sales.CustomerID, customer.Name as CustomerName, salesDetail.ProductID, sum(salesDetail.Quantity) as SalesQuantity
    from [dbo].[Sales] sales
    join [dbo].[SalesDetail] salesDetail on sales.SalesID = salesDetail.SalesID
	left join [dbo].[Customer] customer on sales.CustomerID = customer.CustomerID
    group by sales.SalesID, sales.Date, sales.No, salesDetail.ProductID, sales.CustomerID, customer.Name
),
PurchaseData as (
    select purchase.PurchaseID, purchase.Date as Date, purchase.No, purchase.SupplierID, supplier.Name as SupplierName, purchaseDetail.ProductID, sum(purchaseDetail.Quantity) as PurchaseQuantity
    from [dbo].[Purchase] purchase
    join [dbo].[PurchaseDetail] purchaseDetail on purchase.PurchaseID = purchaseDetail.PurchaseID
	left join [dbo].[Supplier] supplier on purchase.SupplierID = supplier.SupplierID
    group by purchase.PurchaseID, purchase.Date, purchase.No, purchaseDetail.ProductID, purchase.SupplierID, supplier.Name
)
select 
    COALESCE(p.PurchaseID, s.SalesID) as TransactionID,
	COALESCE(p.Date, s.Date) as Date,
    COALESCE(p.No, s.No) as No,
	CASE WHEN p.SupplierID is not null THEN concat('Supplier: ', p.SupplierID, '. ', p.SupplierName)
	WHEN s.CustomerID is not null THEN concat('Pelanggan: ', s.CustomerID, '. ', s.CustomerName)
	ELSE null END as Description,
    SUM(COALESCE(p.PurchaseQuantity, 0)) as PurchaseQuantity,
    SUM(COALESCE(s.SalesQuantity, 0)) as SalesQuantity,
	SUM(COALESCE(p.PurchaseQuantity, s.SalesQuantity, 0)) as Balance
from SalesData s
full outer join PurchaseData p
    on s.Date = p.Date and s.No = p.No
where COALESCE(Convert(varchar, p.Date, 23), Convert(varchar, s.Date, 23)) between @StartDate and @EndDate and COALESCE(p.ProductID, s.ProductID) = @ProductID
group by
	COALESCE(p.PurchaseID, s.SalesID),
	COALESCE(p.Date, s.Date),
	COALESCE(p.No, s.No),
	CASE WHEN p.SupplierID is not null THEN concat('Supplier: ', p.SupplierID, '. ', p.SupplierName)
	WHEN s.CustomerID is not null THEN concat('Customer: ', s.CustomerID, '. ', s.CustomerName)
	ELSE null END
order by Date, No
END;