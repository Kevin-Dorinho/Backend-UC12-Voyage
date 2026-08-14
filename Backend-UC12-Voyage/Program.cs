using System;
using System.Threading.Tasks;

bool rodando = true;

while (rodando)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("==================================================");
    Console.WriteLine("          SISTEMA DE GESTÃO DE VENDAS             ");
    Console.WriteLine("==================================================");
    Console.ResetColor();
    Console.WriteLine("1. Inserir Produto");
    Console.WriteLine("2. Buscar Produto por ID");
    Console.WriteLine("3. Listar Todos os Produtos");
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine("4. Inserir Usuário");
    Console.WriteLine("5. Buscar Usuário por ID");
    Console.WriteLine("6. Listar Todos os Usuários");
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine("7. Registrar Venda");
    Console.WriteLine("8. Buscar Venda por ID");
    Console.WriteLine("9. Listar Todas as Vendas");
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine("10. Inserir Pagamento");
    Console.WriteLine("11. Buscar Pagamento por ID");
    Console.WriteLine("12. Listar Todos os Pagamentos");
    Console.WriteLine("13. Alterar Pagamento");
    Console.WriteLine("14. Excluir Pagamento");
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine("0. Sair");
    Console.WriteLine("==================================================");
    Console.Write("Escolha uma opção: ");

    string opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
        case "2":
        case "3":
        case "4":
        case "5":
        case "6":
        case "7":
        case "8":
        case "9":
            Console.WriteLine("Funcionalidade em desenvolvimento...");
            break;
        case "10":
            await InserirPagamento();
            break;
        case "11":
            await BuscarPagamento();
            break;
        case "12":
            await ListarPagamentos();
            break;
        case "13":
            await AlterarPagamento();
            break;
        case "14":
            await ExcluirPagamento();
            break;
        case "0":
            rodando = false;
            break;
        default:
            Console.WriteLine("Opção inválida!");
            break;
    }

    if (rodando)
    {
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }
}

async Task InserirPagamento()
{
    Console.WriteLine("\n-- Inserir Pagamento --");
    try
    {
        Payment payment = new Payment();
        Console.Write("Company ID: ");
        payment.company_id = int.Parse(Console.ReadLine() ?? "0");
        
        Console.Write("Data de Início (YYYY-MM-DD): ");
        payment.to_date = DateTime.Parse(Console.ReadLine() ?? "");
        
        Console.Write("Data de Vencimento (YYYY-MM-DD): ");
        payment.due_date = DateTime.Parse(Console.ReadLine() ?? "");
        
        Console.Write("Forma de Pagamento: ");
        payment.payment_form = Console.ReadLine() ?? "";
        
        Console.Write("Publicidade: ");
        payment.advertising = Console.ReadLine() ?? "";
        
        Console.Write("Chave: ");
        payment.key = Console.ReadLine() ?? "";
        
        Console.Write("Tipo: ");
        payment.type = Console.ReadLine() ?? "";

        await payment.InserirAsync();
        Console.WriteLine("Pagamento inserido com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

async Task BuscarPagamento()
{
    Console.WriteLine("\n-- Buscar Pagamento --");
    try
    {
        Console.Write("ID do Pagamento: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        Payment payment = new Payment();
        await payment.BuscarAsync(id);

        if (payment.company_id != 0)
        {
            Console.WriteLine($"ID: {payment.id}, Company ID: {payment.company_id}, Forma de Pagamento: {payment.payment_form}");
        }
        else
        {
            Console.WriteLine("Pagamento não encontrado.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

async Task ListarPagamentos()
{
    Console.WriteLine("\n-- Listar Pagamentos --");
    try
    {
        Payment paymentModel = new Payment();
        var pagamentos = await paymentModel.BuscarTodosAsync();

        foreach (var payment in pagamentos)
        {
            Console.WriteLine($"ID: {payment.id} | Company ID: {payment.company_id} | Forma de Pgto: {payment.payment_form}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

async Task AlterarPagamento()
{
    Console.WriteLine("\n-- Alterar Pagamento --");
    try
    {
        Console.Write("ID do Pagamento a ser alterado: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        Payment payment = new Payment();
        await payment.BuscarAsync(id);

        if (payment.company_id != 0)
        {
            Console.Write($"Novo Company ID ({payment.company_id}): ");
            string? input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) payment.company_id = int.Parse(input);

            Console.Write($"Nova Data Início ({payment.to_date:yyyy-MM-dd}): ");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) payment.to_date = DateTime.Parse(input);

            Console.Write($"Nova Data Vencimento ({payment.due_date:yyyy-MM-dd}): ");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) payment.due_date = DateTime.Parse(input);

            Console.Write($"Nova Forma de Pagamento ({payment.payment_form}): ");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) payment.payment_form = input;

            Console.Write($"Nova Publicidade ({payment.advertising}): ");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) payment.advertising = input;

            Console.Write($"Nova Chave ({payment.key}): ");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) payment.key = input;

            Console.Write($"Novo Tipo ({payment.type}): ");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) payment.type = input;

            await payment.AlterarAsync();
            Console.WriteLine("Pagamento alterado com sucesso!");
        }
        else
        {
            Console.WriteLine("Pagamento não encontrado.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

async Task ExcluirPagamento()
{
    Console.WriteLine("\n-- Excluir Pagamento --");
    try
    {
        Console.Write("ID do Pagamento a ser excluído: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        Payment payment = new Payment();
        await payment.ExcluirAsync(id);

        Console.WriteLine("Pagamento excluído com sucesso (se existia)!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}