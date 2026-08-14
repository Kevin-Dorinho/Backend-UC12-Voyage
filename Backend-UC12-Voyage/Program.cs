using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
