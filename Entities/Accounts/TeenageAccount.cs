using Banco.Entities.Exceptions;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Banco.Entities.Accounts
{
    internal class TeenageAccount : BaseAccount
    {
        public string MotherName { get; set; }
        public ulong MotherRG { get; private set; } // não pode ser alterado;
        public double Balance { get; set; }

        public TeenageAccount() { }

        public TeenageAccount(string fullName, string email, char gender, DateTime birthDate, ulong rg, string motherName, ulong motherRG, double balance)
            : base(fullName, email, gender, birthDate, rg)
        {
            MotherName = motherName;
            MotherRG = motherRG;
            Balance = balance;
        }

        // trantando o nome da mãe (tratamento concluído)
        public void ValidateMotherName(string motherName)
        {
            // Caso o campo esteja vazio ou nulo;
            if (string.IsNullOrEmpty(motherName)) throw new TeenageAccExceptions("O nome precisa estar preenchido.");

            // Caso o campo tenha um tamanho menor ou igual a 1 (por padrão, deve ter duas letras ou mais);
            if (motherName.Length <= 1) throw new TeenageAccExceptions("O nome deve ter, no mínimo, duas letras para ser validado.");

            // Caso o nome seja preenchido com números ou caracteres especiais;
            if (!Regex.IsMatch(motherName, @"^[a-zA-Z\s]+$")) throw new TeenageAccExceptions("Nome inválido! Apenas letras são permitidas.");
        }

        // tratando o rg da mãe (tratamento concluído)
        public void ValidateMotherRG(string motherRG)
        {
            // Vai verificar se o campo RG está vazio ou nulo; ou se há algum caractere diferente de números na hora de passar p/ ulong
            if (string.IsNullOrEmpty(motherRG) || !ulong.TryParse(motherRG, out _)) throw new BaseAccExceptions("RG inválido! Por favor, entre apenas com números.");
        }

        // métodos de depósito e saque da poupança
        public void DepositTeen(double amount)
        {
            if (amount > Balance) throw new CommomAccExceptions("A quantia inserida não pode ser maior que o seu saldo.");
            if (amount <= 0.0) throw new CommomAccExceptions("A quantia inserida não pode ser menor ou igual a zero.");
            Balance -= amount;
        }

        public void WithdrawTeen(double amount)
        {
            if (amount > Balance) throw new CommomAccExceptions("A quantia inserida não pode ser maior que o seu saldo.");
            if (amount <= 0.0) throw new CommomAccExceptions("A quantia inserida não pode ser menor ou igual a zero.");
            Balance += amount;
        }

        // display
        public void DisplayTeenage()
        {
            Console.Clear();
            Console.WriteLine($"Name: {FullName}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Saldo: R${Balance}");
            Console.WriteLine(); // pular uma linha
            Console.WriteLine("O que você gostaria de fazer?");
            Console.WriteLine("1 - Depositar na poupança");
            Console.WriteLine("2 - Sacar da poupança");
            Console.WriteLine("3 - Sair");
            int op = int.Parse(Console.ReadLine());

            switch (op)
            {
                case 1:
                    Console.WriteLine(); // pular uma linha.
                    Console.Write("Entre com a quantia ao qual gostaria de depositar: ");
                    double dAmount = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    DepositTeen(dAmount);
                    Console.WriteLine($"Sua quantia foi depositada com sucesso! Seu saldo agora é: R${Balance}");
                    Console.WriteLine($"O saldo em sua conta poupança é de: R${dAmount}");
                    break;
                case 2:
                    Console.WriteLine("Entre com a quantia ao qual gostaria de sacar: ");
                    double wAmount = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    WithdrawTeen(wAmount);
                    Console.WriteLine($"Sua quantia foi depositada com sucesso! Seu saldo agora é: R${Balance}");
                    Console.WriteLine($"O saldo em sua conta poupança é de: R${wAmount}");
                    break;
                case 3:
                    System.Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Número inválido!");
                    break;
            }
        }
    }
}