namespace Sis_Pdv_Controle_Estoque_API.Services.Auth
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}