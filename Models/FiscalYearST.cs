using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class FiscalYearST
    {
        
        public int FiscalYearID { get; set; }
        public DateTime FiscalYearFrom { get; set; }
        public DateTime FiscalYearTo { get; set; }
        public char IsCurrentYear { get; set; }
        public int CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public int UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CompanyID { get; set; }
    }
}
