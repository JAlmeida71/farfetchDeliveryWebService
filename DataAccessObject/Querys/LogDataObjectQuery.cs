using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessObject.Querys
{
    public class LogDataObjectQuery
    {
        public static string Insert = @"
            INSERT INTO [_Logs] (
                [_Logs].[Date]
                ,[_Logs].[User_ID]
                ,[_Logs].[Action]
                ,[_Logs].[ModelName]
                ,[_Logs].[Model_ID]
                ,[_Logs].[Result]
                ,[_Logs].[ExceptionName]
                ,[_Logs].[Message]
                ,[_Logs].[StackTrace]
                ,[_Logs].[AddedMessage]
            ) OUTPUT Inserted.ID VALUES (
                @Date      
                ,@User_ID
                ,@Action
                ,@ModelName
                ,@Model_ID
                ,@Result
                ,@ExceptionName
                ,@Message
                ,@StackTrace
                ,@AddedMessage
            )
        ";
    }
}
