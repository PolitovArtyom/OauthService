using System;
using System.ComponentModel;

namespace OauthService.Model
{
    public enum AuthResourceType
    {
        [DisplayName("None")] 
        None = 0,

        [DisplayName("Test")]
        Test = 1,
    }
}