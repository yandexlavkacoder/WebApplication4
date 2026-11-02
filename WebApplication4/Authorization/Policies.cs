namespace WebApplication4.Authorization;

public static class Policies
{
    public const string AdminOnly = "AdminOnly";

    public const string CanManageCatalog = "CanManageCatalog";

    public const string OwnerOrAdmin = "OwnerOrAdmin";
}

public static class Roles
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
}
