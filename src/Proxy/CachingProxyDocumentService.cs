using DesignPatternChallengProxy.RealSubject;
using DesignPatternChallengProxy.Subject;

namespace DesignPatternChallengProxy.Proxy
{
    public class CachingProxyDocumentService : IDocumentService
    {
        private readonly IDocumentService _innerProxy;
        private readonly Dictionary<string, ConfidentialDocument> _cache;

        public CachingProxyDocumentService(IDocumentService innerProxy)
        {
            _innerProxy = innerProxy;
            _cache = new Dictionary<string, ConfidentialDocument>();
        }

        public void EditDocument(string documentId, User user, string newContent)
        {
            _innerProxy.EditDocument(documentId, user, newContent);
        }

        public void ShowAuditLog()
        {
            _innerProxy.ShowAuditLog();
        }

        public ConfidentialDocument ViewDocument(string documentId, User user)
        {
            if (_cache.TryGetValue(documentId, out var cached))
                return cached;

            var doc = _innerProxy.ViewDocument(documentId, user);
            if (doc != null)
                _cache[documentId] = doc;

            return doc;
        }
    }
}
