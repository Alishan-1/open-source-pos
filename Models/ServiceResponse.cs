using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class ServiceResponse
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public bool Flag { get; set; }
        public object Data { get; set; }
        public object LookUp { get; set; }
        public bool IsValid { get; set; }
        public object Errors { get; set; }
        public int StatusCode { get; set; }

    }
}
