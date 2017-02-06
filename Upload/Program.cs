using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace Upload
{
    class Program
    {
        static string FtpUrl;//= "ftp://192.168.1.173/TEST1/";
        static string fileName1;// = @"C:\Users\dm413\Desktop\Amit\FTP UPLOAD FOLDER\AB.txt";
        FileStream fs = null;
        Stream rs = null;




        public static void UploadFiles(string ftpurl,string folderName,string ServerfilePath, string UserName, string Password)
        {
            FileStream fs = null;
            Stream rs = null;

            try
            {
                string file = @"C:\Users\dm413\Downloads\annual_report_2009.pdf";
                string uploadFileName = new FileInfo(file).Name;
                string uploadUrl = "ftp://192.168.1.173/TEST1/FTP UPLOAD FOLDER/";
                fs = new FileStream(file, FileMode.Open, FileAccess.Read);

                string ftpUrl = string.Format("{0}{1}", uploadUrl, uploadFileName);
                FtpWebRequest requestObj = FtpWebRequest.Create(ftpUrl) as FtpWebRequest;
                requestObj.Method = WebRequestMethods.Ftp.UploadFile;
                requestObj.Credentials = new NetworkCredential("D2KT/d2kuser", "miswak365");
                rs = requestObj.GetRequestStream();

                byte[] buffer = new byte[8092];
                int read = 0;
                while ((read = fs.Read(buffer, 0, buffer.Length)) != 0)
                {
                    rs.Write(buffer, 0, read);
                }
                rs.Flush();
            }
            catch (Exception e)
            {
                //MessageBox.Show("File upload/transfer Failed.\r\nError Message:\r\n" + ex.Message, "Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }

                if (rs != null)
                {
                    rs.Close();
                    rs.Dispose();
                }
            }
        }


        
        public static void CreateDir(string ftpurl,string DirName,string userName,string password )
        {
            if (!IsDirExist(ftpurl,DirName, userName, password))
            {
                try
                {
                    string strUrl = ftpurl + DirName;
                    FtpWebRequest ftpReq = (FtpWebRequest)FtpWebRequest.Create(strUrl);
                    ftpReq.Method = WebRequestMethods.Ftp.MakeDirectory;

                    ftpReq.Credentials = new NetworkCredential(userName, password);
                    FtpWebResponse ftpResp = (FtpWebResponse)ftpReq.GetResponse();
                    ftpResp.Close();
                    //for (int i = 0; i < 4; i++)
                    //{
                    //    string filename = Console.ReadLine();
                    //    UploadFiles(strUrl, filename);
                    //}
                   
                }

                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public static bool IsDirExist(string ftpurl,string directory,string userName,string password)
        {
            try
            {
                string strUrl = ftpurl;
                FtpWebRequest ftpReq = (FtpWebRequest)FtpWebRequest.Create(strUrl);
                ftpReq.Method = WebRequestMethods.Ftp.ListDirectory;

                ftpReq.Credentials = new NetworkCredential(userName, password);
                FtpWebResponse ftpResp = (FtpWebResponse)ftpReq.GetResponse();
                // ftpResp.Close();
                //for (int i = 0; i < 4; i++)
                //{
                //    string filename = Console.ReadLine();
                //    UploadFiles(strUrl, filename);
                //}
                StreamReader reader = new StreamReader(ftpResp.GetResponseStream());
                string content = reader.ReadToEnd();

                bool flg = content.Contains(directory);
                return flg;
            }

            catch (Exception ex)
            {

                throw;
            }

        }

        public static string DownloadFile(string FtpFilePath, string ServerFilePath,string UserName,string Password)
        {
          
            string FileNameToDownload = FtpFilePath;
            int posn = FileNameToDownload.LastIndexOf("/");
            string userName = UserName;
            string password = Password;
            string tempDirPath = ServerFilePath;
            

            string ResponseDescription = "";
            //string PureFileName = new FileInfo(FileNameToDownload).Name;
            string PureFileName = FileNameToDownload.Substring(posn+1);
            string DownloadedFilePath = tempDirPath + "\\" + PureFileName;
            string downloadUrl = FileNameToDownload;
            FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(downloadUrl);
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            req.Credentials = new NetworkCredential(userName, password);
            req.UseBinary = true;
            req.Proxy = null;
            try
            {
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                Stream stream = response.GetResponseStream();
                byte[] buffer = new byte[2048];
                FileStream fs = new FileStream(DownloadedFilePath, FileMode.Create);
                int ReadCount = stream.Read(buffer, 0, buffer.Length);
                while (ReadCount > 0)
                {
                    fs.Write(buffer, 0, ReadCount);
                    ReadCount = stream.Read(buffer, 0, buffer.Length);
                }
                ResponseDescription = response.StatusDescription;
                fs.Close();
                stream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return ResponseDescription;
        }



        






        static void Main(string[] args)
        {
           // CreateDir("FTP UPLOAD FOLDER");

            UploadFiles("ftp://192.168.1.173/TEST1/", "FTP UPLOAD FOLDER", @"C:\Users\dm413\Downloads\annual_report_2009.pdf", "D2KT/d2kuser","miswak365");
            //string response2 = DownloadFile();

            //Console.WriteLine(response2);
            //DownloadFile("ftp://192.168.1.173/TEST1/TEST!.txt", @"C:\Users\dm413\Desktop\Amit\FTP UPLOAD FOLDER" ,"D2KT/d2kuser","miswak365" );

              Console.ReadKey();


        }
    }




}

