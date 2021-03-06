﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccinctlyExamples
{
    class Exercise6_1
    {        
        public void Menu()
        {
            Console.WriteLine("Menu: ");
            Console.WriteLine("[1] Select/Extract All Person.");
            Console.WriteLine("[2] Add Person.");
            Console.WriteLine("[3] Search Person by LastName.");
            Console.WriteLine("[4] Update Person.");
            Console.WriteLine("[5] Delete Person.");
            Console.WriteLine("[0] Exit the Program.");
            Console.Write("Enter the number you selected: ");
            string sel = Console.ReadLine();
            Console.Clear();

            switch (sel)
            {
                case "1":
                    {
                        Console.WriteLine("You have selected [1] Select/Extract All Person.");
                        ExtractPerson();
                        ReturnMenu();
                        break;
                    }
                case "2":
                    {
                        Person P = new Person();

                        Console.WriteLine("You have selected [2] Add Person.");

                        Console.Write("Enter Last Name: ");
                        P.LastName = Console.ReadLine();

                        do
                        {
                            Console.Write("Enter First Name: ");
                            P.FirstName = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(P.FirstName))
                                Console.WriteLine("First name cannot be empty.");

                        } while (string.IsNullOrWhiteSpace(P.FirstName));


                        Console.Write("Enter Middle Name: ");
                        P.MiddleName = Console.ReadLine();

                        Console.Write("Enter Date of Birth: ");
                        DateTime DOB;
                        bool isValidDOB = DateTime.TryParse(Console.ReadLine(), out DOB);
                        if (isValidDOB)
                            P.DateOfBirth = DOB;
                        else
                        {
                            Console.WriteLine("Invalid Date. It will be set to null.");
                            P.DateOfBirth = null;
                        }

                        Console.Write("Enter Gender (0 = Unknown, 1 = Male, 2 = Female): ");
                        int GenderID;
                        bool isValidGender = int.TryParse(Console.ReadLine(), out GenderID);
                        if (isValidGender)
                            isValidGender = GenderID >= 0 && GenderID < 3 ? true : false;
                        if (isValidGender)
                            P.GenderId = GenderID;
                        else
                        {
                            Console.WriteLine("Invalid Gender. It will be set to null.");
                            P.GenderId = null;
                        }

                        if (InsertPerson(P) >= 0)
                            Console.WriteLine("Insert Person Success.");
                        else
                            Console.WriteLine("Insert Person Failed.");

                        ReturnMenu();

                        break;
                    }
                case "3":
                    {
                        Console.WriteLine("You have selected [3] Search Person by LastName.");
                        Console.Write("Enter Last Name: ");
                        string searchKey = Console.ReadLine();
                        SearchPersonByLastName(searchKey);
                        ReturnMenu();
                        break;
                    }
                case "4":
                    {
                        Console.WriteLine("You have selected [4] Update Person.");
                        List<Person> lPersons = ExtractPerson();
                        Console.WriteLine();

                        int nID;
                        bool isValidID = false;

                        do
                        {
                            Console.Write("Enter the ID of Person you want to update, enter 0 to go back: ");
                            isValidID = int.TryParse(Console.ReadLine(), out nID) && (lPersons.Exists(p => p.ID == nID) || nID == 0)? true : false;
                            if (!isValidID)
                                Console.WriteLine("Invalid ID. ");
                        } while (!isValidID);

                        if (nID == 0)
                        {
                            Console.Clear();
                            Menu();
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Enter new data for this person below.");
                            SearchPersonByID(nID);

                            Person P = new Person();

                            P.ID = nID;

                            Console.Write("Enter Last Name: ");
                            P.LastName = Console.ReadLine();
                            do
                            {
                                Console.Write("Enter First Name: ");
                                P.FirstName = Console.ReadLine();

                                if (string.IsNullOrWhiteSpace(P.FirstName))
                                    Console.WriteLine("First name cannot be empty.");

                            } while (string.IsNullOrWhiteSpace(P.FirstName));


                            Console.Write("Enter Middle Name: ");
                            P.MiddleName = Console.ReadLine();

                            Console.Write("Enter Date of Birth: ");
                            DateTime DOB;
                            bool isValidDOB = DateTime.TryParse(Console.ReadLine(), out DOB);
                            if (isValidDOB)
                                P.DateOfBirth = DOB;
                            else
                            {
                                Console.WriteLine("Invalid Date. It will be set to null.");
                                P.DateOfBirth = null;
                            }

                            Console.Write("Enter Gender (0 = Unknown, 1 = Male, 2 = Female): ");
                            int GenderID;
                            bool isValidGender = int.TryParse(Console.ReadLine(), out GenderID);
                            if (isValidGender)
                                isValidGender = GenderID >= 0 && GenderID < 3 ? true : false;
                            if (isValidGender)
                                P.GenderId = GenderID;
                            else
                            {
                                Console.WriteLine("Invalid Gender. It will be set to null.");
                                P.GenderId = null;
                            }

                            if (UpdatePerson(P) >= 0)
                                Console.WriteLine("Update Person Success.");
                            else
                                Console.WriteLine("Update Person Failed.");
                        }

                        ReturnMenu();
                        break;
                    }
                case "5":
                    {
                        Console.WriteLine("You have selected [5] Delete Person.");
                        List<Person> lPersons = ExtractPerson();
                        Console.WriteLine();

                        int nID;
                        bool isValidID = false;

                        do
                        {
                            Console.Write("Enter the ID of Person you want to delete, enter 0 to go back: ");
                            isValidID = int.TryParse(Console.ReadLine(), out nID) && (lPersons.Exists(p => p.ID == nID) || nID == 0) ? true : false;
                            if (!isValidID)
                                Console.WriteLine("Invalid ID. ");
                        } while (!isValidID);

                        if (nID == 0)
                        {
                            Console.Clear();
                            Menu();
                        }
                        else
                        {
                            Console.WriteLine();

                            if (DeletePerson(nID) >= 0)
                                Console.WriteLine("Delete Person Success.");
                            else
                                Console.WriteLine("Delete Person Failed.");
                        }

                        ReturnMenu();
                            break;
                    }
                case "0":
                    {
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid selection. Please select again.");
                        Menu();
                        break;
                    }
            }
        }
        private void ReturnMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to return to menu.");
            Console.ReadKey();
            Console.Clear();
            Menu();
        }
        private static void DisplayPersonTable(List<Person> lPersons)
        {
            int nMaxLength = 15;
            string strOverflow = "...";
            int nPaddingRight = nMaxLength + strOverflow.Length;
            Console.WriteLine();
            Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(" | ID    | Last Name          | First Name         | Middle Name        | Date of Birth                  | Gender     |");
            Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
           
            foreach (Person p in lPersons)
            {
                Console.WriteLine(" | {0} | {1} | {2} | {3} | {4} | {5} |",
                    p.ID.ToString().PadRight(5, ' '),
                    p.LastName.Length > nMaxLength ? p.LastName.Substring(0, nMaxLength) + strOverflow : p.LastName.PadRight(nPaddingRight, ' '),
                    p.FirstName.Length > nMaxLength ? p.FirstName.Substring(0, nMaxLength) + strOverflow : p.FirstName.PadRight(nPaddingRight, ' '),
                    p.MiddleName.Length > nMaxLength ? p.MiddleName.Substring(0, nMaxLength) + strOverflow : p.MiddleName.PadRight(nPaddingRight, ' '),
                    p.DateOfBirth.Equals(DateTime.MinValue) ? ("").PadRight(30, ' ') : p.DateOfBirth.ToString().PadRight(30, ' '),
                    p.Gender.PadRight(10, ' '));
            }
            Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
            
        }

        //****************************************SQL PROCEDURES
        protected static string connectionString = ConfigurationManager.ConnectionStrings["SuccinctlyDB_WinAuth"]?.ConnectionString;
        protected static string cmdExtractPerson = "SELECT P.ID, P.LastName, P.MiddleName, P.FirstName, P.DateOfBirth, G.Code " +
                                                    "FROM dbo.Person AS P " +
                                                    "LEFT JOIN dbo.Gender AS G " +
                                                    "ON P.GenderID = G.ID " +
                                                    "ORDER BY P.ID ASC;";
        protected static string cmdInsertPerson = "INSERT INTO dbo.Person (LastName, MiddleName, FirstName, DateOfBirth, GenderID) " +
                                                                  "VALUES (@LastName, @MiddleName, @FirstName, @DateOfBirth, @GenderID);";
        protected static string cmdSearchPersonByLastName = "SELECT P.ID, P.LastName, P.MiddleName, P.FirstName, P.DateOfBirth, G.Code " +
                                                    "FROM dbo.Person AS P " +
                                                    "LEFT JOIN dbo.Gender AS G " +
                                                    "ON P.GenderID = G.ID " +
                                                    "WHERE P.LastName LIKE '%' + @LastName + '%'" +
                                                    "ORDER BY P.ID ASC;";
        protected static string cmdSearchPersonByID = "SELECT P.ID, P.LastName, P.MiddleName, P.FirstName, P.DateOfBirth, G.Code " +
                                                    "FROM dbo.Person AS P " +
                                                    "LEFT JOIN dbo.Gender AS G " +
                                                    "ON P.GenderID = G.ID " +
                                                    "WHERE P.ID = @ID " +
                                                    "ORDER BY P.ID ASC;";
        protected static string cmdUpdatePerson = "UPDATE dbo.Person " +
                                                    "SET FirstName = @FirstName, " +
                                                    "MiddleName = @MiddleName, " +
                                                    "LastName = @LastName, " +
                                                    "DateOfBirth = @DateOfBirth, " +
                                                    "GenderID = @GenderID " +
                                                    "WHERE ID = @ID;";
        protected static string cmdDeletePerson = "DELETE FROM dbo.Person WHERE ID = @ID;";

        private static List<Person> ExtractPerson()
        {
            List<Person> lPersons = new List<Person>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(cmdExtractPerson, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lPersons.Add(new Person
                                {
                                    ID = reader.GetInt32(0),
                                    LastName = reader[1] == DBNull.Value ? "" : reader.GetString(1),
                                    MiddleName = reader[2] == DBNull.Value ? "" : reader.GetString(2),
                                    FirstName = reader.GetString(3),
                                    DateOfBirth = reader[4] == DBNull.Value ? DateTime.MinValue : reader.GetDateTime(4),
                                    Gender = reader[5] == DBNull.Value ? "" : reader.GetString(5)
                                }) ;
                            }
                        }
                    }
                }

                DisplayPersonTable(lPersons);

                return lPersons;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                return null;
                //Console.WriteLine("Program will be terminated.");
            }
        }
        private static int InsertPerson(Person person)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(cmdInsertPerson, connection))
                    {
                        command.Parameters.Add("LastName", SqlDbType.VarChar, 256).Value = person.LastName;
                        command.Parameters.Add("FirstName", SqlDbType.VarChar, 256).Value = person.FirstName;
                        command.Parameters.Add("MiddleName", SqlDbType.VarChar, 256).Value = person.MiddleName;
                        command.Parameters.Add("DateOfBirth", SqlDbType.SmallDateTime).Value = person.DateOfBirth.ToDbParameter();
                        command.Parameters.Add("GenderID", SqlDbType.Int).Value = person.GenderId.ToDbParameter();

                        connection.Open();
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                //Console.WriteLine("Program will be terminated.");
                return -1;
            }
        }
        private static void SearchPersonByLastName(string LastName)
        {
            List<Person> lPersons = new List<Person>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(cmdSearchPersonByLastName, connection))
                    {
                        command.Parameters.Add("LastName", SqlDbType.VarChar, 256).Value = LastName;

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lPersons.Add(new Person
                                {
                                    ID = reader.GetInt32(0),
                                    LastName = reader[1] == DBNull.Value ? "" : reader.GetString(1),
                                    MiddleName = reader[2] == DBNull.Value ? "" : reader.GetString(2),
                                    FirstName = reader.GetString(3),
                                    DateOfBirth = reader[4] == DBNull.Value ? DateTime.MinValue : reader.GetDateTime(4),
                                    Gender = reader[5] == DBNull.Value ? "" : reader.GetString(5)
                                });
                            }
                        }
                    }
                }

                DisplayPersonTable(lPersons);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                //Console.WriteLine("Program will be terminated.");
            }
        }
        private static void SearchPersonByID(int ID)
        {
            Person P = new Person();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(cmdSearchPersonByID, connection))
                    {
                        command.Parameters.Add("ID", SqlDbType.Int).Value = ID;

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                P.ID = reader.GetInt32(0);
                                P.LastName = reader[1] == DBNull.Value ? "" : reader.GetString(1);
                                P.MiddleName = reader[2] == DBNull.Value ? "" : reader.GetString(2);
                                P.FirstName = reader.GetString(3);
                                P.DateOfBirth = reader[4] == DBNull.Value ? DateTime.MinValue : reader.GetDateTime(4);
                                P.Gender = reader[5] == DBNull.Value ? "" : reader.GetString(5);
                            }
                        }
                    }
                }

                int nMaxLength = 15;
                string strOverflow = "...";
                int nPaddingRight = nMaxLength + strOverflow.Length;
                Console.WriteLine();
                Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine(" | ID    | Last Name          | First Name         | Middle Name        | Date of Birth                  | Gender     |");
                Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine(" | {0} | {1} | {2} | {3} | {4} | {5} |",
                    P.ID.ToString().PadRight(5, ' '),
                    P.LastName.Length > nMaxLength ? P.LastName.Substring(0, nMaxLength) + strOverflow : P.LastName.PadRight(nPaddingRight, ' '),
                    P.FirstName.Length > nMaxLength ? P.FirstName.Substring(0, nMaxLength) + strOverflow : P.FirstName.PadRight(nPaddingRight, ' '),
                    P.MiddleName.Length > nMaxLength ? P.MiddleName.Substring(0, nMaxLength) + strOverflow : P.MiddleName.PadRight(nPaddingRight, ' '),
                    P.DateOfBirth.ToString().PadRight(30, ' '),
                    P.Gender.PadRight(10, ' '));
                Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                //Console.WriteLine("Program will be terminated.");
            }
        }
        private static int UpdatePerson(Person P)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(cmdUpdatePerson, connection))
                    {
                        command.Parameters.Add("LastName", SqlDbType.VarChar, 256).Value = P.LastName;
                        command.Parameters.Add("FirstName", SqlDbType.VarChar, 256).Value = P.FirstName;
                        command.Parameters.Add("MiddleName", SqlDbType.VarChar, 256).Value = P.MiddleName;
                        command.Parameters.Add("DateOfBirth", SqlDbType.SmallDateTime).Value = P.DateOfBirth.ToDbParameter();
                        command.Parameters.Add("GenderID", SqlDbType.Int).Value = P.GenderId.ToDbParameter();
                        command.Parameters.Add("ID", SqlDbType.Int).Value = P.ID;

                        connection.Open();
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                //Console.WriteLine("Program will be terminated.");
                return -1;
            }
        }
        private static int DeletePerson(int ID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(cmdDeletePerson, connection))
                    {
                        command.Parameters.Add("ID", SqlDbType.Int).Value = ID;

                        connection.Open();
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                //Console.WriteLine("Program will be terminated.");
                return -1;
            }
        }
    }
}
