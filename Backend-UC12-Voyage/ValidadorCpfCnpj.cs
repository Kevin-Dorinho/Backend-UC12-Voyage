using System.Text.RegularExpressions;

public static class ValidadorCpfCnpj
{
    public static bool ValidarCPF(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        // Remove tudo que não for dígito
        cpf = Regex.Replace(cpf, @"[^\d]", "");

        if (cpf.Length != 11)
            return false;

        // Elimina CPFs invalidos conhecidos (todos números iguais)
        if (new string(cpf[0], 11) == cpf)
            return false;

        int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf.Substring(0, 9);
        int soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores1[i];

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        tempCpf = tempCpf + digito1;
        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores2[i];

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        return cpf.EndsWith(digito1.ToString() + digito2.ToString());
    }

    public static bool ValidarCNPJ(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        // Remove tudo que não for dígito
        cnpj = Regex.Replace(cnpj, @"[^\d]", "");

        if (cnpj.Length != 14)
            return false;

        // Elimina CNPJs inválidos conhecidos
        if (new string(cnpj[0], 14) == cnpj)
            return false;

        int[] multiplicadores1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicadores2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCnpj = cnpj.Substring(0, 12);
        int soma = 0;

        for (int i = 0; i < 12; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicadores1[i];

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        tempCnpj = tempCnpj + digito1;
        soma = 0;

        for (int i = 0; i < 13; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicadores2[i];

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        return cnpj.EndsWith(digito1.ToString() + digito2.ToString());
    }

    public static bool ValidarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }

    public static bool ValidarCEP(string cep)
    {
        if (string.IsNullOrWhiteSpace(cep))
            return false;

        string cepLimpo = Regex.Replace(cep, @"[^\d]", "");
        return cepLimpo.Length == 8;
    }
}
