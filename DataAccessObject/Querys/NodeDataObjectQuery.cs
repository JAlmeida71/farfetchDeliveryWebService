using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessObject.Querys
{
    public static class NodeDataObjectQuery 
    {
        public static string Select = @"
            SELECT 
                [Node].[ID],
                [Node].[Name]
            FROM
                [Node]
            WHERE
                [Node].[Deleted] = 0
        ";

        public static string Insert = @"
            INSERT INTO [Node] (
                [Node].[Name]
            ) OUTPUT Inserted.ID VALUES (
                @Name      
            )
        ";

        public static string Update = @"
            UPDATE [Node] SET
                [Node].[Name] = @Name
        ";

        public static string Delete = @"
            UPDATE [Node] SET
                [Deleted] = 1
        ";

    }
}
