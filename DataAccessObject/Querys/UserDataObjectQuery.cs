using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessObject.Querys
{
    public class UserDataObjectQuery
    {
        public static string Select = @"
            SELECT 
                [User].[ID]
                ,[User].[UserName]
                ,[User].[Password]
                ,[User].[Role]
            FROM
                [User]
            WHERE
                [User].[Deleted] = 0
        ";

        public static string Insert = @"
            INSERT INTO [User] (
                [User].[UserName]
                ,[User].[Password]
                ,[User].[Role]
            ) OUTPUT Inserted.ID VALUES (
                @UserName
                ,@Password
                ,@Role
            )
        ";

        public static string Update = @"
            UPDATE [User] SET
                [User].[UserName] = @Password
                ,[User].[Password] = @UserName
                ,[User].[Role] = @Role
        ";

        public static string Delete = @"
            UPDATE [User] SET
                [Deleted] = 1
        ";
    }
}
