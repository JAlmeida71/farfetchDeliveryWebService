using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessObject.Querys
{
    public class ConnectionDataObjectQuery
    {
        public static string Select = @"
            SELECT 
                [Connection].[ID]
                ,[Connection].[StartNode_ID]
                ,[Connection].[EndNode_ID]
                ,[Connection].[Cost]
                ,[Connection].[Time]
            FROM
                [Connection]
            WHERE
                [Connection].[Deleted] = 0
        ";

        public static string Insert = @"
            INSERT INTO [Connection] (
                [Connection].[StartNode_ID]
                ,[Connection].[EndNode_ID]
                ,[Connection].[Cost]
                ,[Connection].[Time]
            ) OUTPUT Inserted.ID VALUES (
                @StartNode_ID    
                ,@EndNode_ID    
                ,@Cost    
                ,@Time   
            )
        ";

        public static string Update = @"
            UPDATE [Connection] SET
                [Connection].[StartNode_ID] = @StartNode_ID
                ,[Connection].[EndNode_ID] = @EndNode_ID
                ,[Connection].[Cost] = @Cost
                ,[Connection].[Time] = @Time
        ";

        public static string Delete = @"
            UPDATE [Connection] SET
                [Connection].[Deleted] = 1
        ";

        public static string Count = @"
            SELECT COUNT(*) AS Counter
            FROM [Connection]
            WHERE [Connection].[Deleted] = 0
        ";
    }
}
