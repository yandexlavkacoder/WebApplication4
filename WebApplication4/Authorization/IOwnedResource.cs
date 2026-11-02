namespace WebApplication4.Authorization;

public interface IOwnedResource
{
    int OwnerUserId { get; }
}
