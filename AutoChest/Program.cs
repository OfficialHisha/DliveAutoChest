using System;
using System.IO;
using DSharp.Dlive;
using DSharp.Dlive.Query;
using DSharp.Dlive.Subscription;
using DSharp.Dlive.Subscription.Chat;
using Newtonsoft.Json;

namespace AutoChest
{
    class Program
    {
        private static Config Config { get; set; } = null;
        private static DliveAccount Account { get; set; } = null;

        static void Main()
        {
            PrintStatus("Loading config..", ConsoleColor.Green);
            try
            {
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "config.json")));
            }
            catch (JsonReaderException e)
            {
                SetError(e.Message);
            }
            catch (FileNotFoundException ex)
            {
                SetError(ex.Message);
            }

            if (string.IsNullOrWhiteSpace(Config.DisplayName))
                SetError("No display name specified!");

            if (string.IsNullOrWhiteSpace(Config.AuthToken))
                SetError("No authentication token provided!");

            PrintStatus("Setting up account..", ConsoleColor.Green);
            Account = new DliveAccount(Config.AuthToken);
            try
            {
                Account.Query.GetMyInfo();
            }
            catch (JsonSerializationException)
            {
                SetError("Invalid authentication token provided!");
            }
            catch (Exception e)
            {
                SetError(e.Message);
            }

            PublicUserData usr = PublicQuery.GetPublicInfoByDisplayName(Config.DisplayName);

            if (usr.Username == "invalid user")
                SetError("Invalid display name provided!");

            PrintStatus("Connecting to event stream..", ConsoleColor.Green);
            Subscription subscription = Subscription.SubscriptionByDisplayName(Config.DisplayName);
            subscription.OnChatEvent += OnChat;
            subscription.OnConnected += () => PrintStatus($"Connected to {Config.DisplayName}!", ConsoleColor.Green);
            subscription.OnError += (err) => SetError(err);
            subscription.Connect();

            Console.WriteLine("Press enter to stop the program");
            Console.ReadLine();
        }

        static void PrintStatus(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"[STATUS]: {message}");
            Console.ResetColor();
        }

        static void SetError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR]: {message}");
            Console.ResetColor();
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();

            Environment.Exit(13);
        }

        static void OnChat(ChatMessage message)
        {
            switch (message.EventType)
            {
                case ChatEventType.GIFT:
                    ChatGiftMessage giftMessage = message as ChatGiftMessage;

                    int amountToAdd = SplitDonation(giftMessage.GiftLemonValue);

                    Account.Mutation.AddChestValue(amountToAdd);
                    Console.WriteLine($"{giftMessage.User.Displayname} gifted {giftMessage.GiftLemonValue} Lemon! Added {amountToAdd} Lemon to the chest");
                    break;
                case ChatEventType.SUBSCRIPTION:
                    ChatSubscriptionMessage subMessage = message as ChatSubscriptionMessage;
                    int toAdd = SplitDonation(298);

                    Account.Mutation.AddChestValue(toAdd);
                    Console.WriteLine($"{subMessage.User.Displayname} just subscribed! Added {toAdd} Lemon to the chest");
                    break;
                default:
                    break;
            }
        }

        static int SplitDonation(int value)
        {
            return (int)(value * (Config.Percentage / 100f));
        }
    }
}