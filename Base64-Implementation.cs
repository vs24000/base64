using System;
//using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

class Base64
{
    static void Main()
    {
        Console.Write("Enter a string: ");
        var str = Console.ReadLine();

          var timer = new Stopwatch();
          timer.Start();
        var msg = new List<byte>(Encoding.UTF8.GetBytes(str));
        string r = Base64Encode(msg);
        Console.WriteLine($"'{r}'");
          timer.Stop();
          Console.WriteLine($"MY time = {timer.Elapsed.Microseconds}");

        timer.Start();
        var oringinalEncoder = Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        Console.WriteLine($"'{oringinalEncoder}'");
        timer.Stop();
        Console.WriteLine($"LIB time = {timer.Elapsed.Microseconds}");

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
}
