using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace КП1
{
    static class Hash
    {
        public static byte[] EncryptPassword(string userName, string password)
        {
            //пароль+ соль из логина
            string tmpPassword = userName.ToLower() + password;

            //Строка пароля преобразуется в массив байтов.
            UTF8Encoding textConverter = new UTF8Encoding();
            byte[] passBytes = textConverter.GetBytes(tmpPassword);

            //Возвращаются зашифрованные байты
            return new SHA384Managed().ComputeHash(passBytes);

        }

        // Сравниваются два массива на равенство
        // Можно добавить сравнение длины, но обычно
        // размер всех хешей одинаков.
        public static bool PasswordsMatch(byte[] psswd1, byte[] psswd2)
        {
            try
            {
                for (int i = 0; i < psswd1.Length; i++)
                {
                    if (psswd1[i] != psswd2[i])
                        return false;
                }
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }
    }
}
