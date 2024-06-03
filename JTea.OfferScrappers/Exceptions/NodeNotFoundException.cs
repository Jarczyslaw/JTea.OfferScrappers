namespace JTea.OfferScrappers.Exceptions
{
    internal class NodeNotFoundException : ScrapperParsingException
    {
        public NodeNotFoundException(string nodeName)
            : base($"Can nod find node {nodeName}")
        {
        }
    }
}