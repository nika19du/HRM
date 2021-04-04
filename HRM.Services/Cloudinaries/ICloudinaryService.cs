﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.Cloudinaries
{
    public interface ICloudinaryService
    { 
        Task<string> UploadPictureAsync(IFormFile pictureFile, string fileName);
    }
}
