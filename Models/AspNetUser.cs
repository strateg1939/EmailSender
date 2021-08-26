using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

#nullable disable

namespace EmailSender.Models
{
    public class AspNetUser : IdentityUser
    {       
        public ICollection<connection_user_topic> connection_User_Topics { get; set; }
        public ICollection<connection_user_article> connection_User_Articles { get; set; }
    }
}
