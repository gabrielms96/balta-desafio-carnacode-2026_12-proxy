
using DesignPatternChallengProxy.RealSubject;

namespace DesignPatternChallengProxy.Subject
{
    public interface IDocumentService
    {
        ConfidentialDocument ViewDocument(string documentId, User user);
        void EditDocument(string documentId, User user, string newContent);
        void ShowAuditLog();
    }
}
