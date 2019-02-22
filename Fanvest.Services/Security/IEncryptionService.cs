namespace Fanvest.Services.Security
{
    public partial interface IEncryptionService
    {
        string CreateSaltKey(int size);

        string CreatePasswordHash(string password, string saltkey, string passwordFormat);

        string CreateHash(byte[] data, string hashAlgorithm);

        string EncryptText(string plainText, string encryptionPrivateKey = "");

        string DecryptText(string cipherText, string encryptionPrivateKey = "");
    }
}