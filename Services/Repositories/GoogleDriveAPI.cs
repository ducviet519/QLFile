using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Plus.v1;
using Google.Apis.Plus.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using WebTools.Models.Entities;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public class GoogleDriveAPI : IGoogleDriveAPI
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GoogleDriveAPI(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;

        }

        private string[] Scopes = { DriveService.Scope.Drive };
        private string ApplicationName = "VBNBTA";
        public string PathToServiceAccountKeyFile { get; private set; }
        public string p12Path { get; private set; }

        public void DownloadFile(string blobId, string savePath)
        {
            var service = GetDriveServiceAccount();
            var request = service.Files.Get(blobId);
            var stream = new MemoryStream();
            // Add a handler which will be notified on progress changes.
            // It will notify on each chunk download and when the
            // download is completed or failed.
            request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case Google.Apis.Download.DownloadStatus.Downloading:
                        {
                            Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Completed:
                        {
                            Console.WriteLine("Download complete.");
                            SaveStream(stream, savePath);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Failed:
                        {
                            Console.WriteLine("Download failed.");
                            break;
                        }
                }
            };
            request.Download(stream);
        }

        public string UploadFile(string path)
        {
            var service = GetDriveServiceAccount();
            var fileMetadata = new Google.Apis.Drive.v3.Data.File();
            fileMetadata.Name = Path.GetFileName(path);
            fileMetadata.MimeType = MimeTypes.GetMimeType(path);
            FilesResource.CreateMediaUpload request;
            using (var stream = new FileStream(path, FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, MimeTypes.GetMimeType(path));
                request.Fields = "id";
                request.Upload();
            }

            var file = request.ResponseBody;

            return file.Id;
        }

        private DriveService GetDriveServiceInstance()
        {
            UserCredential credential;
            // Xác thực tài khoản đăng nhập vào google drive của người dùng lần đầu tiên sau khi người dùng đăng nhập
            // thông tin sẽ được lưu vào "token.json" và lần chạy chương trình, sẽ lấy thông tin đăng nhập lại từ đây
            // và người dùng không phải đăng nhập lại
            using (var stream =
                new FileStream(Path.Combine(_webHostEnvironment.ContentRootPath, "credentials.json"), FileMode.Open, FileAccess.Read))
            {
                // Yêu cầu người dùng xác thực lần đầu và thông tin sẽ được lưu vào thư mục GoogleDriveFiles
                string FolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "GoogleDriveFiles");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(FolderPath, true)).Result;
            }

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }
        private DriveService GetDriveServiceAccount()
        {
            string[] scopes = new string[] { DriveService.Scope.Drive}; // Full access
            p12Path = Path.Combine(_webHostEnvironment.WebRootPath, "keys\\vbnb-367607-9cec436a0f37.p12");
            var serviceAccountEmail = "vbnb-ta-232@vbnb-367607.iam.gserviceaccount.com";
            var certificate = new X509Certificate2(p12Path, "notasecret", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
            ServiceAccountCredential credential = new ServiceAccountCredential(
               new ServiceAccountCredential.Initializer(serviceAccountEmail)
               {
                   Scopes = scopes
               }.FromCertificate(certificate));

            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                //ApiKey = "[AIzaSyCrtTRzUmd-3pp3sP6vfZlflhQBJtP6Wt0]",
                ApplicationName = ApplicationName
            });
            return service;          
        }

       

        private static void SaveStream(MemoryStream stream, string saveTo)
        {
            using (FileStream file = new FileStream(saveTo, FileMode.Create, FileAccess.Write))
            {
                stream.WriteTo(file);
            }
        }



        //get all files from Google Drive.
        public async Task<List<GoogleDriveFile>> GetDriveFiles()
        {
            DriveService service = GetDriveServiceAccount();
            // Define parameters of request.
            FilesResource.ListRequest FileListRequest = service.Files.List();

            // for getting folders only.
            //FileListRequest.Q = "mimeType='application/vnd.google-apps.folder'";
            FileListRequest.Fields = "nextPageToken, files(*)";
            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = (await FileListRequest.ExecuteAsync()).Files;
            List<GoogleDriveFile> FileList = new List<GoogleDriveFile>();
            // For getting only folders
            // files = files.Where(x => x.MimeType == "application/vnd.google-apps.folder").ToList();
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    GoogleDriveFile File = new GoogleDriveFile
                    {
                        Id = file.Id,
                        Name = file.Name,
                        Size = file.Size,
                        Version = file.Version,
                        CreatedTime = file.CreatedTime,
                        Parents = file.Parents,
                        MimeType = file.MimeType,
                        FileName = file.Name
                    };
                    FileList.Add(File);
                }
            }
            return FileList;
        }



        //get all files from Google Drive.
        public async Task<List<GoogleDriveFile>> SearchDriveFiles(string searchString)
        {
            DriveService service = GetDriveServiceAccount();

            // Define parameters of request.
            FilesResource.ListRequest FileListRequest = service.Files.List();

            //FileListRequest.Q = $"(fullText contains '{searchString}') or (mimeType = 'application/vnd.google-apps.document')";
            FileListRequest.Q = $"(fullText contains '{searchString}')";
            // List files.
            var files2 = await FileListRequest.ExecuteAsync();
            IList<Google.Apis.Drive.v3.Data.File> files = (await FileListRequest.ExecuteAsync()).Files;
            List<GoogleDriveFile> FileList = new List<GoogleDriveFile>();

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    GoogleDriveFile File = new GoogleDriveFile
                    {
                        Id = file.Id,
                        Name = file.Name,
                        Size = file.Size,
                        Version = file.Version,
                        CreatedTime = file.CreatedTime,
                        Parents = file.Parents,
                        MimeType = file.MimeType,
                        FileName = file.Name
                    };
                    FileList.Add(File);
                }
            }
            return FileList;
        }

    }
}
