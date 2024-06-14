using System.Text;

namespace JTea.OfferScrappers.OtoDom
{
    public class OtoDomOffer : Offer
    {
        public override string Description
        {
            get
            {
                var builder = new StringBuilder();
                builder.AppendLine($"Location: {Location}");
                builder.AppendLine($"Specification: {Specification}");
                builder.AppendLine($"Is part of investment: {(IsPartOfInvestment ? "Tak" : "Nie")}");

                return builder.ToString();
            }
        }

        public bool IsPartOfInvestment { get; set; }

        public override bool IsValid
            => base.IsValid
                && !string.IsNullOrEmpty(Location);

        public string Location { get; set; }

        public string Specification { get; set; }
    }
}