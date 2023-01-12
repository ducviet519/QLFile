using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface ISoftwareServices
    {
        public Task<List<Software>> GetSoftwareAsync();
    }
}
