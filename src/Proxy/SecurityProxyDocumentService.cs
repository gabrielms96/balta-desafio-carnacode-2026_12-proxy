// 1. Security Proxy (Protection)
using DesignPatternChallengProxy.RealSubject;
using DesignPatternChallengProxy.Subject;

public class SecurityProxyDocumentService : IDocumentService
{
    private readonly DocumentService _realService;
    private readonly List<string> _auditLog;

    public SecurityProxyDocumentService(DocumentService realService, List<string> auditLog)
    {
        _realService = realService;
        _auditLog = auditLog;
    }

    public void EditDocument(string documentId, User user, string newContent)
    {
        _realService.EditDocument(documentId, user, newContent);
    }

    public void ShowAuditLog()
    {
        _realService.ShowAuditLog();
    }

    public ConfidentialDocument ViewDocument(string documentId, User user)
    {

        return _realService.ViewDocument(documentId, user);
    }
}
