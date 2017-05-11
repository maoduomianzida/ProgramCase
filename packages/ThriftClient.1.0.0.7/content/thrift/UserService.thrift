namespace csharp ThriftService

struct UserInfo
{
	1:i64 UserId;
	2:string UserName;
	3:i32 Age;
	4:bool Sex;
}

service UserService
{
	UserInfo Get(1:i64 userId);
	
	bool AddRange(1:list<UserInfo> userList);

	bool Exists(1:i64 userId);
}