namespace TimeTracking.Templates.Models
{
    public class ConfirmAccountEmailViewModel
    {
        public ConfirmAccountEmailViewModel()
        {
            
        }
        public ConfirmAccountEmailViewModel(string confirmEmailUrl)
        {
            ConfirmEmailUrl = confirmEmailUrl;
        }

        public string ConfirmEmailUrl { get; set; }
    }
}