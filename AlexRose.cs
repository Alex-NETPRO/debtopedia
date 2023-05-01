using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Security.Cryptography;

namespace DebtopediaDM
{
    public class AlexRose
    {
       Random randomobject= new Random();
       private const int Keysize = 256;
       private const int DerivationIterations = 1000;
       //private const string SQLConnString = "Data Source=JESSICA\\MAMUS;Initial Catalog=detopedia ;User Id=alex;Password=rose";
       private const string SQLConnString = "Data Source=164.160.128.120;Initial Catalog=debtoped_ia ;User Id=debtoped_ia123456;Password=debtoped_ia123456";
        
       public string Encrypt(string plainText, string passPhrase)
       {
           // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
           // so that the same Salt and IV values can be used when decrypting.  
           var saltStringBytes = Generate256BitsOfRandomEntropy();
           var ivStringBytes = Generate256BitsOfRandomEntropy();
           var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
           using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
           {
               var keyBytes = password.GetBytes(Keysize / 8);
               using (var symmetricKey = new RijndaelManaged())
               {
                   symmetricKey.BlockSize = 256;
                   symmetricKey.Mode = CipherMode.CBC;
                   symmetricKey.Padding = PaddingMode.PKCS7;
                   using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                   {
                       using (var memoryStream = new MemoryStream())
                       {
                           using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                           {
                               cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                               cryptoStream.FlushFinalBlock();
                               // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                               var cipherTextBytes = saltStringBytes;
                               cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                               cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                               memoryStream.Close();
                               cryptoStream.Close();
                               return Convert.ToBase64String(cipherTextBytes);
                           }
                       }
                   }
               }
           }
       }

       public string Decrypt(string cipherText, string passPhrase)
       {
           // Get the complete stream of bytes that represent:
           // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
           var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
           // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
           var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
           // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
           var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
           // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
           var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

           using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
           {
               var keyBytes = password.GetBytes(Keysize / 8);
               using (var symmetricKey = new RijndaelManaged())
               {
                   symmetricKey.BlockSize = 256;
                   symmetricKey.Mode = CipherMode.CBC;
                   symmetricKey.Padding = PaddingMode.PKCS7;
                   using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                   {
                       using (var memoryStream = new MemoryStream(cipherTextBytes))
                       {
                           using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                           {
                               var plainTextBytes = new byte[cipherTextBytes.Length];
                               var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                               memoryStream.Close();
                               cryptoStream.Close();
                               return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                           }
                       }
                   }
               }
           }
       }

       private static byte[] Generate256BitsOfRandomEntropy()
       {
           var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
           using (var rngCsp = new RNGCryptoServiceProvider())
           {
               // Fill the array with cryptographically secure random bytes.
               rngCsp.GetBytes(randomBytes);
           }
           return randomBytes;
       }

        public Decimal FileSizeCheck(String FilesPaths)
        {
          FileInfo Love = new FileInfo(FilesPaths);
          return (Love.Length) / ((1024) * (1024));
            }
        
        public DataSet SelectDataSet(string query){
        SqlConnection conn = new SqlConnection(SQLConnString);  
        DataSet Ds = new DataSet();
        try
        {
          conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            
            SqlDataAdapter Da = new SqlDataAdapter();
            Da.SelectCommand = cmd;
            Da.Fill(Ds,"Jessica");
        }
        catch (SqlException exSql1) 
        {
         Console.WriteLine("Error: {0}", exSql1.Message);
        }
        finally
        {
         conn.Close();
        }
        return Ds;
        }
   
        public string alphabet()
        {
            string alphabetx = "A";
            int firstnumber = randomobject.Next (1,26);
            if (firstnumber == 1){
            alphabetx = "A";}
            else if (firstnumber == 2){
            alphabetx = "B";
            }
            else if (firstnumber == 3){
            alphabetx = "C";
            }
            else if (firstnumber == 4){
            alphabetx = "D";
            }
            else if (firstnumber == 5){
            alphabetx = "E";
            }
            else if (firstnumber == 6){
            alphabetx = "F";
            }
            else if (firstnumber == 7){
            alphabetx = "G";
            }
            else if (firstnumber == 8){
            alphabetx = "H";
            }
            else if (firstnumber == 9){
            alphabetx = "I";
            }
            else if (firstnumber == 10){
            alphabetx = "J";
            }
           else if (firstnumber == 11){
            alphabetx = "K";
            }
            else if (firstnumber == 12){
            alphabetx = "L";
            }
            else if (firstnumber == 13){
            alphabetx = "M";
            }
            else if (firstnumber == 14){
            alphabetx = "N";
            }
            else if (firstnumber == 15){
            alphabetx = "O";
            }
            else if (firstnumber == 16){
            alphabetx = "P";
            }
            else if (firstnumber == 17){
            alphabetx = "Q";
            }
            else if (firstnumber == 18){
            alphabetx = "R";
            }
            else if (firstnumber == 19){
            alphabetx = "S";
            }
            else if (firstnumber == 20){
            alphabetx = "T";
            }
            else if (firstnumber == 21){
            alphabetx = "U";
            }
            else if (firstnumber == 22){
            alphabetx = "V";
            }
            else if (firstnumber == 23){
            alphabetx = "W";
            }
            else if (firstnumber == 24){
            alphabetx = "X";
            }
            else if (firstnumber == 25){
            alphabetx = "Y";
            }
            else if (firstnumber == 26){
            alphabetx = "Z";
            }
            else{alphabetx = "Z";}
            return alphabetx;
            }
         
            public int numeric()
            {
            int numb = 1000;
                numb = randomobject.Next(1000,9999);
                return numb;
            }
   
                public string ReturnFromQuerry(string Querry)
                {
                    SqlConnection conn = new SqlConnection(SQLConnString);
                    string returnValueString;
                    try
                    {
                        
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(Querry, conn);
                        var returnValuec = cmd.ExecuteScalar();

                        if (string.IsNullOrEmpty(Convert.ToString(returnValuec)) == true)
                        {
                            returnValueString = "";
                        }
                        else
                        {
                            returnValueString = returnValuec.ToString();
                        }

                    }
                    //catch 
                    //{
                    //    return "";
                    //}

                    finally{ conn.Close() ;}
                    return returnValueString;
                }
       
                public void AnySQLQuery(string Query)
                {
                    SqlConnection conn = new SqlConnection(SQLConnString);
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(Query, conn);
                        
                        cmd.ExecuteNonQuery();           
                    }
                    catch (SqlException exSql1) 
                    {
                     Console.WriteLine("Error: {0}", exSql1.Message);
                    }
                    finally{ conn.Close() ;}
                 }

            public void SendEmail(string RecievingEmailAddresses,string mySubject,string body , string SourceMailx,string SmtpClientx ,string Portx ,string Passwordx,string MailTypex)
            {
                try
                {
                    MailMessage MyMailMessage = new MailMessage();
                    MyMailMessage.From = new MailAddress(SourceMailx);
                    MyMailMessage.To.Add(RecievingEmailAddresses.Replace(",,", ","));
                    MyMailMessage.Subject = mySubject;
                    MailTypex = MailTypex.ToUpper();
                    if (MailTypex == "HTML"){MyMailMessage.IsBodyHtml = true;}
                    MyMailMessage.Body = body;
                    SmtpClient SMTPServer = new SmtpClient(SmtpClientx);
                    SMTPServer.Port = Convert.ToInt32(Portx);
                    SMTPServer.Credentials = new System.Net.NetworkCredential(SourceMailx, Passwordx);
                    SMTPServer.Send(MyMailMessage);
                }
                catch (Exception ex)
                {
                  Console.WriteLine("Error: {0}", ex.Message);
                }
            }

            public void SendEmailCC(string CCEmailAddresses,string RecievingEmailAddresses,string mySubject,string body , string SourceMailx,string SmtpClientx ,string Portx ,string Passwordx,string MailTypex)
            {
                try
                {
                    MailMessage MyMailMessage = new MailMessage();
                    MyMailMessage.From = new MailAddress(SourceMailx);
                    MyMailMessage.To.Add(RecievingEmailAddresses.Replace(",,", ","));
                    MyMailMessage.CC.Add(RecievingEmailAddresses.Replace(",,", ","));
                    MyMailMessage.Subject = mySubject;
                    MailTypex = MailTypex.ToUpper();
                    if (MailTypex == "HTML"){MyMailMessage.IsBodyHtml = true;}
                    MyMailMessage.Body = body;
                    SmtpClient SMTPServer = new SmtpClient(SmtpClientx);
                    SMTPServer.Port = Convert.ToInt32(Portx);
                    SMTPServer.Credentials = new System.Net.NetworkCredential(SourceMailx, Passwordx);
                    SMTPServer.Send(MyMailMessage);
                }
                catch (Exception ex)
                {
                  Console.WriteLine("Error: {0}", ex.Message);
                }
            }
    

           public void SendSMS(string toPhonez,string body,string senderid)
           {
                WebRequest webrequest;
                WebResponse webresponse;
                string  webresponsestring = "";
                string message = body;
                toPhonez = toPhonez.Trim();
                message = message.Replace("&", "n");
                message = message.Replace(" ", "+");
                toPhonez = toPhonez.Replace(",0", ",234");
                if (toPhonez.StartsWith("0") == true)
                {
                toPhonez = toPhonez.Remove(0, 1);
                }
      
                string url = "http://smsc.xwireless.net/API/WebSMS/Http/v3.1/index.php?username=DNMP&password=AlexRose_1&sender=" + senderid + "&to=" + toPhonez + "&message=" + message + "&reqid=1&format=json";
                webrequest = HttpWebRequest.Create(url);
                webrequest.Timeout = 2000;
                webresponse = webrequest.GetResponse();
                StreamReader reader = new StreamReader(webresponse.GetResponseStream());
                webresponsestring = reader.ReadToEnd();
                webresponse.Close();
           }

        public double vall(string Input)
        {
            double valx = 0;
            try {valx = double.Parse(Input); }
            catch { valx = 0; }
            return valx;
        }
    

    }
}