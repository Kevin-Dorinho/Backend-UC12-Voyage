using MySqlConnector;
using System.Data;

public class Address
{
    public int id { get; set; }
    public string place { get; set; }
    public string number { get; set; }
    public string zipcode { get; set; }
    public double lat { get; set; }
    public double @long { get; set; }
    public string url { get; set; }
    public User user { get; set; }
    public Company company { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }

    public const string tabela = "address";

    public Address() { }

    public Address(
        int id,
        string place,
        string number,
        string zipcode,
        double lat,
        double @long,
        string url,
        DateTime createdAt,
        DateTime updatedAt)
    {
        this.id = id;
        this.place = place;
        this.number = number;
        this.zipcode = zipcode;
        this.lat = lat;
        this.@long = @long;
        this.url = url;
        this.createdAt = createdAt;
        this.updatedAt = updatedAt;
    }

    public Address(
        string place,
        string number,
        string zipcode,
        double lat,
        double @long,
        string url)
    {
        this.place = place;
        this.number = number;
        this.zipcode = zipcode;
        this.lat = lat;
        this.@long = @long;
        this.url = url;
    }

    public void Mostrar()
    {
        Console.WriteLine(
            $"[{id}] - {place}, {number} | CEP: {zipcode} | " +
            $"Lat: {lat:F6} | Long: {@long:F6} | URL: {url} | " +
            $"Criado em: {createdAt}"
        );
    }

    public void Mostrar(List<Address> addresses)
    {
        for (int i = 0; i < addresses.Count; i++)
        {
            addresses[i].Mostrar();
        }
    }

    public static async Task<bool> ExisteAsync(Address address)
    {
        string query = $"SELECT COUNT(1) FROM {tabela} WHERE id = @id;";
        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("id", address.id);
        await conexao.OpenAsync();
        var count = Convert.ToInt32(await comando.ExecuteScalarAsync());
        return count > 0;
    }

    public async Task ValidarAsync()
    {
        if (string.IsNullOrWhiteSpace(place))
            throw new ArgumentException("O logradouro/rua não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("O número não pode ser vazio.");

        if (!ValidadorCpfCnpj.ValidarCEP(zipcode))
            throw new ArgumentException("O CEP informado é inválido (deve conter 8 dígitos).");
    }

    public async Task<int> InserirAsync()
    {
        await ValidarAsync();

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        await conexao.OpenAsync();
        using var transacao = await conexao.BeginTransactionAsync();

        try
        {
            string query = $"""
                           INSERT INTO {tabela}
                           (place, number, zipcode, lat, `long`, url)
                           VALUES
                           (@place, @number, @zipcode, @lat, @long, @url);
                           SELECT LAST_INSERT_ID();
                           """;

            using var comando = new MySqlCommand(query, conexao, transacao);
            comando.Parameters.AddWithValue("place", place);
            comando.Parameters.AddWithValue("number", number);
            comando.Parameters.AddWithValue("zipcode", zipcode);
            comando.Parameters.AddWithValue("lat", lat);
            comando.Parameters.AddWithValue("long", @long);
            comando.Parameters.AddWithValue("url", url);

            var result = await comando.ExecuteScalarAsync();
            if (result != null)
            {
                this.id = Convert.ToInt32(result);
            }

            if (this.user != null && this.user.id > 0)
            {
                string qUser = "INSERT INTO _addressUser (A, B) VALUES (@addressId, @userId);";
                using var cmdU = new MySqlCommand(qUser, conexao, transacao);
                cmdU.Parameters.AddWithValue("addressId", this.id);
                cmdU.Parameters.AddWithValue("userId", this.user.id);
                await cmdU.ExecuteNonQueryAsync();
            }

            if (this.company != null && this.company.id > 0)
            {
                string qComp = "INSERT INTO address_company (address_id, company_id) VALUES (@addressId, @companyId);";
                using var cmdC = new MySqlCommand(qComp, conexao, transacao);
                cmdC.Parameters.AddWithValue("addressId", this.id);
                cmdC.Parameters.AddWithValue("companyId", this.company.id);
                await cmdC.ExecuteNonQueryAsync();
            }

            await transacao.CommitAsync();
            return this.id;
        }
        catch
        {
            await transacao.RollbackAsync();
            throw;
        }
    }

    public async Task EditarAsync()
    {
        await ValidarAsync();

        string query = $"""
                       UPDATE {tabela}
                       SET place = @place, number = @number, zipcode = @zipcode, lat = @lat, `long` = @long, url = @url, updatedAt = NOW()
                       WHERE id = @id;
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);

        comando.Parameters.AddWithValue("id", id);
        comando.Parameters.AddWithValue("place", place);
        comando.Parameters.AddWithValue("number", number);
        comando.Parameters.AddWithValue("zipcode", zipcode);
        comando.Parameters.AddWithValue("lat", lat);
        comando.Parameters.AddWithValue("long", @long);
        comando.Parameters.AddWithValue("url", url);

        await conexao.OpenAsync();
        await comando.ExecuteNonQueryAsync();
    }

    public async Task BuscarAsync(int id)
    {
        string query = $"""
                       SELECT *
                       FROM {tabela}
                       WHERE id = @id;
                       """;

        using var conexao = new MySqlConnection(
            ConfiguracaoBD.connectionString
        );

        using var comando = new MySqlCommand(query, conexao);

        comando.Parameters.AddWithValue("id", id);

        await conexao.OpenAsync();
        await using var dados = await comando.ExecuteReaderAsync();

        while (await dados.ReadAsync())
        {
            this.id = dados.GetInt32("id");
            this.place = dados.GetString("place");
            this.number = dados.GetString("number");
            this.zipcode = dados.GetString("zipcode");
            this.lat = dados.GetDouble("lat");
            this.@long = dados.GetDouble("long");
            this.url = dados.GetString("url");
            this.createdAt = dados.GetDateTime("created_at");
            this.updatedAt = dados.GetDateTime("updated_at");
        }
    }

    public async Task<List<Address>> BuscarTodosAsync()
    {
        string query = $"""
                       SELECT *
                       FROM {tabela};
                       """;

        using var conexao = new MySqlConnection(
            ConfiguracaoBD.connectionString
        );

        using var comando = new MySqlCommand(query, conexao);

        await conexao.OpenAsync();
        await using var dados = await comando.ExecuteReaderAsync();

        List<Address> addresses = new List<Address>();

        while (await dados.ReadAsync())
        {
            Address address = new Address();

            address.id = dados.GetInt32("id");
            address.place = dados.GetString("place");
            address.number = dados.GetString("number");
            address.zipcode = dados.GetString("zipcode");
            address.lat = dados.GetDouble("lat");
            address.@long = dados.GetDouble("long");
            address.url = dados.GetString("url");
            address.createdAt = dados.GetDateTime("created_at");
            address.updatedAt = dados.GetDateTime("updated_at");

            addresses.Add(address);
        }

        return addresses;
    }

    public async Task<List<Address>> BuscarPorRaioAsync(double targetLat, double targetLong, double radiusKm)
    {
        string query = $"""
                       SELECT *, (6371 * acos(cos(radians(@targetLat)) * cos(radians(lat)) * cos(radians(`long`) - radians(@targetLong)) + sin(radians(@targetLat)) * sin(radians(lat)))) AS distance 
                       FROM {tabela}
                       HAVING distance <= @radiusKm
                       ORDER BY distance;
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("targetLat", targetLat);
        comando.Parameters.AddWithValue("targetLong", targetLong);
        comando.Parameters.AddWithValue("radiusKm", radiusKm);

        await conexao.OpenAsync();
        await using var dados = await comando.ExecuteReaderAsync();
        List<Address> addresses = new List<Address>();
        while (await dados.ReadAsync())
        {
            Address address = new Address();
            address.id = dados.GetInt32("id");
            address.place = dados.GetString("place");
            address.number = dados.GetString("number");
            address.zipcode = dados.GetString("zipcode");
            address.lat = dados.GetDouble("lat");
            address.@long = dados.GetDouble("long");
            address.url = dados.GetString("url");
            address.createdAt = dados.GetDateTime("created_at");
            address.updatedAt = dados.GetDateTime("updated_at");
            addresses.Add(address);
        }
        return addresses;
    }

    public async Task<List<Address>> BuscarPorCompanhiaAsync(int companyId)
    {
        string query = $"""
                       SELECT a.*
                       FROM {tabela} a
                       INNER JOIN address_company ac ON a.id = ac.address_id
                       WHERE ac.company_id = @companyId;
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("companyId", companyId);

        await conexao.OpenAsync();
        await using var dados = await comando.ExecuteReaderAsync();
        List<Address> addresses = new List<Address>();
        while (await dados.ReadAsync())
        {
            Address address = new Address();
            address.id = dados.GetInt32("id");
            address.place = dados.GetString("place");
            address.number = dados.GetString("number");
            address.zipcode = dados.GetString("zipcode");
            address.lat = dados.GetDouble("lat");
            address.@long = dados.GetDouble("long");
            address.url = dados.GetString("url");
            address.createdAt = dados.GetDateTime("created_at");
            address.updatedAt = dados.GetDateTime("updated_at");
            addresses.Add(address);
        }
        return addresses;
    }

    public async Task<List<Address>> BuscarPorCategoriaAsync(string category)
    {
        string query = $"""
                       SELECT DISTINCT a.*
                       FROM {tabela} a
                       INNER JOIN address_company ac ON a.id = ac.address_id
                       INNER JOIN companies c ON ac.company_id = c.id
                       WHERE c.category = @category;
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("category", category);

        await conexao.OpenAsync();
        await using var dados = await comando.ExecuteReaderAsync();
        List<Address> addresses = new List<Address>();
        while (await dados.ReadAsync())
        {
            Address address = new Address();
            address.id = dados.GetInt32("id");
            address.place = dados.GetString("place");
            address.number = dados.GetString("number");
            address.zipcode = dados.GetString("zipcode");
            address.lat = dados.GetDouble("lat");
            address.@long = dados.GetDouble("long");
            address.url = dados.GetString("url");
            address.createdAt = dados.GetDateTime("created_at");
            address.updatedAt = dados.GetDateTime("updated_at");
            addresses.Add(address);
        }
        return addresses;
    }

    public async Task<List<Address>> BuscarPorFavoritosAsync(int userId)
    {
        string query = $"""
                       SELECT DISTINCT a.*
                       FROM {tabela} a
                       INNER JOIN address_company ac ON a.id = ac.address_id
                       INNER JOIN favorites f ON ac.company_id = f.company_id
                       WHERE f.user_id = @userId;
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("userId", userId);

        await conexao.OpenAsync();
        await using var dados = await comando.ExecuteReaderAsync();
        List<Address> addresses = new List<Address>();
        while (await dados.ReadAsync())
        {
            Address address = new Address();
            address.id = dados.GetInt32("id");
            address.place = dados.GetString("place");
            address.number = dados.GetString("number");
            address.zipcode = dados.GetString("zipcode");
            address.lat = dados.GetDouble("lat");
            address.@long = dados.GetDouble("long");
            address.url = dados.GetString("url");
            address.createdAt = dados.GetDateTime("created_at");
            address.updatedAt = dados.GetDateTime("updated_at");
            addresses.Add(address);
        }
        return addresses;
    }

    public async Task<List<Address>> BuscarPorUsuarioAsync(int userId)
    {
        string query = $"""
                       SELECT a.*
                       FROM {tabela} a
                       INNER JOIN _addressUser au ON a.id = au.A
                       WHERE au.B = @userId;
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("userId", userId);

        await conexao.OpenAsync();
        await using var dados = await comando.ExecuteReaderAsync();
        List<Address> addresses = new List<Address>();
        while (await dados.ReadAsync())
        {
            Address address = new Address();
            address.id = dados.GetInt32("id");
            address.place = dados.GetString("place");
            address.number = dados.GetString("number");
            address.zipcode = dados.GetString("zipcode");
            address.lat = dados.GetDouble("lat");
            address.@long = dados.GetDouble("long");
            address.url = dados.GetString("url");
            address.createdAt = dados.GetDateTime("created_at");
            address.updatedAt = dados.GetDateTime("updated_at");
            addresses.Add(address);
        }
        return addresses;
    }

    public static async Task AdicionarAoUsuarioAsync(Address address, User user)
    {
        string query = "INSERT INTO _addressUser (A, B) VALUES (@addressId, @userId);";
        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("addressId", address.id);
        comando.Parameters.AddWithValue("userId", user.id);
        await conexao.OpenAsync();
        await comando.ExecuteNonQueryAsync();
    }

    public static async Task<(double lat, double @long)> ObterCoordenadasIncrementadasAsync()
    {
        string query = "SELECT MAX(lat), MAX(`long`) FROM address;";
        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);
        await conexao.OpenAsync();
        using var reader = await comando.ExecuteReaderAsync();
        if (await reader.ReadAsync() && !reader.IsDBNull(0))
        {
            double maxLat = reader.GetDouble(0);
            double maxLong = reader.GetDouble(1);
            return (maxLat + 0.01, maxLong + 0.01);
        }
        return (-23.55052, -46.633308); // Default base coordinates (São Paulo)
    }

    public static async Task DeletarAsync(Address address)
    {
        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        await conexao.OpenAsync();
        using var transacao = await conexao.BeginTransactionAsync();
        try
        {
            string queryCleanCompany = "DELETE FROM address_company WHERE address_id = @id;";
            using (var cmd = new MySqlCommand(queryCleanCompany, conexao, transacao))
            {
                cmd.Parameters.AddWithValue("id", address.id);
                await cmd.ExecuteNonQueryAsync();
            }

            string queryCleanUser = "DELETE FROM _addressUser WHERE A = @id;";
            using (var cmd = new MySqlCommand(queryCleanUser, conexao, transacao))
            {
                cmd.Parameters.AddWithValue("id", address.id);
                await cmd.ExecuteNonQueryAsync();
            }

            string queryDeleteAddress = $"DELETE FROM {tabela} WHERE id = @id;";
            using (var cmd = new MySqlCommand(queryDeleteAddress, conexao, transacao))
            {
                cmd.Parameters.AddWithValue("id", address.id);
                await cmd.ExecuteNonQueryAsync();
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