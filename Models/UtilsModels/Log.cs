using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Text;
using static Models.UtilsModels.LogActionEnum;

namespace Models.UtilsModels
{
    public class Log
    {
        public int ID { get; set; }

        public DateTime Date { get; set; } = new DateTime();

        public eLogAction Action { get; set; } = eLogAction.Get;
        public eLogResult Result { get; set; } = eLogResult.Sucess;

        public int ModelID { get; set; } = 0;
        public int UserID { get; set; } = 0;

        public string ModelName { get; set; } = String.Empty;
        public string AddedMessage { get; set; } = String.Empty; 
        public string ExceptionName { get; set; } = String.Empty;
        public string Message { get; set; } = String.Empty;
        public string StackTrace { get; set; } = String.Empty;


        public override bool Equals(object obj)
        {
            if (!(obj is Log)) return false;

            Log model = obj as Log;

            if (this.ID != model.ID) return false;
            if (this.Date != model.Date) return false;
            if (this.Action != model.Action) return false;
            if (this.Result != model.Result) return false;
            if (this.ModelID != model.ModelID) return false;
            if (this.UserID != model.UserID) return false;
            if (!this.ModelName.Equals(model.ModelName)) return false;
            if (!this.AddedMessage.Equals(model.AddedMessage)) return false;
            if (!this.ExceptionName.Equals(model.ExceptionName)) return false;
            if (!this.Message.Equals(model.Message)) return false;
            if (!this.StackTrace.Equals(model.StackTrace)) return false;

            return true;
        }
    }
}