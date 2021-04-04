using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Services.Mapping
{
    public interface IHaveCustomMappings
    {
        void CreateMappings(IProfileExpression configuration);
    }
}
