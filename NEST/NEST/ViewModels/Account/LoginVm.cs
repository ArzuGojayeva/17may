using System.ComponentModel.DataAnnotations;

namespace NEST.ViewModels.Account
{
    public class LoginVm
    {
        public string UserNameorEmail { get; set; } = null!;
        [MaxLength(255), DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}
