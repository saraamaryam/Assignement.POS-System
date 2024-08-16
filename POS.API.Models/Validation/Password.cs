using System.Text.RegularExpressions;
namespace POS.API.Models.Validation
{
    public static class Password
    {
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        //Function to Decode Password
        public static string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] byte_toDecode = Convert.FromBase64String(encodedData);
            int characterCount = utf8Decode.GetCharCount(byte_toDecode, 0, byte_toDecode.Length);
            char[] decode_char = new char[characterCount];
            utf8Decode.GetChars(byte_toDecode, 0, byte_toDecode.Length, decode_char, 0);
            string result = new String(decode_char);
            return result;
        }

        public static Regex ValidatePassword()
        {
            string regex = "(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,15})$";

            return new Regex(regex, RegexOptions.IgnoreCase);
        }
    }
}
