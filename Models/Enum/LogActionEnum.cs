using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.UtilsModels
{
    public class LogActionEnum
    {
        public enum eLogAction
        {

            [Description( "Specific")]
            Specific = -1,
            [Description( "GetAll")]
            GetAll = 0,
            [Description( "Get")]
            Get = 1,
            [Description( "Create")]
            Create = 2,
            [Description( "Update")]
            Update = 3,
            [Description( "Delete")]
            Delete = 4
        }

        public enum eLogResult
        {
            [Description("Success")]
            Sucess = 0,
            [Description("Error")]
            Error = 1

        }
    }
}
