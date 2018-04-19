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
        /// Operation is not allowed
        /// </summary>
        public const string NotAllowed = "not_allowed";
    }        
}
