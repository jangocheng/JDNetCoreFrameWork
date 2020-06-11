// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.ApiSite.Middleware/IdentityExt 
// 创建人:             研小艾   
// 创建时间:           2020/5/6 13:37:18

using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
using IdentityServer4.Validation;
using JDNetCore.Common;
using JDNetCore.Common.Interface;
using JDNetCore.Model.DTO;
using JDNetCore.Service.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JDNetCore.ApiSite.Middleware
{
    public static class IdentityExt
    {
        static IdentityExt()
        {
            TokenPath = Appsettings.app<string>("Identity:Authority") + Appsettings.app<string>("Identity:TokenPath");
        }
        public static void AddIdentityServerSetup(this IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(IdentityConfig.GetApiResource())
                .AddInMemoryIdentityResources(IdentityConfig.GetIdentityResource())
                .AddInMemoryClients(IdentityConfig.GetClients())
                //.AddCookieAuthentication()//为了hangfire 可以登陆
                //.AddTestUsers(IdentityConfig.GetUsers())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = Appsettings.app<string>("Identity:Authority");
                    options.RequireHttpsMetadata = options.Authority.Contains("https");
                    options.Audience = Appsettings.app<string>("Identity:Audience");
                });
                //.AddIdentityServerAuthentication(options =>
                //{
                //    options.Authority = Appsettings.app<string>("Identity:Authority");
                //    options.RequireHttpsMetadata = options.Authority.Contains("https");
                //    options.ApiName = "job_api";
                //});

        }

        public static string TokenPath
        {
            private set;
            get;
        }

        public static JDNetCore.Model.DTO.Token GetToken(string userName=null,string pwd=null)
        {
            userName = userName ?? "admin";
            pwd = pwd ?? "Ww123123";
            var client = new RestClient(IdentityExt.TokenPath);
            var request = new RestRequest(Method.POST);
            request.AddHeader("ClientId", "JDNetCore");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("username", userName);
            request.AddParameter("client_id", "local");
            request.AddParameter("client_secret", "zy_try");
            request.AddParameter("grant_type", "password");
            request.AddParameter("password", pwd);
            var result = client.ExecuteAsync<JDNetCore.Model.DTO.Token> (request).Result.Data;
            return result;
        }
    }

    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IAccountService accountService;
        private readonly ICache cache;

        public ResourceOwnerPasswordValidator(IAccountService _accountService, ICache _cache)
        {
            accountService = _accountService;
            cache = _cache;
        }


        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var clilentId = context.Request.ClientId;
            //判断clientId合法性

            //从缓存获取用户
            AccountResult accountResult = new AccountResult();
            var ssouser = cache.Get<SSOUser>(context.UserName + "|" + context.Password);
            if (ssouser != null)
            {
                context.Result = GetUserClaims(ssouser);
                cache.Set(context.UserName, accountResult.user);
                return;
            }
            accountResult = await accountService.SignInAsync(context.UserName, context.Password);
            if (accountResult.state == Model.DTO.SSOState.success)
            {
                context.Result = GetUserClaims(accountResult.user);
                cache.Set(context.UserName + "|" + context.Password, accountResult.user);
            }
            else
            {
                //验证失败
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "用户验证失败");
            }
        }

        private static GrantValidationResult GetUserClaims(SSOUser ssouser)
        {
            return new GrantValidationResult(ssouser.username, "client_secret_basic", new Claim[] {
                    new Claim(JwtClaimTypes.PreferredUserName, ssouser.username),
                    new Claim(JwtClaimTypes.Id, ssouser.id),
                    new Claim(JwtClaimTypes.NickName, ssouser.nickname),
                    new Claim(JwtClaimTypes.Gender, ssouser.sex?"male":"female"),
                    new Claim(JwtClaimTypes.Picture, ssouser.headimgurl??""),
                    new Claim(JwtClaimTypes.Role,"admin"),
                    //获取用户所有的role
                    //获取用户所有的group
                    //获取用户所有的permission
                });
        }
    }

    public class IdentityConfig
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "admin",
                    Password = "Ww123123"
                }
            };
        }
        public static IEnumerable<IdentityResource> GetIdentityResource()
        {
            return new List<IdentityResource>
             {
                 new IdentityResources.OpenId(),
                 new IdentityResources.Profile(),
             };
        }
        public static IEnumerable<ApiResource> GetApiResource()
        {
            return new List<ApiResource>
            {
                new ApiResource("base_api", "defaultapi"),
                new ApiResource("job_api", "jobapi")
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                // resource owner password grant client
                new Client
                {
                    ClientId = "local",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireClientSecret = false,
                    ClientSecrets =
                    {
                        new Secret("zy_try".Sha256())
                    },
                    AllowedScopes = { 
                        "base_api",
                        IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                        IdentityServerConstants.StandardScopes.Profile 
                    }
                },
                new Client
                {
                    ClientId = "hangfire",//client_id hangfire
                    AllowedGrantTypes = GrantTypes.ClientCredentials,// grant_type client_credentials
                    ClientSecrets =
                    {
                        new Secret("hangfire".Sha256()) // client_secret hangfire
                    },
                    AllowedScopes = { "job_api" },
                    //AccessTokenType = AccessTokenType.Reference,
                }
            };
        }
    }
}
