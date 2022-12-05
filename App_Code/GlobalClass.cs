using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Collections.Specialized;
public class GlobalClass
{
    public readonly static string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
    public readonly static string DatabaseError = "Unable to process your request. Please try again.";
    public readonly static string CustomError = "Something went wrong!";

    public static DataTable LoadUser(string userId)
    {
        DataTable dtUser = new DataTable();
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from tblUser where UserId = @UserId COLLATE SQL_Latin1_General_CP1_CS_AS";
            cmd.Parameters.AddWithValue("@UserId", userId);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dtUser);
        }
        return dtUser;
    }
    public static DataTable LoadUserInfo(string userId)
    {
        DataTable dtUserInfo = new DataTable();
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from tblUserInfo where UserId = @UserId";
            cmd.Parameters.AddWithValue("@UserId", userId);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dtUserInfo);
        }
        return dtUserInfo;
    }
   
    public static DataTable LoadAdmin(string adminId)
    {
        DataTable dtAdmin = new DataTable();
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from tblAdmin where AdminId = @AdminId COLLATE SQL_Latin1_General_CP1_CS_AS";
            cmd.Parameters.AddWithValue("@AdminId", adminId);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dtAdmin);
        }
        return dtAdmin;
    }
   
    public static DataTable LoadEmailService()
    {
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from tblEmailService";
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
        }
        return dt;
    }
    public static DataTable LoadMessageService()
    {
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from tblMessageService";
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
        }
        return dt;
    }

    public static bool IsUserValid(string userId, string password)
    {
        bool valid;
        DataTable dtUser = new DataTable();
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select * from tblUser where UserId = @UserId COLLATE SQL_Latin1_General_CP1_CS_AS and Password = @Password COLLATE SQL_Latin1_General_CP1_CS_AS";
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@Password", password);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dtUser);
        }
        if (dtUser.Rows.Count != 0)
            valid = true;
        else
            valid = false;
        return valid;
    }
    public static bool IsAdminValid(string adminId, string password)
    {
        bool valid;
        DataTable dtAdmin = new DataTable();
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from tblAdmin where AdminId = @AdminId COLLATE SQL_Latin1_General_CP1_CS_AS and Password = @Password COLLATE SQL_Latin1_General_CP1_CS_AS";
            cmd.Parameters.AddWithValue("@AdminId", adminId);
            cmd.Parameters.AddWithValue("@Password", password);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dtAdmin);
        }
        if (dtAdmin.Rows.Count != 0)
            valid = true;
        else
            valid = false;
        return valid;
    }
    public static bool IsAdminPermanent(string adminId)
    {
        bool permanent;
        DataTable dtAdmin = new DataTable();
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from tblAdmin where AdminId = @AdminId COLLATE SQL_Latin1_General_CP1_CS_AS";
            cmd.Parameters.AddWithValue("@AdminId", adminId);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dtAdmin);
        }
        if (dtAdmin.Rows[0]["Type"].ToString().Trim() == "Permanent")
            permanent = true;
        else
            permanent = false;
        return permanent;
    }
    public static bool IsUserExpire(string userId)
    {
        bool expire;
        DateTime expiryDate = new DateTime();
        DateTime today = DateTime.Parse(CurrentDateOnly());
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select ExpDate from tblUserInfo where UserId = @UserId";
            cmd.Parameters.AddWithValue("@UserId", userId);
            con.Open();
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                    expiryDate = DateTime.Parse(rdr["ExpDate"].ToString().Trim());
            }
        }
        int flag = expiryDate.CompareTo(today);
        if (flag >= 0)
            expire = false;
        else
            expire = true;
        return expire;
    }
    public static string CheckUserValidity(string userId)
    {
        string validity = null;
        DataTable dt = LoadUser(userId);
        if (dt.Rows[0]["Status"].ToString().Trim() == "Active")
        {
            DateTime expiryDate = new DateTime();
            DateTime today = DateTime.Parse(CurrentDateOnly());
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "select ExpDate from tblUserInfo where UserId = @UserId";
                cmd.Parameters.AddWithValue("@UserId", userId);
                con.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                        expiryDate = DateTime.Parse(rdr["ExpDate"].ToString().Trim());
                }
            }
            string span = expiryDate.Subtract(today).ToString("dd");
            int gap = expiryDate.CompareTo(today);
            if (gap >= 0)
                validity = span + " day left";
            else
                validity = "Expired on: " + expiryDate.ToString("dd-MM-yy");
        }
        else
            validity = "N/A";
        return validity;
    }
    public static int CountActiveTeam(string userId)
    {
        int team = 0;
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select count (RefId) from tblUser where RefId = @RefId and Status = 'Active'";
            cmd.Parameters.AddWithValue("@RefId", userId);
            con.Open();
            team = (int)cmd.ExecuteScalar();
        }
        return team;
    }
  
  
    public static string GenerateOTP()
    {
        const String code = "ABCDEFGHIJ0123456789";
        string OTP = "";
        Random ran = new Random();
        for (int i = 0; i < 6; i++)
        {
            int a = ran.Next(20);
            OTP = OTP + code.ElementAt(a);
        }
        return OTP;
    }
    public static string CurrentDateOnly()
    {
        string date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("dd-MM-yyyy");
        return date;
    }
    public static string CurrentDateTime()
    {
        string datetime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("dd-MM-yyyy HH:mm:ss");
        return datetime;
    }

    public static string SendEmail(string email, string subject, string message)
    {
        string response = null;
        DataTable dt = GlobalClass.LoadEmailService();
        string username = dt.Rows[0]["Username"].ToString().Trim();
        string password = dt.Rows[0]["Password"].ToString().Trim();
        string server = dt.Rows[0]["ServerAddress"].ToString().Trim();
        int port = Convert.ToInt32(dt.Rows[0]["Port"].ToString().Trim());
        try
        {
            SmtpClient client = new SmtpClient(server, port);
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = true;
            client.Credentials = new NetworkCredential(username, password);
            MailMessage msg = new MailMessage();
            msg.To.Add(email);
            msg.From = new MailAddress(username);
            msg.Subject = subject;
            msg.Body = message;
            client.Send(msg);
        }
        catch (Exception ex)
        {
            response = "Could not send email. " + ex.Message;
        }
        return response;
    }
    public static string SendMessage(string number, string message)
    {
        string result = null;
        DataTable dt = GlobalClass.LoadMessageService();
        string key = dt.Rows[0]["ApiKey"].ToString().Trim();
        string Sendername = dt.Rows[0]["SenderName"].ToString().Trim();
        string Number = "91" + number;
        try
        {
            String Message = HttpUtility.UrlEncode(message);
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                {"apikey" , key},
                {"numbers" , Number},
                {"message" , Message},
                {"sender" , Sendername}
                });
                /*result = Encoding.UTF8.GetString(response);*/
            }
        }
        catch (Exception ex)
        {
            result = "Unable to process your request. Please try again later." + ex.Message;
        }
        return result;
    }
}