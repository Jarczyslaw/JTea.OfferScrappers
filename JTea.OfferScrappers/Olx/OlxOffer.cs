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
                builder.AppendLine($"Stan: {Condition}");
                builder.AppendLine($"Do negocjacji: {(!string.IsNullOrEmpty(ToNegotiate) ? "Tak" : "Nie")}");
                builder.Append($"Lokalizacja: {LocationAndDate}");

                return builder.ToString();
            }
        }

        public string LocationAndDate { get; set; }

        public string ToNegotiate { get; set; }
    }
}