AdressMatheus();

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

async void AdressMatheus()
{
    Address addressService = new Address();
    bool executing = true;

    while (executing)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==================================================");
        Console.WriteLine("          VOYAGE - TESTES DE ENDEREÇOS            ");
        Console.WriteLine("==================================================");
        Console.ResetColor();
        Console.WriteLine("1. Listar todos os endereços");
        Console.WriteLine("2. Listar um único endereço");
        Console.WriteLine("3. Buscar por Lat + Long (Raio de busca)");
        Console.WriteLine("4. Filtrar por Companhia");
        Console.WriteLine("5. Listar por Categoria");
        Console.WriteLine("6. Listar por Favoritos (Usuário)");
        Console.WriteLine("7. Criar novo endereço");
        Console.WriteLine("8. Editar um endereço");
        Console.WriteLine("9. Excluir um endereço");
        Console.WriteLine("0. Sair");
        Console.WriteLine("==================================================");
        Console.Write("Escolha uma opção: ");

        string opcao = Console.ReadLine();
        Console.WriteLine();

        try
        {
            switch (opcao)
            {
                case "1":
                    await ListarTodosEnderecos();
                    break;
                case "2":
                    await ListarUnicoEndereco();
                    break;
                case "3":
                    await BuscarPorRaio();
                    break;
                case "4":
                    await FiltrarPorCompanhia();
                    break;
                case "5":
                    await ListarPorCategoria();
                    break;
                case "6":
                    await ListarPorFavoritos();
                    break;
                case "7":
                    await CriarEndereco();
                    break;
                case "8":
                    await EditarEndereco();
                    break;
                case "9":
                    await ExcluirEndereco();
                    break;
                case "0":
                    executing = false;
                    Console.WriteLine("Saindo...");
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Opção inválida!");
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

        if (executing)
        {
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }

    async Task ListarTodosEnderecos()
    {
        Console.WriteLine("--- LISTANDO TODOS OS ENDEREÇOS ---");
        var addresses = await addressService.BuscarTodosAsync();
        if (addresses.Count == 0)
        {
            Console.WriteLine("Nenhum endereço cadastrado.");
        }
        else
        {
            addressService.Mostrar(addresses);
        }
    }

    async Task ListarUnicoEndereco()
    {
        Console.Write("Digite o ID do endereço: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Address address = new Address();
            await address.BuscarAsync(id);
            if (address.id != 0)
            {
                address.Mostrar();
            }
            else
            {
                Console.WriteLine($"Endereço com ID {id} não encontrado.");
            }
        }
        else
        {
            Console.WriteLine("ID inválido.");
        }
    }

    async Task BuscarPorRaio()
    {
        Console.Write("Digite a Latitude (ex: -23.5505): ");
        if (!double.TryParse(Console.ReadLine(), out double lat)) return;

        Console.Write("Digite a Longitude (ex: -46.6333): ");
        if (!double.TryParse(Console.ReadLine(), out double lon)) return;

        Console.Write("Deseja usar raio customizado? (s/N): ");
        string custom = Console.ReadLine();
        double radius = 5.0; // Padrão 5km

        if (custom.ToLower() == "s")
        {
            Console.Write("Digite o raio (em km): ");
            double.TryParse(Console.ReadLine(), out radius);
        }

        Console.WriteLine($"\nBuscando endereços num raio de {radius}km de ({lat}, {lon})...");
        var results = await addressService.BuscarPorRaioAsync(lat, lon, radius);
        if (results.Count == 0)
        {
            Console.WriteLine("Nenhum endereço encontrado nesse raio.");
        }
        else
        {
            addressService.Mostrar(results);
        }
    }

    async Task FiltrarPorCompanhia()
    {
        Console.Write("Digite o ID da Companhia: ");
        if (int.TryParse(Console.ReadLine(), out int companyId))
        {
            var results = await addressService.BuscarPorCompanhiaAsync(companyId);
            if (results.Count == 0)
            {
                Console.WriteLine("Nenhum endereço associado a esta companhia.");
            }
            else
            {
                addressService.Mostrar(results);
            }
        }
    }

    async Task ListarPorCategoria()
    {
        Console.Write("Digite a Categoria (ex: Alimentação, Beleza): ");
        string category = Console.ReadLine();

        var results = await addressService.BuscarPorCategoriaAsync(category);
        if (results.Count == 0)
        {
            Console.WriteLine("Nenhum endereço encontrado para esta categoria.");
        }
        else
        {
            addressService.Mostrar(results);
        }
    }

    async Task ListarPorFavoritos()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Opções de favoritos:");
        Console.WriteLine("1. Endereços de Companhias Favoritadas pelo usuário (tabela favorites)");
        Console.WriteLine("2. Endereços Próprios do Usuário (tabela _addressUser)");
        Console.ResetColor();
        Console.Write("Escolha uma opção: ");
        string opt = Console.ReadLine();

        Console.Write("Digite o ID do Usuário: ");
        if (int.TryParse(Console.ReadLine(), out int userId))
        {
            List<Address> results;
            if (opt == "2")
            {
                results = await addressService.BuscarPorUsuarioAsync(userId);
            }
            else
            {
                results = await addressService.BuscarPorFavoritosAsync(userId);
            }

            if (results.Count == 0)
            {
                Console.WriteLine("Nenhum endereço encontrado.");
            }
            else
            {
                addressService.Mostrar(results);
            }
        }
    }

    async Task CriarEndereco()
    {
        Console.WriteLine("--- CRIAR ENDEREÇO ---");
        Console.Write("Endereço (Nome da rua): ");
        string place = Console.ReadLine();

        Console.Write("Número: ");
        string number = Console.ReadLine();

        Console.Write("Adicionar URL da Imagem: ");
        string url = Console.ReadLine();

        Console.Write("CEP (Zipcode): ");
        string zipcode = Console.ReadLine();

        var (lat, lon) = await Address.ObterCoordenadasIncrementadasAsync();
        Console.WriteLine($"\n[Coordenadas Geradas Automaticamente (Auto-Incrementadas)]");
        Console.WriteLine($"Latitude: {lat:F6}");
        Console.WriteLine($"Longitude: {lon:F6}");

        Address newAddress = new Address(place, number, zipcode, lat, lon, url);
        int addressId = await newAddress.InserirAsync();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Endereço criado com sucesso! ID: {addressId}");
        Console.ResetColor();

        Console.Write("Deseja colocar este endereço nos favoritos de um usuário? (s/N): ");
        string fav = Console.ReadLine();
        if (fav.ToLower() == "s")
        {
            Console.Write("Digite o ID do Usuário: ");
            if (int.TryParse(Console.ReadLine(), out int userId))
            {
                await Address.AdicionarAoUsuarioAsync(addressId, userId);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Endereço associado ao usuário {userId} com sucesso!");
                Console.ResetColor();
            }
        }
    }

    async Task EditarEndereco()
    {
        Console.WriteLine("--- EDITAR ENDEREÇO ---");
        Console.Write("Digite o ID do endereço que deseja editar: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Address address = new Address();
            await address.BuscarAsync(id);
            if (address.id == 0)
            {
                Console.WriteLine("Endereço não encontrado.");
                return;
            }

            Console.WriteLine("Valores atuais:");
            address.Mostrar();
            Console.WriteLine("\nDeixe em branco para manter o valor atual.");

            Console.Write($"Endereço (Rua) [{address.place}]: ");
            string place = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(place)) address.place = place;

            Console.Write($"Número [{address.number}]: ");
            string number = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(number)) address.number = number;

            Console.Write($"URL da Imagem [{address.url}]: ");
            string url = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(url)) address.url = url;

            Console.Write($"CEP [{address.zipcode}]: ");
            string zipcode = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(zipcode)) address.zipcode = zipcode;

            Console.Write($"Latitude [{address.lat}]: ");
            string latInput = Console.ReadLine();
            if (double.TryParse(latInput, out double lat)) address.lat = lat;

            Console.Write($"Longitude [{address.@long}]: ");
            string lonInput = Console.ReadLine();
            if (double.TryParse(lonInput, out double lon)) address.@long = lon;

            await address.EditarAsync();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Endereço atualizado com sucesso!");
            Console.ResetColor();
        }
    }

    async Task ExcluirEndereco()
    {
        Console.WriteLine("--- EXCLUIR ENDEREÇO ---");
        Console.Write("Digite o ID do endereço que deseja excluir: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Address address = new Address();
            await address.BuscarAsync(id);
            if (address.id == 0)
            {
                Console.WriteLine("Endereço não encontrado.");
                return;
            }

            Console.WriteLine("Valores do endereço a ser excluído:");
            address.Mostrar();
            Console.Write("Tem certeza que deseja excluir este endereço? (s/N): ");
            string confirm = Console.ReadLine();
            if (confirm.ToLower() == "s")
            {
                await Address.DeletarAsync(id);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Endereço excluído com sucesso!");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Exclusão cancelada.");
            }
        }
    }
}
