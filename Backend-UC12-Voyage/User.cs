using MySqlConnector;
using System.Security.Cryptography;
using System.Text;

public class User
{
    public int id { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string phone { get; set; }
    public string cpf { get; set; }

    public const string tabela = "users";

    public User() { }

    public User(int id, string name, string type, string email, string password, string phone, string cpf)
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.email = email;
        this.password = password;
        this.phone = phone;
        this.cpf = cpf;
    }

    public User(string name, string type, string email, string password, string phone, string cpf)
    {
        this.name = name;
        this.type = type;
        this.email = email;
        this.password = password;
        this.phone = phone;
        this.cpf = cpf;
    }

    public static string CriptografarSenha(string senha)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(senha));
        return Convert.ToHexString(bytes).ToLower();
    }

    public static async Task<bool> ExisteAsync(int id)
    {
        string query = $"SELECT COUNT(1) FROM {tabela} WHERE id = @id;";
        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("id", id);
        await conexao.OpenAsync();
        var count = Convert.ToInt32(await comando.ExecuteScalarAsync());
        return count > 0;
    }

    public async Task ValidarAsync()
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do usuário não pode ser vazio.");

        if (!ValidadorCpfCnpj.ValidarEmail(email))
            throw new ArgumentException("O e-mail informado é inválido.");

        if (!ValidadorCpfCnpj.ValidarCPF(cpf))
            throw new ArgumentException("O CPF informado é inválido (falha na validação do Módulo 11).");
    }

    public async Task InserirAsync()
    {
        await ValidarAsync();

        string query = $"""
                       INSERT INTO {tabela}
                       (name, type, email, password, phone, cpf)
                       VALUES
                       (@name, @type, @email, @password, @phone, @cpf);
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("name", name);
        comando.Parameters.AddWithValue("type", type);
        comando.Parameters.AddWithValue("email", email);
        comando.Parameters.AddWithValue("password", CriptografarSenha(password));
        comando.Parameters.AddWithValue("phone", phone);
        comando.Parameters.AddWithValue("cpf", cpf);

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
            this.name = dados.IsDBNull(dados.GetOrdinal("name")) ? "" : dados.GetString("name");
            this.type = dados.IsDBNull(dados.GetOrdinal("type")) ? "" : dados.GetString("type");
            this.email = dados.IsDBNull(dados.GetOrdinal("email")) ? "" : dados.GetString("email");
            this.password = dados.IsDBNull(dados.GetOrdinal("password")) ? "" : dados.GetString("password");
            this.phone = dados.IsDBNull(dados.GetOrdinal("phone")) ? "" : dados.GetString("phone");
            this.cpf = dados.IsDBNull(dados.GetOrdinal("cpf")) ? "" : dados.GetString("cpf");
        }
    }

    public async Task<List<User>> BuscarTodosAsync()
    {
        string query = $"""
                       SELECT * FROM {tabela};
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);

        await conexao.OpenAsync();
        await using var dados = await comando.ExecuteReaderAsync();

        List<User> lista_users = new List<User>();
        while (await dados.ReadAsync())
        {
            User user = new User();
            user.id = dados.GetInt32("id");
            user.name = dados.IsDBNull(dados.GetOrdinal("name")) ? "" : dados.GetString("name");
            user.type = dados.IsDBNull(dados.GetOrdinal("type")) ? "" : dados.GetString("type");
            user.email = dados.IsDBNull(dados.GetOrdinal("email")) ? "" : dados.GetString("email");
            user.password = dados.IsDBNull(dados.GetOrdinal("password")) ? "" : dados.GetString("password");
            user.phone = dados.IsDBNull(dados.GetOrdinal("phone")) ? "" : dados.GetString("phone");
            user.cpf = dados.IsDBNull(dados.GetOrdinal("cpf")) ? "" : dados.GetString("cpf");
            lista_users.Add(user);
        }

        return lista_users;
    }

    public async Task AlterarAsync()
    {
        await ValidarAsync();

        string query = $"""
                       UPDATE {tabela}
                       SET name = @name,
                           type = @type,
                           email = @email,
                           password = @password,
                           phone = @phone,
                           cpf = @cpf
                       WHERE id = @id;
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("name", name);
        comando.Parameters.AddWithValue("type", type);
        comando.Parameters.AddWithValue("email", email);
        comando.Parameters.AddWithValue("password", CriptografarSenha(password));
        comando.Parameters.AddWithValue("phone", phone);
        comando.Parameters.AddWithValue("cpf", cpf);
        comando.Parameters.AddWithValue("id", id);

        await conexao.OpenAsync();
        await comando.ExecuteNonQueryAsync();
    }

    public async Task ExcluirAsync(int id)
    {
        string query = $"""
                       DELETE FROM {tabela}
                       WHERE id = @id;
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("id", id);

        await conexao.OpenAsync();
        await comando.ExecuteNonQueryAsync();
    }
}
