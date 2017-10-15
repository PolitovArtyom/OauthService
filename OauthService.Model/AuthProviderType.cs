using System;
using System.ComponentModel;

namespace OauthService.Model
{
    public enum AuthProviderType
    {
        [DisplayName("None")] 
        None = 0,

        [DisplayName("TestProivder")]
        TestProivder = 1,
    }
}