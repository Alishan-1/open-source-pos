using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public static class ServiceMessages
    {

        public static string TitleSuccess
        {
            get
            {
                return "Success";
            }
        }

        public static string TitleFailure
        {
            get
            {
                return "Failure";
            }
        }

        public static string Title
        {
            get
            {
                return "Message";
            }
        }

        public static string DataNotFound
        {
            get
            { 
                return "Data not found.";
            }
        }

        public static string DataFound
        {
            get
            {
                return "Data found.";
            }
        }

        public static string DataSaved
        {
            get
            {
                return "Data has been saved.";
            }
        }

        public static string DataNotSaved
        {
            get
            {
                return "Data has not been saved.";
            }
        }

        public static string DataRetrieved
        {
            get
            {
                return "Data has been retrived.";
            }
        }
    }

    public static class ServiceErrorsMessages
    {
        public static string Title
        {
            get
            {
                return "Error";
            }
        }

        public static string DataInvalid
        {
            get
            { 
                return "Data is invalid.";
            }
        }

        public static string InvalidParam
        {
            get
            {
                return "Invalid parameter(s).";
            }
        }

        public static string InvalidJson
        {
            get
            {
                return "Invalid json request.";
            }
        }

        public static string NoRecord
        {
            get
            {
                return "No record found";
            }
        }

        public static string Unknown
        {
            get
            {
                return "Unknown error found.";
            }
        }

        public static string UnknownDb
        {
            get
            {
                return "Unknown database error found.";
            }
        }

    }
}
