using System;
using System.ComponentModel.DataAnnotations;

namespace FileProc.Data
{
    public class FileProcData
    {
        public int Id { get; set; }

        public int? CustomerNumber { get; set; }

        [MaxLength(30)]
        public string CustomerName { get; set; }

        [MaxLength(30)]
        public string CustomerSurname { get; set; }

        [MaxLength(30)]
        public string AddressStreet1 { get; set; }

        [MaxLength(30)]
        public string AddressStreet2 { get; set; }

        [MaxLength(30)]
        public string AddressStreet3 { get; set; }

        [MaxLength(10)]
        public string AddressState { get; set; }

        [MaxLength(5)]
        public string AddressPostcode { get; set; }

        [MaxLength(20)]
        public string AccountNumber { get; set; }

        public decimal? AccountBalance { get; set; }

        [MaxLength(10)]
        public string BankNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(255)]
        public string Message { get; set; }

        [MaxLength(20)]
        public string SourceCode { get; set; }
    }
}
