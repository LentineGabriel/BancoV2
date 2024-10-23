using Banco.Entities.Exceptions;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Banco.Entities.Accounts
{
    internal class BaseAccount
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public char Gender { get; set; }
        public DateTime BirhtDate { get; private set; } // não pode ser alterado.
        public ulong RG { get; private set; } // não pode ser alterado.

        public BaseAccount() { }

        public BaseAccount(string fullName, string email, char gender, DateTime birhtDate, ulong rg)
        {
            FullName = fullName;
            Email = email;
            Gender = gender;
            BirhtDate = birhtDate;
            RG = rg;
        }

        // tratando o nome (tratamento concluído)
        public void ValidateName(string fullName)
        {
            // Caso o campo esteja vazio ou nulo;
            if (string.IsNullOrEmpty(fullName)) throw new NameExceptions("O nome precisa estar preenchido.");

            // Caso o campo tenha um tamanho menor ou igual a 1 (por padrão, deve ter duas letras ou mais);
            if (fullName.Length <= 1) throw new NameExceptions("O nome deve ter, no mínimo, duas letras para ser validado.");

            // Caso o nome seja preenchido com números ou caracteres especiais;
            if (!Regex.IsMatch(fullName, @"^[a-zA-Z\s]+$")) throw new NameExceptions("Nome inválido! Apenas letras são permitidas.");
        }
        
        // tratando o email (tratamento concluído)
        public void ValidateEmail(string email)
        {
            // email não pode ser vazio ou conter espaços em branco
            if (string.IsNullOrWhiteSpace(email)) throw new EmailExceptions("O email não pode ser vazio ou conter espaços em branco.");

            // email precisa ter OBRIGATORIAMENTE o sufixo '@'
            if (!email.Contains("@")) throw new EmailExceptions("É necessário ter o @");

            // email precisa terminar OBRIGATORIAMENTE com '.com'
            if (!email.EndsWith(".com")) throw new EmailExceptions("O email precisa terminar com \".com\".");

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
            else throw new EmailExceptions("O email não possui um domínio válido.");
        }

        // tratando o gênero (tratamento concluído)
        public void ValidateGender(char gender)
        {
            // caso o usuário deixar o campo vazio
            if (char.IsWhiteSpace(gender)) throw new GenderExceptions("O gênero precisa estar preenchido.");

            // Se o gênero não for 'M' ou 'F'
            if (gender != 'M' && gender != 'F') throw new GenderExceptions("Gênero inválido! Apenas 'M' ou 'F' são permitidos.");
        }

        // tratando a data de nascimento (tratamento concluído)
        public int ValidateBirthDate (DateTime birthDate)
        {
            int age;
            DateTime today = DateTime.Today;

            // caso o ano de nascimento (inserido pelo usuário) seja maior que o ano atual (de acordo com a aplicação rodando)
            if (birthDate.Year > today.Year) throw new BirthExceptions("O ano de seu nascimento é maior que o ano atual.");
            else age = today.Year - birthDate.Year;
            return age;
        }

        // tratando o rg (tratamento concluído)
        public void ValidateRG(string rg)
        {
            // Vai verificar se o campo RG está vazio ou nulo; ou se há algum caractere diferente de números na hora de passar p/ ulong
            if (string.IsNullOrEmpty(rg) || !ulong.TryParse(rg, out _)) throw new RGExcpetions("RG inválido! Por favor, entre apenas com números.");
        }

        // display
        public void Display()
        {
            Console.Clear();
            Console.WriteLine($"Name: {FullName}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine();
        }

    }
}