using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Payment
{
    public int id { get; set; }
    public int company_id { get; set; }
    public DateTime to_date { get; set; }
    public DateTime due_date { get; set; }
    public string payment_form { get; set; }
    public string advertising { get; set; }
    public string key { get; set; }
    public string type { get; set; }

    public const string tabela = "payments";

    public Payment() { }

    public Payment(int id, int company_id, DateTime to_date, DateTime due_date, string payment_form, string advertising, string key, string type)
    {
        this.id = id;
        this.company_id = company_id;
        this.to_date = to_date;
        this.due_date = due_date;
        this.payment_form = payment_form;
        this.advertising = advertising;
        this.key = key;
        this.type = type;
    }

    public Payment(int company_id, DateTime to_date, DateTime due_date, string payment_form, string advertising, string key, string type)
    {
        this.company_id = company_id;
        this.to_date = to_date;
        this.due_date = due_date;
        this.payment_form = payment_form;
        this.advertising = advertising;
        this.key = key;
        this.type = type;
    }

    public async Task InserirAsync()
    {
        string query = $"""
                       INSERT INTO {tabela}
                       (company_id, to_date, due_date, payment_form, advertising, `key`, `type`)
                       VALUES
                       (@company_id, @to_date, @due_date, @payment_form, @advertising, @key, @type);
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("company_id", company_id);
        comando.Parameters.AddWithValue("to_date", to_date);
        comando.Parameters.AddWithValue("due_date", due_date);
        comando.Parameters.AddWithValue("payment_form", payment_form);
        comando.Parameters.AddWithValue("advertising", advertising);
        comando.Parameters.AddWithValue("key", key);
        comando.Parameters.AddWithValue("type", type);

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
            this.company_id = dados.GetInt32("company_id");
            this.to_date = dados.GetDateTime("to_date");
            this.due_date = dados.GetDateTime("due_date");
            this.payment_form = dados.GetString("payment_form");
            this.advertising = dados.GetString("advertising");
            this.key = dados.GetString("key");
            this.type = dados.GetString("type");
        }
    }

    public async Task<List<Payment>> BuscarTodosAsync()
    {
        string query = $"""
                       SELECT * FROM {tabela};
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);

        await conexao.OpenAsync();
        await using var dados = await comando.ExecuteReaderAsync();

        List<Payment> Lista_payments = new List<Payment>();
        while (await dados.ReadAsync())
        {
            Payment payment = new Payment();
            payment.id = dados.GetInt32("id");
            payment.company_id = dados.GetInt32("company_id");
            payment.to_date = dados.GetDateTime("to_date");
            payment.due_date = dados.GetDateTime("due_date");
            payment.payment_form = dados.GetString("payment_form");
            payment.advertising = dados.GetString("advertising");
            payment.key = dados.GetString("key");
            payment.type = dados.GetString("type");
            Lista_payments.Add(payment);
        }

        return Lista_payments;
    }

    public async Task AlterarAsync()
    {
        string query = $"""
                       UPDATE {tabela}
                       SET company_id = @company_id,
                           to_date = @to_date,
                           due_date = @due_date,
                           payment_form = @payment_form,
                           advertising = @advertising,
                           `key` = @key,
                           `type` = @type
                       WHERE id = @id;
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("id", id);
        comando.Parameters.AddWithValue("company_id", company_id);
        comando.Parameters.AddWithValue("to_date", to_date);
        comando.Parameters.AddWithValue("due_date", due_date);
        comando.Parameters.AddWithValue("payment_form", payment_form);
        comando.Parameters.AddWithValue("advertising", advertising);
        comando.Parameters.AddWithValue("key", key);
        comando.Parameters.AddWithValue("type", type);

        await conexao.OpenAsync();
        await comando.ExecuteNonQueryAsync();
    }

    public async Task ExcluirAsync(int idExclusao)
    {
        string query = $"""
                       DELETE FROM {tabela}
                       WHERE id = @id;
                       """;

        using var conexao = new MySqlConnection(ConfiguracaoBD.connectionString);
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("id", idExclusao);

        await conexao.OpenAsync();
        await comando.ExecuteNonQueryAsync();
    }
}
