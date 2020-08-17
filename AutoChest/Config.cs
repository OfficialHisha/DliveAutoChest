using System;
using DSharp.Dlive.Subscription;
using Newtonsoft.Json;

namespace AutoChest
{
    struct Config
    {
        public struct PercentagesStructure
        {
            public int Subscription { get; set; }
            [JsonProperty("ice_cream")]
            public int IceCream { get; set; }
            public int Diamond { get; set; }
            public int Ninjaghini { get; set; }
            public int Ninjet { get; set; }

            public int DonationPercentage(GiftType type)
            {
                return type switch
                {
                    GiftType.LEMON => 0,
                    GiftType.ICE_CREAM => IceCream,
                    GiftType.DIAMOND => Diamond,
                    GiftType.NINJAGHINI => Ninjaghini,
                    GiftType.NINJET => Ninjet,
                    _ => throw new ArgumentException($"Invalid argument: {type}!"),
                };
            }
        }

        public string DisplayName { get; set; }
        public string AuthToken { get; set; }
        public int Percentage { get; set; }
        public PercentagesStructure Percentages { get; set; }

        public int GetPercentageForGiftType(GiftType giftType)
        {
            if (Percentage > 0) return Percentage;

            return Percentages.DonationPercentage(giftType);
        }

        public int GetPercentageForSub()
        {
            if (Percentage > 0) return Percentage;

            return Percentages.Subscription;
        }
    }
}
