using IdentityServiceAPI.Utils;

namespace IdentityServiceAPI.Models
{
    public class Role
    {
        public int Id { get; set;}
        public string RoleName { get; set; }

        public Role()
        {

        }

        public Role(int i)
        {
            Id = i;

            switch (i)
            {
                case 1:
                    RoleName = RoleType.User;
                    break;
                case 2:
                    RoleName = RoleType.Manager;
                    break;
                case 3:
                    RoleName = RoleType.Admin;
                    break;
                default:
                    RoleName = "Undefined";
                    break;
            }
        }
    }
}
