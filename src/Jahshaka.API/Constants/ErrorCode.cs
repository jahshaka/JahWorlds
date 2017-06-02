namespace Jahshaka.API.Constants
{
    public class ErrorCode
    {
        /// <summary>
        /// An unrecoverable error in the application 
        /// </summary>
        public const string ServerError = "server_error";

        /// <summary>
        /// The requested resource could not be found
        /// </summary>
        public const string NotFound = "not_found";

        /// <summary>
        /// The requested resource could not be found
        /// </summary>
        public const string ModelError = "model_error";

        /// <summary>
        /// Access to the requested resource is denied
        /// </summary>
        public const string AccessDenied = "access_denied";

        /// <summary>
        /// Account balance is insufficient
        /// </summary>
        public const string InsufficientBalance = "insufficient_balance";

        /// <summary>
        /// Incorrect password
        /// </summary>
        public const string IncorrectPassword = "incorrect_password";

        /// <summary>
        /// Password is not set for user
        /// </summary>
        public const string PasswordNotSet = "password_not_set";

        /// <summary>
        /// Incorrect pin code
        /// </summary>
        public const string IncorrectPinCode = "incorrect_pin_code";

        /// <summary>
        /// Pin code is not set for user
        /// </summary>
        public const string PinCodeNotSet = "pin_code_not_set";

        /// <summary>
        /// Email address is in use
        /// </summary>
        public const string EmailAddressInUse = "email_address_in_use";

        /// <summary>
        /// Mobile number is in use
        /// </summary>
        public const string MobileNumberInUse = "mobile_number_in_use";


        /// <summary>
        /// Coinbase two factor is required
        /// </summary>
        public const string CoinbaseTwoFactor = "two_factor_required";

        /// <summary>
        /// Operation is not allowed
        /// </summary>
        public const string NotAllowed = "not_allowed";

        /// <summary>
        /// The user's tier does not have the required permission
        /// </summary>
        public const string PermissionRequired = "permission_required";
    }        
}
