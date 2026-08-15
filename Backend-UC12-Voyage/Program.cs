using System;
using System.Threading.Tasks;

bool rodando = true;

while (rodando)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("==================================================");
    Console.WriteLine("          SISTEMA DE GESTÃO INTEGRADO             ");
    Console.WriteLine("==================================================");
    Console.ResetColor();
    Console.WriteLine("1. Gerenciar Usuários");
    Console.WriteLine("2. Gerenciar Empresas");
    Console.WriteLine("3. Gerenciar Endereços");
    Console.WriteLine("4. Gerenciar Pagamentos");
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine("0. Sair");
    Console.WriteLine("==================================================");
    Console.Write("Escolha uma opção: ");

    string? opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
            await MenuUsuarios();
            break;
        case "2":
            await MenuEmpresas();
            break;
        case "3":
            await MenuEnderecos();
            break;
        case "4":
            await MenuPagamentos();
            break;
        case "0":
            rodando = false;
            break;
        default:
            Console.WriteLine("Opção inválida!");
            await Pausa();
            break;
    }
}

async Task Pausa()
{
    Console.WriteLine("\nPressione qualquer tecla para continuar...");
    Console.ReadKey();
}

// ==========================================================
// MENUS DE SUB-SETORES
// ==========================================================

async Task MenuUsuarios()
{
    bool subRodando = true;
    while (subRodando)
    {
        Console.Clear();
        Console.WriteLine("--- GERENCIAR USUÁRIOS ---");
        Console.WriteLine("1. Inserir");
        Console.WriteLine("2. Buscar por ID");
        Console.WriteLine("3. Listar Todos");
        Console.WriteLine("4. Alterar");
        Console.WriteLine("5. Excluir");
        Console.WriteLine("0. Voltar");
        Console.Write("Opção: ");
        switch (Console.ReadLine())
        {
            case "1": await InserirUsuario(); await Pausa(); break;
            case "2": await BuscarUsuario(); await Pausa(); break;
            case "3": await ListarUsuarios(); await Pausa(); break;
            case "4": await AlterarUsuario(); await Pausa(); break;
            case "5": await ExcluirUsuario(); await Pausa(); break;
            case "0": subRodando = false; break;
            default: Console.WriteLine("Inválido!"); await Pausa(); break;
        }
    }
}

async Task MenuEmpresas()
{
    bool subRodando = true;
    while (subRodando)
    {
        Console.Clear();
        Console.WriteLine("--- GERENCIAR EMPRESAS ---");
        Console.WriteLine("1. Inserir");
        Console.WriteLine("2. Buscar por ID");
        Console.WriteLine("3. Listar Todas");
        Console.WriteLine("4. Alterar");
        Console.WriteLine("5. Excluir");
        Console.WriteLine("0. Voltar");
        Console.Write("Opção: ");
        switch (Console.ReadLine())
        {
            case "1": await InserirEmpresa(); await Pausa(); break;
            case "2": await BuscarEmpresa(); await Pausa(); break;
            case "3": await ListarEmpresas(); await Pausa(); break;
            case "4": await AlterarEmpresa(); await Pausa(); break;
            case "5": await ExcluirEmpresa(); await Pausa(); break;
            case "0": subRodando = false; break;
            default: Console.WriteLine("Inválido!"); await Pausa(); break;
        }
    }
}

async Task MenuEnderecos()
{
    bool subRodando = true;
    while (subRodando)
    {
        Console.Clear();
        Console.WriteLine("--- GERENCIAR ENDEREÇOS ---");
        Console.WriteLine("1. Inserir");
        Console.WriteLine("2. Buscar por ID");
        Console.WriteLine("3. Listar Todos");
        Console.WriteLine("4. Alterar");
        Console.WriteLine("5. Excluir");
        Console.WriteLine("0. Voltar");
        Console.Write("Opção: ");
        switch (Console.ReadLine())
        {
            case "1": await InserirEndereco(); await Pausa(); break;
            case "2": await BuscarEndereco(); await Pausa(); break;
            case "3": await ListarEnderecos(); await Pausa(); break;
            case "4": await AlterarEndereco(); await Pausa(); break;
            case "5": await ExcluirEndereco(); await Pausa(); break;
            case "0": subRodando = false; break;
            default: Console.WriteLine("Inválido!"); await Pausa(); break;
        }
    }
}

async Task MenuPagamentos()
{
    bool subRodando = true;
    while (subRodando)
    {
        Console.Clear();
        Console.WriteLine("--- GERENCIAR PAGAMENTOS ---");
        Console.WriteLine("1. Inserir");
        Console.WriteLine("2. Buscar por ID");
        Console.WriteLine("3. Listar Todos");
        Console.WriteLine("4. Alterar");
        Console.WriteLine("5. Excluir");
        Console.WriteLine("0. Voltar");
        Console.Write("Opção: ");
        switch (Console.ReadLine())
        {
            case "1": await InserirPagamento(); await Pausa(); break;
            case "2": await BuscarPagamento(); await Pausa(); break;
            case "3": await ListarPagamentos(); await Pausa(); break;
            case "4": await AlterarPagamento(); await Pausa(); break;
            case "5": await ExcluirPagamento(); await Pausa(); break;
            case "0": subRodando = false; break;
            default: Console.WriteLine("Inválido!"); await Pausa(); break;
        }
    }
}

// ==========================================================
// FUNÇÕES DE USUÁRIOS (User)
// ==========================================================

async Task InserirUsuario()
{
    Console.WriteLine("\n-- Inserir Usuário --");
    try
    {
        User u = new User();
        Console.Write("Nome: "); u.name = Console.ReadLine() ?? "";
        Console.Write("Tipo: "); u.type = Console.ReadLine() ?? "";
        Console.Write("Email: "); u.email = Console.ReadLine() ?? "";
        Console.Write("Senha: "); u.password = Console.ReadLine() ?? "";
        Console.Write("Telefone: "); u.phone = Console.ReadLine() ?? "";
        Console.Write("CPF: "); u.cpf = Console.ReadLine() ?? "";

        await u.InserirAsync();
        Console.WriteLine("Usuário inserido com sucesso!");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

async Task BuscarUsuario()
{
    try
    {
        Console.Write("ID do Usuário: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        User u = new User();
        await u.BuscarAsync(id);
        if (!string.IsNullOrEmpty(u.name))
            Console.WriteLine($"[{u.id}] {u.name} | Tipo: {u.type} | Email: {u.email}");
        else
            Console.WriteLine("Usuário não encontrado.");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

async Task ListarUsuarios()
{
    try
    {
        var u = new User();
        var lista = await u.BuscarTodosAsync();
        foreach (var item in lista)
            Console.WriteLine($"[{item.id}] {item.name} | Tipo: {item.type} | Email: {item.email}");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

async Task AlterarUsuario()
{
    try
    {
        Console.Write("ID do Usuário a alterar: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        User u = new User();
        await u.BuscarAsync(id);
        if (!string.IsNullOrEmpty(u.name))
        {
            Console.Write($"Nome ({u.name}): "); string? input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) u.name = input;
            Console.Write($"Tipo ({u.type}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) u.type = input;
            Console.Write($"Email ({u.email}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) u.email = input;
            Console.Write($"Senha: "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) u.password = input;
            Console.Write($"Telefone ({u.phone}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) u.phone = input;
            Console.Write($"CPF ({u.cpf}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) u.cpf = input;

            await u.AlterarAsync();
            Console.WriteLine("Usuário alterado com sucesso!");
        }
        else Console.WriteLine("Usuário não encontrado.");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

async Task ExcluirUsuario()
{
    try
    {
        Console.Write("ID do Usuário a excluir: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        User u = new User { id = id };
        await u.ExcluirAsync(u);
        Console.WriteLine("Excluído com sucesso (se existia)!");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

// ==========================================================
// FUNÇÕES DE EMPRESAS (Company)
// ==========================================================

async Task InserirEmpresa()
{
    Console.WriteLine("\n-- Inserir Empresa --");
    try
    {
        Company c = new Company();
        Console.Write("Nome: "); c.name = Console.ReadLine() ?? "";
        Console.Write("Categoria: "); c.category = Console.ReadLine() ?? "";
        Console.Write("CNPJ: "); c.cnpj = Console.ReadLine() ?? "";
        Console.Write("Avaliação: "); c.evaluate = double.Parse(Console.ReadLine() ?? "0");
        Console.Write("Endereços (places): "); c.places = Console.ReadLine() ?? "";
        
        c.user = new User();
        Console.Write("User ID Vinculado: "); c.user.id = int.Parse(Console.ReadLine() ?? "0");

        await c.InserirAsync();
        Console.WriteLine("Empresa inserida com sucesso!");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

async Task BuscarEmpresa()
{
    try
    {
        Console.Write("ID da Empresa: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        Company c = new Company();
        await c.BuscarAsync(id);
        if (!string.IsNullOrEmpty(c.name))
            c.Mostrar();
        else
            Console.WriteLine("Empresa não encontrada.");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

async Task ListarEmpresas()
{
    try
    {
        var c = new Company();
        var lista = await c.BuscarTodosAsync();
        c.Mostrar(lista);
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

async Task AlterarEmpresa()
{
    try
    {
        Console.Write("ID da Empresa a alterar: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        Company c = new Company();
        await c.BuscarAsync(id);
        if (!string.IsNullOrEmpty(c.name))
        {
            Console.Write($"Nome ({c.name}): "); string? input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) c.name = input;
            Console.Write($"Categoria ({c.category}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) c.category = input;
            Console.Write($"CNPJ ({c.cnpj}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) c.cnpj = input;
            Console.Write($"Avaliação ({c.evaluate}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) c.evaluate = double.Parse(input);
            Console.Write($"Endereços ({c.places}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) c.places = input;
            
            if(c.user == null) c.user = new User();
            Console.Write($"User ID ({c.user.id}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) c.user.id = int.Parse(input);

            await c.AtualizarAsync();
            Console.WriteLine("Empresa alterada com sucesso!");
        }
        else Console.WriteLine("Empresa não encontrada.");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

async Task ExcluirEmpresa()
{
    try
    {
        Console.Write("ID da Empresa a excluir: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        Company c = new Company { id = id };
        await c.DeletarAsync(c);
        Console.WriteLine("Excluída com sucesso (se existia)!");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

// ==========================================================
// FUNÇÕES DE ENDEREÇOS (Address)
// ==========================================================

async Task InserirEndereco()
{
    Console.WriteLine("\n-- Inserir Endereço --");
    try
    {
        Address a = new Address();
        Console.Write("Lugar/Rua (place): "); a.place = Console.ReadLine() ?? "";
        Console.Write("Número: "); a.number = Console.ReadLine() ?? "";
        Console.Write("CEP: "); a.zipcode = Console.ReadLine() ?? "";
        Console.Write("Latitude: "); a.lat = double.Parse(Console.ReadLine() ?? "0");
        Console.Write("Longitude: "); a.@long = double.Parse(Console.ReadLine() ?? "0");
        Console.Write("URL: "); a.url = Console.ReadLine() ?? "";

        Console.Write("User ID Vinculado (0 se nenhum): "); 
        int userId = int.Parse(Console.ReadLine() ?? "0");
        if (userId > 0) a.user = new User { id = userId };

        Console.Write("Company ID Vinculada (0 se nenhuma): ");
        int companyId = int.Parse(Console.ReadLine() ?? "0");
        if (companyId > 0) a.company = new Company { id = companyId };

        await a.InserirAsync();
        Console.WriteLine($"Endereço inserido com sucesso! ID: {a.id}");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

async Task BuscarEndereco()
{
    try
    {
        Console.Write("ID do Endereço: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        Address a = new Address();
        await a.BuscarAsync(id);
        if (!string.IsNullOrEmpty(a.place))
            a.Mostrar();
        else
            Console.WriteLine("Endereço não encontrado.");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

async Task ListarEnderecos()
{
    try
    {
        var a = new Address();
        var lista = await a.BuscarTodosAsync();
        a.Mostrar(lista);
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

async Task AlterarEndereco()
{
    try
    {
        Console.Write("ID do Endereço a alterar: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        Address a = new Address();
        await a.BuscarAsync(id);
        if (!string.IsNullOrEmpty(a.place))
        {
            Console.Write($"Lugar ({a.place}): "); string? input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) a.place = input;
            Console.Write($"Número ({a.number}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) a.number = input;
            Console.Write($"CEP ({a.zipcode}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) a.zipcode = input;
            Console.Write($"Latitude ({a.lat}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) a.lat = double.Parse(input);
            Console.Write($"Longitude ({a.@long}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) a.@long = double.Parse(input);
            Console.Write($"URL ({a.url}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) a.url = input;

            await a.EditarAsync();
            Console.WriteLine("Endereço alterado com sucesso!");
        }
        else Console.WriteLine("Endereço não encontrado.");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

async Task ExcluirEndereco()
{
    try
    {
        Console.Write("ID do Endereço a excluir: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        Address a = new Address { id = id };
        await Address.DeletarAsync(a);
        Console.WriteLine("Excluído com sucesso (se existia)!");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

// ==========================================================
// FUNÇÕES DE PAGAMENTOS (Payment)
// ==========================================================

async Task InserirPagamento()
{
    Console.WriteLine("\n-- Inserir Pagamento --");
    try
    {
        Payment p = new Payment();
        p.company = new Company();
        Console.Write("Company ID: "); p.company.id = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Data de Início (YYYY-MM-DD): "); p.to_date = DateTime.Parse(Console.ReadLine() ?? "");
        Console.Write("Data de Vencimento (YYYY-MM-DD): "); p.due_date = DateTime.Parse(Console.ReadLine() ?? "");
        Console.Write("Forma de Pagamento: "); p.payment_form = Console.ReadLine() ?? "";
        Console.Write("Publicidade: "); p.advertising = Console.ReadLine() ?? "";
        Console.Write("Chave: "); p.key = Console.ReadLine() ?? "";
        Console.Write("Tipo: "); p.type = Console.ReadLine() ?? "";

        await p.InserirAsync();
        Console.WriteLine("Pagamento inserido com sucesso!");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

async Task BuscarPagamento()
{
    try
    {
        Console.Write("ID do Pagamento: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        Payment p = new Payment();
        await p.BuscarAsync(id);

        if (p.company != null && p.company.id != 0)
            Console.WriteLine($"ID: {p.id}, Company ID: {p.company.id}, Forma de Pagamento: {p.payment_form}");
        else
            Console.WriteLine("Pagamento não encontrado.");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

async Task ListarPagamentos()
{
    try
    {
        Payment pModel = new Payment();
        var lista = await pModel.BuscarTodosAsync();
        foreach (var p in lista)
            Console.WriteLine($"ID: {p.id} | Company ID: {p.company?.id} | Forma de Pgto: {p.payment_form}");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

async Task AlterarPagamento()
{
    try
    {
        Console.Write("ID do Pagamento a alterar: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        Payment p = new Payment();
        await p.BuscarAsync(id);

        if (p.company != null && p.company.id != 0)
        {
            Console.Write($"Novo Company ID ({p.company.id}): "); string? input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) p.company.id = int.Parse(input);
            Console.Write($"Nova Data Início ({p.to_date:yyyy-MM-dd}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) p.to_date = DateTime.Parse(input);
            Console.Write($"Nova Data Vencimento ({p.due_date:yyyy-MM-dd}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) p.due_date = DateTime.Parse(input);
            Console.Write($"Nova Forma de Pagamento ({p.payment_form}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) p.payment_form = input;
            Console.Write($"Nova Publicidade ({p.advertising}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) p.advertising = input;
            Console.Write($"Nova Chave ({p.key}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) p.key = input;
            Console.Write($"Novo Tipo ({p.type}): "); input = Console.ReadLine(); if (!string.IsNullOrEmpty(input)) p.type = input;

            await p.AlterarAsync();
            Console.WriteLine("Pagamento alterado com sucesso!");
        }
        else Console.WriteLine("Pagamento não encontrado.");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}

async Task ExcluirPagamento()
{
    try
    {
        Console.Write("ID do Pagamento a excluir: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        Payment p = new Payment { id = id };
        await p.ExcluirAsync(p);
        Console.WriteLine("Excluído com sucesso (se existia)!");
    }
    catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
}