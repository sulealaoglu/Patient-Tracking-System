
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public static class Database
    {
        public static Doctor staticDoctor = null;
        public static SqlConnection GetSqlConnection()
        {
            string connectionString = @"Data Source=DESKTOP-68OLV01\SQLEXPRESS;Initial Catalog=patient_tracking_db;Integrated Security=SSPI;MultipleActiveResultSets=true;";
            // driver, provider
            return new SqlConnection(connectionString);

        }
        //string mail,string password
        public static Object findUser_P(string mail, string password)
        {

            using (var connection = GetSqlConnection())
            {
                connection.Open();
                string sql = "select * from [dbo].[User] where UserName=@mail and Password=@password";
                SqlCommand f2command = new SqlCommand(sql, connection);
                f2command.Parameters.AddWithValue("@password", password.ToString());
                f2command.Parameters.AddWithValue("@mail", mail.ToString());
                SqlDataReader reader = null;
                try
                {
                    reader = f2command.ExecuteReader();
                    if (reader.Read())
                    {
                        Console.WriteLine(reader[0].ToString(), reader[1].ToString(), reader[2].ToString());
                    }
                    else
                        return null;

                    // read 
                }
                catch (Exception e)
                {
                }
                if (reader[2].ToString().Equals("1")) //type patient login
                {
                    Patient patient = null;
                    string sql1 = "SELECT * FROM [dbo].[User] U INNER JOIN Patient P ON p.UserName = u.UserName where u.username=@mail; ";
                    SqlCommand fcommand1 = new SqlCommand(sql1, connection);
                    fcommand1.Parameters.Add("@mail", System.Data.SqlDbType.VarChar);
                    fcommand1.Parameters["@mail"].Value = mail;
                    reader = fcommand1.ExecuteReader();
                    Console.WriteLine();
                    if (reader.Read())
                    {
                        patient = new Patient
                        {
                            name = reader["Name"].ToString(),
                            surname = reader["Surname"].ToString(),
                            treatment = reader["Treatment"].ToString(),
                            drug = reader["Drug"].ToString(),
                            bdate = reader["Bdate"].ToString(),
                            id = Int32.Parse(reader["PId"].ToString())
                        };

                        string sql2 = "select * from Doctor where DId IN (select DId from Patient WHERE userName = @mail) ";
                        SqlCommand fcommand2 = new SqlCommand(sql2, connection);
                        fcommand2.Parameters.Add("@mail", System.Data.SqlDbType.VarChar);
                        fcommand2.Parameters["@mail"].Value = mail;
                        reader = fcommand2.ExecuteReader();

                        if (reader.Read())
                        {

                            Doctor dr = new Doctor
                            {
                                name = reader["Name"].ToString(),
                                surname = reader["Surname"].ToString(),
                                id = Int32.Parse(reader["DId"].ToString())
                            };

                            patient.dr = dr;

                        }
                    }
                    return patient;
                }

                else if (reader[2].ToString().Equals("2")) //doctor login
                {
                    string sql1 = "SELECT * FROM [dbo].[User] U INNER JOIN Doctor D ON D.UserName = U.UserName where U.username=@mail; ";
                    SqlCommand fcommand1 = new SqlCommand(sql1, connection);
                    fcommand1.Parameters.Add("@mail", System.Data.SqlDbType.VarChar);
                    fcommand1.Parameters["@mail"].Value = mail;
                    reader = fcommand1.ExecuteReader();
                    Doctor dr = null;
                    if (reader.Read())
                    {
                        dr = new Doctor
                        {
                            name = reader["Name"].ToString(),
                            surname = reader["Surname"].ToString(),
                            id = Int32.Parse(reader["DId"].ToString())
                        };
                        Console.WriteLine();

                    }
                    return dr;
                }

                else return null;
                reader.Close();
            }


        }
        public static Doctor findDoctor(Int32 id)
        {
            Doctor doctor = null;
            using (var connection = GetSqlConnection())
            {
                try
                {
                    connection.Open();
                    string sql = "select * from Doctor where DId= @id;";
                    SqlCommand fcommand = new SqlCommand(sql, connection);
                    fcommand.Parameters.Add("@id", System.Data.SqlDbType.Int);
                    fcommand.Parameters["@id"].Value = id;
                    SqlDataReader reader = fcommand.ExecuteReader();
                    if (reader.Read())
                        doctor = new Doctor { id = Int32.Parse(reader[0].ToString()), name = reader[1].ToString(), surname = reader[2].ToString() };
                    reader.Close();
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return doctor;
        }

        public static Patient findPatient(Int32 id)
        {
            Patient p = null;
            using (var connection = GetSqlConnection())
            {

                try
                {
                    connection.Open();
                    string sql = "select * from Patient where PId= @id;";
                    SqlCommand fcommand = new SqlCommand(sql, connection);
                    fcommand.Parameters.Add("@id", System.Data.SqlDbType.Int);
                    fcommand.Parameters["@id"].Value = id;
                    SqlDataReader reader = fcommand.ExecuteReader();
                    if (reader.Read())
                    {

                        p = new Patient
                        {
                            id = Int32.Parse(reader["PId"].ToString()),
                            bdate = reader["Bdate"].ToString(),
                            drug = reader["Drug"].ToString(),
                            treatment = reader["Treatment"].ToString(),
                            name = reader["Name"].ToString(),
                            surname = reader["Surname"].ToString()
                        };

                        int did = Int32.Parse(reader["DId"].ToString());
                        Doctor d = findDoctor(did);
                        p.dr = d;
                    }
                    reader.Close();
                    p.pharmacies = Database.GetDrugFromPharmacies(p.id);
                }


                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return p;
        }


        public static List<Doctor> GetAllDoctors()
        {
            List<Doctor> doctors = null;
            using (var connection = GetSqlConnection())
            {
                try
                {
                    connection.Open();
                    string sql = "select * from dbo.Doctor";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    doctors = new List<Doctor>();
                    while (reader.Read())
                    {
                        doctors.Add(new Doctor { id = Int32.Parse(reader[0].ToString()), name = reader[1].ToString(), surname = reader[2].ToString() });
                        Console.WriteLine($"name: {reader[1]} id: {reader[0]}");
                    }
                    reader.Close();
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return doctors;
        }

        public static List<Pharmacy> GetAllPharmacies()
        {
            List<Pharmacy> pharmacies = null;
            using (var connection = GetSqlConnection())
            {
                try
                {
                    connection.Open();
                    string sql = "select * from dbo.Pharmacy";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    pharmacies = new List<Pharmacy>();

                    while (reader.Read())
                    {

                        pharmacies.Add(new Pharmacy { id = Int32.Parse(reader[0].ToString()), name = reader[1].ToString(), address = reader[2].ToString() });

                    }
                    reader.Close();
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return pharmacies;
        }

        public static List<DrugPharmacyViewModel> GetDrugFromPharmacies(Int32 id)
        {
            List<DrugPharmacyViewModel> phd = null;
            using (var connection = GetSqlConnection())
            {
                try
                {
                    connection.Open();
                    string sql = "select D.Name,Ph.Name,pd.Count from Pharmacy as Ph inner join PhDrug as PD on PD.PhId=Ph.PhId inner join Drug as D on D.DrugId = PD.DrugId inner join Patient as P on P.Drug=D.DrugId where P.PId = @id and pd.Count >0";
                    SqlCommand fcommand = new SqlCommand(sql, connection);
                    fcommand.Parameters.Add("@id", System.Data.SqlDbType.Int);
                    fcommand.Parameters["@id"].Value = id;
                    SqlDataReader reader = fcommand.ExecuteReader();
                    phd = new List<DrugPharmacyViewModel>();

                    while (reader.Read())
                    {
                        phd.Add(new DrugPharmacyViewModel
                        {
                            DrugName = reader[0].ToString(),
                            PharmacyName = reader[1].ToString(),
                            Count = Int32.Parse(reader[2].ToString())
                        });

                    }
                    reader.Close();
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return phd;
        }

        public static List<Patient> GetAllPatients()
        {
            List<Patient> patients = null;
            using (var connection = GetSqlConnection())
            {
                try
                {
                    connection.Open();
                    string sql = "select * from dbo.Patient";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    patients = new List<Patient>();
                    while (reader.Read())
                    {
                        Patient p = new Patient
                        {
                            id = Int32.Parse(reader[0].ToString()),
                            bdate = reader["Bdate"].ToString(),
                            drug = reader["Drug"].ToString(),
                            treatment = reader["Treatment"].ToString(),
                            name = reader[3].ToString(),
                            surname = reader[4].ToString()
                        };

                        int id = Int32.Parse(reader["DId"].ToString());
                        Doctor d = findDoctor(id);
                        p.dr = d;
                        patients.Add(p);
                    }
                    reader.Close();
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return patients;
        }

        public static int Delete_Patient(int id)
        {
            int result = 0;

            using (var connection = GetSqlConnection())
            {
                try
                {
                    connection.Open();

                    string sql = "DELETE FROM Patient WHERE PId = @id";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@id",id);
                    result = command.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return result;
        }

        public static string generateRandomStr()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            finalString += ("@ptsystem.com");
            return finalString.ToString();
        }
        public static int Create(string name, string surname, DateTime bdate, string treatment, int drug_id, Doctor doctor)
        {
            int result = 0;

            using (var connection = GetSqlConnection())
            {
                try
                {
                    connection.Open();

                    string sql1 = "insert into [dbo].[User] VALUES (@username ,'123456' ,1)";
                    string userName = generateRandomStr();
                    SqlCommand command1 = new SqlCommand(sql1, connection);
                    command1.Parameters.AddWithValue("@username", userName);
                    result = command1.ExecuteNonQuery();



                    int did = doctor.id;
                    string sql = "insert into Patient VALUES (@did,@bdate,@name,@surname,@userName,@treatment,@drug_id)";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@did", did);
                    command.Parameters.AddWithValue("@bdate", bdate);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@surname", surname);
                    command.Parameters.AddWithValue("@userName", userName);
                    command.Parameters.AddWithValue("@treatment", treatment);
                    command.Parameters.AddWithValue("@drug_id", drug_id);


                    result = command.ExecuteNonQuery();
                    Console.WriteLine($"{result} adet kayıt eklendi");

                }
                catch (Exception e)
                {
                    result = -1;
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }
        public static int CreateAppointment(AddAppointmentViewModel appointment)
        {
            int result = 0;

            using (var connection = GetSqlConnection())
            {
                try
                {
                    connection.Open();

                    string sql = "insert into Appointment values(@pid,@did,@date,@time)";

                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@pid", appointment.PatientID);
                    command.Parameters.AddWithValue("@did", appointment.DoctorID);
                    command.Parameters.AddWithValue("@date", appointment.AppointmentDate);
                    command.Parameters.AddWithValue("@time", appointment.AppointmentTime);
                    result = command.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }




        public static int addLog(String userName)
        {
            int result = 0;

            using (var connection = GetSqlConnection())
            {
                try
                {
                    connection.Open();

                    string sql = "insert into Logs VALUES (@userName,@dt)";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@userName", userName);
                    DateTime dt = DateTime.Now;
                    command.Parameters.AddWithValue("@dt", dt);


                    result = command.ExecuteNonQuery();


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }


        public static List<LogViewModel> getLogs()
        {

            List<LogViewModel> logs = null;
            using (var connection = GetSqlConnection())
            {
                try
                {
                    connection.Open();
                    string sql = "select * from dbo.Logs";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    logs = new List<LogViewModel>();
                    while (reader.Read())
                    {
                        
                        LogViewModel l = new LogViewModel
                        {
                            username = reader["UserName"].ToString(),
                            DateTime = reader["Time"].ToString()
                        };


                        logs.Add(l);
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                   
            }
            return logs;
           
        }

    public static int count(string table_name)
    {
        int count = 0;

        using (var connection = GetSqlConnection())
        {
            try
            {
                connection.Open();

                string sql = "select count(*) from " + table_name;

                SqlCommand count_command = new SqlCommand(sql, connection);
                object result = count_command.ExecuteScalar();
                if (result != null)
                {
                    count = Convert.ToInt32(result);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        return count;
    }

    
}


}


