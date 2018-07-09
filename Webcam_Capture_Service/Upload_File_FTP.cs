using System;
using System.Net;
using System.IO;
using System.Text;
using System.Security;
using Secure_Data;

namespace Upload_FTP
{
    class Upload_File_FTP
    {
        public static bool Upload(string fileName, string fileLocation = "")
        {
            SecureString user = SecureData.DecryptString(System.Configuration.ConfigurationManager.AppSettings["serverUser"]);
            SecureString pass = SecureData.DecryptString(System.Configuration.ConfigurationManager.AppSettings["serverPass"]);
            
            try
            {
                // Get the object used to communicate with the server.  
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(@"ftp://tqbdev.ddns.net:2121/temp/" + fileName);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous login.  
                request.Credentials = new NetworkCredential(SecureData.ToInsecureString(user), SecureData.ToInsecureString(pass));
                // Copy the contents of the file to the request stream.  
                StreamReader sourceStream = new StreamReader(fileLocation + fileName);
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                sourceStream.Close();
                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Utilities.Log.WriteLogSucess("Upload File " + fileName + " Complete, status " + response.StatusDescription);

                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                Utilities.Log.WriteLogError(ex);
                return false;
            }
        }
    }

    class Upload_Folder_FTP
    {
        public static bool Upload(string folderLocation)
        {
            SecureString user = SecureData.DecryptString(System.Configuration.ConfigurationManager.AppSettings["serverUser"]);
            SecureString pass = SecureData.DecryptString(System.Configuration.ConfigurationManager.AppSettings["serverPass"]);

            string[] filesName = Directory.GetFiles(folderLocation);
            foreach (string fullfileName in filesName)
            {
                string fileName = Path.GetFileName(fullfileName);
                try
                {
                    // Get the object used to communicate with the server.  
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(@"ftp://tqbdev.ddns.net:2121/temp/" + fileName);
                    request.Method = WebRequestMethods.Ftp.UploadFile;

                    // This example assumes the FTP site uses anonymous login.  
                    request.Credentials = new NetworkCredential(SecureData.ToInsecureString(user), SecureData.ToInsecureString(pass));
                    // Copy the contents of the file to the request stream.  
                    StreamReader sourceStream = new StreamReader(fullfileName);
                    byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                    sourceStream.Close();
                    request.ContentLength = fileContents.Length;

                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(fileContents, 0, fileContents.Length);
                    requestStream.Close();

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                    Utilities.Log.WriteLogSucess("Upload File " + fileName + " Complete, status " + response.StatusDescription);

                    File.Delete(fullfileName);
                    response.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    Utilities.Log.WriteLogError(ex);
                    return false;
                }
            }

            return false;
        }
    }
}