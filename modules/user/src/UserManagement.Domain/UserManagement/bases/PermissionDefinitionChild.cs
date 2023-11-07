namespace UserManagement.Domain.UserManagement.bases
{
    public class PermissionDefinitionChild
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public List<PermissionDefinitionChild> Children { get; set; }
    }
}
