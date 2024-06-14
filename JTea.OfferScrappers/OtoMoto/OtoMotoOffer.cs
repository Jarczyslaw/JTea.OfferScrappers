using System.Text;

namespace JTea.OfferScrappers.OtoMoto
{
    public class OtoMotoOffer : Offer
    {
        public override string Description
        {
            get
            {
                var builder = new StringBuilder();
                builder.AppendLine($"Specification: {Specification}");
                builder.AppendLine($"Features: {Features}");
                builder.AppendLine($"Location and date: {LocationAndDate}");

                return builder.ToString();
            }
        }

        public string Features { get; set; }

        public override bool IsValid
            => base.IsValid
                && !string.IsNullOrEmpty(Specification);

        public string LocationAndDate { get; set; }

        public string Specification { get; set; }
    }
}