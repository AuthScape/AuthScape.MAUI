namespace AuthScapeMAUI.Models
{
    public class SignedInUser
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid? Identifier { get; set; }
        public long? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public long? LocationId { get; set; }
        public string? LocationName { get; set; }
        public string locale { get; set; }
        public List<QueryRole> Roles { get; set; }
        public List<Permission> Permissions { get; set; }
    }

    public class QueryRole
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class Permission
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}