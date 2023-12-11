using green_craze_be_v1.Application.Model.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IMailService
    {
        void SendMail(CreateMailRequest request);
    }
}