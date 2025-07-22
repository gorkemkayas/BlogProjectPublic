using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Interfaces
{
    public interface IFeedbackService
    {
        Task<bool> SubmitFeedbackAsync(string message);
    }
}
