namespace VisitorBook.Test.ControllerTests
{
    public class AuthControllerTest
    {
        #region Login 

        public void Login_ActionExecutes_ReturnView()
        {

        }

        public async void Login_ModelIsInvalid_ReturnView()
        {

        }

        public async void Login_UserIsNull_ReturnView()
        {

        }

        public async void Login_VisitorRecorderEmailNotConfirmed_ReturnView()
        {

        }

        public async void Login_OtherUsersEmailNotConfirmed_ReturnView()
        {

        }

        public async void Login_SignInResultIsSucceeded_ReturnRedirectToReturnUrl()
        {

        }

        public async void Login_SignInResultIsLockedOut_ReturnView()
        {

        }

        public async void Login_SignInResultIsFailedWithCount_ReturnView()
        {

        }

        #endregion

        #region Register 

        public void Register_ActionExecutes_ReturnView()
        {

        }

        public async void Register_ModelIsInvalid_ReturnView()
        {

        }

        public async void Register_UserCreateIsFailed_ReturnView()
        {

        }

        public async void Register_UserAddToRoleIsFailed_ReturnView()
        {

        }

        public async void Register_EmailSendIsSucceeded_ReturnView()
        {

        }

        public async void Register_EmailSendIsFailed_ReturnView()
        {

        }

        #endregion

        #region Register Confirmation

        public async void RegisterConfirmation_UserIdOrTokenIsNull_ReturnRedirectToHomeControllerIndexAction()
        {

        }

        public async void RegisterConfirmation_UserIsNull_ReturnRedirectToHomeControllerIndexAction()
        {

        }

        public async void RegisterConfirmation_UserTokenVerificationResultIsFalse_ReturnRedirectToHomeControllerIndexAction()
        {

        }

        public async void RegisterConfirmation_UserEmailConfirmationResultIsSucceeded_ReturnRedirectToReturnUrl()
        {

        }

        public async void RegisterConfirmation_UserEmailConfirmationResultIsFailed_ReturnRedirectToHomeControllerIndexAction()
        {

        }

        #endregion

        #region Forgot Password

        public void ForgotPassword_ActionExecutes_ReturnView()
        {

        }

        public async void ForgotPassword_ModelIsInvalid_ReturnView()
        {

        }

        public async void ForgotPassword_EmailSendIsSucceeded_ReturnView()
        {

        }

        public async void ForgotPassword_EmailSendIsFailed_ReturnView()
        {

        }

        #endregion

        #region Reset Password

        public void ResetPassword_ActionExecutes_ReturnView()
        {

        }

        public async void ResetPassword_ModelIsInvalid_ReturnView()
        {

        }

        public async void ResetPassword_UserIdOrTokenIsNull_ReturnView()
        {

        }

        public async void ResetPassword_UserIsNull_ReturnView()
        {

        }

        public async void ResetPassword_UserTokenVerificationResultIsFalse_ReturnView()
        {

        }

        public async void ResetPassword_ResetPasswordIsFailed_ReturnView()
        {

        }

        public async void ResetPassword_ResetPasswordIsSucceeded_ReturnView()
        {

        }

        #endregion

        #region Register Application

        public void RegisterApplication_ActionExecutes_ReturnView()
        {

        }

        public async void RegisterApplication_ModelIsInvalid_ReturnView()
        {

        }

        public async void RegisterApplication_UserCreateIsFailed_ReturnView()
        {

        }

        public async void RegisterApplication_UserAddToRoleIsFailed_ReturnView()
        {

        }

        public async void RegisterApplication_RegisterApplicationResultIsSucceeded_ReturnView()
        {

        }

        public async void RegisterApplication_RegisterApplicationResultIsFailed_ReturnView()
        {

        }

        #endregion
    }
}
