using Banco.Entities.Accounts;
using System.Globalization;

namespace Banco
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BaseAccount ba = new BaseAccount();
            Console.WriteLine("Seja bem-vindo(a)! Por favor, preencha os campos abaixo para continuarmos.");
            try
            {
                // assim que o usuário digitar, os campos serão verificados;
                Console.Write("Nome completo: "); string name = Console.ReadLine();
                ba.ValidateName(name);

                Console.Write("Email: "); string email = Console.ReadLine();
                ba.ValidateEmail(email);

                Console.Write("Gênero: "); char gender = char.Parse(Console.ReadLine());
                ba.ValidateGender(gender);

                Console.Write("Data de nascimento: "); DateTime birthDate = DateTime.Parse(Console.ReadLine());
                int age = ba.ValidateBirthDate(birthDate);

                Console.Write("RG (apenas números): "); string rgInput = Console.ReadLine();
                ba.ValidateRG(rgInput);
                ulong rg = ulong.Parse(rgInput); // o rg entra como string e sai como ulong após a verificação.

                // quando tudo estiver validado
                BaseAccount Account = new BaseAccount(name, email, gender, birthDate, rg);

                Console.WriteLine("Wait...");
                Thread.Sleep(2500);
                
                // caso o usuário for menor de idade (verificação realizada no método BirthDate()), a única conta possível é a de adolescentes
                if (age < 18)
                {
                    Console.Clear();
                    TeenageAccount ta = new TeenageAccount();
                    Console.WriteLine("Por motivos de segurança, verificamos que você é menor de idade. Por isso, te ajudaremos a abrir uma conta para adolescentes.");
                    Console.WriteLine(); // pular uma linha
                    Console.Write("Nome completo da mãe: "); string motherName = Console.ReadLine();
                    ta.ValidateMotherName(motherName);

                    Console.Write("RG da mãe (apenas números): "); string motherRGInput = Console.ReadLine();
                    ta.ValidateMotherRG(motherRGInput);
                    ulong motherRG = ulong.Parse(motherRGInput);

                    Console.Write("Saldo inicial: "); double initialBalance = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

                    TeenageAccount teenageAccount = new TeenageAccount(name, email, gender, birthDate, rg, motherName, motherRG, initialBalance);

                    teenageAccount.DisplayTeenage();
                }
                else Account.DisplayBase();
            }
            catch (FormatException ex) // caso, onde se pede números, o usuário escreva letras, palavras ou frases.
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception ex) // outros erros (lembrando que os principais já estão sendo tratados em suas respectivas classes)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }
    }
}