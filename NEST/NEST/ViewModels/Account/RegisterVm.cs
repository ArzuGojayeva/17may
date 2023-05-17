using System.ComponentModel.DataAnnotations;

namespace NEST.ViewModels.Account
{
    public class RegisterVm
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; }= null!;
        public string UserName { get; set; } = null!;
        [MaxLength(255),DataType(DataType.EmailAddress)]
        public string Email { get; set; }=null!;
        [MaxLength(255), DataType(DataType.Password),Compare(nameof(Password))]
        public string Password { get; set; }=null !;
        public string ConfirmPassword { get; set; }
    }
}
