using PrettyNeatGenericAPI.Models.DbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrettyNeatGenericAPI.Data.Models
{
    public class Qualifiers : AuditableEntity
    {

        public string? ResponsiblePerson { get; set; } // NAME : B
        public string? DateOfIssue { get; set; }    //DATE : C
        public string? PartNumber { get; set; } //PART NO   : D
        public string? Type { get; set; }   //TYPE + COMMENTS : E+F
        public string? TssCode { get; set; }    // ID + OD + FACE : H + I + J
        public float? ChargePerPiece { get; set; } //TSS RATE : K
        public float? PayPerPiece { get; set; } //INH RATE : L 
        public float? ChargePerPieceDelay { get; set; } // TSS RATE BX2 : M
        public float? PayPerPieceDelay { get; set; } // INH RATE BX2 : N
        public float? ChargePerPieceExtraDelay { get; set; } // TSS RATE BX3 : ?
        public float? PayPerPieceExtraDelay { get; set; } //INH RATE BX3 : ?
        public float? EmployeePiecesPerHour { get; set; } //INH 2023/1000PCS : O 
        public float? ClientPiecesPerHour { get; set; } //TSS 2023/1000PCS : P

    }
}
