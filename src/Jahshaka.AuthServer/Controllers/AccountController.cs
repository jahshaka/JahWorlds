using System;
using System.Threading.Tasks;
using Jahshaka.Core.Data;
using Jahshaka.AuthServer.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using AspNet.Security.OpenIdConnect.Extensions;
using OpenIddict.Core;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Jahshaka.AuthServer.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly HttpClient _httpClient;
        protected ILogger _logger;
        private static bool _databaseChecked;

        private string baseUrl = "http://localhost:5000";

        public AccountController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext applicationDbContext,
            IOptions<IdentityOptions> identityOptions,
            ILoggerFactory loggerFactory,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
            _signInManager = signInManager;
            _identityOptions = identityOptions;

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            _logger = loggerFactory.CreateLogger<AccountController>();
        }


        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    //_logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                /*if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
                }*/
                if (result.IsLockedOut)
                {
                    //_logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AddPassword(string returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPassword(AddPasswordViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return BadRequest();
                }

                if(user.PasswordHash != null){
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.ServerError,
                        ErrorDescription = "The specified user is already has a password."
                    });
                }

                //var passwordHash = _userManager.PasswordHasher.HashPassword(model.Password);

                var result = await _userManager.AddPasswordAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //_logger.LogInformation("User logged in.");
                    return RedirectToLocal(null);
                } else {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.ServerError,
                        ErrorDescription = "Password could not be added."
                    });
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        /*
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                //_logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                //_logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                //_logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }*/

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        /*[HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                //_logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                //_logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
               // _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }*/

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            EnsureDatabaseCreated(_applicationDbContext);
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CreatedAt = DateTime.UtcNow
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok();
                }
                AddErrors(result);
            }

            // If we got this far, something failed.
            return BadRequest(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

         [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            //_logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet, Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }            

            var address = info.Principal.FindFirstValue(ClaimTypes.Email);
            var lastname = info.Principal.FindFirstValue(ClaimTypes.Surname);
            var firstname = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            //_logger.LogInformation($"User Email Address: {address}");
            //_logger.LogInformation($"User lastname: {lastname}");
            //_logger.LogInformation($"User firstname: {firstname}");

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }

            var user = _applicationDbContext.Users.FirstOrDefault(u => u.Email == address);

            if(user == null){

                var newUser = new ApplicationUser { 
                    UserName = address, 
                    Email = address, 
                    LastName = lastname, 
                    FirstName = firstname,
                    CreatedAt = DateTime.UtcNow
                };
                
                var res = await _userManager.CreateAsync(newUser);

                user = _applicationDbContext.Users.FirstOrDefault(u => u.Email == newUser.Email);

                _logger.LogCritical($"New user data: {newUser}");

                /*
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.ServerError,
                    ErrorDescription = "The specified user is not allowed to sign in."
                });
                */
            }

            //if(await _userManager.HasPasswordAsync(user)) {
            if(user.PasswordHash == null) {
                return RedirectToAction(nameof(AddPassword));
            }

            var data = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:external_account"),
                new KeyValuePair<string, string>("user_id", user.Id.ToString() ),
                new KeyValuePair<string, string>("concurrency_stamp", user.ConcurrencyStamp )
            };

            //var body = JsonConvert.SerializeObject(data);

            var response = await _httpClient.PostAsync("connect/token", new FormUrlEncodedContent(data));

            var content = await response.Content.ReadAsStringAsync();

            _logger.LogDebug($"Conetnt:   {content}");
            _logger.LogDebug($"Status Code:   {response.StatusCode}");
            _logger.LogDebug($"URI:   {response.RequestMessage.RequestUri}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Unable to submit request: {content}.");
            }

            var resp = JsonConvert.DeserializeObject<ExternalUserTokenViewModel>(content);

            return Ok(resp);

                
        }

        [HttpPost, Produces("application/json")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();

                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        var userData = _applicationDbContext.Users.FirstOrDefault(u => u.Email == user.Email);

                        if(userData == null){
                            return BadRequest(new OpenIdConnectResponse
                            {
                                Error = OpenIdConnectConstants.Errors.ServerError,
                                ErrorDescription = "The specified user is not allowed to sign in."
                            });
                        }

                        if(user.PasswordHash == null){
                            return RedirectToAction(nameof(AddPassword));
                        }

                        var data = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:external_account"),
                            new KeyValuePair<string, string>("user_id", userData.Id.ToString() ),
                            new KeyValuePair<string, string>("concurrency_stamp", userData.ConcurrencyStamp )
                        };

                        //var body = JsonConvert.SerializeObject(data);

                        var response = await _httpClient.PostAsync("connect/token", new FormUrlEncodedContent(data));

                        var content = await response.Content.ReadAsStringAsync();

                        _logger.LogDebug($"Conetnt:   {content}");
                        _logger.LogDebug($"Status Code:   {response.StatusCode}");
                        _logger.LogDebug($"URI:   {response.RequestMessage.RequestUri}");

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception($"Unable to submit request: {content}.");
                        }

                        var resp = JsonConvert.DeserializeObject<ExternalUserTokenViewModel>(content);

                        return Ok(resp);
                        
                        //_logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        //return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            _logger.LogInformation($"MODEL DATA {model}");

            return BadRequest(new OpenIdConnectResponse()
            {
                Error = OpenIdConnectConstants.Errors.ServerError,
                ErrorDescription = ModelState?.Values.FirstOrDefault(m => m.ValidationState == ModelValidationState.Invalid)?.Errors.FirstOrDefault()?.ErrorMessage
            });

            //ViewData["ReturnUrl"] = returnUrl;
            //return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /*
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        */

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }


        // The following code creates the database and schema if they don't exist.
        // This is a temporary workaround since deploying database through EF migrations is
        // not yet supported in this release.
        // Please see this http://go.microsoft.com/fwlink/?LinkID=615859 for more information on how to do deploy the database
        // when publishing your application.
        private static void EnsureDatabaseCreated(ApplicationDbContext context)
        {
            if (!_databaseChecked)
            {
                _databaseChecked = true;
                context.Database.EnsureCreated();
            }
        }

        #endregion

    }
}