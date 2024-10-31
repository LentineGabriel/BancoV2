using Banco.Entities.Exceptions;
using System.Globalization;

namespace Banco.Entities.Accounts
{
    internal class EnterpriseAccount : BaseAccount
    {
        public ulong CNPJ { get; private set; } // não pode ser alterado;
        public double WithdrawLimit { get; private set; } // não pode ser alterado;
        public double Balance { get; set; }
        public double Credit { get; set; }
        
        public EnterpriseAccount() { }

        public EnterpriseAccount(string fullName, string email, char gender, DateTime birthDate, ulong rg, ulong cnpj, double balance, double credit)
            : base(fullName, email, gender, birthDate, rg)
        {
            CNPJ = cnpj;
            Balance = balance + credit;
            Credit = credit;
            WithdrawLimit = 100000.00;
        }

        // tratando o CNPJ (tratamento concluído)
        public void ValidateCNPJ(string cnpj)
        {
            // Vai verificar se o campo CPF está vazio ou nulo; ou se há algum caractere diferente de números na hora de passar p/ ulong
            if (string.IsNullOrEmpty(cnpj) || !ulong.TryParse(cnpj, out _)) throw new EnterpriseAccExceptions("CNPJ inválido! Por favor, entre apenas com números.");
        }
        
        // métodos de depósito e saque da poupança
        public double? Deposit(double balance, double amount)
        {
            if (amount > balance) throw new EnterpriseAccExceptions("A quantia inserida não pode ser maior que o seu saldo.");
            if (amount <= 0.0) throw new EnterpriseAccExceptions("A quantia inserida não pode ser menor ou igual a zero.");
            return balance - amount;
        }

        public double? Withdraw(double balance, double amount)
        {
            if (amount > balance) throw new EnterpriseAccExceptions("A quantia inserida não pode ser maior que o seu saldo.");
            if (amount <= 0.0) throw new EnterpriseAccExceptions("A quantia inserida não pode ser menor ou igual a zero.");
            return balance + amount;
        }

        // display
        public void DisplayEnterprise()
        {
            Console.Clear();
            Console.WriteLine($"Name: {FullName}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Saldo: R${Balance}");
            Console.WriteLine($"Crédito: R${Credit}");
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
                    Balance = Deposit(Balance, dAmount) ?? Balance;
                    Console.WriteLine($"Sua quantia foi depositada com sucesso! Seu saldo agora é: R${Balance}");
                    Console.WriteLine($"O saldo em sua conta poupança é de: R${dAmount}");
                    break;
                case 2:
                    Console.WriteLine("Entre com a quantia ao qual gostaria de sacar: ");
                    double wAmount = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    Balance = Withdraw(Balance, wAmount) ?? Balance;
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