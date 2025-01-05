namespace PrettyNeatGenericAPI.Data.Models.LocalModels
{
    public class EmployeeWage
    {

        public string Name { get; set; }

        public string Surname { get; set; }

        public string? PunchCode { get; set; }

        public List<string>? AssignedBag { get; set; }
        public string? PartNumber { get; set; }  
        public string? TssCode { get; set; }
        public float? INHRate { get; set; }
        public float? EmployeePayPerPiece { get; set; }
        public int? QuantityIssuedToEmployee { get; set; }
        public float? EmployeeWagePerBag { get; set; }

    }
}
