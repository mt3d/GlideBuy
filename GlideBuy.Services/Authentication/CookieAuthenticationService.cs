using GlideBuy.Core.Domain.Customers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GlideBuy.Services.Authentication
{
    /**
     * A bridge between Customer (domain) and ASP.NET Core authentication infrastructure
     * (claims and cookies).
     */
    public class CookieAuthenticationService : IAuthenticationService
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected Customer? _cachedCustomer;

        public CookieAuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /**
         * Comparison with SignInManager.SignInAsync()
         * 
         *  public virtual Task SignInAsync(
         *  TUser user, AuthenticationProperties authenticationProperties, string? authenticationMethod = null)
         *  {
         *      // Omitted code... (additional claims for the authentication method)
         *      
         *      return SignInWithClaimsAsync(user, authenticationProperties, additionalClaims);
         *  }
         *  
         *  public virtual async Task SignInWithClaimsAsync(TUser user, AuthenticationProperties? authenticationProperties, IEnumerable<Claim> additionalClaims)
         *  {
         *      var userPrincipal = await CreateUserPrincipalAsync(user);
         *      foreach (var claim in additionalClaims)
         *      {
         *          userPrincipal.Identities.First().AddClaim(claim);
         *      }
         *      
         *      authenticationProperties ??= new AuthenticationProperties();
         *      
         *      await Context.SignInAsync(AuthenticationScheme, userPrincipal, authenticationProperties);
         *      
         *      // This is useful for updating claims immediately when hitting
         *      // MapIdentityApi's /account/info endpoint with cookies.
         *      Context.User = userPrincipal;
         *      
         *      // Omitted code... (for metrics)
         *  }
         *  
         *  public IUserClaimsPrincipalFactory<TUser> ClaimsFactory { get; set; }
         *  
         *  public virtual async Task<ClaimsPrincipal> CreateUserPrincipalAsync(TUser user)
         *      => await ClaimsFactory.CreateAsync(user);
         *      
         *  protected virtual async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
         *  {
         *      var userId = await UserManager.GetUserIdAsync(user).ConfigureAwait(false);
         *      var userName = await UserManager.GetUserNameAsync(user).ConfigureAwait(false);
         *      
         *      var id = new ClaimsIdentity("Identity.Application", // REVIEW: Used to match Application scheme
         *      Options.ClaimsIdentity.UserNameClaimType,
         *      Options.ClaimsIdentity.RoleClaimType);
         *      
         *      id.AddClaim(new Claim(Options.ClaimsIdentity.UserIdClaimType, userId));
         *      id.AddClaim(new Claim(Options.ClaimsIdentity.UserNameClaimType, userName!));
         *      
         *      if (UserManager.SupportsUserEmail)
         *      {
         *          var email = await UserManager.GetEmailAsync(user).ConfigureAwait(false);
         *          if (!string.IsNullOrEmpty(email))
         *          {
         *              id.AddClaim(new Claim(Options.ClaimsIdentity.EmailClaimType, email));
         *          }
         *      }
         *      
         *      // Omitted code... (claim for security stamp)
         *      
         *      if (UserManager.SupportsUserClaim)
         *      {
         *          id.AddClaims(await UserManager.GetClaimsAsync(user).ConfigureAwait(false));
         *      }
         *      
         *      return id;
         *  }
         */
        public virtual async Task SignInAsync(Customer customer, bool isPersistent)
        {
            ArgumentNullException.ThrowIfNull(customer);

            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(customer.UserName))
            {
                // TODO: Move issuer to a defaults class
                claims.Add(new Claim(ClaimTypes.Name, customer.UserName, ClaimValueTypes.String, "GlideBuy"));
            }

            if (!string.IsNullOrWhiteSpace(customer.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, customer.Email, ClaimValueTypes.Email, "GlideBuy"));
            }

            var userIdentity = new ClaimsIdentity(claims, AuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                IssuedUtc = DateTime.UtcNow,
            };

            // Forwards the call to:
            // => IAuthenticationService.SignInAsync()
            // => IAuthenticationSignInHandler.SignInAsync()
            await _httpContextAccessor.HttpContext.SignInAsync(AuthenticationDefaults.AuthenticationScheme, userPrincipal, authenticationProperties);

            _cachedCustomer = customer;
        }
    }
}
