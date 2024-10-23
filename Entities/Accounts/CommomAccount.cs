using Banco.Entities.Exceptions;
using System.Globalization;

namespace Banco.Entities.Accounts
{
    internal class CommomAccount : BaseAccount
    {
        public ulong CPF { get; private set; } // não pode ser alterado;
        public double Balance { get; set; }

        public CommomAccount() { }
        public CommomAccount(string fullName, string email, char gender, DateTime birthDate, ulong rg, ulong cpf, double balance)
            : base (fullName, email, gender, birthDate, rg)
        {
            CPF = cpf;
            Balance = balance;
        }
        
        // tratando o CPF (tratamento concluído)
        public void ValidateCPF(string cpf)
        {
            // Vai verificar se o campo CPF está vazio ou nulo; ou se há algum caractere diferente de números na hora de passar p/ ulong
            if (string.IsNullOrEmpty(cpf) || !ulong.TryParse(cpf, out _)) throw new CommomAccExceptions("CPF inválido! Por favor, entre apenas com números.");
        }

        // métodos de depósito e saque da poupança
        public void Deposit(double amount)
        {
            if (amount > Balance) throw new CommomAccExceptions("A quantia inserida não pode ser maior que o seu saldo.");
            if (amount <= 0.0) throw new CommomAccExceptions("A quantia inserida não pode ser menor ou igual a zero.");
            Balance -= amount;
        }

        public void Withdraw(double amount)
        {
            if (amount > Balance) throw new CommomAccExceptions("A quantia inserida não pode ser maior que o seu saldo.");
            if (amount <= 0.0) throw new CommomAccExceptions("A quantia inserida não pode ser menor ou igual a zero.");
            Balance += amount;
        }

        // display
        public void DisplayCommom()
        {
            Console.Clear();
            Console.WriteLine($"Name: {FullName}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Saldo: R${Balance}");
            Console.WriteLine();// pular uma linha.
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
                    Deposit(dAmount);
                    Console.WriteLine($"Sua quantia foi depositada com sucesso! Seu saldo agora é: R${Balance}");
                    Console.WriteLine($"O saldo em sua conta poupança é de: R${dAmount}");
                    break;
                case 2:
                    Console.WriteLine("Entre com a quantia ao qual gostaria de sacar: ");
                    double wAmount = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    Withdraw(wAmount);
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