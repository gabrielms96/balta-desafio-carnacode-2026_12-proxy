using DesignPatternChallengProxy.Subject;

namespace DesignPatternChallengProxy.RealSubject
{
    public class DocumentService : IDocumentService
    {

        private DocumentRepository _repository;
        private Dictionary<string, ConfidentialDocument> _cache;
        private List<string> _auditLog;


        public DocumentService()
        {
            _repository = new DocumentRepository(); // Conexão custosa sempre criada
            _cache = new Dictionary<string, ConfidentialDocument>();
            _auditLog = new List<string>();
        }

        public ConfidentialDocument ViewDocument(string documentId, User user)
        {
            // Problema 1: Lógica de auditoria misturada
            var logEntry = $"[{DateTime.Now:HH:mm:ss}] {user.Username} tentou acessar {documentId}";
            _auditLog.Add(logEntry);
            Console.WriteLine($"[Audit] {logEntry}");

            // Problema 2: Verificação de acesso manual
            ConfidentialDocument doc;

            if (_cache.ContainsKey(documentId))
            {
                Console.WriteLine($"[Cache] Documento {documentId} encontrado no cache");
                doc = _cache[documentId];
            }
            else
            {
                doc = _repository.GetDocument(documentId);
                if (doc != null)
                {
                    _cache[documentId] = doc; // Cache manual
                }
            }

            if (doc == null)
            {
                Console.WriteLine($"❌ Documento {documentId} não encontrado");
                return null;
            }

            // Problema 3: Controle de acesso espalhado no código
            if (user.ClearanceLevel < doc.SecurityLevel)
            {
                Console.WriteLine($"❌ Acesso negado! Nível {user.ClearanceLevel} < Requerido {doc.SecurityLevel}");
                _auditLog.Add($"[{DateTime.Now:HH:mm:ss}] ACESSO NEGADO para {user.Username}");
                return null;
            }

            Console.WriteLine($"✅ Acesso permitido ao documento: {doc.Title}");
            return doc;
        }

        public void EditDocument(string documentId, User user, string newContent)
        {
            // Problema: Mesmo código de controle e auditoria repetido
            var logEntry = $"[{DateTime.Now:HH:mm:ss}] {user.Username} tentou editar {documentId}";
            _auditLog.Add(logEntry);
            Console.WriteLine($"[Audit] {logEntry}");

            var doc = _cache.ContainsKey(documentId)
                ? _cache[documentId]
                : _repository.GetDocument(documentId);

            if (doc == null || user.ClearanceLevel < doc.SecurityLevel)
            {
                Console.WriteLine($"❌ Operação não autorizada");
                return;
            }

            _repository.UpdateDocument(documentId, newContent);

            // Problema: Invalidar cache manualmente
            if (_cache.ContainsKey(documentId))
            {
                _cache.Remove(documentId);
            }

            Console.WriteLine($"✅ Documento atualizado");
        }

        public void ShowAuditLog()
        {
            Console.WriteLine("\n=== Log de Auditoria ===");
            foreach (var entry in _auditLog)
            {
                Console.WriteLine(entry);
            }
        }
    }
}
