using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Models
{
    public class GoogleFilesViewModel
    {
        public GoogleDriveFile GoogleDriveFile { get; set; }
        public List<GoogleDriveFile> GoogleDriveList { get; set; }
    }
}
