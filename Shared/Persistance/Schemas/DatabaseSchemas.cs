namespace Shared.Persistance.Schemas;
public static class DatabaseSchemas
{
    public static class UserSchema
    {
        public const string TableName = "Users";
        public const string SchemaName = "Security";

        //properties names
        public const string HouseNo = "HouseNo";
        public const string StreetName = "StreetName";
        public const string City = "City";
        public const string State = "State";
        public const string Country = "Country";
    }

    public static class RoleSchema
    {
        public const string TableName = "Roles";
        public const string SchemaName = "Security";

        //properties names

    }

    public static class UserRoleSchema
    {
        public const string TableName = "UserRoles";
        public const string SchemaName = "Security";

        //properties names

    }
    
    public static class OrganizationSchema
    {
        public const string TableName = "Organizations";
        public const string SchemaName = "Ownership";

        //properties names

    }
    
    public static class BillSchema
    {
        public const string TableName = "Bills";
        public const string SchemaName = "Expenditure";

        //properties names

    }

    public static class BillDetailSchema
    {
        public const string TableName = "BillDetails";
        public const string SchemaName = "Expenditure";

        //properties names

    }
}
