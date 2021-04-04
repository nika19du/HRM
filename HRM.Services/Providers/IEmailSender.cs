using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.Providers
{
    public interface IEmailSender
    {
        void SendEmail(Message message);

    }
}
