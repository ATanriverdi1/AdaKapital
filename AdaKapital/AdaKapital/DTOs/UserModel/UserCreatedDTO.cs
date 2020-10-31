using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaKapital.DTOs.UserModel
{
    public class UserCreatedDTO : UserPatchDTO
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ModifieldDate { get; set; }

        public UserCreatedDTO()
        {
            this.ModifieldDate = DateTime.Now;
        }
    }
}
