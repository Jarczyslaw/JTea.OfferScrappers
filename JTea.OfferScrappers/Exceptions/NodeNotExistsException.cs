namespace JTea.OfferScrappers.Exceptions
{
    internal class NodeNotExistsException : ScrapperParsingException
    {
        public NodeNotExistsException(string nodeName)
            : base($"Node {nodeName} not exists")
        {
        }
    }
}