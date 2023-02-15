using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Dapper;
using Data_Management.Models;

namespace Data_Management
{
    /// <summary>
    /// This class contains a collection of methods that facilitate the 
    /// connection to a database throught the use of queries
    /// </summary>
    public class DataAccess
    {
        public List<FormatCase> FormatCases = new List<FormatCase>
            {
                new FormatCase
                {
                    CaseTable = "dbo.Results",
                    CaseRawColumn = "ResultValue",
                    CaseViewColumn = "Result",
                    Cases = new string[]
                    {
                        "0 THEN 'Draw'",
                        "1 THEN 'Team 1 Won'",
                        "2 THEN 'Team 2 Won'"
                    }
                }
            };

        #region Query Builders

        #region Create
        public string GetAddQuery<TRaw>()
        {
            string tableName = typeof(TRaw).Name + "s";
            string[] properties = Array.ConvertAll(typeof(TRaw).GetProperties(), x => x.Name);

            string insertProperties = "";
            for (int i = 1; i < properties.Length; i++)
            {
                insertProperties += properties[i];
                if (i != properties.Length - 1) insertProperties += ", ";
            }

            string valueProperties = "";
            for (int i = 1; i < properties.Length; i++)
            {
                valueProperties += "@" + properties[i];
                if (i != properties.Length - 1) valueProperties += ", ";
            }
            return
                $"INSERT INTO {tableName} ({insertProperties}) " +
                $"VALUES ({valueProperties})";

        }
        public void AddEntry<TRaw>(TRaw input)
        {
            Execute(GetAddQuery<TRaw>(), input);
        }
        #endregion

        #region Read

        public TRaw GetEntryById<TRaw>(int id)
        {
            try
            {
                using (var connection = Helper.CreateSQLConnection("Default"))
                {
                    string query = $"SELECT * FROM {typeof(TRaw).Name}s WHERE Id = {id}";

                    return connection.QuerySingle<TRaw>(query);
                }
            }
            catch (Exception e)
            {
                return default;
            }
        }

        public List<TRaw> GetEntries<TRaw>()
        {
            string query = $"SELECT * FROM {typeof(TRaw).Name}s";
            return Query<TRaw>(query);
        }

        public List<TView> GetEntries<TRaw, TView>()
        {
            string tableName = typeof(TRaw).Name + "s";
            string[] rawProperties = Array.ConvertAll(typeof(TRaw).GetProperties(), x => x.Name);
            string[] viewProperties = Array.ConvertAll(typeof(TView).GetProperties(), x => x.Name);

            string relationshipQuery =
                "SELECT ColumnName, ReferencedObject, ReferencedColumnName " +
                "FROM dbo.[Foreign Keys] " +
                $"WHERE(TableName = '{tableName}')";
            List<Relationship> relations = Query<Relationship>(relationshipQuery);

            string tableName2 = "dbo." + tableName;
            string select = $"SELECT ";
            string from = $"FROM {tableName2} ";

            for (int i = 0; i < viewProperties.Length; i++)
            {
                Relationship relation = null;
                foreach (var rel in relations)
                {
                    if (rel.ColumnName == rawProperties[i])
                    {
                        relation = rel;
                        // Silly function that removes the 's' from the table name and
                        // slaps on 'Name' to get the column name to reference
                        relation.DesiredColumnName = rel.ReferencedObject.Remove(rel.ReferencedObject.Length - 1, 1) + "Name";
                    }
                }
                FormatCase formatCase = null;
                foreach (var item in FormatCases)
                {
                    if (item.CaseTable == tableName2 &&
                        item.CaseViewColumn == viewProperties[i])
                    {
                        formatCase = item;
                    }
                }

                if (relation != null)
                {
                    select += $" r{i}.{relation.DesiredColumnName} AS {viewProperties[i]}";
                    if (i < viewProperties.Length - 1)
                    {
                        select += ", ";
                    }

                    from += $"INNER JOIN dbo.[{relation.ReferencedObject}] AS r{i} ON r{i}.{relation.ReferencedColumnName} = {tableName2}.{relation.ColumnName} ";
                }
                else if (formatCase != null)
                {
                    select += formatCase.ToString();
                    if (i < viewProperties.Length - 1)
                    {
                        select += ", ";
                    }
                }
                else
                {
                    select += $"{tableName2}.{rawProperties[i]} AS {viewProperties[i]}, ";
                }
            }
            string query = $"{select} {from}";
            return Query<TView>(query);
        }
        #endregion

        #region Update
        public void UpdateEntry<TRaw>(TRaw newEntry)
        {
            Execute(GetUpdateQuery<TRaw>(), newEntry);
        }
        public string GetUpdateQuery<TRaw>()
        {
            string tableName = typeof(TRaw).Name + "s";
            string[] rawProperties = Array.ConvertAll(typeof(TRaw).GetProperties(), x => x.Name);
            string query = $"UPDATE {tableName} SET ";
            for (int i = 1; i < rawProperties.Length; i++)
            {
                query += $"{rawProperties[i]} = @{rawProperties[i]}";
                query += (i == rawProperties.Length - 1) ? " " : ", ";
            }
            return query += "WHERE Id = @Id";
        }
        #endregion

        #region Delete
        /// <summary>
        /// Attempts to delete the entry that matches the given ID
        /// </summary>
        /// <typeparam name="TRaw"></typeparam>
        /// <param name="id"></param>
        /// <returns>True if the query succeeded, false if it failed</returns>
        public bool DeleteEntry<TRaw>(int id)
        {
            string tableName = typeof(TRaw).Name + "s";
            string query = $"DELETE FROM {tableName} WHERE Id = {id}";
            return Execute(query);
        }
        #endregion

        #region Transactions
        /// <summary>
        /// Sends a query that creates a new result in the database 
        /// and sets the teams ppints accordingly
        /// </summary>
        /// <param name="parameters"></param>
        public void NewResultTransaction(object parameters)
        {
            // Concatenates the regular add query for an entry with queries to
            // update each teams points based on the reult value
            string query =
                GetAddQuery<Result>() +

                " UPDATE Teams " +
                "SET  Points = CASE @ResultValue " +
                "WHEN 0 THEN Points + 1 " +
                "WHEN 1 THEN Points + 2 " +
                "ELSE Points END " +
                "WHERE Id = @fkTeam1_Id; " +

                " UPDATE Teams " +
                "SET  Points = CASE @ResultValue " +
                "WHEN 0 THEN Points + 1 " +
                "WHEN 2 THEN Points + 2 " +
                "ELSE Points END " +
                "WHERE Id = @fkTeam2_Id; ";
            Transaction(query, parameters);
        }

        public void UpdateResultTransaction(Result newResult)
        {
            string query =
                GetUpdateQuery<Result>() +

                " UPDATE Teams " +
                "SET  Points = CASE @ResultValue " +
                "WHEN 0 THEN Points + 1 " +
                "WHEN 1 THEN Points + 2 " +
                "ELSE Points END " +
                "WHERE Id = @fkTeam1_Id; " +

                " UPDATE Teams " +
                "SET  Points = CASE @ResultValue " +
                "WHEN 0 THEN Points + 1 " +
                "WHEN 2 THEN Points + 2 " +
                "ELSE Points END " +
                "WHERE Id = @fkTeam2_Id; ";
            Transaction(query, newResult);
        }

        public void RemoveResultEffects(int id)
        {
            Result existingResult = GetEntryById<Result>(id);
            string preQuery =
                " UPDATE Teams " +
                "SET  Points = CASE @ResultValue " +
                "WHEN 0 THEN Points - 1 " +
                "WHEN 1 THEN Points - 2 " +
                "ELSE Points END " +
                "WHERE Id = @fkTeam1_Id; " +

                " UPDATE Teams " +
                "SET  Points = CASE @ResultValue " +
                "WHEN 0 THEN Points - 1 " +
                "WHEN 2 THEN Points - 2 " +
                "ELSE Points END " +
                "WHERE Id = @fkTeam2_Id; ";
            Transaction(preQuery, existingResult);
        }

        #endregion

        #endregion

        #region Query Executors

        public bool Execute(string query)
        {
            try
            {
                using (var connection = Helper.CreateSQLConnection("Default"))
                {
                    connection.Execute(query);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void Execute(string query, object param)
        {
            try
            {
                using (var connection = Helper.CreateSQLConnection("Default"))
                {
                    connection.Execute(query, param);
                }
            }
            catch (Exception e)
            {
                
            }
        }

        public List<T> Query<T>(string query)
        {
            try
            {
                using (var connection = Helper.CreateSQLConnection("Default"))
                {
                    return connection.Query<T>(query).ToList();
                }
            }
            catch (Exception e)
            {
                return new List<T>();
            }
        }

        public void Transaction(string sql, object parameters)
        {
            using (var connection = Helper.CreateSQLConnection("Default"))
            {
                connection.Open();
                using (SqlTransaction tran = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute(sql, parameters, tran);
                        tran.Commit();
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        throw;
                    }
                    connection.Close();
                }
            }
        }
        #endregion
    }
    public class Relationship
    {
        public string ColumnName { get; set; }
        public string ReferencedObject { get; set; }
        public string ReferencedColumnName { get; set; }
        public string DesiredColumnName { get; set; }
    }
    public class FormatCase
    {
        public string CaseTable { get; set; }
        public string CaseRawColumn { get; set; }
        public string CaseViewColumn { get; set; }
        public string[] Cases { get; set; }

        public override string ToString()
        {
            string outputString = "CASE ";
            for (int i = 0; i < Cases.Length; i++)
            {
                outputString += $"WHEN {CaseTable}.{CaseRawColumn} = {Cases[i]} ";
            }
            return outputString += $"ELSE 'ERROR' END AS {CaseViewColumn} ";
        }
    }
}
