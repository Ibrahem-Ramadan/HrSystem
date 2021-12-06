namespace HrSystem.Models
{
    public class Permissions‏
    {
        public int Permissions‏Id { get; set; }
        public string Permissions‏Name { get; set; }

        public string Permissions‏Description { get; set; }

        public virtual ICollection<RolesPermession> RolesPermessions { get; set; }

    }
}
