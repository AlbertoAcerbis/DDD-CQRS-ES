using System;
using System.IdentityModel.Tokens.Jwt;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FourSolid.Cqrs.OrdiniClienti.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class CommonController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        protected string Authorization;
        /// <summary>
        /// 
        /// </summary>
        protected JwtSecurityTokenHandler JwtSecurityTokenHandler;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual CommandInfo DecodeJwtToken()
        {
            var when = new When(DateTime.UtcNow);
            var accountId = Guid.NewGuid().ToString();
            var accountName = "Unknown";
            var accountRole = string.Empty;

            try
            {
                this.Authorization = this.Request.Headers["Authorization"];
                this.Authorization = this.Authorization.Replace("Bearer ", "");
                this.JwtSecurityTokenHandler = new JwtSecurityTokenHandler();

                if (!(this.JwtSecurityTokenHandler.ReadToken(this.Authorization) is JwtSecurityToken jwt))
                {
                    return new CommandInfo(AccountInfoFactory(accountId, accountName, accountRole), when);
                }

                foreach (var claim in jwt.Claims)
                {
                    if (claim.Type.ToLower() == "accountid")
                        accountId = claim.Value;

                    if (claim.Type.ToLower() == "accountname")
                        accountName = claim.Value;

                    if (claim.Type.ToLower() == "role")
                        accountRole = claim.Value;
                }

                return new CommandInfo(AccountInfoFactory(accountId, accountName, accountRole), when);
            }
            catch
            {
                this.Authorization = string.Empty;
            }

            return new CommandInfo(AccountInfoFactory(accountId, accountName, accountRole), when);
        }

        protected static Uri GetUri(HttpRequest request)
        {
            var builder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Value,
                Path = request.Path,
                Query = request.QueryString.ToUriComponent()
            };
            return builder.Uri;
        }

        #region Helpers
        private static AccountInfo AccountInfoFactory(string accountId, string accountName, string accountRole)
        {
            return new AccountInfo(new AccountId(accountId), new AccountName(accountName),
                new AccountRole(accountRole));
        }
        #endregion
    }
}