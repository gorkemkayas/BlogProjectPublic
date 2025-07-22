using BlogProject.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Domain.Entities
{
    public class FeedbackEntity : BaseEntity
    {
        public string Message { get; set; }
    }
}
