using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using Dapper;

namespace webapi_sidorova
{
    public class SQL
    {
        SqlConnection cnn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Sidorova\\Desktop\\webapi_sidorova\\bin\\Debug\\netcoreapp3.1\\test_db.mdf;Integrated Security=True;Connect Timeout=30");
        SqlDataReader reader;
        
        public string GetThemes()
        {
            try
            {
                cnn.Open();
                Dictionary<int, string> dict = new Dictionary<int, string>();
                reader = new SqlCommand("SELECT * FROM Themes", cnn).ExecuteReader();
                while (reader.Read())
                {
                    dict.Add(reader.GetInt32("ThemeID"), reader.GetString("Content"));
                }
                reader.Close();
                return JsonConvert.SerializeObject(dict);
            }
            catch (Exception)
            {
                return "[ ]";
            }
            finally
            {
                cnn.Close();
            }
        }

        public int GetContact(string user, string mail, string phone)
        {
            try
            {
                cnn.Open();
                int contact = 0;
                reader = new SqlCommand(String.Format("SELECT * FROM Contacts WHERE Mail=\'{0}\' " +
                    "AND Phone=\'{1}\'", mail, phone), cnn).ExecuteReader();
                while (reader.Read())
                {
                    contact = reader.GetInt32("ContactId");
                }
                reader.Close();
                if (contact == 0)
                {
                    int result = new SqlCommand(String.Format("INSERT INTO Contacts ([User], Mail, Phone) " +
                        "VALUES (\'{0}\', \'{1}\', \'{2}\')", user, mail, phone), cnn).ExecuteNonQuery();
                    if (result > 0)
                    {
                        reader = new SqlCommand(String.Format("SELECT * FROM Contacts WHERE Mail=\'{0}\' " +
                        "AND Phone=\'{1}\'", mail, phone), cnn).ExecuteReader();
                        while (reader.Read())
                        {
                            contact = reader.GetInt32("ContactId");
                        }
                        reader.Close();
                    }
                }
                return contact;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                cnn.Close();
            }
        }

        public int NewMessage(int contact, int theme, string text)
        {
            try
            {
                cnn.Open();
                int result = new SqlCommand(string.Format("INSERT INTO Messages (ContactId, ThemeId, Text) " +
                    "VALUES ({0}, {1}, \'{2}\')", contact, theme, text), cnn).ExecuteNonQuery();
                int msg = 0;
                if (result > 0)
                {
                    reader = new SqlCommand("SELECT Id FROM Messages ORDER BY Id DESC", cnn).ExecuteReader();
                    while (reader.Read())
                    {
                        msg = reader.GetInt32("Id");
                        break;
                    }
                    reader.Close();
                }
                return msg;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                cnn.Close();
            }
        }

        public string GetMessage(int id)
        {
            try
            {
                //cnn.Open();
                //SqlDataAdapter adapter = new SqlDataAdapter(new SqlCommand("SELECT Contacts.[User] AS [name], " +
                //    "Contacts.Mail AS mail, Contacts.Phone AS phone, Themes.Content AS theme, " +
                //    "Messages.Text AS message, Messages.Date AS [date]" +
                //    "FROM Messages JOIN Contacts ON Messages.ContactId=Contacts.ContactId " +
                //    "JOIN Themes ON Messages.ThemeId=Themes.ThemeId WHERE Messages.Id=" + id, cnn));
                //DataTable dt = new DataTable();
                //adapter.Fill(dt);
                //return JsonConvert.SerializeObject(dt);


                var builder = new SqlBuilder();

                builder.Where("Messages.Id=@id", new { id = id });
                //builder.Where("Messages.ContactId=@user", new { user = 1 });
                //builder.Where("Messages.Text LIKE @some", new { some = "%test%" });

                var builderTemplate = builder.AddTemplate("SELECT Messages.Id, Contacts.[User] AS [Name], " +
                    "Contacts.Mail AS Mail, Contacts.Phone AS Phone, Themes.Content AS Theme, " +
                    "Messages.Text AS Message, Messages.Date AS [Date] " +
                    "FROM Messages JOIN Contacts ON Messages.ContactId=Contacts.ContactId " +
                    "JOIN Themes ON Messages.ThemeId=Themes.ThemeId /**where**/");
                var info = cnn.Query<Models.MessageInfo>(builderTemplate.RawSql, builderTemplate.Parameters).FirstOrDefault();
                /*Models.MessageInfo info = cnn.Query<Models.MessageInfo>("SELECT Messages.Id, Contacts.[User] AS [Name], " +
                    "Contacts.Mail AS Mail, Contacts.Phone AS Phone, Themes.Content AS Theme, " +
                    "Messages.Text AS Message, Messages.Date AS [Date]" +
                    "FROM Messages JOIN Contacts ON Messages.ContactId=Contacts.ContactId " +
                    "JOIN Themes ON Messages.ThemeId=Themes.ThemeId WHERE Messages.Id=@Id", new { id }).FirstOrDefault();*/
                
                info.Addition = "bla bla bla";
                return JsonConvert.SerializeObject(info);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "[ ]";
            }
            finally
            {
                cnn.Close();
            }
        }
    }
}
