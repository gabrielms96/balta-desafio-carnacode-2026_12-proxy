using DesignPatternChallengProxy.Proxy;
using DesignPatternChallengProxy.RealSubject;
using DesignPatternChallengProxy.Subject;

namespace DesignPatternChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Documentos Confidenciais ===\n");

            var auditLog = new List<string>();
            var realService = new DocumentService();
            var securityProxy = new SecurityProxyDocumentService(realService, auditLog);
            var cachingProxy = new CachingProxyDocumentService(securityProxy);
            IDocumentService service = cachingProxy;

            var manager = new User("joao.silva", 5);
            var employee = new User("maria.santos", 2);

            Console.WriteLine("\n--- Gerente acessando documento de alto nível ---");
            var doc1 = service.ViewDocument("DOC002", manager);

            Console.WriteLine("\n--- Funcionário tentando acessar mesmo documento ---");
            var doc2 = service.ViewDocument("DOC002", employee);

            Console.WriteLine("\n--- Gerente acessando novamente (deveria usar cache) ---");
            var doc3 = service.ViewDocument("DOC002", manager);

            Console.WriteLine("\n--- Funcionário acessando documento permitido ---");
            var doc4 = service.ViewDocument("DOC003", employee);

            service.ShowAuditLog();
        }
    }
}