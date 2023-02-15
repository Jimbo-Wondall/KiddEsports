using Dapper;
using Data_Management.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Data_Management
{
    public class DBS_Builder : DataAccess
    {
        public void DropDatabase()
        {
            //Output connection object to link to the database
            SqlConnection connection = Helper.CreateSQLConnection("Default");

            try
            {
                //Custom connection string to only connect to the server layer of your SQL Database
                string connectionString = $"Data Source={connection.DataSource}; Integrated Security=true";
                //Query to build new Satabase if it does not already exist
                string query =
                    $"IF EXISTS (SELECT 1 FROM sys.databases WHERE name = '{connection.Database}') " +
                    $"DROP DATABASE {connection.Database}";

                using (connection = new SqlConnection(connectionString))
                {
                    //A command object which will send our request to the database
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (connection.State == System.Data.ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public void CreateDatabase()
        {
            //Output connection object to link to the database
            SqlConnection connection = Helper.CreateSQLConnection("Default");

            try
            {
                //Custom connection string to only connect to the server layer of your SQL Database
                string connectionString = $"Data Source={connection.DataSource}; Integrated Security=true";
                //Query to build new Satabase if it does not already exist
                string query =
                    $"IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = '{connection.Database}') " +
                    $"CREATE DATABASE {connection.Database}";

                using (connection = new SqlConnection(connectionString))
                {
                    //A command object which will send our request to the database
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (connection.State == System.Data.ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public bool DoTablesExist()
        {
            using SqlConnection connection = Helper.CreateSQLConnection("Default");
            string query = $"SELECT COUNT(*) FROM {connection.Database}.INFORMATION_SCHEMA.TABLES " +
                $"WHERE TABLE_TYPE = 'BASE TABLE'";
            int count = connection.QuerySingle<int>(query);
            return count > 0;
        }

        /// <summary>
        /// Sends a create table query to the database based on the provided strings
        /// </summary>
        /// <param name="name"></param>
        /// <param name="structure"></param>
        private void CreateTable(string name, string structure)
        {
            try
            {
                string query = $"CREATE TABLE {name} ({structure})";

                using (var connection = Helper.CreateSQLConnection("Default"))
                {
                    connection.Execute(query);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        private void CreateView(string title, string queryContent)
        {
            try
            {
                string query = $"CREATE VIEW [{title}] AS {queryContent}";

                using (var connection = Helper.CreateSQLConnection("Default"))
                {
                    connection.Execute(query);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void BuildDatabaseTables()
        {
            BuildTeamTable();
            BuildEventTable();
            BuildGameTable();
            BuildContactTable();
            BuildResultTable();
            BuildFKGetterQuery();
        }

        public void SeedDatabaseTables()
        {
            SeedTeamTable();
            SeedEventTable();
            SeedGameTable();
            SeedContactTable();
            SeedResultTable();
        }

        private void BuildContactTable()
        {
            string tableName = "Contacts";
            string tableStructure =
                "Id int IDENTITY (1,1) PRIMARY KEY, " +
                "FirstName VARCHAR(50) NOT NULL, " +
                "LastName VARCHAR(50) NOT NULL, " +
                "Phone VARCHAR(50) NOT NULL, " +
                "Email VARCHAR(50) NOT NULL, " +
                "fkTeam_Id int NOT NULL " +

                "FOREIGN KEY(fkTeam_Id) REFERENCES Teams(Id) " +
                "ON UPDATE No Action ON DELETE No Action";
            CreateTable(tableName, tableStructure);
        }
        private void BuildEventTable()
        {
            string tableName = "Events";
            string tableStructure =
                "Id int IDENTITY (1,1) PRIMARY KEY, " +
                "EventName VARCHAR(50) NOT NULL, " +
                "EventDate DATETIME NOT NULL, " +
                "EventLocation VARCHAR(50) NOT NULL";
            CreateTable(tableName, tableStructure);
        }
        private void BuildGameTable()
        {
            string tableName = "Games";
            string tableStructure =
                "Id int IDENTITY (1,1) PRIMARY KEY, " +
                "GameName VARCHAR(50) NOT NULL, " +
                "GameType VARCHAR(50) NOT NULL, " +
                "Description VARCHAR(255)";
            CreateTable(tableName, tableStructure);
        }
        private void BuildResultTable()
        {
            string tableName = "Results";
            string tableStructure =
                "Id int IDENTITY (1,1) PRIMARY KEY, " +
                "ResultValue TINYINT NOT NULL, " +
                "fkGameType_Id int NOT NULL, " +
                "fkTeam1_Id int NOT NULL, " +
                "fkTeam2_Id int NOT NULL, " +
                "fkEvent_Id int NOT NULL " +

                "FOREIGN KEY(fkGameType_Id) REFERENCES Games(Id) " +
                "ON UPDATE No Action ON DELETE No Action, " +
                "FOREIGN KEY(fkTeam1_Id) REFERENCES Teams(Id) " +
                "ON UPDATE No Action ON DELETE No Action, " +
                "FOREIGN KEY(fkTeam2_Id) REFERENCES Teams(Id) " +
                "ON UPDATE No Action ON DELETE No Action, " +
                "FOREIGN KEY(fkEvent_Id) REFERENCES Events(Id) " +
                "ON UPDATE No Action ON DELETE No Action";
            CreateTable(tableName, tableStructure);
        }
        private void BuildTeamTable()
        {
            string tableName = "Teams";
            string tableStructure =
                "Id int IDENTITY (1,1) PRIMARY KEY, " +
                "TeamName VARCHAR(50) NOT NULL, " +
                "Points int NOT NULL";
            CreateTable(tableName, tableStructure);
        }
        private void BuildFKGetterQuery()
        {
            string queryTitle = "Foreign Keys";
            string queryContent = 
                "SELECT f.name AS foreign_key_name, " +
                "OBJECT_NAME(f.parent_object_id) AS TableName, " +
                "COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName, " +
                "OBJECT_NAME(f.referenced_object_id) AS ReferencedObject, " +
                "COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferencedColumnName " +
                "FROM sys.foreign_keys AS f " +
                "INNER JOIN sys.foreign_key_columns AS fc " +
                "ON f.object_id = fc.constraint_object_id";
            CreateView(queryTitle, queryContent);
        }

        private void SeedTeamTable()
        {
            List<Team> teams = new List<Team>
            {
                new Team
                {
                    TeamName = "Ravens"
                },
                new Team
                {
                    TeamName = "Ellen Degenerates"
                },
                new Team
                {
                    TeamName = "Berserkers"
                },
                new Team
                {
                    TeamName = "Band of the hawk"
                },
                new Team
                {
                    TeamName = "FaZe Clan"
                },
                new Team
                {
                    TeamName = "Team Liquid"
                },
                new Team
                {
                    TeamName = "Cloud9"
                },
                new Team
                {
                    TeamName = "100 Thieves"
                },
                new Team
                {
                    TeamName = "Fnatic"
                },
                new Team
                {
                    TeamName = "G2 Esports"
                },
                new Team
                {
                    TeamName = "Natus Vincere"
                },
                new Team
                {
                    TeamName = "OpTic Gaming"
                },
                new Team
                {
                    TeamName = "Evil Geniuses"
                }
            };
            foreach (var team in teams)
            {
                AddEntry(team);
            }
        }
        private void SeedEventTable()
        {
            List<Event> events = new List<Event>
            {
                new Event
                {
                     EventName = "BiggEsportsTourney",
                     EventDate = new DateTime(2020,10,10),
                     EventLocation = "Brisbane"
                },
                new Event
                {
                     EventName = "Parkinsons Charity Event",
                     EventDate = new DateTime(2020,9,9),
                     EventLocation = "Brisbane"
                },
                new Event
                {
                    EventName = "A-NotherESports",
                    EventDate = new DateTime(2020,8,8),
                    EventLocation = "Brisbane"
                },
                new Event
                {
                    EventName = "Blockbuster Videogame Tournament",
                    EventDate = new DateTime(1993,7,7),
                    EventLocation = "USA"
                }
            };
            foreach (var anEvent in events)
            {
                AddEntry(anEvent);
            }
        }
        private void SeedContactTable()
        {
            List<Contact> contacts = new List<Contact>
            {
                new Contact
                {
                    FirstName = "Alika",
                    LastName = "Gibson",
                    Phone = "0486932511",
                    Email = "vulputate.ullamcorper.magna@outlook.org",
                    fkTeam_Id = 1
                },
                new Contact
                {
                    FirstName = "Keane",
                    LastName = "Gonzalez",
                    Phone = "0471304768",
                    Email = "nulla.interdum@outlook.com",
                    fkTeam_Id = 2
                },
                new Contact
                {
                    FirstName = "Ronan",
                    LastName = "Stein",
                    Phone = "0429964433",
                    Email = "cras.sed@icloud.net",
                    fkTeam_Id = 3
                },
                new Contact
                {
                    FirstName = "Lisandra",
                    LastName = "Webb",
                    Phone = "0474301508",
                    Email = "in.condimentum@hotmail.edu",
                    fkTeam_Id = 4
                },
                new Contact
                {
                    FirstName = "Willa",
                    LastName = "Cotton",
                    Phone = "0469474652",
                    Email = "ante.dictum@yahoo.org",
                    fkTeam_Id = 5
                },
                new Contact
                {
                    FirstName = "Abbot",
                    LastName = "Morse",
                    Phone = "0411227461",
                    Email = "vestibulum.nec.euismod@outlook.org",
                    fkTeam_Id = 6
                },
                new Contact
                {
                    FirstName = "Kerry",
                    LastName = "Wolfe",
                    Phone = "0484495398",
                    Email = "quam@icloud.couk",
                    fkTeam_Id = 7
                },
                new Contact
                {
                    FirstName = "Tanner",
                    LastName = "Mccarthy",
                    Phone = "0411444230",
                    Email = "convallis@icloud.edu",
                    fkTeam_Id = 8
                },
                new Contact
                {
                    FirstName = "Katell",
                    LastName = "Mccall",
                    Phone = "0417369768",
                    Email = "augue.scelerisque@icloud.ca",
                    fkTeam_Id = 9
                }
            };
            foreach (Contact contact in contacts)
            {
                AddEntry(contact);
            }
        }
        private void SeedGameTable()
        {
            List<Game> games = new List<Game>
            {
                new Game
                {
                    GameName = "Team Fortress 2",
                    GameType = "Capture the flag",
                    Description = "Two teams compete to steal the enemies intel back to their own base"
                },
                new Game
                {
                    GameName = "Team Fortress 2",
                    GameType = "Control Points",
                    Description = "Red team must capture all control points within the time limit, blue team defends"
                },
                new Game
                {
                    GameName = "Overwatch",
                    GameType = "Team Deathmatch",
                    Description = "Two teams compete to get the most amount of points within the time limit"
                },
                new Game
                {
                    GameName = "Mario Kart",
                    GameType = "Race",
                    Description = "Teams compete to get the fastest time"
                }
            };

            foreach (var game in games)
            {
                AddEntry(game);
            }
        }
        private void SeedResultTable()
        {
            List<Result> results = new List<Result>
            {
                new Result
                {
                    fkEvent_Id = 1,
                    fkGameType_Id = 4,
                    fkTeam1_Id = 1,
                    fkTeam2_Id = 11,
                    ResultValue = 2
                },
                new Result
                {
                    fkEvent_Id = 2,
                    fkGameType_Id = 3,
                    fkTeam1_Id =  7,
                    fkTeam2_Id = 10,
                    ResultValue = 0
                },
                new Result
                {
                    fkEvent_Id = 3,
                    fkGameType_Id = 2,
                    fkTeam1_Id = 1,
                    fkTeam2_Id = 2,
                    ResultValue = 0
                },
                new Result
                {
                    fkEvent_Id = 4,
                    fkGameType_Id = 1,
                    fkTeam1_Id = 12,
                    fkTeam2_Id = 8,
                    ResultValue = 1
                },
                new Result
                {
                    fkEvent_Id = 1,
                    fkGameType_Id = 4,
                    fkTeam1_Id = 10,
                    fkTeam2_Id = 6,
                    ResultValue = 2
                },
                new Result
                {
                    fkEvent_Id = 2,
                    fkGameType_Id = 3,
                    fkTeam1_Id = 8,
                    fkTeam2_Id = 4,
                    ResultValue = 0
                },
                new Result
                {
                    fkEvent_Id = 3,
                    fkGameType_Id = 2,
                    fkTeam1_Id = 4,
                    fkTeam2_Id = 1,
                    ResultValue = 0
                },
                new Result
                {
                    fkEvent_Id = 4,
                    fkGameType_Id = 1,
                    fkTeam1_Id = 10,
                    fkTeam2_Id = 12,
                    ResultValue = 1
                },
                new Result
                {
                    fkEvent_Id = 1,
                    fkGameType_Id = 4,
                    fkTeam1_Id = 5,
                    fkTeam2_Id = 12,
                    ResultValue = 2
                },
                new Result
                {
                    fkEvent_Id = 2,
                    fkGameType_Id = 3,
                    fkTeam1_Id = 4,
                    fkTeam2_Id = 7,
                    ResultValue = 0
                },
                new Result
                {
                    fkEvent_Id = 3,
                    fkGameType_Id = 2,
                    fkTeam1_Id = 8,
                    fkTeam2_Id = 7,
                    ResultValue = 0
                },
                new Result
                {
                    fkEvent_Id = 4,
                    fkGameType_Id = 1,
                    fkTeam1_Id = 13,
                    fkTeam2_Id = 10,
                    ResultValue = 1
                }
            };

            foreach (Result result in results)
            {
                NewResultTransaction(result);
            }
        }
    }
}