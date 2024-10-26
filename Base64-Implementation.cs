using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

class Base64
{
    static void Main()
    {        
        Console.Write("Enter a string: ");
        var str = Console.ReadLine();

        /* -- тест на енкодера */
        var msg = new List<byte>(Encoding.UTF8.GetBytes(str));
        string encodedStr = Base64Encode(msg);
        Console.WriteLine($"'{encodedStr}'");

        /* -- тест на декодера */
        string decodedStr = Encoding.UTF8.GetString(Base64Decode(encodedStr).ToArray());
        Console.WriteLine($"'{decodedStr}'");
    }

    const string base64Chars = "АBCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
    static string Base64Encode(List<byte> message)
    {
        if (message == null) throw new ArgumentException("Null pointer.");
        if (message.Count == 0) return string.Empty;

        // изчислява колко е дълга подложката и добадя нули до съобщение кратно на 3.
        int padCount = 0;
        while (message.Count % 3 != 0)
        {
            message.Add(0);
            padCount++;
        }
        
        // за резултата
        var result = new StringBuilder();
        
        // чете в групи по три байта, изхода:
        // b1 - съдържа старшите 6 бита от msg[i]
        // b2 - младшите 2 от msg[i] и старшите 4 от msg[i+1]
        // b2 - младшите 4 бита от msg[i+1] и старшите 2 от msg[i+2]
        // b3 - останалите битове от msg[i+2]
        for (int i = 0; i < message.Count; i += 3)
        {
            byte b1, b2, b3, b4;

            b1 = (byte) (message[i] >> 2);
            b2 = (byte) (((message[i] & 0b00000011) << 4) | (message[i + 1] >> 4));
            b3 = (byte) (((message[i + 1] & 0b00001111) << 2) | (message[i + 2] >> 6));
            b4 = (byte) (message[i + 2] & 0b00111111);

            result.Append(base64Chars[b1]);
            result.Append(base64Chars[b2]);
            result.Append(base64Chars[b3]);
            result.Append(base64Chars[b4]);
        }

        if (padCount > 0)
        {
            // изтрива последните padCount байта, те ще са 'А'
            result.Length = result.Length - padCount;
            for (int i = 0; i < padCount; i++)
            {
                // допълва със знака "няма симнол" за коректно декодиране
                result.Append('=');
            }
        }

        return result.ToString();
    }

    static List<byte> Base64Decode(string message)
    {
        if (message == null) throw new ArgumentException("Null pointer.");
        var result = new List<byte>();
        if (message.Length == 0) return result; // празен списък

        // изчислява padCount и заменя '=' с 'А'
        int padCount = 0, pEnd = message.Length - 1;
        while (message[pEnd] == '=') { pEnd--; padCount++; }
        if (padCount > 0) message = message.Replace('=', 'A');

        for (int i = 0; i < message.Length; i += 4)
        {
            int b1 = base64Chars.IndexOf(message[i]);
            int b2 = base64Chars.IndexOf(message[i + 1]);
            int b3 = base64Chars.IndexOf(message[i + 2]);
            int b4 = base64Chars.IndexOf(message[i + 3]);

            byte r1 = (byte)((b1 << 2) | (b2 >> 4));
            byte r2 = (byte)(((b2 & 0b00001111) << 4) | (b3 >> 2));
            byte r3 = (byte)(((b3 & 0b00000011) << 6) | b4);

            result.Add(r1);
            if (b3 != 64)
            {
                result.Add(r2);
            }
            if (b4 != 64)
            {
                result.Add(r3);
            }
        }

        // Премахване на добавените байтове
        result.RemoveRange(result.Count - padCount, padCount);

        return result;
    }
}