using System;
using System.Security;
using Microsoft.SharePoint.Client;
using System.Threading;

namespace SPO.HideField
{
    static class SharePointContext
    {
        public static ClientContext GetContext(string username, string url)
        {
            ClientContext ctx = null;
            bool correctPass = false;

            while (!correctPass)
                try
                {
                    Console.WriteLine("Please enter your Office 365 password: ");

                    ConsoleKeyInfo pressedKey;
                    string SpOnlinePassword = string.Empty;

                    do
                    {
                        pressedKey = Console.ReadKey(true);
                        if (pressedKey.Key != ConsoleKey.Backspace && pressedKey.Key != ConsoleKey.Enter)
                        {
                            SpOnlinePassword += pressedKey.KeyChar;
                            Console.Write("*");
                        }
                        else
                        {
                            if (pressedKey.Key == ConsoleKey.Backspace && SpOnlinePassword.Length > 0)
                            {
                                SpOnlinePassword = SpOnlinePassword.Substring(0, (SpOnlinePassword.Length - 1));
                                Console.Write("\b \b");
                            }
                        }
                    } while (pressedKey.Key != ConsoleKey.Enter);

                    Console.WriteLine();

                    SecureString passwordSecureString = new SecureString();

                    foreach (char c in SpOnlinePassword)
                        passwordSecureString.AppendChar(c);

                    ctx = new ClientContext(url);
                    ctx.Credentials = new SharePointOnlineCredentials(username, passwordSecureString);
                    ctx.ExecuteQuery();

                    correctPass = true;

                    Console.WriteLine("Successfully connected to SharePoint Online\n");
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }

            return ctx;
        }
    }

    static class ContextExtension
    {
        public static void ExecuteQueryWithRetry(this ClientContext context, int retryCount, int delay)
        {
            int retryAttempts = 0;
            int backoffInterval = delay;
            if (retryCount <= 0)
                throw new ArgumentException("Provide a retry count greater than zero.");
            if (delay <= 0)
                throw new ArgumentException("Provide a delay greater than zero.");

            while (retryAttempts < retryCount)
            {
                try
                {
                    context.ExecuteQuery();
                    return;
                }
                catch (Exception ex)
                {
                    MessageLog.Write(ex.Message);
                    Thread.Sleep(backoffInterval);
                    retryAttempts++;
                }
            }
        }
    }
}
