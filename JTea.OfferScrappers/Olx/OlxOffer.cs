using System.Text;

namespace JTea.OfferScrappers.Olx
{
    public class OlxOffer : Offer
    {
        public string Condition { get; set; }

        public override string Description
        {
            get
            {
                var builder = new StringBuilder();
                builder.AppendLine($"State: {Condition}");
                builder.AppendLine($"To negotiate: {(!string.IsNullOrEmpty(ToNegotiate) ? "Tak" : "Nie")}");
                builder.Append($"Location and date: {LocationAndDate}");

                return builder.ToString();
            }
        }

        public override bool IsValid
            => base.IsValid
                && !string.IsNullOrEmpty(LocationAndDate);

        public string LocationAndDate { get; set; }

        public string ToNegotiate { get; set; }
    }
}