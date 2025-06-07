using System.Collections.Generic;

namespace NetProject.ViewModels
{
    public class UserWithRolesViewModel
    {
        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public IList<string> Roles { get; set; } = new List<string>();
        public IList<string> AllRoles { get; set; } = new List<string>();
        // For binding selected roles in POST
        public string SelectedRole { get; set; } = default!;
    }
}