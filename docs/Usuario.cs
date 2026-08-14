using MySqlConnector;

public class Usuario
{

    int id { get; set; }

    string nome { get; set; }

    int idade { get; set; }

    DateTime criado_em { get; set; }

    public const string tabela = "usuarios";


    public Usuario() { }

    public Usuario(int id, string nome, int idade, DateTime criado_em)

    {

        this.id = id;

        this.nome = nome;

        this.idade = idade;

        this.criado_em = criado_em;

    }


    public Usuario(string nome, int idade = 0)

    {

        this.nome = nome;

        this.idade = idade;

    }


    public void Mostrar()

    {

        Console.WriteLine($"[{id}] - {nome} | {idade} anos | Criado em: {criado_em.ToString()}");

    }

    public void Mostrar(List<Usuario> usuarios)
    {
        for (int i = 0; i < usuarios.Count; i++)
        {
            usuarios[i].Mostrar();
        }
    }


    public async Task InserirAsync()

    {

        string query = $"""

                       INSERT INTO {tabela}

                       (nome, idade)

                       VALUES

                       (@nome, @idade);

                       """;


        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);

        using var comando = new MySqlCommand(query, conexao);

        comando.Parameters.AddWithValue("nome", nome);

        comando.Parameters.AddWithValue("idade", idade);


        await conexao.OpenAsync();

        await comando.ExecuteNonQueryAsync();

    }


    public async Task BuscarAsync(int id)

    {
        string query = $"""
                       SELECT * FROM {tabela}
                       WHERE id = {id};

                       """;


        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);

        await conexao.OpenAsync();
        await using var dados = await comando.ExecuteReaderAsync();

        while (await dados.ReadAsync())
        {
            
            this.id = dados.GetInt32("id");
            this.nome = dados.GetString("nome");
            this.idade = dados.GetInt32("idade");
            this.criado_em = dados.GetDateTime("criado_em");
            
        }

      

    }


    public async Task<List<Usuario>> BuscarTodosAsync()

    {

        string query = $"""
                       SELECT * FROM {tabela};

                       """;


        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);

        await conexao.OpenAsync();
        await using var dados = await comando.ExecuteReaderAsync();

        List<Usuario> lista_usuarios = new List<Usuario>();

        while(await dados.ReadAsync())
        {
            Usuario usuario = new Usuario();
            usuario.id = dados.GetInt32("id");
            usuario.nome = dados.GetString("nome");
            usuario.idade = dados.GetInt32("idade");
            usuario.criado_em = dados.GetDateTime("criado_em");
            lista_usuarios.Add(usuario);
        }

        return lista_usuarios;

    }

}