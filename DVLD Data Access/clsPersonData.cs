using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DVLD_Data_Access
{
    public static class clsPersonData
    {
        public static bool GetPersonInfoByID(int PersonID, ref string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref byte Gender, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM people
                             WHERE personid = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            
            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    NationalNo = reader["NationalNo"].ToString();
                    FirstName  = reader["FirstName"].ToString();
                    LastName   = reader["LastName"].ToString();
                    //DateOfBirth = DateTime.Parse(reader["DateOfBirth"].ToString());
                    DateTime.TryParse(reader["DateOfBirth"].ToString(), out DateOfBirth);
                    //Gender = char.Parse(reader["Gender"].ToString());
                    byte.TryParse(reader["Gendor"].ToString(), out Gender);
                    Address = reader["Address"].ToString();
                    Phone = reader["Phone"].ToString();
                    // NationalityCountryID = int.Parse(reader["nationalitycountryid"].ToString());
                    int.TryParse(reader["nationalitycountryid"].ToString(), out NationalityCountryID);

                    if (reader["SecondName"] != System.DBNull.Value)
                        SecondName = reader["SecondName"].ToString();
                    else
                        SecondName = string.Empty;

                    if (reader["ThirdName"] != System.DBNull.Value)
                        ThirdName = reader["ThirdName"].ToString();
                    else
                        ThirdName = string.Empty;

                    if (reader["Email"] != System.DBNull.Value)
                        Email = reader["Email"].ToString();
                    else
                        Email = string.Empty;

                    if (reader["imagePath"] != System.DBNull.Value)
                        ImagePath = reader["imagepath"].ToString();
                    else
                        ImagePath = string.Empty;

                    // mark record as found
                    isFound = true;
                }

                reader.Close();
            }
            catch (Exception)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetPersonInfoByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref byte Gender, ref string Address, ref string 
Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM people
                             WHERE nationalno = @NationalNo;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //PersonID = int.Parse(reader["PersonID"].ToString());
                    int.TryParse(reader["PersonID"].ToString(), out PersonID);
                    FirstName = reader["FirstName"].ToString();
                    SecondName = reader["SecondName"].ToString();
                    ThirdName = reader["ThirdName"].ToString();
                    LastName = reader["LastName"].ToString();
                    //DateOfBirth = DateTime.Parse(reader["DateOfBirth"].ToString());
                    DateTime.TryParse(reader["DateOfBirth"].ToString(), out DateOfBirth);
                    //Gender = char.Parse(reader["Gender"].ToString());
                    byte.TryParse(reader["Gendor"].ToString(), out Gender);
                    Address = reader["Address"].ToString();
                    Phone = reader["Phone"].ToString();
                    Email = reader["Email"].ToString();
                    // NationalityCountryID = int.Parse(reader["nationalitycountryid"].ToString());
                    int.TryParse(reader["nationalitycountryid"].ToString(), out NationalityCountryID);

                    if (reader["SecondName"] != System.DBNull.Value)
                        SecondName = reader["SecondName"].ToString();
                    else
                        SecondName = string.Empty;

                    if (reader["ThirdName"] != System.DBNull.Value)
                        ThirdName = reader["ThirdName"].ToString();
                    else
                        ThirdName = string.Empty;

                    if (reader["Email"] != System.DBNull.Value)
                        Email = reader["Email"].ToString();
                    else
                        Email = string.Empty;
                    
                    if (reader["imagePath"] != System.DBNull.Value)
                        ImagePath = reader["imagepath"].ToString();
                    else
                        ImagePath = string.Empty;

                    // mark record as found
                    isFound = true;
                }

                reader.Close();
            }
            catch (Exception)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        
        public static int AddNewPerson(string FirstName, string SecondName, string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth, byte Gender, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            // -1 indicates invalide id 
            // for whatever reason (insertion failed, exception occured...etc)
            int PersonID = -1;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"Insert Into People
                            Values (@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth, @Gender, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath);
                            Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gender", Gender);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (SecondName != null)
                command.Parameters.AddWithValue("@SecondName", SecondName);
            else
                command.Parameters.AddWithValue("@SecondName", System.DBNull.Value);
            
            if (ThirdName != null)
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            if (Email != null)
                command.Parameters.AddWithValue("@Email", Email);
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);    


            if (ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);    
            
            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                int InsertedID;
                if (result!=null && int.TryParse(result.ToString(), out InsertedID))
	                PersonID = InsertedID; 
            }
            catch (Exception)
            {
                // nothing to do seemingly
            }
            finally
            {
                connection.Close();
            }

            return PersonID;
        }

        public static bool UpdatePerson(int PersonID, string FirstName, string SecondName, string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth, byte Gender, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"UPDATE people
SET          nationalno = @NationalNo, firstname = @FirstName, secondname = @SecondName, thirdname = @ThirdName, lastname = @LastName, dateofbirth = @DateOfBirth, gendor = @Gender, address = @Address, phone = @Phone, email = @Email, nationalitycountryid = @NationalityCountryID, imagepath = @ImagePath
                            WHERE personid = @PersonID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gender", Gender);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            if (SecondName != null)
                command.Parameters.AddWithValue("@SecondName", SecondName);
            else
                command.Parameters.AddWithValue("@SecondName", System.DBNull.Value);

            if (ThirdName != null)
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            if (Email != null)
                command.Parameters.AddWithValue("@Email", Email);
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);    


            if (ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);    
            
            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                rowsAffected = 0;
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected>0);
        }

        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"SELECT people.*, countries.countryname
                            FROM     countries INNER JOIN
                                people ON countries.countryid = people.nationalitycountryid";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        dt.Load(reader);
                }    
            }
            catch (Exception)
            {
                // nothing to do i believe !
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }
        
        public static bool DeletePerson(int PersonID)
            {
            int rowsDeleted = 0;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"DELETE FROM people
                             WHERE personid = @PersonID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                rowsDeleted = command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                rowsDeleted = 0;
            }
            finally
            {
                connection.Close();
            }

            return (rowsDeleted>0);
        }
            
        public static bool IsPersonExist(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"SELECT isExist=1 FROM people
                             WHERE personid = @PersonID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                using(SqlDataReader reader = command.ExecuteReader())
                {
                    isFound = reader.HasRows;
                }
            }
            catch (Exception)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        
        public static bool IsPersonExist(string NationalNo)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"SELECT isExist=1 FROM people
                             WHERE nationalno = @NationalNo;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    isFound = reader.HasRows;
                }
            }
            catch (Exception)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
    }
}
