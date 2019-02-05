using Newtonsoft.Json;
using System.Diagnostics;
using System;

namespace FunctionDelegate
{
    public class Program
    {      
        static void Main(string[] args)
        {
            var valor1 = obterNumeroAleatorio.Invoke();
            var valor2 = obterNumeroAleatorio.Invoke();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            LogCalculo(ExecutarSoma, new Somar(valor1, valor2));
            LogCalculo(ExecutarSubtracao, new Subtrair(valor1, valor2));
            LogCalculo(ExecutarMultiplicacao, new Multiplicar(valor1, valor2));
            LogCalculo(ExecutarDivisao, new Dividir(valor1, valor2));

            stopWatch.Stop();
            escrever($"Tempo de execução do método { stopWatch.ElapsedMilliseconds } milisegundos.");

            Console.ReadKey();
        }

        static void LogCalculo<TObjeto, TRetorno>(Func<TObjeto, TRetorno> metodoSolicitado, TObjeto objetoSolicitado)
        {
            escrever($"Desc. -> { typeof(TObjeto).Name }, saída -> { JsonConvert.SerializeObject(metodoSolicitado(objetoSolicitado)) };");
        }

        static Action<string> escrever = str => Console.WriteLine(str);

        static Func<int> obterNumeroAleatorio = () =>
        {
            var rnd = new Random();
            return rnd.Next(0, 100);
        };

        #region --> Métodos

        static Somar ExecutarSoma(Somar somar)
        {
            somar.AtribuirResultado();
            return somar;
        }
        static Subtrair ExecutarSubtracao(Subtrair subtrair)
        {
            subtrair.AtribuirResultado();
            return subtrair;
        }
        static Multiplicar ExecutarMultiplicacao(Multiplicar multiplicar)
        {
            multiplicar.AtribuirResultado();
            return multiplicar;
        }
        static Dividir ExecutarDivisao(Dividir dividir)
        {
            dividir.AtribuirResultado();
            return dividir;
        }

        #endregion
    }

    #region --> Classes

    public abstract class FatoresBase
    {
        public double V1 { get; protected set; }
        public double V2 { get; protected set; }
        public double Resultado { get; protected set; }
        public virtual void AtribuirResultado() { }
    }
    public class Somar : FatoresBase
    {
        public Somar(int v1, int v2)
        {
            this.V1 = v1;
            this.V2 = v2;
        }
        public override void AtribuirResultado() => this.Resultado = this.V1 + this.V2;
    }
    public class Subtrair : FatoresBase
    {
        public Subtrair(int v1, int v2)
        {
            this.V1 = v1;
            this.V2 = v2;
        }
        public override void AtribuirResultado() => this.Resultado = this.V1 - this.V2;
    }
    public class Multiplicar : FatoresBase
    {
        public Multiplicar(int v1, int v2)
        {
            this.V1 = v1;
            this.V2 = v2;
        }
        public override void AtribuirResultado() => this.Resultado = this.V1 * this.V2;
    }
    public class Dividir : FatoresBase
    {
        public Dividir(int v1, int v2)
        {
            this.V1 = v1;
            this.V2 = v2;
        }
        public override void AtribuirResultado()
        {
            AtribuirValorSeDivisorEhZero((x) => x == 0);
            this.Resultado = this.V1 / this.V2;
        } 
        public void AtribuirValorSeDivisorEhZero(Predicate<double> predicate)
        {
            if (predicate(this.V2))
                this.V2 = Divisor.Minimo;
        }
    }
    public struct Divisor
    {
        public const int Minimo = 1;
        public const int Maximo = 10;
    }

    #endregion
}