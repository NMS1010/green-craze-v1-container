namespace green_craze_be_v1.Application.Intefaces
{
    public interface ITokenService
    {
        public string GenerateOTP(int digitNumber = 6);
    }
}