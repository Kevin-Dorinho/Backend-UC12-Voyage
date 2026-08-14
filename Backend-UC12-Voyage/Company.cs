using MySqlConnector;

public class Company
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string category { get; set; } = string.Empty;
    public string cnpj { get; set; } = string.Empty;
    public double evaluate { get; set; }
    public string places { get; set; } = string.Empty;
    public int user_id { get; set; }
    public DateTime criado_em { get; set; }
    public DateTime atualizado_em { get; set; }

    public const string tabela = "companies";

    public Company() { }

    public Company(int id, string name, string category, string cnpj, double evaluate, string places, int user_id, DateTime criado_em, DateTime atualizado_em)
    {
        this.id = id;
        this.name = name;
        this.category = category;
        this.cnpj = cnpj;
        this.evaluate = evaluate;
        this.places = places;
        this.user_id = user_id;
        this.criado_em = criado_em;
        this.atualizado_em = atualizado_em;
    }

    public Company(string name, string category, string cnpj, string places, int user_id, double evaluate = 0)
    {
        this.name = name;
        this.category = category;
        this.cnpj = cnpj;
        this.places = places;
        this.user_id = user_id;
        this.evaluate = evaluate;
    }

    public void Mostrar()
    {
        Console.WriteLine($"[{id}] - {name} | Categoria: {category} | CNPJ: {cnpj} | Avaliação: {evaluate} | Endereço: {places} | ID Usuário: {user_id} | Criado em: {criado_em}");
    }

    public void Mostrar(List<Company> empresas)
    {
        for (int i = 0; i < empresas.Count; i++)
        {
            empresas[i].Mostrar();
        }
    }

    public async Task InserirAsync()
    {
        string query = $"""
                       INSERT INTO {tabela}
                       (name, category, cnpj, evaluate, places, user_id, created_at, updated_at)
                       VALUES
                       (@name, @category, @cnpj, @evaluate, @places, @user_id, NOW(), NOW());
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);

        comando.Parameters.AddWithValue("name", name);
        comando.Parameters.AddWithValue("category", category);
        comando.Parameters.AddWithValue("cnpj", cnpj);
        comando.Parameters.AddWithValue("evaluate", evaluate);
        comando.Parameters.AddWithValue("places", places);
        comando.Parameters.AddWithValue("user_id", user_id);

        await conexao.OpenAsync();
        await comando.ExecuteNonQueryAsync();
    }

    public async Task BuscarAsync(int id)
    {
        string query = $"""
                       SELECT * FROM {tabela}
                       WHERE id = @id;
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("id", id);

        await conexao.OpenAsync();
        await using var dados = await comando.ExecuteReaderAsync();

        while (await dados.ReadAsync())
        {
            this.id = dados.GetInt32("id");
            this.name = dados.GetString("name");
            this.category = dados.GetString("category");
            this.cnpj = dados.GetString("cnpj");
            this.evaluate = dados.GetDouble("evaluate");
            this.places = dados.GetString("places");
            this.user_id = dados.GetInt32("user_id");
            this.criado_em = dados.GetDateTime("created_at");
            this.atualizado_em = dados.GetDateTime("updated_at");
        }
    }

    public async Task<List<Company>> BuscarTodosAsync()
    {
        string query = $"""
                       SELECT * FROM {tabela};
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);

        await conexao.OpenAsync();
        await using var dados = await comando.ExecuteReaderAsync();

        List<Company> lista_empresas = new List<Company>();

        while (await dados.ReadAsync())
        {
            Company empresa = new Company
            {
                id = dados.GetInt32("id"),
                name = dados.GetString("name"),
                category = dados.GetString("category"),
                cnpj = dados.GetString("cnpj"),
                evaluate = dados.GetDouble("evaluate"),
                places = dados.GetString("places"),
                user_id = dados.GetInt32("user_id"),
                criado_em = dados.GetDateTime("created_at"),
                atualizado_em = dados.GetDateTime("updated_at")
            };
            lista_empresas.Add(empresa);
        }

        return lista_empresas;
    }

    public async Task AtualizarAsync()
    {
        string query = $"""
                       UPDATE {tabela}
                       SET name = @name,
                           category = @category,
                           cnpj = @cnpj,
                           evaluate = @evaluate,
                           places = @places,
                           user_id = @user_id,
                           updated_at = NOW()
                       WHERE id = @id;
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);

        comando.Parameters.AddWithValue("id", id);
        comando.Parameters.AddWithValue("name", name);
        comando.Parameters.AddWithValue("category", category);
        comando.Parameters.AddWithValue("cnpj", cnpj);
        comando.Parameters.AddWithValue("evaluate", evaluate);
        comando.Parameters.AddWithValue("places", places);
        comando.Parameters.AddWithValue("user_id", user_id);

        await conexao.OpenAsync();
        await comando.ExecuteNonQueryAsync();
    }

    public async Task DeletarAsync(int id)
    {
        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        await conexao.OpenAsync();
        using var transacao = await conexao.BeginTransactionAsync();

        try
        {
            // 1. Remover dependências em address_company (endereços vinculados)
            using (var cmd1 = new MySqlCommand("DELETE FROM address_company WHERE company_id = @id;", conexao, transacao))
            {
                cmd1.Parameters.AddWithValue("id", id);
                await cmd1.ExecuteNonQueryAsync();
            }

            // 2. Remover dependências em favorites (favoritos)
            using (var cmd2 = new MySqlCommand("DELETE FROM favorites WHERE company_id = @id;", conexao, transacao))
            {
                cmd2.Parameters.AddWithValue("id", id);
                await cmd2.ExecuteNonQueryAsync();
            }

            // 3. Remover dependências em payments (pagamentos/anúncios)
            using (var cmd3 = new MySqlCommand("DELETE FROM payments WHERE company_id = @id;", conexao, transacao))
            {
                cmd3.Parameters.AddWithValue("id", id);
                await cmd3.ExecuteNonQueryAsync();
            }

            // 4. Remover a empresa da tabela companies
            using (var cmd4 = new MySqlCommand($"DELETE FROM {tabela} WHERE id = @id;", conexao, transacao))
            {
                cmd4.Parameters.AddWithValue("id", id);
                await cmd4.ExecuteNonQueryAsync();
            }

            await transacao.CommitAsync();
        }
        catch
        {
            await transacao.RollbackAsync();
            throw;
        }
    }
}
