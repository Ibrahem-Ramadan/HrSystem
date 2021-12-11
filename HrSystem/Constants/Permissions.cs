#nullable disable
namespace HrSystem.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsList()
        {
            return new List<string>()
            {
                "View",    //index 0
                "Add",     //index 1
                "Edit",    //index 2   
                "Delete"   //index 3
            };
        }

         
        public static List<Tuple<string, List<string>>> GenerateAllPermissions()
        {
            var allPermissions = new List<Tuple<string, List<string>>> ();

            var modules = Enum.GetValues(typeof(Modules));

            foreach (var module in modules)
                allPermissions.Add(new Tuple<string, List<string>>(module.ToString(), GeneratePermissionsList()));
             
            return allPermissions;
        }

    }
}
