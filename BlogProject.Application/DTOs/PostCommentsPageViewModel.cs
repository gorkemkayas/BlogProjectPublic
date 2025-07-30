using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.DTOs
{
    class PostCommentsPageViewModel
    {
        public List<CommentViewModel> Comments { get; set; }

        public CurrentUserDto? CurrentUser { get; set; } // Kullanıcı giriş yaptıysa
    }
}
