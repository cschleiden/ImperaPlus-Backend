﻿using System.Linq;
using System.Security.Claims;
using ImperaPlus.DataAccess;
using Microsoft.AspNetCore.Http;

namespace ImperaPlus.Web.Providers
{
    /// <summary>
    /// Provides the currently authenticated user
    /// </summary>
    public class UserProvider : IUserProvider
    {
        private IHttpContextAccessor httpContextAccessor;

        public UserProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            return this.httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}