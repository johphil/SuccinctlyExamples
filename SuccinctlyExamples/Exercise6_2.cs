using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccinctlyExamples
{
    class Exercise6_2
    {
        private DataTable dtPerson()
        {
            DataTable dtP = new DataTable();
            dtP.Columns.Add("LastName", typeof(string));
            dtP.Columns.Add("FirstName", typeof(string));
            dtP.Columns.Add("MiddleName", typeof(string));
            dtP.Columns.Add("DateOfBirth", typeof(DateTime));
            dtP.Columns.Add("GenderID", typeof(int));
            return dtP;
        }
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
                        DisplayPersonTable(GetPeople());

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

                        DataTable dtP = dtPerson();

                        dtP.Rows.Add(new object[] 
                        {
                            P.LastName,
                            P.FirstName,
                            P.MiddleName,
                            P.DateOfBirth,
                            P.GenderId
                        });

                        InsertPerson(dtP);

                        Console.WriteLine("Insert Person Success.");

                        ReturnMenu();

                        break;
                    }
                case "3":
                    {
                        Console.WriteLine("You have selected [3] Search Person by LastName.");
                        Console.Write("Enter Last Name: ");
                        string searchKey = Console.ReadLine();

                        DataTable dtPeople = GetPeople();
                        //DataTable dtPeopleSearch = dtPeople;
                        DataTable dtPeopleSearch = dtPeople.Clone();
                        dtPeopleSearch.Rows.Clear();

                        foreach (DataRow dr in dtPeople.Rows)
                        {
                            if (dr["LastName"].ToString().ToLower().Contains(searchKey.ToLower()))
                            {
                                dtPeopleSearch.ImportRow(dr);
                            }
                        }

                        DisplayPersonTable(dtPeopleSearch);

                        ReturnMenu();
                        break;
                    }
                case "4":
                    {
                        Console.WriteLine("You have selected [4] Update Person.");
                        DataTable dtP = GetPeople();
                        DisplayPersonTable(dtP);
                        Console.WriteLine();

                        int nID;
                        bool isValidID = false;

                        do
                        {
                            Console.Write("Enter the ID of Person you want to update, enter 0 to go back: ");
                            isValidID = int.TryParse(Console.ReadLine(), out nID);
                            if (isValidID)
                            {
                                isValidID = false;
                                foreach (DataRow dr in dtP.Rows)
                                {
                                    if (Convert.ToInt32(dr["ID"].ToString()) == nID)
                                    {
                                        isValidID = true;
                                        break;
                                    }

                                }
                            }
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

                            DataTable dtPeopleSearch = dtP.Clone();
                            dtPeopleSearch.Rows.Clear();

                            foreach (DataRow dr in dtP.Rows)
                            {
                                if (Convert.ToInt32(dr["ID"].ToString()) == nID)
                                {
                                    dtPeopleSearch.ImportRow(dr);
                                }
                            }

                            DisplayPersonTable(dtPeopleSearch);

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
                        DisplayPersonTable(GetPeople());
                        Console.WriteLine();

                        int nID;
                        bool isValidID = false;

                        do
                        {
                            Console.Write("Enter the ID of Person you want to delete, enter 0 to go back: ");
                            isValidID = int.TryParse(Console.ReadLine(), out nID);
                            if (isValidID)
                            {
                                if (nID == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    foreach (DataRow dr in GetPeople().Rows)
                                    {
                                        if (Convert.ToInt32(dr["ID"].ToString()) == nID)
                                        {
                                            if (DeletePerson(nID) >= 0)
                                                Console.WriteLine("Delete Person Success.");
                                            else
                                                Console.WriteLine("Delete Person Failed.");

                                            break;
                                        }
                                    }
                                }
                            }
                            if (!isValidID)
                                Console.WriteLine("Invalid ID. ");
                        } while (!isValidID);

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
        private static void DisplayPersonTable(DataTable dtPeople)
        {
            int nMaxLength = 15;
            string strOverflow = "...";
            int nPaddingRight = nMaxLength + strOverflow.Length;
            Console.WriteLine();
            Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(" | ID    | Last Name          | First Name         | Middle Name        | Date of Birth                  | Gender     |");
            Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
            foreach (DataRow dr in dtPeople.Rows)
            {
                Console.WriteLine(" | {0} | {1} | {2} | {3} | {4} | {5} |",
                    dr["ID"].ToString().PadRight(5, ' '),
                    dr["LastName"].ToString().Length > nMaxLength ? dr["LastName"].ToString().Substring(0, nMaxLength) + strOverflow : dr["LastName"].ToString().PadRight(nPaddingRight, ' '),
                    dr["FirstName"].ToString().Length > nMaxLength ? dr["FirstName"].ToString().Substring(0, nMaxLength) + strOverflow : dr["FirstName"].ToString().PadRight(nPaddingRight, ' '),
                    dr["MiddleName"].ToString().Length > nMaxLength ? dr["MiddleName"].ToString().Substring(0, nMaxLength) + strOverflow : dr["MiddleName"].ToString().PadRight(nPaddingRight, ' '),
                    dr["DateOfBirth"].Equals(DateTime.MinValue) ? ("").PadRight(30, ' ') : dr["DateOfBirth"].ToString().PadRight(30, ' '),
                    dr["Gender"].ToString().PadRight(10, ' '));
            }
            Console.WriteLine(" ----------------------------------------------------------------------------------------------------------------------");
        }


        //****************************************SQL PROCEDURES
        protected static string connectionString = ConfigurationManager.ConnectionStrings["SuccinctlyDB_WinAuth"]?.ConnectionString;
        private static DataTable GetPeople()
        {
            DataTable people = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spGetAllPersons", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(people);
                        }
                    }
                }
                return people;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                return null;
                //Console.WriteLine("Program will be terminated.");
            }
        }
        private static int InsertPerson(DataTable person)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spInsertPerson", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("Person", SqlDbType.Structured).Value = person;

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
        private static int UpdatePerson(Person P)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spUpdatePerson", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("ID", SqlDbType.Int).Value = P.ID;
                        command.Parameters.Add("LastName", SqlDbType.VarChar, 256).Value = P.LastName;
                        command.Parameters.Add("FirstName", SqlDbType.VarChar, 256).Value = P.FirstName;
                        command.Parameters.Add("MiddleName", SqlDbType.VarChar, 256).Value = P.MiddleName;
                        command.Parameters.Add("DateOfBirth", SqlDbType.SmallDateTime).Value = P.DateOfBirth.ToDbParameter();
                        command.Parameters.Add("GenderID", SqlDbType.Int).Value = P.GenderId.ToDbParameter();
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
                    using (SqlCommand command = new SqlCommand("spDeletePerson", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
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
