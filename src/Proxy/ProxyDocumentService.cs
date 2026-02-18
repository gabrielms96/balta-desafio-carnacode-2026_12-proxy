using DesignPatternChallengProxy.RealSubject;
using DesignPatternChallengProxy.Subject;

namespace DesignPatternChallengProxy.Proxy
{
    public class ProxyDocumentService : IDocumentService
    {
        DocumentService _documentService;
        public ProxyDocumentService(string documentId, User user)
        {
            _documentService ??= new DocumentService();
            _documentService.ViewDocument(documentId, user);
        }

        public void EditDocument(string documentId, User user, string newContent)
        {
            _documentService.EditDocument(documentId, user, newContent);

        }

        public void ShowAuditLog()
        {
            _documentService.ShowAuditLog();
        }

        public ConfidentialDocument ViewDocument(string documentId, User user)
        {
            return _documentService.ViewDocument(documentId, user);
        }
    }
}
