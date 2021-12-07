namespace HrSystem.ViewModels
{
    public class VacatiosVM
    {
        public int id { get; set; }

        public string VacationTitle { get; set; }
      
        public string VacationType { get; set; }
       
    
        public DateTime DateFrom { get; set; }
      
        public DateTime DateTo { get; set; }
    
        public string Status { get; set; }
        public string EmployeeName { get; set; }
    }
}
