using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battaglia_Navale
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //dichiarazione ed inizializzazione delle variabili:

            //matrici del campo di gioco
            int[,] player = new int[10, 10];
            int[,] playerHidden = new int[10, 10];
            int[,] ia = new int[10, 10];
            int[,] iaHidden = new int[10, 10];

            //variabili coordinate per la selezione della casella durante il gioco
            int riga = 0;
            int colonna = 0;

            //variabile turno - cambia a seconda di chi è il turno
            char turn = ' ';






            //fin
            Console.ReadKey();
        }
    }
}
