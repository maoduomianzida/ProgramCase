namespace csharp ThriftService

struct OrderInfo
{
	1:i64 OrderID;
	2:string OrderNo;
	3:i32 BuyerID;
	4:i32 Amount;
}

service OrderService
{
	OrderInfo GetOrder(1:i64 orderID);
}