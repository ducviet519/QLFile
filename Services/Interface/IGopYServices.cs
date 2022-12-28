using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Services.Interface
{
    public interface IGopYServices
    {
        Task<string> ThemGopY(string IDBieuMau, string noidung, string user);
    }
}
