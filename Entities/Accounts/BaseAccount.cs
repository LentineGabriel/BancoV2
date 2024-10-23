using Banco.Entities.Exceptions;
using System;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Banco.Entities.Accounts
{
    internal class BaseAccount
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public char Gender { get; set; }
        public DateTime BirthDate { get; private set; } // não pode ser alterado.
        public ulong RG { get; private set; } // não pode ser alterado.

        public BaseAccount() { }

        public BaseAccount(string fullName, string email, char gender, DateTime birthDate, ulong rg)
        {
            FullName = fullName;
            Email = email;
            Gender = gender;
            BirthDate = birthDate;
            RG = rg;
        }

        // tratando o nome (tratamento concluído)
        public void ValidateName(string fullName)
        {
            // Caso o campo esteja vazio ou nulo;
            if (string.IsNullOrEmpty(fullName)) throw new BaseAccExceptions("O nome precisa estar preenchido.");

            // Caso o campo tenha um tamanho menor ou igual a 1 (por padrão, deve ter duas letras ou mais);
            if (fullName.Length <= 1) throw new BaseAccExceptions("O nome deve ter, no mínimo, duas letras para ser validado.");

            // Caso o nome seja preenchido com números ou caracteres especiais;
            if (!Regex.IsMatch(fullName, @"^[a-zA-Z\s]+$")) throw new BaseAccExceptions("Nome inválido! Apenas letras são permitidas.");
        }
        
        // tratando o email (tratamento concluído)
        public void ValidateEmail(string email)
        {
            // email não pode ser vazio ou conter espaços em branco
            if (string.IsNullOrWhiteSpace(email)) throw new BaseAccExceptions("O email não pode ser vazio ou conter espaços em branco.");

            // email precisa ter OBRIGATORIAMENTE o sufixo '@'
            if (!email.Contains("@")) throw new BaseAccExceptions("É necessário ter o @");

            // email precisa terminar OBRIGATORIAMENTE com '.com'
            if (!email.EndsWith(".com")) throw new BaseAccExceptions("O email precisa terminar com \".com\".");

            // uma lista com alguns domínios válidos, qualquer domínio fora desta lista lançará um erro
            List<string> domains = new List<string> { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com" };
            bool validDomain = false;

            foreach (string domain in domains)
            {
                if (email.EndsWith("@" + domain))
                {
                    validDomain = true;
                    break;
                }
            }
            if (validDomain) return;
            else throw new BaseAccExceptions("O email não possui um domínio válido.");
        }

        // tratando a data de nascimento (tratamento concluído)
        public int ValidateBirthDate (DateTime birthDate)
        {
            int age;
            DateTime today = DateTime.Today;

            // caso o ano de nascimento (inserido pelo usuário) seja maior que o ano atual (de acordo com a aplicação rodando)
            if (birthDate.Year > today.Year) throw new BaseAccExceptions("O ano de seu nascimento é maior que o ano atual.");
            else age = today.Year - birthDate.Year;
            return age;
        }

        // tratando o rg (tratamento concluído)
        public void ValidateRG(string rg)
        {
            // Vai verificar se o campo RG está vazio ou nulo; ou se há algum caractere diferente de números na hora de passar p/ ulong
            if (string.IsNullOrEmpty(rg) || !ulong.TryParse(rg, out _)) throw new BaseAccExceptions("RG inválido! Por favor, entre apenas com números.");
        }

        // tratando o gênero (tratamento concluído)
        public void ValidateGender(char gender)
        {
            // caso o usuário deixar o campo vazio
            if (char.IsWhiteSpace(gender)) throw new BaseAccExceptions("O gênero precisa estar preenchido.");

            // Se o gênero não for 'M' ou 'F'
            if (gender != 'M' && gender != 'F') throw new BaseAccExceptions("Gênero inválido! Apenas 'M' ou 'F' são permitidos.");
        }

        // display
        public void DisplayBase()
        {
            Console.Clear();
            Console.WriteLine($"Name: {FullName}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine(); // pular uma linha
            Console.WriteLine("Qual tipo de conta você gostaria de abrir?");
            Console.WriteLine("1 - Conta Comum");
            Console.WriteLine("2 - Conta Empresarial");
            int op = int.Parse(Console.ReadLine());

            switch (op)
            {
                case 1: // conta comum
                    CommomAccount ca = new CommomAccount();

                    Console.Clear();
                    Console.Write("CPF (apenas os números): "); string cpfInput = Console.ReadLine();
                    ca.ValidateRG(cpfInput);
                    ulong cpf = ulong.Parse(cpfInput); // o cpf entra como string e sai como ulong após a verificação.
                    Console.Write("Saldo inicial: "); double cBalance = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    CommomAccount CAccount = new CommomAccount(FullName, Email, Gender, BirthDate, RG, cpf, cBalance);

                    CAccount.DisplayCommom();
                    break;
                
                case 2: // conta empresarial
                    EnterpriseAccount ea = new EnterpriseAccount();

                    Console.Clear();
                    Console.Write("CNPJ (apenas os números): "); string cnpjInput = Console.ReadLine();
                    ea.ValidateCNPJ(cnpjInput);
                    ulong cnpj = ulong.Parse(cnpjInput); // o rg entra como string e sai como ulong após a verificação.
                    Console.Write("Saldo inicial: "); double eBalance = double.Parse(Console.ReadLine() , CultureInfo.InvariantCulture);
                    Console.Write("Crédito adicional: "); double credit = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    EnterpriseAccount EAccount = new EnterpriseAccount(FullName, Email, Gender, BirthDate, RG, cnpj, eBalance, credit);

                    EAccount.DisplayEnterprise();
                    break;
            }
        }
    }
}