UsuarioJhonas();
async void UsuarioJhonas()
{

    bool rodando = true;

    while (rodando)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==================================================");
        Console.WriteLine("           SISTEMA DE GESTÃO DE USUÁRIOS          ");
        Console.WriteLine("==================================================");
        Console.ResetColor();
        Console.WriteLine("1. Inserir Usuário");
        Console.WriteLine("2. Buscar Usuário por ID");
        Console.WriteLine("3. Listar Todos os Usuários");
        Console.WriteLine("4. Alterar Usuário");
        Console.WriteLine("5. Excluir Usuário");
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("0. Sair");
        Console.WriteLine("==================================================");
        Console.Write("Escolha uma opção: ");

        string opcao = Console.ReadLine();

        Console.Clear();

        try
        {
            switch (opcao)
            {
                case "1":
                    Console.WriteLine("=== INSERIR USUÁRIO ===");
                    Console.Write("Nome: ");
                    string uNome = Console.ReadLine();
                    Console.Write("Tipo (admin/cliente): ");
                    string uTipo = Console.ReadLine();
                    Console.Write("Email: ");
                    string uEmail = Console.ReadLine();
                    Console.Write("Senha: ");
                    string uSenha = Console.ReadLine();
                    Console.Write("Telefone: ");
                    string uTelefone = Console.ReadLine();
                    Console.Write("CPF: ");
                    string uCpf = Console.ReadLine();

                    User novoUser = new User(uNome, uTipo, uEmail, uSenha, uTelefone, uCpf);
                    await novoUser.InserirAsync();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Usuário inserido com sucesso!");
                    Console.ResetColor();
                    break;

                case "2":
                    Console.WriteLine("=== BUSCAR USUÁRIO POR ID ===");
                    Console.Write("ID do Usuário: ");
                    int buscarId = int.Parse(Console.ReadLine());

                    User userEncontrado = new User();
                    await userEncontrado.BuscarAsync(buscarId);

                    if (userEncontrado.name != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"ID:       {userEncontrado.id}");
                        Console.WriteLine($"Nome:     {userEncontrado.name}");
                        Console.WriteLine($"Tipo:     {userEncontrado.type}");
                        Console.WriteLine($"Email:    {userEncontrado.email}");
                        Console.WriteLine($"Telefone: {userEncontrado.phone}");
                        Console.WriteLine($"CPF:      {userEncontrado.cpf}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Usuário não encontrado.");
                        Console.ResetColor();
                    }
                    break;

                case "3":
                    Console.WriteLine("=== LISTA DE USUÁRIOS ===");
                    User listaUser = new User();
                    List<User> users = await listaUser.BuscarTodosAsync();

                    if (users.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Nenhum usuário cadastrado.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"{"ID",-5} {"Nome",-25} {"Tipo",-10} {"Email",-30} {"Telefone",-15} {"CPF",-14}");
                        Console.WriteLine(new string('-', 101));
                        Console.ResetColor();
                        foreach (var u in users)
                            Console.WriteLine($"{u.id,-5} {u.name,-25} {u.type,-10} {u.email,-30} {u.phone,-15} {u.cpf,-14}");
                    }
                    break;

                case "4":
                    Console.WriteLine("=== ALTERAR USUÁRIO ===");
                    Console.Write("ID do Usuário a alterar: ");
                    int alterarId = int.Parse(Console.ReadLine());

                    User userAlterar = new User();
                    await userAlterar.BuscarAsync(alterarId);

                    if (userAlterar.name == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Usuário não encontrado.");
                        Console.ResetColor();
                        break;
                    }

                    Console.Write($"Nome atual [{userAlterar.name}] - Novo (Enter para manter): ");
                    string novoNome = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novoNome)) userAlterar.name = novoNome;

                    Console.Write($"Tipo atual [{userAlterar.type}] - Novo (Enter para manter): ");
                    string novoTipo = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novoTipo)) userAlterar.type = novoTipo;

                    Console.Write($"Email atual [{userAlterar.email}] - Novo (Enter para manter): ");
                    string novoEmail = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novoEmail)) userAlterar.email = novoEmail;

                    Console.Write("Nova senha (Enter para manter): ");
                    string novaSenha = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novaSenha)) userAlterar.password = novaSenha;

                    Console.Write($"Telefone atual [{userAlterar.phone}] - Novo (Enter para manter): ");
                    string novoTelefone = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novoTelefone)) userAlterar.phone = novoTelefone;

                    Console.Write($"CPF atual [{userAlterar.cpf}] - Novo (Enter para manter): ");
                    string novoCpf = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novoCpf)) userAlterar.cpf = novoCpf;

                    await userAlterar.AlterarAsync();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Usuário alterado com sucesso!");
                    Console.ResetColor();
                    break;

                case "5":
                    Console.WriteLine("=== EXCLUIR USUÁRIO ===");
                    Console.Write("ID do Usuário a excluir: ");
                    int excluirId = int.Parse(Console.ReadLine());

                    Console.Write("Confirma exclusão? (s/n): ");
                    string confirma = Console.ReadLine();
                    if (confirma?.ToLower() == "s")
                    {
                        User userExcluir = new User();
                        await userExcluir.ExcluirAsync(excluirId);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Usuário excluído com sucesso!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Operação cancelada.");
                        Console.ResetColor();
                    }
                    break;

                case "0":
                    rodando = false;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Encerrando o sistema. Até logo!");
                    Console.ResetColor();
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    Console.ResetColor();
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Erro: {ex.Message}");
            Console.ResetColor();
        }

        if (rodando)
        {
            Console.WriteLine();
            Console.Write("Pressione Enter para continuar...");
            Console.ReadLine();
        }
    }
}



async void EmpresaKevin()
{

    bool rodando = true;
    Company companyDao = new Company();

    while (rodando)
    {
        Console.WriteLine("\n=================================");
        Console.WriteLine("    GERENCIADOR DE EMPRESAS     ");
        Console.WriteLine("=================================");
        Console.WriteLine("1 - Cadastrar Empresa");
        Console.WriteLine("2 - Listar Todas as Empresas");
        Console.WriteLine("3 - Buscar Empresa por ID");
        Console.WriteLine("4 - Atualizar Empresa");
        Console.WriteLine("5 - Excluir Empresa");
        Console.WriteLine("0 - Sair");
        Console.WriteLine("=================================");
        Console.Write("Escolha uma opção: ");

        string? opcao = Console.ReadLine();

        switch (opcao)
        {
            case "1":
                await CadastrarEmpresa();
                break;
            case "2":
                await ListarEmpresas();
                break;
            case "3":
                await BuscarEmpresa();
                break;
            case "4":
                await AtualizarEmpresa();
                break;
            case "5":
                await ExcluirEmpresa();
                break;
            case "0":
                rodando = false;
                Console.WriteLine("Encerrando o programa...");
                break;
            default:
                Console.WriteLine("Opção inválida! Tente novamente.");
                break;
        }
    }

    async Task CadastrarEmpresa()
    {
        Console.WriteLine("\n--- CADASTRAR EMPRESA ---");
        Console.Write("Nome: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Categoria (ex: Lanchonete, Restaurante, Pizzaria, Mercado, Serviços, Bar, Outros): ");
        string category = Console.ReadLine() ?? "";

        Console.Write("CNPJ: ");
        string cnpj = Console.ReadLine() ?? "";

        Console.Write("Endereço / Descrição do local (places): ");
        string places = Console.ReadLine() ?? "";

        Console.Write("ID do Usuário Dono (user_id): ");
        int.TryParse(Console.ReadLine(), out int userId);

        Console.Write("Avaliação Inicial (0 a 5): ");
        double.TryParse(Console.ReadLine(), out double evaluate);

        Company novaEmpresa = new Company(name, category, cnpj, places, userId, evaluate);

        try
        {
            await novaEmpresa.InserirAsync();
            Console.WriteLine("Empresa cadastrada com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao cadastrar empresa: {ex.Message}");
        }
    }

    async Task ListarEmpresas()
    {
        Console.WriteLine("\n--- TODAS AS EMPRESAS ---");
        try
        {
            List<Company> empresas = await companyDao.BuscarTodosAsync();
            if (empresas.Count == 0)
            {
                Console.WriteLine("Nenhuma empresa cadastrada no momento.");
            }
            else
            {
                companyDao.Mostrar(empresas);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar empresas: {ex.Message}");
        }
    }

    async Task BuscarEmpresa()
    {
        Console.WriteLine("\n--- BUSCAR EMPRESA POR ID ---");
        Console.Write("Informe o ID da Empresa: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                Company empresa = new Company();
                await empresa.BuscarAsync(id);
                if (empresa.id > 0)
                {
                    empresa.Mostrar();
                }
                else
                {
                    Console.WriteLine("Empresa não encontrada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar empresa: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("ID inválido!");
        }
    }

    async Task AtualizarEmpresa()
    {
        Console.WriteLine("\n--- ATUALIZAR EMPRESA ---");
        Console.Write("Informe o ID da empresa que deseja atualizar: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                Company empresa = new Company();
                await empresa.BuscarAsync(id);
                if (empresa.id == 0)
                {
                    Console.WriteLine("Empresa não encontrada.");
                    return;
                }

                Console.WriteLine("Dados atuais da empresa:");
                empresa.Mostrar();

                Console.Write($"Novo Nome (deixe em branco para manter '{empresa.name}'): ");
                string? input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) empresa.name = input;

                Console.Write($"Nova Categoria (deixe em branco para manter '{empresa.category}'): ");
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) empresa.category = input;

                Console.Write($"Novo CNPJ (deixe em branco para manter '{empresa.cnpj}'): ");
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) empresa.cnpj = input;

                Console.Write($"Novo Endereço/Places (deixe em branco para manter '{empresa.places}'): ");
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) empresa.places = input;

                Console.Write($"Novo ID de Usuário (deixe em branco para manter '{empresa.user_id}'): ");
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input) && int.TryParse(input, out int newUserId))
                    empresa.user_id = newUserId;

                Console.Write($"Nova Avaliação (deixe em branco para manter '{empresa.evaluate}'): ");
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input) && double.TryParse(input, out double newEvaluate))
                    empresa.evaluate = newEvaluate;

                await empresa.AtualizarAsync();
                Console.WriteLine("Empresa atualizada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar empresa: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("ID inválido!");
        }
    }

    async Task ExcluirEmpresa()
    {
        Console.WriteLine("\n--- EXCLUIR EMPRESA ---");
        Console.Write("Informe o ID da empresa a ser excluída: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                Company empresa = new Company();
                await empresa.BuscarAsync(id);
                if (empresa.id == 0)
                {
                    Console.WriteLine("Empresa não encontrada.");
                    return;
                }

                empresa.Mostrar();
                Console.Write("Tem certeza que deseja excluir esta empresa? (S/N): ");
                string? confirmacao = Console.ReadLine();
                if (confirmacao?.Trim().ToUpper() == "S")
                {
                    await companyDao.DeletarAsync(id);
                    Console.WriteLine("Empresa excluída com sucesso!");
                }
                else
                {
                    Console.WriteLine("Operação cancelada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir empresa: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("ID inválido!");
        }
    }

}
